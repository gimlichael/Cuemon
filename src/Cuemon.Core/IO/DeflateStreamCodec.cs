using System;
using System.IO;
using System.IO.Compression;
using Cuemon.ComponentModel;

namespace Cuemon.IO
{
    /// <summary>
    /// Provides a codec that is used to compress and decompress a <see cref="Stream"/> using the Deflate algorithm.
    /// </summary>
    public class DeflateStreamCodec : ICodec, IEncoder<Stream, Stream, StreamCompressionOptions>, IDecoder<Stream, Stream, StreamCopyOptions>
    {
        /// <summary>
        /// Compresses the specified <paramref name="value"/> using the Deflate algorithm.
        /// </summary>
        /// <param name="value">The <see cref="Stream"/> to be Deflate encoded.</param>
        /// <param name="setup">The <see cref="StreamCompressionOptions"/> which may be configured.</param>
        /// <returns>A compressed version of <paramref name="value"/> using the Deflate algorithm.</returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="value"/> cannot be null.
        /// </exception>
        /// <exception cref="ArgumentException">
        /// <paramref name="value"/> does not support write operations such as compression.
        /// </exception>
        public Stream Encode(Stream value, Action<StreamCompressionOptions> setup = null)
        {
            Validator.ThrowIfNull(value, nameof(value));
            
            var options = Patterns.Configure(setup);
            return Disposable.SafeInvoke(() => new MemoryStream(), target =>
            {
                using (var compressed = new DeflateStream(target, options.Level, true))
                {
                    Infrastructure.CopyStream(value, compressed, options.BufferSize);
                }
                target.Flush();
                target.Position = 0;
                return target;
            });
        }

        /// <summary>
        /// Decompress the specified <paramref name="encoded"/> using the Deflate algorithm.
        /// </summary>
        /// <param name="encoded">The <see cref="Stream"/> to be Deflate decoded.</param>
        /// <param name="setup">The <see cref="StreamCopyOptions"/> which may be configured.</param>
        /// <returns>A decompressed version of <paramref name="encoded"/> using the Deflate algorithm.</returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="encoded"/> cannot be null.
        /// </exception>
        /// <exception cref="ArgumentException">
        /// <paramref name="encoded"/> does not support write operations such as compression.
        /// </exception>
        public Stream Decode(Stream encoded, Action<StreamCopyOptions> setup = null)
        {
            Validator.ThrowIfNull(encoded, nameof(encoded));

            var options = Patterns.Configure(setup);
            return Disposable.SafeInvoke(() => new MemoryStream(), target =>
            {
                using (var uncompressed = new DeflateStream(encoded, CompressionMode.Decompress, true))
                {
                    Infrastructure.CopyStream(uncompressed, target, options.BufferSize);
                }
                target.Flush();
                target.Position = 0;
                return target;
            });
        }
    }
}