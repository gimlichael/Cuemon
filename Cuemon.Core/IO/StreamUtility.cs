using System;
using System.IO;
using System.IO.Compression;
using System.Text;

namespace Cuemon.IO
{
    /// <summary>
    /// This utility class is designed to make <see cref="Stream"/> operations easier to work with.
    /// </summary>
    public static class StreamUtility
    {
        /// <summary>
        /// Tries to resolve the Unicode <see cref="Encoding"/> object from the specified <see cref="Stream"/> object.
        /// </summary>
        /// <param name="value">The <see cref="Stream"/> object to resolve the Unicode <see cref="Encoding"/> object from.</param>
        /// <param name="result">When this method returns, it contains the Unicode <see cref="Encoding"/> value equivalent to the encoding contained in <paramref name="value"/>, if the conversion succeeded, or a null reference (Nothing in Visual Basic) if the conversion failed. The conversion fails if the <paramref name="value"/> parameter is null, or does not contain a Unicode representation of an <see cref="Encoding"/>.</param>
        /// <returns><c>true</c> if the <paramref name="value"/> parameter was converted successfully; otherwise, <c>false</c>.</returns>
        public static bool TryDetectUnicodeEncoding(Stream value, out Encoding result)
        {
            Validator.ThrowIfNull(value, nameof(value));
            if (!value.CanSeek)
            {
                result = null;
                return false;
            }

            byte[] byteOrderMarks = { 0, 0, 0, 0 };

            long startingPosition = value.Position;
            value.Position = 0;
            value.Read(byteOrderMarks, 0, 4); // only read the first 4 bytes
            value.Seek(startingPosition, SeekOrigin.Begin); // reset to original position}

            return ByteUtility.TryDetectUnicodeEncoding(byteOrderMarks, out result);
        }

        /// <summary>
        /// Combines a variable number of streams into one stream.
        /// </summary>
        /// <param name="leaveOpen">if <c>true</c>, each of the <see cref="Stream"/> in the <paramref name="streams"/> sequence is being left open; otherwise it is being closed and disposed.</param>
        /// <param name="streams">The streams to combine.</param>
        /// <returns>A variable number of <b>streams</b> combined into one <b>stream</b>.</returns>
        public static Stream CombineStreams(bool leaveOpen = false, params Stream[] streams)
        {
            Validator.ThrowIfNull(streams, nameof(streams));
            var result = new MemoryStream();
            foreach (var stream in streams)
            {
                var buffer = new byte[4096];
                try
                {
                    int read;
                    while ((read = stream.Read(buffer, 0, buffer.Length)) > 0)
                    {
                        result.Write(buffer, 0, read);
                    }
                }
                finally
                {
                    if (!leaveOpen) { stream.Dispose(); }
                }
            }
            result.Position = 0;
            return result;
        }

        /// <summary>
		/// Compresses the <paramref name="value"/> stream using the <see cref="CompressionType.Deflate"/> algorithm.
		/// </summary>
		/// <param name="value">The stream to compress.</param>
        /// <param name="bufferSize">The size of the buffer. This value must be greater than zero. The default size is 81920.</param>
		/// <returns>A Deflate compressed <see cref="Stream"/> of the <paramref name="value"/>.</returns>
		public static Stream CompressDeflate(Stream value, int bufferSize = 81920)
        {
            return CompressStream(value, CompressionType.Deflate, bufferSize);
        }

        /// <summary>
        /// Compresses the <paramref name="value"/> stream using the <see cref="CompressionType.GZip"/> algorithm.
        /// </summary>
        /// <param name="value">The stream to compress.</param>
        /// <param name="bufferSize">The size of the buffer. This value must be greater than zero. The default size is 81920.</param>
        /// <returns>A GZip compressed <see cref="Stream"/> of the <paramref name="value"/>.</returns>
        public static Stream CompressGZip(Stream value, int bufferSize = 81920)
        {
            return CompressStream(value, CompressionType.GZip, bufferSize);
        }

        private static Stream CompressStream(Stream value, CompressionType compressionType, int bufferSize)
        {
            Validator.ThrowIfNull(value, nameof(value));
            Validator.ThrowIfLowerThanOrEqual(bufferSize, 0, nameof(bufferSize));
            var target = new MemoryStream();
            try
            {
                Stream compressed;
                switch (compressionType)
                {
                    case CompressionType.Deflate:
                        compressed = new DeflateStream(target, CompressionMode.Compress, true);
                        break;
                    case CompressionType.GZip:
                        compressed = new GZipStream(target, CompressionMode.Compress, true);
                        break;
                    default:
                        throw new ArgumentOutOfRangeException(nameof(compressionType));
                }

                using (compressed)
                {
                    Infrastructure.CopyStream(value, compressed, bufferSize);
                }
            }
            catch (Exception)
            {
                target.Dispose();
                throw;
            }
            target.Position = 0;
            return target;
        }

        /// <summary>
        /// Decompresses the source stream using <see cref="CompressionType.Deflate"/> algorithm.
        /// </summary>
        /// <param name="value">The stream to decompress.</param>
        /// <param name="bufferSize">The size of the buffer. This value must be greater than zero. The default size is 81920.</param>
        /// <returns>A decompressed <see cref="Stream"/> of the <paramref name="value"/>.</returns>
        public static Stream DecompressDeflate(Stream value, int bufferSize = 81920)
        {
            return DecompressStream(value, CompressionType.Deflate, bufferSize);
        }

        /// <summary>
        /// Decompresses the source stream using <see cref="CompressionType.GZip"/> algorithm.
        /// </summary>
        /// <param name="value">The stream to decompress.</param>
        /// <param name="bufferSize">The size of the buffer. This value must be greater than zero. The default size is 81920.</param>
        /// <returns>A decompressed <see cref="Stream"/> of the <paramref name="value"/>.</returns>
        public static Stream DecompressGZip(Stream value, int bufferSize = 81920)
        {
            return DecompressStream(value, CompressionType.GZip, bufferSize);
        }

        private static Stream DecompressStream(Stream value, CompressionType compressionType, int bufferSize)
        {
            Validator.ThrowIfNull(value, nameof(value));
            Validator.ThrowIfLowerThanOrEqual(bufferSize, 0, nameof(bufferSize));
            var target = new MemoryStream();
            try
            {
                Stream uncompressed;
                switch (compressionType)
                {
                    case CompressionType.Deflate:
                        uncompressed = new DeflateStream(value, CompressionMode.Decompress, true);
                        break;
                    case CompressionType.GZip:
                        uncompressed = new GZipStream(value, CompressionMode.Decompress, true);
                        break;
                    default:
                        throw new ArgumentOutOfRangeException(nameof(compressionType));
                }

                using (uncompressed)
                {
                    Infrastructure.CopyStream(uncompressed, target, bufferSize);
                }
            }
            catch (Exception)
            {
                target.Dispose();
                throw;
            }
            return target;
        }
    }
}