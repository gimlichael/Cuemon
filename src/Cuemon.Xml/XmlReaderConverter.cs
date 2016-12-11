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
            return FromStream(value, null);
        }

        /// <summary>
        /// Converts the given stream to an XmlReader object.
        /// </summary>
        /// <param name="value">The stream to be converted.</param>
        /// <param name="encoding">The text encoding to use.</param>
        /// <param name="setup">The <see cref="XmlReaderSettings"/> which need to be configured.</param>
        /// <returns>An <see cref="System.Xml.XmlReader"/> object.</returns>
        /// <remarks>If <paramref name="encoding"/> is null, an <see cref="Encoding"/> object will be attempted resolved by <see cref="XmlEncodingUtility.ReadEncoding(Stream)"/>.</remarks>
        public static XmlReader FromStream(Stream value, Encoding encoding, Action<XmlReaderSettings> setup = null)
        {
            Validator.ThrowIfNull(value, nameof(value));
            if (encoding == null) { encoding = XmlEncodingUtility.ReadEncoding(value); }
            if (value.CanSeek) { value.Position = 0; }
            var options = setup.ConfigureOptions();
            XmlReader reader = XmlReader.Create(new StreamReader(value, encoding), options);
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
        /// <param name="setup">The <see cref="XmlReaderSettings"/> which need to be configured.</param>
        /// <returns>An <see cref="System.Xml.XmlReader"/> object.</returns>
        public static XmlReader FromUri(Uri value, Action<XmlReaderSettings> setup = null)
        {
            Validator.ThrowIfNull(value, nameof(value));
            var options = setup.ConfigureOptions();
            return XmlReader.Create(value.ToString(), options);
        }
    }
}