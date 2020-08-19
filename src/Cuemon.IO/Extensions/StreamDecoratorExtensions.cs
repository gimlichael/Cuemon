using System;
using System.ComponentModel;
using System.IO;
using System.IO.Compression;
using System.Threading.Tasks;
using Cuemon.Text;

namespace Cuemon.IO
{
    /// <summary>
    /// Extension methods for the <see cref="Stream"/> class tailored to adhere the decorator pattern.
    /// </summary>
    /// <seealso cref="IDecorator{T}"/>
    /// <seealso cref="Decorator{T}"/>
    public static class StreamDecoratorExtensions
    {
        #if NETSTANDARD2_1
        /// <summary>
        /// Compress the enclosed <see cref="Stream"/> of the specified <paramref name="decorator"/> using the <c>Brotli</c> algorithm.
        /// </summary>
        /// <param name="decorator">The <see cref="IDecorator{Stream}"/> to extend.</param>
        /// <param name="setup">The <see cref="StreamCompressionOptions"/> which may be configured.</param>
        /// <returns>A compressed version of the enclosed <see cref="Stream"/> of the specified <paramref name="decorator"/>.</returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="decorator"/> cannot be null.
        /// </exception>
        /// <exception cref="ArgumentException">
        /// The enclosed <see cref="Stream"/> of <paramref name="decorator"/> does not support write operations such as compression.
        /// </exception>
        public static Stream CompressBrotli(this IDecorator<Stream> decorator, Action<StreamCompressionOptions> setup = null)
        {
            Validator.ThrowIfNull(decorator, nameof(decorator));
            return Compress(decorator, Patterns.Configure(setup), (stream, level, leaveOpen) => new BrotliStream(stream, level, leaveOpen));
        }

        /// <summary>
        /// Compress the enclosed <see cref="Stream"/> of the specified <paramref name="decorator"/> using the <c>Brotli</c> algorithm.
        /// </summary>
        /// <param name="decorator">The <see cref="IDecorator{Stream}"/> to extend.</param>
        /// <param name="setup">The <see cref="StreamCompressionOptions"/> which may be configured.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains a compressed version of the enclosed <see cref="Stream"/> of the specified <paramref name="decorator"/>.</returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="decorator"/> cannot be null.
        /// </exception>
        /// <exception cref="ArgumentException">
        /// The enclosed <see cref="Stream"/> of <paramref name="decorator"/> does not support write operations such as compression.
        /// </exception>
        public static Task<Stream> CompressBrotliAsync(this IDecorator<Stream> decorator, Action<StreamCompressionOptions> setup = null)
        {
            Validator.ThrowIfNull(decorator, nameof(decorator));
            return CompressAsync(decorator, Patterns.Configure(setup), (stream, level, leaveOpen) => new BrotliStream(stream, level, leaveOpen));
        }

        /// <summary>
        /// Decompress the enclosed <see cref="Stream"/> of the specified <paramref name="decorator"/> using <c>Brotli</c> data format specification.
        /// </summary>
        /// <param name="decorator">The <see cref="IDecorator{Stream}"/> to extend.</param>
        /// <param name="setup">The <see cref="StreamCopyOptions"/> which may be configured.</param>
        /// <returns>A decompressed version of the enclosed <see cref="Stream"/> of the specified <paramref name="decorator"/>.</returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="decorator"/> cannot be null.
        /// </exception>
        /// <exception cref="ArgumentException">
        /// The enclosed <see cref="Stream"/> of <paramref name="decorator"/> does not support write operations such as compression.
        /// </exception>
        /// <exception cref="InvalidDataException">
        /// The enclosed <see cref="Stream"/> of <paramref name="decorator"/> was compressed using an unsupported compression method.
        /// </exception>
        public static Stream DecompressBrotli(this IDecorator<Stream> decorator, Action<StreamCopyOptions> setup = null)
        {
            Validator.ThrowIfNull(decorator, nameof(decorator));
            return Decompress(decorator, Patterns.Configure(setup), (stream, mode, leaveOpen) => new BrotliStream(stream, mode, leaveOpen));
        }

        /// <summary>
        /// Decompress the enclosed <see cref="Stream"/> of the specified <paramref name="decorator"/> using <c>Brotli</c> data format specification.
        /// </summary>
        /// <param name="decorator">The <see cref="IDecorator{Stream}"/> to extend.</param>
        /// <param name="setup">The <see cref="StreamCopyOptions"/> which may be configured.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains a decompressed version of the enclosed <see cref="Stream"/> of the specified <paramref name="decorator"/>.</returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="decorator"/> cannot be null.
        /// </exception>
        /// <exception cref="ArgumentException">
        /// The enclosed <see cref="Stream"/> of <paramref name="decorator"/> does not support write operations such as compression.
        /// </exception>
        /// <exception cref="InvalidDataException">
        /// The enclosed <see cref="Stream"/> of <paramref name="decorator"/> was compressed using an unsupported compression method.
        /// </exception>
        public static Task<Stream> DecompressBrotliAsync(this IDecorator<Stream> decorator, Action<StreamCopyOptions> setup = null)
        {
            Validator.ThrowIfNull(decorator, nameof(decorator));
            return DecompressAsync(decorator, Patterns.Configure(setup), (stream, mode, leaveOpen) => new BrotliStream(stream, mode, leaveOpen));
        }
        #endif

