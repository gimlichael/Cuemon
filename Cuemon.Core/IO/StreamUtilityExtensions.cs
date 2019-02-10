using System.IO;

namespace Cuemon.IO
{
    /// <summary>
    /// This is an extension implementation of the most common methods on the <see cref="StreamUtility"/> class.
    /// </summary>
    public static class StreamUtilityExtensions
    {
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