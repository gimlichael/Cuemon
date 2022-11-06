using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Cuemon.Runtime.Serialization;

namespace Cuemon.Text.Yaml.Converters
{
    /// <summary>
    /// Converts an <see cref="Exception"/> to or from YAML.
    /// </summary>
    /// <seealso cref="YamlConverter{Exception}" />
    public class ExceptionConverter : YamlConverter<Exception>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ExceptionConverter"/> class.
        /// </summary>
        /// <param name="includeStackTrace">A value that indicates if the stack of an exception is included in the converted result.</param>
        /// <param name="includeData">A value that indicates if the data of an exception is included in the converted result.</param>
        public ExceptionConverter(bool includeStackTrace = false, bool includeData = false)
        {
            IncludeStackTrace = includeStackTrace;
            IncludeData = includeData;
        }

        /// <summary>
        /// Gets a value indicating whether the data of an exception is included in the converted result.
        /// </summary>
        /// <value><c>true</c> if the data of an exception is included in the converted result; otherwise, <c>false</c>.</value>
        public bool IncludeData { get; }

        /// <summary>
        /// Gets a value indicating whether the stack of an exception is included in the converted result.
        /// </summary>
        /// <value><c>true</c> if the stack of an exception is included in the converted result; otherwise, <c>false</c>.</value>
        public bool IncludeStackTrace { get; }

        /// <summary>
        /// Determines whether this instance can convert the specified object type.
        /// </summary>
        /// <param name="typeToConvert">Type of the object.</param>
        /// <returns><c>true</c> if this instance can convert the specified object type; otherwise, <c>false</c>.</returns>
        public override bool CanConvert(Type typeToConvert)
        {
            return typeof(Exception).IsAssignableFrom(typeToConvert);
        }

        /// <summary>
        /// Reads and converts the YAML to <see cref="Exception"/>.
        /// </summary>
        /// <param name="reader">The reader to read from.</param>
        /// <param name="typeToConvert">The type to convert.</param>
        /// <param name="so">An object that specifies serialization options to use.</param>
        /// <returns>The converted value.</returns>
        /// <exception cref="NotImplementedException"></exception>
        public override Exception ReadYaml(YamlTextReader reader, Type typeToConvert, YamlSerializerOptions so)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Writes a specified <paramref name="value" /> as YAML.
        /// </summary>
        /// <param name="writer">The writer to write to.</param>
        /// <param name="value">The value to convert to YAML.</param>
        /// <param name="so">An object that specifies serialization options to use.</param>
        public override void WriteYaml(YamlTextWriter writer, Exception value, YamlSerializerOptions so)
        {
            var exceptionType = value.GetType();
            writer.WriteStartObject();
            writer.WriteString(so.SetPropertyName("Type"), exceptionType.FullName);
            WriteExceptionCore(writer, value, IncludeStackTrace, IncludeData, so);
            writer.WriteEndObject();
        }

        private static void WriteExceptionCore(YamlTextWriter writer, Exception exception, bool includeStackTrace, bool includeData, YamlSerializerOptions so)
        {
            if (!string.IsNullOrWhiteSpace(exception.Source))
            {
                writer.WriteString(so.SetPropertyName("Source"), exception.Source);
            }

            if (!string.IsNullOrWhiteSpace(exception.Message))
            {
                writer.WriteString(so.SetPropertyName("Message"), exception.Message);
            }

            if (exception.StackTrace != null && includeStackTrace)
            {
                writer.WritePropertyName(so.SetPropertyName("Stack"));
                writer.WriteStartArray();
                var lines = exception.StackTrace.Split(new[] { Alphanumeric.NewLine }, StringSplitOptions.RemoveEmptyEntries);
                foreach (var line in lines)
                {
                    writer.WriteLine(line.Trim());
                }
                writer.WriteEndArray();
            }

            if (includeData && exception.Data.Count > 0)
            {
                writer.WritePropertyName(so.SetPropertyName("Data"));
                writer.WriteStartObject();
                foreach (DictionaryEntry entry in exception.Data)
                {
                    writer.WritePropertyName(so.SetPropertyName(entry.Key.ToString()));
                    writer.WriteObject(entry.Value, so);
                }
                writer.WriteEndObject();
            }

            var properties = Decorator.Enclose(exception.GetType()).GetRuntimePropertiesExceptOf<AggregateException>().Where(pi => !Decorator.Enclose(pi.PropertyType).IsComplex());
            foreach (var property in properties)
            {
                var value = property.GetValue(exception);
                if (value == null) { continue; }
                writer.WritePropertyName(so.SetPropertyName(property.Name));
                writer.WriteObject(value, so);
            }

            WriteInnerExceptions(writer, exception, includeStackTrace, includeData, so);
        }

        private static void WriteInnerExceptions(YamlTextWriter writer, Exception exception, bool includeStackTrace, bool includeData, YamlSerializerOptions so)
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
                    writer.WritePropertyName(so.SetPropertyName("Inner"));
                    var exceptionType = inner.GetType();
                    writer.WriteStartObject();
                    writer.WriteString(so.SetPropertyName("Type"), exceptionType.FullName);
                    WriteExceptionCore(writer, inner, includeStackTrace, includeData, so);
                    endElementsToWrite++;
                }

                for (var i = 0; i < endElementsToWrite; i++) { writer.WriteEndObject(); }
            }
        }
    }
}
