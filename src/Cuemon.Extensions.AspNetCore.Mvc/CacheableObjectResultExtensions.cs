using System;
using Cuemon.AspNetCore.Mvc;
using Cuemon.AspNetCore.Mvc.Filters.Cacheable;
using Cuemon.Data.Integrity;

namespace Cuemon.Extensions.AspNetCore.Mvc
{
    /// <summary>
    /// Extension methods for the <see cref="ICacheableObjectResult"/> interface.
    /// </summary>
    public static class CacheableObjectResultExtensions
    {
        /// <summary>
        /// Encapsulates the specified <paramref name="instance" /> within a timestamp based object that is processed by a Last-Modified filter implementation.
        /// </summary>
        /// <param name="instance">The instance to make cacheable.</param>
        /// <param name="timestampProvider">The function delegate that resolves a timestamp from when the specified <paramref name="instance"/> was first created, expressed as the Coordinated Universal Time (UTC).</param>
        /// <param name="changedTimestampProvider">The function delegate that resolves a timestamp from when the specified <paramref name="instance"/> was last modified, expressed as the Coordinated Universal Time (UTC).</param>
        /// <returns>An <see cref="ICacheableObjectResult" /> implementation.</returns>
        /// <seealso cref="IEntityDataTimestamp" />
        /// <seealso cref="CacheableObjectResult" />
        /// <seealso cref="HttpLastModifiedHeaderFilter"/>
        /// <seealso cref="HttpCacheableFilter"/>
        public static ICacheableObjectResult MakeCacheable(this object instance, Func<DateTime> timestampProvider, Func<DateTime> changedTimestampProvider = null)
        {
            return CacheableObjectFactory.CreateCacheableObjectResult(instance, timestampProvider, changedTimestampProvider);
        }

        /// <summary>
        /// Encapsulates the specified <paramref name="instance" /> within an integrity based object that is processed by an HTTP ETag filter implementation.
        /// </summary>
        /// <param name="instance">The instance to make cacheable.</param>
        /// <param name="checksumProvider">The function delegate that resolves a checksum defining the data integrity of the specified <paramref name="instance"/>.</param>
        /// <param name="weakChecksumProvider">The function delegate that resolves a value hinting whether the specified <paramref name="checksumProvider"/> resembles a weak or a strong checksum strength.</param>
        /// <returns>An <see cref="ICacheableObjectResult" /> implementation.</returns>
        /// <seealso cref="IEntityDataIntegrity" />
        /// <seealso cref="CacheableObjectResult" />
        /// <seealso cref="HttpEntityTagHeaderFilter"/>
        /// <seealso cref="HttpCacheableFilter"/>
        public static ICacheableObjectResult MakeCacheable(this object instance, Func<byte[]> checksumProvider, Func<bool> weakChecksumProvider = null)
        {
            return CacheableObjectFactory.CreateCacheableObjectResult(instance, checksumProvider, weakChecksumProvider);
        }

        /// <summary>
        /// Encapsulates the specified <paramref name="instance" /> within a timestamp and integrity based object that is processed by both HTTP Last-Modified and HTTP ETag filters implementation.
        /// </summary>
        /// <param name="instance">The instance to make cacheable.</param>
        /// <param name="timestampProvider">The function delegate that resolves a timestamp from when the specified <paramref name="instance"/> was first created, expressed as the Coordinated Universal Time (UTC).</param>
        /// <param name="checksumProvider">The function delegate that resolves a checksum defining the data integrity of the specified <paramref name="instance"/>.</param>
        /// <param name="changedTimestampProvider">The function delegate that resolves a timestamp from when the specified <paramref name="instance"/> was last modified, expressed as the Coordinated Universal Time (UTC).</param>
        /// <param name="weakChecksumProvider">The function delegate that resolves a value hinting whether the specified <paramref name="checksumProvider"/> resembles a weak or a strong checksum strength.</param>
        /// <returns>An <see cref="ICacheableObjectResult" /> implementation.</returns>
        /// <seealso cref="IEntityDataTimestamp" />
        /// <seealso cref="IEntityDataIntegrity" />
        /// <seealso cref="IEntityInfo" />
        /// <seealso cref="CacheableObjectResult" />
        /// <seealso cref="HttpLastModifiedHeaderFilter"/>
        /// <seealso cref="HttpEntityTagHeaderFilter"/>
        /// <seealso cref="HttpCacheableFilter"/>
        public static ICacheableObjectResult MakeCacheable(this object instance, Func<DateTime> timestampProvider, Func<byte[]> checksumProvider, Func<DateTime> changedTimestampProvider = null, Func<bool> weakChecksumProvider = null)
        {
            return CacheableObjectFactory.CreateCacheableObjectResult(instance, timestampProvider, checksumProvider, changedTimestampProvider, weakChecksumProvider);
        }

