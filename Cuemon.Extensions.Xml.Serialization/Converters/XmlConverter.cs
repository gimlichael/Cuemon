using System;
using System.Xml;
using Cuemon.Xml.Serialization;

namespace Cuemon.Extensions.Xml.Serialization.Converters
{
    /// <summary>
    /// Converts an object to and from XML.
    /// </summary>
    public abstract class XmlConverter
    {
        /// <summary>
        /// Writes the XML representation of the <paramref name="value" />.
        /// </summary>
        /// <param name="writer">The <see cref="XmlWriter" /> to write to.</param>
        /// <param name="value">The object to serialize.</param>
        /// <param name="elementName">The element name to encapsulate around <paramref name="value"/>.</param>
        public abstract void WriteXml(XmlWriter writer, object value, XmlQualifiedEntity elementName = null);

        /// <summary>
        /// Reads the XML representation of the <paramref name="objectType"/>.
        /// </summary>
        /// <param name="reader">The <see cref="XmlReader"/> to read from.</param>
        /// <param name="objectType">The <seealso cref="Type"/> of the object.</param>
        /// <returns>An object of <paramref name="objectType"/>.</returns>
        public abstract object ReadXml(XmlReader reader, Type objectType);

        /// <summary>
        /// Determines whether this instance can convert the specified object type.
        /// </summary>
        /// <param name="objectType">The <seealso cref="Type"/> of the object.</param>
        /// <returns><c>true</c> if this instance can convert the specified object type; otherwise, <c>false</c>.</returns>
        public abstract bool CanConvert(Type objectType);

        /// <summary>
        /// Gets a value indicating whether this <seealso cref="XmlConverter"/> can XML.
        /// </summary>
        /// <value><c>true</c> if this <seealso cref="XmlConverter"/> can read XML; otherwise, <c>false</c>.</value>
        public virtual bool CanRead => true;

        /// <summary>
        /// Gets a value indicating whether this <seealso cref="XmlConverter"/> can write XML.
        /// </summary>
        /// <value><c>true</c> if this <seealso cref="XmlConverter"/> can write XML; otherwise, <c>false</c>.</value>
        public virtual bool CanWrite => true;
    }
}