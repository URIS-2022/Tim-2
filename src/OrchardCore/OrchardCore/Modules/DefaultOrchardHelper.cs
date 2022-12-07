using Microsoft.AspNetCore.Http;

namespace OrchardCore.Modules
{
    // Comment 1
    // Comment 2
    public class DefaultOrchardHelper : IOrchardHelper
    {
        public DefaultOrchardHelper(IHttpContextAccessor httpContextAccessor)
        {
            HttpContext = httpContextAccessor.HttpContext;
        }

        public HttpContext HttpContext { get; set; }
    }
}
