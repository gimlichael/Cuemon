using System;
using Cuemon.ComponentModel.Converters;

namespace Cuemon.ComponentModel.TypeConverters
{
    /// <summary>
    /// Provides a converter that converts a <see cref="string"/>, represented in URL-safe base64 characters, to its equivalent <see cref="T:byte[]"/>.
    /// </summary>
    public class UrlEncodedBase64StringConverter : IConverter<string, byte[]>
    {
        /// <summary>
        /// Converts the specified <paramref name="input"/> of URL-safe base64 characters to its equivalent <see cref="T:byte[]"/> representation.
        /// </summary>
        /// <param name="input">The <see cref="string"/> to be converted into a <see cref="T:byte[]"/>.</param>
        /// <returns>A <see cref="T:byte[]"/> that is equivalent to <paramref name="input"/>.</returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="input"/> cannot be null.
        /// </exception>
        /// <exception cref="ArgumentException">
        /// <paramref name="input"/> cannot be empty or consist only of white-space characters.
        /// </exception>
        /// <exception cref="FormatException">
        /// <paramref name="input"/> consist of illegal base64 characters.
        /// </exception>
        /// <remarks>Source: http://tools.ietf.org/html/draft-ietf-jose-json-web-signature-08#appendix-C</remarks>
        public byte[] ChangeType(string input)
        {
            Validator.ThrowIfNullOrWhitespace(input, nameof(input));
            input = input.Replace('-', '+');
            input = input.Replace('_', '/');
            switch (input.Length % 4)
            {
                case 0:
                    break;
                case 2:
                    input += "==";
                    break;
                case 3:
                    input += "=";
                    break;
                default:
                    throw new FormatException(FormattableString.Invariant($"The format of {nameof(input)} consist of illegal base64 characters."));
            }
            return System.Convert.FromBase64String(input);
        }
    }
}