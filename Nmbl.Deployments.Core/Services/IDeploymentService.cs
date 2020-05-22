using System.Collections.Generic;
using System.Threading.Tasks;
using Nmbl.Deployments.Core.Models;

namespace Nmbl.Deployments.Core.Services
{
    public interface IDeploymentService
    {
        InitializationState GetInitializationStatus();
        Task<IEnumerable<Deployment>> GetDeploymentsAsync(Dictionary<string, string> queryParams = null);
        Task<Deployment> GetLatestProductionDeploymentAsync();
        Task<Deployment> GetDeploymentAsync(string id);
        Task<DeploymentResponse> DeployAsync();
    }
}
