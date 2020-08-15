using System;
using System.Globalization;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Cuemon.AspNetCore.Http.Headers;
using Cuemon.AspNetCore.Http.Throttling;
using Cuemon.Collections.Generic;
using Microsoft.AspNetCore.Http;
using Microsoft.Net.Http.Headers;

namespace Cuemon.AspNetCore.Infrastructure
{
    internal static class AspNetCoreInfrastructure
    {
        private static readonly SemaphoreSlim ThrottleLocker = new SemaphoreSlim(1);

        public static async Task InvokeUserAgentSentinelAsync(HttpContext context, UserAgentSentinelOptions options, Action<HttpResponseMessage, HttpResponse> transformer)
        {
            var userAgent = context.Request.Headers[HeaderNames.UserAgent].FirstOrDefault();
            if (options.RequireUserAgentHeader)
            {
                var message = options.ResponseBroker?.Invoke(userAgent);
                if (message != null)
                {
                    context.Response.OnStarting(() =>
                    {
                        transformer?.Invoke(message, context.Response);
                        return Task.CompletedTask;
                    });
                    throw new UserAgentException((int)message.StatusCode, await message.Content.ReadAsStringAsync().ConfigureAwait(false));
                }
            }
        }

        public static async Task InvokeThrottlerSentinelAsync(HttpContext context, IThrottlingCache tc, ThrottlingSentinelOptions options, Action<HttpResponseMessage, HttpResponse> transformer)
        {
            var utcNow = DateTime.UtcNow;
            var throttlingContext = options.ContextResolver?.Invoke(context);
            if (!string.IsNullOrWhiteSpace(throttlingContext))
            {
                try
                {
                    await ThrottleLocker.WaitAsync().ConfigureAwait(false);

                    if (!tc.TryGetValue(throttlingContext, out var tr))
                    {
                        tr = new ThrottleRequest(options.Quota);
                        Decorator.Enclose(tc).TryAdd(throttlingContext, tr);
                    }
                    else
                    {
                        tr.Refresh();
                        tr.IncrementTotal();
                    }

                    var window = new TimeRange(utcNow, tr.Expires);
                    var delta = window.Duration;
                    var reset = utcNow.Add(delta);
                    Decorator.Enclose(context.Response.Headers).TryAddOrUpdate(options.RateLimitHeaderName, tr.Quota.RateLimit.ToString(CultureInfo.InvariantCulture));
                    Decorator.Enclose(context.Response.Headers).TryAddOrUpdate(options.RateLimitRemainingHeaderName, Math.Max(tr.Quota.RateLimit - tr.Total, 0).ToString(CultureInfo.InvariantCulture));
                    Decorator.Enclose(context.Response.Headers).TryAddOrUpdate(options.RateLimitResetHeaderName, Decorator.Enclose(reset).ToUnixEpochTime().ToString(CultureInfo.InvariantCulture));
                    if (tr.Total > tr.Quota.RateLimit && tr.Expires > utcNow)
                    {
                        var message = options.ResponseBroker?.Invoke(delta, reset);
                        if (message != null)
                        {
                            context.Response.OnStarting(() =>
                            {
                                transformer?.Invoke(message, context.Response);
                                return Task.CompletedTask;
                            });
                            throw new ThrottlingException((int)message.StatusCode, await message.Content.ReadAsStringAsync().ConfigureAwait(false), tr.Quota.RateLimit, delta, reset);
                        }
                    }

                    tc[throttlingContext] = tr;
                }
                finally
                {
                    ThrottleLocker.Release();
                }
            }
        }
    }
}