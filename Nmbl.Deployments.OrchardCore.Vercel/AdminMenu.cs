using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Localization;
using OrchardCore.Navigation;

namespace Nmbl.Deployments.OrchardCore.Vercel
{
    public class AdminMenu : INavigationProvider
    {
        public AdminMenu(IStringLocalizer<AdminMenu> localizer)
        {
            T = localizer;
        }

        public IStringLocalizer T { get; set; }

        public Task BuildNavigationAsync(string name, NavigationBuilder builder)
        {
            if (!String.Equals(name, "admin", StringComparison.OrdinalIgnoreCase))
            {
                return Task.CompletedTask;
            }

            builder
                .Add(T["Content"], configuration => configuration
                    .Add(T["Deployments"], "ZZZZ-100", tasks => tasks
                        .Add(T["Vercel"], "10", navItemBuilder => navItemBuilder
                            .Action("Index", "VercelDeployments", new { area = "Nmbl.Deployments.OrchardCore.Vercel" })
                            .Permission(Permissions.ManageDeploymentSettings)
                            .LocalNav()
                        )
                    )
                );

            return Task.CompletedTask;
        }
    }
}
