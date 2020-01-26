using System;
using System.Collections.Generic;

namespace Cuemon.Text
{
    /// <summary>
    /// Provides a parser that converts a <see cref="string"/>, represented as an URI scheme, to its equivalent <see cref="UriScheme"/>.
    /// </summary>
    public class UriSchemeParser : IParser<UriScheme>
    {
        internal static readonly IDictionary<string, UriScheme> StringToUriSchemeLookupTable = new Dictionary<string, UriScheme>()
        {
            { "file", UriScheme.File },
            { "ftp", UriScheme.Ftp },
            { "gopher", UriScheme.Gopher },
            { "http", UriScheme.Http },
            { "https", UriScheme.Https },
            { "mailto", UriScheme.Mailto },
            { "net.pipe", UriScheme.NetPipe },
            { "net.tcp", UriScheme.NetTcp },
            { "news", UriScheme.News },
            { "nntp", UriScheme.Nntp }
        };

        /// <summary>
        /// Converts the string representation of an URI scheme to its <see cref="UriScheme"/> equivalent.
        /// </summary>
        /// <param name="value">A string consisting of an URI scheme.</param>
        /// <returns>A <see cref="UriScheme"/> equivalent to <paramref name="value"/>.</returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="value"/> cannot be null.
        /// </exception>
        /// <exception cref="ArgumentException">
        /// <paramref name="value"/> cannot be empty or consist only of white-space characters.
        /// </exception>
        public UriScheme Parse(string value)
        {
            Validator.ThrowIfNullOrWhitespace(value, nameof(value));
            if (!StringToUriSchemeLookupTable.TryGetValue(value ?? "", out var result))
            {
                result = UriScheme.Undefined;
            }
            return result;
        }

        /// <summary>
        /// Converts the string representation of an URI scheme to its <see cref="UriScheme"/> equivalent. A return value indicates whether the conversion succeeded.
        /// </summary>
        /// <param name="value">A string consisting of an URI scheme.</param>
        /// <param name="result">When this method returns, contains the <see cref="UriScheme"/> equivalent of the URI scheme contained within <paramref name="value"/>, if the conversion succeeded, or <c>default</c> if the conversion failed.</param>
        /// <returns><c>true</c> if <paramref name="value"/> was converted successfully; otherwise, <c>false</c>.</returns>
        public bool TryParse(string value, out UriScheme result)
        {
            return Patterns.TryInvoke(() => Parse(value), out result);
        }
    }
}