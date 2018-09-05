using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Xml;
using Cuemon.IO;
using Cuemon.Serialization.Formatters;
using Cuemon.Xml.Linq;
using Cuemon.Xml.Serialization;

namespace Cuemon.Serialization.Xml.Converters
{
    /// <summary>
    /// Extension methods for the <see cref=" IList{XmlConverter}"/>.
    /// </summary>
    public static class XmlConverterListExtensions
    {
        /// <summary>
        /// Adds an XML converter to the list.
        /// </summary>
        /// <typeparam name="T">The type of the object to converts to and from XML.</typeparam>
        /// <param name="converters">The list of XML converters.</param>
        /// <param name="writer">The delegate that converts <typeparamref name="T"/> to its XML representation.</param>
        /// <param name="reader">The delegate that generates <typeparamref name="T"/> from its XML representation.</param>
        /// <param name="canConvertPredicate">The delegate that determines if an object can be converted.</param>
        public static void AddXmlConverter<T>(this IList<XmlConverter> converters, Action<XmlWriter, T, XmlQualifiedEntity> writer = null, Func<XmlReader, Type, T> reader = null, Func<Type, bool> canConvertPredicate = null)
        {
            converters.Add(DynamicXmlConverter.Create(writer, reader, canConvertPredicate));
        }

        /// <summary>
        /// Inserts an XML converter to the list at the specified <paramref name="index" />.
        /// </summary>
        /// <typeparam name="T">The type of the object to converts to and from XML.</typeparam>
        /// <param name="converters">The list of XML converters.</param>
        /// <param name="index">The zero-based index at which an XML converter should be inserted.</param>
        /// <param name="writer">The delegate that converts <typeparamref name="T" /> to its XML representation.</param>
        /// <param name="reader">The delegate that generates <typeparamref name="T" /> from its XML representation.</param>
        /// <param name="canConvertPredicate">The delegate that determines if an object can be converted.</param>
        public static void InsertXmlConverter<T>(this IList<XmlConverter> converters, int index, Action<XmlWriter, T, XmlQualifiedEntity> writer = null, Func<XmlReader, Type, T> reader = null, Func<Type, bool> canConvertPredicate = null)
        {
            converters.Insert(index, DynamicXmlConverter.Create(writer, reader, canConvertPredicate));
        }

        /// <summary>
        /// Returns the first <see cref="XmlConverter"/> of the <paramref name="converters"/> that <see cref="XmlConverter.CanConvert"/> and <see cref="XmlConverter.CanRead"/> the specified <paramref name="objectType"/>; otherwise <c>null</c> if no <see cref="XmlConverter"/> is found.
        /// </summary>
        /// <param name="converters">The list of XML converters.</param>
        /// <param name="objectType">Type of the object to deserialize.</param>
        /// <returns>An <see cref="XmlConverter"/> that can deserialize the specified <paramref name="objectType"/>; otherwise <c>null</c>.</returns>
        public static XmlConverter FirstOrDefaultReaderConverter(this IList<XmlConverter> converters, Type objectType)
        {
            return converters.FirstOrDefault(c => c.CanConvert(objectType) && c.CanRead);
        }

        /// <summary>
        /// Returns the first <see cref="XmlConverter"/> of the <paramref name="converters"/> that <see cref="XmlConverter.CanConvert"/> and <see cref="XmlConverter.CanWrite"/> the specified <paramref name="objectType"/>; otherwise <c>null</c> if no <see cref="XmlConverter"/> is found.
        /// </summary>
        /// <param name="converters">The list of XML converters.</param>
        /// <param name="objectType">Type of the object to serialize.</param>
        /// <returns>An <see cref="XmlConverter"/> that can serialize the specified <paramref name="objectType"/>; otherwise <c>null</c>.</returns>
        public static XmlConverter FirstOrDefaultWriterConverter(this IList<XmlConverter> converters, Type objectType)
        {
            return converters.FirstOrDefault(c => c.CanConvert(objectType) && c.CanWrite);
        }

