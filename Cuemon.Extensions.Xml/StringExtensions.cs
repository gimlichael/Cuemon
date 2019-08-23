using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Cuemon.ComponentModel.TypeConverters;
using Cuemon.Xml;

namespace Cuemon.Extensions.Xml
{
    /// <summary>
    /// Extension methods for the <see cref="string"/> class.
    /// </summary>
    public static class StringExtensions
    {
        /// <summary>
        /// Escapes the given XML <paramref name="value"/>.
        /// </summary>
        /// <param name="value">The XML <see cref="string"/> to escape.</param>
        /// <returns>The input <paramref name="value"/> with an escaped equivalent.</returns>
        public static string EscapeXml(this string value)
        {
            return XmlUtility.Escape(value);
        }

        /// <summary>
        /// Unescapes the given XML <paramref name="value"/>.
        /// </summary>
        /// <param name="value">The XML <see cref="string"/> to unescape.</param>
        /// <returns>The input <paramref name="value"/> with an unescaped equivalent.</returns>
        public static string UnescapeXml(this string value)
        {
            return XmlUtility.Unescape(value);
        }

        /// <summary>
        /// Sanitizes the <paramref name="value"/> for any invalid characters.
        /// </summary>
        /// <param name="value">The name of the XML element to sanitize.</param>
        /// <returns>A sanitized <see cref="string"/> of <paramref name="value"/>.</returns>
        /// <remarks>Sanitation rules are as follows:<br/>
        /// 1. Names can contain letters, numbers, and these 4 characters: _ | : | . | -<br/>
        /// 2. Names cannot start with a number or punctuation character<br/>
        /// 3. Names cannot contain spaces<br/>
        /// </remarks>
        public static string SanitizeXmlElementName(this string value)
        {
            Validator.ThrowIfNull(value, nameof(value));
            if (value.StartsWith(StringComparison.OrdinalIgnoreCase, ConvertFactory.UseConverter<TextEnumerableConverter>().ChangeType(Alphanumeric.Numbers).Concat(new[] { "." } )))
            {
                var startIndex = 0;
                var numericsAndPunctual = new List<char>(Alphanumeric.Numbers.ToCharArray().Concat(new[] { '.' }));
                foreach (var c in value)
                {
                    if (numericsAndPunctual.Contains(c))
                    {
                        startIndex++;
                        continue;
                    }
                    break;
                }
                return SanitizeXmlElementName(value.Substring(startIndex));
            }

            var validElementName = new StringBuilder();
            foreach (var c in value)
            {
                var validCharacters = new List<char>(Alphanumeric.LettersAndNumbers.ToCharArray().Concat(new[] { '_', ':', '.', '-' }));
                if (validCharacters.Contains(c)) { validElementName.Append(c); }
            }
            return validElementName.ToString();
        }

        /// <summary>
        /// Sanitizes the <paramref name="value"/> for any invalid characters.
        /// </summary>
        /// <param name="value">The content of an XML element to sanitize.</param>
        /// <param name="cdataSection">if set to <c>true</c> supplemental CDATA-section rules is applied to <paramref name="value"/>.</param>
        /// <returns>A sanitized <see cref="String"/> of <paramref name="value"/>.</returns>
        /// <remarks>Sanitation rules are as follows:<br/>
        /// 1. The <paramref name="value"/> cannot contain characters less or equal to a Unicode value of U+0019 (except U+0009, U+0010, U+0013)<br/>
        /// 2. The <paramref name="value"/> cannot contain the string "]]&lt;" if <paramref name="cdataSection"/> is <c>true</c>.<br/>
        /// </remarks>
        public static string SanitizeXmlElementText(this string value, bool cdataSection = false)
        {
            if (string.IsNullOrEmpty(value)) { return value; }
            value = StringReplacePair.RemoveAll(value, '\x0001', '\x0002', '\x0003', '\x0004', '\x0005', '\x0006', '\x0007', '\x0008', '\x0011', '\x0012', '\x0014', '\x0015', '\x0016', '\x0017', '\x0018', '\x0019');
            return cdataSection ? StringReplacePair.RemoveAll(value, "]]>") : value;
        }
    }
}