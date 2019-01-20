using System;
using System.Globalization;
using System.Threading;
using System.Threading.Tasks;
using Cuemon.Collections.Generic;
using Cuemon.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using Microsoft.Net.Http.Headers;

namespace Cuemon.AspNetCore.Http.Throttling
{
    /// <summary>
    /// Provides an API throttling middleware implementation for ASP.NET Core.
    /// </summary>
    public class ThrottlingMiddleware : ConfigurableMiddleware<IThrottlingCache, ThrottlingOptions>
    {
        private readonly SemaphoreSlim _locker = new SemaphoreSlim(1);

        /// <summary>
        /// Initializes a new instance of the <see cref="ThrottlingMiddleware"/> class.
        /// </summary>
        /// <param name="next">The delegate of the request pipeline to invoke.</param>
        /// <param name="setup">The <see cref="ThrottlingOptions" /> which need to be configured.</param>
        public ThrottlingMiddleware(RequestDelegate next, IOptions<ThrottlingOptions> setup) : base(next, setup)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ThrottlingMiddleware"/> class.
        /// </summary>
        /// <param name="next">The delegate of the request pipeline to invoke.</param>
        /// <param name="setup">The <see cref="ThrottlingOptions" /> which need to be configured.</param>
        public ThrottlingMiddleware(RequestDelegate next, Action<ThrottlingOptions> setup) : base(next, setup)
        {
        }

        /// <summary>
        /// Executes the <see cref="ThrottlingMiddleware" />.
        /// </summary>
        /// <param name="context">The context of the current request.</param>
        /// <param name="tc">The dependency injected <see cref="IThrottlingCache"/> of <see cref="InvokeAsync"/>.</param>
        /// <returns>A task that represents the execution of this middleware.</returns>
        public override async Task InvokeAsync(HttpContext context, IThrottlingCache tc)
        {
            var utcNow = DateTime.UtcNow;
            var throttlingContext = Options.ContextResolver?.Invoke(context);
            if (!throttlingContext.IsNullOrWhiteSpace())
            {
                await _locker.WaitAsync();
                try
                {
                    if (!tc.TryGetValue(throttlingContext, out var tr))
                    {
                        tr = new ThrottleRequest(Options.Quota);
                        tc.AddIfNotContainsKey(throttlingContext, tr);
                    }
                    else
                    {
                        tr.Refresh();
                        tr.Total++;
                    }

                    var window = new TimeRange(utcNow, tr.Expires);
                    var delta = window.Duration;
                    var reset = utcNow.Add(delta);
                    context.Response.Headers.AddOrUpdate(Options.RateLimitHeaderName, tr.Quota.RateLimit.ToString(CultureInfo.InvariantCulture));
                    context.Response.Headers.AddOrUpdate(Options.RateLimitRemainingHeaderName, Math.Max(tr.Quota.RateLimit - tr.Total, 0).ToString(CultureInfo.InvariantCulture));
                    context.Response.Headers.AddOrUpdate(Options.RateLimitResetHeaderName, reset.ToEpochTime().ToString(CultureInfo.InvariantCulture));
                    if (tr.Total > tr.Quota.RateLimit && tr.Expires > utcNow)
                    {
                        context.Response.StatusCode = StatusCodes.Status429TooManyRequests;
                        if (Options.UseRetryAfterHeader && Options.RetryAfterResolver != null) { context.Response.Headers.AddOrUpdate(HeaderNames.RetryAfter, Options.RetryAfterResolver.Invoke(reset, delta).ToString()); }
                        await context.Response.WriteBodyAsync(Options.TooManyRequestsBody);
                        return;
                    }

                    tc[throttlingContext] = tr;
                }
                finally
                {
                    _locker.Release();
                }
            }
            await Next(context).ContinueWithSuppressedContext();
        }
    }
}