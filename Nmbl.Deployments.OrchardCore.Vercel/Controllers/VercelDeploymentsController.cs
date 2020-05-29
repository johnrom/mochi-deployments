using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Nmbl.Deployments.Core.Services;
using OrchardCore.Admin;
using Nmbl.Deployments.Core.Models;
using Nmbl.Deployments.OrchardCore.Vercel.ViewModels;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;

namespace Nmbl.Deployments.OrchardCore.Vercel.Controllers
{
    [Admin]
    public class VercelDeploymentsController : Controller
    {
        private readonly IAuthorizationService _authorizationService;
        private readonly IDeploymentService _deploymentService;
        private readonly DeploymentStatusService _deploymentStatusService;
        private readonly DeploymentStatus _serviceState;
        private readonly ILogger<VercelDeploymentsController> _logger;

        public VercelDeploymentsController(
            IAuthorizationService authorizationService,
            IDeploymentService deploymentService,
            DeploymentStatusService deploymentStatusService,
            DeploymentStatus serviceState,
            ILogger<VercelDeploymentsController> logger
        ) {
            _authorizationService = authorizationService;
            _deploymentService = deploymentService;
            _deploymentStatusService = deploymentStatusService;
            _serviceState = serviceState;
            _logger = logger;
        }

        public async Task<IActionResult> Index()
        {
            if (!await _authorizationService.AuthorizeAsync(User, Permissions.ManageDeploymentSettings))
            {
                return Forbid();
            }

            IEnumerable<Deployment> deployments = null;

            try
            {
                deployments = await _deploymentService.GetDeploymentsAsync();
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Could not get latest Vercel Deployments.");
            }

            var model = new VercelDeploymentsAdminIndexViewModel
            {
                InitializationStatus = _deploymentService.GetInitializationStatus(),
                Deployments = deployments,
            };

            return View(model);
        }

        public async Task<IActionResult> Deploy()
        {
            if (!await _authorizationService.AuthorizeAsync(User, Permissions.ManageDeploymentSettings))
            {
                return Forbid();
            }

            await _deploymentService.DeployAsync();

            return RedirectToAction(nameof(Index));
        }
    }
}
