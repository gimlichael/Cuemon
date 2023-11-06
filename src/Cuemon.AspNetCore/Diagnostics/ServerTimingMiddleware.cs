using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;

namespace Cuemon.AspNetCore.Diagnostics
{
    /// <summary>
    /// Provides a Server-Timing middleware implementation for ASP.NET Core.
    /// </summary>
    public class ServerTimingMiddleware : Middleware
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ServerTimingMiddleware"/> class.
        /// </summary>
        /// <param name="next">The delegate of the request pipeline to invoke.</param>
        public ServerTimingMiddleware(RequestDelegate next) : base(next)
        {
        }

        /// <summary>
        /// Executes the <see cref="ServerTimingMiddleware" />.
        /// </summary>
        /// <param name="context">The context of the current request.</param>
        /// <returns>A task that represents the execution of this middleware.</returns>
        public override Task InvokeAsync(HttpContext context)
        {
            context.Response.OnStarting(() =>
            {
                var serverTiming = context.RequestServices.GetService<IServerTiming>();
                if (serverTiming != null)
                {
                    context.Response.Headers.Append(ServerTiming.HeaderName, serverTiming.Metrics.Select(metric => metric.ToString()).ToArray());
                }
                return Task.CompletedTask;
            });
            return Next(context);
        }
    }
}
