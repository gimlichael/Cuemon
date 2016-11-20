using System;
using System.Xml.Serialization;

namespace Cuemon.Xml.Serialization
{
    /// <summary>
    /// This utility class is designed to make XML serialization operations easier to work with.
    /// </summary>
    public static partial class XmlSerializationUtility
    {
        /// <summary>
        /// Determines whether the specified object contains XML serialization attributes.
        /// </summary>
        /// <param name="source">The object to parse for XML serialization attributes.</param>
        /// <returns>
        /// 	<c>true</c> if the specified object contains XML serialization attributes; otherwise, <c>false</c>.
        /// </returns>
        public static bool ContainsXmlSerializationAttributes(object source)
        {
            if (source == null) throw new ArgumentNullException(nameof(source));
            return ContainsXmlSerializationAttributes(source.GetType());
        }

        /// <summary>
        /// Determines whether the specified type contains XML serialization attributes.
        /// </summary>
        /// <param name="sourceType">The type to parse for XML serialization attributes.</param>
        /// <returns>
        /// 	<c>true</c> if the specified type contains XML serialization attributes; otherwise, <c>false</c>.
        /// </returns>
        public static bool ContainsXmlSerializationAttributes(Type sourceType)
        {
            return TypeUtility.ContainsAttributeType(sourceType, typeof(XmlAnyAttributeAttribute), typeof(XmlAnyElementAttribute), typeof(XmlArrayAttribute), typeof(XmlArrayItemAttribute),
                typeof(XmlAttributeAttribute), typeof(XmlChoiceIdentifierAttribute), typeof(XmlElementAttribute), typeof(XmlEnumAttribute),
                typeof(XmlTextAttribute), typeof(XmlTypeAttribute));
            // decided to leave out XmlRootAttribute, as this can favor more use of the automated serialization (where the coder still can override the class name with a new root name).
        }
    }
}