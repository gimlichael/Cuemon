using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Cuemon.Text;

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
        /// Converts the enclosed <see cref="T:byte[]"/> of the specified <paramref name="decorator"/> to its equivalent <see cref="string"/> representation.
        /// </summary>
        /// <param name="decorator">The <see cref="T:IDecorator{byte[]}"/> to extend.</param>
        /// <param name="setup">The <see cref="EncodingOptions"/> which may be configured.</param>
        /// <returns>A <see cref="string"/> that is equivalent to the enclosed <see cref="T:byte[]"/> of the specified <paramref name="decorator"/>.</returns>
        /// <remarks><see cref="EncodingOptions"/> will be initialized with <see cref="EncodingOptions.DefaultPreambleSequence"/> and <see cref="EncodingOptions.DefaultEncoding"/>.</remarks>
        public static string ToEncodedString(this IDecorator<byte[]> decorator, Action<EncodingOptions> setup = null)
        {
            Validator.ThrowIfNull(decorator, nameof(decorator));
            return Convertible.ToString(decorator.Inner, setup);
        }

        /// <summary>
        /// Converts the enclosed <see cref="T:byte[]"/> of the specified <paramref name="decorator"/> to its equivalent <see cref="Stream"/> representation.
        /// </summary>
        /// <param name="decorator">The <see cref="T:IDecorator{byte[]}"/> to extend.</param>
        /// <returns>A <see cref="Stream"/> that is equivalent to the enclosed <see cref="T:byte[]"/> of the specified <paramref name="decorator"/>.</returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="decorator"/> cannot be null.
        /// </exception>
        public static Stream ToStream(this IDecorator<byte[]> decorator)
        {
            Validator.ThrowIfNull(decorator, nameof(decorator));
            return Patterns.SafeInvoke(() => new MemoryStream(decorator.Inner.Length), ms =>
            {
                ms.Write(decorator.Inner, 0, decorator.Inner.Length);
                ms.Position = 0;
                return ms;
            });
        }

        /// <summary>
        /// Converts the enclosed <see cref="T:byte[]"/> of the specified <paramref name="decorator"/> to its equivalent <see cref="Stream"/> representation.
        /// </summary>
        /// <param name="decorator">The <see cref="T:IDecorator{byte[]}"/> to extend.</param>
        /// <param name="ct">The token to monitor for cancellation requests. The default value is <see cref="CancellationToken.None"/>.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains a <see cref="Stream"/> that is equivalent to the enclosed <see cref="T:byte[]"/> of the specified <paramref name="decorator"/>.</returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="decorator"/> cannot be null.
        /// </exception>
        public static Task<Stream> ToStreamAsync(this IDecorator<byte[]> decorator, CancellationToken ct = default)
        {
            Validator.ThrowIfNull(decorator, nameof(decorator));
            return Patterns.SafeInvokeAsync<Stream>(() => new MemoryStream(decorator.Inner.Length), async (ms, cti) =>
            {
                #if NETSTANDARD
                await ms.WriteAsync(decorator.Inner, 0, decorator.Inner.Length, cti).ConfigureAwait(false);
                #else
                await ms.WriteAsync(decorator.Inner.AsMemory(0, decorator.Inner.Length), cti).ConfigureAwait(false);
                #endif
                ms.Position = 0;
                return ms;
            }, ct);
        }
    }
}