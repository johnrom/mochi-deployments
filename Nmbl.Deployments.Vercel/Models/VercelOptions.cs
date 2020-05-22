using System;
using System.Collections.Generic;
using System.Text;

namespace Nmbl.Vercel.Models
{
    public class VercelOptions
    {
        public string BaseAddress { get; set; } = "https://api.vercel.com";
        public string TeamId { get; set; }
        public string DeployHook { get; set; }
        public string ApiToken { get; set; }
        public int HttpPolicyRetryCount { get; set; } = 3;
        public int CircuitBreakerPolicyCount { get; set; } = 3;
        public int CircuitBreakerPolicyIntervalInSeconds { get; set; } = 60;
    }
}
