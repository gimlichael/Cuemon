using System;

namespace Cuemon.Text
{
    /// <summary>
    /// Provides a parser that converts a <see cref="string"/>, represented in base-64 digits, to its equivalent <see cref="T:byte[]"/>.
    /// </summary>
    public class Base64Parser : IParser<byte[]>
    {
        /// <summary>
        /// Converts the string representation of base-64 digits to its <see cref="T:byte[]"/> equivalent.
        /// </summary>
        /// <param name="value">A string consisting of base-64 digits.</param>
        /// <returns>A <see cref="T:byte[]"/> equivalent to <paramref name="value"/>.</returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="value"/> cannot be null.
        /// </exception>
        /// <exception cref="FormatException">
        /// <paramref name="value"/> consist of illegal base-64 digits.
        /// </exception>
        /// <seealso cref="Convert.FromBase64String"/>
        public byte[] Parse(string value)
        {
            return Convert.FromBase64String(value);
        }

        /// <summary>
        /// Converts the string representation of base-64 digits to its <see cref="T:byte[]"/> equivalent. A return value indicates whether the conversion succeeded.
        /// </summary>
        /// <param name="value">A string consisting of base-64 digits.</param>
        /// <param name="result">When this method returns, contains the <see cref="T:byte[]"/> equivalent of the base-64 digits contained within <paramref name="value"/>, if the conversion succeeded, or <c>default</c> if the conversion failed.</param>
        /// <returns><c>true</c> if <paramref name="value"/> was converted successfully; otherwise, <c>false</c>.</returns>
        public bool TryParse(string value, out byte[] result)
        {
            return Patterns.TryInvoke(() => Parse(value), out result);
        }
    }
}