        /// <summary>
        /// Compress the enclosed <see cref="Stream"/> of the specified <paramref name="decorator"/> using the <c>GZip</c> algorithm.
        /// </summary>
        /// <param name="decorator">The <see cref="IDecorator{Stream}"/> to extend.</param>
        /// <param name="setup">The <see cref="StreamCompressionOptions"/> which may be configured.</param>
        /// <returns>A compressed version of the enclosed <see cref="Stream"/> of the specified <paramref name="decorator"/>.</returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="decorator"/> cannot be null.
        /// </exception>
        /// <exception cref="ArgumentException">
        /// The enclosed <see cref="Stream"/> of <paramref name="decorator"/> does not support write operations such as compression.
        /// </exception>
        public static Stream CompressGZip(this IDecorator<Stream> decorator, Action<StreamCompressionOptions> setup = null)
        {
            Validator.ThrowIfNull(decorator, nameof(decorator));
            return Compress(decorator, Patterns.Configure(setup), (stream, level, leaveOpen) => new GZipStream(stream, level, leaveOpen));
        }

        /// <summary>
        /// Compress the enclosed <see cref="Stream"/> of the specified <paramref name="decorator"/> using the <c>GZip</c> algorithm.
        /// </summary>
        /// <param name="decorator">The <see cref="IDecorator{Stream}"/> to extend.</param>
        /// <param name="setup">The <see cref="StreamCompressionOptions"/> which may be configured.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains a compressed version of the enclosed <see cref="Stream"/> of the specified <paramref name="decorator"/>.</returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="decorator"/> cannot be null.
        /// </exception>
        /// <exception cref="ArgumentException">
        /// The enclosed <see cref="Stream"/> of <paramref name="decorator"/> does not support write operations such as compression.
        /// </exception>
        public static Task<Stream> CompressGZipAsync(this IDecorator<Stream> decorator, Action<StreamCompressionOptions> setup = null)
        {
            Validator.ThrowIfNull(decorator, nameof(decorator));
            return CompressAsync(decorator, Patterns.Configure(setup), (stream, level, leaveOpen) => new GZipStream(stream, level, leaveOpen));
        }

        /// <summary>
        /// Decompress the enclosed <see cref="Stream"/> of the specified <paramref name="decorator"/> using <c>GZip</c> data format specification.
        /// </summary>
        /// <param name="decorator">The <see cref="IDecorator{Stream}"/> to extend.</param>
        /// <param name="setup">The <see cref="StreamCopyOptions"/> which may be configured.</param>
        /// <returns>A decompressed version of the enclosed <see cref="Stream"/> of the specified <paramref name="decorator"/>.</returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="decorator"/> cannot be null.
        /// </exception>
        /// <exception cref="ArgumentException">
        /// The enclosed <see cref="Stream"/> of <paramref name="decorator"/> does not support write operations such as compression.
        /// </exception>
        /// <exception cref="InvalidDataException">
        /// The enclosed <see cref="Stream"/> of <paramref name="decorator"/> was compressed using an unsupported compression method.
        /// </exception>
        public static Stream DecompressGZip(this IDecorator<Stream> decorator, Action<StreamCopyOptions> setup = null)
        {
            Validator.ThrowIfNull(decorator, nameof(decorator));
            return Decompress(decorator, Patterns.Configure(setup), (stream, mode, leaveOpen) => new GZipStream(stream, mode, leaveOpen));
        }

        /// <summary>
        /// Decompress the enclosed <see cref="Stream"/> of the specified <paramref name="decorator"/> using <c>GZip</c> data format specification.
        /// </summary>
        /// <param name="decorator">The <see cref="IDecorator{Stream}"/> to extend.</param>
        /// <param name="setup">The <see cref="StreamCopyOptions"/> which may be configured.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains a decompressed version of the enclosed <see cref="Stream"/> of the specified <paramref name="decorator"/>.</returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="decorator"/> cannot be null.
        /// </exception>
        /// <exception cref="ArgumentException">
        /// The enclosed <see cref="Stream"/> of <paramref name="decorator"/> does not support write operations such as compression.
        /// </exception>
        /// <exception cref="InvalidDataException">
        /// The enclosed <see cref="Stream"/> of <paramref name="decorator"/> was compressed using an unsupported compression method.
        /// </exception>
        public static Task<Stream> DecompressGZipAsync(this IDecorator<Stream> decorator, Action<StreamCopyOptions> setup = null)
        {
            Validator.ThrowIfNull(decorator, nameof(decorator));
            return DecompressAsync(decorator, Patterns.Configure(setup), (stream, mode, leaveOpen) => new GZipStream(stream, mode, leaveOpen));
        }

