using System.Collections.Generic;
using Cuemon.Serialization.Xml;
using Cuemon.Serialization.Xml.Converters;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Primitives;

namespace Cuemon.AspNetCore.Mvc.Formatters.Xml.Converters
{
    /// <summary>
    /// Extension methods for the <see cref=" IList{XmlConverter}"/>.
    /// </summary>
    public static class XmlConverterListExtensions
    {
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