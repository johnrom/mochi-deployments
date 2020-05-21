using System;
using System.Threading.Tasks;
using LazyCache;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Options;
using Nmbl.OcModules.Vercel.Models;
using Nmbl.Vercel.Models;
using Nmbl.Vercel.Services;
using OrchardCore.Environment.Cache;

namespace Nmbl.OcModules.Vercel.Services
{
    public class VercelDeploymentStatusService
    {
        private readonly IVercelService _vercelService;
        private readonly VercelServiceState _serviceState;
        private readonly VercelDeploymentOptions _cacheOptions;
        private readonly IAppCache _cache;
        private readonly ISignal _signal;

        public VercelDeploymentStatusService(
            IVercelService vercelService,
            VercelServiceState serviceState,
            IOptions<VercelDeploymentOptions> cacheOptions,
            IAppCache cache,
            ISignal signal
        ) {
            _vercelService = vercelService;
            _serviceState = serviceState;
            _cacheOptions = cacheOptions.Value;
            _cache = cache;
            _signal = signal;
        }
        public bool IsWaitingForDeployment()
        {
            return _serviceState.IsWaitingForDeployment;
        }
        public void FlushCache()
        {
            _signal.SignalToken(VercelCacheKeys.LatestDeployment);
        }
        private void ThrowIfApiTokenNotConfigured()
        {
            var initializationStatus = _vercelService.GetInitializationStatus();

            if (!initializationStatus.ConfiguredApiToken)
            {
                throw new InvalidOperationException("Api Token is not configured in AppSettings.");
            }
        }
        /// <summary>
        /// Sometimes a new deployment takes a few seconds to register,
        /// so we want to mark the latest deployment as deprecated and enter a high-priority cache mode.
        /// </summary>
        public async Task SetWaitingForDeploymentAsync()
        {
            // skip cache to get the absolute latest deployment
            var latestDeployment = await _vercelService.GetLatestProductionDeploymentAsync();

            _serviceState.PreviousDeploymentUid = latestDeployment?.Uid;
            _serviceState.IsWaitingForDeployment = true;

            FlushCache();
        }
        /// <summary>
        /// Switch back to low priority cache mode because we found a new deployment.
        /// </summary>
        public void ResetWaitingForDeployment()
        {
            _serviceState.PreviousDeploymentUid = null;
            _serviceState.IsWaitingForDeployment = false;
        }
        public Task<VercelDeployment> GetLatestProductionDeploymentAsync()
        {
            return _cache.GetOrAddAsync(VercelCacheKeys.LatestDeployment, async entry => {
                entry.AddExpirationToken(_signal.GetToken(VercelCacheKeys.LatestDeployment));

                var deployment = await _vercelService.GetLatestProductionDeploymentAsync();

                if (
                    _serviceState.IsWaitingForDeployment &&
                    _serviceState.PreviousDeploymentUid != deployment?.Uid
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
            var initializationStatus = _vercelService.GetInitializationStatus();

            if (!initializationStatus.ConfiguredDeployHook)
            {
                throw new InvalidOperationException("Deploy hook is not configured in AppSettings.");
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
                await _vercelService.DeployAsync();
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
