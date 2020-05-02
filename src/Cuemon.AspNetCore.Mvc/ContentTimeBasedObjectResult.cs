using System;
using Cuemon.AspNetCore.Mvc.Filters.Cacheable;
using Cuemon.Integrity;

namespace Cuemon.AspNetCore.Mvc
{
    /// <summary>
    /// Provides a content and time based object result that is processed by both an HTTP ETag filter- and a Last-Modified filter implementation.
    /// </summary>
    /// <seealso cref="CacheableObjectResult" />
    /// <seealso cref="ICacheableIntegrity" />
    /// <seealso cref="HttpLastModifiedHeaderFilter"/>
    /// <seealso cref="HttpEntityTagHeaderFilter"/>
    /// <seealso cref="HttpCacheableFilter"/>
    internal class ContentTimeBasedObjectResult :  CacheableObjectResult, ICacheableEntity
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ContentTimeBasedObjectResult"/> class.
        /// </summary>
        /// <param name="instance">The object to make cacheable.</param>
        /// <param name="timestamp">An object implementing the <see cref="ICacheableTimestamp"/> interface.</param>
        /// <param name="integrity">An object implementing the <see cref="ICacheableIntegrity"/> interface.</param>
        internal ContentTimeBasedObjectResult(object instance, ICacheableTimestamp timestamp, ICacheableIntegrity integrity) : base(instance)
        {
            Created = timestamp.Created;
            Checksum = integrity.Checksum;
            Modified = timestamp.Modified;
            Validation = integrity.Validation;
        }

        /// <summary>
        /// Gets a <see cref="DateTime" /> value from when data this instance represents was first created, expressed as the Coordinated Universal Time (UTC).
        /// </summary>
        /// <value>The timestamp from when data this instance represents was first created.</value>
        public DateTime Created { get; set; }

        /// <summary>
        /// Gets a <see cref="DateTime" /> value from when data this instance represents was last modified, expressed as the Coordinated Universal Time (UTC).
        /// </summary>
        /// <value>The timestamp from when data this instance represents was last modified.</value>
        public DateTime? Modified { get; set; }

        /// <summary>
        /// Gets the validation strength of the integrity of this instance.
        /// </summary>
        /// <value>The validation strength of the integrity of this instance.</value>
        public ChecksumStrength Validation { get; set; }

        /// <summary>
        /// Gets a <see cref="HashResult" /> that represents the integrity of this instance.
        /// </summary>
        /// <value>The checksum that represents the integrity of this instance.</value>
        public HashResult Checksum { get; set; }
    }

    /// <summary>
    /// Provides a content and time based object result that is processed by both an HTTP ETag filter- and a Last-Modified filter implementation.
    /// </summary>
    /// <typeparam name="T">The type of the object to make cacheable.</typeparam>
    /// <seealso cref="CacheableObjectResult{T}" />
    /// <seealso cref="ContentTimeBasedObjectResult" />
    internal class ContentTimeBasedObjectResult<T> : ContentTimeBasedObjectResult
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ContentTimeBasedObjectResult{T}"/> class.
        /// </summary>
        /// <param name="instance">The object to make cacheable.</param>
        /// <param name="timestamp">An object implementing the <see cref="ICacheableTimestamp"/> interface.</param>
        /// <param name="integrity">An object implementing the <see cref="ICacheableIntegrity"/> interface.</param>
        internal ContentTimeBasedObjectResult(T instance, ICacheableTimestamp timestamp, ICacheableIntegrity integrity) : base(instance, timestamp, integrity)
        {
        }
    }
}