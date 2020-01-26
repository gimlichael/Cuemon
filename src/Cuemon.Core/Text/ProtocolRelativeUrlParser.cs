using System;

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
        /// <param name="value">The <see cref="string"/> to convert.</param>
        /// <param name="setup">The <see cref="ProtocolRelativeUrlOptions"/> which may be configured.</param>
        /// <returns>A <see cref="Uri"/> similar to <paramref name="value"/>.</returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="value"/> cannot be null.
        /// </exception>
        /// <exception cref="ArgumentException">
        /// <paramref name="value"/> cannot be empty or consist only of white-space characters.
        /// </exception>
        public Uri Parse(string value, Action<ProtocolRelativeUrlOptions> setup = null)
        {
            Validator.ThrowIfNullOrWhitespace(value, nameof(value));
            var options = Patterns.Configure(setup, validator: o =>
            {
                Validator.ThrowIfFalse(value.StartsWith(o.RelativeReference, StringComparison.OrdinalIgnoreCase), nameof(value), FormattableString.Invariant($"The specified value did not start with the expected value of: {o.RelativeReference}."));
            });
            
            var relativeReferenceLength = options.RelativeReference.Length;
            return new Uri(value.Remove(0, relativeReferenceLength).Insert(0, FormattableString.Invariant($"{StringFactory.CreateUriScheme(options.Protocol)}://")));
        }

        /// <summary>
        /// Converts the string representation of a protocol relative URI to a <see cref="Uri"/>. A return value indicates whether the conversion succeeded.
        /// </summary>
        /// <param name="value">The <see cref="string"/> to convert.</param>
        /// <param name="result">When this method returns, contains the <see cref="Uri"/> of the <paramref name="value"/>, if the conversion succeeded, or <c>default</c> if the conversion failed.</param>
        /// <param name="setup">The <see cref="ProtocolRelativeUrlOptions"/> which may be configured.</param>
        /// <returns><c>true</c> if <paramref name="value"/> was converted successfully; otherwise, <c>false</c>.</returns>
        public bool TryParse(string value, out Uri result, Action<ProtocolRelativeUrlOptions> setup = null)
        {
            return Patterns.TryInvoke(() => Parse(value, setup), out result);
        }
    }
}