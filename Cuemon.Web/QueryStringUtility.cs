using System.Collections.Specialized;
using System.Linq;
using Cuemon.Collections.Generic;

namespace Cuemon.Web
{
    /// <summary>
    /// This utility class is designed to make HTTP request querystring operations easier to work with.
    /// </summary>
    public static class QueryStringUtility
    {
        /// <summary>
        /// Combines the specified query string <paramref name="values"/> into one <see cref="NameValueCollection"/> equivalent.
        /// </summary>
        /// <param name="values">A variable number of query string <paramref name="values"/>.</param>
        /// <returns>A <see cref="NameValueCollection"/> equivalent to the combined query string <paramref name="values"/>.</returns>
        public static NameValueCollection Combine(params string[] values)
        {
            Validator.ThrowIfNull(values, nameof(values));
            return Combine(EnumerableConverter.Parse(values, QueryStringConverter.FromString).ToArray());
        }

        /// <summary>
        /// Combines the specified query string <paramref name="values"/> into one <see cref="NameValueCollection"/> equivalent.
        /// </summary>
        /// <param name="values">A variable number of query string <paramref name="values"/>.</param>
        /// <returns>A <see cref="NameValueCollection"/> equivalent to the combined query string <paramref name="values"/>.</returns>
        public static NameValueCollection Combine(params NameValueCollection[] values)
        {
            Validator.ThrowIfNull(values, nameof(values));
            return Infrastructure.CombineFieldValuePairs(FieldValueSeparator.Ampersand, values);
        }

        /// <summary>
        /// Sanitizes the <paramref name="query"/> so that all keys (with matching values) is removed.
        /// </summary>
        /// <param name="query">The query string values to sanitize.</param>
        /// <param name="keys">The keys to use in the sanitation process.</param>
        /// <returns>A sanitized <see cref="NameValueCollection"/> query equivalent of <paramref name="query"/>.</returns>
        public static NameValueCollection Remove(NameValueCollection query, params string[] keys)
        {
            return FilterCore(FieldValueFilter.Remove, query, keys);
        }

        /// <summary>
        /// Sanitizes the <paramref name="query"/> so that all keys is assured only the latest value applied.
        /// </summary>
        /// <param name="query">The query string values to sanitize.</param>
        /// <param name="keys">The keys to use in the sanitation process.</param>
        /// <returns>A sanitized <see cref="NameValueCollection"/> query equivalent of <paramref name="query"/>.</returns>
        public static NameValueCollection RemoveDublets(NameValueCollection query, params string[] keys)
        {
            return FilterCore(FieldValueFilter.RemoveDublets, query, keys);
        }

        /// <summary>
        /// Sanitizes the <paramref name="query"/> so that all keys (with matching values) is removed.
        /// </summary>
        /// <param name="query">The query string values to sanitize.</param>
        /// <param name="keys">The keys to use in the sanitation process.</param>
        /// <returns>A sanitized <see cref="NameValueCollection"/> query equivalent of <paramref name="query"/>.</returns>
        public static NameValueCollection Remove(string query, params string[] keys)
        {
            return FilterCore(FieldValueFilter.Remove, query, keys);
        }

        /// <summary>
        /// Sanitizes the <paramref name="query"/> so that all keys is assured only the latest value applied.
        /// </summary>
        /// <param name="query">The query string values to sanitize.</param>
        /// <param name="keys">The keys to use in the sanitation process.</param>
        /// <returns>A sanitized <see cref="NameValueCollection"/> query equivalent of <paramref name="query"/>.</returns>
        public static NameValueCollection RemoveDublets(string query, params string[] keys)
        {
            return FilterCore(FieldValueFilter.RemoveDublets, query, keys);
        }

        private static NameValueCollection FilterCore(FieldValueFilter filter, string query, params string[] keys)
        {
            Validator.ThrowIfNull(query, nameof(query));
            Validator.ThrowIfNull(keys, nameof(keys));
            return FilterCore(filter, QueryStringConverter.FromString(query), keys);
        }

        private static NameValueCollection FilterCore(FieldValueFilter filter, NameValueCollection query, params string[] keys)
        {
            Validator.ThrowIfNull(query, nameof(query));
            Validator.ThrowIfNull(keys, nameof(keys));
            return Infrastructure.SanitizeFieldValuePairs(query, filter, keys);
        }
    }
}