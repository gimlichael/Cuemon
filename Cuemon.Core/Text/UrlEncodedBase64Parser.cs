using System;

namespace Cuemon.Text
{
    /// <summary>
    /// Provides a parser that converts a <see cref="string"/>, represented in URL-safe base-64 digits, to its equivalent <see cref="T:byte[]"/>.
    /// </summary>
    public class UrlEncodedBase64Parser : IParser<byte[]>
    {
        /// <summary>
        /// Converts the string representation of an URL-safe base-64 digits to its <see cref="T:byte[]"/> equivalent.
        /// </summary>
        /// <param name="value">A string consisting of URL-safe base-64 digits.</param>
        /// <returns>A <see cref="T:byte[]"/> equivalent to <paramref name="value"/>.</returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="value"/> cannot be null.
        /// </exception>
        /// <exception cref="ArgumentException">
        /// <paramref name="value"/> cannot be empty or consist only of white-space characters.
        /// </exception>
        /// <exception cref="FormatException">
        /// <paramref name="value"/> consist of illegal base-64 digits.
        /// </exception>
        public byte[] Parse(string value)
        {
            Validator.ThrowIfNullOrWhitespace(value, nameof(value));
            value = value.Replace('-', '+');
            value = value.Replace('_', '/');
            switch (value.Length % 4)
            {
                case 0:
                    break;
                case 2:
                    value += "==";
                    break;
                case 3:
                    value += "=";
                    break;
                default:
                    throw new FormatException(FormattableString.Invariant($"The format of {nameof(value)} consist of illegal base64 characters."));
            }
            return Convert.FromBase64String(value);
        }

        /// <summary>
        /// Converts the string representation of an URL-safe base-64 digits to its <see cref="T:byte[]"/> equivalent. A return value indicates whether the conversion succeeded.
        /// </summary>
        /// <param name="value">A string consisting of URL-safe base-64 digits.</param>
        /// <param name="result">When this method returns, contains the <see cref="T:byte[]"/> equivalent of the URL-safe base-64 digits contained within <paramref name="value"/>, if the conversion succeeded, or <c>default</c> if the conversion failed.</param>
        /// <returns><c>true</c> if <paramref name="value"/> was converted successfully; otherwise, <c>false</c>.</returns>
        public bool TryParse(string value, out byte[] result)
        {
            return Patterns.TryInvoke(() => Parse(value), out result);
        }
    }
}