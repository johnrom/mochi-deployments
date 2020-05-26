using System;
using Microsoft.Extensions.DependencyInjection;
using Nmbl.Deployments.Core.Extensions;
using Nmbl.Deployments.Core.Models;
using Nmbl.Deployments.OrchardCore.Models;

namespace Nmbl.Deployments.OrchardCore.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static void AddOrchardDeployments(this IServiceCollection services, Action<DeploymentOptions> deploymentSetup = null, Action<OrchardDeploymentOptions> orchardSetup = null)
        {
            if (orchardSetup != null)
            {
                services.Configure(orchardSetup);
            }
            services.AddDeployments(deploymentSetup);

            services.AddLazyCache();
            services.AddSingleton<DeploymentStatus>();
        }
    }
}