        /// <summary>
        /// Encapsulates the specified <paramref name="instance" /> within a timestamp based object that is processed by a Last-Modified filter implementation.
        /// </summary>
        /// <typeparam name="T">The type of the object to make cacheable.</typeparam>
        /// <param name="instance">The instance to make cacheable.</param>
        /// <param name="timestampProvider">The function delegate that resolves a timestamp from when the specified <paramref name="instance"/> was first created, expressed as the Coordinated Universal Time (UTC).</param>
        /// <param name="changedTimestampProvider">The function delegate that resolves a timestamp from when the specified <paramref name="instance"/> was last modified, expressed as the Coordinated Universal Time (UTC).</param>
        /// <returns>An <see cref="ICacheableObjectResult" /> implementation.</returns>
        /// <seealso cref="IEntityDataTimestamp" />
        /// <seealso cref="CacheableObjectResult{T}" />
        /// <seealso cref="HttpLastModifiedHeaderFilter"/>
        /// <seealso cref="HttpCacheableFilter"/>
        public static ICacheableObjectResult MakeCacheable<T>(this T instance, Func<T, DateTime> timestampProvider, Func<T, DateTime> changedTimestampProvider = null)
        {
            return CacheableObjectFactory.CreateCacheableObjectResult(instance, timestampProvider, changedTimestampProvider);
        }

        /// <summary>
        /// Encapsulates the specified <paramref name="instance" /> within an integrity based object that is processed by an HTTP ETag filter implementation.
        /// </summary>
        /// <typeparam name="T">The type of the object to make cacheable.</typeparam>
        /// <param name="instance">The instance to make cacheable.</param>
        /// <param name="checksumProvider">The function delegate that resolves a checksum defining the data integrity of the specified <paramref name="instance"/>.</param>
        /// <param name="weakChecksumProvider">The function delegate that resolves a value hinting whether the specified <paramref name="checksumProvider"/> resembles a weak or a strong checksum strength.</param>
        /// <returns>An <see cref="ICacheableObjectResult" /> implementation.</returns>
        /// <seealso cref="IEntityDataIntegrity" />
        /// <seealso cref="CacheableObjectResult{T}" />
        /// <seealso cref="HttpEntityTagHeaderFilter"/>
        /// <seealso cref="HttpCacheableFilter"/>
        public static ICacheableObjectResult MakeCacheable<T>(this T instance, Func<T, byte[]> checksumProvider, Func<T, bool> weakChecksumProvider = null)
        {
            return CacheableObjectFactory.CreateCacheableObjectResult(instance, checksumProvider, weakChecksumProvider);
        }

        /// <summary>
        /// Encapsulates the specified <paramref name="instance" /> within a timestamp and integrity based object that is processed by both HTTP Last-Modified and HTTP ETag filters implementation.
        /// </summary>
        /// <typeparam name="T">The type of the object to make cacheable.</typeparam>
        /// <param name="instance">The instance to make cacheable.</param>
        /// <param name="timestampProvider">The function delegate that resolves a timestamp from when the specified <paramref name="instance"/> was first created, expressed as the Coordinated Universal Time (UTC).</param>
        /// <param name="checksumProvider">The function delegate that resolves a checksum defining the data integrity of the specified <paramref name="instance"/>.</param>
        /// <param name="changedTimestampProvider">The function delegate that resolves a timestamp from when the specified <paramref name="instance"/> was last modified, expressed as the Coordinated Universal Time (UTC).</param>
        /// <param name="weakChecksumProvider">The function delegate that resolves a value hinting whether the specified <paramref name="checksumProvider"/> resembles a weak or a strong checksum strength.</param>
        /// <returns>An <see cref="ICacheableObjectResult" /> implementation.</returns>
        /// <seealso cref="IEntityDataTimestamp" />
        /// <seealso cref="IEntityDataIntegrity" />
        /// <seealso cref="IEntityInfo" />
        /// <seealso cref="CacheableObjectResult{T}" />
        /// <seealso cref="HttpLastModifiedHeaderFilter"/>
        /// <seealso cref="HttpEntityTagHeaderFilter"/>
        /// <seealso cref="HttpCacheableFilter"/>
        public static ICacheableObjectResult MakeCacheable<T>(this T instance, Func<T, DateTime> timestampProvider, Func<T, byte[]> checksumProvider, Func<T, DateTime> changedTimestampProvider = null, Func<T, bool> weakChecksumProvider = null)
        {
            return CacheableObjectFactory.CreateCacheableObjectResult(instance, timestampProvider, checksumProvider, changedTimestampProvider, weakChecksumProvider);
        }
    }
}