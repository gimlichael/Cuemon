using System;
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
            return XmlUtility.SanitizeElementName(value);
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
            return XmlUtility.SanitizeElementText(value, cdataSection);
        }
    }
}