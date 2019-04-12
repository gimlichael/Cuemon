using System;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using Cuemon.Text;

namespace Cuemon.IO
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
        /// <param name="stream">The <see cref="Stream"/> object to resolve the Unicode <see cref="Encoding"/> object from.</param>
        /// <param name="result">When this method returns, it contains the Unicode <see cref="Encoding"/> value equivalent to the encoding contained in <paramref name="stream"/>, if the conversion succeeded, or a null reference (Nothing in Visual Basic) if the conversion failed. The conversion fails if the <paramref name="stream"/> parameter is null, or does not contain a Unicode representation of an <see cref="Encoding"/>.</param>
        /// <returns><c>true</c> if the <paramref name="stream"/> parameter was converted successfully; otherwise, <c>false</c>.</returns>
        public static bool TryDetectUnicodeEncoding(this Stream stream, out Encoding result)
        {
            Validator.ThrowIfNull(stream, nameof(stream));
            if (!stream.CanSeek)
            {
                result = null;
                return false;
            }

            byte[] byteOrderMarks = new byte[] { 0, 0, 0, 0 };

            long startingPosition = stream.Position;
            stream.Position = 0;
            stream.Read(byteOrderMarks, 0, 4); // only read the first 4 bytes
            stream.Seek(startingPosition, SeekOrigin.Begin); // reset to original position}

            return byteOrderMarks.TryDetectUnicodeEncoding(out result);
        }
    }
}