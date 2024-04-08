using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Cuemon.Reflection;
using Cuemon.Runtime.Serialization.Formatters;
using Newtonsoft.Json;

namespace Cuemon.Extensions.Newtonsoft.Json.Converters
{
    /// <summary>
    /// Converts an <see cref="Exception"/> to or from JSON.
    /// </summary>
    /// <seealso cref="JsonConverter" />
    public class ExceptionConverter : JsonConverter
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
        /// Writes the JSON representation of the object.
        /// </summary>
        /// <param name="writer">The <see cref="T:Newtonsoft.Json.JsonWriter" /> to write to.</param>
        /// <param name="value">The value.</param>
        /// <param name="serializer">The calling serializer.</param>
        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            WriteException(writer, (Exception)value, IncludeStackTrace, IncludeData, serializer);
        }

        /// <summary>
        /// Reads the JSON representation of the object.
        /// </summary>
        /// <param name="reader">The <see cref="T:Newtonsoft.Json.JsonReader" /> to read from.</param>
        /// <param name="objectType">Type of the object.</param>
        /// <param name="existingValue">The existing value of object being read.</param>
        /// <param name="serializer">The calling serializer.</param>
        /// <returns>The object value.</returns>
        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            var stack = ParseJsonReader(reader, objectType);
            return Decorator.Enclose(stack).CreateException();
        }

        private static Stack<IList<MemberArgument>> ParseJsonReader(JsonReader reader, Type objectType)
        {
            var stack = new Stack<IList<MemberArgument>>();
            var properties = new List<PropertyInfo>();
            var lastDepth = 1;
            var blueprints = new List<MemberArgument>();
            while (reader.Read())
            {
                if (reader.Depth != lastDepth && blueprints.Count > 0)
                {
                    stack.Push(blueprints);
                    blueprints = new List<MemberArgument>();
                }

                switch (reader.TokenType)
                {
                    case JsonToken.PropertyName:
                        string memberName = MapOrDefault(reader.Value!.ToString()!);
                        if (!reader.Read())
                        {
                            // throw
                        }
                        var property = properties.SingleOrDefault(pi => pi.Name.Equals(memberName, StringComparison.OrdinalIgnoreCase));
                        if (property != null)
                        {
                            if (property.Name == nameof(Exception.InnerException))
                            {
                                blueprints.Add(new MemberArgument(memberName, null));
                            }
                            else
                            {
                                blueprints.Add(new MemberArgument(memberName, reader.Value));
                            }
                        }
                        else
                        {
                            if (memberName.Equals("type", StringComparison.OrdinalIgnoreCase))
                            {
                                objectType = Formatter.GetType(reader.Value.ToString());
                                properties = objectType.GetProperties(MemberReflection.CreateFlags(o => o.ExcludeStatic = true)).ToList();
                                blueprints.Add(new MemberArgument(memberName, objectType));
                            }
                        }
                        break;
                    case JsonToken.Comment:
                        break;
                    case JsonToken.EndObject:
                        break;
                }
                lastDepth = reader.Depth;
            }

            return stack;
        }

        private static string MapOrDefault(string memberName)
        {
            switch (memberName.ToLowerInvariant())
            {
                case "inner":
                    return nameof(Exception.InnerException);
                case "stack":
                    return nameof(Exception.StackTrace);
                default:
                    return memberName;
            }
        }

        /// <summary>
        /// Determines whether this instance can convert the specified object type.
        /// </summary>
        /// <param name="objectType">Type of the object.</param>
        /// <returns><c>true</c> if this instance can convert the specified object type; otherwise, <c>false</c>.</returns>
        public override bool CanConvert(Type objectType)
        {
            return typeof(Exception).IsAssignableFrom(objectType);
        }

        private static void WriteException(JsonWriter writer, Exception exception, bool includeStackTrace, bool includeData, JsonSerializer serializer)
        {
            var exceptionType = exception.GetType();
            writer.WriteStartObject();
            writer.WritePropertyName("Type", serializer);
            writer.WriteValue(exceptionType.FullName);
            WriteExceptionCore(writer, exception, includeStackTrace, includeData, serializer);
            writer.WriteEndObject();
        }

        private static void WriteExceptionCore(JsonWriter writer, Exception exception, bool includeStackTrace, bool includeData, JsonSerializer serializer)
        {
            if (!string.IsNullOrWhiteSpace(exception.Source))
            {
                writer.WritePropertyName("Source", serializer);
                writer.WriteValue(exception.Source);
            }

            if (!string.IsNullOrWhiteSpace(exception.Message))
            {
                writer.WritePropertyName("Message", serializer);
                writer.WriteValue(exception.Message);
            }

            if (exception.StackTrace != null && includeStackTrace)
            {
                writer.WritePropertyName("Stack", serializer);
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
                writer.WritePropertyName("Data", serializer);
                writer.WriteStartObject();
                foreach (DictionaryEntry entry in exception.Data)
                {
                    writer.WritePropertyName(entry.Key.ToString()!);
                    writer.WriteObject(entry.Value, serializer);
                }
                writer.WriteEndObject();
            }

            var properties = Decorator.Enclose(exception.GetType()).GetRuntimePropertiesExceptOf<AggregateException>();
            foreach (var property in properties)
            {
                var value = property.GetValue(exception);
                if (value == null) { continue; }
                writer.WritePropertyName(property.Name, serializer);
                writer.WriteObject(value, serializer);
            }

            WriteInnerExceptions(writer, exception, includeStackTrace, includeData, serializer);
        }

        private static void WriteInnerExceptions(JsonWriter writer, Exception exception, bool includeStackTrace, bool includeData, JsonSerializer serializer)
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
                    writer.WritePropertyName("Inner", serializer);
                    var exceptionType = inner.GetType();
                    writer.WriteStartObject();
                    writer.WritePropertyName("Type", serializer);
                    writer.WriteValue(exceptionType.FullName);
                    WriteExceptionCore(writer, inner, includeStackTrace, includeData, serializer);
                    endElementsToWrite++;
                }

                for (var i = 0; i < endElementsToWrite; i++) { writer.WriteEndObject(); }
            }
        }
    }
}
