using System;
using Cuemon.Text;

namespace Cuemon.ComponentModel.Codecs
{
    /// <summary>
    /// Provides an encoder that converts a <see cref="string"/> representation of a protocol-relative URL to its equivalent <see cref="Uri"/> and a vice versa decoder.
    /// </summary>
    public class UrlProtocolRelativeCodec : ICodec<string, Uri, ProtocolRelativeUrlOptions>
    {
        /// <summary>
        /// Encodes the specified <paramref name="input"/> of a protocol-relative URL to its <see cref="Uri"/> representation.
        /// </summary>
        /// <param name="input">The <see cref="string"/> to be converted into a <see cref="Uri"/>.</param>
        /// <param name="setup">The <see cref="ProtocolRelativeUrlOptions"/> which may be configured.</param>
        /// <returns>A <see cref="Uri"/> that is similiar to <paramref name="input"/>.</returns>
        /// <seealso cref="ProtocolRelativeUrlParser"/>
        public Uri Encode(string input, Action<ProtocolRelativeUrlOptions> setup = null)
        {
            return ParserFactory.CreateProtocolRelativeUrlParser().Parse(input, setup);
        }

        /// <summary>
        /// Decodes the specified <paramref name="input"/> to a protocol-relative URL <see cref="string"/> representation.
        /// </summary>
        /// <param name="input">The <see cref="Uri"/> to be converted into a <see cref="string"/>.</param>
        /// <param name="setup">The <see cref="ProtocolRelativeUrlOptions"/> which may be configured.</param>
        /// <returns>A <see cref="string"/> that represents a protocol-relative URL from the specified <paramref name="input"/>.</returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="input"/> cannot be null.
        /// </exception>
        /// <exception cref="ArgumentException">
        /// <paramref name="input"/> must be an absolute URI.
        /// </exception>
        public string Decode(Uri input, Action<ProtocolRelativeUrlOptions> setup = null)
        {
            Validator.ThrowIfNull(input, nameof(input));
            Validator.ThrowIfFalse(input.IsAbsoluteUri, nameof(input), "Uri must be absolute.");
            var options = Patterns.Configure(setup);
            var schemeLength = input.GetComponents(UriComponents.Scheme | UriComponents.KeepDelimiter, UriFormat.Unescaped).Length;
            return FormattableString.Invariant($"{options.RelativeReference}{input.OriginalString.Remove(0, schemeLength)}");
        }
    }
}