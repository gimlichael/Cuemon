using System.IO;
using Cuemon.IO;

namespace Cuemon.Extensions.IO
{
    /// <summary>
    /// Extension methods for the <see cref="byte"/> struct.
    /// </summary>
    public static class ByteExtensions
    {
        /// <summary>
        /// Converts the specified <paramref name="bytes"/> to a <see cref="Stream"/>.
        /// </summary>
        /// <param name="bytes">The byte array to extend.</param>
        /// <returns>A <see cref="Stream"/> object.</returns>
        public static Stream ToStream(this byte[] bytes)
        {
            return StreamConverter.FromBytes(bytes);
        }
    }
}