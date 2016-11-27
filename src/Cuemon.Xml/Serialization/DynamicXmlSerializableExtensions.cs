using System;
using System.Xml;
using System.Xml.Serialization;

namespace Cuemon.Xml.Serialization
{
    /// <summary>
    /// Extension methods that complements the <see cref="DynamicXmlSerializable"/> class.
    /// </summary>
    public static class DynamicXmlSerializableExtensions
    {
        /// <summary>
        /// Creates a dynamic instance of an <see cref="IXmlSerializable"/> implementation wrapping <see cref="IXmlSerializable.WriteXml"/> through <paramref name="writer"/> and <see cref="IXmlSerializable.ReadXml"/> through <paramref name="reader"/>.
        /// </summary>
        /// <typeparam name="T">The type of the <paramref name="source"/> to implement an <see cref="IXmlSerializable"/>.</typeparam>
        /// <param name="source">The object that needs support for an <see cref="IXmlSerializable"/> implementation.</param>
        /// <param name="writer">The delegate that converts <paramref name="source"/> to its XML representation.</param>
        /// <param name="reader">The delegate that generates <paramref name="source"/> from its XML representation.</param>
        /// <returns>An <see cref="IXmlSerializable"/> implementation of <paramref name="source"/>.</returns>
        public static IXmlSerializable ToXmlSerializable<T>(this T source, Action<XmlWriter, T> writer, Action<XmlReader, T> reader)
        {
            return new DynamicXmlSerializable<T>(source, writer, reader);
        }
    }
}