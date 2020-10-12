using System;
using Cuemon.AspNetCore.Builder;
using Cuemon.AspNetCore.Hosting;
using Cuemon.AspNetCore.Http.Headers;
using Cuemon.AspNetCore.Http.Throttling;
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
        /// <param name="setup">The <see cref="HostingEnvironmentOptions"/> middleware which may be configured.</param>
        /// <returns>A reference to this instance after the operation has completed.</returns>
        /// <remarks>Default HTTP header name is <c>X-Hosting-Environment</c>.</remarks>
        public static IApplicationBuilder UseHostingEnvironment(this IApplicationBuilder builder, Action<HostingEnvironmentOptions> setup = null)
        {
            return MiddlewareBuilderFactory.UseMiddlewareConfigurable<HostingEnvironmentMiddleware, HostingEnvironmentOptions>(builder, setup);
        }

        /// <summary>
        /// Adds a correlation identifier HTTP header to the <see cref="IApplicationBuilder"/> request execution pipeline.
        /// </summary>
        /// <param name="builder">The type that provides the mechanisms to configure an application’s request pipeline.</param>
        /// <param name="setup">The <see cref="CorrelationIdentifierOptions"/> middleware which may be configured.</param>
        /// <returns>A reference to this instance after the operation has completed.</returns>
        /// <remarks>Default HTTP header name is <c>X-Correlation-ID</c>.</remarks>
        public static IApplicationBuilder UseCorrelationIdentifier(this IApplicationBuilder builder, Action<CorrelationIdentifierOptions> setup = null)
        {
            return MiddlewareBuilderFactory.UseMiddlewareConfigurable<CorrelationIdentifierMiddleware, CorrelationIdentifierOptions>(builder, setup);
        }

        /// <summary>
        /// Adds a request identifier HTTP header to the <see cref="IApplicationBuilder"/> request execution pipeline.
        /// </summary>
        /// <param name="builder">The type that provides the mechanisms to configure an application’s request pipeline.</param>
        /// <param name="setup">The <see cref="RequestIdentifierOptions"/> middleware which may be configured.</param>
        /// <returns>A reference to this instance after the operation has completed.</returns>
        /// <remarks>Default HTTP header name is <c>X-Request-ID</c>.</remarks>
        public static IApplicationBuilder UseRequestIdentifier(this IApplicationBuilder builder, Action<RequestIdentifierOptions> setup = null)
        {
            return MiddlewareBuilderFactory.UseMiddlewareConfigurable<RequestIdentifierMiddleware, RequestIdentifierOptions>(builder, setup);
        }

        /// <summary>
        /// Adds a HTTP User-Agent header parser to the <see cref="IApplicationBuilder"/> request execution pipeline.
        /// </summary>
        /// <param name="builder">The type that provides the mechanisms to configure an application’s request pipeline.</param>
        /// <param name="setup">The <see cref="UserAgentSentinelOptions"/> middleware which may be configured.</param>
        /// <returns>A reference to this instance after the operation has completed.</returns>
        public static IApplicationBuilder UseUserAgentSentinel(this IApplicationBuilder builder, Action<UserAgentSentinelOptions> setup = null)
        {
            return MiddlewareBuilderFactory.UseMiddlewareConfigurable<UserAgentSentinelMiddleware, UserAgentSentinelOptions>(builder, setup);
        }

        /// <summary>
        /// Adds a HTTP requests rate limiting / throttling guard to the <see cref="IApplicationBuilder"/> request execution pipeline.
        /// </summary>
        /// <param name="builder">The type that provides the mechanisms to configure an application’s request pipeline.</param>
        /// <param name="setup">The <see cref="ThrottlingSentinelOptions"/> middleware which may be configured.</param>
        /// <returns>A reference to this instance after the operation has completed.</returns>
        public static IApplicationBuilder UseThrottlingSentinel(this IApplicationBuilder builder, Action<ThrottlingSentinelOptions> setup = null)
        {
            return MiddlewareBuilderFactory.UseMiddlewareConfigurable<ThrottlingSentinelMiddleware, ThrottlingSentinelOptions>(builder, setup);
        }
    }
}