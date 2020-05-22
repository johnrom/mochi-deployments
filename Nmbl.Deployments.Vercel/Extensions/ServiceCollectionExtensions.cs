using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Nmbl.Deployments.Core.Models;
using Nmbl.Deployments.Core.Extensions;
using System;
using Nmbl.Deployments.Vercel.Models;
using Nmbl.Deployments.Core.Services;
using Nmbl.Deployments.Core;

namespace Nmbl.Deployments.Vercel.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static void AddVercel(this IServiceCollection services, Action<DeploymentOptions> deploymentSetup = null, Action<VercelOptions> vercelSetup = null)
        {
            if (vercelSetup != null)
            {
                services.Configure(vercelSetup);
            }
            services.AddDeployments(deploymentSetup);

            services.AddHttpClient<IDeploymentService>()
                .ConfigureHttpClient((services, httpClient) =>
                {
                    var vercelOptions = services.GetService<IOptions<VercelOptions>>()?.Value ?? new VercelOptions();

                    httpClient.BaseAddress = new Uri(vercelOptions.BaseAddress);

                    if (!string.IsNullOrEmpty(vercelOptions.ApiToken))
                    {
                        httpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {vercelOptions.ApiToken}");
                    }
                })
                .AddPolicyHandler((sp, message) =>
                {
                    var policies = sp.GetRequiredService<DeploymentPolicies>();

                    return policies.HttpRetryPolicy;
                })
                .AddPolicyHandler((sp, message) =>
                {
                    var policies = sp.GetRequiredService<DeploymentPolicies>();

                    return policies.HttpCircuitBreakerPolicy;
                });
        }
    }
}