        /// <summary>
        /// Adds an <see cref="IEnumerable"/> XML converter to the list.
        /// </summary>
        /// <param name="converters">The list of XML converters.</param>
        public static void AddEnumerableConverter(this IList<XmlConverter> converters)
        {
            converters.AddXmlConverter<IEnumerable>((w, o, q) =>
            {
                w.WriteXmlRootElement(o, (writer, sequence, qe) =>
                {
                    var type = sequence.GetType();
                    var hasKeyValuePairType = type.GetGenericArguments().Any(gt => gt.IsKeyValuePair());
                
                    if (type.IsDictionary() || hasKeyValuePairType)
                    {
                        foreach (var element in sequence)
                        {
                            var elementType = element.GetType();
                            var keyProperty = elementType.GetProperty("Key");
                            var valueProperty = elementType.GetProperty("Value");
                            var keyValue = keyProperty.GetValue(element, null);
                            var valueValue = valueProperty.GetValue(element, null);
                            var valuePropertyType = valueProperty.PropertyType;
                            if (valuePropertyType == typeof(object) && valueValue != null) { valuePropertyType = valueValue.GetType(); }
                            writer.WriteStartElement("Item");
                            writer.WriteAttributeString("name", keyValue.ToString());
                            if (TypeUtility.IsComplex(valuePropertyType))
                            {
                                writer.WriteObject(valueValue, valuePropertyType);
                            }
                            else
                            {
                                writer.WriteValue(valueValue);
                            }
                            writer.WriteEndElement();
                        }
                    }
                    else
                    {
                        foreach (var item in sequence)
                        {
                            if (item == null) { continue; }
                            var itemType = item.GetType();
                            writer.WriteStartElement("Item");
                            if (TypeUtility.IsComplex(itemType))
                            {
                                writer.WriteObject(item, itemType);
                            }
                            else
                            {
                                writer.WriteValue(item);
                            }
                            writer.WriteEndElement();
                        }
                    }
                });
            }, (reader, type) => type.IsDictionary() ? reader.ToHierarchy().UseDictionary(type.GetGenericArguments()) : reader.ToHierarchy().UseCollection(type.GetGenericArguments().First()), type => type != typeof(string));
        }

        /// <summary>
        /// Adds an <see cref="ExceptionDescriptor"/> XML converter to the list.
        /// </summary>
        /// <param name="converters">The list of XML converters.</param>
        public static void AddExceptionDescriptorConverter(this IList<XmlConverter> converters)
        {
            converters.AddXmlConverter<ExceptionDescriptor>((writer, descriptor, qe) =>
            {
                writer.WriteStartElement("ExceptionDescriptor");
                writer.WriteElementString("Code", descriptor.Code);
                writer.WriteElementString("Message", descriptor.Message);
                writer.WriteStartElement("Failure");
                writer.WriteObject(descriptor.Failure);
                writer.WriteEndElement();
                if (descriptor.Evidence.Any())
                {
                    writer.WriteStartElement("Evidence");
                    foreach (var evidence in descriptor.Evidence)
                    {
                        writer.WriteObject(evidence.Value);
                    }
                    writer.WriteEndElement();
                }
                if (descriptor.HelpLink != null)
                {
                    writer.WriteElementString("HelpLink", descriptor.HelpLink.OriginalString);
                }
                if (!descriptor.RequestId.IsNullOrWhiteSpace()) { writer.WriteElementString("RequestId", descriptor.RequestId); }
                writer.WriteEndElement();
            });
        }

        /// <summary>
        /// Adds an <see cref="Exception" /> XML converter to the list.
        /// </summary>
        /// <param name="converters">The list of XML converters.</param>
        /// <param name="includeStackTrace">if set to <c>true</c> the stack of an exception is included.</param>
        public static void AddExceptionConverter(this IList<XmlConverter> converters, bool includeStackTrace)
        {
            AddExceptionConverter(converters, () => includeStackTrace);
        }

        internal static void AddExceptionConverter(this IList<XmlConverter> converters, Func<bool> includeStackTrace)
        {
            converters.AddXmlConverter<Exception>((writer, exception, qe) =>
            {
                XmlStreamConverter.WriteException(writer, exception, includeStackTrace?.Invoke() ?? false);
            });
        }

        /// <summary>
        /// Adds an <see cref="Uri"/> XML converter to the list.
        /// </summary>
        /// <param name="converters">The list of XML converters.</param>
        public static void AddUriConverter(this IList<XmlConverter> converters)
        {
            converters.AddXmlConverter(reader: (reader, type) => reader.ToHierarchy().UseUriFormatter());
        }

        /// <summary>
        /// Adds an <see cref="DateTime"/> XML converter to the list.
        /// </summary>
        /// <param name="converters">The list of XML converters.</param>
        public static void AddDateTimeConverter(this IList<XmlConverter> converters)
        {
            converters.AddXmlConverter(reader: (reader, type) => reader.ToHierarchy().UseDateTimeFormatter());
        }

        /// <summary>
        /// Adds an <see cref="TimeSpan"/> XML converter to the list.
        /// </summary>
        /// <param name="converters">The list of XML converters.</param>
        public static void AddTimeSpanConverter(this IList<XmlConverter> converters)
        {
            converters.AddXmlConverter(reader: (reader, type) => reader.ToHierarchy().UseTimeSpanFormatter());
        }

        /// <summary>
        /// Adds an <see cref="string"/> XML converter to the list.
        /// </summary>
        /// <param name="converters">The list of XML converters.</param>
        public static void AddStringConverter(this IList<XmlConverter> converters)
        {
            converters.AddXmlConverter<string>((w, s, q) =>
            {
                w.WriteEncapsulatingElementIfNotNull(s, q, (writer, value) =>
                {
                    if (value.IsXmlString())
                    {
                        writer.WriteCData(value);
                    }
                    else
                    {
                        writer.WriteValue(value);
                    }
                });
            });
        }
    }
}