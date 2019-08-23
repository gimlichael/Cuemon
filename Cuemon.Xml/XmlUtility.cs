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