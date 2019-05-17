using System;

namespace Cuemon.ComponentModel.TypeConverters
{
    /// <summary>
    /// Provides a converter that converts a <see cref="string"/>, represented as a protocol-relative URL, to its equivalent <see cref="Uri"/>.
    /// </summary>
    public class UrlProtocolRelativeStringConverter : IConverter<string, Uri, UrlProtocolRelativeStringOptions>
    {
        /// <summary>
        /// Converts the specified <paramref name="input"/> of a protocol-relative URL to its equivalent <see cref="Uri"/> representation.
        /// </summary>
        /// <param name="input">The <see cref="string"/> to be converted into a <see cref="Uri"/>.</param>
        /// <param name="setup">The <see cref="UrlProtocolRelativeStringOptions"/> which may be configured.</param>
        /// <returns>A <see cref="Uri"/> that is equivalent to <paramref name="input"/>.</returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="input"/> cannot be null -or-
        /// <see cref="UrlProtocolRelativeStringOptions.RelativeReference"/> cannot be null.
        /// </exception>
        /// <exception cref="ArgumentException">
        /// <paramref name="input"/> cannot be empty or consist only of white-space characters -or-
        /// <paramref name="input"/> did not start with <see cref="UrlProtocolRelativeStringOptions.RelativeReference"/> -or-
        /// <see cref="UrlProtocolRelativeStringOptions.RelativeReference"/> cannot be empty or consist only of white-space characters.
        /// </exception>
        public Uri ChangeType(string input, Action<UrlProtocolRelativeStringOptions> setup = null)
        {
            var options = Patterns.Configure(setup);
            Validator.ThrowIfNullOrWhitespace(input, nameof(input));
            Validator.ThrowIfNullOrWhitespace(options.RelativeReference, nameof(options.RelativeReference));
            Validator.ThrowIfFalse(input.StartsWith(options.RelativeReference, StringComparison.OrdinalIgnoreCase), nameof(input), FormattableString.Invariant($"The specified input did not start with the expected input of: {options.RelativeReference}."));
            var relativeReferenceLength = options.RelativeReference.Length;
            return new Uri(input.Remove(0, relativeReferenceLength).Insert(0, FormattableString.Invariant($"{StringConverter.FromUriScheme(options.Protocol)}://")));
        }
    }
}