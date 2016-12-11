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
using Cuemon.Xml;
using Cuemon.Xml.Serialization;

namespace Cuemon.Serialization.Xml
{
    /// <summary>
    /// Provides a way to convert objects to and from XML.
    /// </summary>
    public class XmlConverter
    {
        private const string EnumerableElementName = "Item";
        private const string XmlWriterMethod = "WriteXml";

        /// <summary>
        /// Initializes a new instance of the <see cref="XmlConverter"/> class.
        /// </summary>
        /// <param name="setup">The <see cref="XmlConverterOptions"/> which need to be configured.</param>
        public XmlConverter(Action<XmlConverterOptions> setup = null)
        {
            Options = setup.ConfigureOptions();
        }

        /// <summary>
        /// Gets the configured options of this <see cref="XmlConverter"/>.
        /// </summary>
        /// <value>The configured options of this <see cref="XmlConverter"/>.</value>
        public XmlConverterOptions Options { get; }

        /// <summary>
        /// Converts an object into its XML representation.
        /// </summary>
        /// <param name="writer">The <see cref="XmlWriter" /> stream to which the object is serialized.</param>
        /// <param name="value">The object to convert.</param>
        /// <exception cref="InvalidOperationException">There is an error in the XML document.</exception>
        public virtual void WriteXml(XmlWriter writer, object value)
        {
            Validator.ThrowIfNull(writer, nameof(writer));
            Validator.ThrowIfNull(value, nameof(value));
            try
            {
                var root = new HierarchySerializer(value);
                var rootElement = root.Nodes.LookupXmlStartElement(Options.RootName);
                writer.WriteStartElement(rootElement.Prefix, rootElement.LocalName, rootElement.Namespace);
                WriteXmlNodes(writer, root.Nodes);
            }
            catch (Exception ex)
            {
                Exception innerException = ex;
                if (innerException is OutOfMemoryException) { throw; }
                if (innerException is TargetInvocationException) { innerException = innerException.InnerException; }
                throw ExceptionUtility.Refine(new InvalidOperationException("There is an error in the XML document.", innerException), MethodBaseConverter.FromType(typeof(XmlConverter), flags: ReflectionUtility.BindingInstancePublicAndPrivateNoneInheritedIncludeStatic), writer, value);
            }
            writer.WriteEndElement();
            writer.Flush();
        }

        /// <summary>
        /// Generates an object from its XML representation.
        /// </summary>
        /// <param name="reader">The <see cref="XmlReader" /> stream from which the object is deserialized.</param>
        /// <param name="valueType">The <see cref="Type"/> of the object to generate.</param>
        /// <returns>The generated (deserialized) object.</returns>
        public virtual object ReadXml(XmlReader reader, Type valueType)
        {
            Validator.ThrowIfNull(reader, nameof(reader));
            Validator.ThrowIfNull(valueType, nameof(valueType));
            if (valueType.IsEnumerable() && valueType != typeof(string))
            {
                if (valueType.IsDictionary())
                {
                    return ParseReadXmlDictionary(reader, valueType);
                }
                return ParseReadXmlEnumerable(reader, valueType);
            }
            var valueTypeInfo = valueType.GetTypeInfo();
            if (valueTypeInfo.IsPrimitive ||
                valueType == typeof(string) ||
                valueType == typeof(Guid) ||
                valueType == typeof(decimal))
            {
                return ParseReadXmlSimple(reader, valueType);
            }
            return ParseReadXmlDefault(reader, valueType);
        }

        private object ParseReadXmlDictionary(XmlReader reader, Type valueType)
        {
            var values = new Dictionary<string, string>();
            var hierarchy = reader.ToHierarchy();
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

            var dictionaryType = (valueType.GetGenericArguments() ?? valueType.GetAncestorsAndSelf(typeof(object)).Yield()).ToArray();
            var dictionary = typeof(Dictionary<,>).MakeGenericType(dictionaryType);
            var castedValues = values.Select(pair => TupleUtility.CreateTwo(ObjectConverter.ChangeType(pair.Key, dictionaryType[0]), ObjectConverter.ChangeType(pair.Value, dictionaryType[1]))).ToList();
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
            var hierarchy = reader.ToHierarchy();
            var items = hierarchy.Find(h => h.Instance.Name == EnumerableElementName && h.Depth == 1).ToList();
            if (items.FirstOrDefault()?.HasChildren ?? false) { throw new NotSupportedException("Deserialization of complex objects is not supported in this version."); }
            values.AddRange(items.Select(h => new KeyValuePair<string, string>(h.Instance.Name, h.Instance.Value.ToString())));

            var enumerableType = valueType.GetGenericArguments().FirstOrDefault() ?? valueType.GetAncestorsAndSelf(typeof(object));
            var listEnumerable = typeof(List<>).MakeGenericType(enumerableType);
            var castedValues = values.Where(pair => pair.Key == EnumerableElementName).Select(pair => ObjectConverter.ChangeType(pair.Value, enumerableType)).ToList();
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
                        if (reader.HasAttributes)
                        {
                            if (reader.MoveToFirstAttribute()) { goto case XmlNodeType.Attribute; }
                        }
                        break;
                    case XmlNodeType.CDATA:
                    case XmlNodeType.Text:
                        values.AddOrUpdate(key, reader.Value);
                        break;
                }
            }

