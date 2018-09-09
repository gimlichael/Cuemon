using System;
using System.Net;
using System.Reflection;
using Cuemon.Diagnostics;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Cuemon.AspNetCore.Mvc.Filters.Diagnostics
{
    /// <summary>
    /// A filter that, after an action has faulted, provides developer friendly information about an <see cref="Exception"/> along with a correct <see cref="HttpStatusCode"/>.
    /// </summary>
    /// <seealso cref="IExceptionFilter"/>.
    public class ExceptionDescriptorFilter : IExceptionFilter
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ExceptionDescriptorFilter"/> class.
        /// </summary>
        /// <param name="setup">The <see cref="ExceptionDescriptorFilterOptions"/> which need to be configured.</param>
        public ExceptionDescriptorFilter(Action<ExceptionDescriptorFilterOptions> setup = null)
        {
            Options = setup.ConfigureOptions();
        }

        /// <summary>
        /// Gets the configured options of this <see cref="ExceptionDescriptorFilter"/>.
        /// </summary>
        /// <value>The configured options of this <see cref="ExceptionDescriptorFilter"/>.</value>
        private ExceptionDescriptorFilterOptions Options { get; }

        /// <summary>
        /// Called after an action has thrown an <see cref="Exception" />.
        /// </summary>
        /// <param name="context">The <see cref="ExceptionContext" />.</param>
        public void OnException(ExceptionContext context)
        {
            var actionDescriptor = context.ActionDescriptor as ControllerActionDescriptor;
            var descriptor = context.ParseMethodDescriptor(actionDescriptor);
            if (descriptor != null)
            {
                var statusCode = Options.HttpStatusCodeResolver?.Invoke(context.Exception);
                if (statusCode.HasValue) { context.HttpContext.Response.StatusCode = (int)statusCode.Value; }
                var exceptionDescriptor = Options.ExceptionDescriptorResolver?.Invoke(context.Exception);
                exceptionDescriptor?.PostInitializeWith(actionDescriptor?.MethodInfo.GetCustomAttributes<ExceptionDescriptorAttribute>());
                Options.ExceptionCallback?.Invoke(context.Exception, exceptionDescriptor);
                context.Result = new ExceptionDescriptorResult(exceptionDescriptor);
            }
        }
    }
}