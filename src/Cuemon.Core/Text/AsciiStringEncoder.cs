using System;
using System.Text;
using Cuemon.ComponentModel;

namespace Cuemon.Text
{
    /// <summary>
    /// Provides an encoder that converts a <see cref="string"/> to its ASCII encoded variant.
    /// </summary>
    public sealed class AsciiStringEncoder : IEncoder<string, string, EncodingOptions>
    {
        /// <summary>
        /// Encodes all the characters in the specified <paramref name="value"/> to its ASCII encoded variant.
        /// </summary>
        /// <param name="value">The <see cref="string"/> to apply with an ASCII encoding conversion.</param>
        /// <param name="setup">The <see cref="EncodingOptions"/> which may be configured.</param>
        /// <returns>A <see cref="string"/> variant of <paramref name="value"/> that is ASCII encoded.</returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="value"/> cannot be null.
        /// </exception>
        /// <seealso cref="EncodingOptions"/>
        /// <seealso cref="StringEncoder"/>
        public string Encode(string value, Action<EncodingOptions> setup = null)
        {
            Validator.ThrowIfNull(value, nameof(value));

            var options = Patterns.Configure(setup);
            return ConvertFactory.UseEncoder<StringEncoder>().Encode(value, o =>
            {
                o.TargetEncoding = Encoding.ASCII;
                o.Encoding = options.Encoding;
                o.Preamble = options.Preamble;
                o.EncoderFallback = new EncoderReplacementFallback("");
            });
        }
    }
}