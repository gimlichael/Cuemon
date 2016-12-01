using System;
using System.IO;
using System.Text;

namespace Cuemon.Xml
{
    /// <summary>
    /// Extension methods for the <see cref="XmlUtility"/> class.
    /// </summary>
    public static class XmlUtilityExtensions
    {
        /// <summary>
        /// Remove the XML namespace declarations from the specified <see cref="Stream"/> <paramref name="value"/>.
        /// </summary>
        /// <param name="value">An XML <see cref="Stream"/> to purge namespace declarations from.</param>
        /// <returns>A <see cref="Stream"/> object representing the specified <paramref name="value"/> but with no namespace declarations.</returns>
        public static Stream RemoveXmlNamespaceDeclarations(this Stream value)
        {
            return XmlUtility.RemoveNamespaceDeclarations(value);
        }

        /// <summary>
        /// Remove the XML namespace declarations from the specified <see cref="Stream"/> <paramref name="value"/>.
        /// </summary>
        /// <param name="value">An XML <see cref="Stream"/> to purge namespace declarations from.</param>
        /// <param name="omitXmlDeclaration">if set to <c>true</c> omit the XML declaration; otherwise <c>false</c>. The default is false.</param>
        /// <returns>A <see cref="Stream"/> object representing the specified <paramref name="value"/> but with no namespace declarations.</returns>
        public static Stream RemoveXmlNamespaceDeclarations(this Stream value, bool omitXmlDeclaration)
        {
            return XmlUtility.RemoveNamespaceDeclarations(value, omitXmlDeclaration);
        }

        /// <summary>
        /// Remove the XML namespace declarations from the specified <see cref="Stream"/> <paramref name="value"/>.
        /// </summary>
        /// <param name="value">An XML <see cref="Stream"/> to purge namespace declarations from.</param>
        /// <param name="omitXmlDeclaration">if set to <c>true</c> omit the XML declaration; otherwise <c>false</c>. The default is false.</param>
        /// <param name="encoding">The text encoding to use.</param>
        /// <returns>A <see cref="Stream"/> object representing the specified <paramref name="value"/> but with no namespace declarations.</returns>
        public static Stream RemoveXmlNamespaceDeclarations(this Stream value, bool omitXmlDeclaration, Encoding encoding)
        {
            return XmlUtility.RemoveNamespaceDeclarations(value, omitXmlDeclaration, encoding);
        }

        /// <summary>
        /// Escapes the given XML <see cref="string"/>.
        /// </summary>
        /// <param name="value">The XML <see cref="string"/> to escape.</param>
        /// <returns>The input <paramref name="value"/> with an escaped equivalent.</returns>
        public static string EscapeXml(this string value)
        {
            return XmlUtility.Escape(value);
        }

        /// <summary>
        /// Unescapes the given XML <see cref="string"/>.
        /// </summary>
        /// <param name="value">The XML <see cref="string"/> to unescape.</param>
        /// <returns>The input <paramref name="value"/> with an unescaped equivalent.</returns>
        public static string UnescapeXml(this string value)
        {
            return XmlUtility.Unescape(value);
        }

        /// <summary>
        /// Sanitizes the <paramref name="elementName"/> for any invalid characters.
        /// </summary>
        /// <param name="elementName">The name of the XML element to sanitize.</param>
        /// <returns>A sanitized <see cref="string"/> of <paramref name="elementName"/>.</returns>
        /// <remarks>Sanitation rules are as follows:<br/>
        /// 1. Names can contain letters, numbers, and these 4 characters: _ | : | . | -<br/>
        /// 2. Names cannot start with a number or punctuation character<br/>
        /// 3. Names cannot contain spaces<br/>
        /// </remarks>
        public static string SanitizeElementName(this string elementName)
        {
            return XmlUtility.SanitizeElementName(elementName);
        }

        /// <summary>
        /// Sanitizes the <paramref name="text"/> for any invalid characters.
        /// </summary>
        /// <param name="text">The content of an XML element to sanitize.</param>
        /// <returns>A sanitized <see cref="String"/> of <paramref name="text"/>.</returns>
        /// <remarks>The <paramref name="text"/> is sanitized for characters less or equal to a Unicode value of U+0019 (except U+0009, U+0010, U+0013).</remarks>
        public static string SanitizeElementText(this string text)
        {
            return XmlUtility.SanitizeElementText(text);
        }

        /// <summary>
        /// Sanitizes the <paramref name="text"/> for any invalid characters.
        /// </summary>
        /// <param name="text">The content of an XML element to sanitize.</param>
        /// <param name="cdataSection">if set to <c>true</c> supplemental CDATA-section rules is applied to <paramref name="text"/>.</param>
        /// <returns>A sanitized <see cref="String"/> of <paramref name="text"/>.</returns>
        /// <remarks>Sanitation rules are as follows:<br/>
        /// 1. The <paramref name="text"/> cannot contain characters less or equal to a Unicode value of U+0019 (except U+0009, U+0010, U+0013)<br/>
        /// 2. The <paramref name="text"/> cannot contain the string "]]&lt;" if <paramref name="cdataSection"/> is <c>true</c>.<br/>
        /// </remarks>
        public static string SanitizeElementText(this string text, bool cdataSection)
        {
            return XmlUtility.SanitizeElementText(text, cdataSection);
        }
    }
}