            var constructors = valueType.GetConstructors(ReflectionUtility.BindingInstancePublicAndPrivate).ToList();
            var properties = valueType.GetProperties(ReflectionUtility.BindingInstancePublicAndPrivate).Where(info => info.CanWrite).ToDictionary(info => info.Name);
            var propertyNames = properties.Select(info => info.Key).Intersect(values.Select(pair => pair.Key), StringComparer.OrdinalIgnoreCase).ToList();

            List<object> args = new List<object>();
            foreach (var ctr in constructors)
            {
                var arguments = ctr.GetParameters();
                var argumentsLength = arguments.Select(info => info.Name).Intersect(values.Select(pair => pair.Key), StringComparer.OrdinalIgnoreCase).Count();
                if (arguments.Length == argumentsLength)
                {
                    foreach (var arg in arguments)
                    {
                        args.Add(ObjectConverter.ChangeType(values.First(pair => pair.Key.Equals(arg.Name, StringComparison.OrdinalIgnoreCase)).Value, arg.ParameterType));
                    }
                    break;
                }
            }

            if (args.Count == 0)
            {
                var staticMethods = valueType.GetMethods(ReflectionUtility.BindingInstancePublicAndPrivateNoneInheritedIncludeStatic).Where(info => info.ReturnType == valueType && info.IsStatic).ToList();
                foreach (var method in staticMethods)
                {
                    var arguments = method.GetParameters();
                    var argumentsLength = arguments.Select(info => info.Name).Intersect(values.Select(pair => pair.Key), StringComparer.OrdinalIgnoreCase).Count();
                    if (arguments.Length == argumentsLength)
                    {
                        foreach (var arg in arguments)
                        {
                            args.Add(ObjectConverter.ChangeType(values.First(pair => pair.Key.Equals(arg.Name, StringComparison.OrdinalIgnoreCase)).Value, arg.ParameterType));
                        }
                        return method.Invoke(null, args.ToArray());
                    }
                }
                throw new SerializationException("Unable to find a suitable constructor or static method for deserialization.");
            }

