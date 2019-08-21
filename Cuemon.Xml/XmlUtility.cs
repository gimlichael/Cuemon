using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml;
using System.Xml.XPath;
using Cuemon.ComponentModel.TypeConverters;
using Cuemon.IO;
using Cuemon.Text;
using Cuemon.Xml.XPath;

namespace Cuemon.Xml
{
	/// <summary>
	/// This utility class is designed to make XML operations easier to work with.
	/// </summary>
	public static class XmlUtility
	{
		private static readonly string[][] EscapeStringPairs = new[] { new[] { "&lt;", "&gt;", "&quot;", "&apos;", "&amp;" }, new[] {"<", ">", "\"", "'", "&"} };
        private static readonly char[] InvalidXmlCharacters = new[] { '\x0001', '\x0002', '\x0003', '\x0004', '\x0005', '\x0006', '\x0007', '\x0008', '\x0011', '\x0012', '\x0014', '\x0015', '\x0016', '\x0017', '\x0018', '\x0019' };

        /// <summary>
        /// Remove the namespace declarations from the specified <paramref name="stream"/>.
        /// </summary>
        /// <param name="stream">An XML <see cref="Stream"/> to purge namespace declarations from.</param>
        /// <param name="omitXmlDeclaration">if set to <c>true</c> omit the XML declaration; otherwise <c>false</c>. The default is false.</param>
        /// <returns>A <see cref="Stream"/> object representing the specified <paramref name="stream"/> but with no namespace declarations.</returns>
        public static Stream RemoveNamespaceDeclarations(Stream stream, bool omitXmlDeclaration = false)
		{
			return RemoveNamespaceDeclarations(stream, omitXmlDeclaration, Encoding.Unicode);
		}

        /// <summary>
        /// Remove the namespace declarations from the specified <see cref="Stream"/> <paramref name="stream"/>.
        /// </summary>
        /// <param name="stream">An XML <see cref="Stream"/> to purge namespace declarations from.</param>
        /// <param name="omitXmlDeclaration">if set to <c>true</c> omit the XML declaration; otherwise <c>false</c>. The default is false.</param>
        /// <param name="encoding">The text encoding to use.</param>
        /// <returns>A <see cref="Stream"/> object representing the specified <paramref name="stream"/> but with no namespace declarations.</returns>
        public static Stream RemoveNamespaceDeclarations(Stream stream, bool omitXmlDeclaration, Encoding encoding)
		{
			Validator.ThrowIfNull(stream, nameof(stream));
            Validator.ThrowIfNull(encoding, nameof(encoding));
			var navigable = XPathNavigableConverter.FromStream(stream, true); // todo: leaveStreamOpen
			var navigator = navigable.CreateNavigator();
            return Disposable.SafeInvoke(() => new MemoryStream(), ms => 
            {
                using (var writer = XmlWriter.Create(ms, XmlWriterUtility.CreateSettings(o =>
                {
                    o.Encoding = encoding;
                    o.OmitXmlDeclaration = omitXmlDeclaration;
                })))
                {
                    WriteElements(navigator, writer);
                    writer.Flush();
                }
                ms.Position = 0;
                return ms;
            });
        }

		private static void WriteAttributes(XPathNavigator navigator, XmlWriter writer)
		{
			var attributeIterator = navigator.Select("@*");

			while (attributeIterator.MoveNext())
			{
				writer.WriteAttributeString(attributeIterator.Current.Prefix, attributeIterator.Current.LocalName, null, attributeIterator.Current.Value);
			}
		}

