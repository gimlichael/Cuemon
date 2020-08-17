using System;
using System.ComponentModel;
using System.IO;
using System.Threading;
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
        /// Reads the bytes from the enclosed <see cref="Stream"/> of the specified <paramref name="decorator"/> and writes them to the <paramref name="destination"/>.
        /// </summary>
        /// <param name="decorator">The <see cref="IDecorator{Stream}"/> to extend.</param>
        /// <param name="destination">The <see cref="Stream"/> to which the contents of the current stream will be copied.</param>
        /// <param name="bufferSize">The size of the buffer. This value must be greater than zero. The default size is 81920.</param>
        /// <param name="changePosition">if <c>true</c>, the enclosed <see cref="Stream"/> of the specified <paramref name="decorator"/> will temporarily have its position changed to 0; otherwise the position is left untouched.</param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="decorator"/> cannot be null.
        /// </exception>
        public static void CopyStream(this IDecorator<Stream> decorator, Stream destination, int bufferSize = 81920, bool changePosition = true)
        {
            Validator.ThrowIfNull(decorator, nameof(decorator));
            var source = decorator.Inner;
            long lastPosition = 0;
            if (changePosition && source.CanSeek)
            {
                lastPosition = source.Position;
                if (source.CanSeek) { source.Position = 0; }
            }

            source.CopyTo(destination, bufferSize);
            destination.Flush();

            if (changePosition && source.CanSeek) { source.Position = lastPosition; }
            if (changePosition && destination.CanSeek) { destination.Position = 0; }
        }

        /// <summary>
        /// Asynchronously reads the bytes from the enclosed <see cref="Stream"/> of the specified <paramref name="decorator"/> and writes them to the <paramref name="destination"/>.
        /// </summary>
        /// <param name="decorator">The <see cref="IDecorator{Stream}"/> to extend.</param>
        /// <param name="destination">The <see cref="Stream"/> to which the contents of the current stream will be copied.</param>
        /// <param name="bufferSize">The size of the buffer. This value must be greater than zero. The default size is 81920.</param>
        /// <param name="ct">The token to monitor for cancellation requests. The default value is <see cref="CancellationToken.None"/>.</param>
        /// <param name="changePosition">if <c>true</c>, the enclosed <see cref="Stream"/> of the specified <paramref name="decorator"/> will temporarily have its position changed to 0; otherwise the position is left untouched.</param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="decorator"/> cannot be null.
        /// </exception>
        public static async Task CopyStreamAsync(this IDecorator<Stream> decorator, Stream destination, int bufferSize = 81920, CancellationToken ct = default, bool changePosition = true)
        {
            Validator.ThrowIfNull(decorator, nameof(decorator));
            var source = decorator.Inner;
            long lastPosition = 0;
            if (changePosition && source.CanSeek)
            {
                lastPosition = source.Position;
                if (source.CanSeek) { source.Position = 0; }
            }

            await source.CopyToAsync(destination, bufferSize, ct).ConfigureAwait(false);
            await destination.FlushAsync(ct).ConfigureAwait(false);

            if (changePosition && source.CanSeek) { source.Position = lastPosition; }
            if (changePosition && destination.CanSeek) { destination.Position = 0; }
        }
    }
}