using System;
using System.Globalization;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Cuemon.AspNetCore.Http;
using Cuemon.AspNetCore.Http.Headers;
using Cuemon.AspNetCore.Http.Throttling;
using Cuemon.Extensions;
using Cuemon.Extensions.Collections.Generic;
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
                    message.ToHttpResponse(context.Response, transformer);
                    throw new UserAgentException((int)message.StatusCode, await message.Content.ReadAsStringAsync());
                }
            }
        }

        public static async Task InvokeThrottlerSentinelAsync(HttpContext context, IThrottlingCache tc, ThrottlingSentinelOptions options, Action<HttpResponseMessage, HttpResponse> transformer)
        {
            var utcNow = DateTime.UtcNow;
            var throttlingContext = options.ContextResolver?.Invoke(context);
            if (!throttlingContext.IsNullOrWhiteSpace())
            {
                try
                {
                    await ThrottleLocker.WaitAsync();

                    if (!tc.TryGetValue(throttlingContext, out var tr))
                    {
                        tr = new ThrottleRequest(options.Quota);
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
                    context.Response.Headers.AddOrUpdate(options.RateLimitHeaderName, tr.Quota.RateLimit.ToString(CultureInfo.InvariantCulture));
                    context.Response.Headers.AddOrUpdate(options.RateLimitRemainingHeaderName, Math.Max(tr.Quota.RateLimit - tr.Total, 0).ToString(CultureInfo.InvariantCulture));
                    context.Response.Headers.AddOrUpdate(options.RateLimitResetHeaderName, reset.ToUnixEpochTime().ToString(CultureInfo.InvariantCulture));
                    if (tr.Total > tr.Quota.RateLimit && tr.Expires > utcNow)
                    {
                        var message = options.ResponseBroker?.Invoke(delta, reset);
                        if (message != null)
                        {
                            message.ToHttpResponse(context.Response, transformer);
                            throw new ThrottlingException((int)message.StatusCode, await message.Content.ReadAsStringAsync(), tr.Quota.RateLimit, delta, reset);
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