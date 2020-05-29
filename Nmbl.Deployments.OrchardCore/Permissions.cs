using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using OrchardCore.Security.Permissions;

namespace Nmbl.Deployments.OrchardCore
{
    public class Permissions : IPermissionProvider
    {
        public static readonly Permission ManageDeploymentSettings = new Permission("ManageDeploymentSettings", "Manage Deployment Settings");

        public Task<IEnumerable<Permission>> GetPermissionsAsync()
        {
            return Task.FromResult(new[]
            {
                ManageDeploymentSettings
            }
            .AsEnumerable());
        }

        public IEnumerable<PermissionStereotype> GetDefaultStereotypes()
        {
            return new[]
            {
                new PermissionStereotype
                {
                    Name = "Administrator",
                    Permissions = new[] { ManageDeploymentSettings }
                },
            };
        }
    }
}
