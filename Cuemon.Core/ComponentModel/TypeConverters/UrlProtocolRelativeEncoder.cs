using System;

namespace Cuemon.ComponentModel.TypeConverters
{
    /// <summary>
    /// Provides an encoder that converts a <see cref="string"/> representation of a protocol-relative URL to its equivalent <see cref="Uri"/> and vice versa.
    /// </summary>
    public class UrlProtocolRelativeEncoder : IEncoder<string, Uri, UrlProtocolRelativeOptions>
    {
        /// <summary>
        /// Encodes the specified <paramref name="input"/> of a protocol-relative URL to its equivalent <see cref="Uri"/> representation.
        /// </summary>
        /// <param name="input">The <see cref="string"/> to be converted into a <see cref="Uri"/>.</param>
        /// <param name="setup">The <see cref="UrlProtocolRelativeOptions"/> which may be configured.</param>
        /// <returns>A <see cref="Uri"/> that is equivalent to <paramref name="input"/>.</returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="input"/> cannot be null -or-
        /// <see cref="UrlProtocolRelativeOptions.RelativeReference"/> cannot be null.
        /// </exception>
        /// <exception cref="ArgumentException">
        /// <paramref name="input"/> cannot be empty or consist only of white-space characters -or-
        /// <paramref name="input"/> did not start with <see cref="UrlProtocolRelativeOptions.RelativeReference"/> -or-
        /// <see cref="UrlProtocolRelativeOptions.RelativeReference"/> cannot be empty or consist only of white-space characters.
        /// </exception>
        /// <seealso cref="StringUriSchemeConverter"/>
        public Uri Encode(string input, Action<UrlProtocolRelativeOptions> setup = null)
        {
            var options = Patterns.Configure(setup);
            Validator.ThrowIfNullOrWhitespace(input, nameof(input));
            Validator.ThrowIfNullOrWhitespace(options.RelativeReference, nameof(options.RelativeReference));
            Validator.ThrowIfFalse(input.StartsWith(options.RelativeReference, StringComparison.OrdinalIgnoreCase), nameof(input), FormattableString.Invariant($"The specified input did not start with the expected input of: {options.RelativeReference}."));
            var relativeReferenceLength = options.RelativeReference.Length;
            return new Uri(input.Remove(0, relativeReferenceLength).Insert(0, FormattableString.Invariant($"{ConvertFactory.UseConverter<StringUriSchemeConverter>().ChangeType(options.Protocol)}://")));
        }

        /// <summary>
        /// Decodes the specified <paramref name="input"/> to a protocol-relative URL <see cref="string"/> representation.
        /// </summary>
        /// <param name="input">The <see cref="Uri"/> to be converted into a <see cref="string"/>.</param>
        /// <param name="setup">The <see cref="UrlProtocolRelativeOptions"/> which may be configured.</param>
        /// <returns>A <see cref="string"/> that represents a protocol-relative URL from the specified <paramref name="input"/>.</returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="input"/> cannot be null -or-
        /// <paramref name="setup"/> property, <see cref="UrlProtocolRelativeOptions.RelativeReference"/>, cannot be null.
        /// </exception>
        /// <exception cref="ArgumentException">
        /// <paramref name="input"/> must be an absolute URI -or-
        /// <paramref name="setup"/> property, <see cref="UrlProtocolRelativeOptions.RelativeReference"/>, cannot be empty or consist only of white-space characters.
        /// </exception>
        public string Decode(Uri input, Action<UrlProtocolRelativeOptions> setup = null)
        {
            Validator.ThrowIfNull(input, nameof(input));
            Validator.ThrowIfFalse(input.IsAbsoluteUri, nameof(input), "Uri must be absolute.");

            var options = Patterns.Configure(setup);
            Validator.ThrowIfNullOrWhitespace(options.RelativeReference, nameof(options.RelativeReference));

            var schemeLength = input.GetComponents(UriComponents.Scheme | UriComponents.KeepDelimiter, UriFormat.Unescaped).Length;
            return FormattableString.Invariant($"{options.RelativeReference}{input.OriginalString.Remove(0, schemeLength)}");
        }
    }
}