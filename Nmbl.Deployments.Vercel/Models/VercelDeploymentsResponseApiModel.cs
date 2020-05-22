using System;
using System.Collections.Generic;
using System.Text;

namespace Nmbl.Vercel.Models
{
    public class VercelDeploymentsResponseApiModel
    {
        public IEnumerable<VercelDeploymentApiModel> Deployments { get; set; }
    }
}
