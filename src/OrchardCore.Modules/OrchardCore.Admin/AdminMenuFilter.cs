using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.RazorPages;
using OrchardCore.DisplayManagement;
using OrchardCore.DisplayManagement.Layout;
using OrchardCore.DisplayManagement.Shapes;

namespace OrchardCore.Admin
{
    /// <summary>
    /// This filter inject a Navigation shape in the Navigation zone of the Layout
    /// for any ViewResult returned from an Admin controller.
    /// </summary>
    public class AdminMenuFilter : IAsyncResultFilter
    {
        private readonly ILayoutAccessor _layoutAccessor;
        private readonly IShapeFactory _shapeFactory;

        public AdminMenuFilter(ILayoutAccessor layoutAccessor,
            IShapeFactory shapeFactory)
        {
            _layoutAccessor = layoutAccessor;
            _shapeFactory = shapeFactory;
        }

        public async Task OnResultExecutionAsync(ResultExecutingContext context, ResultExecutionDelegate next)
        {
            // Should only run on a full view rendering result
            if (!(context.Result is ViewResult) && !(context.Result is PageResult))
            {
                await next();
                return;
            }

            // Should only run on the Admin
            if (!AdminAttribute.IsApplied(context.HttpContext))
            {
                await next();
                return;
            }

            // Should only run for authenticated users
            if (!(context.HttpContext.User?.Identity?.IsAuthenticated ?? false))
            {
                await next();
                return;
            }

            // Don't create the menu if the status code is 3xx
            var statusCode = context.HttpContext.Response.StatusCode;
            if (statusCode >= 300 && statusCode < 400)
            {
                await next();
                return;
            }

            // Populate main nav
            var menuShape = await _shapeFactory.CreateAsync("Navigation",
                Arguments.From(new
                {
                    MenuName = "admin",
                    context.RouteData,
                }));

            var layout = await _layoutAccessor.GetLayoutAsync();

            var navigation = layout.Zones["Navigation"];

            if (navigation is Shape shape)
            {
                await shape.AddAsync(menuShape);
            }

            await next();
        }
    }
}
