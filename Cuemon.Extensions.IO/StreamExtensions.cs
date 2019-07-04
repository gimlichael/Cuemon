using System;
using System.ComponentModel;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Cuemon.ComponentModel.Codecs;
using Cuemon.ComponentModel.TypeConverters;
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
        /// Combines a variable number of streams into one stream.
        /// </summary>
        /// <param name="streams">The streams to combine.</param>
        /// <param name="leaveOpen">if <c>true</c>, each of the <see cref="Stream"/> in the <paramref name="streams"/> sequence is being left open; otherwise it is being closed and disposed.</param>
        /// <returns>A variable number of <b>streams</b> combined into one <b>stream</b>.</returns>
        public static Stream Concat(this Stream first, Stream second, Action<DisposableOptions> setup = null)
        {
            Validator.ThrowIfNull(first, nameof(first));
            Validator.ThrowIfNull(second, nameof(second));
            var options = Patterns.Configure(setup);
            var result = new MemoryStream();
            try
            {
                first.CopyTo(result);
                second.CopyTo(result);
            }
            finally
            {
                if (!options.LeaveOpen)
                {
                    first.Dispose();
                    second.Dispose();
                }
            }
            result.Flush();
            result.Position = 0;
            return result;
        }

        /// <summary>
        /// Converts the specified <paramref name="input"/> to its equivalent <see cref="T:char[]"/> representation.
        /// </summary>
        /// <param name="input">The <see cref="Stream"/> to extend.</param>
        /// <param name="setup">The <see cref="EncodingOptions"/> which may be configured.</param>
        /// <returns>A <see cref="T:char[]"/> that is equivalent to <paramref name="input"/>.</returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="input"/> cannot be null.
        /// </exception>
        /// <exception cref="ArgumentException">
        /// <paramref name="input"/> cannot be read from.
        /// </exception>
        /// <exception cref="ArgumentOutOfRangeException">
        /// <paramref name="input" /> length is greater than <see cref="int.MaxValue"/>.
        /// </exception>
        /// <exception cref="InvalidEnumArgumentException">
        /// <paramref name="setup"/> was initialzied with an invalid <see cref="EncodingOptions.Preamble"/>.
        /// </exception>
        /// <seealso cref="EncodedStreamConverter"/>
        /// <seealso cref="EncodingOptions"/>
        
        public static char[] ToCharArray(this Stream input, Action<EncodingOptions> setup = null)
        {
            return ConvertFactory.UseConverter<EncodedStreamConverter>().ChangeType(input, setup);
        }

        /// <summary>
        /// Converts the specified <paramref name="input"/> to its equivalent <see cref="T:byte[]"/> representation.
        /// </summary>
        /// <param name="input">The <see cref="Stream"/> to extend.</param>
        /// <param name="setup">The <see cref="DisposableOptions"/> which may be configured.</param>
        /// <returns>A <see cref="T:byte[]"/> that is equivalent to <paramref name="input"/>.</returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="input"/> cannot be null.
        /// </exception>
        /// <exception cref="ArgumentException">
        /// <paramref name="input"/> cannot be read from.
        /// </exception>
        /// <exception cref="ArgumentOutOfRangeException">
        /// <paramref name="input" /> length is greater than <see cref="int.MaxValue"/>.
        /// </exception>
        /// <seealso cref="StreamToByteArrayCodec"/>
        /// <seealso cref="DisposableOptions"/>
        public static byte[] ToByteArray(this Stream input, Action<DisposableOptions> setup = null)
        {
            return ConvertFactory.UseCodec<StreamToByteArrayCodec>().Encode(input, setup);
        }

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
            return ByteOrderMark.TryDetectEncoding(value, out result);
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
            return ConvertFactory.UseCodec<StreamToStringCodec>().Encode(value, setup);
        }

        /// <summary>
        /// Compresses the <paramref name="value"/> using the <see cref="CompressionType.Deflate"/> algorithm.
        /// </summary>
        /// <param name="value">The <see cref="Stream"/> to extend.</param>
        /// <param name="bufferSize">The size of the buffer. This value must be greater than zero. The default size is 81920.</param>
        /// <returns>A Deflate compressed <see cref="Stream"/> of the <paramref name="value"/>.</returns>
        public static Stream Deflate(this Stream value, Action<StreamCompressionOptions> setup = null)
        {
            return ConvertFactory.UseEncoder<DeflateStreamCodec>().Encode(value, setup);
        }

        /// <summary>
        /// Compresses the <paramref name="value"/> using the <see cref="CompressionType.GZip"/> algorithm.
        /// </summary>
        /// <param name="value">The <see cref="Stream"/> to extend.</param>
        /// <param name="bufferSize">The size of the buffer. This value must be greater than zero. The default size is 81920.</param>
        /// <returns>A GZip compressed <see cref="Stream"/> of the <paramref name="value"/>.</returns>
        public static Stream GZip(this Stream value, Action<StreamCompressionOptions> setup = null)
        {
            return ConvertFactory.UseEncoder<GZipStreamCodec>().Encode(value, setup);
        }

        /// <summary>
        /// Decompresses the source stream using <see cref="CompressionType.Deflate"/> algorithm.
        /// </summary>
        /// <param name="value">The <see cref="Stream"/> to extend.</param>
        /// <param name="bufferSize">The size of the buffer. This value must be greater than zero. The default size is 81920.</param>
        /// <returns>A decompressed <see cref="Stream"/> of the <paramref name="value"/>.</returns>
        public static Stream FromDeflate(this Stream value, Action<StreamCopyOptions> setup = null)
        {
            return ConvertFactory.UseDecoder<DeflateStreamCodec>().Decode(value, setup);
        }

        /// <summary>
        /// Decompresses the source stream using <see cref="CompressionType.GZip"/> algorithm.
        /// </summary>
        /// <param name="value">The <see cref="Stream"/> to extend.</param>
        /// <param name="bufferSize">The size of the buffer. This value must be greater than zero. The default size is 81920.</param>
        /// <returns>A decompressed <see cref="Stream"/> of the <paramref name="value"/>.</returns>
        public static Stream FromGZip(this Stream value, Action<StreamCopyOptions> setup = null)
        {
            return ConvertFactory.UseDecoder<GZipStreamCodec>().Decode(value, setup);
        }

    }
}