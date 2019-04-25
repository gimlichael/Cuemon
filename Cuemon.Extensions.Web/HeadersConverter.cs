using System.Collections.Generic;
using System.Collections.Specialized;
using Cuemon.Extensions.Collections.Specialized;
using Cuemon.Web;

namespace Cuemon.Extensions.Web
{
    /// <summary>
    /// This utility class is designed to make HTTP request headers specific <see cref="NameValueCollection"/> related conversions easier to work with.
    /// </summary>
    public static class HeadersConverter
    {
        /// <summary>
        /// Converts the specified <paramref name="headers"/> into its <see cref="string"/> equivalent.
        /// </summary>
        /// <param name="headers">The request-header values to convert.</param>
        /// <returns>A <see cref="string"/> equivalent to the values in the <paramref name="headers"/>.</returns>
        public static string FromDictionary(IDictionary<string, string[]> headers)
        {
            Validator.ThrowIfNull(headers, nameof(headers));
            return FromNameValueCollection(headers.ToNameValueCollection());
        }

        /// <summary>
        /// Converts the specified <paramref name="headers"/> into its <see cref="string"/> equivalent.
        /// </summary>
        /// <param name="headers">The request-header values to convert.</param>
        /// <returns>A <see cref="string"/> equivalent to the values in the <paramref name="headers"/>.</returns>
        public static string FromNameValueCollection(NameValueCollection headers)
        {
            Validator.ThrowIfNull(headers, nameof(headers));
            return Infrastructure.ParseFieldValuePairs(headers, FieldValueSeparator.Semicolon, false);
        }

        /// <summary>
        /// Converts the specified <paramref name="headers"/> into its <see cref="NameValueCollection"/> equivalent.
        /// </summary>
        /// <param name="headers">The request-header values to convert.</param>
        /// <returns>A <see cref="NameValueCollection"/> equivalent to the values in the <paramref name="headers"/>.</returns>
        public static NameValueCollection FromString(string headers)
        {
            Validator.ThrowIfNull(headers, nameof(headers));
            return Infrastructure.ParseFieldValuePairs(headers, FieldValueSeparator.Semicolon, false);
        }
    }
}