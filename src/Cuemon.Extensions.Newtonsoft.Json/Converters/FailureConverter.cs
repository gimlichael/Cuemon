using System;
using System.Collections.Generic;
using System.Linq;
using Cuemon.Diagnostics;
using Newtonsoft.Json;

namespace Cuemon.Extensions.Newtonsoft.Json.Converters
{
    /// <summary>
    /// Converts a <see cref="Failure"/> object to JSON.
    /// </summary>
    public class FailureConverter : JsonConverter<Failure>
    {
        /// <summary>
        /// Writes the JSON representation of the <see cref="Failure"/> object.
        /// </summary>
        /// <param name="writer">The <see cref="JsonWriter"/> to write to.</param>
        /// <param name="value">The <see cref="Failure"/> object to write.</param>
        /// <param name="serializer">The calling serializer.</param>
        public override void WriteJson(JsonWriter writer, Failure value, JsonSerializer serializer)
        {
            writer.WriteStartObject();
            writer.WritePropertyName(nameof(value.Type), serializer);
            writer.WriteValue(value.Type);

            WriteException(writer, value, serializer);

            writer.WriteEndObject();
        }

        /// <summary>
        /// Reads the JSON representation of the <see cref="Failure"/> object.
        /// </summary>
        /// <param name="reader">The <see cref="JsonReader"/> to read from.</param>
        /// <param name="objectType">Type of the object.</param>
        /// <param name="existingValue">The existing value of object being read.</param>
        /// <param name="hasExistingValue">Whether the existing value has a value.</param>
        /// <param name="serializer">The calling serializer.</param>
        /// <returns>The <see cref="Failure"/> object.</returns>
        /// <exception cref="NotImplementedException"></exception>
        public override Failure ReadJson(JsonReader reader, Type objectType, Failure existingValue, bool hasExistingValue, JsonSerializer serializer)
        {
            throw new NotImplementedException();
        }

        private static void WriteException(JsonWriter writer, Failure value, JsonSerializer serializer)
        {
            if (!string.IsNullOrWhiteSpace(value.Source))
            {
                writer.WritePropertyName(nameof(value.Source), serializer);
                writer.WriteValue(value.Source);
            }

            if (!string.IsNullOrWhiteSpace(value.Message))
            {
                writer.WritePropertyName(nameof(value.Message), serializer);
                writer.WriteValue(value.Message);
            }

            if (value.Stack.Any())
            {
                writer.WritePropertyName(nameof(value.Stack), serializer);
                writer.WriteStartArray();
                foreach (var line in value.Stack)
                {
                    writer.WriteValue(line);
                }
                writer.WriteEndArray();
            }

            if (value.Data.Count > 0)
            {
                writer.WritePropertyName(nameof(value.Data), serializer);
                writer.WriteStartObject();
                foreach (var kvp in value.Data)
                {
                    writer.WritePropertyName(nameof(kvp.Key), serializer);
                    writer.WriteObject(kvp.Value, serializer);
                }
                writer.WriteEndObject();
            }

            foreach (var kvp in value)
            {
                writer.WritePropertyName(kvp.Key, serializer);
                writer.WriteObject(kvp.Value, serializer);
            }

            WriteInnerExceptions(writer, value, serializer);
        }

        private static void WriteInnerExceptions(JsonWriter writer, Failure value, JsonSerializer serializer)
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
                    writer.WritePropertyName("Inner", serializer);
                    writer.WriteStartObject();
                    writer.WritePropertyName(nameof(value.Type), serializer);
                    writer.WriteValue(innerValue.Type);
                    WriteException(writer, innerValue, serializer);
                    endElementsToWrite++;
                }

                for (var i = 0; i < endElementsToWrite; i++) { writer.WriteEndObject(); }
            }
        }

        /// <summary>
        /// Gets a value indicating whether this <see cref="T:Newtonsoft.Json.JsonConverter" /> can read JSON.
        /// </summary>
        /// <value><c>true</c> if this <see cref="T:Newtonsoft.Json.JsonConverter" /> can read JSON; otherwise, <c>false</c>.</value>
        public override bool CanRead => false;
    }
}
