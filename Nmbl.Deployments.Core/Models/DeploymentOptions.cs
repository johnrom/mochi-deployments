using System;
using System.Collections.Generic;
using System.Text;

namespace Nmbl.Deployments.Core.Models
{
    public class DeploymentOptions
    {
        /// <summary>
        /// Default Cache in Seconds is slightly higher, to avoid Rate Limiting.
        /// Immediately after triggering a deployment, see <see cref="HighPriorityCacheInSeconds" />.
        /// </summary>
        public int CacheInSeconds { get; set; } = 30;
        /// <summary>
        /// High Priority Cache kicks in when a deployment was recently triggered.
        /// See <see cref="DeploymentState.IsWaitingForDeployment" />.
        /// </summary>
        /// <value></value>
        public int HighPriorityCacheInSeconds { get; set; } = 5;
        public int HttpPolicyRetryCount { get; set; } = 3;
        public int CircuitBreakerPolicyCount { get; set; } = 3;
        public int CircuitBreakerPolicyIntervalInSeconds { get; set; } = 60;
    }
}
