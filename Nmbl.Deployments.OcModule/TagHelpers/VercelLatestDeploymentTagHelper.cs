using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Razor.TagHelpers;
using Microsoft.Extensions.Logging;
using Nmbl.Vercel.OcModule.Services;
using Nmbl.Vercel.OcModule.ViewModels;
using Nmbl.Vercel.Services;

namespace Nmbl.Vercel.OcModule.TagHelpers
{
    [HtmlTargetElement("vercel-latest-deployment", TagStructure = TagStructure.WithoutEndTag)]
    public class VercelLatestDeploymentTagHelper : TagHelper
    {
        [HtmlAttributeNotBound]
        [ViewContext]
        public ViewContext ViewContext { get; set; }

        private readonly IHtmlHelper _html;
        private readonly IVercelService _vercelService;
        private readonly VercelDeploymentStatusService _deploymentStatusService;
        private readonly ILogger<VercelLatestDeploymentTagHelper> _logger;

        public VercelLatestDeploymentTagHelper(
            IHtmlHelper html,
            IVercelService vercelService,
            VercelDeploymentStatusService deploymentStatusService,
            ILogger<VercelLatestDeploymentTagHelper> logger
        ) {
            _html = html;
            _vercelService = vercelService;
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
                InitializationStatus = _vercelService.GetInitializationStatus(),
            };

            if (
                viewModel.InitializationStatus.ConfiguredApiToken
            ) {
                try
                {
                    viewModel.IsWaitingForDeployment = _deploymentStatusService.IsWaitingForDeployment();
                    viewModel.LatestDeployment = await _deploymentStatusService.GetLatestProductionDeploymentAsync();
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
