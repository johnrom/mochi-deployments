using Microsoft.Extensions.Options;
using Nmbl.Deployments.Core.Models;
using Polly;
using Polly.Extensions.Http;
using System;
using System.Net.Http;

namespace Nmbl.Deployments.Core
{
    public class DeploymentPolicies
    {
        public IAsyncPolicy<HttpResponseMessage> HttpRetryPolicy { get; }
        public IAsyncPolicy<HttpResponseMessage> HttpCircuitBreakerPolicy { get; }

        public DeploymentPolicies(IOptions<DeploymentOptions> options)
        {
            if (options.Value.HttpPolicyRetryCount > 0) {
                HttpRetryPolicy = HttpPolicyExtensions
                    .HandleTransientHttpError()
                    .WaitAndRetryAsync(
                        options.Value.HttpPolicyRetryCount,
                        retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt))
                    );
            }

            if (options.Value.CircuitBreakerPolicyCount > 0)
            {
                HttpCircuitBreakerPolicy = Policy
                    .Handle<HttpRequestException>()
                    .CircuitBreakerAsync(
                        exceptionsAllowedBeforeBreaking: options.Value.CircuitBreakerPolicyCount,
                        durationOfBreak: TimeSpan.FromSeconds(options.Value.CircuitBreakerPolicyIntervalInSeconds)
                    ).AsAsyncPolicy<HttpResponseMessage>();
            }
        }
    }
}
