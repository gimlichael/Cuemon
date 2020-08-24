using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization;
using System.Xml;
using System.Xml.Serialization;
using Cuemon.Collections.Generic;
using Cuemon.Reflection;

namespace Cuemon.Xml.Serialization.Converters
{
    /// <summary>
    /// Provides a default way to convert objects to and from XML.
    /// </summary>
    public sealed class DefaultXmlConverter : XmlConverter
    {
        private const string EnumerableElementName = "Item";
        private const string XmlWriterMethod = "WriteXml";

        /// <summary>
        /// Initializes a new instance of the <see cref="DefaultXmlConverter"/> class.
        /// </summary>
        public DefaultXmlConverter(XmlQualifiedEntity rootName, IList<XmlConverter> converters)
        {
            RootName = rootName;
            Converters = converters ?? new List<XmlConverter>();
        }

        private XmlQualifiedEntity RootName { get; }

        private IList<XmlConverter> Converters { get; }

        /// <summary>
        /// Converts an object into its XML representation.
        /// </summary>
        /// <param name="writer">The <see cref="XmlWriter" /> stream to which the object is serialized.</param>
        /// <param name="value">The object to convert.</param>
        /// <param name="elementName">The element name to encapsulate around <paramref name="value" />.</param>
        /// <exception cref="InvalidOperationException">There is an error in the XML document.</exception>
        public override void WriteXml(XmlWriter writer, object value, XmlQualifiedEntity elementName = null)
        {
            Decorator.Enclose(writer).WriteXmlRootElement(value, WriteXmlNodes, elementName ?? RootName);
        }

        /// <summary>
        /// Generates an object from its XML representation.
        /// </summary>
        /// <param name="reader">The <see cref="XmlReader" /> stream from which the object is deserialized.</param>
        /// <param name="objectType">The <see cref="Type"/> of the object to generate.</param>
        /// <returns>The generated (deserialized) object.</returns>
        public override object ReadXml(XmlReader reader, Type objectType)
        {
            Validator.ThrowIfNull(reader, nameof(reader));
            Validator.ThrowIfNull(objectType, nameof(objectType));
            if (Decorator.Enclose(objectType).HasEnumerableImplementation() && objectType != typeof(string))
            {
                if (Decorator.Enclose(objectType).HasDictionaryImplementation())
                {
                    return ParseReadXmlDictionary(reader, objectType);
                }
                return ParseReadXmlEnumerable(reader, objectType);
            }
            var valueTypeInfo = objectType.GetTypeInfo();
            if (valueTypeInfo.IsPrimitive ||
                objectType == typeof(string) ||
                objectType == typeof(Guid) ||
                objectType == typeof(decimal))
            {
                return ParseReadXmlSimple(reader, objectType);
            }
            return ParseReadXmlDefault(reader, objectType);
        }

        /// <summary>
        /// Determines whether this instance can convert the specified object type.
        /// </summary>
        /// <param name="objectType">The <seealso cref="Type" /> of the object.</param>
        /// <returns><c>true</c> if this instance can convert the specified object type; otherwise, <c>false</c>.</returns>
        public override bool CanConvert(Type objectType)
        {
            return true;
        }

        private object ParseReadXmlDictionary(XmlReader reader, Type valueType)
        {
            var values = new Dictionary<string, string>();
            var hierarchy = Decorator.Enclose(reader).ToHierarchy();
            var items = hierarchy.Find(h => h.Instance.Name == EnumerableElementName && h.Depth == 1).ToList();
            foreach (var item in items)
            {
                if (item.HasChildren)
                {
                    try
                    {
                        var key = item.GetChildren().SingleOrDefault();
                        var value = key?.GetChildren().SingleOrDefault();
                        if (value != null)
                        {
                            values.Add(key.Instance.Value.ToString(), value.Instance.Value.ToString());
                        }
                    }
                    catch (Exception ex)
                    {
                        throw new NotSupportedException("Deserialization of complex objects is not supported in this version.", ex);
                    }
                }
            }

            var dictionaryType = valueType.GetGenericArguments().Length > 0 ? valueType.GetGenericArguments() : new[] { typeof(object), typeof(object) };
            var dictionary = typeof(Dictionary<,>).MakeGenericType(dictionaryType);
            var castedValues = values.Select(pair => Template.CreateTwo(Decorator.Enclose(pair.Key).ChangeType(dictionaryType[0]), Decorator.Enclose(pair.Value).ChangeType(dictionaryType[1]))).ToList();
            var instance = Activator.CreateInstance(dictionary);
            var addMethod = valueType.GetMethod("Add");
            foreach (var item in castedValues)
            {
                addMethod.Invoke(instance, new[] { item.Arg1, item.Arg2 });
            }
            return instance;
        }

