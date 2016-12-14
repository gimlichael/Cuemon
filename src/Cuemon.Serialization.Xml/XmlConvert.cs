using System;
using System.IO;
using Cuemon.Xml;

namespace Cuemon.Serialization.Xml
{
    /// <summary>
    /// Provides methods to make XML serialization operations easier to work with.
    /// </summary>
    /// <seealso cref="XmlConverter"/>
    public static class XmlConvert
    {
        /// <summary>
        /// Serializes the specified <paramref name="value"/> to a <see cref="Stream" />.
        /// </summary>
        /// <param name="value">The object to serialize to XML format.</param>
        /// <param name="serializer">The object that will handle the serialization.</param>
        /// <returns>A stream of the serialized object.</returns>
        public static Stream SerializeObject(object value, XmlConverter serializer)
        {
            return XmlWriterUtility.CreateXml(writer =>
            {
                serializer.WriteXml(writer, value);
            }, settings =>
            {
                settings.Encoding = serializer.Options.WriterSettings.Encoding;
                settings.OmitXmlDeclaration = serializer.Options.WriterSettings.OmitXmlDeclaration;
                settings.CheckCharacters = serializer.Options.WriterSettings.CheckCharacters;
                settings.CloseOutput = serializer.Options.WriterSettings.CloseOutput;
                settings.ConformanceLevel = serializer.Options.WriterSettings.ConformanceLevel;
                settings.Indent = serializer.Options.WriterSettings.Indent;
                settings.IndentChars = serializer.Options.WriterSettings.IndentChars;
                settings.NamespaceHandling = serializer.Options.WriterSettings.NamespaceHandling;
                settings.NewLineChars = serializer.Options.WriterSettings.NewLineChars;
                settings.NewLineHandling = serializer.Options.WriterSettings.NewLineHandling;
                settings.NewLineOnAttributes = serializer.Options.WriterSettings.NewLineOnAttributes;
                settings.WriteEndDocumentOnClose = serializer.Options.WriterSettings.WriteEndDocumentOnClose;
                settings.Async = serializer.Options.WriterSettings.Async;
            });
        }

        /// <summary>
        /// Deserializes the specified <paramref name="value"/> into an object of <typeparamref name="T"/>.
        /// </summary>
        /// <typeparam name="T">The type of the object to deserialize.</typeparam>
        /// <param name="value">The object to deserialize from XML format.</param>
        /// <param name="serializer">The object that will handle the deserialization.</param>
        /// <returns>An object of <typeparamref name="T"/>.</returns>
        public static T DeserializeObject<T>(Stream value, XmlConverter serializer)
        {
            return (T)DeserializeObject(value, typeof(T), serializer);
        }

        /// <summary>
        /// Deserializes the specified <paramref name="value" /> into an object of <paramref name="valueType"/>.
        /// </summary>
        /// <param name="value">The object to deserialize from XML format.</param>
        /// <param name="valueType">The type of the object to deserialize.</param>
        /// <param name="serializer">The object that will handle the deserialization.</param>
        /// <returns>An object of <paramref name="valueType"/>.</returns>
        public static object DeserializeObject(Stream value, Type valueType, XmlConverter serializer)
        {
            using (var reader = XmlReaderConverter.FromStream(value, null, settings =>
            {
                settings.ConformanceLevel = serializer.Options.ReaderSettings.ConformanceLevel;
                settings.IgnoreComments = serializer.Options.ReaderSettings.IgnoreComments;
                settings.IgnoreProcessingInstructions = serializer.Options.ReaderSettings.IgnoreProcessingInstructions;
                settings.IgnoreWhitespace = serializer.Options.ReaderSettings.IgnoreWhitespace;
                settings.LineNumberOffset = serializer.Options.ReaderSettings.LineNumberOffset;
                settings.LinePositionOffset = serializer.Options.ReaderSettings.LinePositionOffset;
                settings.MaxCharactersFromEntities = serializer.Options.ReaderSettings.MaxCharactersFromEntities;
                settings.MaxCharactersInDocument = serializer.Options.ReaderSettings.MaxCharactersInDocument;
                settings.NameTable = serializer.Options.ReaderSettings.NameTable;
                settings.Async = serializer.Options.ReaderSettings.Async;
                settings.DtdProcessing = serializer.Options.ReaderSettings.DtdProcessing;
                settings.CloseInput = serializer.Options.ReaderSettings.CloseInput;
                settings.CheckCharacters = serializer.Options.ReaderSettings.CheckCharacters;
            }))
            {
                return serializer.ReadXml(reader, valueType);
            }
        }
    }
}