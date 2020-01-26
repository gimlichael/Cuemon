using System;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using Microsoft.AspNetCore.Http;
using Microsoft.Net.Http.Headers;

namespace Cuemon.AspNetCore.Http.Throttling
{
    /// <summary>
    /// Configuration options for <see cref="ThrottlingSentinelMiddleware"/>.
    /// </summary>
    public class ThrottlingSentinelOptions
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ThrottlingSentinelOptions"/> class.
        /// </summary>
        /// <remarks>
        /// The following table shows the initial property values for an instance of <see cref="ThrottlingSentinelOptions"/>.
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
        ///         <term><see cref="RetryAfterHeader"/></term>
        ///         <description><c>ThrottlingRetryAfterHeader.DeltaSeconds</c></description>
        ///     </item>
        ///     <item>
        ///         <term><see cref="TooManyRequestsMessage"/></term>
        ///         <description>Throttling rate limit quota violation. Quota limit exceeded.</description>
        ///     </item>
        ///     <item>
        ///         <term><see cref="ContextResolver"/></term>
        ///         <description><c>null</c></description>
        ///     </item>
        ///     <item>
        ///         <term><see cref="Quota"/></term>
        ///         <description><c>null</c></description>
        ///     </item>
        ///     <item>
        ///         <term><see cref="ResponseBroker"/></term>
        ///         <description>A <see cref="HttpResponseMessage"/> initialized to a HTTP status code 429 with zero of one Retry-After header and a body of <see cref="TooManyRequestsMessage"/>.</description>
        ///     </item>
        /// </list>
        /// </remarks>
        public ThrottlingSentinelOptions()
        {
            RateLimitHeaderName = "X-RateLimit-Limit";
            RateLimitRemainingHeaderName = "X-RateLimit-Remaining";
            RateLimitResetHeaderName = "X-RateLimit-Reset";
            UseRetryAfterHeader = true;
            RetryAfterHeader = ThrottlingRetryAfterHeader.DeltaSeconds;
            TooManyRequestsMessage = "Throttling rate limit quota violation. Quota limit exceeded.";
            ResponseBroker = (delta, reset) =>
            {
                var message = new HttpResponseMessage((HttpStatusCode) StatusCodes.Status429TooManyRequests);
                if (UseRetryAfterHeader)
                {
                    switch (RetryAfterHeader)
                    {
                        case ThrottlingRetryAfterHeader.DeltaSeconds:
                            message.Headers.Add(HeaderNames.RetryAfter, new RetryConditionHeaderValue(delta).ToString());
                            break;
                        case ThrottlingRetryAfterHeader.HttpDate:
                            message.Headers.Add(HeaderNames.RetryAfter, new RetryConditionHeaderValue(reset).ToString());
                            break;
                    }
                }
                message.Content = new StringContent(TooManyRequestsMessage);
                return message;
            };
        }

        /// <summary>
        /// Gets or sets the function delegate that configures the response in the form of a <see cref="HttpResponseMessage"/>.
        /// </summary>
        /// <value>The function delegate that configures the response in the form of a <see cref="HttpResponseMessage"/>.</value>
        public Func<TimeSpan, DateTime, HttpResponseMessage> ResponseBroker { get; set; }

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
        /// Gets or sets the message of a throttled request that has exceeded the rate limit.
        /// </summary>
        /// <value>The message of a throttled request that has exceeded the rate limit.</value>
        public string TooManyRequestsMessage { get; set; }

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
        /// Gets or sets a value indicating whether to include a Retry-After HTTP header specifying how long to wait before making a new request.
        /// </summary>
        /// <value><c>true</c> to include a Retry-After HTTP header specifying how long to wait before making a new request; otherwise, <c>false</c>.</value>
        public bool UseRetryAfterHeader { get; set; }

        /// <summary>
        /// Gets or sets the preferred Retry-After HTTP header value that conforms with RFC 2616.
        /// </summary>
        /// <value>The preferred Retry-After HTTP header value that conforms with RFC 2616.</value>
        public ThrottlingRetryAfterHeader RetryAfterHeader { get; set; }
    }
}