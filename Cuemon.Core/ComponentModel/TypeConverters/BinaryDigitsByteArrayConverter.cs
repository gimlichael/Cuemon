using System;
using System.Linq;
using Cuemon.ComponentModel.Converters;

namespace Cuemon.ComponentModel.TypeConverters
{
    /// <summary>
    /// Provides a converter that converts a <see cref="T:byte[]"/> to an encoded <see cref="string"/> of binary digits.
    /// </summary>
    public class BinaryDigitsByteArrayConverter : IConverter<byte[], string>
    {
        /// <summary>
        /// Converts the specified <paramref name="input"/> to a <see cref="string"/> representation of binary digits.
        /// </summary>
        /// <param name="input">The <see cref="T:byte[]"/> to be converted into a <see cref="string"/>.</param>
        /// <returns>A <see cref="string"/> representation, of binary digits, from the content of <paramref name="input"/>.</returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="input"/> cannot be null.
        /// </exception>
        public string ChangeType(byte[] input)
        {
            Validator.ThrowIfNull(input, nameof(input));
            return string.Concat(input.Select(b => System.Convert.ToString(b, 2).PadLeft(8, '0')));
        }
    }
}