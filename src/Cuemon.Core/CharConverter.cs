using System;
using System.IO;
using System.Text;
using Cuemon.Text;

namespace Cuemon
{
    /// <summary>
    /// This utility class is designed to make <see cref="Char"/> related conversions easier to work with.
    /// </summary>
    public static class CharConverter
    {
        /// <summary>
        /// Converts the given <see cref="Stream"/> to a char array starting from position 0 (when supported), using UTF-16 for the encoding preserving any preamble sequences.
        /// </summary>
        /// <param name="value">The <see cref="Stream"/> value to be converted.</param>
        /// <returns>A sequence of characters equivalent to the <see cref="Stream"/> value.</returns>
        public static char[] FromStream(Stream value)
        {
            return FromStream(value, options =>
            {
                options.Encoding = Encoding.Unicode;
                options.Preamble = PreambleSequence.Keep;
            });
        }

        /// <summary>
        /// Converts the given <see cref="Stream"/> to a char array starting from position 0 (when supported).
        /// </summary>
        /// <param name="value">The <see cref="Stream"/> value to be converted.</param>
        /// <param name="setup">The <see cref="EncodingOptions"/> which need to be configured.</param>
        /// <returns>A sequence of characters equivalent to the <see cref="Stream"/> value.</returns>
        public static char[] FromStream(Stream value, Action<EncodingOptions> setup)
        {
            Validator.ThrowIfNull(value, nameof(value));
            var options = setup.ConfigureOptions();

            byte[] valueInBytes = ByteConverter.FromStream(value);
            switch (options.Preamble)
            {
                case PreambleSequence.Keep:
                    break;
                case PreambleSequence.Remove:
                    valueInBytes = ByteUtility.RemovePreamble(valueInBytes, options.Encoding);
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(options));
            }
            return options.Encoding.GetChars(valueInBytes);
        }

        /// <summary>
        /// Converts the given <see cref="string"/> to an equivalent sequence of characters using UTF-16 for the encoding and preserving any preamble sequences.
        /// </summary>
        /// <param name="value">The <see cref="string"/> value to be converted.</param>
        /// <returns>A sequence of characters equivalent to the <see cref="string"/> value.</returns>
        public static char[] FromString(string value)
        {
            return FromString(value, options =>
            {
                options.Encoding = Encoding.Unicode;
                options.Preamble = PreambleSequence.Keep;
            });
        }

        /// <summary>
        /// Converts the given <see cref="string"/> to an equivalent sequence of characters.
        /// </summary>
        /// <param name="value">The <see cref="string"/> value to be converted.</param>
        /// <param name="setup">The <see cref="EncodingOptions"/> which need to be configured.</param>
        /// <returns>A sequence of characters equivalent to the <see cref="string"/> value.</returns>
        public static char[] FromString(string value, Action<EncodingOptions> setup)
        {
            Validator.ThrowIfNull(value, nameof(value));
            var options = setup.ConfigureOptions();
            return options.Encoding.GetChars(ByteConverter.FromString(value, setup));
        }
    }
}