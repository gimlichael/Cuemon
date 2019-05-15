using System;
using System.Collections.Generic;

namespace Cuemon.ComponentModel.TypeConverters
{
    /// <summary>
    /// Provides a converter that converts a <see cref="string"/>, represented in binary digits, to its equivalent <see cref="T:byte[]"/>.
    /// </summary>
    public class BinaryDigitsStringConverter : IConverter<string, byte[]>
    {
        /// <summary>
        /// Converts the specified <paramref name="input"/> of binary digits to its equivalent <see cref="T:byte[]"/> representation.
        /// </summary>
        /// <param name="input">The <see cref="string"/> to be converted into a <see cref="T:byte[]"/>.</param>
        /// <returns>A <see cref="T:byte[]"/> that is equivalent to <paramref name="input"/>.</returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="input"/> cannot be null.
        /// </exception>
        /// <exception cref="ArgumentException">
        /// <paramref name="input"/> cannot be empty or consist only of white-space characters.
        /// </exception>
        /// <exception cref="ArgumentOutOfRangeException">
        /// <paramref name="input"/> must consist only of binary digits.
        /// </exception>
        public byte[] ChangeType(string input)
        {
            Validator.ThrowIfNullOrWhitespace(input, nameof(input));
            Validator.ThrowIfNotBinaryDigits(input, nameof(input));
            var bytes = new List<byte>();
            for (var i = 0; i < input.Length; i += 8)
            {
                bytes.Add(Convert.ToByte(input.Substring(i, 8), 2));
            }
            return bytes.ToArray();
        }
    }
}