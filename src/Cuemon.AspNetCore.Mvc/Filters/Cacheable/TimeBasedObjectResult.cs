using System;
using Cuemon.Integrity;

namespace Cuemon.AspNetCore.Mvc.Filters.Cacheable
{
    /// <summary>
    /// Provides a time based object result that is processed by a Last-Modified filter implementation.
    /// </summary>
    /// <typeparam name="T">The type of the object to make cacheable.</typeparam>
    /// <seealso cref="CacheableObjectResult{T, ContentBasedOptions}" />
    /// <seealso cref="ICacheableIntegrity" />
    /// <seealso cref="HttpLastModifiedHeader"/>
    /// <seealso cref="HttpCacheableFilter"/>
    public class TimeBasedObjectResult<T> : CacheableObjectResult<T, TimeBasedOptions>, ICacheableTimestamp
    {
        internal TimeBasedObjectResult(T instance, DateTime created, Action<TimeBasedOptions> setup = null) : base(instance, setup)
        {
            Created = created;
            Modified = Options.Modified;
        }

        /// <summary>
        /// Gets a <see cref="DateTime" /> value from when data this instance represents was first created, expressed as the Coordinated Universal Time (UTC).
        /// </summary>
        /// <value>The timestamp from when data this instance represents was first created.</value>
        public DateTime Created { get; }

        /// <summary>
        /// Gets a <see cref="DateTime" /> value from when data this instance represents was last modified, expressed as the Coordinated Universal Time (UTC).
        /// </summary>
        /// <value>The timestamp from when data this instance represents was last modified.</value>
        public DateTime? Modified { get; }
    }
}