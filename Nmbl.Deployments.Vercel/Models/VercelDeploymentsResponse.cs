using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Nmbl.Deployments.Vercel.Models
{
    public class VercelDeploymentsResponse
    {
        public IEnumerable<VercelDeployment> Deployments { get; set; }

        public VercelDeploymentsResponse(VercelDeploymentsResponseApiModel apiModel)
        {
            Deployments = apiModel.Deployments.Select(deploymentApiModel => new VercelDeployment(deploymentApiModel));
        }
    }
}
