using System;
using System.Net.Http.Headers;
using System.Text;
using Microsoft.AspNetCore.Http;

namespace Cuemon.AspNetCore.Http.Throttling
{
    /// <summary>
    /// Configuration options for <see cref="ThrottlingMiddleware"/>.
    /// </summary>
    public class ThrottlingOptions
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ThrottlingOptions"/> class.
        /// </summary>
        /// <remarks>
        /// The following table shows the initial property values for an instance of <see cref="ThrottlingOptions"/>.
        /// <list type="table">
        ///     <listheader>
        ///         <term>Property</term>
        ///         <description>Initial Value</description>
        ///     </listheader>
        ///     <item>
        ///         <term><see cref="RateLimitHeaderName"/></term>
        ///         <description><c>X-RateLimit-Limit</c></description>
        ///     </item>
        ///     <item>
        ///         <term><see cref="RateLimitRemainingHeaderName"/></term>
        ///         <description><c>X-RateLimit-Remaining</c></description>
        ///     </item>
        ///     <item>
        ///         <term><see cref="RateLimitResetHeaderName"/></term>
        ///         <description><c>X-RateLimit-Reset</c></description>
        ///     </item>
        ///     <item>
        ///         <term><see cref="UseRetryAfterHeader"/></term>
        ///         <description><c>true</c></description>
        ///     </item>
        ///     <item>
        ///         <term><see cref="RetryAfterResolver"/></term>
        ///         <description><c>(dt, ts) => new RetryConditionHeaderValue(ts);</c></description>
        ///     </item>
        ///     <item>
        ///         <term><see cref="TooManyRequestsBody"/></term>
        ///         <description><c>() =>
        /// {
        /// return "Throttling rate limit quota violation. Quota limit exceeded.".ToByteArray(o =&gt;
        /// {
        /// o.Encoding = Encoding.UTF8;
        /// o.Preamble = PreambleSequence.Remove;
        /// });
        /// };</c></description>
        ///     </item>
        ///     <item>
        ///         <term><see cref="ContextResolver"/></term>
        ///         <description><c>null</c></description>
        ///     </item>
        ///     <item>
        ///         <term><see cref="Quota"/></term>
        ///         <description><c>null</c></description>
        ///     </item>
        /// </list>
        /// </remarks>
        public ThrottlingOptions()
        {
            RateLimitHeaderName = "X-RateLimit-Limit";
            RateLimitRemainingHeaderName = "X-RateLimit-Remaining";
            RateLimitResetHeaderName = "X-RateLimit-Reset";
            UseRetryAfterHeader = true;
            RetryAfterResolver = (dt, ts) => new RetryConditionHeaderValue(ts);
            TooManyRequestsBody =  () =>
            {
                return "Throttling rate limit quota violation. Quota limit exceeded.".ToByteArray(o =>
                {
                    o.Encoding = Encoding.UTF8;
                    o.Preamble = PreambleSequence.Remove;
                });
            };
        }

        /// <summary>
        /// Gets or sets the function delegate that will resolve a unique context of the throttling middleware (eg. IP-address, Authorization header, etc.).
        /// </summary>
        /// <value>The function delegate that will resolve a unique context of the throttling middleware.</value>
        public Func<HttpContext, string> ContextResolver { get; set; }

        /// <summary>
        /// Gets or sets the allowed quota for a given context.
        /// </summary>
        /// <value>The allowed quota for a given context.</value>
        public ThrottleQuota Quota { get; set; }

        /// <summary>
        /// Gets or sets the function delegate that retrieves the body of a throttled request that has exceeded the rate limit.
        /// </summary>
        /// <value>A <see cref="Func{TResult}"/> that retrieves the body of a throttled request that has exceeded the rate limit.</value>
        public Func<byte[]> TooManyRequestsBody { get; set; }

        /// <summary>
        /// Gets or sets the name of the rate limit HTTP header.
        /// </summary>
        /// <value>The name of the rate limit HTTP header.</value>
        public string RateLimitHeaderName { get; set; }

        /// <summary>
        /// Gets or sets the name of the rate limit remaining HTTP header.
        /// </summary>
        /// <value>The name of the rate limit remaining HTTP header.</value>
        public string RateLimitRemainingHeaderName { get; set; }

        /// <summary>
        /// Gets or sets the name of the rate limit reset HTTP header.
        /// </summary>
        /// <value>The name of the rate limit reset HTTP header.</value>
        public string RateLimitResetHeaderName { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether to include a Retry-After HTTP header indicating how long to wait before making a new request.
        /// </summary>
        /// <value><c>true</c> to include a Retry-After HTTP header indicating how long to wait before making a new request; otherwise, <c>false</c>.</value>
        public bool UseRetryAfterHeader { get; set; }

        /// <summary>
        /// Gets or sets the function delegate that will resolve a <see cref="RetryConditionHeaderValue"/> that conforms with RFC 2616.
        /// </summary>
        /// <value>The function delegate that will resolve a <see cref="RetryConditionHeaderValue"/> that conforms with RFC 2616.</value>
        public Func<DateTime, TimeSpan, RetryConditionHeaderValue> RetryAfterResolver { get; set; }
    }
}