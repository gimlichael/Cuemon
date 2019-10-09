using System;
using System.Text;
using Cuemon.ComponentModel;
using Cuemon.Integrity;

namespace Cuemon.Text
{
    /// <summary>
    /// Provides an encoder that converts a <see cref="string"/> to an encoded variant.
    /// </summary>
    public class StringEncoder : IEncoder<string, string, FallbackEncodingOptions>
    {
        /// <summary>
        /// Encodes all the characters in the specified <paramref name="input"/> to its encoded <see cref="string"/> variant.
        /// </summary>
        /// <param name="input">The <see cref="string"/> to apply with <see cref="FallbackEncodingOptions.TargetEncoding"/> conversion.</param>
        /// <param name="setup">The <see cref="FallbackEncodingOptions"/> which may be configured.</param>
        /// <returns>A <see cref="string"/> variant of <paramref name="input"/> that is encoded with <see cref="FallbackEncodingOptions.TargetEncoding"/>.</returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="input"/> cannot be null.
        /// </exception>
        /// <remarks>The inspiration for this method was retrieved @ SO: https://stackoverflow.com/a/135473/175073.</remarks>
        /// <seealso cref="FallbackEncodingOptions"/>
        public string Encode(string input, Action<FallbackEncodingOptions> setup = null)
        {
            Validator.ThrowIfNull(input, nameof(input));
            
            var options = Patterns.Configure(setup);
            var result = Encoding.Convert(options.Encoding, Encoding.GetEncoding(options.TargetEncoding.WebName, options.EncoderFallback, options.DecoderFallback), Convertible.GetBytes(input, o =>
            {
                o.Encoding = options.Encoding;
                o.Preamble = options.Preamble;
            }));
            return options.TargetEncoding.GetString(result);
        }
    }
}