        private object ParseReadXmlEnumerable(XmlReader reader, Type valueType)
        {
            var values = new List<KeyValuePair<string, string>>();
            var hierarchy = Decorator.Enclose(reader).ToHierarchy();
            var items = hierarchy.Find(h => h.Instance.Name == EnumerableElementName && h.Depth == 1).ToList();
            if (items.FirstOrDefault()?.HasChildren ?? false) { throw new NotSupportedException("Deserialization of complex objects is not supported in this version."); }
            values.AddRange(items.Select(h => new KeyValuePair<string, string>(h.Instance.Name, h.Instance.Value.ToString())));

            var enumerableType = valueType.GetGenericArguments().FirstOrDefault() ?? typeof(object);
            var listEnumerable = typeof(List<>).MakeGenericType(enumerableType);
            var castedValues = values.Where(pair => pair.Key == EnumerableElementName).Select(pair => Decorator.Enclose(pair.Value).ChangeType(enumerableType)).ToList();
            var instance = Activator.CreateInstance(listEnumerable);
            var addMethod = valueType.GetMethod("Add");
            foreach (var item in castedValues)
            {
                addMethod.Invoke(instance, new[] { item });
            }
            return instance;
        }

        private object ParseReadXmlSimple(XmlReader reader, Type valueType)
        {
            string simpleValue = null;
            try
            {
                while (reader.Read())
                {
                    switch (reader.NodeType)
                    {
                        case XmlNodeType.CDATA:
                        case XmlNodeType.Text:
                            simpleValue = reader.Value;
                            break;
                    }
                }
            }
            catch (Exception ex)
            {
                throw new NotSupportedException("Deserialization of complex objects is not supported in this version.", ex);
            }
            return TypeDescriptor.GetConverter(valueType).ConvertFromInvariantString(simpleValue);
        }

        private object ParseReadXmlDefault(XmlReader reader, Type valueType)
        {
            var key = "";
            var values = new Dictionary<string, string>();
            while (reader.Read())
            {
                switch (reader.NodeType)
                {
                    case XmlNodeType.Attribute:
                        values.Add(reader.Name, reader.Value);
                        while (reader.MoveToNextAttribute()) { goto case XmlNodeType.Attribute; }
                        reader.MoveToElement();
                        break;
                    case XmlNodeType.Element:
                        if (reader.Depth == 0) { continue; }
                        key = reader.Name;
                        if (reader.HasAttributes && reader.MoveToFirstAttribute()) { goto case XmlNodeType.Attribute; }
                        break;
                    case XmlNodeType.CDATA:
                    case XmlNodeType.Text:
                        Decorator.Enclose(values).TryAddOrUpdate(key, reader.Value);
                        break;
                }
            }

            var hasDefaultCtor = false;
            var constructors = valueType.GetConstructors(new MemberReflection(excludeStatic: true)).ToList();
            var properties = valueType.GetProperties(new MemberReflection(excludeStatic: true)).Where(info => info.CanWrite).ToDictionary(info => info.Name);
            var propertyNames = properties.Select(info => info.Key).Intersect(values.Select(pair => pair.Key), StringComparer.OrdinalIgnoreCase).ToList();

            var args = new List<object>();
            foreach (var ctr in constructors)
            {
                var arguments = ctr.GetParameters();
                var argumentsLength = arguments.Select(info => info.Name).Intersect(values.Select(pair => pair.Key), StringComparer.OrdinalIgnoreCase).Count();
                if (arguments.Length == argumentsLength)
                {
                    if (!hasDefaultCtor && argumentsLength == 0) { hasDefaultCtor = true; }
                    foreach (var arg in arguments)
                    {
                        args.Add(Decorator.Enclose(values.First(pair => pair.Key.Equals(arg.Name, StringComparison.OrdinalIgnoreCase)).Value).ChangeType(arg.ParameterType));
                    }
                    break;
                }
            }

            if (args.Count == 0)
            {
                var staticMethods = valueType.GetMethods(new MemberReflection(excludeInheritancePath: true)).Where(info => info.ReturnType == valueType && info.IsStatic && !info.IsSpecialName).ToList();
                foreach (var method in staticMethods)
                {
                    var arguments = method.GetParameters();
                    var argumentsLength = arguments.Select(info => info.Name).Intersect(values.Select(pair => pair.Key), StringComparer.OrdinalIgnoreCase).Count();
                    if (arguments.Length == argumentsLength)
                    {
                        foreach (var arg in arguments)
                        {
                            args.Add(Decorator.Enclose(values.First(pair => pair.Key.Equals(arg.Name, StringComparison.OrdinalIgnoreCase)).Value).ChangeType(arg.ParameterType));
                        }
                        return method.Invoke(null, args.ToArray());
                    }
                }
                if (!hasDefaultCtor) { throw new SerializationException("Unable to find a suitable constructor or static method for deserialization."); }
            }

            var instance = Activator.CreateInstance(valueType, args.ToArray());
            foreach (var propertyName in propertyNames)
            {
                var property = properties[propertyName];
                property.SetValue(instance, Decorator.Enclose(values.First(pair => pair.Key == propertyName).Value).ChangeType(property.PropertyType));
            }
            return instance;
        }

