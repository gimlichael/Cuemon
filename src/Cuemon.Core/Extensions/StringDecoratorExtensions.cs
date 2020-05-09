using System;
using System.ComponentModel;
using System.IO;
using System.Text;
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
        /// Encodes all the characters in the enclosed <see cref="string"/> of the specified <paramref name="decorator"/> to its encoded <see cref="string"/> variant.
        /// </summary>
        /// <param name="decorator">The <see cref="IDecorator{String}"/> to extend.</param>
        /// <param name="setup">The <see cref="FallbackEncodingOptions"/> which may be configured.</param>
        /// <returns>A <see cref="string"/> variant of the enclosed <see cref="string"/> of the specified <paramref name="decorator"/> that is encoded with <see cref="FallbackEncodingOptions.TargetEncoding"/>.</returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="decorator"/> cannot be null.
        /// </exception>
        /// <remarks>The inspiration for this method was retrieved @ SO: https://stackoverflow.com/a/135473/175073.</remarks>
        public static string ToEncodedString(this IDecorator<string> decorator, Action<FallbackEncodingOptions> setup = null)
        {
            Validator.ThrowIfNull(decorator, nameof(decorator));
            var options = Patterns.Configure(setup);
            var result = Encoding.Convert(options.Encoding, Encoding.GetEncoding(options.TargetEncoding.WebName, options.EncoderFallback, options.DecoderFallback), Convertible.GetBytes(decorator.Inner, o =>
            {
                o.Encoding = options.Encoding;
                o.Preamble = options.Preamble;
            }));
            return options.TargetEncoding.GetString(result);
        }

        /// <summary>
        /// Encodes all the characters in the enclosed <see cref="string"/> of the specified <paramref name="decorator"/> to its ASCII encoded variant.
        /// </summary>
        /// <param name="decorator">The <see cref="IDecorator{String}"/> to extend.</param>
        /// <param name="setup">The <see cref="EncodingOptions"/> which may be configured.</param>
        /// <returns>A <see cref="string"/> variant of the enclosed <see cref="string"/> of the specified <paramref name="decorator"/> that is ASCII encoded.</returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="decorator"/> cannot be null.
        /// </exception>
        public static string ToAsciiEncodedString(this IDecorator<string> decorator, Action<EncodingOptions> setup = null)
        {
            Validator.ThrowIfNull(decorator, nameof(decorator));
            var options = Patterns.Configure(setup);
            return ToEncodedString(decorator, o =>
            {
                o.TargetEncoding = Encoding.ASCII;
                o.Encoding = options.Encoding;
                o.Preamble = options.Preamble;
                o.EncoderFallback = new EncoderReplacementFallback("");
            });
        }

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