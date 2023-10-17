using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection;
using Cuemon.Reflection;
using Cuemon.Runtime.Serialization.Formatters;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

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
            var stack = new Stack<Dictionary<string, object>>();
            var properties = new List<PropertyInfo>();
            var lastDepth = 1;
            var blueprints = new Dictionary<string, object>(StringComparer.OrdinalIgnoreCase);
            while (reader.Read())
            {
                if (reader.Depth != lastDepth && blueprints.Count > 0)
                {
                    stack.Push(blueprints);
                    blueprints = new Dictionary<string, object>(StringComparer.OrdinalIgnoreCase);
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
                                blueprints.Add(memberName, null);
                            }
                            else
                            {
                                blueprints.Add(memberName, reader.Value);
                            }
                        }
                        else
                        {
                            if (memberName.Equals("type", StringComparison.OrdinalIgnoreCase))
                            {
                                objectType = Formatter.GetType(reader.Value.ToString());
                                properties = objectType.GetProperties(MemberReflection.CreateFlags(o => o.ExcludeStatic = true)).ToList();
                                blueprints.Add(memberName, objectType);
                            }
                        }
                        break;
                    case JsonToken.Comment:
                        break;
                    case JsonToken.EndObject:
                        break;
                    default:
                        break;
                        // throw
                }
                lastDepth = reader.Depth;
            }

            Exception instance = null;

            while (stack.Count > 0)
            {
                var blueprint = stack.Pop();
                var desiredType = blueprint["type"] as Type;
                blueprint.Remove("type");

                if (typeof(ArgumentException).IsAssignableFrom(objectType) && blueprint.ContainsKey("message"))
                {
#if NETSTANDARD2_0_OR_GREATER
                    blueprint["message"] = ((string)blueprint["message"]).Replace($"{Environment.NewLine}Parameter name: {blueprint["paramName"]}", "");
#else
                    blueprint["message"] = ((string)blueprint["message"]).Replace($" (Parameter '{blueprint["paramName"]}')", "");
#endif
                }

                if (typeof(ArgumentOutOfRangeException).IsAssignableFrom(objectType) && blueprint.ContainsKey("message"))
                {
                    blueprint["message"] = ((string)blueprint["message"]).Replace($"{Environment.NewLine}Actual value was {blueprint["actualValue"]}.", "");
                }

                if (blueprint.ContainsKey(nameof(Exception.InnerException)))
                {
                    blueprint[nameof(Exception.InnerException)] = instance;
                }

                var exceptionBaseCompatibleCtors = desiredType!.GetConstructors(MemberReflection.CreateFlags(o => o.ExcludeStatic = true)).Where(ci => ci.GetParameters().Any(pi => pi.ParameterType.IsAssignableFrom(typeof(Exception)) ||
                                                                                                                                                   pi.ParameterType.IsAssignableFrom(typeof(string)))).Reverse().ToList();

                var args = new List<object>();
                foreach (var ctor in exceptionBaseCompatibleCtors) // 1:1 match with constructor
                {
                    var ctorArgs = ctor.GetParameters();
                    var blueprintMatchLength = ctorArgs.Select(info => info.Name).Intersect(blueprint.Select(pair => pair.Key), StringComparer.OrdinalIgnoreCase).Count(); 
                    if (ctorArgs.Length == blueprintMatchLength)
                    {
                        foreach (var arg in ctorArgs)
                        {
                            var kvp = blueprint.First(pair => pair.Key.Equals(arg.Name, StringComparison.OrdinalIgnoreCase));
                            args.Add(Decorator.Enclose(arg.ParameterType).IsComplex() 
                                ? kvp.Value
                                : Decorator.Enclose(kvp.Value).ChangeType(arg.ParameterType));
                            blueprint.Remove(arg.Name);
                        }
                        break;
                    }
                }

                if (args.Count == 0)
                {
                    foreach (var ctor in exceptionBaseCompatibleCtors) // partial match with constructor
                    {
                        var ctorArgs = ctor.GetParameters();
                        var blueprintMatchLength = ctorArgs.Select(info => info.Name).Count(ctorArgName => blueprint.Keys.Any(key => ctorArgName.EndsWith(key, StringComparison.OrdinalIgnoreCase)));
                        if (ctorArgs.Length == blueprintMatchLength)
                        {
                            foreach (var arg in ctorArgs)
                            {
                                var kvp = blueprint.First(pair => arg.Name.EndsWith(pair.Key, StringComparison.OrdinalIgnoreCase));
                                args.Add(Decorator.Enclose(kvp.Value).ChangeType(arg.ParameterType));
                                blueprint.Remove(kvp.Key);
                            }
                            break;
                        }
                    }
                }

                instance = Activator.CreateInstance(desiredType, args.ToArray()) as Exception;

                if (blueprint.Count > 0) { PopulateBlueprintToInstance(blueprint, desiredType, instance); }
            }

            return instance;
        }

        private static void PopulateBlueprintToInstance(Dictionary<string, object> blueprint, Type desiredType, object instance)
        {
            var fields = desiredType.GetFields(MemberReflection.CreateFlags(o => o.ExcludeStatic = true)).ToList();
            var properties = desiredType.GetProperties(MemberReflection.CreateFlags(o => o.ExcludeStatic = true)).ToList();
            foreach (var kvp in blueprint)
            {
                var property = properties.SingleOrDefault(pi => pi.Name.Equals(kvp.Key, StringComparison.OrdinalIgnoreCase));
                if (property != null && property.CanWrite)
                {
                    property.SetValue(instance, Decorator.Enclose(kvp.Value).ChangeType(property.PropertyType));
                    blueprint.Remove(kvp.Key);
                }
                else // fallback to potential backing field
                {
                    var field = fields.SingleOrDefault(fi => fi.Name.EndsWith(kvp.Key, StringComparison.OrdinalIgnoreCase));
                    if (field != null)
                    {
                        field.SetValue(instance, Decorator.Enclose(kvp.Value).ChangeType(field.FieldType));
                        blueprint.Remove(kvp.Key);
                    }
                }
                if (blueprint.Count == 0) { break; }
            }
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
        /// Gets a value indicating whether this <see cref="T:Newtonsoft.Json.JsonConverter" /> can read JSON.
        /// </summary>
        /// <value><c>true</c> if this <see cref="T:Newtonsoft.Json.JsonConverter" /> can read JSON; otherwise, <c>false</c>.</value>
        public override bool CanRead => true;

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
                var lines = exception.StackTrace.Split(new[] { Alphanumeric.NewLine }, StringSplitOptions.RemoveEmptyEntries);
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

            var properties = Decorator.Enclose(exception.GetType()).GetRuntimePropertiesExceptOf<AggregateException>().ToList();
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
