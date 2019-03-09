using System;
using System.Text;

namespace Cuemon.Text
{
    /// <summary>
    /// This utility class is designed to make <see cref="Encoding"/> conversions easier to work with.
    /// </summary>
    public static class EncodingConverter
    {
        /// <summary>
        /// Converts the specified <paramref name="value"/> to its equivalent ASCII encoded representation.
        /// </summary>
        /// <param name="value">The value to apply with an ASCII encoding conversion.</param>
        /// <param name="setup">The <see cref="EncodingOptions"/> which need to be configured.</param>
        /// <returns>A string that is ASCII encoded.</returns>
        public static string ToAsciiEncodedString(string value, Action<EncodingOptions> setup = null)
        {
            var options = setup.Configure();
            return Encoding.ASCII.ToEncodedString(value, o =>
            {
                o.Encoding = options.Encoding;
                o.Preamble = options.Preamble;
                o.EncoderFallback = new EncoderReplacementFallback("");
            });
        }
    }
}