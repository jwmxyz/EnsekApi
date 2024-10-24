using Ensek.Api.Factory;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Ensek.Api.Filters;

public class EnsekExceptionFilter : ExceptionFilterAttribute
{
    /// <summary>
    /// An exception filter that will catch any exceptions thrown within the application and mask them.
    /// </summary>
    /// <param name="exceptionContext"></param>
    public override void OnException(ExceptionContext exceptionContext)
    {
        var responseFactory = exceptionContext.HttpContext.RequestServices.GetService<IResponseFactory>();

        exceptionContext.Result = responseFactory?.CreateResponse(exceptionContext.Exception);
    }
}