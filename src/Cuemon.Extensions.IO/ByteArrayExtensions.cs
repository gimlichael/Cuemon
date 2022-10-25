using System;
using System.IO;
using System.Threading.Tasks;

namespace Cuemon.Extensions.IO
{
    /// <summary>
    /// Extension methods for the <see cref="T:byte[]"/>.
    /// </summary>
    public static class ByteArrayExtensions
    {
        /// <summary>
        /// Converts the specified <paramref name="bytes"/> to its equivalent <see cref="Stream"/> representation.
        /// </summary>
        /// <param name="bytes">The <see cref="T:byte[]"/> to extend.</param>
        /// <returns>A <see cref="Stream"/> that is equivalent to <paramref name="bytes"/>.</returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="bytes"/> cannot be null.
        /// </exception>
        public static Stream ToStream(this byte[] bytes)
        {
            Validator.ThrowIfNull(bytes);
            return Decorator.Enclose(bytes).ToStream();
        }

        /// <summary>
        /// Converts the the specified <paramref name="bytes"/> to its equivalent <see cref="Stream"/> representation.
        /// </summary>
        /// <param name="bytes">The <see cref="T:byte[]"/> to extend.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains a <see cref="Stream"/> that is equivalent to <paramref name="bytes"/>.</returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="bytes"/> cannot be null.
        /// </exception>
        public static Task<Stream> ToStreamAsync(this byte[] bytes)
        {
            Validator.ThrowIfNull(bytes);
            return Decorator.Enclose(bytes).ToStreamAsync();
        }
    }
}