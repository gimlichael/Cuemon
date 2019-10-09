using System;
using System.ComponentModel;

namespace Cuemon.Text
{
    /// <summary>
    /// Provides a parser that converts a <see cref="string"/> to its equivalent <see cref="Uri"/>.
    /// </summary>
    public class SimpleUriParser : IParser<Uri, SimpleUriOptions>
    {
        /// <summary>
        /// Converts the string representation of a URI to its <see cref="Uri"/> equivalent.
        /// </summary>
        /// <param name="value">The <see cref="string"/> to convert.</param>
        /// <param name="setup">The <see cref="SimpleUriOptions"/> which may be configured.</param>
        /// <returns>A <see cref="Uri"/> equivalent to <paramref name="value"/>.</returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="value"/> cannot be null.
        /// </exception>
        /// <exception cref="ArgumentException">
        /// <paramref name="value"/> cannot be empty or consist only of white-space characters.
        /// </exception>
        public Uri Parse(string value, Action<SimpleUriOptions> setup = null)
        {
            Validator.ThrowIfNullOrWhitespace(value, nameof(value));
            var options = Patterns.Configure(setup);
            var isValid = false;
            foreach (var scheme in options.Schemes)
            {
                switch (scheme)
                {
                    case UriScheme.Undefined:
                        break;
                    case UriScheme.File:
                    case UriScheme.Ftp:
                    case UriScheme.Gopher:
                    case UriScheme.Http:
                    case UriScheme.Https:
                    case UriScheme.Mailto:
                    case UriScheme.NetPipe:
                    case UriScheme.NetTcp:
                    case UriScheme.News:
                    case UriScheme.Nntp:
                        var validUriScheme = StringFactory.CreateUriScheme(scheme);
                        isValid = value.StartsWith(validUriScheme, StringComparison.OrdinalIgnoreCase);
                        break;
                    default:
                        throw new InvalidEnumArgumentException(nameof(options.Schemes), (int)scheme, typeof(UriScheme));
                }
                if (isValid) { break; }
            }
            if (!isValid || !Uri.TryCreate(value, options.Kind, out var result)) { throw new ArgumentException("The specified value is not a valid URI.", nameof(value)); }
            return result;
        }

        /// <summary>
        /// Converts the string representation of a URI to its <see cref="Uri"/> equivalent. A return value indicates whether the conversion succeeded.
        /// </summary>
        /// <param name="value">The <see cref="string"/> to convert.</param>
        /// <param name="result">When this method returns, contains the <see cref="Uri"/> equivalent of the <paramref name="value"/>, if the conversion succeeded, or <c>default</c> if the conversion failed.</param>
        /// <param name="setup">The <see cref="SimpleUriOptions"/> which may be configured.</param>
        /// <returns><c>true</c> if <paramref name="value"/> was converted successfully; otherwise, <c>false</c>.</returns>
        public bool TryParse(string value, out Uri result, Action<SimpleUriOptions> setup = null)
        {
            return Patterns.TryInvoke(() => Parse(value, setup), out result);
        }
    }
}