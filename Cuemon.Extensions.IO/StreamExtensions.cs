using System;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using Cuemon.IO;
using Cuemon.Text;

namespace Cuemon.Extensions.IO
{
    /// <summary>
    /// Extension methods for the <see cref="Stream"/> class.
    /// </summary>
    public static class StreamExtensions
    {
        /// <summary>
        /// Asynchronously writes a sequence of bytes to the current stream with the entire size of the <paramref name="buffer"/> starting from position 0.
        /// </summary>
        /// <param name="stream">The <see cref="Stream"/> to extend.</param>
        /// <param name="buffer">The buffer to write data from.</param>
        /// <returns>A task that represents the asynchronous write operation.</returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="buffer">buffer</paramref> is null.
        /// </exception>
        /// <exception cref="NotSupportedException">
        /// The stream does not support writing.
        /// </exception>
        /// <exception cref="ObjectDisposedException">
        /// The stream has been disposed.
        /// </exception>
        /// <exception cref="InvalidOperationException">
        /// The stream is currently in use by a previous write operation.
        /// </exception>
        public static Task WriteAsync(this Stream stream, byte[] buffer)
        {
            return stream.WriteAsync(buffer, 0, buffer.Length);
        }

        /// <summary>
        /// Tries to resolve the Unicode <see cref="Encoding"/> object from the specified <see cref="Stream"/> object.
        /// </summary>
        /// <param name="value">The <see cref="Stream"/> to extend.</param>
        /// <param name="result">When this method returns, it contains the Unicode <see cref="Encoding"/> value equivalent to the encoding contained in <paramref name="value"/>, if the conversion succeeded, or a null reference (Nothing in Visual Basic) if the conversion failed. The conversion fails if the <paramref name="value"/> parameter is null, or does not contain a Unicode representation of an <see cref="Encoding"/>.</param>
        /// <returns><c>true</c> if the <paramref name="value"/> parameter was converted successfully; otherwise, <c>false</c>.</returns>
        public static bool TryDetectUnicodeEncoding(this Stream value, out Encoding result)
        {
            return StreamUtility.TryDetectUnicodeEncoding(value, out result);
        }

        /// <summary>
        /// Converts the given <see cref="Stream"/> to a char array starting from position 0 (when supported).
        /// </summary>
        /// <param name="value">The <see cref="Stream"/> to extend.</param>
        /// <param name="setup">The <see cref="EncodingOptions"/> which need to be configured.</param>
        /// <returns>A sequence of characters equivalent to the <see cref="Stream"/> value.</returns>
        /// <remarks><see cref="EncodingOptions"/> will be initialized with <see cref="EncodingOptions.DefaultPreambleSequence"/> and <see cref="EncodingOptions.DefaultEncoding"/>.</remarks>
        public static char[] ToCharArray(this Stream value, Action<EncodingOptions> setup = null)
        {
            return CharConverter.FromStream(value, setup);
        }

        /// <summary>
        /// Converts the specified <paramref name="value"/> to a string using the provided preferred encoding.
        /// </summary>
        /// <param name="value">The <see cref="Stream"/> to extend.</param>
        /// <param name="setup">The <see cref="StreamEncodingOptions"/> which need to be configured.</param>
        /// <returns>A <see cref="string"/> containing the decoded result of the specified <paramref name="value"/>.</returns>
        /// <remarks><see cref="EncodingOptions"/> will be initialized with <see cref="EncodingOptions.DefaultPreambleSequence"/> and <see cref="EncodingOptions.DefaultEncoding"/>.</remarks>
        public static string ToEncodedString(this Stream value, Action<StreamEncodingOptions> setup = null)
        {
            return StringConverter.FromStream(value, setup);
        }

        /// <summary>
        /// Converts the specified <paramref name="value"/> to a byte array.
        /// </summary>
        /// <param name="value">The <see cref="Stream"/> to extend.</param>
        /// <param name="leaveOpen">if <c>true</c>, the <see cref="Stream"/> object is being left open; otherwise it is being closed and disposed.</param>
        /// <returns>A <b>byte array</b> containing the data from the stream.</returns>
        public static byte[] ToByteArray(this Stream value, bool leaveOpen = false)
        {
            return ByteConverter.FromStream(value, leaveOpen);
        }

        /// <summary>
        /// Compresses the <paramref name="value"/> using the <see cref="CompressionType.Deflate"/> algorithm.
        /// </summary>
        /// <param name="value">The <see cref="Stream"/> to extend.</param>
        /// <param name="bufferSize">The size of the buffer. This value must be greater than zero. The default size is 81920.</param>
        /// <returns>A Deflate compressed <see cref="Stream"/> of the <paramref name="value"/>.</returns>
        public static Stream Deflate(this Stream value, int bufferSize = 81920)
        {
            return StreamUtility.CompressDeflate(value, bufferSize);
        }

        /// <summary>
        /// Compresses the <paramref name="value"/> using the <see cref="CompressionType.GZip"/> algorithm.
        /// </summary>
        /// <param name="value">The <see cref="Stream"/> to extend.</param>
        /// <param name="bufferSize">The size of the buffer. This value must be greater than zero. The default size is 81920.</param>
        /// <returns>A GZip compressed <see cref="Stream"/> of the <paramref name="value"/>.</returns>
        public static Stream GZip(this Stream value, int bufferSize = 81920)
        {
            return StreamUtility.CompressGZip(value, bufferSize);
        }

        /// <summary>
        /// Decompresses the source stream using <see cref="CompressionType.Deflate"/> algorithm.
        /// </summary>
        /// <param name="value">The <see cref="Stream"/> to extend.</param>
        /// <param name="bufferSize">The size of the buffer. This value must be greater than zero. The default size is 81920.</param>
        /// <returns>A decompressed <see cref="Stream"/> of the <paramref name="value"/>.</returns>
        public static Stream FromDeflate(this Stream value, int bufferSize = 81920)
        {
            return StreamUtility.DecompressDeflate(value, bufferSize);
        }

        /// <summary>
        /// Decompresses the source stream using <see cref="CompressionType.GZip"/> algorithm.
        /// </summary>
        /// <param name="value">The <see cref="Stream"/> to extend.</param>
        /// <param name="bufferSize">The size of the buffer. This value must be greater than zero. The default size is 81920.</param>
        /// <returns>A decompressed <see cref="Stream"/> of the <paramref name="value"/>.</returns>
        public static Stream FromGZip(this Stream value, int bufferSize = 81920)
        {
            return StreamUtility.DecompressGZip(value, bufferSize);
        }

        /// <summary>
        /// Removes the preamble information (if present) from the specified <see cref="Stream"/>, and <paramref name="value"/> is being closed and disposed.
        /// </summary>
        /// <param name="value">The <see cref="Stream"/> to extend.</param>
        /// <param name="encoding">The encoding to use when determining the preamble to remove.</param>
        /// <returns>A <see cref="Stream"/> without preamble information.</returns>
        public static Stream RemovePreamble(this Stream value, Encoding encoding)
        {
            return StreamConverter.RemovePreamble(value, encoding);
        }
    }
}