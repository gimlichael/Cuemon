using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Text.Json.Serialization;
using Cuemon.Diagnostics;

namespace Cuemon.Extensions.Text.Json.Converters
{
    /// <summary>
    /// Converts a <see cref="Failure"/> object to JSON.
    /// </summary>
    public class FailureConverter : JsonConverter<Failure>
    {
        /// <summary>
        /// Reads and converts the JSON to type <see cref="Failure"/>.
        /// </summary>
        /// <param name="reader">The reader.</param>
        /// <param name="typeToConvert">The type to convert.</param>
        /// <param name="options">An object that specifies serialization options to use.</param>
        /// <returns>The converted value.</returns>
        /// <exception cref="NotImplementedException"></exception>
        public override Failure Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Writes a specified value as JSON.
        /// </summary>
        /// <param name="writer">The writer to write to.</param>
        /// <param name="value">The value to convert to JSON.</param>
        /// <param name="options">An object that specifies serialization options to use.</param>
        public override void Write(Utf8JsonWriter writer, Failure value, JsonSerializerOptions options)
        {
            writer.WriteStartObject();
            writer.WriteString(options.PropertyNamingPolicy.DefaultOrConvertName(nameof(value.Type)), value.Type);

            WriteException(writer, value, options);

            writer.WriteEndObject();
        }

        private static void WriteException(Utf8JsonWriter writer, Failure value, JsonSerializerOptions options)
        {
            if (!string.IsNullOrWhiteSpace(value.Source)) { writer.WriteString(options.SetPropertyName(nameof(value.Source)), value.Source); }
            if (!string.IsNullOrWhiteSpace(value.Message)) { writer.WriteString(options.SetPropertyName(nameof(value.Message)), value.Message); }

            if (value.Stack.Any())
            {
                writer.WritePropertyName(options.SetPropertyName(nameof(value.Stack)));
                writer.WriteStartArray();
                foreach (var line in value.Stack)
                {
                    writer.WriteStringValue(line);
                }
                writer.WriteEndArray();
            }

            if (value.Data.Count > 0)
            {
                writer.WritePropertyName(options.SetPropertyName(nameof(value.Data)));
                writer.WriteStartObject();
                foreach (var kvp in value.Data)
                {
                    writer.WritePropertyName(options.SetPropertyName(nameof(kvp.Key)));
                    writer.WriteObject(kvp.Value, options);
                }
                writer.WriteEndObject();
            }

            foreach (var kvp in value)
            {
                writer.WritePropertyName(options.SetPropertyName(kvp.Key));
                writer.WriteObject(kvp.Value, options);
            }

            WriteInnerExceptions(writer, value, options);
        }

        private static void WriteInnerExceptions(Utf8JsonWriter writer, Failure value, JsonSerializerOptions options)
        {
            var exception = value.GetUnderlyingException();
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
                    var innerValue = new Failure(inner, value.GetUnderlyingSensitivity());
                    writer.WritePropertyName(options.SetPropertyName("Inner"));
                    writer.WriteStartObject();
                    writer.WriteString(options.SetPropertyName(nameof(value.Type)), innerValue.Type);
                    WriteException(writer, innerValue, options);
                    endElementsToWrite++;
                }

                for (var i = 0; i < endElementsToWrite; i++) { writer.WriteEndObject(); }
            }
        }
    }
}
