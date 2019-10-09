using System;
using Cuemon.Text;

namespace Cuemon.Extensions.Text
{
    /// <summary>
    /// Extension methods for the <see cref="string"/> class.
    /// </summary>
    public static class StringExtensions
    {
        /// <summary>
        /// Encodes all the characters in the specified <paramref name="value"/> to its ASCII encoded variant.
        /// </summary>
        /// <param name="value">The <see cref="string"/> to extend.</param>
        /// <param name="setup">The <see cref="EncodingOptions"/> which may be configured.</param>
        /// <returns>A <see cref="string"/> variant of <paramref name="value"/> that is ASCII encoded.</returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="value"/> cannot be null.
        /// </exception>
        /// <seealso cref="AsciiStringEncoder"/>
        public static string ToAsciiEncodedString(this string value, Action<EncodingOptions> setup = null)
        {
            return ConvertFactory.UseEncoder<AsciiStringEncoder>().Encode(value, setup);
        }
    }
}