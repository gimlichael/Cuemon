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
        /// Converts the given <see cref="Stream"/> to an <see cref="XmlDocument"/>. The stream is closed and disposed of afterwards.
        /// </summary>
        /// <param name="value">The <see cref="Stream"/> to be converted.</param>
        /// <returns>An <see cref="XmlDocument"/> object.</returns>
        public static XmlDocument FromStream(Stream value)
        {
            return FromStream(value, false);
        }

        /// <summary>
        /// Converts the given <see cref="Stream"/> to an <see cref="XmlDocument"/>.
        /// </summary>
        /// <param name="value">The <see cref="Stream"/> to be converted.</param>
        /// <param name="leaveStreamOpen">if <c>true</c>, the <see cref="Stream"/> object is being left open; otherwise it is being closed and disposed.</param>
        /// <returns>An <see cref="XmlDocument"/> object.</returns>
        public static XmlDocument FromStream(Stream value, bool leaveStreamOpen)
        {
            if (value == null) { throw new ArgumentNullException(nameof(value)); }
            long startPosition = -1;
            if (value.CanSeek)
            {
                startPosition = value.Position;
                value.Position = 0;
            }

            XmlDocument document = new XmlDocument();
            if (leaveStreamOpen)
            {
                document.Load(value);
                if (value.CanSeek) { value.Seek(startPosition, SeekOrigin.Begin); }
            }
            else
            {
                using (value) { document.Load(value); }
            }

            return document;
        }

        /// <summary>
        /// Converts the given <see cref="XmlReader"/> to an <see cref="XmlDocument"/>.
        /// </summary>
        /// <param name="value">The <see cref="XmlReader"/> to be converted.</param>
        /// <returns>An <see cref="XmlDocument"/> object.</returns>
        public static XmlDocument FromReader(XmlReader value)
        {
            return FromReader(value, false);
        }

        /// <summary>
        /// Converts the given <see cref="XmlReader"/> to an <see cref="XmlDocument"/>.
        /// </summary>
        /// <param name="value">The <see cref="XmlReader"/> to be converted.</param>
        /// <param name="leaveStreamOpen">if <c>true</c>, the <see cref="XmlReader"/> object is being left open; otherwise it is being closed and disposed.</param>
        /// <returns>An <see cref="XmlDocument"/> object.</returns>
        public static XmlDocument FromReader(XmlReader value, bool leaveStreamOpen)
        {
            XmlDocument document = new XmlDocument();
            if (leaveStreamOpen)
            {
                document.Load(value);
            }
            else
            {
                using (value) { document.Load(value); }
            }
            return document;
        }

        /// <summary>
        /// Converts the given URI to an <see cref="XmlDocument"/>.
        /// </summary>
        /// <param name="value">The URI to be converted.</param>
        /// <returns>An <b>XmlDocument</b>object.</returns>
        public static XmlDocument FromUri(Uri value)
        {
            if (value == null) { throw new ArgumentNullException(nameof(value)); }
            XmlDocument document = new XmlDocument();
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
            if (value == null) { throw new ArgumentNullException(nameof(value)); }
            XmlDocument document = new XmlDocument();
            try
            {
                document.LoadXml(value);
            }
            catch (XmlException ex)
            {
                throw new XmlException("Unable to load XML - this is typical because you are trying to load a file. Use the overloaded method that takes an Uri as parameter instead.", ex);
            }
            return document;
        }
    }
}