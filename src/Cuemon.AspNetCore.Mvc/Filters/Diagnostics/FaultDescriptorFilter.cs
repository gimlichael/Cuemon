using System;
using System.Net;
using System.Reflection;
using Cuemon.AspNetCore.Http.Headers;
using Cuemon.Configuration;
using Cuemon.Diagnostics;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Options;

namespace Cuemon.AspNetCore.Mvc.Filters.Diagnostics
{
    /// <summary>
    /// A filter that, after an action has faulted, provides developer friendly information about an <see cref="Exception"/> along with a correct <see cref="HttpStatusCode"/>.
    /// </summary>
    /// <seealso cref="IExceptionFilter"/>.
    public class FaultDescriptorFilter : Configurable<FaultDescriptorOptions>, IExceptionFilter
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="FaultDescriptorFilter"/> class.
        /// </summary>
        /// <param name="setup">The <see cref="FaultDescriptorOptions"/> which need to be configured.</param>
        public FaultDescriptorFilter(IOptions<FaultDescriptorOptions> setup) : base(setup.Value)
        {
        }

        /// <summary>
        /// Called after an action has thrown an <see cref="Exception" />.
        /// </summary>
        /// <param name="context">The <see cref="ExceptionContext" />.</param>
        public virtual void OnException(ExceptionContext context)
        {
            if (context.ActionDescriptor is ControllerActionDescriptor actionDescriptor)
            {
                var statusCode = Options.HttpStatusCodeResolver?.Invoke(context.Exception);
                if (statusCode.HasValue) { context.HttpContext.Response.StatusCode = (int)statusCode.Value; }
                var exceptionDescriptor = Options.ExceptionDescriptorResolver?.Invoke(context.Exception);
                exceptionDescriptor?.PostInitializeWith(actionDescriptor.MethodInfo.GetCustomAttributes<ExceptionDescriptorAttribute>());
                if (exceptionDescriptor != null && context.HttpContext.Items.TryGetValue(CorrelationIdentifierMiddleware.HttpContextItemsKey, out var correlationId))
                {
                    exceptionDescriptor.RequestId = correlationId.ToString();
                }
                Options.ExceptionCallback?.Invoke(context.Exception, exceptionDescriptor);
                if (Options.MarkExceptionHandled) { context.ExceptionHandled = true; }
                if (Options.IncludeRequest) { exceptionDescriptor?.AddEvidence("Request", context.HttpContext.Request, request => new HttpRequestEvidence(request, Options.RequestBodyParser)); }
                Options.ExceptionDescriptorHandler?.Invoke(context, exceptionDescriptor);
                context.Result = new ExceptionDescriptorResult(exceptionDescriptor);
            }
        }
    }
}