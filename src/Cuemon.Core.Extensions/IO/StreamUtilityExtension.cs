using System.IO;

namespace Cuemon.IO
{
    /// <summary>
    /// This is an extension implementation of the most common methods on the <see cref="StreamUtility"/> class.
    /// </summary>
    public static class StreamUtilityExtension
    {
        /// <summary>
        /// Reads all the bytes from the <paramref name="source"/> stream and writes them to the <paramref name="destination"/> stream.
        /// </summary>
        /// <param name="source">The stream to read the contents from.</param>
        /// <param name="destination">The stream that will contain the contents of the <paramref name="source"/> stream.</param>
        /// <exception cref="T:System.ArgumentNullException"><paramref name="destination"/> is null.</exception>
        /// <exception cref="T:System.NotSupportedException">The <paramref name="source"/> stream does not support reading.-or-<paramref name="destination"/> does not support writing.</exception>
        /// <exception cref="T:System.ObjectDisposedException">Either the <paramref name="source"/> stream or <paramref name="destination"/> were closed before the <see cref="CopyTo(Stream,Stream)"/> method was called.</exception>
        /// <exception cref="T:System.IO.IOException">An I/O error occurred.</exception>
        public static void CopyTo(this Stream source, Stream destination)
        {
            StreamUtility.CopyStream(source, destination);
        }


        /// <summary>
        /// Reads all the bytes from the <paramref name="source"/> stream and writes them to the <paramref name="destination"/> stream, using the specified buffer size of <paramref name="bufferSize"/>.
        /// </summary>
        /// <param name="source">The stream to read the contents from.</param>
        /// <param name="destination">The stream that will contain the contents of the <paramref name="source"/> stream.</param>
        /// <param name="bufferSize">The size of the buffer. This value must be greater than zero. The default size is 2048.</param>
        /// <exception cref="T:System.ArgumentNullException"><paramref name="destination"/> is null.</exception>
        /// <exception cref="T:System.ArgumentOutOfRangeException"><paramref name="bufferSize"/> is negative or zero.</exception>
        /// <exception cref="T:System.NotSupportedException">The <paramref name="source"/> stream does not support reading.-or-<paramref name="destination"/> does not support writing.</exception>
        /// <exception cref="T:System.ObjectDisposedException">Either the <paramref name="source"/> stream or <paramref name="destination"/> were closed before the <see cref="CopyTo(Stream,Stream)"/> method was called.</exception>
        /// <exception cref="T:System.IO.IOException">An I/O error occurred.</exception>
        public static void CopyTo(this Stream source, Stream destination, int bufferSize)
        {
            StreamUtility.CopyStream(source, destination, bufferSize);
        }


        /// <summary>
        /// Creates and returns a seekable copy of the source <see cref="Stream"/>.
        /// </summary>
        /// <param name="source">The source <see cref="Stream"/> to create a copy from.</param>
        /// <returns>A seekable <see cref="Stream"/> that will contain the contents of the source stream.</returns>
        public static Stream Copy(this Stream source)
        {
            return StreamUtility.CopyStream(source);
        }

        /// <summary>
        /// Creates and returns a seekable copy of the source <see cref="Stream"/>.
        /// </summary>
        /// <param name="source">The source <see cref="Stream"/> to create a copy from.</param>
        /// <param name="leaveStreamOpen">if <c>true</c>, the source <see cref="Stream"/> is being left open; otherwise it is being closed and disposed.</param>
        /// <returns>A seekable <see cref="Stream"/> that will contain the contents of the source stream.</returns>
        public static Stream Copy(this Stream source, bool leaveStreamOpen)
        {
            return StreamUtility.CopyStream(source, leaveStreamOpen);
        }

        /// <summary>
        /// Creates and returns a seekable copy of the source <see cref="Stream"/>.
        /// </summary>
        /// <param name="source">The source <see cref="Stream"/> to create a copy from.</param>
        /// <param name="leaveStreamOpen">if <c>true</c>, the source <see cref="Stream"/> is being left open; otherwise it is being closed and disposed.</param>
        /// <param name="bufferSize">The size of the buffer. This value must be greater than zero. The default size is 2048.</param>
        /// <returns>A seekable <see cref="Stream"/> that will contain the contents of the <paramref name="source"/> stream.</returns>
        public static Stream Copy(this Stream source, bool leaveStreamOpen, int bufferSize)
        {
            return StreamUtility.CopyStream(source, leaveStreamOpen, bufferSize);
        }
    }
}