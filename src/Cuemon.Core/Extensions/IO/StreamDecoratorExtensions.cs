using System;
using System.IO;
using System.Threading.Tasks;

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
        public static async Task<byte[]> ToByteArrayAsync(this IDecorator<Stream> decorator, Action<StreamCopyOptions> setup = null)
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
                    await decorator.Inner.CopyToAsync(memoryStream, options.BufferSize, options.CancellationToken);
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