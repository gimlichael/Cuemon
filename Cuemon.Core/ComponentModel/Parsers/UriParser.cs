using System;
using System.ComponentModel;
using Cuemon.ComponentModel.TypeConverters;

namespace Cuemon.ComponentModel.Parsers
{
    /// <summary>
    /// Provides a parser that converts a <see cref="string"/> to its equivalent <see cref="Uri"/>.
    /// </summary>
    public class UriParser : IParser<Uri, UriOptions>
    {
        /// <summary>
        /// Converts the string representation of a URI to its <see cref="Uri"/> equivalent.
        /// </summary>
        /// <param name="input">The <see cref="string"/> to convert.</param>
        /// <param name="setup">The <see cref="UriOptions"/> which may be configured.</param>
        /// <returns>A <see cref="Uri"/> equivalent to <paramref name="input"/>.</returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="input"/> cannot be null.
        /// </exception>
        /// <exception cref="ArgumentException">
        /// <paramref name="input"/> cannot be empty or consist only of white-space characters.
        /// </exception>
        /// <seealso cref="StringUriSchemeConverter"/>
        public Uri Parse(string input, Action<UriOptions> setup = null)
        {
            Validator.ThrowIfNullOrWhitespace(input, nameof(input));
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
                        var validUriScheme = ConvertFactory.UseConverter<StringUriSchemeConverter>().ChangeType(scheme);
                        isValid = input.StartsWith(validUriScheme, StringComparison.OrdinalIgnoreCase);
                        break;
                    default:
                        throw new InvalidEnumArgumentException(nameof(options.Schemes), (int)scheme, typeof(UriScheme));
                }
                if (isValid) { break; }
            }
            if (!isValid || !Uri.TryCreate(input, options.Kind, out var result)) { throw new ArgumentException("The specified value is not a valid URI.", nameof(input)); }
            return result;
        }

        /// <summary>
        /// Converts the string representation of a URI to its <see cref="Uri"/> equivalent. A return value indicates whether the conversion succeeded.
        /// </summary>
        /// <param name="input">The <see cref="string"/> to convert.</param>
        /// <param name="result">When this method returns, contains the <see cref="Uri"/> equivalent of the <paramref name="input"/>, if the conversion succeeded, or <c>default</c> if the conversion failed.</param>
        /// <param name="setup">The <see cref="UriOptions"/> which may be configured.</param>
        /// <returns><c>true</c> if <paramref name="input"/> was converted successfully; otherwise, <c>false</c>.</returns>
        public bool TryParse(string input, out Uri result, Action<UriOptions> setup = null)
        {
            return Patterns.TryInvoke(() => Parse(input, setup), out result);
        }

        /// <summary>
        /// Indicates whether the <paramref name="input"/> is valid by attempting to construct a URI.
        /// </summary>
        /// <param name="input">The <see cref="string"/> used to attempt to construct a <see cref="Uri"/>.</param>
        /// <param name="setup">The <see cref="UriOptions"/> which may be configured.</param>
        /// <returns><c>true</c> if the <paramref name="input"/> is a valid URI; otherwise, <c>false</c>.</returns>
        public bool IsUri(string input, Action<UriOptions> setup = null)
        {
            return TryParse(input, out _, setup);
        }
    }
}