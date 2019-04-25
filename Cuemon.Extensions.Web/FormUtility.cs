using System.Collections.Specialized;
using System.Linq;
using Cuemon.Web;

namespace Cuemon.Extensions.Web
{
    /// <summary>
    /// This utility class is designed to make HTTP request form-data operations easier to work with.
    /// </summary>
    public static class FormUtility
    {
        /// <summary>
        /// Combines the specified form-data <paramref name="values"/> into one <see cref="NameValueCollection"/> equivalent.
        /// </summary>
        /// <param name="values">A variable number of form-data <paramref name="values"/>.</param>
        /// <returns>A <see cref="NameValueCollection"/> equivalent to the combined form-data <paramref name="values"/>.</returns>
        public static NameValueCollection Combine(params string[] values)
        {
            Validator.ThrowIfNull(values, nameof(values));
            return Combine(values.Select(FormConverter.FromString).ToArray());
        }

        /// <summary>
        /// Combines the specified form-data <paramref name="values"/> into one <see cref="NameValueCollection"/> equivalent.
        /// </summary>
        /// <param name="values">A variable number of form-data <paramref name="values"/>.</param>
        /// <returns>A <see cref="NameValueCollection"/> equivalent to the combined form-data <paramref name="values"/>.</returns>
        public static NameValueCollection Combine(params NameValueCollection[] values)
        {
            Validator.ThrowIfNull(values, nameof(values));
            return Infrastructure.CombineFieldValuePairs(FieldValueSeparator.Semicolon, values);
        }

        /// <summary>
        /// Sanitizes the <paramref name="form"/> so that all keys (with matching values) is removed.
        /// </summary>
        /// <param name="form">The form-data values to sanitize.</param>
        /// <param name="keys">The keys to use in the sanitation process.</param>
        /// <returns>A sanitized <see cref="NameValueCollection"/> form-data equivalent of <paramref name="form"/>.</returns>
        public static NameValueCollection Remove(NameValueCollection form, params string[] keys)
        {
            return FilterCore(FieldValueFilter.Remove, form, keys);
        }

        /// <summary>
        /// Sanitizes the <paramref name="form"/> so that all keys is assured only the latest value applied.
        /// </summary>
        /// <param name="form">The form-data values to sanitize.</param>
        /// <param name="keys">The keys to use in the sanitation process.</param>
        /// <returns>A sanitized <see cref="NameValueCollection"/> form-data equivalent of <paramref name="form"/>.</returns>
        public static NameValueCollection RemoveDublets(NameValueCollection form, params string[] keys)
        {
            return FilterCore(FieldValueFilter.RemoveDublets, form, keys);
        }

        /// <summary>
        /// Sanitizes the <paramref name="form"/> so that all keys (with matching values) is removed.
        /// </summary>
        /// <param name="form">The form-data values to sanitize.</param>
        /// <param name="keys">The keys to use in the sanitation process.</param>
        /// <returns>A sanitized <see cref="NameValueCollection"/> form-data equivalent of <paramref name="form"/>.</returns>
        public static NameValueCollection Remove(string form, params string[] keys)
        {
            return FilterCore(FieldValueFilter.Remove, form, keys);
        }

        /// <summary>
        /// Sanitizes the <paramref name="form"/> so that all keys is assured only the latest value applied.
        /// </summary>
        /// <param name="form">The form-data values to sanitize.</param>
        /// <param name="keys">The keys to use in the sanitation process.</param>
        /// <returns>A sanitized <see cref="NameValueCollection"/> form-data equivalent of <paramref name="form"/>.</returns>
        public static NameValueCollection RemoveDublets(string form, params string[] keys)
        {
            return FilterCore(FieldValueFilter.RemoveDublets, form, keys);
        }

        private static NameValueCollection FilterCore(FieldValueFilter filter, string form, params string[] keys)
        {
            Validator.ThrowIfNull(form, nameof(form));
            Validator.ThrowIfNull(keys, nameof(keys));
            return FilterCore(filter, FormConverter.FromString(form), keys);
        }

        private static NameValueCollection FilterCore(FieldValueFilter filter, NameValueCollection form, params string[] keys)
        {
            Validator.ThrowIfNull(form, nameof(form));
            Validator.ThrowIfNull(keys, nameof(keys));
            return Infrastructure.SanitizeFieldValuePairs(form, filter, keys);
        }
    }
}