using System;

namespace Cuemon.AspNetCore.Http.Throttling
{
    /// <summary>
    /// Represents the request usage and quota in the context of throttling.
    /// </summary>
    public class ThrottleRequest
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ThrottleRequest"/> class.
        /// </summary>
        /// <param name="quota">The allowed quota of HTTP requests.</param>
        public ThrottleRequest(ThrottleQuota quota)
        {
            Validator.ThrowIfNull(quota, nameof(quota));
            Quota = quota;
            Total = 1;
            Expires = DateTime.UtcNow.Add(quota.Window);
        }

        /// <summary>
        /// Gets the total amount of HTTP requests.
        /// </summary>
        /// <value>The total amount of HTTP requests.</value>
        public int Total { get; internal set; }

        /// <summary>
        /// Gets the computed expiration value of a throttled rate limit.
        /// </summary>
        /// <value>The computed expiration value of throttled rate limit.</value>
        /// <remarks>This property is measured in Coordinated Universal Time (UTC) (also known as Greenwich Mean Time).</remarks>
        public DateTime Expires { get; private set; }

        /// <summary>
        /// Gets the throttling quota that defines the rate limit of HTTP requests.
        /// </summary>
        /// <value>The throttling quota that defines the rate limit of HTTP requests.</value>
        public ThrottleQuota Quota { get; }

        internal void Refresh()
        {
            var utcNow = DateTime.UtcNow;
            if (utcNow > Expires)
            {
                Expires = utcNow.Add(Quota.Window);
                Total = 0;
            }
        }
    }
}