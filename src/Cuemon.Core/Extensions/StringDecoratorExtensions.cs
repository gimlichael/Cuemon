using System;
using System.ComponentModel;
using System.IO;
using System.Threading.Tasks;
using Cuemon.Integrity;
using Cuemon.Text;

namespace Cuemon
{
    /// <summary>
    /// Extension methods for the <see cref="string"/> class tailored to adhere the decorator pattern.
    /// </summary>
    /// <seealso cref="IDecorator{T}"/>
    /// <seealso cref="Decorator{T}"/>
    public static class StringDecoratorExtensions
    {
        /// <summary>
        /// Converts the enclosed <see cref="string"/> of the specified <paramref name="decorator"/> to a <see cref="Stream"/>.
        /// </summary>
        /// <param name="decorator">The <see cref="IDecorator{String}"/> to extend.</param>
        /// <param name="setup">The <see cref="EncodingOptions"/> which may be configured.</param>
        /// <returns>A <see cref="Stream"/> containing the result of the enclosed <see cref="string"/> of the specified <paramref name="decorator"/>.</returns>
        /// <remarks><see cref="IEncodingOptions"/> will be initialized with <see cref="EncodingOptions.DefaultPreambleSequence"/> and <see cref="EncodingOptions.DefaultEncoding"/>.</remarks>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="decorator"/> cannot be null.
        /// </exception>
        /// <exception cref="InvalidEnumArgumentException">
        /// <paramref name="setup"/> was initialized with an invalid <see cref="EncodingOptions.Preamble"/>.
        /// </exception>
        public static Stream ToStream(this IDecorator<string> decorator, Action<EncodingOptions> setup = null)
        {
            Validator.ThrowIfNull(decorator, nameof(decorator));
            return Disposable.SafeInvoke(() => new MemoryStream(), ms =>
            {
                var bytes = Convertible.GetBytes(decorator.Inner, setup);
                ms.Write(bytes, 0, bytes.Length);
                ms.Position = 0;
                return ms;
            });
        }

        /// <summary>
        /// Converts the enclosed <see cref="string"/> of the specified <paramref name="decorator"/> to a <see cref="Stream"/>.
        /// </summary>
        /// <param name="decorator">The <see cref="IDecorator{String}"/> to extend.</param>
        /// <param name="setup">The <see cref="EncodingOptions"/> which may be configured.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains a <see cref="Stream"/> containing the result of the enclosed <see cref="string"/> of the specified <paramref name="decorator"/>.</returns>
        /// <remarks><see cref="IEncodingOptions"/> will be initialized with <see cref="EncodingOptions.DefaultPreambleSequence"/> and <see cref="EncodingOptions.DefaultEncoding"/>.</remarks>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="decorator"/> cannot be null.
        /// </exception>
        /// <exception cref="InvalidEnumArgumentException">
        /// <paramref name="setup"/> was initialized with an invalid <see cref="EncodingOptions.Preamble"/>.
        /// </exception>
        public static Task<Stream> ToStreamAsync(this IDecorator<string> decorator, Action<EncodingOptions> setup = null)
        {
            Validator.ThrowIfNull(decorator, nameof(decorator));
            return Disposable.SafeInvokeAsync<Stream>(() => new MemoryStream(), async ms =>
            {
                var bytes = Convertible.GetBytes(decorator.Inner, setup);
                var options = Patterns.Configure(setup);
                await ms.WriteAsync(bytes, 0, bytes.Length, options.CancellationToken).ConfigureAwait(false);
                ms.Position = 0;
                return ms;
            });
        }
    }
}