using System;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Cuemon.AspNetCore.Diagnostics
{
    /// <summary>
    /// Provides a Server-Timing middleware implementation for ASP.NET Core.
    /// </summary>
    public class ServerTimingMiddleware : ConfigurableMiddleware<ILogger<ServerTimingMiddleware>, IHostEnvironment, ServerTimingOptions>
	{
		/// <summary>
        /// Initializes a new instance of the <see cref="ServerTimingMiddleware"/> class.
        /// </summary>
        /// <param name="next">The delegate of the request pipeline to invoke.</param>
        /// <param name="setup">The <see cref="ServerTimingOptions" /> which need to be configured.</param>
        public ServerTimingMiddleware(RequestDelegate next, IOptions<ServerTimingOptions> setup) : base(next, setup)
        {
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="ServerTimingMiddleware"/> class.
		/// </summary>
		/// <param name="next">The delegate of the request pipeline to invoke.</param>
		/// <param name="setup">The <see cref="ServerTimingOptions" /> which need to be configured.</param>
		public ServerTimingMiddleware(RequestDelegate next, Action<ServerTimingOptions> setup) : base(next, setup)
		{
		}

        /// <summary>
        /// Executes the <see cref="ServerTimingMiddleware" />.
        /// </summary>
        /// <param name="context">The context of the current request.</param>
        /// <param name="di1">The <see cref="ILogger"/> used in combination with <see cref="ServerTimingOptions.ServerTimingLogLevel"/>.</param>
        /// <param name="di2">The dependency injected <see cref="IHostEnvironment"/> of <see cref="InvokeAsync"/>.</param>
        /// <returns>A task that represents the execution of this middleware.</returns>
        public override Task InvokeAsync(HttpContext context, ILogger<ServerTimingMiddleware> di1, IHostEnvironment di2)
        {
            context.Response.OnStarting(() =>
            {
	            var logger = di1;
	            var environment = di2;
                var serverTiming = context.RequestServices.GetService<IServerTiming>();
                if (serverTiming != null)
                {
	                var serverTimingMetrics = serverTiming.Metrics.ToList();
	                if (!Options.SuppressHeaderPredicate(environment)) { context.Response.Headers.Append(ServerTiming.HeaderName, serverTimingMetrics.Select(metric => metric.ToString()).ToArray()); }
	                if (logger != null && Options.LogLevelSelector != null)
	                {
		                foreach (var metric in serverTimingMetrics)
		                {
                            var logLevel = Options.LogLevelSelector(metric);
							logger.Log(logLevel, "ServerTimingMetric {{ Name: {Name}, Duration: {Duration}ms, Description: \"{Description}\" }}", 
	                            metric.Name,
	                            metric.Duration?.TotalMilliseconds.ToString("F1", CultureInfo.InvariantCulture) ?? 0.ToString("F1"),
	                            metric.Description ?? "N/A");
		                }
	                }
                }
                return Task.CompletedTask;
            });
            return Next(context);
        }
    }
}
