namespace Nmbl.Deployments.Core.Models
{
    public class DeploymentStatus
    {
        public string PreviousDeploymentId { get; set; }
        public bool IsWaitingForDeployment { get; set; }
    }
}
