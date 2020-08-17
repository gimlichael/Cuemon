using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using Cuemon.AspNetCore.Http;
using Cuemon.Diagnostics;
using Cuemon.Extensions.Xml;
using Cuemon.Extensions.Xml.Serialization.Converters;
using Cuemon.Runtime.Serialization;
using Cuemon.Xml.Serialization;
using Cuemon.Xml.Serialization.Converters;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Primitives;

namespace Cuemon.Extensions.AspNetCore.Mvc.Formatters.Xml.Converters
{
    /// <summary>
    /// Extension methods for the <see cref=" XmlConverter"/> class.
    /// </summary>
    public static class XmlConverterExtensions
    {
        static XmlConverterExtensions()
        {
            XmlSerializerOptions.DefaultConverters += list =>
            {
                list.AddHttpExceptionDescriptorConverter()
                    .AddStringValuesConverter()
                    .AddHeaderDictionaryConverter()
                    .AddFormCollectionConverter()
                    .AddQueryCollectionConverter()
                    .AddCookieCollectionConverter();
            };
        }

        /// <summary>
        /// Adds an <see cref="ExceptionDescriptor"/> XML converter to the list.
        /// </summary>
        /// <param name="converters">The <see cref="T:IList{XmlConverter}" /> to extend.</param>
        /// <param name="setup">The <see cref="ExceptionDescriptorSerializationOptions"/> which need to be configured.</param>
        /// <returns>A reference to <paramref name="converters"/> after the operation has completed.</returns>
        public static IList<XmlConverter> AddHttpExceptionDescriptorConverter(this IList<XmlConverter> converters, Action<ExceptionDescriptorSerializationOptions> setup = null)
        {
            var options = Patterns.Configure(setup);
            return converters.AddXmlConverter<HttpExceptionDescriptor>((writer, descriptor, qe) =>
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
        /// <param name="converters">The <see cref="T:IList{XmlConverter}" /> to extend.</param>
        /// <returns>A reference to <paramref name="converters"/> after the operation has completed.</returns>
        public static IList<XmlConverter> AddStringValuesConverter(this IList<XmlConverter> converters)
        {
            return converters.InsertXmlConverter<StringValues>(0, (writer, values, qe) =>
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
        /// <returns>A reference to <paramref name="converters"/> after the operation has completed.</returns>
        public static IList<XmlConverter> AddHeaderDictionaryConverter(this IList<XmlConverter> converters)
        {
            return converters.InsertXmlConverter<IHeaderDictionary>(0, (writer, headers, qe) =>
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
        /// <param name="converters">The <see cref="T:IList{XmlConverter}" /> to extend.</param>
        /// <returns>A reference to <paramref name="converters"/> after the operation has completed.</returns>
        public static IList<XmlConverter> AddQueryCollectionConverter(this IList<XmlConverter> converters)
        {
            return converters.InsertXmlConverter<IQueryCollection>(0, (writer, collection, qe) =>
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
        /// <param name="converters">The <see cref="T:IList{XmlConverter}" /> to extend.</param>
        /// <returns>A reference to <paramref name="converters"/> after the operation has completed.</returns>
        public static IList<XmlConverter> AddFormCollectionConverter(this IList<XmlConverter> converters)
        {
            return converters.InsertXmlConverter<IFormCollection>(0, (writer, collection, qe) =>
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
        /// <param name="converters">The <see cref="T:IList{XmlConverter}" /> to extend.</param>
        /// <returns>A reference to <paramref name="converters"/> after the operation has completed.</returns>
        public static IList<XmlConverter> AddCookieCollectionConverter(this IList<XmlConverter> converters)
        {
            return converters.InsertXmlConverter<IRequestCookieCollection>(0, (writer, collection, qe) =>
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