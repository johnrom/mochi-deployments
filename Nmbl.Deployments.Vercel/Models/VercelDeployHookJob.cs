using System;
using System.Collections.Generic;
using System.Text;
using Nmbl.Deployments.Vercel.Extensions;

namespace Nmbl.Deployments.Vercel.Models
{
    public class VercelDeployHookJob
    {
        public string Id { get; set; }
        public VercelDeploymentState State { get; set; }
        public DateTime CreatedAt { get; set; }

        public VercelDeployHookJob(VercelDeployHookJobApiModel apiModel)
        {
            Id = apiModel.Id;
            State = apiModel.State;
            CreatedAt = apiModel.CreatedAt.ToDateTimeUtc();
        }
    }
}
