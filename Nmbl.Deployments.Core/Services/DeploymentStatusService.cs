using Nmbl.Deployments.Core.Models;

namespace Nmbl.Deployments.Core.Services
{
    public class DeploymentStatusService
    {
        private readonly DeploymentStatus _deploymentStatus;

        public DeploymentStatusService(
            DeploymentStatus deploymentStatus
        ) {
            _deploymentStatus = deploymentStatus;
        }
        public bool IsWaitingForDeployment()
        {
            return _deploymentStatus.IsWaitingForDeployment;
        }
        /// <summary>
        /// Sometimes a new deployment takes a few seconds to register,
        /// so we want to mark the latest deployment as deprecated and enter a high-priority cache mode.
        /// </summary>
        public void SetWaitingForDeployment(string deploymentId)
        {
            _deploymentStatus.PreviousDeploymentId = deploymentId;
            _deploymentStatus.IsWaitingForDeployment = true;
        }
        /// <summary>
        /// When a new deployment comes in, reset waiting for deployment
        /// </summary>
        public void MaybeResetWaitingForDeployment(string deploymentId)
        {
            if (
                _deploymentStatus.IsWaitingForDeployment &&
                _deploymentStatus.PreviousDeploymentId != deploymentId
            ) {
                ResetWaitingForDeployment();
            }
        }
        /// <summary>
        /// Reset WaitingForDeployment
        /// </summary>
        public void ResetWaitingForDeployment()
        {
            _deploymentStatus.PreviousDeploymentId = null;
            _deploymentStatus.IsWaitingForDeployment = false;
        }
    }
}
