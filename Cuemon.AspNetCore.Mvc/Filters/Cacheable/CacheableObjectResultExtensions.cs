using System;

namespace Cuemon.AspNetCore.Mvc.Filters.Cacheable
{
    /// <summary>
    /// Extension methods for operations related to the <see cref="ICacheableObjectResult"/> interface.
    /// </summary>
    public static class CacheableObjectResultExtensions
    {
        /// <summary>
        /// Encapsulates the specified <paramref name="instance"/> within a <see cref="TimeBasedObjectResult{T}"/>.
        /// </summary>
        /// <typeparam name="T">The type of the object to make cacheable.</typeparam>
        /// <param name="instance">The instance to make cacheable.</param>
        /// <param name="created">The value from when data the <paramref name="instance"/> represents was first created, expressed as the Coordinated Universal Time (UTC).</param>
        /// <param name="setup">The <see cref="TimeBasedOptions"/> which need to be configured.</param>
        /// <returns>An <see cref="ICacheableObjectResult"/> implementation.</returns>
        public static ICacheableObjectResult ToCacheableObjectResult<T>(this T instance, DateTime created, Action<TimeBasedOptions> setup = null)
        {
            return new TimeBasedObjectResult<T>(instance, created, setup);
        }

        /// <summary>
        /// Encapsulates the specified <paramref name="instance"/> within a <see cref="TimeBasedObjectResult{T}"/>.
        /// </summary>
        /// <typeparam name="T">The type of the object to make cacheable.</typeparam>
        /// <param name="instance">The instance to make cacheable.</param>
        /// <param name="createdProvider">The function delegate that resolves a value from when data the <paramref name="instance"/> represents was first created, expressed as the Coordinated Universal Time (UTC).</param>
        /// <param name="setup">The <see cref="TimeBasedOptions"/> which need to be configured.</param>
        /// <returns>An <see cref="ICacheableObjectResult"/> implementation.</returns>
        public static ICacheableObjectResult ToCacheableObjectResult<T>(this T instance, Func<T, DateTime> createdProvider, Action<TimeBasedOptions> setup = null)
        {
            Validator.ThrowIfNull(createdProvider, nameof(createdProvider));
            return new TimeBasedObjectResult<T>(instance, createdProvider(instance), setup);
        }

        /// <summary>
        /// Encapsulates the specified <paramref name="instance"/> within a <see cref="ContentBasedObjectResult{T}"/>.
        /// </summary>
        /// <typeparam name="T">The type of the object to make cacheable.</typeparam>
        /// <param name="instance">The instance to make cacheable.</param>
        /// <param name="checksum">The checksum that defines the data integrity of <paramref name="instance"/>.</param>
        /// <param name="setup">The <see cref="ContentBasedOptions"/> which need to be configured.</param>
        /// <returns>An <see cref="ICacheableObjectResult"/> implementation.</returns>
        public static ICacheableObjectResult ToCacheableObjectResult<T>(this T instance, byte[] checksum, Action<ContentBasedOptions> setup = null)
        {
            return new ContentBasedObjectResult<T>(instance, checksum, setup);
        }

        /// <summary>
        /// Encapsulates the specified <paramref name="instance"/> within a <see cref="ContentBasedObjectResult{T}"/>.
        /// </summary>
        /// <typeparam name="T">The type of the object to make cacheable.</typeparam>
        /// <param name="instance">The instance to make cacheable.</param>
        /// <param name="checksumProvider">The function delegate that resolves a checksum that defines the data integrity of <paramref name="instance"/>.</param>
        /// <param name="setup">The <see cref="ContentBasedOptions"/> which need to be configured.</param>
        /// <returns>An <see cref="ICacheableObjectResult"/> implementation.</returns>
        public static ICacheableObjectResult ToCacheableObjectResult<T>(this T instance, Func<T, byte[]> checksumProvider, Action<ContentBasedOptions> setup = null)
        {
            Validator.ThrowIfNull(checksumProvider, nameof(checksumProvider));
            return new ContentBasedObjectResult<T>(instance, checksumProvider(instance), setup);
        }

