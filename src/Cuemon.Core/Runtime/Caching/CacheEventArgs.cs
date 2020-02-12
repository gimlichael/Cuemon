using System;

namespace Cuemon.Runtime.Caching
{
    /// <summary>
    /// Provides data for cache related operations. This class cannot be inherited.
    /// </summary>
    public sealed class CacheEventArgs : EventArgs
    {
        internal CacheEventArgs(Cache cache)
        {
            Cache = cache;
        }

        internal Cache Cache { get; private set; }
    }
}