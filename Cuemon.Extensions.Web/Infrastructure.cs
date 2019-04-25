using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Text;

namespace Cuemon.Web
{
    internal static class Infrastructure
    {
        /// <summary>
        /// Combines the specified query string, header or a form-data <paramref name="sources"/> into one <see cref="NameValueCollection"/> equivalent field-value pairs.
        /// </summary>
        /// <param name="sources">A variable number of query string, header or a form-data <paramref name="sources"/> to combine into one <see cref="NameValueCollection"/>.</param>
        /// <param name="separator">The <see cref="FieldValueSeparator"/> to use in the combination.</param>
        /// <returns>A variable number of query string, header or a form-data <paramref name="sources"/> combined into one <see cref="NameValueCollection"/> equivalent field-value pairs.</returns>
        internal static NameValueCollection CombineFieldValuePairs(FieldValueSeparator separator, params NameValueCollection[] sources)
        {
            if (sources == null) { throw new ArgumentNullException(nameof(sources)); }
            var mergedFieldValuePairs = new StringBuilder(separator == FieldValueSeparator.Ampersand ? "?" : "");
            foreach (var fieldValuePairs in sources)
            {
                if (fieldValuePairs.Count == 0) { continue; }
                mergedFieldValuePairs.Append(separator == FieldValueSeparator.Ampersand ? ParseFieldValuePairs(fieldValuePairs, separator, true).Remove(0, 1) : ParseFieldValuePairs(fieldValuePairs, separator, true));
                mergedFieldValuePairs.Append(GetSeparator(separator));
            }
            if (mergedFieldValuePairs.Length > 0) { mergedFieldValuePairs.Remove(mergedFieldValuePairs.Length - 1, 1); }
            return ParseFieldValuePairs(mergedFieldValuePairs.ToString(), separator, false);
        }

        /// <summary>
        /// Sanitizes the query string, header or form-data from the specified arguments.
        /// </summary>
        /// <param name="fieldValuePairs">The query string, header or form-data to sanitize.</param>
        /// <param name="sanitizing">The sanitizing action to perform on the <paramref name="fieldValuePairs"/>.</param>
        /// <param name="keys">The keys to use in the sanitizing process.</param>
        /// <returns>A sanitized <see cref="NameValueCollection"/> equivalent of <paramref name="fieldValuePairs"/>.</returns>
        internal static NameValueCollection SanitizeFieldValuePairs(NameValueCollection fieldValuePairs, FieldValueFilter sanitizing, IEnumerable<string> keys)
        {
            if (fieldValuePairs == null) throw new ArgumentNullException(nameof(fieldValuePairs));
            if (keys == null) throw new ArgumentNullException(nameof(keys));
            var modifiedFieldValuePairs = new NameValueCollection(fieldValuePairs);
            switch (sanitizing)
            {
                case FieldValueFilter.RemoveDublets:
                    foreach (var key in keys)
                    {
                        var values = modifiedFieldValuePairs[key].Split(',');
                        var zeroBasedIndex = values.Length - 1;
                        if (zeroBasedIndex >= 0) { modifiedFieldValuePairs[key] = values[zeroBasedIndex]; }
                    }
                    break;
                case FieldValueFilter.Remove:
                    foreach (var key in keys)
                    {
                        modifiedFieldValuePairs.Remove(key);
                    }
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(sanitizing));
            }
            return modifiedFieldValuePairs;
        }

        /// <summary>
        /// Parses a query string, header or a form-data into a <see cref="string"/> equivalent field-value pairs as specified by the W3C.
        /// </summary>
        /// <param name="fieldValuePairs">The query string, header or form-data values to parse.</param>
        /// <param name="separator">The <see cref="FieldValueSeparator"/> to use in the conversion.</param>
        /// <param name="urlEncode">Encodes the output URL string. Default is true.</param>
        /// <returns>A <see cref="string"/> value equivalent to the values in the <paramref name="fieldValuePairs"/>.</returns>
        internal static string ParseFieldValuePairs(NameValueCollection fieldValuePairs, FieldValueSeparator separator, bool urlEncode)
        {
            if (fieldValuePairs == null) throw new ArgumentNullException(nameof(fieldValuePairs));
            var characterSeperator = GetSeparator(separator);
            var builder = new StringBuilder(separator == FieldValueSeparator.Ampersand ? "?" : "");
            foreach (string item in fieldValuePairs)
            {
                builder.AppendFormat("{0}={1}", item, urlEncode ? HttpUtility.UrlEncode(HttpUtility.UrlDecode(fieldValuePairs[item])) : fieldValuePairs[item]); // the HttpUtility.UrlDecode is called when used outside of IIS .. IIS auto UrlEncode .. why we have to assure, that we don't double UrlEncode ..
                builder.Append(characterSeperator);
            }
            if (builder.Length > 0 && separator == FieldValueSeparator.Ampersand) { builder.Remove(builder.Length - 1, 1); }
            return builder.ToString();
        }

        /// <summary>
        /// Parses a query string, header or a form-data into a <see cref="NameValueCollection"/> equivalent field-value pairs.
        /// </summary>
        /// <param name="fieldValuePairs">The query string, header or form-data values to parse.</param>
        /// <param name="urlDecode">Converts <paramref name="fieldValuePairs"/> that has been encoded for transmission in a URL into a decoded string. Default is false.</param>
        /// <param name="separator">The <see cref="FieldValueSeparator"/> to use in the conversion.</param>
        /// <returns>A <see cref="NameValueCollection"/> value equivalent to the values in the <paramref name="fieldValuePairs"/>.</returns>
        internal static NameValueCollection ParseFieldValuePairs(string fieldValuePairs, FieldValueSeparator separator, bool urlDecode)
        {
            if (fieldValuePairs == null) { throw new ArgumentNullException(nameof(fieldValuePairs)); }
            var modifiedFieldValuePairs = new NameValueCollection();
            if (fieldValuePairs.Length == 0) { return modifiedFieldValuePairs; }
            var characterSeperator = GetSeparator(separator);
            if (separator == FieldValueSeparator.Ampersand && fieldValuePairs.StartsWith("?", StringComparison.OrdinalIgnoreCase)) { fieldValuePairs = fieldValuePairs.Remove(0, 1); }
            var namesAndValues = fieldValuePairs.Split(characterSeperator);
            foreach (var nameAndValue in namesAndValues)
            {
                var equalLocation = nameAndValue.IndexOf("=", StringComparison.OrdinalIgnoreCase);
                if (equalLocation < 0) { continue; } // we have no parameter values, just a value pair like lcid=1030& or lcid=1030&test
                var value = equalLocation == nameAndValue.Length ? null : urlDecode ? HttpUtility.UrlDecode(nameAndValue.Substring(equalLocation + 1)) : nameAndValue.Substring(equalLocation + 1);
                modifiedFieldValuePairs.Add(nameAndValue.Substring(0, nameAndValue.IndexOf("=", StringComparison.OrdinalIgnoreCase)), value);
            }
            return modifiedFieldValuePairs;
        }

        private static char GetSeparator(FieldValueSeparator separator)
        {
            switch (separator)
            {
                case FieldValueSeparator.Ampersand:
                    return '&';
                case FieldValueSeparator.Semicolon:
                    return ';';
            }
            throw new ArgumentOutOfRangeException(nameof(separator));
        }
    }
}