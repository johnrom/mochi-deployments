using Microsoft.Extensions.DependencyInjection;
using Nmbl.Deployments.Core.Models;
using System;

namespace Nmbl.Deployments.Core.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static void AddDeployments(this IServiceCollection services, Action<DeploymentOptions> setup = null)
        {
            if (setup != null)
            {
                services.Configure(setup);
            }
            services.AddSingleton<DeploymentPolicies>();
        }
    }
}
