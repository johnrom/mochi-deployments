using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using LazyCache;
using Microsoft.Extensions.Options;
using Nmbl.Deployments.Core;
using Nmbl.Deployments.Core.Models;
using Nmbl.Deployments.Core.Services;
using Nmbl.Deployments.OrchardCore.Services;
using Nmbl.Deployments.Vercel.Services;
using OrchardCore.Environment.Cache;

namespace Nmbl.Deployments.OrchardCore.Vercel.Services
{
    public class VercelOrchardCoreDeploymentService : BaseOrchardCoreDeploymentService
    {
        private readonly VercelDeploymentService _vercelService;

        /// <summary>
        /// Is this an **anti-pattern**?
        /// </summary>
        public VercelOrchardCoreDeploymentService(
            VercelDeploymentService vercelService,
            DeploymentStatusService deploymentStatus,
            IOptions<DeploymentOptions> deploymentOptions,
            IAppCache cache,
            ISignal signal
        ): base(deploymentStatus, deploymentOptions, cache, signal) {
            _vercelService = vercelService;
        }
        protected override Task<Deployment> GetLatestProductionDeploymentAsync_Impl() =>
            _vercelService.GetLatestProductionDeploymentAsync();
        protected override Task<DeploymentResponse> DeployAsync_Impl() =>
            _vercelService.DeployAsync();
        public override Task<Deployment> GetDeploymentAsync(string id) =>
            _vercelService.GetDeploymentAsync(id);
        public override Task<IEnumerable<Deployment>> GetDeploymentsAsync(Dictionary<string, string> queryParams = null) =>
            _vercelService.GetDeploymentsAsync(queryParams);
        public override InitializationState GetInitializationStatus() =>
            _vercelService.GetInitializationStatus();
    }
}
