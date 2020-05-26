using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Nmbl.Deployments.Core.Models;
using Nmbl.Deployments.Core.Extensions;
using System;
using Nmbl.Deployments.Vercel.Models;
using Nmbl.Deployments.Core.Services;
using Nmbl.Deployments.Core;
using Nmbl.Deployments.Vercel.Services;

namespace Nmbl.Deployments.Vercel.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static void AddVercelCore(this IServiceCollection services, Action<VercelOptions> vercelSetup = null, Action<DeploymentOptions> deploymentSetup = null)
        {
            if (vercelSetup != null)
            {
                services.Configure(vercelSetup);
            }
            services.AddDeployments(deploymentSetup);
        }
        private static void ConfigureVercelHttpClient(this IHttpClientBuilder clientBuilder)
        {
            clientBuilder
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

        /// <summary>
        /// Add Vercel Core with default `IDeploymentService`
        /// </summary>
        public static void AddVercel(
            this IServiceCollection services,
            Action<VercelOptions> vercelSetup = null,
            Action<DeploymentOptions> deploymentSetup = null
        ) {
            services.AddVercelCore(vercelSetup, deploymentSetup);
            services
                .AddHttpClient<IDeploymentService, VercelDeploymentService>()
                .ConfigureVercelHttpClient();
        }
        /// <summary>
        /// Add Vercel without default `IDeploymentService` implementation,
        /// instead registering `VercelDeploymentService` itself.
        /// </summary>
        public static void AddVercelWithoutDefaultService(this IServiceCollection services, Action<VercelOptions> vercelSetup = null, Action<DeploymentOptions> deploymentSetup = null)
        {
            services.AddVercelCore(vercelSetup, deploymentSetup);

            services
                .AddHttpClient<VercelDeploymentService>()
                .ConfigureVercelHttpClient();
        }
    }
}
