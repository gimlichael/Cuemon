using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Cuemon.Extensions.YamlDotNet.Formatters;
using YamlDotNet.Core;

namespace Cuemon.Extensions.YamlDotNet.Converters
{
    /// <summary>
    /// Converts an <see cref="Exception"/> to YAML.
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
        /// <returns>The converted value.</returns>
        /// <exception cref="NotImplementedException"></exception>
        public override Exception ReadYaml(IParser reader, Type typeToConvert)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Writes a specified <paramref name="value" /> as YAML.
        /// </summary>
        /// <param name="writer">The writer to write to.</param>
        /// <param name="value">The value to convert to YAML.</param>
        public override void WriteYaml(IEmitter writer, Exception value)
        {
            var exceptionType = value.GetType();
            writer.WriteStartObject();
            writer.WriteString(SetPropertyName("Type"), exceptionType.FullName);
            WriteExceptionCore(writer, value, IncludeStackTrace, IncludeData, FormatterOptions);
            writer.WriteEndObject();
        }

        private static void WriteExceptionCore(IEmitter writer, Exception exception, bool includeStackTrace, bool includeData, YamlFormatterOptions formatterOptions)
        {
            if (!string.IsNullOrWhiteSpace(exception.Source))
            {
                writer.WriteString(formatterOptions.SetPropertyName("Source"), exception.Source);
            }

            if (!string.IsNullOrWhiteSpace(exception.Message))
            {
                writer.WriteString(formatterOptions.SetPropertyName("Message"), exception.Message);
            }

            if (exception.StackTrace != null && includeStackTrace)
            {
                writer.WritePropertyName(formatterOptions.SetPropertyName("Stack"));
                writer.WriteStartArray();
                var lines = exception.StackTrace.Split(new[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries);
                foreach (var line in lines)
                {
                    writer.WriteValue(line.Trim());
                }
                writer.WriteEndArray();
            }

            if (includeData && exception.Data.Count > 0)
            {
                writer.WritePropertyName(formatterOptions.SetPropertyName("Data"));
                writer.WriteStartObject();
                foreach (DictionaryEntry entry in exception.Data)
                {
                    writer.WritePropertyName(formatterOptions.SetPropertyName(entry.Key.ToString()));
                    writer.WriteObject(entry.Value, formatterOptions);
                }
                writer.WriteEndObject();
            }

            var properties = Decorator.Enclose(exception.GetType()).GetRuntimePropertiesExceptOf<AggregateException>().Where(pi => !Decorator.Enclose(pi.PropertyType).IsComplex());
            foreach (var property in properties)
            {
                var value = property.GetValue(exception);
                if (value == null) { continue; }
                writer.WritePropertyName(formatterOptions.SetPropertyName(property.Name));
                writer.WriteObject(value, formatterOptions);
            }

            WriteInnerExceptions(writer, exception, includeStackTrace, includeData, formatterOptions);
        }

        private static void WriteInnerExceptions(IEmitter writer, Exception exception, bool includeStackTrace, bool includeData, YamlFormatterOptions formatterOptions)
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
                    writer.WritePropertyName(formatterOptions.SetPropertyName("Inner"));
                    var exceptionType = inner.GetType();
                    writer.WriteStartObject();
                    writer.WriteString(formatterOptions.SetPropertyName("Type"), exceptionType.FullName);
                    WriteExceptionCore(writer, inner, includeStackTrace, includeData, formatterOptions);
                    endElementsToWrite++;
                }

                for (var i = 0; i < endElementsToWrite; i++) { writer.WriteEndObject(); }
            }
        }
    }
}
