using System;
using System.IO;
using System.Text;
using System.Xml;
using Cuemon.IO;

namespace Cuemon.Xml
{
    /// <summary>
    /// This utility class is designed to make <see cref="XmlReader"/> related conversions easier to work with.
    /// </summary>
    public static class XmlReaderConverter
    {
        /// <summary>
        /// Converts the given stream to an XmlReader object.
        /// </summary>
        /// <param name="value">The stream to be converted.</param>
        /// <returns>An <see cref="System.Xml.XmlReader"/> object.</returns>
        public static XmlReader FromStream(Stream value)
        {
            return FromStream(value, XmlEncodingUtility.ReadEncoding(value));
        }

        /// <summary>
        /// Converts the given stream to an XmlReader object.
        /// </summary>
        /// <param name="value">The stream to be converted.</param>
        /// <param name="encoding">The text encoding to use.</param>
        /// <returns>An <see cref="System.Xml.XmlReader"/> object.</returns>
        public static XmlReader FromStream(Stream value, Encoding encoding)
        {
            if (value == null) { throw new ArgumentNullException(nameof(value)); }
            if (value.CanSeek) { value.Position = 0; }
            XmlReader reader = XmlReader.Create(new StreamReader(value, encoding), XmlReaderUtility.CreateXmlReaderSettings());
            return reader;
        }

        /// <summary>
        /// Converts the given byte array to an XmlReader object.
        /// </summary>
        /// <param name="value">The byte array to be converted.</param>
        /// <returns>An <see cref="System.Xml.XmlReader"/> object.</returns>
        public static XmlReader FromBytes(byte[] value)
        {
            return FromStream(StreamConverter.FromBytes(value));
        }

        /// <summary>
        /// Converts the given URI to an XmlReader object.
        /// </summary>
        /// <param name="value">The URI to be converted.</param>
        /// <returns>An <see cref="System.Xml.XmlReader"/> object.</returns>
        public static XmlReader FromUri(Uri value)
        {
            if (value == null) { throw new ArgumentNullException(nameof(value)); }
            XmlReaderSettings settings = new XmlReaderSettings();
            return XmlReader.Create(value.ToString(), settings);
        }
    }
}