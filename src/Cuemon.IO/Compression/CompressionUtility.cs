using System;
using System.IO;
using System.IO.Compression;

namespace Cuemon.IO.Compression
{
	/// <summary>
	/// This utility class is designed to make <see cref="Stream"/> related compression operations easier to work with.
	/// </summary>
	public static class CompressionUtility
	{
		/// <summary>
		/// Compresses the <paramref name="source"/> stream using the Deflate algorithm.
		/// </summary>
		/// <param name="source">The source stream to compress.</param>
		/// <returns>A compressed <see cref="System.IO.Stream"/> of the <paramref name="source"/>.</returns>
		public static Stream CompressStream(Stream source)
		{
			return CompressStream(source, CompressionType.Deflate);
		}

	    /// <summary>
	    /// Compresses the <paramref name="source"/> stream using the specified <paramref name="compressionType"/> algorithm.
	    /// </summary>
	    /// <param name="source">The source stream to compress.</param>
	    /// <param name="compressionType">The compression algorithm to use for the compression.</param>
	    /// <returns>A compressed <see cref="System.IO.Stream"/> of the <paramref name="source"/>.</returns>
	    public static Stream CompressStream(Stream source, CompressionType compressionType)
	    {
	        return CompressStream(source, compressionType, Infrastructure.DefaultBufferSize);
	    }

        /// <summary>
        /// Compresses the <paramref name="source"/> stream using the specified <paramref name="compressionType"/> algorithm.
        /// </summary>
        /// <param name="source">The source stream to compress.</param>
        /// <param name="compressionType">The compression algorithm to use for the compression.</param>
        /// <param name="bufferSize">The size of the buffer. This value must be greater than zero. The default size is 2048.</param>
        /// <returns>A compressed <see cref="System.IO.Stream"/> of the <paramref name="source"/>.</returns>
        public static Stream CompressStream(Stream source, CompressionType compressionType, int bufferSize)
		{
			Validator.ThrowIfNull(source, nameof(source));
            Validator.ThrowIfLowerThanOrEqual(bufferSize, 0, nameof(bufferSize));
			MemoryStream target = new MemoryStream();
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
                    Infrastructure.WhileSourceReadDestionationWrite(source, compressed, bufferSize, true);
                }
			}
			catch (Exception)
			{
				target.Dispose();
				throw;
			}
			return target;
		}

        /// <summary>
        /// Decompresses the source stream using the Deflate algorithm.
        /// </summary>
        /// <param name="source">The source stream to decompress.</param>
        /// <returns>A decompressed <see cref="System.IO.Stream"/> of the <paramref name="source"/>.</returns>
        public static Stream DecompressStream(Stream source)
        {
            return DecompressStream(source, CompressionType.Deflate);
        }

        /// <summary>
        /// Decompresses the source stream using the specified <paramref name="compressionType"/> algorithm.
        /// </summary>
        /// <param name="source">The source stream to decompress.</param>
        /// <param name="compressionType">The compression algorithm to use for the decompression.</param>
        /// <returns>A decompressed <see cref="System.IO.Stream"/> of the <paramref name="source"/>.</returns>
        public static Stream DecompressStream(Stream source, CompressionType compressionType)
        {
            return DecompressStream(source, compressionType, Infrastructure.DefaultBufferSize);
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
            Validator.ThrowIfNull(source, nameof(source));
            Validator.ThrowIfLowerThanOrEqual(bufferSize, 0, nameof(bufferSize));
            MemoryStream target = new MemoryStream();
            try
            {
                Stream uncompressed;
                switch (compressionType)
                {
                    case CompressionType.Deflate:
                        uncompressed = new DeflateStream(source, CompressionMode.Decompress, true);
                        break;
                    case CompressionType.GZip:
                        uncompressed = new GZipStream(source, CompressionMode.Decompress, true);
                        break;
                    default:
                        throw new ArgumentOutOfRangeException(nameof(compressionType));
                }

                source.Position = 0;
                using (uncompressed)
                {
                    byte[] buffer = new byte[bufferSize];
                    int compressedByte;
                    while ((compressedByte = uncompressed.Read(buffer, 0, buffer.Length)) > 0)
                    {
                        target.Write(buffer, 0, compressedByte);
                    }
                    uncompressed.Flush();
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
    }
}