using System;
using System.Collections.Generic;

namespace Cuemon.Text
{
    /// <summary>
    /// Provides a parser that converts a <see cref="string"/>, represented in binary digits, to its equivalent <see cref="T:byte[]"/>.
    /// </summary>
    public class BinaryDigitsParser : IParser<byte[]>
    {
        /// <summary>
        /// Converts the string representation of binary digits to its <see cref="T:byte[]"/> equivalent.
        /// </summary>
        /// <param name="value">A string containing binary digits.</param>
        /// <returns>A <see cref="T:byte[]"/> equivalent to <paramref name="value"/>.</returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="value"/> cannot be null.
        /// </exception>
        /// <exception cref="ArgumentException">
        /// <paramref name="value"/> cannot be empty or consist only of white-space characters.
        /// </exception>
        /// <exception cref="FormatException">
        /// <paramref name="value"/> must consist only of binary digits.
        /// </exception>
        public byte[] Parse(string value)
        {
            try
            {
                Validator.ThrowIfNullOrWhitespace(value, nameof(value));
                Validator.ThrowIfNotBinaryDigits(value, nameof(value));
                var bytes = new List<byte>();
                for (var i = 0; i < value.Length; i += 8)
                {
                    bytes.Add(Convert.ToByte(value.Substring(i, 8), 2));
                }
                return bytes.ToArray();
            }
            catch (ArgumentOutOfRangeException)
            {
                throw new FormatException(FormattableString.Invariant($"The format of {nameof(value)} must consist only of binary digits."));
            }
        }

        /// <summary>
        /// Converts the string representation of binary digits to its <see cref="T:byte[]"/> equivalent. A return value indicates whether the conversion succeeded.
        /// </summary>
        /// <param name="value">A string consisting of URL-safe base64 characters.</param>
        /// <param name="result">When this method returns, contains the <see cref="T:byte[]"/> equivalent of the binary digits contained within <paramref name="value"/>, if the conversion succeeded, or <c>default</c> if the conversion failed.</param>
        /// <returns><c>true</c> if <paramref name="value"/> was converted successfully; otherwise, <c>false</c>.</returns>
        public bool TryParse(string value, out byte[] result)
        {
            return Patterns.TryInvoke(() => Parse(value), out result);
        }
    }
}