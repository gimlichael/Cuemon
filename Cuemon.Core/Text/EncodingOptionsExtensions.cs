using System.IO;
using System.Text;
using Cuemon.IO;

namespace Cuemon.Text
{
    /// <summary>
    /// Extension methods for the <see cref="EncodingOptions"/> class.
    /// </summary>
    public static class EncodingOptionsExtensions
    {
        /// <summary>
        /// Tries to detect an <see cref="Encoding"/> object from the specified <paramref name="stream"/>.
        /// If unsuccessful, the <see cref="EncodingOptions.Encoding"/> value is returned.
        /// </summary>
        /// <param name="options">The <see cref="IEncodingOptions"/> to extend.</param>
        /// <param name="stream">The <see cref="Stream"/> to parse for an <see cref="Encoding"/>.</param>
        /// <returns>Either the detected encoding of <paramref name="stream"/>  or the encoding of this instance.</returns>
        public static Encoding DetectEncoding(this IEncodingOptions options, Stream stream)
        {
            if (StreamUtility.TryDetectUnicodeEncoding(stream, out var result))
            {
                return result;
            }
            return options.Encoding;
        }

        /// <summary>
        /// Tries to detect an <see cref="Encoding"/> object from the specified <paramref name="bytes"/>.
        /// If unsuccessful, the <see cref="EncodingOptions.Encoding"/> value is returned.
        /// </summary>
        /// <param name="options">The <see cref="IEncodingOptions"/> to extend.</param>
        /// <param name="bytes">The <see cref="byte"/> array to parse for an <see cref="Encoding"/>.</param>
        /// <returns>Either the detected encoding of <paramref name="bytes"/> or the encoding of this instance.</returns>
        public static Encoding DetectEncoding(this IEncodingOptions options, byte[] bytes)
        {
            if (bytes.TryDetectUnicodeEncoding(out var result))
            {
                return result;
            }
            return options.Encoding;
        }
    }
}