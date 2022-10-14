using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Cuemon.Extensions.Text.Json.Converters
{
    /// <summary>
    /// Provides an <see cref="Exception"/> converter that can be configured like the Newtonsoft.JSON equivalent.
    /// </summary>
    /// <seealso cref="JsonConverter{Exception}" />
    public class ExceptionConverter : JsonConverter<Exception>
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
        /// Determines whether this instance can convert the specified object type.
        /// </summary>
        /// <param name="typeToConvert">Type of the object.</param>
        /// <returns><c>true</c> if this instance can convert the specified object type; otherwise, <c>false</c>.</returns>
        public override bool CanConvert(Type typeToConvert)
        {
            return typeof(Exception).IsAssignableFrom(typeToConvert);
        }

        /// <summary>
        /// Reads and converts the JSON to an <see cref="Exception"/>.
        /// </summary>
        /// <param name="reader">The reader.</param>
        /// <param name="typeToConvert">The type to convert.</param>
        /// <param name="options">An object that specifies serialization options to use.</param>
        /// <returns>The converted value.</returns>
        /// <exception cref="NotImplementedException"></exception>
        public override Exception Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Writes a specified value as JSON.
        /// </summary>
        /// <param name="writer">The writer to write to.</param>
        /// <param name="value">The value to convert to JSON.</param>
        /// <param name="options">An object that specifies serialization options to use.</param>
        public override void Write(Utf8JsonWriter writer, Exception value, JsonSerializerOptions options)
        {
            var exceptionType = value.GetType();
            writer.WriteStartObject();
            writer.WriteString(options.SetPropertyName("Type"), exceptionType.FullName);
            WriteExceptionCore(writer, value, IncludeStackTrace, options);
            writer.WriteEndObject();
        }

        private static void WriteExceptionCore(Utf8JsonWriter writer, Exception exception, bool includeStackTrace, JsonSerializerOptions options)
        {
            if (!string.IsNullOrWhiteSpace(exception.Source))
            {
                writer.WriteString(options.SetPropertyName("Source"), exception.Source);
            }

            if (!string.IsNullOrWhiteSpace(exception.Message))
            {
                writer.WriteString(options.SetPropertyName("Message"), exception.Message);
            }

            if (exception.StackTrace != null && includeStackTrace)
            {
                writer.WritePropertyName(options.SetPropertyName("Stack"));
                writer.WriteStartArray();
                var lines = exception.StackTrace.Split(new[] { Alphanumeric.NewLine }, StringSplitOptions.RemoveEmptyEntries);
                foreach (var line in lines)
                {
                    writer.WriteStringValue(line.Trim());
                }
                writer.WriteEndArray();
            }

            if (exception.Data.Count > 0)
            {
                writer.WritePropertyName(options.SetPropertyName("Data"));
                writer.WriteStartObject();
                foreach (DictionaryEntry entry in exception.Data)
                {
                    writer.WritePropertyName(options.SetPropertyName(entry.Key.ToString()));
                    writer.WriteObject(entry.Value, options);
                }
                writer.WriteEndObject();
            }

            var properties = Decorator.Enclose(exception.GetType()).GetRuntimePropertiesExceptOf<AggregateException>().Where(pi => !Decorator.Enclose(pi.PropertyType).IsComplex());
            foreach (var property in properties)
            {
                var value = property.GetValue(exception);
                if (value == null) { continue; }
                writer.WritePropertyName(options.SetPropertyName(property.Name));
                writer.WriteObject(value, options);
            }

            WriteInnerExceptions(writer, exception, includeStackTrace, options);
        }

        private static void WriteInnerExceptions(Utf8JsonWriter writer, Exception exception, bool includeStackTrace, JsonSerializerOptions options)
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
                    writer.WritePropertyName(options.SetPropertyName("Inner"));
                    var exceptionType = inner.GetType();
                    writer.WriteStartObject();
                    writer.WriteString(options.SetPropertyName("Type"), exceptionType.FullName);
                    WriteExceptionCore(writer, inner, includeStackTrace, options);
                    endElementsToWrite++;
                }

                for (var i = 0; i < endElementsToWrite; i++) { writer.WriteEndObject(); }
            }
        }
    }
}
