using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Razor.TagHelpers;
using Microsoft.Extensions.Logging;
using Nmbl.Deployments.Core.Services;
using Nmbl.Deployments.Vercel.Models;
using Nmbl.Deployments.OrchardCore.Vercel.ViewModels;

namespace Nmbl.Deployments.OrchardCore.Vercel.TagHelpers
{
    [HtmlTargetElement("vercel-latest-deployment", TagStructure = TagStructure.WithoutEndTag)]
    public class VercelLatestDeploymentTagHelper : TagHelper
    {
        [HtmlAttributeNotBound]
        [ViewContext]
        public ViewContext ViewContext { get; set; }

        private readonly IHtmlHelper _html;
        private readonly IDeploymentService _deploymentService;
        private readonly DeploymentStatusService _deploymentStatusService;
        private readonly ILogger<VercelLatestDeploymentTagHelper> _logger;

        public VercelLatestDeploymentTagHelper(
            IHtmlHelper html,
            IDeploymentService deploymentService,
            DeploymentStatusService deploymentStatusService,
            ILogger<VercelLatestDeploymentTagHelper> logger
        ) {
            _html = html;
            _deploymentService = deploymentService;
            _deploymentStatusService = deploymentStatusService;
            _logger = logger;
        }

        public override async Task ProcessAsync(TagHelperContext context, TagHelperOutput output)
        {
            if (!(_html is IViewContextAware htmlHelper)) {
                throw new Exception("Uncontextualizable IHtmlHelper.");
            }

            htmlHelper.Contextualize(ViewContext);

            var viewModel = new VercelLatestDeploymentViewModel
            {
                InitializationStatus = _deploymentService.GetInitializationStatus(),
            };

            if (
                viewModel.InitializationStatus.IsReadConfigured
            ) {
                try
                {
                    var latestDeployment = await _deploymentService.GetLatestProductionDeploymentAsync();
                    viewModel.IsWaitingForDeployment = _deploymentStatusService.IsWaitingForDeployment();

                    if (latestDeployment.Source is VercelDeployment vercelDeployment)
                    {
                        viewModel.LatestDeployment = vercelDeployment;
                    }
                }
                catch (Exception e)
                {
                    _logger.LogWarning(e, "Couldn't get latest production deployment.");
                }
            }

            var renderedHtml = await _html.PartialAsync("VercelLatestDeployment", viewModel);

            output.TagName = null;
            output.Content.SetHtmlContent(renderedHtml);
        }
    }
}
