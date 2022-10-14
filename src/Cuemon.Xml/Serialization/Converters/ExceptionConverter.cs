using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Xml;

namespace Cuemon.Xml.Serialization.Converters
{
    /// <summary>
    /// Converts an <see cref="Exception"/> to XML.
    /// </summary>
    /// <seealso cref="XmlConverter" />
    public class ExceptionConverter : XmlConverter<Exception>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ExceptionConverter"/> class.
        /// </summary>
        /// <param name="includeStackTrace">A value that indicates if the stack of an exception is included in the converted result.</param>
        public ExceptionConverter(bool includeStackTrace = false)
        {
            IncludeStackTrace = includeStackTrace;
        }

        /// <summary>
        /// Gets a value indicating whether the stack of an exception is included in the converted result.
        /// </summary>
        /// <value><c>true</c> if the stack of an exception is included in the converted result; otherwise, <c>false</c>.</value>
        public bool IncludeStackTrace { get; }

        /// <summary>
        /// Writes the XML representation of the <paramref name="value" />.
        /// </summary>
        /// <param name="writer">The <see cref="T:System.Xml.XmlWriter" /> to write to.</param>
        /// <param name="value">The object to serialize.</param>
        /// <param name="elementName">The element name to encapsulate around <paramref name="value" />.</param>
        public override void WriteXml(XmlWriter writer, object value, XmlQualifiedEntity elementName = null)
        {
            var exceptionType = value.GetType();
            writer.WriteStartElement(Decorator.Enclose(exceptionType.Name).SanitizeXmlElementName());
            if (exceptionType.Namespace != null) { writer.WriteAttributeString("namespace", exceptionType.Namespace); }
            WriteExceptionCore(writer, (Exception)value, IncludeStackTrace);
            writer.WriteEndElement();
        }

        /// <summary>
        /// Reads the XML representation of the <paramref name="objectType" />.
        /// </summary>
        /// <param name="reader">The <see cref="T:System.Xml.XmlReader" /> to read from.</param>
        /// <param name="objectType">The <seealso cref="T:System.Type" /> of the object.</param>
        /// <returns>An object of <paramref name="objectType" />.</returns>
        /// <exception cref="NotImplementedException"></exception>
        public override object ReadXml(XmlReader reader, Type objectType)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Gets a value indicating whether this <seealso cref="XmlConverter" /> can read XML.
        /// </summary>
        /// <value><c>true</c> if this <seealso cref="XmlConverter" /> can read XML; otherwise, <c>false</c>.</value>
        public override bool CanRead => false;

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
                    writer.WriteStartElement(Decorator.Enclose(entry.Key.ToString()).SanitizeXmlElementName());
                    writer.WriteString(Decorator.Enclose(entry.Value?.ToString()).SanitizeXmlElementText());
                    writer.WriteEndElement();
                }
                writer.WriteEndElement();
            }

            var properties = Decorator.Enclose(exception.GetType()).GetRuntimePropertiesExceptOf<AggregateException>().Where(pi => !Decorator.Enclose(pi.PropertyType).IsComplex());
            foreach (var property in properties)
            {
                var value = property.GetValue(exception);
                if (value == null) { continue; }
                Decorator.Enclose(writer).WriteObject(value, value.GetType(), o => o.Settings.RootName = new XmlQualifiedEntity(property.Name));
            }

            WriteInnerExceptions(writer, exception, includeStackTrace);
        }

        private static void WriteInnerExceptions(XmlWriter writer, Exception exception, bool includeStackTrace)
        {
            var innerExceptions = new List<Exception>();
            if (exception is AggregateException aggregated)
            {
                innerExceptions.AddRange(aggregated.Flatten().InnerExceptions);
            }
            else
            {
                if (exception.InnerException != null) { innerExceptions.Add(exception.InnerException); }
            }
            if (innerExceptions.Count > 0)
            {
                var endElementsToWrite = 0;
                foreach (var inner in innerExceptions)
                {
                    var exceptionType = inner.GetType();
                    writer.WriteStartElement(Decorator.Enclose(exceptionType.Name).SanitizeXmlElementName());
                    if (exceptionType.Namespace != null) { writer.WriteAttributeString("namespace", exceptionType.Namespace); }
                    WriteExceptionCore(writer, inner, includeStackTrace);
                    endElementsToWrite++;
                }
                for (var i = 0; i < endElementsToWrite; i++) { writer.WriteEndElement(); }
            }
        }
    }
}
