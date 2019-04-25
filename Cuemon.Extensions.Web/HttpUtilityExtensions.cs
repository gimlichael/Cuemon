using System.Text;
using Cuemon.Web;

namespace Cuemon.Extensions.Web
{
    /// <summary>
    /// This is an extension implementation of the most common methods on the <see cref="HttpUtility"/> class.
    /// </summary>
    public static class HttpUtilityExtensions
    {
        /// <summary>
        /// Encodes a URL string.
        /// </summary>
        /// <param name="value">The text to encode.</param>
        /// <returns>An encoded string.</returns>
        public static string UrlEncode(this string value)
        {
            return UrlEncode(value, Encoding.UTF8);
        }

        /// <summary>
        /// Encodes a URL string, using the specified <paramref name="encoding"/> object.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <param name="encoding">The <see cref="Encoding"/> that specifies the encoding scheme.</param>
        /// <returns>An encoded string.</returns>
        public static string UrlEncode(this string value, Encoding encoding)
        {
            return HttpUtility.UrlEncode(value, encoding);
        }

        /// <summary>
        /// Converts a string that has been encoded for transmission in a URL into a decoded string.
        /// </summary>
        /// <param name="value">The string to decode.</param>
        /// <returns>A decoded string.</returns>
        public static string UrlDecode(this string value)
        {
            return UrlDecode(value, Encoding.UTF8);
        }

        /// <summary>
        /// Converts a string that has been encoded for transmission in a URL into a decoded string, using the specified <paramref name="encoding"/> object.
        /// </summary>
        /// <param name="value">The string to decode.</param>
        /// <param name="encoding">The <see cref="Encoding"/> that specifies the decoding scheme.</param>
        /// <returns>A decoded string.</returns>
        public static string UrlDecode(this string value, Encoding encoding)
        {
            return HttpUtility.UrlDecode(value, encoding);
        }
    }
}