		private static void WriteElements(XPathNavigator navigator, XmlWriter writer)
		{
			var childrenIterator = navigator.Select("*");
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
        /// Escapes the given XML <paramref name="value"/>.
        /// </summary>
        /// <param name="value">The XML <see cref="string"/> to escape.</param>
        /// <returns>The input <paramref name="value"/> with an escaped equivalent.</returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="value"/> is null.
        /// </exception>
        public static string Escape(string value)
		{
            Validator.ThrowIfNull(value, nameof(value));
			var replacePairs = new List<StringReplacePair>();
			for (byte b = 0; b < EscapeStringPairs[0].Length; b++)
			{
				replacePairs.Add(new StringReplacePair(EscapeStringPairs[1][b], EscapeStringPairs[0][b]));
			}
			return StringReplacePair.ReplaceAll(value, replacePairs, StringComparison.Ordinal);
		}


		/// <summary>
		/// Unescapes the given XML <paramref name="value"/>.
		/// </summary>
		/// <param name="value">The XML <see cref="string"/> to unescape.</param>
		/// <returns>The input <paramref name="value"/> with an unescaped equivalent.</returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="value"/> is null.
        /// </exception>
		public static string Unescape(string value)
		{
            Validator.ThrowIfNull(value, nameof(value));
			var builder = new StringBuilder(value);
			for (byte b = 0; b < EscapeStringPairs[0].Length; b++)
			{
				builder.Replace(EscapeStringPairs[0][b], EscapeStringPairs[1][b]);
			}
			return builder.ToString();
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
        public static string SanitizeElementName(string value)
		{
			if (value == null) { throw new ArgumentNullException(nameof(value)); }
			if (StringUtility.StartsWith(value, StringComparison.OrdinalIgnoreCase, ConvertFactory.UseConverter<TextEnumerableConverter>().ChangeType(Alphanumeric.Numbers).Concat(new[] { "." } )))
			{
				var startIndex = 0;
                IList<char> numericsAndPunctual = new List<char>(Alphanumeric.Numbers.ToCharArray().Concat(new[] { '.' }));
				foreach (var c in value)
				{
					if (numericsAndPunctual.Contains(c))
					{
						startIndex++;
						continue;
					}
					break;
				}
				return SanitizeElementName(value.Substring(startIndex));
			}

			var validElementName = new StringBuilder();
			foreach (var c in value)
			{
                IList<char> validCharacters = new List<char>(Alphanumeric.LettersAndNumbers.ToCharArray().Concat(new[] { '_', ':', '.', '-' }));
				if (validCharacters.Contains(c)) { validElementName.Append(c); }
			}
			return validElementName.ToString();
		}

        /// <summary>
        /// Sanitizes the <paramref name="value"/> for any invalid characters.
        /// </summary>
        /// <param name="value">The content of an XML element to sanitize.</param>
        /// <param name="cdataSection">if set to <c>true</c> supplemental CDATA-section rules is applied to <paramref name="value"/>.</param>
        /// <returns>A sanitized <see cref="string"/> of <paramref name="value"/>.</returns>
        /// <remarks>Sanitation rules are as follows:<br/>
        /// 1. The <paramref name="value"/> cannot contain characters less or equal to a Unicode value of U+0019 (except U+0009, U+0010, U+0013)<br/>
        /// 2. The <paramref name="value"/> cannot contain the string "]]&lt;" if <paramref name="cdataSection"/> is <c>true</c>.<br/>
        /// </remarks>
        public static string SanitizeElementText(string value, bool cdataSection = false)
        {
            if (string.IsNullOrEmpty(value)) { return value; }
            value = StringUtility.RemoveAll(value, InvalidXmlCharacters);
            return cdataSection ? StringUtility.RemoveAll(value, "]]>") : value;
        }

        /// <summary>
        /// Reads the <see cref="Encoding"/> from the specified XML <see cref="Stream"/>. If an encoding cannot be resolved, UTF-8 encoding is assumed for the <see cref="Encoding"/>.
        /// </summary>
        /// <param name="value">The <see cref="Stream"/> to resolve an <see cref="Encoding"/> object from.</param>
        /// <returns>An <see cref="Encoding"/> object equivalent to the encoding used in the <paramref name="value"/>, or <see cref="Encoding.UTF8"/> if unable to resolve the encoding.</returns>
        public static Encoding ReadEncoding(Stream value)
        {
            return ReadEncoding(value, Encoding.UTF8);
        }

        /// <summary>
        /// Reads the <see cref="Encoding"/> from the specified XML <see cref="Stream"/>. If an encoding cannot be resolved, <paramref name="defaultEncoding"/> encoding is assumed for the <see cref="Encoding"/>.
        /// </summary>
        /// <param name="value">The <see cref="Stream"/> to resolve an <see cref="Encoding"/> object from.</param>
        /// <param name="defaultEncoding">The preferred default <see cref="Encoding"/> to use if an encoding cannot be resolved automatically.</param>
        /// <returns>An <see cref="Encoding"/> object equivalent to the encoding used in the <paramref name="value"/>, or <paramref name="defaultEncoding"/> if unable to resolve the encoding.</returns>
        public static Encoding ReadEncoding(Stream value, Encoding defaultEncoding)
        {
            Validator.ThrowIfNull(value, nameof(value));
            if (!ByteOrderMark.TryDetectEncoding(value, out var encoding))
            {
                long startingPosition = -1;
                if (value.CanSeek)
                {
                    startingPosition = value.Position;
                    value.Position = 0;
                }

                var document = new XmlDocument();
                document.Load(value);
                if (document.FirstChild.NodeType == XmlNodeType.XmlDeclaration)
                {
                    var declaration = (XmlDeclaration)document.FirstChild;
                    if (!string.IsNullOrEmpty(declaration.Encoding)) { encoding = Encoding.GetEncoding(declaration.Encoding); }
                }

                if (value.CanSeek) { value.Seek(startingPosition, SeekOrigin.Begin); }
            }
            return encoding ?? defaultEncoding;
        }
    }
}