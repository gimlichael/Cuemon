using System.Collections.Generic;
using Cuemon.Collections.Generic;

namespace Cuemon.ComponentModel.TypeConverters
{
    /// <summary>
    /// Provides a converter that converts a <see cref="string"/> to its equivalent <see cref="UriScheme"/>.
    /// </summary>
    public class UriSchemeStringConverter : IConverter<string, UriScheme>
    {
        internal static readonly IDictionary<string, UriScheme> StringToUriSchemeLookupTable = DictionaryConverter.FromEnumerable(StringUriSchemePairs());

        private static IEnumerable<KeyValuePair<string, UriScheme>> StringUriSchemePairs()
        {
            yield return new KeyValuePair<string, UriScheme>("file", UriScheme.File);
            yield return new KeyValuePair<string, UriScheme>("ftp", UriScheme.Ftp);
            yield return new KeyValuePair<string, UriScheme>("gopher", UriScheme.Gopher);
            yield return new KeyValuePair<string, UriScheme>("http", UriScheme.Http);
            yield return new KeyValuePair<string, UriScheme>("https", UriScheme.Https);
            yield return new KeyValuePair<string, UriScheme>("mailto", UriScheme.Mailto);
            yield return new KeyValuePair<string, UriScheme>("net.pipe", UriScheme.NetPipe);
            yield return new KeyValuePair<string, UriScheme>("net.tcp", UriScheme.NetTcp);
            yield return new KeyValuePair<string, UriScheme>("news", UriScheme.News);
            yield return new KeyValuePair<string, UriScheme>("nntp", UriScheme.Nntp);
        }

        /// <summary>
        /// Converts the specified <paramref name="input"/> of an URI scheme to its equivalent <see cref="UriScheme"/> enumeration.
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