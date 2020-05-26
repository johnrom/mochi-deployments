using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using LazyCache;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Options;
using Nmbl.Deployments.Core;
using Nmbl.Deployments.Core.Models;
using Nmbl.Deployments.Core.Services;
using OrchardCore.Environment.Cache;

namespace Nmbl.Deployments.OrchardCore.Services
{
    public abstract class BaseOrchardCoreDeploymentService : IDeploymentService
    {
        private readonly DeploymentStatusService _deploymentStatus;
        private readonly DeploymentOptions _deploymentOptions;
        private readonly IAppCache _cache;
        private readonly ISignal _signal;

        protected BaseOrchardCoreDeploymentService(
            DeploymentStatusService deploymentStatus,
            IOptions<DeploymentOptions> deploymentOptions,
            IAppCache cache,
            ISignal signal
        ) {
            _deploymentStatus = deploymentStatus;
            _deploymentOptions = deploymentOptions.Value;
            _cache = cache;
            _signal = signal;
        }
        protected virtual void ThrowIfReadNotConfigured()
        {
            if (!GetInitializationStatus().IsReadConfigured)
            {
                throw new InvalidOperationException("Reading Deployments is not configured in AppSettings.");
            }
        }
        protected virtual void ThrowIfDeploymentNotConfigured()
        {
            if (!GetInitializationStatus().IsDeploymentConfigured)
            {
                throw new InvalidOperationException("Deployment is not configured in AppSettings.");
            }
        }
        public virtual Task<Deployment> GetLatestProductionDeploymentAsync()
        {
            return _cache.GetOrAddAsync(DeploymentCacheKeys.LatestDeployment, async entry => {
                ThrowIfReadNotConfigured();

                entry.AddExpirationToken(_signal.GetToken(DeploymentCacheKeys.LatestDeployment));

                var latestDeployment = await GetLatestProductionDeploymentAsync_Impl();

                // Reset waiting for deployment, if another new production deployment has arrived
                _deploymentStatus.MaybeResetWaitingForDeployment(latestDeployment?.Id);

                entry.SetAbsoluteExpiration(DateTime.UtcNow + TimeSpan.FromSeconds(
                    _deploymentStatus.IsWaitingForDeployment()
                        ? _deploymentOptions.HighPriorityCacheInSeconds
                        : _deploymentOptions.CacheInSeconds
                ));

                return latestDeployment;
            });
        }
        /// <summary>
        /// For now, this is pretty lame TBH.
        ///
        /// Eventually need to make sure a Publish only happens once, at the end of a request,
        /// in case multiple changes trigger a Deployment.
        /// </summary>
        public virtual async Task<DeploymentResponse> DeployAsync()
        {
            ThrowIfDeploymentNotConfigured();

            try
            {
                // Get Uncached Latest Deployment to get the absolute latest deployment.
                var latestDeployment = await GetLatestProductionDeploymentAsync_Impl();

                _deploymentStatus.SetWaitingForDeployment(latestDeployment?.Id);

                return await DeployAsync_Impl();
            }
            catch
            {
                // Reset Waiting for Deployment if Deployment Fails
                // Should really add another status like `DeploymentFailed`.
                _deploymentStatus.ResetWaitingForDeployment();

                throw;
            }
        }
        public virtual void FlushCache()
        {
            _signal.SignalToken(DeploymentCacheKeys.LatestDeployment);
        }
        /// <summary>
        /// Defines the underlying Latest Production Deployment Method,
        /// because the public version implements a cache.
        /// </summary>
        protected abstract Task<Deployment> GetLatestProductionDeploymentAsync_Impl();
        /// <summary>
        /// Defines the underlying Deploy Method,
        /// because the public version implements caching and other conveniences.
        /// </summary>
        protected abstract Task<DeploymentResponse> DeployAsync_Impl();
        public abstract Task<Deployment> GetDeploymentAsync(string id);
        public abstract Task<IEnumerable<Deployment>> GetDeploymentsAsync(Dictionary<string, string> queryParams = null);
        public abstract InitializationState GetInitializationStatus();
    }
}
