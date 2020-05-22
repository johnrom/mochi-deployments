using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Nmbl.Deployments.Core;
using Nmbl.Deployments.Core.Models;
using Nmbl.Deployments.Core.Services;
using Nmbl.Deployments.Vercel.Extensions;
using Nmbl.Deployments.Vercel.Models;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace Nmbl.Deployments.Vercel.Services
{
    public class VercelDeploymentService : IDeploymentService
    {
        private readonly HttpClient _httpClient;
        private readonly VercelOptions _vercelOptions;
        private readonly ILogger<VercelDeploymentService> _logger;

        public VercelDeploymentService(
            HttpClient httpClient,
            IOptions<VercelOptions> vercelOptions,
            ILogger<VercelDeploymentService> logger
        ) {
            _httpClient = httpClient;
            _vercelOptions = vercelOptions.Value;
            _logger = logger;
        }

        private async Task<TValue> DeserializeAsync<TValue>(HttpResponseMessage response)
        {
            using var stream = await response.Content.ReadAsStreamAsync();
            using var streamReader = new StreamReader(stream);
            using var jsonReader = new JsonTextReader(streamReader);

            var serializer = new JsonSerializer();

            return serializer.Deserialize<TValue>(jsonReader);
        }

        private string BuildUrl(string path, Func<Uri, string> callback = null)
        {
            var uri = new Uri(_httpClient.BaseAddress, path);

            return callback != null ?
                callback?.Invoke(uri)
                : uri.ToString();
        }

        private string BuildUrl<T>(
            string path,
            T values,
            Func<IDictionary<string, string>, IDictionary<string, string>> callback = null
        )
            where T : new()
        {
            return BuildUrl(path, uri =>
            {
                IDictionary<string, string> dictValues = ImmutableDictionary<string, string>.Empty;

                if (values != null)
                {
                    var jsonValues = JsonConvert.SerializeObject(values);

                    dictValues = JsonConvert.DeserializeObject<IDictionary<string, string>>(jsonValues);
                }

                if (callback != null) {
                    // allow caller to add to this dictionary
                    dictValues = callback(dictValues == ImmutableDictionary<string, string>.Empty
                        ? new Dictionary<string, string>()
                        : dictValues
                    );
                }

                return dictValues == ImmutableDictionary<string, string>.Empty
                    ? uri.ToString()
                    : QueryHelpers.AddQueryString(path, dictValues);
            });
        }

        private IDictionary<string, string> MaybeAddTeamId(IDictionary<string, string> dictValues)
        {
            if (!string.IsNullOrEmpty(_vercelOptions.TeamId))
            {
                dictValues.Add("teamId", _vercelOptions.TeamId);
            }

            return dictValues;
        }

        private string GetEndpoint(string endpoint, IDictionary<string, string> queryParams = null)
        {
            return queryParams != null
                ? BuildUrl<object>(endpoint, null, (dictValues => {
                    foreach (var queryParam in queryParams) {
                        dictValues.Add(queryParam.Key, queryParam.Value);
                    }

                    return MaybeAddTeamId(dictValues);
                }))
                : BuildUrl<object>(endpoint, null, MaybeAddTeamId);
        }

        private async Task<TValue> GetAsync<TValue>(string endpoint)
        {
            var response = await _httpClient.GetAsync(endpoint);

            response.EnsureSuccessStatusCode();

            var responseText = await response.Content.ReadAsStringAsync();

            return JsonConvert.DeserializeObject<TValue>(responseText);
        }

        public InitializationState GetInitializationStatus()
        {
            return new InitializationState
            {
                IsReadConfigured = !string.IsNullOrEmpty(_vercelOptions.ApiToken),
                IsDeploymentConfigured =
                    !string.IsNullOrEmpty(_vercelOptions.ApiToken) &&
                    !string.IsNullOrEmpty(_vercelOptions.DeployHook)
            };
        }

        public async Task<IEnumerable<Deployment>> GetDeploymentsAsync(Dictionary<string, string> queryParams = null)
        {
            var endpoint = GetEndpoint("/v6/now/deployments", queryParams);
            var response = await GetAsync<VercelDeploymentsResponseApiModel>(endpoint);

            return response != null
                ? new VercelDeploymentsResponse(response)?.Deployments?.Select(deployment => deployment.ToDeployment())
                : null;
        }

        public async Task<Deployment> GetDeploymentAsync(string id)
        {
            var endpoint = GetEndpoint($"/v11/now/deployment/{id}");
            var deployment = await GetAsync<VercelDeploymentApiModel>(endpoint);

            return new VercelDeployment(deployment)?.ToDeployment();
        }

        public async Task<Deployment> GetLatestProductionDeploymentAsync()
        {
            var deployments = await GetDeploymentsAsync(new Dictionary<string, string>
            {
                { "meta-githubCommitRef", "master" }
            });

            return deployments.FirstOrDefault();
        }

        public async Task<DeploymentResponse> DeployAsync()
        {
            var response = await _httpClient.PostAsync($"/v1/integrations/deploy/{_vercelOptions.DeployHook}", null);
            var result = await DeserializeAsync<VercelDeployHookResponseApiModel>(response);

            return new DeploymentResponse {
                Success = result != null,
                Result = new VercelDeployHookResponse(result)
            };
        }
    }
}
