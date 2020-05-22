using Nmbl.Deployments.Core.Models;
using Nmbl.Deployments.Vercel.Models;

namespace Nmbl.Deployments.Vercel.Extensions
{
    public static class DeploymentExtensions
    {
        public static Deployment ToDeployment(this VercelDeployment deployment) {
            return new Deployment {
                Id = deployment?.Uid,
                Source = deployment,
            };
        }
    }
}