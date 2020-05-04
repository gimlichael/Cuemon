using System;
using System.IO;
using System.Threading.Tasks;

namespace Cuemon
{
    /// <summary>
    /// Extension methods for the <see cref="T:byte[]"/> tailored to adhere the decorator pattern.
    /// </summary>
    /// <seealso cref="IDecorator{T}"/>
    /// <seealso cref="Decorator{T}"/>
    public static class ByteArrayDecoratorExtensions
    {
        /// <summary>
        /// Converts the enclosed <see cref="T:byte[]"/> of the specified <paramref name="decorator"/> to its equivalent <see cref="Stream"/> representation.
        /// </summary>
        /// <param name="decorator">The <see cref="IDecorator{Stream}"/> to extend.</param>
        /// <returns>A <see cref="Stream"/> that is equivalent to the enclosed <see cref="T:byte[]"/> of the specified <paramref name="decorator"/>.</returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="decorator"/> cannot be null.
        /// </exception>
        public static Stream ToStream(this IDecorator<byte[]> decorator)
        {
            Validator.ThrowIfNull(decorator, nameof(decorator));
            return Disposable.SafeInvoke(() => new MemoryStream(decorator.Inner.Length), ms =>
            {
                ms.Write(decorator.Inner, 0, decorator.Inner.Length);
                ms.Position = 0;
                return ms;
            });
        }

        /// <summary>
        /// Converts the enclosed <see cref="T:byte[]"/> of the specified <paramref name="decorator"/> to its equivalent <see cref="Stream"/> representation.
        /// </summary>
        /// <param name="decorator">The <see cref="IDecorator{Stream}"/> to extend.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains a <see cref="Stream"/> that is equivalent to the enclosed <see cref="T:byte[]"/> of the specified <paramref name="decorator"/>.</returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="decorator"/> cannot be null.
        /// </exception>
        public static Task<Stream> ToStreamAsync(this IDecorator<byte[]> decorator)
        {
            Validator.ThrowIfNull(decorator, nameof(decorator));
            return Disposable.SafeInvokeAsync<Stream>(() => new MemoryStream(decorator.Inner.Length), async ms =>
            {
                await ms.WriteAsync(decorator.Inner, 0, decorator.Inner.Length).ConfigureAwait(false);
                ms.Position = 0;
                return ms;
            });
        }
    }
}