        /// <summary>
        /// Compress the enclosed <see cref="Stream"/> of the specified <paramref name="decorator"/> using the <c>Deflate</c> algorithm.
        /// </summary>
        /// <param name="decorator">The <see cref="IDecorator{Stream}"/> to extend.</param>
        /// <param name="setup">The <see cref="StreamCompressionOptions"/> which may be configured.</param>
        /// <returns>A compressed version of the enclosed <see cref="Stream"/> of the specified <paramref name="decorator"/>.</returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="decorator"/> cannot be null.
        /// </exception>
        /// <exception cref="ArgumentException">
        /// The enclosed <see cref="Stream"/> of <paramref name="decorator"/> does not support write operations such as compression.
        /// </exception>
        public static Stream CompressDeflate(this IDecorator<Stream> decorator, Action<StreamCompressionOptions> setup = null)
        {
            Validator.ThrowIfNull(decorator, nameof(decorator));
            return Compress(decorator, Patterns.Configure(setup), (stream, level, leaveOpen) => new DeflateStream(stream, level, leaveOpen));
        }

        /// <summary>
        /// Compress the enclosed <see cref="Stream"/> of the specified <paramref name="decorator"/> using the <c>Deflate</c> algorithm.
        /// </summary>
        /// <param name="decorator">The <see cref="IDecorator{Stream}"/> to extend.</param>
        /// <param name="setup">The <see cref="StreamCompressionOptions"/> which may be configured.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains a compressed version of the enclosed <see cref="Stream"/> of the specified <paramref name="decorator"/>.</returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="decorator"/> cannot be null.
        /// </exception>
        /// <exception cref="ArgumentException">
        /// The enclosed <see cref="Stream"/> of <paramref name="decorator"/> does not support write operations such as compression.
        /// </exception>
        public static Task<Stream> CompressDeflateAsync(this IDecorator<Stream> decorator, Action<StreamCompressionOptions> setup = null)
        {
            Validator.ThrowIfNull(decorator, nameof(decorator));
            return CompressAsync(decorator, Patterns.Configure(setup), (stream, level, leaveOpen) => new DeflateStream(stream, level, leaveOpen));
        }

        /// <summary>
        /// Decompress the enclosed <see cref="Stream"/> of the specified <paramref name="decorator"/> using <c>Deflate</c> data format specification.
        /// </summary>
        /// <param name="decorator">The <see cref="IDecorator{Stream}"/> to extend.</param>
        /// <param name="setup">The <see cref="StreamCopyOptions"/> which may be configured.</param>
        /// <returns>A decompressed version of the enclosed <see cref="Stream"/> of the specified <paramref name="decorator"/>.</returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="decorator"/> cannot be null.
        /// </exception>
        /// <exception cref="ArgumentException">
        /// The enclosed <see cref="Stream"/> of <paramref name="decorator"/> does not support write operations such as compression.
        /// </exception>
        /// <exception cref="InvalidDataException">
        /// The enclosed <see cref="Stream"/> of <paramref name="decorator"/> was compressed using an unsupported compression method.
        /// </exception>
        public static Stream DecompressDeflate(this IDecorator<Stream> decorator, Action<StreamCopyOptions> setup = null)
        {
            Validator.ThrowIfNull(decorator, nameof(decorator));
            return Decompress(decorator, Patterns.Configure(setup), (stream, mode, leaveOpen) => new DeflateStream(stream, mode, leaveOpen));
        }

        /// <summary>
        /// Decompress the enclosed <see cref="Stream"/> of the specified <paramref name="decorator"/> using <c>Deflate</c> data format specification.
        /// </summary>
        /// <param name="decorator">The <see cref="IDecorator{Stream}"/> to extend.</param>
        /// <param name="setup">The <see cref="StreamCopyOptions"/> which may be configured.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains a decompressed version of the enclosed <see cref="Stream"/> of the specified <paramref name="decorator"/>.</returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="decorator"/> cannot be null.
        /// </exception>
        /// <exception cref="ArgumentException">
        /// The enclosed <see cref="Stream"/> of <paramref name="decorator"/> does not support write operations such as compression.
        /// </exception>
        /// <exception cref="InvalidDataException">
        /// The enclosed <see cref="Stream"/> of <paramref name="decorator"/> was compressed using an unsupported compression method.
        /// </exception>
        public static Task<Stream> DecompressDeflateAsync(this IDecorator<Stream> decorator, Action<StreamCopyOptions> setup = null)
        {
            Validator.ThrowIfNull(decorator, nameof(decorator));
            return DecompressAsync(decorator, Patterns.Configure(setup), (stream, mode, leaveOpen) => new DeflateStream(stream, mode, leaveOpen));
        }

