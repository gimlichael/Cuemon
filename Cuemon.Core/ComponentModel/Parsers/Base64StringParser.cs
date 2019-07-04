using System;

namespace Cuemon.ComponentModel.Parsers
{
    /// <summary>
    /// Provides a parser that converts a <see cref="string"/>, represented in base-64 digits, to its equivalent <see cref="T:byte[]"/>.
    /// </summary>
    public class Base64StringParser : IParser<byte[]>
    {
        /// <summary>
        /// Converts the string representation of base-64 digits to its <see cref="T:byte[]"/> equivalent.
        /// </summary>
        /// <param name="input">A string consisting of base-64 digits.</param>
        /// <returns>A <see cref="T:byte[]"/> equivalent to <paramref name="input"/>.</returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="input"/> cannot be null.
        /// </exception>
        /// <exception cref="FormatException">
        /// <paramref name="input"/> consist of illegal base-64 digits.
        /// </exception>
        /// <seealso cref="Convert.FromBase64String"/>
        public byte[] Parse(string input)
        {
            return Convert.FromBase64String(input);
        }

        /// <summary>
        /// Converts the string representation of base-64 digits to its <see cref="T:byte[]"/> equivalent. A return value indicates whether the conversion succeeded.
        /// </summary>
        /// <param name="input">A string consisting of base-64 digits.</param>
        /// <param name="result">When this method returns, contains the <see cref="T:byte[]"/> equivalent of the base-64 digits contained within <paramref name="input"/>, if the conversion succeeded, or <c>default</c> if the conversion failed.</param>
        /// <returns><c>true</c> if <paramref name="input"/> was converted successfully; otherwise, <c>false</c>.</returns>
        public bool TryParse(string input, out byte[] result)
        {
            return Patterns.TryInvoke(() => Parse(input), out result);
        }
    }
}