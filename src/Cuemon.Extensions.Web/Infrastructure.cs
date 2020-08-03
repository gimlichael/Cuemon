using System;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Text;

namespace Cuemon.Extensions.Web
{
    internal static class Infrastructure
    {
        internal static bool NotEncoded(char c)
        {
            return (c == '!' || c == '(' || c == ')' || c == '*' || c == '-' || c == '.' || c == '_');
        }

        internal static string ParseFieldValuePairs(NameValueCollection fieldValuePairs, FieldValueSeparator separator, bool urlEncode)
        {
            if (fieldValuePairs == null) { throw new ArgumentNullException(nameof(fieldValuePairs)); }
            var characterSeperator = GetSeparator(separator);
            var builder = new StringBuilder(separator == FieldValueSeparator.Ampersand ? "?" : "");
            foreach (string item in fieldValuePairs)
            {
                var values = fieldValuePairs[item].Split(',');
                foreach (var value in values)
                {
                    builder.AppendFormat("{0}={1}", item, urlEncode ? value.UrlDecode().UrlEncode() : value);
                    builder.Append(characterSeperator);
                }
            }
            if (builder.Length > 0 && separator == FieldValueSeparator.Ampersand) { builder.Remove(builder.Length - 1, 1); }
            return builder.ToString();
        }

        internal static QueryStringCollection ParseFieldValuePairs(string fieldValuePairs, FieldValueSeparator separator, bool urlDecode)
        {
            if (fieldValuePairs == null) { throw new ArgumentNullException(nameof(fieldValuePairs)); }
            var modifiedFieldValuePairs = new QueryStringCollection();
            if (fieldValuePairs.Length == 0) { return modifiedFieldValuePairs; }
            var characterSeperator = GetSeparator(separator);
            if (separator == FieldValueSeparator.Ampersand && fieldValuePairs.StartsWith("?", StringComparison.OrdinalIgnoreCase)) { fieldValuePairs = fieldValuePairs.Remove(0, 1); }
            var namesAndValues = fieldValuePairs.Split(characterSeperator);
            foreach (var nameAndValue in namesAndValues)
            {
                var equalLocation = nameAndValue.IndexOf("=", StringComparison.OrdinalIgnoreCase);
                if (equalLocation < 0) { continue; } // we have no parameter values, just a value pair like lcid=1030& or lcid=1030&test
                var value = equalLocation == nameAndValue.Length ? null : urlDecode ? nameAndValue.Substring(equalLocation + 1).UrlDecode() : nameAndValue.Substring(equalLocation + 1);
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
            throw new InvalidEnumArgumentException(nameof(separator), (int)separator, typeof(FieldValueSeparator));
        }
    }
}