using System;
using Cuemon.AspNetCore.Mvc.Filters.Cacheable;
using Cuemon.Integrity;

namespace Cuemon.AspNetCore.Mvc
{
    /// <summary>
    /// Provides a timestamp based object result that is processed by a Last-Modified filter implementation.
    /// </summary>
    /// <seealso cref="CacheableObjectResult" />
    /// <seealso cref="ICacheableTimestamp" />
    /// <seealso cref="HttpLastModifiedHeaderFilter"/>
    /// <seealso cref="HttpCacheableFilter"/>
    internal class TimeBasedObjectResult : CacheableObjectResult, ICacheableTimestamp
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="TimeBasedObjectResult{T}"/> class.
        /// </summary>
        /// <param name="instance">The object to make cacheable.</param>
        /// <param name="created">The created date-time value of an object.</param>
        /// <param name="modified">The modified date-time value of an object.</param>
        internal TimeBasedObjectResult(object instance, DateTime created, DateTime? modified) : base(instance)
        {
            Created = created;
            Modified = modified;
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

    /// <summary>
    /// Provides a time based object result that is processed by a Last-Modified filter implementation.
    /// </summary>
    /// <typeparam name="T">The type of the object to make cacheable.</typeparam>
    /// <seealso cref="TimeBasedObjectResult" />
    internal class TimeBasedObjectResult<T> : TimeBasedObjectResult
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="TimeBasedObjectResult{T}"/> class.
        /// </summary>
        /// <param name="instance">The object to make cacheable.</param>
        /// <param name="created">The created date-time value of an object.</param>
        /// <param name="modified">The modified date-time value of an object.</param>
        internal TimeBasedObjectResult(T instance, DateTime created, DateTime? modified) : base(instance, created, modified)
        {
        }
    }
}