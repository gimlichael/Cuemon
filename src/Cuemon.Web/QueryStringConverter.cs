using System.Collections.Generic;
using System.Collections.Specialized;
using Cuemon.Collections.Specialized.Extensions;

namespace Cuemon.Web
{
    /// <summary>
    /// This utility class is designed to make HTTP request querystring specific <see cref="NameValueCollection"/> related conversions easier to work with.
    /// </summary>
    public static class QueryStringConverter
    {
        /// <summary>
        /// Converts the specified <paramref name="query"/> into its <see cref="string"/> equivalent.
        /// </summary>
        /// <param name="query">The query string values to convert.</param>
        /// <returns>A <see cref="string"/> equivalent to the values in the <paramref name="query"/>.</returns>
        public static string FromDictionary(IDictionary<string, string[]> query)
        {
            Validator.ThrowIfNull(query, nameof(query));
            return FromNameValueCollection(query.ToNameValueCollection());
        }

        /// <summary>
        /// Converts the specified <paramref name="query"/> into its <see cref="string"/> equivalent.
        /// </summary>
        /// <param name="query">The query string values to convert.</param>
        /// <param name="urlEncode">Specify <c>true</c> to encode the <paramref name="query"/> into a URL-encoded string; otherwise, <c>false</c>.</param>
        /// <returns>A <see cref="string"/> equivalent to the values in the <paramref name="query"/>.</returns>
        public static string FromDictionary(IDictionary<string, string[]> query, bool urlEncode)
        {
            Validator.ThrowIfNull(query, nameof(query));
            return FromNameValueCollection(query.ToNameValueCollection(), urlEncode);
        }

        /// <summary>
        /// Converts the specified <paramref name="query"/> into its <see cref="string"/> equivalent.
        /// </summary>
        /// <param name="query">The query string values to convert.</param>
        /// <returns>A <see cref="string"/> equivalent to the values in the <paramref name="query"/>.</returns>
        public static string FromNameValueCollection(NameValueCollection query)
        {
            return FromNameValueCollection(query, true);
        }

        /// <summary>
        /// Converts the specified <paramref name="query"/> into its <see cref="string"/> equivalent.
        /// </summary>
        /// <param name="query">The query string values to convert.</param>
        /// <param name="urlEncode">Specify <c>true</c> to encode the <paramref name="query"/> into a URL-encoded string; otherwise, <c>false</c>.</param>
        /// <returns>A <see cref="string"/> equivalent to the values in the <paramref name="query"/>.</returns>
        public static string FromNameValueCollection(NameValueCollection query, bool urlEncode)
        {
            Validator.ThrowIfNull(query, nameof(query));
            return Infrastructure.ParseFieldValuePairs(query, FieldValueSeparator.Ampersand, urlEncode);
        }

        /// <summary>
        /// Converts the specified <paramref name="query"/> into its <see cref="NameValueCollection"/> equivalent.
        /// </summary>
        /// <param name="query">The query string values to convert.</param>
        /// <returns>A <see cref="NameValueCollection"/> equivalent to the values in the <paramref name="query"/>.</returns>
        public static NameValueCollection FromString(string query)
        {
            return FromString(query, false);
        }

        /// <summary>
        /// Converts the specified <paramref name="query"/> into its <see cref="NameValueCollection"/> equivalent.
        /// </summary>
        /// <param name="query">The query string values to convert.</param>
        /// <param name="urlDecode">Specify <c>true</c> to decode the <paramref name="query"/> that has been encoded for transmission in a URL; otherwise, <c>false</c>.</param>
        /// <returns>A <see cref="NameValueCollection"/> equivalent to the values in the <paramref name="query"/>.</returns>
        public static NameValueCollection FromString(string query, bool urlDecode)
        {
            Validator.ThrowIfNull(query, nameof(query));
            return Infrastructure.ParseFieldValuePairs(query, FieldValueSeparator.Ampersand, urlDecode);
        }
    }
}