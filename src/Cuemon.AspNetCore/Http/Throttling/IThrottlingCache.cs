using System.Collections.Generic;

namespace Cuemon.AspNetCore.Http.Throttling
{
    /// <summary>
    /// Specifies the contract for the storage used with <see cref="ThrottlingSentinelMiddleware"/>.
    /// Implements the <see cref="ThrottleRequest" />.
    /// </summary>
    /// <seealso cref="ThrottleRequest" />
    /// <seealso cref="ThrottlingSentinelMiddleware" />
    public interface IThrottlingCache : IDictionary<string, ThrottleRequest>
    {
    }
}