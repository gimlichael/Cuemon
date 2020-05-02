using System;
using Cuemon.AspNetCore.Builder;
using Cuemon.AspNetCore.Hosting;
using Cuemon.AspNetCore.Http.Headers;
using Cuemon.AspNetCore.Http.Throttling;
using Cuemon.Extensions.AspNetCore.Hosting;
using Microsoft.AspNetCore.Builder;

namespace Cuemon.Extensions.AspNetCore.Builder
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
        /// <remarks>Default HTTP header name is <c>X-Correlation-ID</c>.</remarks>
        public static IApplicationBuilder UseCorrelationIdentifierHeader(this IApplicationBuilder builder, Action<CorrelationIdentifierOptions> setup = null)
        {
            return ApplicationBuilderFactory.UseMiddlewareConfigurable<CorrelationIdentifierMiddleware, CorrelationIdentifierOptions>(builder, setup);
        }

        /// <summary>
        /// Adds a request identifier HTTP header to the <see cref="IApplicationBuilder"/> request execution pipeline.
        /// </summary>
        /// <param name="builder">The type that provides the mechanisms to configure an application’s request pipeline.</param>
        /// <param name="setup">The <see cref="RequestIdentifierOptions"/> middleware which need to be configured.</param>
        /// <returns>A reference to this instance after the operation has completed.</returns>
        /// <remarks>Default HTTP header name is <c>X-Request-ID</c>.</remarks>
        public static IApplicationBuilder UseRequestIdentifierHeader(this IApplicationBuilder builder, Action<RequestIdentifierOptions> setup = null)
        {
            return ApplicationBuilderFactory.UseMiddlewareConfigurable<RequestIdentifierMiddleware, RequestIdentifierOptions>(builder, setup);
        }

        /// <summary>
        /// Adds a HTTP User-Agent header parser to the <see cref="IApplicationBuilder"/> request execution pipeline.
        /// </summary>
        /// <param name="builder">The type that provides the mechanisms to configure an application’s request pipeline.</param>
        /// <param name="setup">The <see cref="UserAgentSentinelOptions"/> middleware which need to be configured.</param>
        /// <returns>A reference to this instance after the operation has completed.</returns>
        public static IApplicationBuilder UseUserAgentSentinel(this IApplicationBuilder builder, Action<UserAgentSentinelOptions> setup = null)
        {
            return ApplicationBuilderFactory.UseMiddlewareConfigurable<UserAgentSentinelMiddleware, UserAgentSentinelOptions>(builder, setup);
        }

        /// <summary>
        /// Adds a custom rate limiting / throttling to the <see cref="IApplicationBuilder"/> request execution pipeline.
        /// </summary>
        /// <param name="builder">The type that provides the mechanisms to configure an application’s request pipeline.</param>
        /// <param name="setup">The <see cref="ThrottlingSentinelOptions"/> middleware which need to be configured.</param>
        /// <returns>A reference to this instance after the operation has completed.</returns>
        public static IApplicationBuilder UseCustomThrottlingSentinel(this IApplicationBuilder builder, Action<ThrottlingSentinelOptions> setup)
        {
            return ApplicationBuilderFactory.UseMiddlewareConfigurable<ThrottlingSentinelMiddleware, ThrottlingSentinelOptions>(builder, setup);
        }
    }
}