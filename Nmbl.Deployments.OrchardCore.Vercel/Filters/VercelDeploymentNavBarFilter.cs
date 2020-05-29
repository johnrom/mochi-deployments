using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Logging;
using OrchardCore.DisplayManagement;
using OrchardCore.DisplayManagement.Layout;
using OrchardCore.Admin;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Nmbl.Deployments.OrchardCore.Vercel.Filters
{
    public class VercelDeploymentNavBarFilter : IAsyncResultFilter
    {
        private readonly ILayoutAccessor _layoutAccessor;
        private readonly ILogger<VercelDeploymentNavBarFilter> _logger;
        private readonly IShapeFactory _shapeFactory;

        public VercelDeploymentNavBarFilter(
            ILayoutAccessor layoutAccessor,
            IShapeFactory shapeFactory,
            ILogger<VercelDeploymentNavBarFilter> logger)
        {
            _layoutAccessor = layoutAccessor;
            _shapeFactory = shapeFactory;
            _logger = logger;
        }

        public async Task OnResultExecutionAsync(ResultExecutingContext context, ResultExecutionDelegate next)
        {
            // Only display on admin pages
            if (!(context.Result is ViewResult || context.Result is PageResult)
                || !AdminAttribute.IsApplied(context.HttpContext))
            {
                await next();
                return;
            }

            dynamic layout = await _layoutAccessor.GetLayoutAsync();

            layout.Zones["NavbarTop"]
                .Add(await _shapeFactory.New.DeploymentNavBar());

            await next();
            return;
        }
    }
}