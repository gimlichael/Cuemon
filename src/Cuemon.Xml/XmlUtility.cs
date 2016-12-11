using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Xml;
using System.Xml.XPath;
using Cuemon.Collections.Generic;
using Cuemon.Xml.XPath;

namespace Cuemon.Xml
{
    /// <summary>
    /// This utility class is designed to make XML operations easier to work with.
    /// </summary>
    public static class XmlUtility
    {
        private static readonly string[][] EscapeStringPairs = new[] { new[] { "&lt;", "&gt;", "&quot;", "&apos;", "&amp;" }, new[] { "<", ">", "\"", "'", "&" } };
        private static readonly char[] InvalidXmlCharacters = new[] { '\x0001', '\x0002', '\x0003', '\x0004', '\x0005', '\x0006', '\x0007', '\x0008', '\x0011', '\x0012', '\x0014', '\x0015', '\x0016', '\x0017', '\x0018', '\x0019' };

        /// <summary>
        /// Remove the namespace declarations from the specified <see cref="Stream"/> <paramref name="value"/>.
        /// </summary>
        /// <param name="value">An XML <see cref="Stream"/> to purge namespace declarations from.</param>
        /// <returns>A <see cref="Stream"/> object representing the specified <paramref name="value"/> but with no namespace declarations.</returns>
        public static Stream RemoveNamespaceDeclarations(Stream value)
        {
            return RemoveNamespaceDeclarations(value, false);
        }

        /// <summary>
        /// Remove the namespace declarations from the specified <see cref="Stream"/> <paramref name="value"/>.
        /// </summary>
        /// <param name="value">An XML <see cref="Stream"/> to purge namespace declarations from.</param>
        /// <param name="omitXmlDeclaration">if set to <c>true</c> omit the XML declaration; otherwise <c>false</c>. The default is false.</param>
        /// <returns>A <see cref="Stream"/> object representing the specified <paramref name="value"/> but with no namespace declarations.</returns>
        public static Stream RemoveNamespaceDeclarations(Stream value, bool omitXmlDeclaration)
        {
            return RemoveNamespaceDeclarations(value, omitXmlDeclaration, Encoding.Unicode);
        }

        /// <summary>
        /// Remove the namespace declarations from the specified <see cref="Stream"/> <paramref name="value"/>.
        /// </summary>
        /// <param name="value">An XML <see cref="Stream"/> to purge namespace declarations from.</param>
        /// <param name="omitXmlDeclaration">if set to <c>true</c> omit the XML declaration; otherwise <c>false</c>. The default is false.</param>
        /// <param name="encoding">The text encoding to use.</param>
        /// <returns>A <see cref="Stream"/> object representing the specified <paramref name="value"/> but with no namespace declarations.</returns>
        public static Stream RemoveNamespaceDeclarations(Stream value, bool omitXmlDeclaration, Encoding encoding)
        {
            if (value == null) { throw new ArgumentNullException(nameof(value)); }
            IXPathNavigable navigable = XPathNavigableConverter.FromStream(value, true); // todo: leaveStreamOpen
            XPathNavigator navigator = navigable.CreateNavigator();
            MemoryStream output;
            MemoryStream tempOutput = null;
            try
            {
                tempOutput = new MemoryStream();
                using (XmlWriter writer = XmlWriter.Create(tempOutput, XmlWriterUtility.CreateSettings(o =>
                {
                    o.Encoding = encoding;
                    o.OmitXmlDeclaration = omitXmlDeclaration;
                })))
                {
                    WriteElements(navigator, writer);
                    writer.Flush();
                }
                output = tempOutput;
                output.Position = 0;
                tempOutput = null;
            }
            finally
            {
                if (tempOutput != null) { tempOutput.Dispose(); }
            }
            return output;
        }

        private static void WriteAttributes(XPathNavigator navigator, XmlWriter writer)
        {
            XPathNodeIterator attributeIterator = navigator.Select("@*");

            while (attributeIterator.MoveNext())
            {
                writer.WriteAttributeString(attributeIterator.Current.Prefix, attributeIterator.Current.LocalName, null, attributeIterator.Current.Value);
            }
        }

