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
        /// Encodes all the characters in the specified <paramref name="input"/> to its ASCII encoded <see cref="string"/> variant.
        /// </summary>
        /// <param name="input">The <see cref="string"/> to apply with an ASCII encoding conversion.</param>
        /// <param name="setup">The <see cref="EncodingOptions"/> which may be configured.</param>
        /// <returns>A <see cref="string"/> variant of <paramref name="input"/> that is ASCII encoded.</returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="input"/> cannot be null.
        /// </exception>
        /// <seealso cref="EncodingOptions"/>
        /// <seealso cref="StringEncoder"/>
        public string Encode(string input, Action<EncodingOptions> setup = null)
        {
            Validator.ThrowIfNull(input, nameof(input));

            var options = Patterns.Configure(setup);
            return ConvertFactory.UseEncoder<StringEncoder>().Encode(input, o =>
            {
                o.TargetEncoding = Encoding.ASCII;
                o.Encoding = options.Encoding;
                o.Preamble = options.Preamble;
                o.EncoderFallback = new EncoderReplacementFallback("");
            });
        }
    }
}