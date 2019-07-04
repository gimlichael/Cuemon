using System;
using Cuemon.ComponentModel.Converters;

namespace Cuemon.ComponentModel.TypeConverters
{
    /// <summary>
    /// Provides a converter that converts a <see cref="T:byte[]"/> to its equivalent hexadecimal <see cref="string"/>.
    /// </summary>
    public class HexadecimalByteArrayConverter : IConverter<byte[], string>
    {
        /// <summary>
        /// Converts the specified <paramref name="input"/> to its equivalent <see cref="string"/> representation in hexadecimal digits.
        /// </summary>
        /// <param name="input">The <see cref="T:byte[]"/> to be converted into a <see cref="string"/>.</param>
        /// <returns>A <see cref="string"/> of hexadecimal pairs that is equivalent to <paramref name="input"/>.</returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="input"/> cannot be null.
        /// </exception>
        public string ChangeType(byte[] input)
        {
            Validator.ThrowIfNull(input, nameof(input));
            return BitConverter.ToString(input).Replace("-", "").ToLowerInvariant();
        }
    }
}