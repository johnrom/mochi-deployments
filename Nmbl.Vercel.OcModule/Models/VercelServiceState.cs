using System;
using System.Collections.Generic;
using System.Text;

namespace Nmbl.OcModules.Vercel.Models
{
    public class VercelServiceState
    {
        public string PreviousDeploymentUid { get; set; }
        public bool IsWaitingForDeployment { get; set; }
    }
}