            var instance = Activator.CreateInstance(valueType, args.ToArray());
            foreach (var propertyName in propertyNames)
            {
                var property = properties[propertyName];
                property.SetValue(instance, ObjectConverter.ChangeType(values.First(pair => pair.Key == propertyName).Value, property.PropertyType));
            }
            return instance;
        }

        private void WriteXmlNodes(XmlWriter writer, IHierarchy<object> node)
        {
            if (node.HasXmlIgnoreAttribute()) { return; }

            var writerMethod = node.InstanceType.GetMethod(XmlWriterMethod, ReflectionUtility.BindingInstancePublicAndPrivate);
            var useWriterMethod = (writerMethod != null) && node.InstanceType.HasInterfaces(typeof(IXmlSerializable));

            if (useWriterMethod)
            {
                writerMethod.Invoke(node.Instance, new object[] { writer });
            }
            else
            {
                ParseWriteXml(writer, node);
            }
        }

        private void WriteXmlValue(XmlWriter writer, IHierarchy<object> node)
        {
            if (node.IsNodeEnumerable()) { return; }
            bool enumerableCaller = node.Data.ContainsKey("enumerableCaller") && node.Data["enumerableCaller"].As<bool>();

            bool hasAttributeAttribute = node.HasMemberReference && TypeUtility.ContainsAttributeType(node.MemberReference, typeof(XmlAttributeAttribute));
            bool hasElementAttribute = node.HasMemberReference && TypeUtility.ContainsAttributeType(node.MemberReference, typeof(XmlElementAttribute));
            bool hasTextAttribute = node.HasMemberReference && TypeUtility.ContainsAttributeType(node.MemberReference, typeof(XmlTextAttribute));

            bool isType = node.Instance is Type;
            Type nodeType = isType ? (Type)node.Instance : node.InstanceType;
            string attributeOrElementName = XmlUtility.SanitizeElementName(node.HasMemberReference ? node.MemberReference.Name : StringConverter.FromType(nodeType));

            if (!hasAttributeAttribute && !hasElementAttribute && !hasTextAttribute)
            {
                hasElementAttribute = true; // default serialization value for legacy Cuemon
                if ((!TypeUtility.IsComplex(nodeType) || isType) && !node.HasChildren && (!enumerableCaller || isType))
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
            if (enumerableCaller)
            {
                writer.WriteString(value);
            }
            else if (hasAttributeAttribute)
            {
                writer.WriteAttributeString(attributeOrElementName, value);
            }
            else if (hasElementAttribute)
            {
                writer.WriteElementString(attributeOrElementName, value);
            }
            else if (hasTextAttribute)
            {
                writer.WriteString(value);
            }
        }

        private void ParseWriteXml(XmlWriter writer, IHierarchy<object> node)
        {
            WriteXmlEnumerable(writer, node, !node.HasParent);
            Condition.FlipFlop(node.HasChildren, WriteXmlChildren, WriteXmlValue, writer, node);
        }

        private void WriteXmlChildren(XmlWriter writer, IHierarchy<object> node)
        {
            foreach (IHierarchy<object> childNode in node.GetChildren().OrderByXmlAttributes())
            {
                if (childNode.HasXmlIgnoreAttribute()) { return; }

                XmlQualifiedEntity qualifiedEntity = childNode.LookupXmlStartElement();
                if (childNode.HasChildren && TypeUtility.IsComplex(childNode.InstanceType)) { writer.WriteStartElement(qualifiedEntity.Prefix, qualifiedEntity.LocalName, qualifiedEntity.Namespace); }
                WriteXmlNodes(writer, childNode);
                if (childNode.HasChildren && TypeUtility.IsComplex(childNode.InstanceType)) { writer.WriteEndElement(); }
            }
        }

        private void WriteXmlEnumerable(XmlWriter writer, IHierarchy<object> current, bool skipStartElement = false)
        {
            Type currentType = current.InstanceType;
            Type[] genericParameters = currentType.GetGenericArguments();
            if (genericParameters.Length == 0) { genericParameters = null; }
            if (TypeUtility.IsEnumerable(currentType) && currentType != typeof(string))
            {
                bool isDictionary = TypeUtility.IsDictionary(currentType);
                IEnumerable enumerable = current.Instance as IEnumerable;
                if (enumerable != null)
                {
                    var list = enumerable.Cast<object>().ToList();
                    if (!skipStartElement && !current.HasChildren && list.Count > 0)
                    {
                        XmlQualifiedEntity qualifiedEntity = null;
                        if (!current.InstanceType.HasAttributes(typeof(XmlRootAttribute)) && current.MemberReference == null)
                        {
                            qualifiedEntity = new XmlQualifiedEntity(StringConverter.FromType(current.InstanceType, false, true).SanitizeElementName());
                        }
                        qualifiedEntity = current.LookupXmlStartElement(qualifiedEntity);
                        writer.WriteStartElement(qualifiedEntity.Prefix, qualifiedEntity.LocalName, qualifiedEntity.Namespace);
                    }
                    IEnumerator enumerator = list.GetEnumerator();
                    IHierarchy<object> enumeratorNode = new Hierarchy<object>();
                    enumeratorNode.Add(enumerator);
                    while (enumerator.MoveNext())
                    {
                        object value = enumerator.Current;
                        if (value == null) { continue; }
                        writer.WriteStartElement(EnumerableElementName);
                        Type valueType = value.GetType();

                        if (isDictionary)
                        {
                            PropertyInfo keyProperty = valueType.GetProperty("Key");
                            PropertyInfo valueProperty = valueType.GetProperty("Value");
                            object keyValue = keyProperty.GetValue(value, null) ?? "null";
                            object valueValue = valueProperty.GetValue(value, null) ?? "null";
                            var kvpWrapper = DynamicXmlSerializable.Create(new[] { keyValue, valueValue }, (xmlWriter, o) =>
                            {
                                var k = o[0];
                                var v = ReflectionUtility.GetObjectHierarchy(o[1], options => options.MaxDepth = 0);
                                v.Data.Add("enumerableCaller", true);
                                writer.WriteAttributeString("key", k.ToString());

                                XmlQualifiedEntity qualifiedEntity = null;
                                if (genericParameters != null && genericParameters.Length > 0)
                                {
                                    qualifiedEntity = new XmlQualifiedEntity(StringConverter.FromType(genericParameters.Last(), false, true).SanitizeElementName());
                                }

                                qualifiedEntity = v.LookupXmlStartElement(qualifiedEntity);
                                writer.WriteStartElement(qualifiedEntity.Prefix, qualifiedEntity.LocalName, qualifiedEntity.Namespace);
                                WriteXmlNodes(writer, v);
                                writer.WriteEndElement();
                            });
                            WriteXmlNodes(writer, ReflectionUtility.GetObjectHierarchy(kvpWrapper, options => options.MaxDepth = 0));
                        }
                        else
                        {
                            IHierarchy<object> itemNode = ReflectionUtility.GetObjectHierarchy(value, options => options.MaxDepth = 0);
                            itemNode.Data.Add("enumerableCaller", true);

                            XmlQualifiedEntity qualifiedEntity = null;

                            if (genericParameters != null && genericParameters.Length > 0)
                            {
                                qualifiedEntity = new XmlQualifiedEntity(StringConverter.ToDelimitedString(genericParameters, "And", StringConverter.FromType));
                            }

                            qualifiedEntity = itemNode.LookupXmlStartElement(qualifiedEntity);
                            writer.WriteStartElement(qualifiedEntity.Prefix, qualifiedEntity.LocalName, qualifiedEntity.Namespace);
                            WriteXmlNodes(writer, itemNode);
                            writer.WriteEndElement();
                        }
                        writer.WriteEndElement();
                    }
                    if (!skipStartElement && !current.HasChildren && list.Count > 0) { writer.WriteEndElement(); }
                }
            }
        }
    }
}