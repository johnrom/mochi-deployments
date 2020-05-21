using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OrchardCore.Security.Permissions;

namespace Nmbl.OcModules.Vercel
{
    public class Permissions : IPermissionProvider
    {
        public static readonly Permission ManageVercelSettings = new Permission("ManageVercelSettings", "Manage Vercel Settings");

        public Task<IEnumerable<Permission>> GetPermissionsAsync()
        {
            return Task.FromResult(new[]
            {
                ManageVercelSettings
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
                    Permissions = new[] { ManageVercelSettings }
                },
            };
        }
    }
}
