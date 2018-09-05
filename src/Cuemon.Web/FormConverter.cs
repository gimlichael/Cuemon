using System.Collections.Generic;
using System.Collections.Specialized;
using Cuemon.Collections.Specialized;

namespace Cuemon.Web
{
    /// <summary>
    /// This utility class is designed to make HTTP request form-data specific <see cref="NameValueCollection"/> related conversions easier to work with.
    /// </summary>
    public static class FormConverter
    {
        /// <summary>
        /// Converts the specified <paramref name="form"/> into its <see cref="string"/> equivalent.
        /// </summary>
        /// <param name="form">The form-data values to convert.</param>
        /// <returns>A <see cref="string"/> equivalent to the values in the <paramref name="form"/>.</returns>
        public static string FromDictionary(IDictionary<string, string[]> form)
        {
            Validator.ThrowIfNull(form, nameof(form));
            return FromNameValueCollection(form.ToNameValueCollection());
        }

        /// <summary>
        /// Converts the specified <paramref name="form"/> into its <see cref="string"/> equivalent.
        /// </summary>
        /// <param name="form">The form-data values to convert.</param>
        /// <returns>A <see cref="string"/> equivalent to the values in the <paramref name="form"/>.</returns>
        public static string FromNameValueCollection(NameValueCollection form)
        {
            Validator.ThrowIfNull(form, nameof(form));
            return Infrastructure.ParseFieldValuePairs(form, FieldValueSeparator.Semicolon, false);
        }

        /// <summary>
        /// Converts the specified <paramref name="form"/> into its <see cref="NameValueCollection"/> equivalent.
        /// </summary>
        /// <param name="form">The form-data values to convert.</param>
        /// <returns>A <see cref="NameValueCollection"/> equivalent to the values in the <paramref name="form"/>.</returns>
        public static NameValueCollection FromString(string form)
        {
            Validator.ThrowIfNull(form, nameof(form));
            return Infrastructure.ParseFieldValuePairs(form, FieldValueSeparator.Semicolon, false);
        }
    }
}