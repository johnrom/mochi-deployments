using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Nmbl.Deployments.OrchardCore.Vercel.ViewModels;
using Nmbl.Deployments.Core.Services;
using OrchardCore.Admin;
using Nmbl.Deployments.Core.Models;
using System.Linq;
using Nmbl.Deployments.Vercel.Models;

namespace Nmbl.Deployments.OrchardCore.Vercel.Controllers
{
    [Admin]
    public class VercelDeploymentsController : Controller
    {
        private readonly IAuthorizationService _authorizationService;
        private readonly IDeploymentService _deploymentService;
        private readonly DeploymentStatusService _deploymentStatusService;
        private readonly DeploymentStatus _serviceState;

        public VercelDeploymentsController(
            IAuthorizationService authorizationService,
            IDeploymentService deploymentService,
            DeploymentStatusService deploymentStatusService,
            DeploymentStatus serviceState
        ) {
            _authorizationService = authorizationService;
            _deploymentService = deploymentService;
            _deploymentStatusService = deploymentStatusService;
            _serviceState = serviceState;
        }

        public async Task<IActionResult> Index()
        {
            if (!await _authorizationService.AuthorizeAsync(User, Permissions.ManageVercelSettings))
            {
                return Forbid();
            }

            var deployments = (await _deploymentService.GetDeploymentsAsync()).Select(deployment => {
                if (deployment.Source is VercelDeployment vercelDeployment) {
                    return vercelDeployment;
                }

                return null;
            });

            var model = new VercelAdminIndexViewModel
            {
                Deployments = deployments,
            };

            return View(model);
        }

        public async Task<IActionResult> Deploy()
        {
            if (!await _authorizationService.AuthorizeAsync(User, Permissions.ManageVercelSettings))
            {
                return Forbid();
            }

            await _deploymentService.DeployAsync();

            return RedirectToAction(nameof(Index));
        }
    }
}
