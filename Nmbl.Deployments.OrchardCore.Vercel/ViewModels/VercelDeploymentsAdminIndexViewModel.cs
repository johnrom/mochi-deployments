using System.Collections.Generic;
using Nmbl.Deployments.Core.Models;

namespace Nmbl.Deployments.OrchardCore.Vercel.ViewModels
{
    public class VercelDeploymentsAdminIndexViewModel
    {
        public InitializationState InitializationStatus { get; set; }
        public IEnumerable<Deployment> Deployments { get; set; }
    }
}
