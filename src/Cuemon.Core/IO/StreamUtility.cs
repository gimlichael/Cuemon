using System;
using System.Collections.Generic;
using System.IO;

namespace Cuemon.IO
{
    /// <summary>
    /// This utility class is designed to make <see cref="Stream"/> operations easier to work with.
    /// </summary>
    public static class StreamUtility
    {
		/// <summary>
		/// Reads all the bytes from the <paramref name="source"/> stream and writes them to the <paramref name="destination"/> stream.
		/// </summary>
		/// <param name="source">The stream to read the contents from.</param>
		/// <param name="destination">The stream that will contain the contents of the <paramref name="source"/> stream.</param>
		public static void CopyStream(Stream source, Stream destination)
		{
			CopyStream(source, destination, Infrastructure.DefaultBufferSize);
		}


		/// <summary>
		/// Reads all the bytes from the <paramref name="source"/> stream and writes them to the <paramref name="destination"/> stream, using the specified buffer size of <paramref name="bufferSize"/>.
		/// </summary>
		/// <param name="source">The stream to read the contents from.</param>
		/// <param name="destination">The stream that will contain the contents of the <paramref name="source"/> stream.</param>
		/// <param name="bufferSize">The size of the buffer. This value must be greater than zero. The default size is 2048.</param>
		public static void CopyStream(Stream source, Stream destination, int bufferSize)
		{
            Validator.ThrowIfNull(source, nameof(source));
            Validator.ThrowIfNull(source, nameof(destination));
            Validator.ThrowIfLowerThanOrEqual(bufferSize, 0, nameof(bufferSize), "The buffer size is negative or zero.");
            Validator.ThrowIfTrue(!source.CanRead && !source.CanWrite, nameof(source), "Source stream appears to be disposed.");
            Validator.ThrowIfTrue(!destination.CanRead && !destination.CanWrite, nameof(destination), "Destination stream appears to be disposed.");
            Validator.ThrowIfFalse(source.CanRead, nameof(source), "Source stream cannot be read from.");
            Validator.ThrowIfFalse(destination.CanWrite, nameof(destination), "Destination stream cannot be written to.");

            Infrastructure.WhileSourceReadDestionationWrite(source, destination, bufferSize);
		}

        /// <summary>
        /// Creates and returns a seekable copy of the source <see cref="Stream"/>.
        /// </summary>
        /// <param name="source">The source <see cref="Stream"/> to create a copy from.</param>
        /// <returns>A seekable <see cref="Stream"/> that will contain the contents of the source stream.</returns>
        public static Stream CopyStream(Stream source)
        {
            return CopyStream(source, false);
        }

        /// <summary>
        /// Creates and returns a seekable copy of the source <see cref="Stream"/>.
        /// </summary>
        /// <param name="source">The source <see cref="Stream"/> to create a copy from.</param>
        /// <param name="leaveStreamOpen">if <c>true</c>, the source <see cref="Stream"/> is being left open; otherwise it is being closed and disposed.</param>
        /// <returns>A seekable <see cref="Stream"/> that will contain the contents of the <paramref name="source"/> stream.</returns>
        public static Stream CopyStream(Stream source, bool leaveStreamOpen)
        {
            return CopyStream(source, leaveStreamOpen, Infrastructure.DefaultBufferSize);
        }

        /// <summary>
        /// Creates and returns a seekable copy of the source <see cref="Stream"/>.
        /// </summary>
        /// <param name="source">The source <see cref="Stream"/> to create a copy from.</param>
        /// <param name="leaveStreamOpen">if <c>true</c>, the source <see cref="Stream"/> is being left open; otherwise it is being closed and disposed.</param>
        /// <param name="bufferSize">The size of the buffer. This value must be greater than zero. The default size is 2048.</param>
        /// <returns>A seekable <see cref="Stream"/> that will contain the contents of the <paramref name="source"/> stream.</returns>
        public static Stream CopyStream(Stream source, bool leaveStreamOpen, int bufferSize)
        {
            Validator.ThrowIfNull(source, nameof(source));
            Validator.ThrowIfNull(source, "destination");
            Validator.ThrowIfLowerThanOrEqual(bufferSize, 0, nameof(bufferSize), "The buffer size is negative or zero.");
            Validator.ThrowIfFalse(source.CanRead, nameof(source), "Source stream cannot be read from.");

            MemoryStream destination = new MemoryStream();
            try
            {
                Infrastructure.WhileSourceReadDestionationWrite(source, destination, bufferSize, true);
            }
            finally
            {
                if (!leaveStreamOpen) { source.Dispose(); }
            }
            return destination;
        }

        /// <summary>
        /// Combines a variable number of streams into one stream.
        /// </summary>
        /// <param name="streams">The streams to combine.</param>
        /// <returns>A variable number of <b>streams</b> combined into one <b>stream</b>.</returns>
        public static Stream CombineStreams(params Stream[] streams)
        {
            if (streams == null) throw new ArgumentNullException(nameof(streams));
            List<byte[]> bytes = new List<byte[]>();
            foreach (Stream stream in streams)
            {
                byte[] bytesFromStream = new byte[stream.Length];
                using (stream) // close and dispose the stream, as we are returning a new combined stream
                {
                    stream.Read(bytesFromStream, 0, (int)stream.Length);
                    bytes.Add(bytesFromStream);
                }
            }

            MemoryStream output;
            MemoryStream tempOutput = null;
            try
            {
                tempOutput = new MemoryStream(ByteUtility.CombineByteArrays(bytes.ToArray()));
                tempOutput.Position = 0;
                output = tempOutput;
                tempOutput = null;
            }
            finally 
            {
                if (tempOutput != null) { tempOutput.Dispose(); }
            }
            output.Position = 0;
            return output;
        }
    }
}