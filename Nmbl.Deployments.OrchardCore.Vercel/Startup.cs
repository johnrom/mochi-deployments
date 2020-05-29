using System;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Nmbl.Deployments.Vercel.Extensions;
using OrchardCore.Modules;
using Nmbl.Deployments.Core.Models;
using Nmbl.Deployments.Core.Services;
using Nmbl.Deployments.OrchardCore.Vercel.Services;
using Nmbl.Deployments.OrchardCore.Extensions;
using Nmbl.Deployments.OrchardCore.Models;
using Nmbl.Deployments.Vercel.Models;
using Microsoft.AspNetCore.Mvc;
using Nmbl.Deployments.OrchardCore.Vercel.TagHelpers;
using Nmbl.Deployments.OrchardCore.Vercel.Filters;
using OrchardCore.Security.Permissions;
using OrchardCore.Navigation;

namespace Nmbl.Deployments.OrchardCore.Vercel
{
    public class Startup : StartupBase
    {
        private readonly IConfiguration _configuration;

        public Startup(
            IConfiguration configuration
        ) {
            _configuration = configuration;
        }

        public override void ConfigureServices(IServiceCollection services)
        {
            services.AddVercelWithoutDefaultService(
                options => _configuration.GetSection(nameof(VercelOptions)).Bind(options),
                options => _configuration.GetSection(nameof(DeploymentOptions)).Bind(options)
            );
            services.AddOrchardDeployments(options => _configuration.GetSection(nameof(OrchardDeploymentOptions)).Bind(options));
            services.AddScoped<IDeploymentService, VercelOrchardCoreDeploymentService>();
            services.AddScoped<INavigationProvider, AdminMenu>();
            services.AddTagHelpers<VercelLatestDeploymentTagHelper>();
            services.Configure<MvcOptions>((options) =>
            {
                options.Filters.Add(typeof(VercelDeploymentNavBarFilter));
            });
        }

        public override void Configure(IApplicationBuilder builder, IEndpointRouteBuilder routes, IServiceProvider serviceProvider)
        {
        }
    }
}
