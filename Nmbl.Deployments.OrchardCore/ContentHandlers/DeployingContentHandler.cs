using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Nmbl.Deployments.OrchardCore.Models;
using Nmbl.Deployments.OrchardCore.Services;
using OrchardCore.ContentManagement.Handlers;

namespace Nmbl.Deployments.OrchardCore.ContentHandlers
{
    public class DeployingContentHandler : ContentHandlerBase
    {
        private readonly DeploymentStatusService _deploymentStatusService;
        private readonly OrchardDeploymentOptions _deploymentOptions;
        private readonly ILogger<DeployingContentHandler> _logger;

        public DeployingContentHandler(
            DeploymentStatusService deploymentStatusService,
            IOptions<OrchardDeploymentOptions> deploymentOptions,
            ILogger<DeployingContentHandler> logger
        ) {
            _deploymentStatusService = deploymentStatusService;
            _deploymentOptions = deploymentOptions.Value;
            _logger = logger;
        }
        public override async Task PublishedAsync(PublishContentContext context)
        {
            if (_deploymentOptions.DeployOnPublish)
            {
                try
                {
                    await _deploymentStatusService.SetWaitingForDeploymentAndDeployAsync();
                }
                catch (Exception e)
                {
                    _logger.LogError(e, "Couldn't create deployment on PublishedAsync.");
                }
            }
        }
        /// <summary>
        /// This will create a build on CustomSettings,
        /// but for now I don't want to enable it by default.
        /// There are probably plenty of other scenarios that would trigger this deployment.
        /// </summary>
        public override async Task UpdatedAsync(UpdateContentContext context)
        {
            // If no ID, probably a custom setting
            if (
                context.ContentItem != null &&
                context.ContentItem.Id == 0 &&
                _deploymentOptions.Experimental_DeployOnCustomSettingsUpdated
            ) {
                try
                {
                    await _deploymentStatusService.SetWaitingForDeploymentAndDeployAsync();
                }
                catch (Exception e)
                {
                    _logger.LogError(e, "Couldn't create deployment on UpdatedAsync.");
                }
            }
        }
    }
}
