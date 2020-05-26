using System;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Nmbl.Deployments.Vercel.Extensions;
using OrchardCore.Modules;
using OrchardCore.Navigation;
using OrchardCore.Security.Permissions;
using Nmbl.Deployments.OrchardCore.Vercel.Filters;
using Nmbl.Deployments.OrchardCore.Vercel.TagHelpers;
using Nmbl.Deployments.Core.Models;

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
            services.AddVercel(options => _configuration.GetSection(nameof(DeploymentOptions)).Bind(options));
            services.AddScoped<IPermissionProvider, Permissions>();
            services.AddScoped<INavigationProvider, AdminMenu>();
            services.AddTagHelpers<VercelLatestDeploymentTagHelper>();
            services.Configure<MvcOptions>((options) =>
            {
                options.Filters.Add(typeof(VercelNavBarFilter));
            });
        }

        public override void Configure(IApplicationBuilder builder, IEndpointRouteBuilder routes, IServiceProvider serviceProvider)
        {
        }
    }
}
