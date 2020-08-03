using System.Xml;
using Cuemon.Xml;

namespace Cuemon.Extensions.Xml.Serialization
{
    /// <summary>
    /// Extension methods for the <see cref="XmlReader"/> class.
    /// </summary>
    public static class XmlReaderExtensions
    {
        /// <summary>
        /// Converts the XML hierarchy of an <see cref="XmlReader"/> into an <see cref="IHierarchy{T}"/>.
        /// </summary>
        /// <param name="reader">The <see cref="XmlReader"/> to extend.</param>
        /// <returns>An <see cref="IHierarchy{T}"/> implementation that uses <see cref="DataPair"/>.</returns>
        public static IHierarchy<DataPair> ToHierarchy(this XmlReader reader)
        {
            return Decorator.Enclose(reader).ToHierarchy();
        }
    }
}