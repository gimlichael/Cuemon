using System;
using System.IO;
using System.Text;
using Cuemon.Text;

namespace Cuemon.IO
{
    /// <summary>
    /// This utility class is designed to make <see cref="Stream"/> related conversions easier to work with.
    /// </summary>
    public static class StreamConverter
    {
        /// <summary>
        /// Removes the preamble information (if present) from the specified <see cref="Stream"/>, and <paramref name="value"/> is being closed and disposed.
        /// </summary>
        /// <param name="value">The input <see cref="Stream"/> to process.</param>
        /// <param name="encoding">The encoding to use when determining the preamble to remove.</param>
        /// <param name="leaveOpen">if <c>true</c>, the <see cref="Stream"/> object is being left open; otherwise it is being closed and disposed.</param>
        /// <returns>A <see cref="Stream"/> without preamble information.</returns>
        public static Stream RemovePreamble(Stream value, Encoding encoding, bool leaveOpen = false)
        {
            Validator.ThrowIfNull(value, nameof(value));
            Validator.ThrowIfNull(encoding, nameof(encoding));
            byte[] bytes = ByteConverter.FromStream(value, leaveOpen);
            bytes = ByteArrayUtility.RemovePreamble(bytes, encoding);
            return Disposable.SafeInvoke(() => new MemoryStream(bytes.Length), ms => ms.Write(bytes, 0, bytes.Length));
        }

        /// <summary>
        /// Converts the specified <paramref name="value"/> to a <see cref="Stream"/>.
        /// </summary>
        /// <param name="value">The byte array to be converted.</param>
        /// <returns>A <see cref="Stream"/> object.</returns>
        public static Stream FromBytes(byte[] value)
        {
            Validator.ThrowIfNull(value, nameof(value));
            return Disposable.SafeInvoke(() => new MemoryStream(value.Length), ms => ms.Write(value, 0, value.Length));
        }

        /// <summary>
        /// Converts the specified <paramref name="value"/> to a <see cref="Stream"/>.
        /// </summary>
        /// <param name="value">The string to be converted.</param>
        /// <param name="setup">The <see cref="EncodingOptions"/> which need to be configured.</param>
        /// <returns>A <b><see cref="Stream"/></b> object.</returns>
        /// <remarks><see cref="EncodingOptions"/> will be initialized with <see cref="EncodingOptions.DefaultPreambleSequence"/> and <see cref="EncodingOptions.DefaultEncoding"/>.</remarks>
        public static Stream FromString(string value, Action<EncodingOptions> setup = null)
        {
            Validator.ThrowIfNull(value, nameof(value));
            return Disposable.SafeInvoke(() => new MemoryStream(), ms =>
            {
                var bytes = ByteConverter.FromString(value, setup);
                ms.Write(bytes, 0, bytes.Length);
            });
        }
    }
}