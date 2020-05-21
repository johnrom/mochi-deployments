using System;
using System.Collections.Generic;
using System.Text;

namespace Nmbl.Vercel.Models
{
    public enum VercelDeploymentState
    {
      Initializing,
      Queued,
      Pending,
      Analyzing,
      Building,
      Deploying,
      Ready,
      Error,
      Canceled,
      Unknown,
    }
}
