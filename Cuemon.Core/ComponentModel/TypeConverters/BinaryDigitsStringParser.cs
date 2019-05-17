using System;

namespace Cuemon.ComponentModel.TypeConverters
{
    /// <summary>
    /// Provides a parser that converts a <see cref="string"/>, represented in binary digits, to its equivalent <see cref="T:byte[]"/>.
    /// </summary>
    public class BinaryDigitsStringParser : IParser<byte[]>
    {
        /// <summary>
        /// Converts the string representation of binary digits to its <see cref="T:byte[]"/> equivalent.
        /// </summary>
        /// <param name="input">A string containing binary digits.</param>
        /// <returns>A <see cref="T:byte[]"/> equivalent to <paramref name="input"/>.</returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="input"/> cannot be null.
        /// </exception>
        /// <exception cref="ArgumentException">
        /// <paramref name="input"/> cannot be empty or consist only of white-space characters.
        /// </exception>
        /// <exception cref="FormatException">
        /// <paramref name="input"/> must consist only of binary digits.
        /// </exception>
        /// <seealso cref="BinaryDigitsStringConverter"/>
        public byte[] Parse(string input)
        {
            try
            {
                return Converter.FromString.ToByteArray<BinaryDigitsStringConverter>(input);
            }
            catch (ArgumentOutOfRangeException)
            {
                throw new FormatException(FormattableString.Invariant($"The format of {nameof(input)} must consist only of binary digits."));
            }
        }

        /// <summary>
        /// Converts the string representation of binary digits to its <see cref="T:byte[]"/> equivalent. A return value indicates whether the conversion succeeded.
        /// </summary>
        /// <param name="input">A string consisting of URL-safe base64 characters.</param>
        /// <param name="result">When this method returns, contains the <see cref="T:byte[]"/> equivalent of the binary digits contained within <paramref name="input"/>, if the conversion succeeded, or <c>default</c> if the conversion failed.</param>
        /// <returns><c>true</c> if <paramref name="input"/> was converted successfully; otherwise, <c>false</c>.</returns>
        public bool TryParse(string input, out byte[] result)
        {
            return Patterns.TryInvoke(() => Parse(input), out result);
        }
    }
}