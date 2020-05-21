using Nmbl.Vercel.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Nmbl.Vercel.Services
{
    public interface IVercelService
    {
        VercelInitializationState GetInitializationStatus();
        Task<VercelDeploymentsResponse> GetDeploymentsAsync(Dictionary<string, string> queryParams = null);
        Task<VercelDeployment> GetLatestProductionDeploymentAsync();
        Task<VercelDeployment> GetDeploymentAsync(string id);
        Task<VercelDeployHookResponse> DeployAsync();
    }
}
