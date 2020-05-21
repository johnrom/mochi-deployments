namespace Nmbl.Vercel.OcModule.Models
{
    public class VercelDeploymentOptions
    {
        /// <summary>
        /// Default Cache in Seconds is slightly higher, to avoid Rate Limiting.
        /// Immediately after triggering a deployment, see <see cref="HighPriorityCacheInSeconds" />.
        /// </summary>
        public int CacheInSeconds { get; set; } = 30;
        /// <summary>
        /// High Priority Cache kicks in when a deployment was recently triggered.
        /// See <see cref="VercelServiceState.IsWaitingForDeployment" />.
        /// </summary>
        /// <value></value>
        public int HighPriorityCacheInSeconds { get; set; } = 5;
        /// <summary>
        /// Trigger a Deployment on IContentManager.PublishedAsync.
        /// </summary>
        public bool DeployOnPublish { get; set; } = true;
        /// <summary>
        /// CustomSettings don't trigger the normal PublishedAsync action in OrchardCore.
        /// For now, the best I could find is UpdatedAsync on a content item without an ID,
        /// but that can be triggered multiple times in a single Publish.
        /// </summary>
        public bool Experimental_DeployOnCustomSettingsUpdated { get; set; } = false;
    }
}
