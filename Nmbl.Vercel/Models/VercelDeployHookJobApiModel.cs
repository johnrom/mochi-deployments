using System;
using System.Collections.Generic;
using System.Text;

namespace Nmbl.Vercel.Models
{
    public class VercelDeployHookJobApiModel
    {
        public string Id { get; set; }
        public VercelDeploymentState State { get; set; }
        public long CreatedAt { get; set; }
    }
}
