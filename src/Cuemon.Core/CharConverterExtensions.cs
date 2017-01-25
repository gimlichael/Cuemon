using System;
using System.IO;
using Cuemon.Text;

namespace Cuemon
{
    /// <summary>
    /// This is an extensions implementation of the <see cref="CharConverter"/> class.
    /// </summary>
    public static class CharConverterExtensions
    {
        /// <summary>
        /// Converts the given <see cref="Stream"/> to a char array starting from position 0 (when supported).
        /// </summary>
        /// <param name="value">The <see cref="Stream"/> value to be converted.</param>
        /// <param name="setup">The <see cref="EncodingOptions"/> which need to be configured.</param>
        /// <returns>A sequence of characters equivalent to the <see cref="Stream"/> value.</returns>
        /// <remarks><see cref="EncodingOptions"/> will be initialized with <see cref="EncodingOptions.DefaultPreambleSequence"/> and <see cref="EncodingOptions.DefaultEncoding"/>.</remarks>
        public static char[] ToCharArray(this Stream value, Action<EncodingOptions> setup = null)
        {
            return CharConverter.FromStream(value, setup);
        }

        /// <summary>
        /// Converts the given <see cref="String"/> to an equivalent sequence of characters.
        /// </summary>
        /// <param name="value">The <see cref="String"/> value to be converted.</param>
        /// <param name="setup">The <see cref="EncodingOptions"/> which need to be configured.</param>
        /// <returns>A sequence of characters equivalent to the <see cref="String"/> value.</returns>
        /// <remarks><see cref="EncodingOptions"/> will be initialized with <see cref="EncodingOptions.DefaultPreambleSequence"/> and <see cref="EncodingOptions.DefaultEncoding"/>.</remarks>
        public static char[] ToCharArray(this string value, Action<EncodingOptions> setup = null)
        {
            return CharConverter.FromString(value, setup);
        }
    }
}