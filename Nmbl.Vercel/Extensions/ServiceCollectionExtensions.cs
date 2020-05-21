using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Nmbl.Vercel.Models;
using Nmbl.Vercel.Services;
using System;

namespace Nmbl.Vercel.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static void AddVercel(this IServiceCollection services, Action<VercelOptions> setup)
        {
            services.Configure(setup);
            services.AddSingleton<VercelPolicies>();

            services.AddHttpClient<IVercelService, VercelService>()
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
                    var policies = sp.GetRequiredService<VercelPolicies>();

                    return policies.HttpRetryPolicy;
                })
                .AddPolicyHandler((sp, message) =>
                {
                    var policies = sp.GetRequiredService<VercelPolicies>();

                    return policies.HttpCircuitBreakerPolicy;
                });
        }
    }
}
