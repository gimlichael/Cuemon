using System.Collections.Generic;

namespace Cuemon.AspNetCore.Http.Throttling
{
    /// <summary>
    /// Specifies the contract for the storage used with <see cref="ThrottlingMiddleware"/>.
    /// Implements the <see cref="ThrottleRequest" />.
    /// </summary>
    /// <seealso cref="ThrottleRequest" />
    /// <seealso cref="ThrottlingMiddleware" />
    public interface IThrottlingCache : IDictionary<string, ThrottleRequest>
    {
    }
}