        private static Stream Compress<T>(IDecorator<Stream> decorator, StreamCompressionOptions options, Func<Stream, CompressionLevel, bool, T> decompressor) where T : Stream
        {
            return Disposable.SafeInvoke(() => new MemoryStream(), target =>
            {
                using (var compressed = decompressor(target, options.Level, true))
                {
                    Decorator.Enclose(decorator.Inner).CopyStream(compressed, options.BufferSize);
                }
                target.Flush();
                target.Position = 0;
                return target;
            });
        }

        private static Task<Stream> CompressAsync<T>(IDecorator<Stream> decorator, StreamCompressionOptions options, Func<Stream, CompressionLevel, bool, T> decompressor) where T : Stream
        {
            return Disposable.SafeInvokeAsync<Stream>(() => new MemoryStream(), async (target, ct) =>
            {
                #if NETSTANDARD2_1
                await using (var compressed = decompressor(target, options.Level, true))
                {
                    await Decorator.Enclose(decorator.Inner).CopyStreamAsync(compressed, options.BufferSize, ct).ConfigureAwait(false);
                }
                #else
                using (var compressed = decompressor(target, options.Level, true))
                {
                    await Decorator.Enclose(decorator.Inner).CopyStreamAsync(compressed, options.BufferSize, ct).ConfigureAwait(false);
                }
                #endif
                await target.FlushAsync(ct).ConfigureAwait(false);
                target.Position = 0;
                return target;
            }, options.CancellationToken);
        }

        private static Stream Decompress<T>(IDecorator<Stream> decorator, StreamCopyOptions options, Func<Stream, CompressionMode, bool, T> compressor) where T : Stream
        {
            return Disposable.SafeInvoke(() => new MemoryStream(), target =>
            {
                using (var uncompressed = compressor(decorator.Inner, CompressionMode.Decompress, true))
                {
                    Decorator.Enclose(uncompressed).CopyStream(target, options.BufferSize);
                }
                target.Flush();
                target.Position = 0;
                return target;
            });
        }

        private static Task<Stream> DecompressAsync<T>(IDecorator<Stream> decorator, StreamCopyOptions options, Func<Stream, CompressionMode, bool, T> compressor) where T : Stream
        {
            return Disposable.SafeInvokeAsync<Stream>(() => new MemoryStream(), async (target, ct) =>
            {
                #if NETSTANDARD2_1
                await using (var uncompressed = compressor(decorator.Inner, CompressionMode.Decompress, true))
                {
                    await Decorator.Enclose(uncompressed).CopyStreamAsync(target, options.BufferSize, ct).ConfigureAwait(false);
                }
                #else
                using (var uncompressed = compressor(decorator.Inner, CompressionMode.Decompress, true))
                {
                    await Decorator.Enclose(uncompressed).CopyStreamAsync(target, options.BufferSize, ct).ConfigureAwait(false);
                }
                #endif
                await target.FlushAsync(ct).ConfigureAwait(false);
                target.Position = 0;
                return target;
            }, options.CancellationToken);
        }

        /// <summary>
        /// Asynchronously writes a sequence of bytes to the enclosed <see cref="Stream"/> of the <paramref name="decorator"/> with the entire size of the <paramref name="buffer"/> starting from position 0.
        /// </summary>
        /// <param name="decorator">The <see cref="IDecorator{T}"/> to extend.</param>
        /// <param name="buffer">The buffer to write data from.</param>
        /// <returns>A task that represents the asynchronous write operation.</returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="decorator"/> is null -or-
        /// <paramref name="buffer"/> is null.
        /// </exception>
        /// <exception cref="NotSupportedException">
        /// The enclosed <see cref="Stream"/> of <paramref name="decorator"/> does not support writing.
        /// </exception>
        /// <exception cref="ObjectDisposedException">
        /// The enclosed <see cref="Stream"/> of <paramref name="decorator"/> has been disposed.
        /// </exception>
        /// <exception cref="InvalidOperationException">
        /// The enclosed <see cref="Stream"/> of <paramref name="decorator"/> is currently in use by a previous write operation.
        /// </exception>
        public static Task WriteAsync(this IDecorator<Stream> decorator, byte[] buffer)
        {
            Validator.ThrowIfNull(decorator, nameof(decorator));
            return decorator.Inner.WriteAsync(buffer, 0, buffer.Length);
        }
    }
}