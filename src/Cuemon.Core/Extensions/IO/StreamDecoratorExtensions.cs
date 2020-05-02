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
        /// Asynchronously writes a sequence of bytes to the enclosed <see cref="Stream"/> of the <paramref name="decorator"/> with the entire size of the <paramref name="buffer"/> starting from position 0.
        /// </summary>
        /// <param name="decorator">The <see cref="IDecorator{T}"/> to extend.</param>
        /// <param name="buffer">The buffer to write data from.</param>
        /// <returns>A task that represents the asynchronous write operation.</returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="decorator"/> is null -or-
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
        public static Task WriteAsync(this IDecorator<Stream> decorator, byte[] buffer)
        {
            Validator.ThrowIfNull(decorator, nameof(decorator));
            return decorator.Inner.WriteAsync(buffer, 0, buffer.Length);
        }
    }
}