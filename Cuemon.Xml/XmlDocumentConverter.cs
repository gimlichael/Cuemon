using System;
using System.IO;
using System.Xml;
using Cuemon.IO;

namespace Cuemon.Xml
{
    /// <summary>
    /// This utility class is designed to make <see cref="XmlDocument"/> related conversions easier to work with.
    /// </summary>
    public static class XmlDocumentConverter
    {
        /// <summary>
        /// Converts the given <see cref="Stream"/> to an <see cref="XmlDocument"/>.
        /// </summary>
        /// <param name="stream">The <see cref="Stream"/> to be converted.</param>
        /// <param name="leaveOpen">if <c>true</c>, the <see cref="Stream"/> object is being left open; otherwise it is being closed and disposed.</param>
        /// <returns>An <see cref="XmlDocument"/> object.</returns>
        public static XmlDocument FromStream(Stream stream, bool leaveOpen = false)
        {
            Validator.ThrowIfNull(stream, nameof(stream));
            long startPosition = -1;
            if (stream.CanSeek)
            {
                startPosition = stream.Position;
                stream.Position = 0;
            }

            var document = new XmlDocument();
            if (leaveOpen)
            {
                document.Load(stream);
                if (stream.CanSeek) { stream.Seek(startPosition, SeekOrigin.Begin); }
            }
            else
            {
                using (stream) { document.Load(stream); }
            }

            return document;
        }

        /// <summary>
        /// Converts the given <see cref="XmlReader"/> to an <see cref="XmlDocument"/>.
        /// </summary>
        /// <param name="reader">The <see cref="XmlReader"/> to be converted.</param>
        /// <param name="leaveOpen">if <c>true</c>, the <see cref="XmlReader"/> object is being left open; otherwise it is being closed and disposed.</param>
        /// <returns>An <see cref="XmlDocument"/> object.</returns>
        public static XmlDocument FromReader(XmlReader reader, bool leaveOpen = false)
        {
            var document = new XmlDocument();
            if (leaveOpen)
            {
                document.Load(reader);
            }
            else
            {
                using (reader) { document.Load(reader); }
            }
            return document;
        }

        /// <summary>
        /// Converts the given URI to an <see cref="XmlDocument"/>.
        /// </summary>
        /// <param name="value">The URI to be converted.</param>
        /// <returns>An <b>XmlDocument</b> object.</returns>
        public static XmlDocument FromUri(Uri value)
        {
            Validator.ThrowIfNull(value, nameof(value));
            var document = new XmlDocument();
            document.Load(TextReaderConverter.FromString(value.ToString()));
            return document;
        }

        /// <summary>
        /// Converts the given string to an <see cref="XmlDocument"/>.
        /// </summary>
        /// <param name="value">The string to be converted.</param>
        /// <returns>An <b>XmlDocument</b>object.</returns>
        public static XmlDocument FromString(string value)
        {
            Validator.ThrowIfNullOrWhitespace(value, nameof(value));
            var document = new XmlDocument();
            try
            {
                document.LoadXml(value);
            }
            catch (XmlException ex)
            {
                throw new XmlException("Unable to load XML - this is typical because you are trying to load a file. Use the overloaded method that takes a URI as parameter instead.", ex);
            }
            return document;
        }
    }
}