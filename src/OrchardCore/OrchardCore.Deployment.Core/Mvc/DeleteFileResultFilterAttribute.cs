using System.IO;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace OrchardCore.Deployment.Core.Mvc
{
    public class DeleteFileResultFilterAttribute : ResultFilterAttribute
    {
        public override void OnResultExecuted(ResultExecutedContext context)
        {
            var result = context.Result as PhysicalFileResult;

            if (result == null)
            {
                return;
            }

            var fileInfo = new FileInfo(result.FileName);

            if (fileInfo.Exists)
            {
                fileInfo.Delete();
            }
        }
    }
}
