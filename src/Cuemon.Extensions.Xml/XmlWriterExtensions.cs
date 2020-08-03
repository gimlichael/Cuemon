using System;
using System.Xml;
using Cuemon.Xml;
using Cuemon.Xml.Serialization;

namespace Cuemon.Extensions.Xml
{
    /// <summary>
    /// Extension methods for the <see cref="XmlWriter"/>.
    /// </summary>
    public static class XmlWriterExtensions
    {
        /// <summary>
        /// Serializes the specified <paramref name="value"/> into an XML format.
        /// </summary>
        /// <typeparam name="T">The type of the object to serialize.</typeparam>
        /// <param name="writer">The writer used in the serialization process.</param>
        /// <param name="value">The object to serialize.</param>
        /// <param name="setup">The <see cref="XmlSerializerOptions"/> which need to be configured.</param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="writer"/> cannot be null.
        /// </exception>
        public static void WriteObject<T>(this XmlWriter writer, T value, Action<XmlSerializerOptions> setup = null)
        {
            Validator.ThrowIfNull(writer, nameof(writer));
            WriteObject(writer, value, typeof(T), setup);
        }

        /// <summary>
        /// Serializes the specified <paramref name="value"/> into an XML format.
        /// </summary>
        /// <param name="writer">The writer used in the serialization process.</param>
        /// <param name="value">The object to serialize.</param>
        /// <param name="objectType">The type of the object to serialize.</param>
        /// <param name="setup">The <see cref="XmlSerializerOptions"/> which need to be configured.</param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="writer"/> cannot be null.
        /// </exception>
        public static void WriteObject(this XmlWriter writer, object value, Type objectType, Action<XmlSerializerOptions> setup = null)
        {
            Validator.ThrowIfNull(writer, nameof(writer));
            Decorator.Enclose(writer).WriteObject(value, objectType, setup);
        }

        /// <summary>
        /// Writes the specified start tag and associates it with the given <paramref name="elementName"/>.
        /// </summary>
        /// <param name="writer">The writer used in the serialization process.</param>
        /// <param name="elementName">The fully qualified name of the element.</param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="writer"/> cannot be null.
        /// </exception>
        public static void WriteStartElement(this XmlWriter writer, XmlQualifiedEntity elementName)
        {
            Validator.ThrowIfNull(writer, nameof(writer));
            Decorator.Enclose(writer).WriteStartElement(elementName);
        }

        /// <summary>
        /// Writes the specified <paramref name="value"/> with the delegate <paramref name="nodeWriter"/>.
        /// If <paramref name="elementName"/> is not null, then the delegate <paramref name="nodeWriter"/> is called from within an encapsulating Start- and End-element.
        /// </summary>
        /// <typeparam name="T">The type of the object to serialize.</typeparam>
        /// <param name="writer">The writer used in the serialization process.</param>
        /// <param name="value">The object to serialize.</param>
        /// <param name="elementName">The optional fully qualified name of the element.</param>
        /// <param name="nodeWriter">The delegate node writer.</param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="writer"/> cannot be null.
        /// </exception>
        public static void WriteEncapsulatingElementIfNotNull<T>(this XmlWriter writer, T value, XmlQualifiedEntity elementName, Action<XmlWriter, T> nodeWriter)
        {
            Validator.ThrowIfNull(writer, nameof(writer));
            Decorator.Enclose(writer).WriteEncapsulatingElementIfNotNull(value, elementName, nodeWriter);
        }

        /// <summary>
        /// Writes the XML root element to an existing <paramref name="writer"/>.
        /// </summary>
        /// <typeparam name="T">The type of the object to serialize.</typeparam>
        /// <param name="writer">The writer used in the serialization process.</param>
        /// <param name="value">The object to serialize.</param>
        /// <param name="treeWriter">The delegate used to write the XML hierarchy.</param>
        /// <param name="rootEntity">The optional <seealso cref="XmlQualifiedEntity"/> that will provide the name of the root element.</param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="writer"/> cannot be null.
        /// </exception>
        public static void WriteXmlRootElement<T>(this XmlWriter writer, T value, Action<XmlWriter, T, XmlQualifiedEntity> treeWriter, XmlQualifiedEntity rootEntity = null)
        {
            Validator.ThrowIfNull(writer, nameof(writer));
            Decorator.Enclose(writer).WriteXmlRootElement(value, treeWriter, rootEntity);
        }
    }
}