namespace Cuemon.Runtime.Caching
{
    /// <summary>
    /// Provides access to caching in an application.
    /// </summary>
    public static class CachingManager
    {
        /// <summary>
        /// Gets a collection of cached objects for the current application domain.
        /// </summary>
        /// <value>A collection of cached objects for the current application domain.</value>
        public static CacheCollection Cache
        {
            get { return CacheCollection.Cache; }
        }
    }
}