        private void WriteXmlNodes(XmlWriter writer, IHierarchy<object> node)
        {
            if (Decorator.Enclose(node).HasXmlIgnoreAttribute()) { return; }

            var writerMethod = node.InstanceType.GetMethod(XmlWriterMethod, new MemberReflection(excludeStatic: true));
            var useWriterMethod = (writerMethod != null) && Decorator.Enclose(node.InstanceType).HasInterface(typeof(IXmlSerializable));

            if (useWriterMethod)
            {
                writerMethod.Invoke(node.Instance, new object[] { writer });
            }
            else
            {
                Condition.FlipFlop(node.HasChildren, WriteXmlChildren, WriteXmlValue, writer, node);
            }
        }

        private void WriteXmlValue(XmlWriter writer, IHierarchy<object> node)
        {
            if (Decorator.Enclose(node).IsNodeEnumerable()) { return; }

            var converter = Decorator.Enclose(Converters).FirstOrDefaultWriterConverter(node.InstanceType);
            if (converter != null)
            {
                converter.WriteXml(writer, node.Instance);
                return;
            }

            var hasAttributeAttribute = node.HasMemberReference && Decorator.Enclose(node.MemberReference).HasAttribute(typeof(XmlAttributeAttribute));
            var hasElementAttribute = node.HasMemberReference && Decorator.Enclose(node.MemberReference).HasAttribute(typeof(XmlElementAttribute));
            var hasTextAttribute = node.HasMemberReference && Decorator.Enclose(node.MemberReference).HasAttribute(typeof(XmlTextAttribute));

            var isType = node.Instance is Type;
            var nodeType = isType ? (Type)node.Instance : node.InstanceType;
            var attributeOrElementName = node.HasMemberReference ? Decorator.Enclose(node.MemberReference.Name).SanitizeXmlElementName() : Decorator.Enclose(Decorator.Enclose(nodeType).ToFriendlyName()).SanitizeXmlElementName();

            if (!hasAttributeAttribute && !hasElementAttribute && !hasTextAttribute)
            {
                hasElementAttribute = true; // default serialization value for legacy Cuemon
                if ((!Decorator.Enclose(nodeType).IsComplex() || isType) && !node.HasChildren && (isType))
                {
                    if (!node.HasParent)
                    {
                        hasElementAttribute = false;
                    }
                    hasTextAttribute = true;
                }
            }
            else
            {
                string elementName = null;
                if (hasAttributeAttribute) { elementName = node.MemberReference.GetCustomAttribute<XmlAttributeAttribute>().AttributeName; }
                if (hasElementAttribute) { elementName = node.MemberReference.GetCustomAttribute<XmlElementAttribute>().ElementName; }

                if (!string.IsNullOrEmpty(elementName))
                {
                    attributeOrElementName = elementName;
                }
            }

            var value = Wrapper.ParseInstance(node);
            if (hasAttributeAttribute)
            {
                writer.WriteAttributeString(attributeOrElementName, value);
            }
            else if (hasElementAttribute)
            {
                if (node.HasMemberReference)
                {
                    writer.WriteElementString(attributeOrElementName, value);
                }
                else
                {
                    writer.WriteValue(value);
                }
            }
            else if (hasTextAttribute)
            {
                writer.WriteString(value);
            }
        }

        private void WriteXmlChildren(XmlWriter writer, IHierarchy<object> node)
        {
            foreach (var childNode in Decorator.Enclose(node.GetChildren()).OrderByXmlAttributes())
            {
                if (Decorator.Enclose(childNode).HasXmlIgnoreAttribute()) { continue; }
                if (!childNode.InstanceType.GetTypeInfo().IsValueType && childNode.Instance == null) { continue; }
                if (Decorator.Enclose(childNode.InstanceType).HasEnumerableImplementation() && childNode.InstanceType != typeof(string) && !Decorator.Enclose(childNode.InstanceType).HasDictionaryImplementation())
                {
                    var i = childNode.Instance as IEnumerable;
                    if (i == null || !i.Cast<object>().Any()) { continue; }
                }
                var qualifiedEntity = Decorator.Enclose(childNode).GetXmlRootOrElement();
                if (childNode.HasChildren && Decorator.Enclose(childNode.InstanceType).IsComplex()) { Decorator.Enclose(writer).WriteStartElement(qualifiedEntity); }
                var converter = Decorator.Enclose(Converters).FirstOrDefaultWriterConverter(childNode.InstanceType);
                if (converter != null)
                {
                    converter.WriteXml(writer, childNode.Instance, qualifiedEntity);
                }
                else
                {
                    WriteXmlNodes(writer, childNode);    
                }
                if (childNode.HasChildren && Decorator.Enclose(childNode.InstanceType).IsComplex()) { writer.WriteEndElement(); }
            }
        }
    }
}