using System.Collections.Concurrent;
using Cuemon.AspNetCore.Http.Throttling;

namespace Cuemon.Extensions.AspNetCore.Http.Throttling
{
    /// <summary>
    /// Provides a simple in-memory representation of the <see cref="IThrottlingCache"/>. This class cannot be inherited.
    /// </summary>
    /// <seealso cref="ThrottleRequest" />
    /// <seealso cref="IThrottlingCache" />
    public sealed class MemoryThrottlingCache : ConcurrentDictionary<string, ThrottleRequest>, IThrottlingCache
    {
    }
}