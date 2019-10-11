using System.Linq;
using System.Net.Http.Headers;
using Cuemon.Extensions;
using Cuemon.Extensions.Collections.Generic;
using Cuemon.Text;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Primitives;

namespace Cuemon.AspNetCore.Http
{
    /// <summary>
    /// Extension methods for the <see cref="IHeaderDictionary"/> interface.
    /// </summary>
    public static class HeaderDictionaryExtensions
    {
        /// <summary>
        /// Adds or updates an element with the provided key and value to the <see cref="IHeaderDictionary"/>.
        /// </summary>
        /// <param name="dic">The <see cref="IHeaderDictionary"/> to extend.</param>
        /// <param name="key">The string to use as the key of the element to add.</param>
        /// <param name="value">The string to use as the value of the element to add.</param>
        /// <param name="useAsciiEncodingConversion">if set to <c>true</c> an ASCII encoding conversion is applied to the <paramref name="value"/>.</param>
        public static void AddOrUpdateHeader(this IHeaderDictionary dic, string key, StringValues value, bool useAsciiEncodingConversion = true)
        {
            var headerValue = useAsciiEncodingConversion ? new StringValues(ConvertFactory.UseEncoder<AsciiStringEncoder>().Encode(value)) : value;
            if (headerValue != StringValues.Empty)
            {
                dic.AddOrUpdate(key, headerValue.ToString().Where(c => !char.IsControl(c)).FromChars());
            }
        }

        /// <summary>
        /// Adds or updates one or more elements from the provided collection of <paramref name="responseHeaders"/> to the <see cref="IHeaderDictionary"/>.
        /// </summary>
        /// <param name="dic">The <see cref="IHeaderDictionary"/> to extend.</param>
        /// <param name="responseHeaders">The <see cref="HttpResponseHeaders"/> to copy.</param>
        public static void AddOrUpdateHeaders(this IHeaderDictionary dic, HttpResponseHeaders responseHeaders)
        {
            if (dic == null || responseHeaders == null) { return; }
            foreach (var header in responseHeaders)
            {
                dic.AddOrUpdate(header.Key, header.Value?.ToDelimitedString());
            }
        }
    }
}