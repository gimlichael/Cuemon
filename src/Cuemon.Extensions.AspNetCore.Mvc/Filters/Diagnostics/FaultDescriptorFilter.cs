using System;
using System.Net;
using System.Reflection;
using Cuemon.AspNetCore.Mvc;
using Cuemon.Configuration;
using Cuemon.Diagnostics;
using Cuemon.Extensions.AspNetCore.Http.Headers;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Options;

namespace Cuemon.Extensions.AspNetCore.Mvc.Filters.Diagnostics
{
    /// <summary>
    /// A filter that, after an action has faulted, provides developer friendly information about an <see cref="Exception"/> along with a correct <see cref="HttpStatusCode"/>.
    /// Implements the <see cref="IExceptionFilter" />
    /// </summary>
    /// <seealso cref="IExceptionFilter"/>
    /// <seealso cref="ExceptionDescriptor" />
    /// <seealso cref="FaultDescriptorOptions"/>
    /// <seealso cref="RequestIdentifierMiddleware"/>
    /// <seealso cref="CorrelationIdentifierMiddleware"/>
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
        /// <param name="context">The <see cref="Microsoft.AspNetCore.Mvc.Filters.ExceptionContext" />.</param>
        public virtual void OnException(Microsoft.AspNetCore.Mvc.Filters.ExceptionContext context)
        {
            if (context.ActionDescriptor is ControllerActionDescriptor actionDescriptor)
            {
                var exceptionDescriptor = Options.ExceptionDescriptorResolver?.Invoke(Options.UseBaseException ? context.Exception.GetBaseException() : context.Exception);
                if (exceptionDescriptor == null) { return; }
                context.HttpContext.Response.StatusCode = exceptionDescriptor.StatusCode;
                exceptionDescriptor.PostInitializeWith(actionDescriptor.MethodInfo.GetCustomAttributes<ExceptionDescriptorAttribute>());
                if (Options.HasRootHelpLink && exceptionDescriptor.HelpLink == null) { exceptionDescriptor.HelpLink = Options.RootHelpLink; }
                if (context.HttpContext.Items.TryGetValue(RequestIdentifierMiddleware.HttpContextItemsKey, out var requestId)) { exceptionDescriptor.RequestId = requestId.ToString(); }
                if (context.HttpContext.Items.TryGetValue(CorrelationIdentifierMiddleware.HttpContextItemsKey, out var correlationId)) { exceptionDescriptor.CorrelationId = correlationId.ToString(); }
                Options.ExceptionCallback?.Invoke(context.Exception, exceptionDescriptor);
                if (Options.MarkExceptionHandled) { context.ExceptionHandled = true; }
                if (Options.IncludeRequest) { exceptionDescriptor.AddEvidence("Request", context.HttpContext.Request, request => new HttpRequestEvidence(request, Options.RequestBodyParser)); }
                Options.ExceptionDescriptorHandler?.Invoke(context, exceptionDescriptor);
                context.Result = new ExceptionDescriptorResult(exceptionDescriptor);
            }
        }
    }
}