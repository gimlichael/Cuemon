using System;
using System.ComponentModel;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using Cuemon.ComponentModel;
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
        /// <param name="first">The <see cref="Stream"/> to extend.</param>
        /// <param name="second">The <see cref="Stream"/> to concat with <paramref name="first"/>.</param>
        /// <param name="setup">The <see cref="DisposableOptions"/> which may be configured.</param>
        /// <returns>The concatenated <see cref="Stream"/> representations of values <paramref name="first"/> and <paramref name="second"/>.</returns>
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
        /// <param name="input">The <see cref="Stream"/> to be extended.</param>
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
        /// <paramref name="setup"/> was initialized with an invalid <see cref="EncodingOptions.Preamble"/>.
        /// </exception>
        /// <remarks><see cref="EncodingOptions"/> will be initialized with <see cref="EncodingOptions.DefaultPreambleSequence"/> and <see cref="EncodingOptions.DefaultEncoding"/>.</remarks>
        public static char[] ToCharArray(this Stream input, Action<EncodingOptions> setup = null)
        {
            Validator.ThrowIfNull(input, nameof(input));
            var options = Patterns.Configure(setup);
            if (options.Encoding.Equals(EncodingOptions.DefaultEncoding)) { options.Encoding = ByteOrderMark.DetectEncodingOrDefault(input, options.Encoding); }
            var valueInBytes = ConvertFactory.UseCodec<StreamByteArrayCodec>().Encode(input);
            switch (options.Preamble)
            {
                case PreambleSequence.Keep:
                    break;
                case PreambleSequence.Remove:
                    valueInBytes = ByteOrderMark.Remove(valueInBytes, options.Encoding);
                    break;
                default:
                    throw new InvalidEnumArgumentException(nameof(setup), (int)options.Preamble, typeof(PreambleSequence));
            }
            return options.Encoding.GetChars(valueInBytes);
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
        /// <seealso cref="StreamByteArrayCodec"/>
        /// <seealso cref="DisposableOptions"/>
        public static byte[] ToByteArray(this Stream input, Action<DisposableOptions> setup = null)
        {
            return ConvertFactory.UseCodec<StreamByteArrayCodec>().Encode(input, setup);
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
            return ConvertFactory.UseCodec<StreamStringCodec>().Encode(value, setup);
        }

        /// <summary>
        /// Compresses the <paramref name="value"/> using the DEFLATE algorithm.
        /// </summary>
        /// <param name="value">The <see cref="Stream"/> to extend.</param>
        /// <param name="setup">The <see cref="StreamCompressionOptions"/> which may be configured.</param>
        /// <returns>A DEFLATE compressed <see cref="Stream"/> of the <paramref name="value"/>.</returns>
        public static Stream Deflate(this Stream value, Action<StreamCompressionOptions> setup = null)
        {
            return ConvertFactory.UseEncoder<DeflateStreamCodec>().Encode(value, setup);
        }

        /// <summary>
        /// Compresses the <paramref name="value"/> using the GZIP algorithm.
        /// </summary>
        /// <param name="value">The <see cref="Stream"/> to extend.</param>
        /// <param name="setup">The <see cref="StreamCompressionOptions"/> which may be configured.</param>
        /// <returns>A GZip compressed <see cref="Stream"/> of the <paramref name="value"/>.</returns>
        public static Stream GZip(this Stream value, Action<StreamCompressionOptions> setup = null)
        {
            return ConvertFactory.UseEncoder<GZipStreamCodec>().Encode(value, setup);
        }

        /// <summary>
        /// Decompresses the source stream using DEFLATE algorithm.
        /// </summary>
        /// <param name="value">The <see cref="Stream"/> to extend.</param>
        /// <param name="setup">The <see cref="StreamCopyOptions"/> which may be configured.</param>
        /// <returns>A decompressed <see cref="Stream"/> of the <paramref name="value"/>.</returns>
        public static Stream FromDeflate(this Stream value, Action<StreamCopyOptions> setup = null)
        {
            return ConvertFactory.UseDecoder<DeflateStreamCodec>().Decode(value, setup);
        }

        /// <summary>
        /// Decompresses the source stream using GZIP algorithm.
        /// </summary>
        /// <param name="value">The <see cref="Stream"/> to extend.</param>
        /// <param name="setup">The <see cref="StreamCopyOptions"/> which may be configured.</param>
        /// <returns>A decompressed <see cref="Stream"/> of the <paramref name="value"/>.</returns>
        public static Stream FromGZip(this Stream value, Action<StreamCopyOptions> setup = null)
        {
            return ConvertFactory.UseDecoder<GZipStreamCodec>().Decode(value, setup);
        }

    }
}