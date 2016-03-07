using System.Collections.Generic;
using System.Net.Http;

namespace Cuemon.Net.Http
{
    /// <summary>
    /// This utility class is designed to make <see cref="HttpMethod"/> related conversions easier to work with.
    /// </summary>
    public static class HttpMethodConverter
    {
        private static readonly IDictionary<string, HttpMethods> StringToHttpMethodLookupTable = InitStringToHttpMethodLookupTable();

        private static IDictionary<string, HttpMethods> InitStringToHttpMethodLookupTable()
        {
            Dictionary<string, HttpMethods> result = new Dictionary<string, HttpMethods>();
            foreach (var pair in EnumUtility.ToEnumerable<HttpMethods>())
            {
                result.Add(pair.Value, EnumUtility.Parse<HttpMethods>(pair.Value));
            }
            return result;
        }

        /// <summary>
        /// Converts the specified <paramref name="source"/> to its equivalent <see cref="HttpMethods"/> representation.
        /// </summary>
        /// <param name="source">The <see cref="HttpMethod"/> to be converted.</param>
        /// <returns>A <see cref="HttpMethods"/> representation of the specified <paramref name="source"/>.</returns>
        public static HttpMethods ToHttpMethod(HttpMethod source)
        {
            HttpMethods result;
            if (!StringToHttpMethodLookupTable.TryGetValue(source.Method, out result))
            {
                result = HttpMethods.Get;
            }
            return result;
        }
    }
}