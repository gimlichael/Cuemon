using System;
using Cuemon.Integrity;

namespace Cuemon.Extensions.AspNetCore.Mvc.Filters.Cacheable
{
    /// <summary>
    /// Provides a content based object result that is processed by an HTTP ETag filter implementation.
    /// </summary>
    /// <typeparam name="T">The type of the object to make cacheable.</typeparam>
    /// <seealso cref="CacheableObjectResult{T, ContentBasedOptions}" />
    /// <seealso cref="ICacheableIntegrity" />
    /// <seealso cref="HttpEntityTagHeader"/>
    /// <seealso cref="HttpCacheableFilter"/>
    public class ContentBasedObjectResult<T> : CacheableObjectResult<T, ContentBasedOptions>, ICacheableIntegrity
    {
        internal ContentBasedObjectResult(T instance, byte[] checksum, Action<ContentBasedOptions> setup = null) : base(instance, setup)
        {
            Checksum = new HashResult(checksum);
            Validation = Options.GetValidation(checksum);
        }

        /// <summary>
        /// Gets the validation strength of the integrity of this instance.
        /// </summary>
        /// <value>The validation strength of the integrity of this instance.</value>
        public ChecksumStrength Validation { get; }

        /// <summary>
        /// Gets a <see cref="HashResult" /> that represents the integrity of this instance.
        /// </summary>
        /// <value>The checksum that represents the integrity of this instance.</value>
        public HashResult Checksum { get; }
    }
}