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

namespace Cuemon.AspNetCore.Http
{
    /// <summary>
    /// Extension methods for the <see cref="HttpContext"/> class tailored to adhere the decorator pattern.
    /// This API supports the product infrastructure and is not intended to be used directly from your code.
    /// </summary>
    /// <seealso cref="IDecorator{T}"/>
    /// <seealso cref="Decorator{T}"/>
    public static class HttpContextDecoratorExtensions
    {
        private static readonly SemaphoreSlim ThrottleLocker = new SemaphoreSlim(1);

        /// <summary>
        /// Common throttler operation logic for ASP.NET Core and ASP.NET Core MVC. Not intended to be used directly from your code.
        /// </summary>
        /// <param name="decorator">The <see cref="IDecorator{HttpContext}"/> to extend.</param>
        /// <param name="tc">The <see cref="IThrottlingCache"/> implementation.</param>
        /// <param name="options">The configured options.</param>
        /// <param name="transformer">The delegate that merges an instance of <see cref="HttpResponseMessage"/> into the <see cref="HttpResponse"/> pipeline.</param>
        public static async Task InvokeThrottlerSentinelAsync(this IDecorator<HttpContext> decorator, IThrottlingCache tc, ThrottlingSentinelOptions options, Action<HttpResponseMessage, HttpResponse> transformer)
        {
            var utcNow = DateTime.UtcNow;
            var throttlingContext = options.ContextResolver?.Invoke(decorator.Inner);
            if (!string.IsNullOrWhiteSpace(throttlingContext))
            {
                ThrottleRequest tr = null;
                try
                {
                    await ThrottleLocker.WaitAsync().ConfigureAwait(false);

                    if (!tc.TryGetValue(throttlingContext, out tr))
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
                    Decorator.Enclose(decorator.Inner.Response.Headers).AddOrUpdate(options.RateLimitHeaderName, tr.Quota.RateLimit.ToString(CultureInfo.InvariantCulture));
                    Decorator.Enclose(decorator.Inner.Response.Headers).AddOrUpdate(options.RateLimitRemainingHeaderName, Math.Max(tr.Quota.RateLimit - tr.Total, 0).ToString(CultureInfo.InvariantCulture));
                    Decorator.Enclose(decorator.Inner.Response.Headers).AddOrUpdate(options.RateLimitResetHeaderName, Decorator.Enclose(reset).ToUnixEpochTime().ToString(CultureInfo.InvariantCulture));
                    if (tr.Total > tr.Quota.RateLimit && tr.Expires > utcNow)
                    {
                        var message = options.ResponseBroker?.Invoke(delta, reset);
                        if (message != null)
                        {
                            transformer?.Invoke(message, decorator.Inner.Response);
                            throw new ThrottlingException((int)message.StatusCode, await message.Content.ReadAsStringAsync().ConfigureAwait(false), tr.Quota.RateLimit, delta, reset);
                        }
                    }
                }
                finally
                {
                    tc[throttlingContext] = tr;
                    ThrottleLocker.Release();
                }
            }
        }

        /// <summary>
        /// Common user agent logic for ASP.NET Core and ASP.NET Core MVC. Not intended to be used directly from your code.
        /// </summary>
        /// <param name="decorator">The <see cref="IDecorator{HttpContext}"/> to extend.</param>
        /// <param name="options">The configured options.</param>
        /// <param name="transformer">The delegate that merges an instance of <see cref="HttpResponseMessage"/> into the <see cref="HttpResponse"/> pipeline.</param>
        public static async Task InvokeUserAgentSentinelAsync(this IDecorator<HttpContext> decorator, UserAgentSentinelOptions options, Action<HttpResponseMessage, HttpResponse> transformer)
        {
            var userAgent = decorator.Inner.Request.Headers[HeaderNames.UserAgent].FirstOrDefault();
            if (options.RequireUserAgentHeader)
            {
                var message = options.ResponseBroker?.Invoke(userAgent);
                if (message != null)
                {
                    transformer?.Invoke(message, decorator.Inner.Response);
                    throw new UserAgentException((int)message.StatusCode, await message.Content.ReadAsStringAsync().ConfigureAwait(false));
                }
            }
        }
    }
}