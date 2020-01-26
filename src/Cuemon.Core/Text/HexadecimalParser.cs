using System;
using System.Collections.Generic;
using System.IO;

namespace Cuemon.Text
{
    /// <summary>
    /// Provides a parser that converts a <see cref="string"/>, represented in hexadecimal digits, to its equivalent <see cref="T:byte[]"/>.
    /// </summary>
    public class HexadecimalParser : IParser<byte[]>
    {
        /// <summary>
        /// Converts the string representation of hexadecimal digits to its <see cref="T:byte[]"/> equivalent.
        /// </summary>
        /// <param name="value">A string consisting of hexadecimal digits.</param>
        /// <returns>A <see cref="T:byte[]"/> equivalent to <paramref name="value"/>.</returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="value"/> cannot be null.
        /// </exception>
        /// <exception cref="ArgumentException">
        /// <paramref name="value"/> must be hexadecimal.
        /// </exception>
        public byte[] Parse(string value)
        {
            Validator.ThrowIfNull(value, nameof(value));
            Validator.ThrowIfNotHex(value, nameof(value));
            var converted = new List<byte>();
            var stringLength = value.Length / 2;
            using (var reader = new StringReader(value))
            {
                for (var i = 0; i < stringLength; i++)
                {
                    var firstChar = (char)reader.Read();
                    var secondChar = (char)reader.Read();
                    converted.Add(Convert.ToByte(new string(new[] { firstChar, secondChar }), 16));
                }
            }
            return converted.ToArray();
        }

        /// <summary>
        /// Converts the string representation of hexadecimal digits to its <see cref="T:byte[]"/> equivalent. A return value indicates whether the conversion succeeded.
        /// </summary>
        /// <param name="value">A string consisting of hexadecimal digits.</param>
        /// <param name="result">When this method returns, contains the <see cref="T:byte[]"/> equivalent of the hexadecimal digits contained within <paramref name="value"/>, if the conversion succeeded, or <c>default</c> if the conversion failed.</param>
        /// <returns><c>true</c> if <paramref name="value"/> was converted successfully; otherwise, <c>false</c>.</returns>
        public bool TryParse(string value, out byte[] result)
        {
            return Patterns.TryInvoke(() => Parse(value), out result);
        }
    }
}