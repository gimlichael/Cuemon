using System;
using Cuemon.ComponentModel.TypeConverters;

namespace Cuemon.Text
{
    /// <summary>
    /// Provides a parser that converts a <see cref="string"/> to a <see cref="Uri"/>.
    /// </summary>
    public class ProtocolRelativeUrlParser : IParser<Uri, ProtocolRelativeUrlOptions>
    {
        /// <summary>
        /// Converts the string representation of a protocol relative URI to a <see cref="Uri"/>.
        /// </summary>
        /// <param name="input">The <see cref="string"/> to convert.</param>
        /// <param name="setup">The <see cref="ProtocolRelativeUrlOptions"/> which may be configured.</param>
        /// <returns>A <see cref="Uri"/> similar to <paramref name="input"/>.</returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="input"/> cannot be null.
        /// </exception>
        /// <exception cref="ArgumentException">
        /// <paramref name="input"/> cannot be empty or consist only of white-space characters.
        /// </exception>
        /// <seealso cref="StringUriSchemeConverter"/>
        public Uri Parse(string input, Action<ProtocolRelativeUrlOptions> setup = null)
        {
            Validator.ThrowIfNullOrWhitespace(input, nameof(input));
            var options = Patterns.Configure(setup, validator: o =>
            {
                Validator.ThrowIfFalse(input.StartsWith(o.RelativeReference, StringComparison.OrdinalIgnoreCase), nameof(input), FormattableString.Invariant($"The specified input did not start with the expected input of: {o.RelativeReference}."));
            });
            
            var relativeReferenceLength = options.RelativeReference.Length;
            return new Uri(input.Remove(0, relativeReferenceLength).Insert(0, FormattableString.Invariant($"{ConvertFactory.UseConverter<StringUriSchemeConverter>().ChangeType(options.Protocol)}://")));
        }

        /// <summary>
        /// Converts the string representation of a protocol relative URI to a <see cref="Uri"/>. A return value indicates whether the conversion succeeded.
        /// </summary>
        /// <param name="input">The <see cref="string"/> to convert.</param>
        /// <param name="result">When this method returns, contains the <see cref="Uri"/> of the <paramref name="input"/>, if the conversion succeeded, or <c>default</c> if the conversion failed.</param>
        /// <param name="setup">The <see cref="ProtocolRelativeUrlOptions"/> which may be configured.</param>
        /// <returns><c>true</c> if <paramref name="input"/> was converted successfully; otherwise, <c>false</c>.</returns>
        public bool TryParse(string input, out Uri result, Action<ProtocolRelativeUrlOptions> setup = null)
        {
            return Patterns.TryInvoke(() => Parse(input, setup), out result);
        }
    }
}