        private static void WriteElements(XPathNavigator navigator, XmlWriter writer)
        {
            XPathNodeIterator childrenIterator = navigator.Select("*");
            while (childrenIterator.MoveNext())
            {
                writer.WriteStartElement(childrenIterator.Current.LocalName);
                WriteAttributes(childrenIterator.Current, writer);
                if (childrenIterator.Current.SelectSingleNode("text()") != null) { writer.WriteString(childrenIterator.Current.SelectSingleNode("text()").Value); }
                WriteElements(childrenIterator.Current, writer);
                writer.WriteEndElement();
            }
        }

        /// <summary>
        /// Escapes the given XML <see cref="string"/>.
        /// </summary>
        /// <param name="value">The XML <see cref="string"/> to escape.</param>
        /// <returns>The input <paramref name="value"/> with an escaped equivalent.</returns>
        public static string Escape(string value)
        {
            if (value == null) throw new ArgumentNullException(nameof(value));
            List<StringReplacePair> replacePairs = new List<StringReplacePair>();
            for (byte b = 0; b < EscapeStringPairs[0].Length; b++)
            {
                replacePairs.Add(new StringReplacePair(EscapeStringPairs[1][b], EscapeStringPairs[0][b]));
            }
            return StringUtility.Replace(value, replacePairs, StringComparison.Ordinal);
        }


        /// <summary>
        /// Unescapes the given XML <see cref="string"/>.
        /// </summary>
        /// <param name="value">The XML <see cref="string"/> to unescape.</param>
        /// <returns>The input <paramref name="value"/> with an unescaped equivalent.</returns>
        public static string Unescape(string value)
        {
            StringBuilder builder = new StringBuilder(value);
            for (byte b = 0; b < EscapeStringPairs[0].Length; b++)
            {
                builder.Replace(EscapeStringPairs[0][b], EscapeStringPairs[1][b]);
            }
            return builder.ToString();
        }

        /// <summary>
        /// Sanitizes the <paramref name="elementName"/> for any invalid characters.
        /// </summary>
        /// <param name="elementName">The name of the XML element to sanitize.</param>
        /// <returns>A sanitized <see cref="String"/> of <paramref name="elementName"/>.</returns>
        /// <remarks>Sanitation rules are as follows:<br/>
        /// 1. Names can contain letters, numbers, and these 4 characters: _ | : | . | -<br/>
        /// 2. Names cannot start with a number or punctuation character<br/>
        /// 3. Names cannot contain spaces<br/>
        /// </remarks>
        public static string SanitizeElementName(string elementName)
        {
            if (elementName == null) { throw new ArgumentNullException(nameof(elementName)); }
            if (StringUtility.StartsWith(elementName, StringComparison.OrdinalIgnoreCase, EnumerableUtility.Concat(StringConverter.FromChars(StringUtility.NumericCharacters), new[] { "." })))
            {
                int startIndex = 0;
                IList<char> numericsAndPunctual = new List<char>(EnumerableUtility.Concat(StringUtility.NumericCharacters.ToCharArray(), new[] { '.' }));
                foreach (char c in elementName)
                {
                    if (numericsAndPunctual.Contains(c))
                    {
                        startIndex++;
                        continue;
                    }
                    break;
                }
                return SanitizeElementName(elementName.Substring(startIndex));
            }

            StringBuilder validElementName = new StringBuilder();
            foreach (char c in elementName)
            {
                IList<char> validCharacters = new List<char>(EnumerableUtility.Concat(StringUtility.AlphanumericCharactersCaseSensitive.ToCharArray(), new[] { '_', ':', '.', '-' }));
                if (validCharacters.Contains(c)) { validElementName.Append(c); }
            }
            return validElementName.ToString();
        }

        /// <summary>
        /// Sanitizes the <paramref name="text"/> for any invalid characters.
        /// </summary>
        /// <param name="text">The content of an XML element to sanitize.</param>
        /// <returns>A sanitized <see cref="String"/> of <paramref name="text"/>.</returns>
        /// <remarks>The <paramref name="text"/> is sanitized for characters less or equal to a Unicode value of U+0019 (except U+0009, U+0010, U+0013).</remarks>
	    public static string SanitizeElementText(string text)
        {
            return SanitizeElementText(text, false);
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
        public static string SanitizeElementText(string text, bool cdataSection)
        {
            if (string.IsNullOrEmpty(text)) { return text; }
            text = StringUtility.Remove(text, InvalidXmlCharacters);
            return cdataSection ? StringUtility.Remove(text, "]]>") : text;
        }
    }
}