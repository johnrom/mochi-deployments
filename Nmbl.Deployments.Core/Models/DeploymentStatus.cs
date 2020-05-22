using System;
using System.Collections.Generic;
using System.Text;

namespace Nmbl.Deployments.Core.Models
{
    public class DeploymentStatus
    {
        public string PreviousDeploymentUid { get; set; }
        public bool IsWaitingForDeployment { get; set; }
    }
}
