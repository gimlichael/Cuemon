using System;
using System.IO;
using System.Text;
using System.Xml;
using System.Xml.XPath;
using Cuemon.Text;

namespace Cuemon.Extensions.Xml.XPath
{
    /// <summary>
    /// This utility class is designed to make <see cref="IXPathNavigable"/> related conversions easier to work with.
    /// </summary>
    public static class XPathNavigableConverter
    {
        /// <summary>
		/// Converts the given XML string to an IXPathNavigable object using UTF-8 for the encoding.
		/// </summary>
		/// <param name="value">The XML string to be converted.</param>
		/// <returns>An <see cref="IXPathNavigable"/> object.</returns>
		public static IXPathNavigable FromString(string value)
        {
            return FromString(value, Encoding.UTF8);
        }

        /// <summary>
        /// Converts the given XML string to an IXPathNavigable object.
        /// </summary>
        /// <param name="value">The XML string to be converted.</param>
        /// <param name="encoding">The preferred encoding to use.</param>
        /// <returns>An <see cref="IXPathNavigable"/> object.</returns>
        public static IXPathNavigable FromString(string value, Encoding encoding)
        {
            Validator.ThrowIfNull(value, nameof(value));
            Validator.ThrowIfNull(encoding, nameof(encoding));
            using (var stream = Decorator.Enclose(value).ToStream(options =>
            {
                options.Encoding = encoding;
                options.Preamble = PreambleSequence.Keep;
            }))
            {
                return FromStream(stream);
            }
        }

        /// <summary>
        /// Converts the given stream to an <see cref="IXPathNavigable"/> object.
        /// </summary>
        /// <param name="value">The <see cref="Stream"/> to be converted.</param>
        /// <param name="leaveOpen">if <c>true</c>, the source <see cref="Stream"/> is being left open; otherwise it is being closed and disposed.</param>
        /// <returns>An <see cref="IXPathNavigable"/> object.</returns>
        public static IXPathNavigable FromStream(Stream value, bool leaveOpen = false)
        {
            Validator.ThrowIfNull(value, nameof(value));
            if (leaveOpen)
            {
                var reader = XmlReader.Create(value);
                var document = new XPathDocument(reader);
                value.Position = 0;
                return document;
            }
            using (value)
            {
                return new XPathDocument(value);
            }
        }

        /// <summary>
        /// Converts the given XmlReader to an IXPathNavigable object.
        /// </summary>
        /// <param name="value">The XmlReader to be converted.</param>
        /// <returns>An <see cref="IXPathNavigable"/> object.</returns>
        public static IXPathNavigable FromXmlReader(XmlReader value)
        {
            Validator.ThrowIfNull(value, nameof(value));
            return new XPathDocument(value);
        }

        /// <summary>
        /// Converts the given <see cref="Uri"/> to an <see cref="IXPathNavigable"/> object.
        /// </summary>
        /// <param name="value">The <see cref="Uri"/> to be converted.</param>
        /// <returns>An <see cref="IXPathNavigable"/> object.</returns>
        public static IXPathNavigable FromUri(Uri value)
        {
            Validator.ThrowIfNull(value, nameof(value));
            return new XPathDocument(value.ToString());
        }
    }
}