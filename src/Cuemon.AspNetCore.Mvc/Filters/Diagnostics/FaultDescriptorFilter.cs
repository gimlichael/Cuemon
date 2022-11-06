using System;
using System.Net;
using System.Reflection;
using Cuemon.AspNetCore.Diagnostics;
using Cuemon.AspNetCore.Http;
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
    /// <seealso cref="IExceptionFilter"/>
    /// <seealso cref="ExceptionDescriptor" />
    /// <seealso cref="MvcFaultDescriptorOptions"/>
    /// <seealso cref="RequestIdentifierMiddleware"/>
    /// <seealso cref="CorrelationIdentifierMiddleware"/>
    public class FaultDescriptorFilter : Configurable<MvcFaultDescriptorOptions>, IExceptionFilter
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="FaultDescriptorFilter"/> class.
        /// </summary>
        /// <param name="setup">The <see cref="MvcFaultDescriptorOptions"/> which need to be configured.</param>
        public FaultDescriptorFilter(IOptions<MvcFaultDescriptorOptions> setup) : base(setup.Value)
        {
            Validator.ThrowIfInvalidOptions(setup.Value, nameof(setup));
        }

        /// <summary>
        /// Called after an action has thrown an <see cref="Exception" />.
        /// </summary>
        /// <param name="context">The <see cref="ExceptionContext" />.</param>
        public virtual void OnException(ExceptionContext context)
        {
            if (context.ActionDescriptor is ControllerActionDescriptor actionDescriptor)
            {
                var exceptionDescriptor = Options.ExceptionDescriptorResolver?.Invoke(Options.UseBaseException ? context.Exception.GetBaseException() : context.Exception);
                if (exceptionDescriptor == null) { return; }
                context.HttpContext.Response.StatusCode = exceptionDescriptor.StatusCode;
                exceptionDescriptor.PostInitializeWith(actionDescriptor.MethodInfo.GetCustomAttributes<ExceptionDescriptorAttribute>());
                if (Options.HasRootHelpLink && exceptionDescriptor.HelpLink == null) { exceptionDescriptor.HelpLink = Options.RootHelpLink; }
                if (context.HttpContext.Items.TryGetValue(RequestIdentifierMiddleware.HttpContextItemsKey, out var requestId) && requestId != null) { exceptionDescriptor.RequestId = requestId.ToString(); }
                if (context.HttpContext.Items.TryGetValue(CorrelationIdentifierMiddleware.HttpContextItemsKey, out var correlationId) && correlationId != null) { exceptionDescriptor.CorrelationId = correlationId.ToString(); }
                Options.ExceptionCallback?.Invoke(context.HttpContext, context.Exception, exceptionDescriptor);
                if (Options.MarkExceptionHandled) { context.ExceptionHandled = true; }
                if (Options.RequestEvidenceProvider != null && Options.SensitivityDetails.HasFlag(FaultSensitivityDetails.Evidence)) { exceptionDescriptor.AddEvidence("Request", context.HttpContext.Request, Options.RequestEvidenceProvider); }
                if (exceptionDescriptor.Failure is HttpStatusCodeException httpFault)
                {
                    Decorator.Enclose(context.HttpContext.Response.Headers).AddRange(httpFault.Headers);
                }
                context.Result = new ExceptionDescriptorResult(exceptionDescriptor);
            }
        }
    }
}
