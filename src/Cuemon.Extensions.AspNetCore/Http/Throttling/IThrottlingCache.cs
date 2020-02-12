using System.Collections.Generic;

namespace Cuemon.Extensions.AspNetCore.Http.Throttling
{
    /// <summary>
    /// Specifies the contract for the storage of a throttling cache.
    /// Implements the <see cref="ThrottleRequest" />.
    /// </summary>
    /// <seealso cref="ThrottleRequest" />
    public interface IThrottlingCache : IDictionary<string, ThrottleRequest>
    {
    }
}