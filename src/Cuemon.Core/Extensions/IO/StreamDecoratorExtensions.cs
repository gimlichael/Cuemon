using System;
using System.IO;

namespace Cuemon.IO
{
    /// <summary>
    /// Extension methods for the <see cref="Stream"/> class hidden behind the <see cref="IDecorator{T}"/> interface.
    /// This API supports the product infrastructure and is not intended to be used directly from your code.
    /// </summary>
    /// <seealso cref="IDecorator{T}"/>
    /// <seealso cref="Decorator{T}"/>
    public static class StreamDecoratorExtensions
    {
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
            Validator.ThrowIfNull(decorator);
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
        /// Converts the enclosed <see cref="Stream"/> of the specified <paramref name="decorator"/> to its equivalent <see cref="T:byte[]"/> representation. Not intended to be used directly from your code.
        /// </summary>
        /// <param name="decorator">The <see cref="IDecorator{Stream}"/> to extend.</param>
        /// <param name="bufferSize">The size of the buffer. This value must be greater than zero. The default size is 81920.</param>
        /// <param name="leaveOpen">if <c>true</c>, the <see cref="Stream"/> object is being left open; otherwise it is being closed and disposed.</param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="decorator"/> cannot be null.
        /// </exception>
        public static byte[] InvokeToByteArray(this IDecorator<Stream> decorator, int bufferSize = 81920, bool leaveOpen = false)
        {
            Validator.ThrowIfNull(decorator);
            Validator.ThrowIfFalse(decorator.Inner.CanRead, nameof(decorator.Inner), "Stream cannot be read from.");
            try
            {
                if (decorator.Inner is MemoryStream s)
                {
                    return s.ToArray();
                }

                using (var memoryStream = new MemoryStream(new byte[decorator.Inner.Length]))
                {
                    var oldPosition = decorator.Inner.Position;
                    if (decorator.Inner.CanSeek)
                    {
                        decorator.Inner.Position = 0;
                    }

                    decorator.Inner.CopyTo(memoryStream, bufferSize);
                    if (decorator.Inner.CanSeek)
                    {
                        decorator.Inner.Position = oldPosition;
                    }

                    return memoryStream.ToArray();
                }
            }
            finally
            {
                if (!leaveOpen)
                {
                    decorator.Inner.Dispose();
                }
            }
        }
    }
}