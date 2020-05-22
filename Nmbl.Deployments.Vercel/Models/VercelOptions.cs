using System;
using System.Collections.Generic;
using System.Text;

namespace Nmbl.Deployments.Vercel.Models
{
    public class VercelOptions
    {
        public string BaseAddress { get; set; } = "https://api.vercel.com";
        public string TeamId { get; set; }
        public string DeployHook { get; set; }
        public string ApiToken { get; set; }
    }
}
