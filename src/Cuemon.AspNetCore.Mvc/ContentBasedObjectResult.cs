using Cuemon.AspNetCore.Mvc.Filters.Cacheable;
using Cuemon.Integrity;

namespace Cuemon.AspNetCore.Mvc
{
    /// <summary>
    /// Provides an integrity based object result that is processed by an HTTP ETag filter implementation.
    /// </summary>
    /// <seealso cref="CacheableObjectResult" />
    /// <seealso cref="ICacheableIntegrity" />
    /// <seealso cref="HttpEntityTagHeaderFilter"/>
    /// <seealso cref="HttpCacheableFilter"/>
    internal class ContentBasedObjectResult : CacheableObjectResult, ICacheableIntegrity
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ContentBasedObjectResult"/> class.
        /// </summary>
        /// <param name="instance">The object to make cacheable.</param>
        /// <param name="checksum">The checksum of an object.</param>
        /// <param name="isWeak">A value hinting whether the specified <paramref name="checksum"/> resembles a weak or a strong <see cref="Validation"/>.</param>
        internal ContentBasedObjectResult(object instance, byte[] checksum, bool isWeak = false) : base(instance)
        {
            Checksum = new HashResult(checksum);
            Validation = checksum == null || checksum.Length == 0 
                ? ChecksumStrength.None 
                : isWeak
                    ? ChecksumStrength.Weak
                    : ChecksumStrength.Strong;
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

    /// <summary>
    /// Provides a content based object result that is processed by an HTTP ETag filter implementation.
    /// </summary>
    /// <typeparam name="T">The type of the object to make cacheable.</typeparam>
    /// <seealso cref="ContentBasedObjectResult" />
    internal class ContentBasedObjectResult<T> : ContentBasedObjectResult
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ContentBasedObjectResult{T}"/> class.
        /// </summary>
        /// <param name="instance">The object to make cacheable.</param>
        /// <param name="checksum">The checksum of an object.</param>
        /// <param name="isWeak">The value that indicates whether this instance has a weak validation.</param>
        internal ContentBasedObjectResult(T instance, byte[] checksum, bool isWeak = false) : base(instance, checksum, isWeak)
        {
        }
    }
}