using System;
using System.Threading.Tasks;
using Cuemon.Collections.Generic;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;

namespace Cuemon.AspNetCore.Hosting
{
    /// <summary>
    /// Provides a hosting environment middleware implementation for ASP.NET Core.
    /// </summary>
    public class HostingEnvironmentMiddleware : ConfigurableMiddleware<IHostingEnvironment, HostingEnvironmentOptions>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="HostingEnvironmentMiddleware"/> class.
        /// </summary>
        /// <param name="next">The delegate of the request pipeline to invoke.</param>
        /// <param name="setup">The <see cref="HostingEnvironmentOptions" /> which need to be configured.</param>
        public HostingEnvironmentMiddleware(RequestDelegate next, IOptions<HostingEnvironmentOptions> setup) : base(next, setup)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="HostingEnvironmentMiddleware"/> class.
        /// </summary>
        /// <param name="next">The delegate of the request pipeline to invoke.</param>
        /// <param name="setup">The <see cref="HostingEnvironmentOptions" /> which need to be configured.</param>
        public HostingEnvironmentMiddleware(RequestDelegate next, Action<HostingEnvironmentOptions> setup) : base(next, setup)
        {
        }

        /// <summary>
        /// Executes the <see cref="HostingEnvironmentMiddleware" />.
        /// </summary>
        /// <param name="context">The context of the current request.</param>
        /// <param name="he">The dependency injected <see cref="IHostingEnvironment"/> of <see cref="InvokeAsync"/>.</param>
        /// <returns>A task that represents the execution of this middleware.</returns>
        public override Task InvokeAsync(HttpContext context, IHostingEnvironment he)
        {
            context.Response.OnStarting(() =>
            {
                if (!Options?.SuppressHeaderPredicate(he) ?? false)
                {
                    context.Response.Headers.AddIfNotContainsKey(Options.HeaderName, he.EnvironmentName);
                }
                return Task.CompletedTask;
            });
            return Next(context);
        }
    }
}