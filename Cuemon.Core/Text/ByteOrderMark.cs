using System;
using System.IO;
using System.Text;
using Cuemon.ComponentModel.Codecs;
using Cuemon.ComponentModel.Decoders;

namespace Cuemon.Text
{
    /// <summary>
    /// Provides a set of static methods for Unicode related operations.
    /// </summary>
    public class ByteOrderMark
    {
        /// <summary>
        /// Tries to detect an <see cref="Encoding"/> object from the specified <paramref name="input"/>.
        /// If unsuccessful, the <paramref name="fallbackEncoding"/> value is returned. Default is <see cref="EncodingOptions.DefaultEncoding"/>.
        /// </summary>
        /// <param name="input">The <see cref="T:byte[]"/> to parse for an <see cref="Encoding"/>.</param>
        /// <param name="fallbackEncoding">The <see cref="Encoding"/> to use when conversion is unsuccessful.</param>
        /// <returns>Either the detected encoding of <paramref name="input"/>  or the <paramref name="fallbackEncoding"/> encoding.</returns>
        public static Encoding DetectEncodingOrDefault(byte[] input, Encoding fallbackEncoding)
        {
            if (TryDetectEncoding(input, out var result))
            {
                return result;
            }
            return fallbackEncoding ?? EncodingOptions.DefaultEncoding;
        }

        /// <summary>
        /// Tries to resolve the Unicode <see cref="Encoding"/> object from the specified <see cref="T:byte[]"/>.
        /// </summary>
        /// <param name="input">The <see cref="T:byte[]"/> to resolve the Unicode <see cref="Encoding"/> object from.</param>
        /// <param name="result">When this method returns, it contains the Unicode <see cref="Encoding"/> value equivalent to the encoding contained in <paramref name="input"/>, if the conversion succeeded, or a null reference (Nothing in Visual Basic) if the conversion failed. The conversion fails if the <paramref name="input"/> parameter is null, or does not contain a Unicode representation of an <see cref="Encoding"/>.</param>
        /// <returns><c>true</c> if the <paramref name="input"/> parameter was converted successfully; otherwise, <c>false</c>.</returns>
        public static bool TryDetectEncoding(byte[] input, out Encoding result)
        {
            return Patterns.TryInvoke(() => ConvertFactory.UseDecoder<ByteOrderMarkDecoder>().Decode(input), out result);
        }


        /// <summary>
        /// Tries to detect an <see cref="Encoding"/> object from the specified <paramref name="value"/>.
        /// If unsuccessful, the <paramref name="fallbackEncoding"/> value is returned.
        /// </summary>
        /// <param name="value">The <see cref="Stream"/> to parse for an <see cref="Encoding"/>.</param>
        /// <param name="fallbackEncoding">The <see cref="Encoding"/> to use when conversion is unsuccessful.</param>
        /// <returns>Either the detected encoding of <paramref name="value"/>  or the <paramref name="fallbackEncoding"/> encoding.</returns>
        public static Encoding DetectEncodingOrDefault(Stream value, Encoding fallbackEncoding)
        {
            if (TryDetectEncoding(value, out var result))
            {
                return result;
            }
            return fallbackEncoding;
        }

        /// <summary>
        /// Tries to resolve the Unicode <see cref="Encoding"/> object from the specified <see cref="Stream"/> object.
        /// </summary>
        /// <param name="value">The <see cref="Stream"/> object to resolve the Unicode <see cref="Encoding"/> object from.</param>
        /// <param name="result">When this method returns, it contains the Unicode <see cref="Encoding"/> value equivalent to the encoding contained in <paramref name="value"/>, if the conversion succeeded, or a null reference (Nothing in Visual Basic) if the conversion failed. The conversion fails if the <paramref name="value"/> parameter is null, or does not contain a Unicode representation of an <see cref="Encoding"/>.</param>
        /// <returns><c>true</c> if the <paramref name="value"/> parameter was converted successfully; otherwise, <c>false</c>.</returns>
        public static bool TryDetectEncoding(Stream value, out Encoding result)
        {
            Validator.ThrowIfNull(value, nameof(value));
            if (!value.CanSeek)
            {
                result = null;
                return false;
            }

            byte[] byteOrderMarks = { 0, 0, 0, 0 };

            var startingPosition = value.Position;
            value.Position = 0;
            value.Read(byteOrderMarks, 0, 4); // only read the first 4 bytes
            value.Seek(startingPosition, SeekOrigin.Begin); // reset to original position}

            return TryDetectEncoding(byteOrderMarks, out result);
        }

        /// <summary>
        /// Removes the preamble information (if present) from the specified <see cref="Stream"/>.
        /// </summary>
        /// <param name="value">The input <see cref="Stream"/> to process.</param>
        /// <param name="encoding">The encoding to use when determining the preamble to remove.</param>
        /// <param name="leaveOpen">if <c>true</c>, the <see cref="Stream"/> object is being left open; otherwise it is being closed and disposed.</param>
        /// <returns>A <see cref="Stream"/> without preamble information.</returns>
        public static Stream Remove(Stream input, Encoding encoding, Action<DisposableOptions> setup = null)
        {
            Validator.ThrowIfNull(input, nameof(input));
            Validator.ThrowIfNull(encoding, nameof(encoding));
            var option = Patterns.Configure(setup);

            var bytes = ConvertFactory.UseCodec<StreamToByteArrayCodec>().Encode(input, o => o.LeaveOpen = option.LeaveOpen);
            bytes = Remove(bytes, encoding);
            return Disposable.SafeInvoke(() => new MemoryStream(bytes.Length), ms =>
            {
                ms.Write(bytes, 0, bytes.Length);
                ms.Position = 0;
                return ms;
            });
        }

        /// <summary>
        /// Removes the preamble information (if present) from the specified <see cref="T:byte[]"/>.
        /// </summary>
        /// <param name="bytes">The input <see cref="T:byte[]"/>  to process.</param>
        /// <param name="encoding">The encoding to use when determining the preamble to remove.</param>
        /// <returns>A <see cref="byte"/> array without preamble information.</returns>
        public static byte[] Remove(byte[] bytes, Encoding encoding)
        {
            Validator.ThrowIfNull(bytes, nameof(bytes));
            Validator.ThrowIfNull(encoding, nameof(encoding));
            if (bytes.Length <= 1) { return bytes; }
            var preamble = encoding.GetPreamble();
            if (preamble.Length == 0) { return bytes; }
            if ((preamble[0] == bytes[0] && preamble[1] == bytes[1]) || (preamble[0] == bytes[1] && preamble[1] == bytes[0]))
            {
                var bytesToRead = bytes.Length;
                bytesToRead -= preamble.Length;
                var bytesWithNoPreamble = new byte[bytesToRead];
                Array.Copy(bytes, preamble.Length, bytesWithNoPreamble, 0, bytesToRead);
                return bytesWithNoPreamble;
            }
            return bytes;
        }
    }
}
