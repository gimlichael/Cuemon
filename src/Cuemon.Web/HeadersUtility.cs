using System.Collections.Specialized;
using System.Linq;
using Cuemon.Collections.Generic;

namespace Cuemon.Web
{

    /// <summary>
    /// This utility class is designed to make HTTP request headers operations easier to work with.
    /// </summary>
    public static class HeadersUtility
    {
        /// <summary>
        /// Combines the specified request-header <paramref name="values"/> into one <see cref="NameValueCollection"/> equivalent.
        /// </summary>
        /// <param name="values">A variable number of request-header <paramref name="values"/>.</param>
        /// <returns>A <see cref="NameValueCollection"/> equivalent to the combined request-header <paramref name="values"/>.</returns>
        public static NameValueCollection Combine(params string[] values)
        {
            Validator.ThrowIfNull(values, nameof(values));
            return Combine(EnumerableConverter.Parse(values, HeadersConverter.FromString).ToArray());
        }

        /// <summary>
        /// Combines the specified request-header <paramref name="values"/> into one <see cref="NameValueCollection"/> equivalent.
        /// </summary>
        /// <param name="values">A variable number of request-header <paramref name="values"/>.</param>
        /// <returns>A <see cref="NameValueCollection"/> equivalent to the combined request-header <paramref name="values"/>.</returns>
        public static NameValueCollection Combine(params NameValueCollection[] values)
        {
            Validator.ThrowIfNull(values, nameof(values));
            return Infrastructure.CombineFieldValuePairs(FieldValueSeparator.Semicolon, values);
        }

        /// <summary>
        /// Sanitizes the <paramref name="headers"/> so that all keys (with matching values) is removed.
        /// </summary>
        /// <param name="headers">The request-header values to sanitize.</param>
        /// <param name="keys">The keys to use in the sanitation process.</param>
        /// <returns>A sanitized <see cref="NameValueCollection"/> request-header equivalent of <paramref name="headers"/>.</returns>
        public static NameValueCollection Remove(NameValueCollection headers, params string[] keys)
        {
            return FilterCore(FieldValueFilter.Remove, headers, keys);
        }

        /// <summary>
        /// Sanitizes the <paramref name="headers"/> so that all keys is assured only the latest value applied.
        /// </summary>
        /// <param name="headers">The request-header values to sanitize.</param>
        /// <param name="keys">The keys to use in the sanitation process.</param>
        /// <returns>A sanitized <see cref="NameValueCollection"/> request-header equivalent of <paramref name="headers"/>.</returns>
        public static NameValueCollection RemoveDublets(NameValueCollection headers, params string[] keys)
        {
            return FilterCore(FieldValueFilter.RemoveDublets, headers, keys);
        }

        /// <summary>
        /// Sanitizes the <paramref name="headers"/> so that all keys (with matching values) is removed.
        /// </summary>
        /// <param name="headers">The request-header values to sanitize.</param>
        /// <param name="keys">The keys to use in the sanitation process.</param>
        /// <returns>A sanitized <see cref="NameValueCollection"/> request-header equivalent of <paramref name="headers"/>.</returns>
        public static NameValueCollection Remove(string headers, params string[] keys)
        {
            return FilterCore(FieldValueFilter.Remove, headers, keys);
        }

        /// <summary>
        /// Sanitizes the <paramref name="headers"/> so that all keys is assured only the latest value applied.
        /// </summary>
        /// <param name="headers">The request-header values to sanitize.</param>
        /// <param name="keys">The keys to use in the sanitation process.</param>
        /// <returns>A sanitized <see cref="NameValueCollection"/> request-header equivalent of <paramref name="headers"/>.</returns>
        public static NameValueCollection RemoveDublets(string headers, params string[] keys)
        {
            return FilterCore(FieldValueFilter.RemoveDublets, headers, keys);
        }

        private static NameValueCollection FilterCore(FieldValueFilter filter, string headers, params string[] keys)
        {
            Validator.ThrowIfNull(headers, nameof(headers));
            Validator.ThrowIfNull(keys, nameof(keys));
            return FilterCore(filter, HeadersConverter.FromString(headers), keys);
        }

        private static NameValueCollection FilterCore(FieldValueFilter filter, NameValueCollection headers, params string[] keys)
        {
            Validator.ThrowIfNull(headers, nameof(headers));
            Validator.ThrowIfNull(keys, nameof(keys));
            return Infrastructure.SanitizeFieldValuePairs(headers, filter, keys);
        }
    }
}