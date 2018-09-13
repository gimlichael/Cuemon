using System;
using System.Threading.Tasks;
using Cuemon.AspNetCore.Builder;
using Cuemon.Collections.Generic;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;

namespace Cuemon.AspNetCore
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

    /// <summary>
    /// This is a factory implementation of the <see cref="HostingEnvironmentMiddleware"/> class.
    /// </summary>
    public static class HostingEnvironmentBuilderExtension
    {
        /// <summary>
        /// Adds a hosting environment HTTP header to the <see cref="IApplicationBuilder"/> request execution pipeline.
        /// </summary>
        /// <param name="builder">The type that provides the mechanisms to configure an application’s request pipeline.</param>
        /// <param name="setup">The <see cref="CorrelationIdentifierOptions"/> middleware which need to be configured.</param>
        /// <returns>A reference to this instance after the operation has completed.</returns>
        public static IApplicationBuilder UseHostingEnvironmentHeader(this IApplicationBuilder builder, Action<HostingEnvironmentOptions> setup = null)
        {
            return ApplicationBuilderFactory.UseMiddlewareConfigurable<HostingEnvironmentMiddleware, HostingEnvironmentOptions>(builder, setup);
        }
    }
}