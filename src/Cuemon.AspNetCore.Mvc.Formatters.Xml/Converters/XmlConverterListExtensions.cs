using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using Cuemon.AspNetCore.Http;
using Cuemon.Diagnostics;
using Cuemon.Extensions.Xml.Serialization;
using Cuemon.Extensions.Xml.Serialization.Converters;
using Cuemon.Runtime.Serialization;
using Cuemon.Xml.Serialization;
using Cuemon.Xml.Serialization.Converters;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Primitives;

namespace Cuemon.Extensions.AspNetCore.Mvc.Formatters.Xml.Converters
{
    /// <summary>
    /// Extension methods for the <see cref=" IList{XmlConverter}"/>.
    /// </summary>
    public static class XmlConverterListExtensions
    {
        static XmlConverterListExtensions()
        {
            XmlSerializerOptions.DefaultConverters += list =>
            {
                list.AddHttpExceptionDescriptorConverter();
                list.AddStringValuesConverter();
                list.AddHeaderDictionaryConverter();
                list.AddFormCollectionConverter();
                list.AddQueryCollectionConverter();
                list.AddCookieCollectionConverter();
            };
        }

        /// <summary>
        /// Adds an <see cref="ExceptionDescriptor"/> XML converter to the list.
        /// </summary>
        /// <param name="converters">The list of XML converters.</param>
        /// <param name="setup">The <see cref="ExceptionDescriptorSerializationOptions"/> which need to be configured.</param>
        public static void AddHttpExceptionDescriptorConverter(this IList<XmlConverter> converters, Action<ExceptionDescriptorSerializationOptions> setup = null)
        {
            var options = Patterns.Configure(setup);
            converters.AddXmlConverter<HttpExceptionDescriptor>((writer, descriptor, qe) =>
            {
                writer.WriteStartElement("HttpExceptionDescriptor");
                writer.WriteStartElement("Error");
                writer.WriteElementString("Status", descriptor.StatusCode.ToString(CultureInfo.InvariantCulture));
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
                if (!string.IsNullOrWhiteSpace(descriptor.CorrelationId)) { writer.WriteElementString("CorrelationId", descriptor.CorrelationId); }
                if (!string.IsNullOrWhiteSpace(descriptor.RequestId)) { writer.WriteElementString("RequestId", descriptor.RequestId); }
                writer.WriteEndElement();
            });
        }

        /// <summary>
        /// Adds an <see cref="StringValues"/> XML converter to the list.
        /// </summary>
        /// <param name="converters">The list of XML converters.</param>
        public static void AddStringValuesConverter(this IList<XmlConverter> converters)
        {
            converters.InsertXmlConverter<StringValues>(0, (writer, values, qe) =>
            {
                if (values.Count <= 1)
                {
                    writer.WriteValue(values.ToString());
                }
                else
                {
                    foreach (var value in values) { writer.WriteElementString("Value", value); }
                }
            });
        }

        /// <summary>
        /// Adds an <see cref="IHeaderDictionary"/> XML converter to the list.
        /// </summary>
        /// <param name="converters">The list of XML converters.</param>
        public static void AddHeaderDictionaryConverter(this IList<XmlConverter> converters)
        {
            converters.InsertXmlConverter<IHeaderDictionary>(0, (writer, headers, qe) =>
            {
                foreach (var header in headers)
                {
                    writer.WriteStartElement("Header");
                    writer.WriteAttributeString("name", header.Key);
                    writer.WriteObject(header.Value);
                    writer.WriteEndElement();
                }
            });
        }

        /// <summary>
        /// Adds an <see cref="IQueryCollection"/> XML converter to the list.
        /// </summary>
        /// <param name="converters">The list of XML converters.</param>
        public static void AddQueryCollectionConverter(this IList<XmlConverter> converters)
        {
            converters.InsertXmlConverter<IQueryCollection>(0, (writer, collection, qe) =>
            {
                foreach (var pair in collection)
                {
                    writer.WriteStartElement("Field");
                    writer.WriteAttributeString("key", pair.Key);
                    writer.WriteObject(pair.Value);
                    writer.WriteEndElement();
                }
            });
        }

        /// <summary>
        /// Adds an <see cref="IFormCollection"/> XML converter to the list.
        /// </summary>
        /// <param name="converters">The list of XML converters.</param>
        public static void AddFormCollectionConverter(this IList<XmlConverter> converters)
        {
            converters.InsertXmlConverter<IFormCollection>(0, (writer, collection, qe) =>
            {
                foreach (var pair in collection)
                {
                    writer.WriteStartElement("Field");
                    writer.WriteAttributeString("key", pair.Key);
                    writer.WriteObject(pair.Value);
                    writer.WriteEndElement();
                }
            });
        }
        
        /// <summary>
        /// Adds an <see cref="IRequestCookieCollection"/> XML converter to the list.
        /// </summary>
        /// <param name="converters">The list of XML converters.</param>
        public static void AddCookieCollectionConverter(this IList<XmlConverter> converters)
        {
            converters.InsertXmlConverter<IRequestCookieCollection>(0, (writer, collection, qe) =>
            {
                foreach (var pair in collection)
                {
                    writer.WriteStartElement("Field");
                    writer.WriteAttributeString("key", pair.Key);
                    writer.WriteValue(pair.Value);
                    writer.WriteEndElement();
                }
            });
        }
    }
}