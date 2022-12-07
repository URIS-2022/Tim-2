using Microsoft.AspNetCore.Http;

namespace OrchardCore.Modules
{
    // Comment 1
    public class DefaultOrchardHelper : IOrchardHelper
    {
        public DefaultOrchardHelper(IHttpContextAccessor httpContextAccessor)
        {
            HttpContext = httpContextAccessor.HttpContext;
        }

        public HttpContext HttpContext { get; set; }
    }
}
