using System.IO;

namespace Cuemon.IO.Compression
{
    /// <summary>
    /// This is an extension implementation of the most common methods on the <see cref="CompressionUtility"/> class.
    /// </summary>
    public static class CompressionUtilityExtension
    {
        /// <summary>
        /// Compresses the <paramref name="source"/> stream using the Deflate algorithm.
        /// </summary>
        /// <param name="source">The source stream to compress.</param>
        /// <returns>A compressed <see cref="System.IO.Stream"/> of the <paramref name="source"/>.</returns>
        public static Stream Compress(this Stream source)
        {
            return CompressionUtility.CompressStream(source);
        }

        /// <summary>
        /// Compresses the <paramref name="source"/> stream using the specified <paramref name="compressionType"/> algorithm.
        /// </summary>
        /// <param name="source">The source stream to compress.</param>
        /// <param name="compressionType">The compression algorithm to use for the compression.</param>
        /// <returns>A compressed <see cref="System.IO.Stream"/> of the <paramref name="source"/>.</returns>
        public static Stream Compress(this Stream source, CompressionType compressionType)
        {
            return CompressionUtility.CompressStream(source, compressionType);
        }

        /// <summary>
        /// Compresses the <paramref name="source"/> stream using the specified <paramref name="compressionType"/> algorithm.
        /// </summary>
        /// <param name="source">The source stream to compress.</param>
        /// <param name="compressionType">The compression algorithm to use for the compression.</param>
        /// <param name="bufferSize">The size of the buffer. This value must be greater than zero. The default size is 2048.</param>
        /// <returns>A compressed <see cref="System.IO.Stream"/> of the <paramref name="source"/>.</returns>
        public static Stream Compress(this Stream source, CompressionType compressionType, int bufferSize)
        {
            return CompressionUtility.CompressStream(source, compressionType, bufferSize);
        }

        /// <summary>
        /// Decompresses the source stream using the Deflate algorithm.
        /// </summary>
        /// <param name="source">The source stream to decompress.</param>
        /// <returns>A decompressed <see cref="System.IO.Stream"/> of the <paramref name="source"/>.</returns>
        public static Stream Decompress(this Stream source)
        {
            return CompressionUtility.DecompressStream(source);
        }

        /// <summary>
        /// Decompresses the source stream using the specified <paramref name="compressionType"/> algorithm.
        /// </summary>
        /// <param name="source">The source stream to decompress.</param>
        /// <param name="compressionType">The compression algorithm to use for the decompression.</param>
        /// <returns>A decompressed <see cref="System.IO.Stream"/> of the <paramref name="source"/>.</returns>
        public static Stream Decompress(this Stream source, CompressionType compressionType)
        {
            return CompressionUtility.DecompressStream(source, compressionType);
        }

        /// <summary>
        /// Decompresses the source stream using the specified <paramref name="compressionType"/> algorithm.
        /// </summary>
        /// <param name="source">The source stream to decompress.</param>
        /// <param name="compressionType">The compression algorithm to use for the decompression.</param>
        /// <param name="bufferSize">The size of the buffer. This value must be greater than zero. The default size is 2048.</param>
        /// <returns>A decompressed <see cref="System.IO.Stream"/> of the <paramref name="source"/>.</returns>
        public static Stream DecompressStream(Stream source, CompressionType compressionType, int bufferSize)
        {
            return CompressionUtility.DecompressStream(source, compressionType, bufferSize);
        }
    }
}
