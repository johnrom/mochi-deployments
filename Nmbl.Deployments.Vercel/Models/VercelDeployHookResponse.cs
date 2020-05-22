using System;
using System.Collections.Generic;
using System.Text;

namespace Nmbl.Deployments.Vercel.Models
{
    public class VercelDeployHookResponse
    {
        public VercelDeployHookJob Job { get; set; }

        public VercelDeployHookResponse(VercelDeployHookResponseApiModel apiModel)
        {
            Job = new VercelDeployHookJob(apiModel.Job);
        }
    }
}
