using System;
using Nmbl.OcModules.Vercel.ContentHandlers;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Nmbl.OcModules.Vercel.Filters;
using Nmbl.OcModules.Vercel.Models;
using Nmbl.OcModules.Vercel.Services;
using Nmbl.OcModules.Vercel.TagHelpers;
using Nmbl.Vercel.Extensions;
using Nmbl.Vercel.Models;
using OrchardCore.ContentManagement.Handlers;
using OrchardCore.Modules;
using OrchardCore.Navigation;
using OrchardCore.Security.Permissions;

namespace Nmbl.OcModules.Vercel
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
            services.AddLazyCache();
            services.AddVercel(options => _configuration.GetSection(nameof(VercelOptions)).Bind(options));
            services.Configure<VercelDeploymentOptions>(options =>
                _configuration.GetSection(nameof(VercelDeploymentOptions)).Bind(options)
            );

            services.AddSingleton<VercelServiceState>();
            services.AddScoped<VercelDeploymentStatusService>();

            services.AddScoped<IContentHandler, DeployingContentHandler>();
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
