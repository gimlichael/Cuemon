using System;
using System.Text;

namespace Cuemon.Text
{
    /// <summary>
    /// Extension methods for the <see cref="Encoding"/> class.
    /// </summary>
    public static class EncodingExtensions
    {
        /// <summary>
        /// Converts the specified <paramref name="value"/> to its equivalent <paramref name="encoding"/> representation.
        /// </summary>
        /// <param name="encoding">The encoding to use in the string that is returned.</param>
        /// <param name="value">The value to convert into the specified <paramref name="encoding"/>.</param>
        /// <param name="setup">The <see cref="FallbackEncodingOptions"/> which need to be configured.</param>
        /// <returns>A string that is encoded according to the value of <paramref name="encoding"/>.</returns>
        /// <remarks>The inspiration for this method was retrieved @ SO: https://stackoverflow.com/a/135473/175073.</remarks>
        public static string ToEncodedString(this Encoding encoding, string value, Action<FallbackEncodingOptions> setup = null)
        {
            Validator.ThrowIfNull(encoding, nameof(encoding));
            if (value.IsNullOrEmpty()) { return value; }
            var options = setup.ConfigureOptions();
            var result = Encoding.Convert(options.Encoding, Encoding.GetEncoding(encoding.EncodingName, options.EncoderFallback, options.DecoderFallback), value.ToByteArray(o =>
            {
                o.Encoding = options.Encoding;
                o.Preamble = options.Preamble;
            }));
            return encoding.GetString(result);
        }
    }
}