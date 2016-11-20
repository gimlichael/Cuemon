using System;
using System.Collections.Generic;
using System.Reflection;
using System.Xml;
using System.Xml.Serialization;
using Cuemon.Reflection;

namespace Cuemon.Xml.Serialization
{
    public static partial class XmlSerializationUtility
    {
        internal static void ParseReadXml(XmlReader reader, object instance)
        {
            if (reader == null) { throw new ArgumentNullException(nameof(reader)); }
            if (instance == null) { throw new ArgumentNullException(nameof(instance)); }

            Type instanceType = instance.GetType();
            IEnumerable<PropertyInfo> properties = ReflectionUtility.GetProperties(instanceType, ReflectionUtility.BindingInstancePublicAndPrivate);
            ReaderToPropertiesCopier(reader, instance, properties);
        }

        private static void ReaderToPropertiesCopier(XmlReader reader, object instance, IEnumerable<PropertyInfo> properties)
        {
            if (reader == null) { throw new ArgumentNullException(nameof(reader)); }
            if (properties == null) { throw new ArgumentNullException(nameof(properties)); }

            properties = new List<PropertyInfo>(properties);
            string localName = null;
            while (reader.Read())
            {
                switch (reader.NodeType)
                {
                    case XmlNodeType.Text:
                    case XmlNodeType.CDATA:
                        PropertyInfo property = GetReaderEquivalentProperty(localName, properties);
                        if (property != null && property.CanWrite)
                        {
                            if (TypeUtility.IsComplex(property.PropertyType))
                            {
                                // TODO: Extend the deserializer with support for complex/enumerable types
                            }
                            else
                            {
                                property.SetValue(instance, ObjectConverter.ChangeType(reader.Value, property.PropertyType), null);
                            }
                        }
                        break;
                    case XmlNodeType.Attribute:
                    case XmlNodeType.Element:
                        localName = reader.LocalName;
                        break;
                    case XmlNodeType.Comment:
                    case XmlNodeType.Document:
                    case XmlNodeType.DocumentFragment:
                    case XmlNodeType.DocumentType:
                    case XmlNodeType.EndElement:
                    case XmlNodeType.EntityReference:
                    case XmlNodeType.EndEntity:
                    case XmlNodeType.None:
                    case XmlNodeType.Notation:
                    case XmlNodeType.Entity:
                    case XmlNodeType.Whitespace:
                    case XmlNodeType.SignificantWhitespace:
                    case XmlNodeType.ProcessingInstruction:
                    case XmlNodeType.XmlDeclaration:
                        break;
                }
            }
        }

        private static PropertyInfo GetReaderEquivalentProperty(string localName, IEnumerable<PropertyInfo> properties)
        {
            foreach (PropertyInfo property in properties)
            {
                string propertyName = property.Name;
                bool hasAttributeAttribute = TypeUtility.ContainsAttributeType(property.PropertyType, typeof(XmlAttributeAttribute));
                bool hasElementAttribute = TypeUtility.ContainsAttributeType(property.PropertyType, typeof(XmlElementAttribute));
                if (hasAttributeAttribute) { propertyName = property.PropertyType.GetTypeInfo().GetCustomAttribute<XmlAttributeAttribute>().AttributeName; }
                if (hasElementAttribute) { propertyName = property.PropertyType.GetTypeInfo().GetCustomAttribute<XmlElementAttribute>().ElementName; }
                if (localName.Equals(propertyName, StringComparison.OrdinalIgnoreCase)) { return property; }
            }
            return null;
        }
    }
}