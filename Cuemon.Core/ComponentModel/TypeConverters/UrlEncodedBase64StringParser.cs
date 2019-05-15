using System;

namespace Cuemon.ComponentModel.TypeConverters
{
    /// <summary>
    /// Provides a parser that converts a <see cref="string"/>, represented in URL-safe base64 characters, to its equivalent <see cref="T:byte[]"/>.
    /// </summary>
    public class UrlEncodedBase64StringParser : IParser<byte[]>
    {
        /// <summary>
        /// Converts the string representation of an URL-safe base64 characters to its <see cref="T:byte[]"/> equivalent.
        /// </summary>
        /// <param name="input">A string consisting of URL-safe base64 characters.</param>
        /// <returns>A <see cref="T:byte[]"/> equivalent to <paramref name="input"/>.</returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="input"/> cannot be null.
        /// </exception>
        /// <exception cref="ArgumentException">
        /// <paramref name="input"/> cannot be empty or consist only of white-space characters.
        /// </exception>
        /// <exception cref="FormatException">
        /// <paramref name="input"/> consist of illegal base64 characters.
        /// </exception>
        /// <seealso cref="UrlEncodedBase64StringConverter"/>
        public byte[] Parse(string input)
        {
            return Converters.FromString.ToByteArray<UrlEncodedBase64StringConverter>(input);
        }

        /// <summary>
        /// Converts the string representation of an URL-safe base64 characters number to its <see cref="T:byte[]"/> equivalent. A return value indicates whether the conversion succeeded.
        /// </summary>
        /// <param name="input">A string consisting of URL-safe base64 characters.</param>
        /// <param name="result">When this method returns, contains the <see cref="T:byte[]"/> equivalent of the URL-safe base64 characters contained within <paramref name="input"/>, if the conversion succeeded, or <c>default</c> if the conversion failed.</param>
        /// <returns><c>true</c> if <paramref name="input"/> was converted successfully; otherwise, <c>false</c>.</returns>
        public bool TryParse(string input, out byte[] result)
        {
            return Patterns.TryParse(() => Parse(input), out result);
        }
    }
}