using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Cuemon
{
    /// <summary>
    /// This is an extension implementation of the <see cref="CharConverter"/> class.
    /// </summary>
    public static class CharConverterExtension
    {
        /// <summary>
        /// Converts the given <see cref="Stream"/> to a char array starting from position 0 (when supported), using UTF-16 for the encoding preserving any preamble sequences.
        /// </summary>
        /// <param name="value">The <see cref="Stream"/> value to be converted.</param>
        /// <returns>A sequence of characters equivalent to the <see cref="Stream"/> value.</returns>
        public static char[] ToCharArray(this Stream value)
        {
            return CharConverter.FromStream(value);
        }

        /// <summary>
        /// Converts the given <see cref="Stream"/> to a char array starting from position 0 (when supported), using UTF-16 for the encoding.
        /// </summary>
        /// <param name="value">The <see cref="Stream"/> value to be converted.</param>
        /// <param name="sequence">Determines whether too keep or remove any preamble sequences.</param>
        /// <returns>A sequence of characters equivalent to the <see cref="Stream"/> value.</returns>
        public static char[] ToCharArray(this Stream value, PreambleSequence sequence)
        {
            return CharConverter.FromStream(value, sequence);
        }

        /// <summary>
        /// Converts the given <see cref="Stream"/> to a char array starting from position 0 (when supported).
        /// </summary>
        /// <param name="value">The <see cref="Stream"/> value to be converted.</param>
        /// <param name="sequence">Determines whether too keep or remove any preamble sequences.</param>
        /// <param name="encoding">The preferred encoding to apply to the result.</param>
        /// <returns>A sequence of characters equivalent to the <see cref="Stream"/> value.</returns>
        public static char[] ToCharArray(this Stream value, PreambleSequence sequence, Encoding encoding)
        {
            return CharConverter.FromStream(value, sequence, encoding);
        }

        /// <summary>
        /// Converts the given <see cref="String"/> to an equivalent sequence of characters using UTF-16 for the encoding and preserving any preamble sequences.
        /// </summary>
        /// <param name="value">The <see cref="String"/> value to be converted.</param>
        /// <returns>A sequence of characters equivalent to the <see cref="String"/> value.</returns>
        public static char[] ToCharArray(this string value)
        {
            return CharConverter.FromString(value);
        }

        /// <summary>
        /// Converts the given <see cref="String"/> to an equivalent sequence of characters using UTF-16 for the encoding.
        /// </summary>
        /// <param name="value">The <see cref="String"/> value to be converted.</param>
        /// <param name="sequence">Determines whether too keep or remove any preamble sequences.</param>
        /// <returns>A sequence of characters equivalent to the <see cref="String"/> value.</returns>
        public static char[] ToCharArray(this string value, PreambleSequence sequence)
        {
            return CharConverter.FromString(value, sequence);
        }

        /// <summary>
        /// Converts the given <see cref="String"/> to an equivalent sequence of characters.
        /// </summary>
        /// <param name="value">The <see cref="String"/> value to be converted.</param>
        /// <param name="sequence">Determines whether too keep or remove any preamble sequences.</param>
        /// <param name="encoding">The preferred encoding to apply to the result.</param>
        /// <returns>A sequence of characters equivalent to the <see cref="String"/> value.</returns>
        public static char[] ToCharArray(this string value, PreambleSequence sequence, Encoding encoding)
        {
            return CharConverter.FromString(value, sequence, encoding);
        }
    }
}