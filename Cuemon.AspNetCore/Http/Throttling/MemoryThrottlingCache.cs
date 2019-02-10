using System.Collections.Concurrent;

namespace Cuemon.AspNetCore.Http.Throttling
{
    /// <summary>
    /// Provides a simple in-memory representation of the <see cref="IThrottlingCache"/>. This class cannot be inherited.
    /// Implements the <see cref="ThrottleRequest" />.
    /// Implements the <see cref="IThrottlingCache" />.
    /// </summary>
    /// <seealso cref="ThrottleRequest" />
    /// <seealso cref="IThrottlingCache" />
    /// <seealso cref="ThrottlingSentinelMiddleware" />
    public sealed class MemoryThrottlingCache : ConcurrentDictionary<string, ThrottleRequest>, IThrottlingCache
    {
    }
}