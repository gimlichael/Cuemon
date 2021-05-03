using Cuemon.AspNetCore.Builder;
using Cuemon.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Builder;

namespace Cuemon.Extensions.AspNetCore.Diagnostics
{
    /// <summary>
    /// Extension methods for the <see cref="IApplicationBuilder"/> interface.
    /// </summary>
    public static class ApplicationBuilderExtensions
    {
        /// <summary>
        /// Adds a Server-Timing HTTP header to the <see cref="IApplicationBuilder"/> request execution pipeline.
        /// </summary>
        /// <param name="builder">The type that provides the mechanisms to configure an application’s request pipeline.</param>
        /// <returns>A reference to this instance after the operation has completed.</returns>
        public static IApplicationBuilder UseServerTiming(this IApplicationBuilder builder)
        {
            return MiddlewareBuilderFactory.UseMiddleware<ServerTimingMiddleware>(builder);
        }
    }
}