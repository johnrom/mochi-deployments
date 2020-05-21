using Nmbl.Vercel.Models;

namespace Nmbl.OcModules.Vercel.ViewModels
{
    public class VercelLatestDeploymentViewModel
    {
        public VercelInitializationState InitializationStatus { get; set; }
        public bool IsWaitingForDeployment { get; set; }
        public VercelDeployment LatestDeployment { get; set; }
    }
}
