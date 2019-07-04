using System.IO;
using Cuemon.ComponentModel.Codecs;

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
        /// <param name="bytes">The <see cref="T:byte[]"/> to extend.</param>
        /// <returns>A <see cref="Stream"/> object.</returns>
        public static Stream ToStream(this byte[] bytes)
        {
            return ConvertFactory.UseCodec<StreamToByteArrayCodec>().Decode(bytes);
        }
    }
}