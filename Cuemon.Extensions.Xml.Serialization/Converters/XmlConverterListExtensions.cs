using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Xml;
using Cuemon.Diagnostics;
using Cuemon.Extensions.Reflection;
using Cuemon.Extensions.Runtime.Serialization;
using Cuemon.Extensions.Xml.Linq;
using Cuemon.Reflection;
using Cuemon.Runtime.Serialization;
using Cuemon.Xml;
using Cuemon.Xml.Serialization;
using Cuemon.Xml.Serialization.Converters;

namespace Cuemon.Extensions.Xml.Serialization.Converters
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
        /// <param name="qe">The optional <seealso cref="XmlQualifiedEntity"/> that will provide the name of the root element.</param>
        public static void AddXmlConverter<T>(this IList<XmlConverter> converters, Action<XmlWriter, T, XmlQualifiedEntity> writer = null, Func<XmlReader, Type, T> reader = null, Func<Type, bool> canConvertPredicate = null, XmlQualifiedEntity qe = null)
        {
            converters.Add(DynamicXmlConverter.Create(writer, reader, canConvertPredicate, qe));
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
        /// <param name="qe">The optional <seealso cref="XmlQualifiedEntity"/> that will provide the name of the root element.</param>
        public static void InsertXmlConverter<T>(this IList<XmlConverter> converters, int index, Action<XmlWriter, T, XmlQualifiedEntity> writer = null, Func<XmlReader, Type, T> reader = null, Func<Type, bool> canConvertPredicate = null, XmlQualifiedEntity qe = null)
        {
            converters.Insert(index, DynamicXmlConverter.Create(writer, reader, canConvertPredicate, qe));
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
                            if (TypeInsight.FromType(valuePropertyType).IsComplex())
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
                            if (TypeInsight.FromType(itemType).IsComplex())
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
                }, q);
            }, (reader, type) => type.IsDictionary() ? reader.ToHierarchy().UseDictionary(type.GetGenericArguments()) : reader.ToHierarchy().UseCollection(type.GetGenericArguments().First()), type => type != typeof(string));
        }

        /// <summary>
        /// Adds an <see cref="ExceptionDescriptor"/> XML converter to the list.
        /// </summary>
        /// <param name="converters">The list of XML converters.</param>
        /// <param name="setup">The <see cref="ExceptionDescriptorSerializationOptions"/> which may be configured.</param>
        public static void AddExceptionDescriptorConverter(this IList<XmlConverter> converters, Action<ExceptionDescriptorSerializationOptions> setup = null)
        {
            var options = setup.Configure();
            converters.AddXmlConverter<ExceptionDescriptor>((writer, descriptor, qe) =>
            {
                writer.WriteStartElement("ExceptionDescriptor");
                writer.WriteStartElement("Error");
                writer.WriteElementString("Code", descriptor.Code);
                writer.WriteElementString("Message", descriptor.Message);
                if (descriptor.HelpLink != null) { writer.WriteElementString("HelpLink", descriptor.HelpLink.OriginalString); }
                if (options.IncludeFailure)
                {
                    writer.WriteStartElement("Failure");
                    writer.WriteObject(descriptor.Failure);
                    writer.WriteEndElement();
                }
                writer.WriteEndElement();
                if (options.IncludeEvidence && descriptor.Evidence.Any())
                {
                    writer.WriteStartElement("Evidence");
                    foreach (var evidence in descriptor.Evidence)
                    {
                        if (evidence.Value == null) { continue; }
                        writer.WriteObject(evidence.Value, evidence.Value.GetType(), o => o.RootName = new XmlQualifiedEntity(evidence.Key));
                    }
                    writer.WriteEndElement();
                }
                writer.WriteEndElement();
            }, canConvertPredicate: type => type == typeof(ExceptionDescriptor));
        }

        /// <summary>
        /// Adds an <see cref="Exception" /> XML converter to the list.
        /// </summary>
        /// <param name="converters">The list of XML converters.</param>
        /// <param name="includeStackTraceFactory">The function delegate that is invoked when it is needed to determine whether the stack of an exception is included in the converted result.</param>
        public static void AddExceptionConverter(this IList<XmlConverter> converters, Func<bool> includeStackTraceFactory)
        {
            converters.AddXmlConverter<Exception>((writer, exception, qe) =>
            {
                WriteException(writer, exception, includeStackTraceFactory?.Invoke() ?? false);
            });
        }

        /// <summary>
        /// Adds a <see cref="Uri"/> XML converter to the list.
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
            converters.AddXmlConverter((w, d, q) =>
            {
                w.WriteEncapsulatingElementIfNotNull(d, q, (writer, value) =>
                {
                    writer.WriteValue(value.ToString("u", CultureInfo.InvariantCulture));
                });
            },(reader, type) => reader.ToHierarchy().UseDateTimeFormatter());
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
                if (s.IsNullOrWhiteSpace()) { return; }
                if (w.WriteState == WriteState.Start && q == null) { q = new XmlQualifiedEntity(typeof(string).ToFriendlyName()); }
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

        private static void WriteException(XmlWriter writer, Exception exception, bool includeStackTrace)
        {
            var exceptionType = exception.GetType();
            writer.WriteStartElement(exceptionType.Name.SanitizeXmlElementName());
            writer.WriteAttributeString("namespace", exceptionType.Namespace);
            WriteExceptionCore(writer, exception, includeStackTrace);
            writer.WriteEndElement();
        }

        private static void WriteExceptionCore(XmlWriter writer, Exception exception, bool includeStackTrace)
        {
            if (!string.IsNullOrEmpty(exception.Source))
            {
                writer.WriteElementString("Source", exception.Source);
            }

            if (!string.IsNullOrEmpty(exception.Message))
            {
                writer.WriteElementString("Message", exception.Message);
            }

            if (exception.StackTrace != null && includeStackTrace)
            {
                writer.WriteStartElement("Stack");
                var lines = exception.StackTrace.Split(new[] { Alphanumeric.NewLine }, StringSplitOptions.RemoveEmptyEntries);
                foreach (var line in lines)
                {
                    writer.WriteElementString("Frame", line.Trim());
                }
                writer.WriteEndElement();
            }

            if (exception.Data.Count > 0)
            {
                writer.WriteStartElement("Data");
                foreach (DictionaryEntry entry in exception.Data)
                {
                    writer.WriteStartElement(entry.Key.ToString().SanitizeXmlElementName());
                    writer.WriteString(entry.Value.ToString().SanitizeXmlElementText());
                    writer.WriteEndElement();
                }
                writer.WriteEndElement();
            }

            var properties = exception.GetType().GetRuntimePropertiesExceptOf<AggregateException>().Where(pi => pi.PropertyType.IsSimple());
            foreach (var property in properties)
            {
                var value = property.GetValue(exception);
                if (value == null) { continue; }
                writer.WriteObject(value, value.GetType(), settings => settings.RootName = new XmlQualifiedEntity(property.Name));
            }

            WriteInnerExceptions(writer, exception, includeStackTrace);
        }

        private static void WriteInnerExceptions(XmlWriter writer, Exception exception, bool includeStackTrace)
        {
            var aggregated = exception as AggregateException;
            var innerExceptions = new List<Exception>();
            if (aggregated != null) { innerExceptions.AddRange(aggregated.Flatten().InnerExceptions); }
            if (exception.InnerException != null) { innerExceptions.Add(exception.InnerException); }
            if (innerExceptions.Count > 0)
            {
                var endElementsToWrite = 0;
                foreach (var inner in innerExceptions)
                {
                    var exceptionType = inner.GetType();
                    writer.WriteStartElement(exceptionType.Name.SanitizeXmlElementName());
                    writer.WriteAttributeString("namespace", exceptionType.Namespace);
                    WriteExceptionCore(writer, inner, includeStackTrace);
                    endElementsToWrite++;
                }
                for (var i = 0; i < endElementsToWrite; i++) { writer.WriteEndElement(); }
            }
        }
    }
}