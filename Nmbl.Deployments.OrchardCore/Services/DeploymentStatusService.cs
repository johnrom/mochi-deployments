using System;
using System.Threading.Tasks;
using LazyCache;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Options;
using Nmbl.Deployments.Core.Models;
using Nmbl.Deployments.Core.Services;
using OrchardCore.Environment.Cache;

namespace Nmbl.Deployments.OrchardCore.Services
{
    public class DeploymentStatusService
    {
        private readonly IDeploymentService _deploymentService;
        private readonly DeploymentStatus _deploymentStatus;
        private readonly DeploymentOptions _cacheOptions;
        private readonly IAppCache _cache;
        private readonly ISignal _signal;

        public DeploymentStatusService(
            IDeploymentService deploymentService,
            DeploymentStatus deploymentStatus,
            IOptions<DeploymentOptions> cacheOptions,
            IAppCache cache,
            ISignal signal
        ) {
            _deploymentService = deploymentService;
            _deploymentStatus = deploymentStatus;
            _cacheOptions = cacheOptions.Value;
            _cache = cache;
            _signal = signal;
        }
        public bool IsWaitingForDeployment()
        {
            return _deploymentStatus.IsWaitingForDeployment;
        }
        public void FlushCache()
        {
            _signal.SignalToken(DeploymentCacheKeys.LatestDeployment);
        }
        private void ThrowIfApiTokenNotConfigured()
        {
            var initializationStatus = _deploymentService.GetInitializationStatus();

            if (!initializationStatus.IsReadConfigured)
            {
                throw new InvalidOperationException("Reading Deployments is not configured in AppSettings.");
            }
        }
        /// <summary>
        /// Sometimes a new deployment takes a few seconds to register,
        /// so we want to mark the latest deployment as deprecated and enter a high-priority cache mode.
        /// </summary>
        public async Task SetWaitingForDeploymentAsync()
        {
            // skip cache to get the absolute latest deployment
            var latestDeployment = await _deploymentService.GetLatestProductionDeploymentAsync();

            _deploymentStatus.PreviousDeploymentUid = latestDeployment?.Id;
            _deploymentStatus.IsWaitingForDeployment = true;

            FlushCache();
        }
        /// <summary>
        /// Switch back to low priority cache mode because we found a new deployment.
        /// </summary>
        public void ResetWaitingForDeployment()
        {
            _deploymentStatus.PreviousDeploymentUid = null;
            _deploymentStatus.IsWaitingForDeployment = false;
        }
        public Task<Deployment> GetLatestProductionDeploymentAsync()
        {
            return _cache.GetOrAddAsync(DeploymentCacheKeys.LatestDeployment, async entry => {
                entry.AddExpirationToken(_signal.GetToken(DeploymentCacheKeys.LatestDeployment));

                var deployment = await _deploymentService.GetLatestProductionDeploymentAsync();

                if (
                    _deploymentStatus.IsWaitingForDeployment &&
                    _deploymentStatus.PreviousDeploymentUid != deployment?.Id
                ) {
                    ResetWaitingForDeployment();
                }

                entry.SetAbsoluteExpiration(DateTime.UtcNow + TimeSpan.FromSeconds(
                    IsWaitingForDeployment()
                        ? _cacheOptions.HighPriorityCacheInSeconds
                        : _cacheOptions.CacheInSeconds
                ));

                return deployment;
            });
        }

        private void ThrowIfDeploymentNotConfigured()
        {
            var initializationStatus = _deploymentService.GetInitializationStatus();

            if (!initializationStatus.IsDeploymentConfigured)
            {
                throw new InvalidOperationException("Deployment is not configured in AppSettings.");
            }
        }
        /// <summary>
        /// For now, this is pretty lame TBH.
        ///
        /// Eventually need to make sure a Publish only happens once, at the end of a request,
        /// in case multiple changes trigger a Deployment.
        /// </summary>
        public async Task SetWaitingForDeploymentAndDeployAsync()
        {
            ThrowIfDeploymentNotConfigured();

            try
            {
                await SetWaitingForDeploymentAsync();
                await _deploymentService.DeployAsync();
            }
            catch
            {
                // Reset Waiting for Deployment if Deployment Fails
                // Should really add another status like `DeploymentFailed`.
                ResetWaitingForDeployment();

                throw;
            }
        }
    }
}
