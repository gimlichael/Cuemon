using System;
using System.IO;
using System.Threading.Tasks;

namespace Cuemon.IO
{
    /// <summary>
    /// Extension methods for the <see cref="TextReader"/> class tailored to adhere the decorator pattern.
    /// </summary>
    /// <seealso cref="IDecorator{T}"/>
    /// <seealso cref="Decorator{T}"/>
    public static class TextReaderDecoratorExtensions
    {
        /// <summary>
        /// Asynchronously reads the bytes from the enclosed <see cref="TextReader"/> of the specified <paramref name="decorator"/> and writes them to the <paramref name="writer"/>.
        /// </summary>
        /// <param name="decorator">The <see cref="IDecorator{TextReader}"/> to extend.</param>
        /// <param name="writer">The <see cref="TextWriter"/> to asynchronously write bytes to.</param>
        /// <param name="bufferSize">The size, in bytes, of the buffer. This value must be greater than zero. The default size is 81920.</param>
        /// <returns>A task that represents the asynchronous copy operation.</returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="decorator"/> cannot be null -or-
        /// <paramref name="writer"/> cannot be null.
        /// </exception>
        /// <exception cref="ArgumentOutOfRangeException">
        /// <paramref name="bufferSize" /> is lower than or equal to 0.
        /// </exception>
        public static async Task CopyToAsync(this IDecorator<TextReader> decorator, TextWriter writer, int bufferSize = 81920)
        {
            Validator.ThrowIfNull(decorator, nameof(decorator));
            Validator.ThrowIfNull(writer, nameof(writer));
            Validator.ThrowIfLowerThanOrEqual(bufferSize, 0, nameof(bufferSize));
            var buffer = new char[bufferSize];
            int read;
            while ((read = await decorator.Inner.ReadAsync(buffer, 0, buffer.Length).ConfigureAwait(false)) != 0)
            {
                await writer.WriteAsync(buffer, 0, read).ConfigureAwait(false);
            }
        }
    }
}