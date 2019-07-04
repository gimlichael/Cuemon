using System.Collections.Generic;

namespace Cuemon.ComponentModel.TypeConverters
{
    /// <summary>
    /// Provides a converter that converts a <see cref="string"/> to its equivalent <see cref="UriScheme"/>.
    /// </summary>
    public class UriSchemeStringConverter : IConverter<string, UriScheme>
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
        /// Converts the specified <paramref name="input"/> of a URI scheme to its equivalent <see cref="UriScheme"/> enumeration.
        /// </summary>
        /// <param name="input">The <see cref="string"/> to be converted into a <see cref="UriScheme"/>.</param>
        /// <returns>A <see cref="UriScheme"/> that is equivalent to <paramref name="input"/>.</returns>
        public UriScheme ChangeType(string input)
        {
            if (!StringToUriSchemeLookupTable.TryGetValue(input ?? "", out var result))
            {
                result = UriScheme.Undefined;
            }
            return result;
        }
    }
}