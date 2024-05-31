using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using Cuemon.Text;

namespace Cuemon
{
    /// <summary>
    /// Provides access to factory methods for creating and configuring encoded <see cref="string"/> instances.
    /// </summary>
    public static class StringFactory
    {
        private static readonly IDictionary<UriScheme, string> UriSchemeToStringLookupTable = ParserFactory.StringToUriSchemeLookupTable.ToDictionary(pair => pair.Value, pair => pair.Key);

        /// <summary>
        /// Creates a hexadecimal string representation from the specified <paramref name="value"/>.
        /// </summary>
        /// <param name="value">The <see cref="T:byte[]"/> to convert.</param>
        /// <returns>A hexadecimal string representation that is equivalent to <paramref name="value"/>.</returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="value"/> cannot be null.
        /// </exception>
        public static string CreateHexadecimal(byte[] value)
        {
            Validator.ThrowIfNull(value);
            return BitConverter.ToString(value).Replace("-", "").ToLowerInvariant();
        }

        /// <summary>
        /// Creates a hexadecimal string representation from the specified <paramref name="value"/>.
        /// </summary>
        /// <param name="value">The <see cref="string"/> to convert.</param>
        /// <param name="setup">The <see cref="EncodingOptions"/> which may be configured.</param>
        /// <returns>A hexadecimal string representation that is equivalent to <paramref name="value"/>.</returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="value"/> cannot be null.
        /// </exception>
        /// <exception cref="InvalidEnumArgumentException">
        /// <paramref name="setup"/> was initialized with an invalid <see cref="PreambleSequence"/>.
        /// </exception>
        public static string CreateHexadecimal(string value, Action<EncodingOptions> setup = null)
        {
            Validator.ThrowIfNull(value);
            var encodedString = Convertible.GetBytes(value, setup);
            return CreateHexadecimal(encodedString);
        }

        /// <summary>
        /// Creates a binary digits string representation from the specified <paramref name="value"/>.
        /// </summary>
        /// <param name="value">The <see cref="T:byte[]"/> to convert.</param>
        /// <returns>A binary digits string representation that is equivalent to <paramref name="value"/>.</returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="value"/> cannot be null.
        /// </exception>
        public static string CreateBinaryDigits(byte[] value)
        {
            Validator.ThrowIfNull(value);
            return string.Concat(value.Select(b => Convert.ToString(b, 2).PadLeft(8, '0')));
        }

        /// <summary>
        /// Creates a base64 string representation, in URL-safe characters, from the specified <paramref name="value"/>.
        /// </summary>
        /// <param name="value">The <see cref="T:byte[]"/> to convert.</param>
        /// <returns>A base64 string representation, in URL-safe characters, that is equivalent to <paramref name="value"/>.</returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="value"/> cannot be null.
        /// </exception>
        /// <remarks>Source: http://tools.ietf.org/html/draft-ietf-jose-json-web-signature-08#appendix-C</remarks>
        public static string CreateUrlEncodedBase64(byte[] value)
        {
            Validator.ThrowIfNull(value);
            var base64 = Convert.ToBase64String(value);
            base64 = base64.Split('=')[0];
            base64 = base64.Replace('+', '-');
            base64 = base64.Replace('/', '_');
            return base64;
        }

        /// <summary>
        /// Creates a protocol-relative URL string representation from the specified <paramref name="value"/>.
        /// </summary>
        /// <param name="value">The <see cref="Uri"/> to convert.</param>
        /// <param name="setup">The <see cref="ProtocolRelativeUriStringOptions"/> which may be configured.</param>
        /// <returns>A protocol-relative URL string representation that is equivalent to <paramref name="value"/>.</returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="value"/> cannot be null.
        /// </exception>
        /// <exception cref="ArgumentException">
        /// <paramref name="value"/> must be an absolute URI.
        /// </exception>
        public static string CreateProtocolRelativeUrl(Uri value, Action<ProtocolRelativeUriStringOptions> setup = null)
        {
            Validator.ThrowIfNull(value);
            Validator.ThrowIfFalse(value.IsAbsoluteUri, nameof(value), "Uri must be absolute.");
            var options = Patterns.Configure(setup);
            var schemeLength = value.GetComponents(UriComponents.Scheme | UriComponents.KeepDelimiter, UriFormat.Unescaped).Length;
            return FormattableString.Invariant($"{options.RelativeReference}{value.OriginalString.Remove(0, schemeLength)}");
        }

        /// <summary>
        /// Creates an URI scheme string representation from the specified <paramref name="value"/>.
        /// </summary>
        /// <param name="value">The <see cref="UriScheme"/> to convert.</param>
        /// <returns>An URI scheme string representation that is equivalent to <paramref name="value"/>.</returns>
        public static string CreateUriScheme(UriScheme value)
        {
            if (!UriSchemeToStringLookupTable.TryGetValue(value, out var result))
            {
                result = UriScheme.Undefined.ToString();
            }
            return result;
        }
    }
}