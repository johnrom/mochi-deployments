using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Nmbl.Deployments.Core.Models;
using Nmbl.Deployments.Core.Services;
using System;

namespace Nmbl.Deployments.Core.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static void AddDeployments(
            this IServiceCollection services,
            Action<DeploymentOptions> setup = null
        ) {
            if (setup != null)
            {
                services.Configure(setup);
            }
            services.TryAddSingleton<DeploymentPolicies>();
            services.TryAddScoped<DeploymentStatusService>();
        }
    }
}
