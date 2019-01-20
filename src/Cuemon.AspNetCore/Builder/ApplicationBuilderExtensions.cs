using System;
using Cuemon.AspNetCore.Hosting;
using Cuemon.AspNetCore.Http.Headers;
using Microsoft.AspNetCore.Builder;

namespace Cuemon.AspNetCore.Builder
{
    /// <summary>
    /// Extension methods for the <see cref="IApplicationBuilder"/> interface.
    /// </summary>
    public static class ApplicationBuilderExtensions
    {
        /// <summary>
        /// Adds a hosting environment HTTP header to the <see cref="IApplicationBuilder"/> request execution pipeline.
        /// </summary>
        /// <param name="builder">The type that provides the mechanisms to configure an application’s request pipeline.</param>
        /// <param name="setup">The <see cref="HostingEnvironmentOptions"/> middleware which need to be configured.</param>
        /// <returns>A reference to this instance after the operation has completed.</returns>
        public static IApplicationBuilder UseHostingEnvironmentHeader(this IApplicationBuilder builder, Action<HostingEnvironmentOptions> setup = null)
        {
            return ApplicationBuilderFactory.UseMiddlewareConfigurable<HostingEnvironmentMiddleware, HostingEnvironmentOptions>(builder, setup);
        }

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