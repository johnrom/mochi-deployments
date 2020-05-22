namespace Nmbl.Deployments.OrchardCore.Models
{
    public class OrchardDeploymentOptions
    {
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