        /// <summary>
        /// Encapsulates the specified <paramref name="instance" /> within a <see cref="ContentTimeBasedObjectResult{T}" />.
        /// </summary>
        /// <typeparam name="T">The type of the object to make cacheable.</typeparam>
        /// <param name="instance">The instance to make cacheable.</param>
        /// <param name="created">The value from when data the <paramref name="instance"/> represents was first created, expressed as the Coordinated Universal Time (UTC).</param>
        /// <param name="checksum">The checksum that defines the data integrity of <paramref name="instance"/>.</param>
        /// <param name="setup">The <see cref="ContentTimeBasedOptions" /> which need to be configured.</param>
        /// <returns>An <see cref="ICacheableObjectResult" /> implementation.</returns>
        public static ICacheableObjectResult ToCacheableObjectResult<T>(this T instance, DateTime created, byte[] checksum, Action<ContentTimeBasedOptions> setup = null)
        {
            return new ContentTimeBasedObjectResult<T>(instance, created, checksum, setup);
        }

        /// <summary>
        /// Encapsulates the specified <paramref name="instance" /> within a <see cref="ContentTimeBasedObjectResult{T}" />.
        /// </summary>
        /// <typeparam name="T">The type of the object to make cacheable.</typeparam>
        /// <param name="instance">The instance to make cacheable.</param>
        /// <param name="createdProvider">The function delegate that resolves a value from when data the <paramref name="instance"/> represents was first created, expressed as the Coordinated Universal Time (UTC).</param>
        /// <param name="checksumProvider">The function delegate that resolves a checksum that defines the data integrity of <paramref name="instance"/>.</param>
        /// <param name="setup">The <see cref="ContentTimeBasedOptions" /> which need to be configured.</param>
        /// <returns>An <see cref="ICacheableObjectResult" /> implementation.</returns>
        public static ICacheableObjectResult ToCacheableObjectResult<T>(this T instance, Func<T, DateTime> createdProvider, Func<T, byte[]> checksumProvider, Action<ContentTimeBasedOptions> setup = null)
        {
            Validator.ThrowIfNull(createdProvider, nameof(createdProvider));
            Validator.ThrowIfNull(checksumProvider, nameof(checksumProvider));
            return new ContentTimeBasedObjectResult<T>(instance, createdProvider(instance), checksumProvider(instance), setup);
        }

        /// <summary>
        /// Encapsulates the specified <paramref name="instance" /> within a <see cref="ContentTimeBasedObjectResult{T}" />.
        /// </summary>
        /// <typeparam name="T">The type of the object to make cacheable.</typeparam>
        /// <param name="instance">The instance to make cacheable.</param>
        /// <param name="created">The value from when data the <paramref name="instance"/> represents was first created, expressed as the Coordinated Universal Time (UTC).</param>
        /// <param name="checksumProvider">The function delegate that resolves a checksum that defines the data integrity of <paramref name="instance"/>.</param>
        /// <param name="setup">The <see cref="ContentTimeBasedOptions" /> which need to be configured.</param>
        /// <returns>An <see cref="ICacheableObjectResult" /> implementation.</returns>
        public static ICacheableObjectResult ToCacheableObjectResult<T>(this T instance, DateTime created, Func<T, byte[]> checksumProvider, Action<ContentTimeBasedOptions> setup = null)
        {
            Validator.ThrowIfNull(checksumProvider, nameof(checksumProvider));
            return new ContentTimeBasedObjectResult<T>(instance, created, checksumProvider(instance), setup);
        }

        /// <summary>
        /// Encapsulates the specified <paramref name="instance" /> within a <see cref="ContentTimeBasedObjectResult{T}" />.
        /// </summary>
        /// <typeparam name="T">The type of the object to make cacheable.</typeparam>
        /// <param name="instance">The instance to make cacheable.</param>
        /// <param name="createdProvider">The function delegate that resolves a value from when data the <paramref name="instance"/> represents was first created, expressed as the Coordinated Universal Time (UTC).</param>
        /// <param name="checksum">The checksum that defines the data integrity of <paramref name="instance"/>.</param>
        /// <param name="setup">The <see cref="ContentTimeBasedOptions" /> which need to be configured.</param>
        /// <returns>An <see cref="ICacheableObjectResult" /> implementation.</returns>
        public static ICacheableObjectResult ToCacheableObjectResult<T>(this T instance, Func<T, DateTime> createdProvider, byte[] checksum, Action<ContentTimeBasedOptions> setup = null)
        {
            Validator.ThrowIfNull(createdProvider, nameof(createdProvider));
            return new ContentTimeBasedObjectResult<T>(instance, createdProvider(instance), checksum, setup);
        }
    }
}