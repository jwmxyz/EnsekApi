using Ensek.Api.Factory;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Ensek.Api.Filters;

public class EnsekExceptionFilter : ExceptionFilterAttribute
{
    public override void OnException(ExceptionContext exceptionContext)
    {
        var responseFactory = exceptionContext.HttpContext.RequestServices.GetService<IResponseFactory>();

        exceptionContext.Result = responseFactory?.CreateResponse(exceptionContext.Exception);
    }
}