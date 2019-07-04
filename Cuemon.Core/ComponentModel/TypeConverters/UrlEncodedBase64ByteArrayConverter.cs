using System;
using Cuemon.ComponentModel.Converters;

namespace Cuemon.ComponentModel.TypeConverters
{
    /// <summary>
    /// Provides a converter that converts a <see cref="T:byte[]"/> to an encoded <see cref="string"/> of URL-safe base64 characters.
    /// </summary>
    public class UrlEncodedBase64ByteArrayConverter : IConverter<byte[], string>
    {
        /// <summary>
        /// Converts the specified <paramref name="input"/> to a <see cref="string"/> representation of URL-safe base64 characters.
        /// </summary>
        /// <param name="input">The <see cref="T:byte[]"/> to be converted into a <see cref="string"/>.</param>
        /// <returns>A <see cref="string"/> representation, in URL-safe base64 characters, from the content of <paramref name="input"/>.</returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="input"/> cannot be null.
        /// </exception>
        /// <remarks>Source: http://tools.ietf.org/html/draft-ietf-jose-json-web-signature-08#appendix-C</remarks>
        public string ChangeType(byte[] input)
        {
            Validator.ThrowIfNull(input, nameof(input));
            var base64 = System.Convert.ToBase64String(input);
            base64 = base64.Split('=')[0];
            base64 = base64.Replace('+', '-');
            base64 = base64.Replace('/', '_');
            return base64;
        }
    }
}