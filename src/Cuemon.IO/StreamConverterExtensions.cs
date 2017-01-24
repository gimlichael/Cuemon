using System;
using System.IO;
using System.Text;
using Cuemon.Text;

namespace Cuemon.IO
{
    /// <summary>
    /// This is an extension implementation of the <see cref="StreamConverter"/> class.
    /// </summary>
    public static class StreamConverterExtensions
    {
        /// <summary>
        /// Removes the preamble information (if present) from the specified <see cref="Stream"/>, and <paramref name="source"/> is being closed and disposed.
        /// </summary>
        /// <param name="source">The input <see cref="Stream"/> to process.</param>
        /// <param name="encoding">The encoding to use when determining the preamble to remove.</param>
        /// <returns>A <see cref="Stream"/> without preamble information.</returns>
        public static Stream RemovePreamble(this Stream source, Encoding encoding)
        {
            return StreamConverter.RemovePreamble(source, encoding);
        }

        /// <summary>
        /// Converts the specified <paramref name="value"/> to a <see cref="Stream"/>.
        /// </summary>
        /// <param name="value">The byte array to be converted.</param>
        /// <returns>A <see cref="System.IO.Stream"/> object.</returns>
        public static Stream ToStream(this byte[] value)
        {
            return StreamConverter.FromBytes(value);
        }

        /// <summary>
        /// Converts the specified <paramref name="value"/> to a <see cref="Stream"/>.
        /// </summary>
        /// <param name="value">The string to be converted.</param>
        /// <param name="setup">The <see cref="EncodingOptions"/> which need to be configured.</param>
        /// <returns>A <b><see cref="System.IO.Stream"/></b> object.</returns>
        public static Stream ToStream(this string value, Action<EncodingOptions> setup = null)
        {
            return StreamConverter.FromString(value, setup);
        }
    }
}