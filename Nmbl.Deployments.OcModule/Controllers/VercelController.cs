using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Nmbl.Vercel.OcModule.Models;
using Nmbl.Vercel.OcModule.Services;
using Nmbl.Vercel.OcModule.ViewModels;
using Nmbl.Vercel.Services;
using OrchardCore.Admin;

namespace Nmbl.Vercel.OcModule.Controllers
{
    [Admin]
    public class VercelController : Controller
    {
        private readonly IAuthorizationService _authorizationService;
        private readonly IVercelService _vercelService;
        private readonly VercelDeploymentStatusService _deploymentStatusService;
        private readonly VercelServiceState _serviceState;

        public VercelController(
            IAuthorizationService authorizationService,
            IVercelService vercelService,
            VercelDeploymentStatusService deploymentStatusService,
            VercelServiceState serviceState
        ) {
            _authorizationService = authorizationService;
            _vercelService = vercelService;
            _deploymentStatusService = deploymentStatusService;
            _serviceState = serviceState;
        }

        public async Task<IActionResult> Index()
        {
            if (!await _authorizationService.AuthorizeAsync(User, Permissions.ManageVercelSettings))
            {
                return Forbid();
            }

            var deployments = await _vercelService.GetDeploymentsAsync();
            var model = new VercelAdminIndexViewModel
            {
                Deployments = deployments.Deployments,
            };

            return View(model);
        }

        public async Task<IActionResult> Deploy()
        {
            if (!await _authorizationService.AuthorizeAsync(User, Permissions.ManageVercelSettings))
            {
                return Forbid();
            }

            await _deploymentStatusService.SetWaitingForDeploymentAndDeployAsync();

            return RedirectToAction(nameof(Index));
        }
    }
}
