using System;
using System.IO;
using System.Text;

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
            return FromStream(value, PreambleSequence.Keep);
        }

        /// <summary>
        /// Converts the given <see cref="Stream"/> to a char array starting from position 0 (when supported), using UTF-16 for the encoding.
        /// </summary>
        /// <param name="value">The <see cref="Stream"/> value to be converted.</param>
        /// <param name="sequence">Determines whether too keep or remove any preamble sequences.</param>
        /// <returns>A sequence of characters equivalent to the <see cref="Stream"/> value.</returns>
        public static char[] FromStream(Stream value, PreambleSequence sequence)
        {
            return FromStream(value, sequence, Encoding.Unicode);
        }

        /// <summary>
        /// Converts the given <see cref="Stream"/> to a char array starting from position 0 (when supported).
        /// </summary>
        /// <param name="value">The <see cref="Stream"/> value to be converted.</param>
        /// <param name="sequence">Determines whether too keep or remove any preamble sequences.</param>
        /// <param name="encoding">The preferred encoding to apply to the result.</param>
        /// <returns>A sequence of characters equivalent to the <see cref="Stream"/> value.</returns>
        public static char[] FromStream(Stream value, PreambleSequence sequence, Encoding encoding)
        {
            Validator.ThrowIfNull(value, nameof(value));
            Validator.ThrowIfNull(encoding, nameof(encoding));

            byte[] valueInBytes = ByteConverter.FromStream(value);
            switch (sequence)
            {
                case PreambleSequence.Keep:
                    break;
                case PreambleSequence.Remove:
                    valueInBytes = ByteUtility.RemovePreamble(valueInBytes, encoding);
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(sequence));
            }
            return encoding.GetChars(valueInBytes);
        }

        /// <summary>
        /// Converts the given <see cref="string"/> to an equivalent sequence of characters using UTF-16 for the encoding and preserving any preamble sequences.
        /// </summary>
        /// <param name="value">The <see cref="string"/> value to be converted.</param>
        /// <returns>A sequence of characters equivalent to the <see cref="string"/> value.</returns>
        public static char[] FromString(string value)
        {
            return FromString(value, PreambleSequence.Keep);
        }

        /// <summary>
        /// Converts the given <see cref="string"/> to an equivalent sequence of characters using UTF-16 for the encoding.
        /// </summary>
        /// <param name="value">The <see cref="string"/> value to be converted.</param>
        /// <param name="sequence">Determines whether too keep or remove any preamble sequences.</param>
        /// <returns>A sequence of characters equivalent to the <see cref="string"/> value.</returns>
        public static char[] FromString(string value, PreambleSequence sequence)
        {
            return FromString(value, sequence, Encoding.Unicode);
        }

        /// <summary>
        /// Converts the given <see cref="string"/> to an equivalent sequence of characters.
        /// </summary>
        /// <param name="value">The <see cref="string"/> value to be converted.</param>
        /// <param name="sequence">Determines whether too keep or remove any preamble sequences.</param>
        /// <param name="encoding">The preferred encoding to apply to the result.</param>
        /// <returns>A sequence of characters equivalent to the <see cref="string"/> value.</returns>
        public static char[] FromString(string value, PreambleSequence sequence, Encoding encoding)
        {
            Validator.ThrowIfNull(value, nameof(value));
            Validator.ThrowIfNull(encoding, nameof(encoding));
            return encoding.GetChars(ByteConverter.FromString(value, sequence, encoding));
        }
    }
}