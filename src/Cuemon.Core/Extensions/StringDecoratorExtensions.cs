using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Text;
using System.Threading.Tasks;
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
        /// Converts the enclosed <see cref="string"/> of the specified <paramref name="decorator"/> to its equivalent <see cref="T:byte[]"/> representation.
        /// </summary>
        /// <param name="decorator">The <see cref="IDecorator{String}"/> to extend.</param>
        /// <param name="setup">The <see cref="EncodingOptions"/> which may be configured.</param>
        /// <returns>A <see cref="T:byte[]"/> containing the result of the enclosed <see cref="string"/> of the specified <paramref name="decorator"/>.</returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="decorator"/> cannot be null.
        /// </exception>
        /// <exception cref="InvalidEnumArgumentException">
        /// <paramref name="setup"/> was initialized with an invalid <see cref="EncodingOptions.Preamble"/>.
        /// </exception>
        public static byte[] ToByteArray(this IDecorator<string> decorator, Action<EncodingOptions> setup = null)
        {
            Validator.ThrowIfNull(decorator, nameof(decorator));
            return Convertible.GetBytes(decorator.Inner, setup);
        }

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

        /// <summary>
        /// Converts the enclosed <see cref="string"/> of the specified <paramref name="decorator"/> to its equivalent <see cref="Uri"/> representation.
        /// </summary>
        /// <param name="decorator">The <see cref="IDecorator{String}"/> to extend.</param>
        /// <param name="uriKind">Specifies whether the URI string is a relative URI, absolute URI, or is indeterminate.</param>
        /// <returns>A <see cref="Uri"/> that corresponds to the enclosed <see cref="string"/> of the specified <paramref name="decorator"/> and <paramref name="uriKind"/>.</returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="decorator"/> cannot be null.
        /// </exception>
        /// <exception cref="ArgumentNullException">
        /// The enclosed <see cref="string"/> of the specified <paramref name="decorator"/> cannot be null.
        /// </exception>
        /// <exception cref="ArgumentException">
        /// The enclosed <see cref="string"/> of the specified <paramref name="decorator"/> cannot be empty or consist only of white-space characters.
        /// </exception>
        public static Uri ToUri(this IDecorator<string> decorator, UriKind uriKind = UriKind.Absolute)
        {
            Validator.ThrowIfNull(decorator, nameof(decorator));
            Validator.ThrowIfNullOrWhitespace(decorator.Inner, nameof(decorator));
            return new Uri(decorator.Inner, uriKind);
        }

        /// <summary>
        /// Determines whether the beginning of the enclosed <see cref="string"/> of the specified <paramref name="decorator"/> matches at least one string in the specified sequence of <paramref name="strings"/>.
        /// </summary>
        /// <param name="decorator">The <see cref="IDecorator{String}"/> to extend.</param>
        /// <param name="strings">A sequence of <see cref="string"/> values to match against.</param>
        /// <returns><c>true</c> if at least one value matches the beginning of the enclosed <see cref="string"/> of the specified <paramref name="decorator"/>; otherwise, <c>false</c>.</returns>
        /// <remarks>This match is performed by using a default value of <see cref="StringComparison.OrdinalIgnoreCase"/>.</remarks>
        public static bool StartsWith(this IDecorator<string> decorator, IEnumerable<string> strings)
        {
            return StartsWith(decorator, StringComparison.OrdinalIgnoreCase, strings);
        }

        /// <summary>
        /// Determines whether the beginning of the enclosed <see cref="string"/> of the specified <paramref name="decorator"/> matches at least one string in the specified sequence of <paramref name="strings"/>.
        /// </summary>
        /// <param name="decorator">The <see cref="IDecorator{String}"/> to extend.</param>
        /// <param name="comparison">One of the enumeration values that specifies the rules to use in the comparison.</param>
        /// <param name="strings">A sequence of <see cref="string"/> values to match against.</param>
        /// <returns><c>true</c> if at least one value matches the beginning of the enclosed <see cref="string"/> of the specified <paramref name="decorator"/>; otherwise, <c>false</c>.</returns>
        public static bool StartsWith(this IDecorator<string> decorator, StringComparison comparison, IEnumerable<string> strings)
        {
            if (decorator?.Inner == null) { return false; }
            if (strings == null) { return false; }
            foreach (var startWithValue in strings)
            {
                if (decorator.Inner.StartsWith(startWithValue, comparison)) { return true; }
            }
            return false;
        }

        /// <summary>
        /// Determines whether the beginning of the enclosed <see cref="string"/> of the specified <paramref name="decorator"/> matches at least one string in the specified sequence of <paramref name="strings"/>.
        /// </summary>
        /// <param name="decorator">The <see cref="IDecorator{String}"/> to extend.</param>
        /// <param name="strings">A sequence of <see cref="string"/> values to match against.</param>
        /// <returns><c>true</c> if at least one value matches the beginning of the enclosed <see cref="string"/> of the specified <paramref name="decorator"/>; otherwise, <c>false</c>.</returns>
        /// <remarks>This match is performed by using a default value of <see cref="StringComparison.OrdinalIgnoreCase"/>.</remarks>
        public static bool StartsWith(this IDecorator<string> decorator, params string[] strings)
        {
            return StartsWith(decorator, (IEnumerable<string>)strings);
        }

        /// <summary>
        /// Determines whether the beginning of the enclosed <see cref="string"/> of the specified <paramref name="decorator"/> matches at least one string in the specified sequence of <paramref name="strings"/>.
        /// </summary>
        /// <param name="decorator">The <see cref="IDecorator{String}"/> to extend.</param>
        /// <param name="comparison">One of the enumeration values that specifies the rules to use in the comparison.</param>
        /// <param name="strings">A sequence of <see cref="string"/> values to match against.</param>
        /// <returns><c>true</c> if at least one value matches the beginning of this string; otherwise, <c>false</c>.</returns>
        /// <remarks>This match is performed by using a default value of <see cref="StringComparison.OrdinalIgnoreCase"/>.</remarks>
        public static bool StartsWith(this IDecorator<string> decorator, StringComparison comparison, params string[] strings)
        {
            return StartsWith(decorator, comparison, (IEnumerable<string>)strings);
        }
    }
}