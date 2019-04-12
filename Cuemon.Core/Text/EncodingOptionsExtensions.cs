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
        /// Tries to detect an <see cref="Encoding"/> object from the specified <paramref name="value"/>.
        /// If unsuccessful, the <see cref="EncodingOptions.Encoding"/> value is returned.
        /// </summary>
        /// <param name="options">The <see cref="EncodingOptions"/> to extend.</param>
        /// <param name="value">The <see cref="Stream"/> to parse for an <see cref="Encoding"/>.</param>
        /// <returns>Either the detected encoding of <paramref name="value"/>  or the encoding of this instance.</returns>
        public static Encoding DetectEncoding(this EncodingOptions options, Stream value)
        {
            if (value.TryDetectUnicodeEncoding(out var result))
            {
                return result;
            }
            return options.Encoding;
        }

        /// <summary>
        /// Tries to detect an <see cref="Encoding"/> object from the specified <paramref name="value"/>.
        /// If unsuccessful, the <see cref="EncodingOptions.Encoding"/> value is returned.
        /// </summary>
        /// <param name="options">The <see cref="EncodingOptions"/> to extend.</param>
        /// <param name="value">The <see cref="byte"/> array to parse for an <see cref="Encoding"/>.</param>
        /// <returns>Either the detected encoding of <paramref name="value"/> or the encoding of this instance.</returns>
        public static Encoding DetectEncoding(this EncodingOptions options, byte[] value)
        {
            if (value.TryDetectUnicodeEncoding(out var result))
            {
                return result;
            }
            return options.Encoding;
        }
    }
}