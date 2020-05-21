using System;
using System.Collections.Generic;
using System.Text;

namespace Nmbl.Vercel.OcModule.Models
{
    public class VercelServiceState
    {
        public string PreviousDeploymentUid { get; set; }
        public bool IsWaitingForDeployment { get; set; }
    }
}
