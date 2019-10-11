using System;
using Cuemon.Extensions;
using Cuemon.Integrity;

namespace Cuemon.AspNetCore.Mvc.Filters.Cacheable
{
    /// <summary>
    /// Provides a content and time based object result that is processed by both an HTTP ETag filter- and a Last-Modified filter implementation.
    /// </summary>
    /// <typeparam name="T">The type of the object to make cacheable.</typeparam>
    /// <seealso cref="CacheableObjectResult{T, ContentBasedOptions}" />
    /// <seealso cref="ICacheableIntegrity" />
    /// <seealso cref="HttpLastModifiedHeader"/>
    /// <seealso cref="HttpEntityTagHeader"/>
    /// <seealso cref="HttpCacheableFilter"/>
    public class ContentTimeBasedObjectResult<T> :  CacheableObjectResult<T, ContentTimeBasedOptions>, ICacheableEntity
    {
        internal ContentTimeBasedObjectResult(T instance, DateTime created, byte[] checksum = null, Action<ContentTimeBasedOptions> setup = null) : base(instance, setup)
        {
            var options = setup.Configure();
            Created = created;
            Checksum = new HashResult(checksum);
            Modified = options.Modified;
            Validation = options.GetValidation(checksum);
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
}