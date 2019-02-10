using System.Collections.Generic;
using Cuemon.Collections.Generic;

namespace Cuemon
{
    /// <summary>
    /// This utility class is designed to make <see cref="UriScheme"/> related conversions easier to work with.
    /// </summary>
    public static class UriSchemeConverter
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
        /// Converts the specified string representation of an URI scheme to its <see cref="UriScheme"/> equivalent.
        /// </summary>
        /// <param name="uriScheme">A string containing an URI scheme to convert.</param>
        /// <returns>An <see cref="UriScheme"/> equivalent to the specified <paramref name="uriScheme"/> or <see cref="UriScheme.Undefined"/> if a conversion is not possible.</returns>
        public static UriScheme FromString(string uriScheme)
        {
            UriScheme result;
            if (!StringToUriSchemeLookupTable.TryGetValue(uriScheme ?? "", out result))
            {
                result = UriScheme.Undefined;
            }
            return result;
        }
    }
}