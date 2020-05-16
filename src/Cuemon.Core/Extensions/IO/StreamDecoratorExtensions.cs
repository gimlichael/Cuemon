using System;
using System.ComponentModel;
using System.IO;
using System.IO.Compression;
using System.Threading.Tasks;
using Cuemon.Integrity;
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
                    Infrastructure.CopyStream(decorator.Inner, compressed, options.BufferSize);
                }
                target.Flush();
                target.Position = 0;
                return target;
            });
        }

        private static Task<Stream> CompressAsync<T>(IDecorator<Stream> decorator, StreamCompressionOptions options, Func<Stream, CompressionLevel, bool, T> decompressor) where T : Stream
        {
            return Disposable.SafeInvokeAsync<Stream>(() => new MemoryStream(), async target =>
            {
                #if NETSTANDARD2_1
                await using (var compressed = decompressor(target, options.Level, true))
                {
                    await Infrastructure.CopyStreamAsync(decorator.Inner, compressed, options.BufferSize, options.CancellationToken).ConfigureAwait(false);
                }
                #else
                using (var compressed = decompressor(target, options.Level, true))
                {
                    await Infrastructure.CopyStreamAsync(decorator.Inner, compressed, options.BufferSize, options.CancellationToken).ConfigureAwait(false);
                }
                #endif
                await target.FlushAsync(options.CancellationToken).ConfigureAwait(false);
                target.Position = 0;
                return target;
            });
        }

        private static Stream Decompress<T>(IDecorator<Stream> decorator, StreamCopyOptions options, Func<Stream, CompressionMode, bool, T> compressor) where T : Stream
        {
            return Disposable.SafeInvoke(() => new MemoryStream(), target =>
            {
                using (var uncompressed = compressor(decorator.Inner, CompressionMode.Decompress, true))
                {
                    Infrastructure.CopyStream(uncompressed, target, options.BufferSize);
                }
                target.Flush();
                target.Position = 0;
                return target;
            });
        }

        private static Task<Stream> DecompressAsync<T>(IDecorator<Stream> decorator, StreamCopyOptions options, Func<Stream, CompressionMode, bool, T> compressor) where T : Stream
        {
            return Disposable.SafeInvokeAsync<Stream>(() => new MemoryStream(), async target =>
            {
                #if NETSTANDARD2_1
                await using (var uncompressed = compressor(decorator.Inner, CompressionMode.Decompress, true))
                {
                    await Infrastructure.CopyStreamAsync(uncompressed, target, options.BufferSize, options.CancellationToken).ConfigureAwait(false);
                }
                #else
                using (var uncompressed = compressor(decorator.Inner, CompressionMode.Decompress, true))
                {
                    await Infrastructure.CopyStreamAsync(uncompressed, target, options.BufferSize, options.CancellationToken).ConfigureAwait(false);
                }
                #endif
                await target.FlushAsync(options.CancellationToken).ConfigureAwait(false);
                target.Position = 0;
                return target;
            });
        }

        /// <summary>
        /// Converts the enclosed <see cref="Stream"/> of the specified <paramref name="decorator"/> to its equivalent <see cref="T:byte[]"/> representation.
        /// </summary>
        /// <param name="decorator">The <see cref="IDecorator{Stream}"/> to extend.</param>
        /// <param name="setup">The <see cref="StreamCopyOptions"/> which may be configured.</param>
        /// <returns>A <see cref="T:byte[]"/> that is equivalent to the enclosed <see cref="Stream"/> of the specified <paramref name="decorator"/>.</returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="decorator"/> cannot be null.
        /// </exception>
        /// <exception cref="ArgumentException">
        /// The enclosed <see cref="Stream"/> of <paramref name="decorator"/> cannot be read from.
        /// </exception>
        public static byte[] ToByteArray(this IDecorator<Stream> decorator, Action<StreamCopyOptions> setup = null)
        {
            Validator.ThrowIfNull(decorator, nameof(decorator));
            Validator.ThrowIfFalse(decorator.Inner.CanRead, nameof(decorator.Inner), "Stream cannot be read from.");
            var options = Patterns.Configure(setup);
            try
            {
                if (decorator.Inner is MemoryStream s) { return s.ToArray(); }
                using (var memoryStream = new MemoryStream(new byte[decorator.Inner.Length]))
                {
                    var oldPosition = decorator.Inner.Position;
                    if (decorator.Inner.CanSeek) { decorator.Inner.Position = 0; }
                    decorator.Inner.CopyTo(memoryStream, options.BufferSize);
                    if (decorator.Inner.CanSeek) { decorator.Inner.Position = oldPosition; }
                    return memoryStream.ToArray();
                }
            }
            finally
            {
                if (!options.LeaveOpen) { decorator.Inner.Dispose(); }
            }
        }

        /// <summary>
        /// Converts the enclosed <see cref="Stream"/> of the specified <paramref name="decorator"/> to its equivalent <see cref="T:byte[]"/> representation.
        /// </summary>
        /// <param name="decorator">The <see cref="IDecorator{Stream}"/> to extend.</param>
        /// <param name="setup">The <see cref="StreamCopyOptions"/> which may be configured.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains a <see cref="T:byte[]"/> that is equivalent to the enclosed <see cref="Stream"/> of the specified <paramref name="decorator"/>.</returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="decorator"/> cannot be null.
        /// </exception>
        /// <exception cref="ArgumentException">
        /// The enclosed <see cref="Stream"/> of <paramref name="decorator"/> cannot be read from.
        /// </exception>
        public static Task<byte[]> ToByteArrayAsync(this IDecorator<Stream> decorator, Action<StreamCopyOptions> setup = null)
        {
            Validator.ThrowIfNull(decorator, nameof(decorator));
            Validator.ThrowIfFalse(decorator.Inner.CanRead, nameof(decorator.Inner), "Stream cannot be read from.");
            return ToByteArrayAsyncCore(decorator, Patterns.Configure(setup));
        }

        private static async Task<byte[]> ToByteArrayAsyncCore(this IDecorator<Stream> decorator, StreamCopyOptions options)
        {
            try
            {
                if (decorator.Inner is MemoryStream s) { return s.ToArray(); }
                using (var memoryStream = new MemoryStream(new byte[decorator.Inner.Length]))
                {
                    var oldPosition = decorator.Inner.Position;
                    if (decorator.Inner.CanSeek) { decorator.Inner.Position = 0; }
                    await decorator.Inner.CopyToAsync(memoryStream, options.BufferSize, options.CancellationToken).ConfigureAwait(false);
                    if (decorator.Inner.CanSeek) { decorator.Inner.Position = oldPosition; }
                    return memoryStream.ToArray();
                }
            }
            finally
            {
                if (!options.LeaveOpen) { decorator.Inner.Dispose(); }
            }
        }

        /// <summary>
        /// Converts the enclosed <see cref="Stream"/> of the specified <paramref name="decorator"/> to a <see cref="string"/>.
        /// </summary>
        /// <param name="decorator">The <see cref="IDecorator{Stream}"/> to extend.</param>
        /// <param name="setup">The <see cref="StreamReaderOptions"/> which may be configured.</param>
        /// <returns>A <see cref="string"/> containing the result of the enclosed <see cref="Stream"/> of the specified <paramref name="decorator"/>.</returns>
        /// <remarks><see cref="IEncodingOptions"/> will be initialized with <see cref="EncodingOptions.DefaultPreambleSequence"/> and <see cref="EncodingOptions.DefaultEncoding"/>.</remarks>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="decorator"/> cannot be null.
        /// </exception>
        /// <exception cref="InvalidEnumArgumentException">
        /// <paramref name="setup"/> was initialized with an invalid <see cref="StreamEncodingOptions.Preamble"/>.
        /// </exception>
        public static string ToEncodedString(this IDecorator<Stream> decorator, Action<StreamReaderOptions> setup = null)
        {
            Validator.ThrowIfNull(decorator, nameof(decorator));
            var options = Patterns.Configure(setup);
            if (options.Encoding.Equals(EncodingOptions.DefaultEncoding)) { options.Encoding = ByteOrderMark.DetectEncodingOrDefault(decorator.Inner, options.Encoding); }
            if (options.Preamble < PreambleSequence.Keep || options.Preamble > PreambleSequence.Remove) { throw new InvalidEnumArgumentException(nameof(setup), (int)options.Preamble, typeof(PreambleSequence)); }

            var bytes = Decorator.Enclose(decorator.Inner).ToByteArray(o =>
            {
                o.BufferSize = options.BufferSize;
                o.LeaveOpen = options.LeaveOpen;
            });
            return Convertible.ToString(bytes, o =>
            {
                o.Encoding = options.Encoding;
                o.Preamble = options.Preamble;
            });
        }

        /// <summary>
        /// Converts the enclosed <see cref="Stream"/> of the specified <paramref name="decorator"/> to a <see cref="string"/>.
        /// </summary>
        /// <param name="decorator">The <see cref="IDecorator{Stream}"/> to extend.</param>
        /// <param name="setup">The <see cref="StreamReaderOptions"/> which may be configured.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains a <see cref="string"/> containing the result of the enclosed <see cref="Stream"/> of the specified <paramref name="decorator"/>.</returns>
        /// <remarks><see cref="IEncodingOptions"/> will be initialized with <see cref="EncodingOptions.DefaultPreambleSequence"/> and <see cref="EncodingOptions.DefaultEncoding"/>.</remarks>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="decorator"/> cannot be null.
        /// </exception>
        /// <exception cref="InvalidEnumArgumentException">
        /// <paramref name="setup"/> was initialized with an invalid <see cref="StreamEncodingOptions.Preamble"/>.
        /// </exception>
        public static Task<string> ToEncodedStringAsync(this IDecorator<Stream> decorator, Action<StreamReaderOptions> setup = null)
        {
            Validator.ThrowIfNull(decorator, nameof(decorator));
            var options = Patterns.Configure(setup);
            if (options.Encoding.Equals(EncodingOptions.DefaultEncoding)) { options.Encoding = ByteOrderMark.DetectEncodingOrDefault(decorator.Inner, options.Encoding); }
            if (options.Preamble < PreambleSequence.Keep || options.Preamble > PreambleSequence.Remove) { throw new InvalidEnumArgumentException(nameof(setup), (int)options.Preamble, typeof(PreambleSequence)); }
            return ToEncodedStringAsyncCore(decorator, options);
        }

        private static async Task<string> ToEncodedStringAsyncCore(this IDecorator<Stream> decorator, StreamReaderOptions options)
        {
            var bytes = await Decorator.Enclose(decorator.Inner).ToByteArrayAsync(o =>
            {
                o.BufferSize = options.BufferSize;
                o.LeaveOpen = options.LeaveOpen;
                o.CancellationToken = options.CancellationToken;
            }).ConfigureAwait(false);
            return Convertible.ToString(bytes, o =>
            {
                o.Encoding = options.Encoding;
                o.Preamble = options.Preamble;
            });
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