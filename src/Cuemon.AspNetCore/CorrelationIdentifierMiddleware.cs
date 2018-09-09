using System;
using System.Threading.Tasks;
using Cuemon.AspNetCore.Builder;
using Cuemon.Collections.Generic;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;

namespace Cuemon.AspNetCore
{
    /// <summary>
    /// Provides a Correlation ID middleware implementation for ASP.NET Core.
    /// </summary>
    public class CorrelationIdentifierMiddleware : ConfigurableMiddleware<CorrelationIdentifierOptions>
    {
        /// <summary>
        /// The key from where the Correlation ID is stored throughout the request scope.
        /// </summary>
        public const string HttpContextItemsKey = "Cuemon.AspNetCore.CorrelationMiddleware";

        /// <summary>
        /// Initializes a new instance of the <see cref="CorrelationIdentifierMiddleware"/> class.
        /// </summary>
        /// <param name="next">The delegate of the request pipeline to invoke.</param>
        /// <param name="setup">The <see cref="CorrelationIdentifierOptions" /> which need to be configured.</param>
        public CorrelationIdentifierMiddleware(RequestDelegate next, IOptions<CorrelationIdentifierOptions> setup) : base(next, setup)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CorrelationIdentifierMiddleware"/> class.
        /// </summary>
        /// <param name="next">The delegate of the request pipeline to invoke.</param>
        /// <param name="setup">The <see cref="CorrelationIdentifierOptions"/> which need to be configured.</param>
        public CorrelationIdentifierMiddleware(RequestDelegate next, Action<CorrelationIdentifierOptions> setup) : base(next, setup)
        {
        }

        /// <summary>
        /// Executes the <see cref="CorrelationIdentifierMiddleware" />.
        /// </summary>
        /// <param name="context">The context of the current request.</param>
        /// <returns>A task that represents the execution of this middleware.</returns>
        public override Task InvokeAsync(HttpContext context)
        {
            if (!context.Request.Headers.TryGetValue(Options.HeaderName, out var correlationId)) { correlationId = Options.CorrelationProvider().CorrelationId; }
            context.Items.AddIfNotContainsKey(HttpContextItemsKey, correlationId);
            context.Response.OnStarting(() =>
            {
                context.Response.Headers.AddOrUpdate(Options.HeaderName, correlationId);
                return Task.CompletedTask;
            });
            return Next(context);
        }
    }

    /// <summary>
    /// This is a factory implementation of the <see cref="CorrelationIdentifierMiddleware"/> class.
    /// </summary>
    public static class CorrelationBuilderExtension
    {
        /// <summary>
        /// Adds a correlation identifier HTTP header to the <see cref="IApplicationBuilder"/> request execution pipeline.
        /// </summary>
        /// <param name="builder">The type that provides the mechanisms to configure an application’s request pipeline.</param>
        /// <param name="setup">The <see cref="CorrelationIdentifierOptions"/> middleware which need to be configured.</param>
        /// <returns>A reference to this instance after the operation has completed.</returns>
        public static IApplicationBuilder UseCorrelationIdentifierHeader(this IApplicationBuilder builder, Action<CorrelationIdentifierOptions> setup = null)
        {
            return ApplicationBuilderFactory.UseMiddlewareConfigurable<CorrelationIdentifierMiddleware, CorrelationIdentifierOptions>(builder, setup);
        }
    }
}