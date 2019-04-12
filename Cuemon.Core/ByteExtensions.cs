using System;
using System.Text;

namespace Cuemon
{
    /// <summary>
    /// Extension methods for the <see cref="byte"/> struct.
    /// </summary>
    public static class ByteExtensions
    {
        /// <summary>
        /// Converts an array of 8-bit unsigned integers to its equivalent string representation that is encoded with base-64 digits.
        /// </summary>
        /// <param name="bytes">An array of 8-bit unsigned integers.</param>
        /// <returns>The string representation, in base 64, of the contents of <paramref name="bytes"/>.</returns>
        public static string ToBase64(this byte[] bytes)
        {
            return Convert.ToBase64String(bytes);
        }

        /// <summary>
        /// Tries to resolve the Unicode <see cref="Encoding"/> object from the specified <see cref="byte"/> array.
        /// </summary>
        /// <param name="bytes">The <see cref="byte"/> array to resolve the Unicode <see cref="Encoding"/> object from.</param>
        /// <param name="result">When this method returns, it contains the Unicode <see cref="Encoding"/> value equivalent to the encoding contained in <paramref name="bytes"/>, if the conversion succeeded, or a null reference (Nothing in Visual Basic) if the conversion failed. The conversion fails if the <paramref name="bytes"/> parameter is null, or does not contain a Unicode representation of an <see cref="Encoding"/>.</param>
        /// <returns><c>true</c> if the <paramref name="bytes"/> parameter was converted successfully; otherwise, <c>false</c>.</returns>
        public static bool TryDetectUnicodeEncoding(this byte[] bytes, out Encoding result)
        {
            if (bytes == null) { throw new ArgumentNullException(nameof(bytes)); }
            result = null;
            if (bytes.Length >= 4)
            {

                if (bytes[0] == 0xEF &&
                    bytes[1] == 0xBB &&
                    bytes[2] == 0xBF)
                {
                    result = Encoding.GetEncoding("UTF-8");
                }
                else if (bytes[0] == 0x00 &&
                         bytes[1] == 0x00 &&
                         bytes[2] == 0xFE &&
                         bytes[3] == 0xFF)
                {
                    result = Encoding.GetEncoding("UTF-32BE");
                }
                else if (bytes[0] == 0xFF &&
                         bytes[1] == 0xFE &&
                         bytes[2] == 0x00 &&
                         bytes[3] == 0x00)
                {
                    result = Encoding.GetEncoding("UTF-32");
                }
                else if (bytes[0] == 0xFE &&
                         bytes[1] == 0xFF)
                {
                    result = Encoding.GetEncoding("UNICODEFFFE");
                }
                else if (bytes[0] == 0xFF &&
                         bytes[1] == 0xFE)
                {
                    result = Encoding.GetEncoding("UTF-16");
                }
                else if (bytes[0] == 0x2B &&
                         bytes[1] == 0x2F &&
                         bytes[2] == 0x76 &&
                         (bytes[3] == 0x38 ||
                          bytes[3] == 0x39 ||
                          bytes[3] == 0x2B ||
                          bytes[3] == 0x2F))
                {
                    result = Encoding.GetEncoding("UTF-7");
                }
            }
            return (result != null);
        }
    }
}