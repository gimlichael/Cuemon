namespace Cuemon.AspNetCore.Http.Throttling
{
    /// <summary>
    /// Specifies a set of values defining what value to use with a HTTP Retry-After header.
    /// </summary>
    public enum ThrottlingRetryAfterHeader
    {
        /// <summary>
        /// A non-negative decimal integer indicating the seconds to delay after the response is received.
        /// </summary>
        DeltaSeconds,
        /// <summary>
        /// A date after which to retry.
        /// </summary>
        HttpDate
    }
}