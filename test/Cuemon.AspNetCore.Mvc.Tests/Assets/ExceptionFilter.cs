using Cuemon.AspNetCore.Http.Throttling;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Cuemon.AspNetCore.Mvc.Assets
{
    public class ExceptionFilter : ExceptionFilterAttribute
    {
        public override void OnException(ExceptionContext context)
        {
            if (context.ActionDescriptor is ControllerActionDescriptor)
            {
                var exception = context.Exception;
                if (exception is ThrottlingException)
                {
                    context.ExceptionHandled = true;
                    context.Result = new TooManyRequestsObjectResult(exception.Message);
                }
            }
        }
    }
}