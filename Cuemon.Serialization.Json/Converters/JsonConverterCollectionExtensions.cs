using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Cuemon.Diagnostics;
using Cuemon.Serialization.Formatters;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace Cuemon.Serialization.Json.Converters
{
    /// <summary>
    /// Extension methods for the <see cref="ICollection{JsonConverter}"/>.
    /// </summary>
    public static class JsonConverterCollectionExtensions
    {
        /// <summary>
        /// Adds an <see cref="Enum"/> JSON converter to the list.
        /// </summary>
        /// <param name="converters">The list of JSON converters.</param>
        public static void AddStringEnumConverter(this ICollection<JsonConverter> converters)
        {
            converters.Add(new StringEnumConverter());
        }

        /// <summary>
        /// Adds a combined <see cref="Enum"/> and <see cref="FlagsAttribute"/> JSON converter to the list.
        /// </summary>
        /// <param name="converters">The list of JSON converters.</param>
        public static void AddStringFlagsEnumConverter(this ICollection<JsonConverter> converters)
        {
            converters.Add(new StringFlagsEnumConverter());
        }

        /// <summary>
        /// Adds an <see cref="ExceptionDescriptor"/> JSON converter to the list.
        /// </summary>
        /// <param name="converters">The list of JSON converters.</param>
        /// <param name="setup">The <see cref="ExceptionDescriptorSerializationOptions"/> which need to be configured.</param>
        public static void AddExceptionDescriptorConverter(this ICollection<JsonConverter> converters, Action<ExceptionDescriptorSerializationOptions> setup = null)
        {
            var options = setup.ConfigureOptions();
            converters.Add(DynamicJsonConverter.Create<ExceptionDescriptor>((writer, descriptor) =>
            {
                writer.WriteStartObject();
                writer.WritePropertyName("error", () => DynamicJsonConverter.UseCamelCase);
                writer.WriteStartObject();
                writer.WritePropertyName("code", () => DynamicJsonConverter.UseCamelCase);
                writer.WriteValue(descriptor.Code);
                writer.WritePropertyName("message", () => DynamicJsonConverter.UseCamelCase);
                writer.WriteValue(descriptor.Message);
                if (descriptor.HelpLink != null)
                {
                    writer.WritePropertyName("helpLink", () => DynamicJsonConverter.UseCamelCase);
                    writer.WriteValue(descriptor.HelpLink.OriginalString);
                }
                if (options.IncludeFailure)
                {
                    writer.WritePropertyName("failure", () => DynamicJsonConverter.UseCamelCase);
                    writer.WriteObject(descriptor.Failure);
                }
                writer.WriteEndObject();
                if (options.IncludeEvidence && descriptor.Evidence.Any())
                {
                    writer.WritePropertyName("evidence", () => DynamicJsonConverter.UseCamelCase);
                    writer.WriteStartObject();
                    foreach (var evidence in descriptor.Evidence)
                    {
                        writer.WritePropertyName(evidence.Key, () => DynamicJsonConverter.UseCamelCase);
                        writer.WriteObject(evidence.Value);
                    }
                    writer.WriteEndObject();
                }
                writer.WriteEndObject();
            }));
        }

        /// <summary>
        /// Adds a <see cref="TimeSpan"/> JSON converter to the list.
        /// </summary>
        /// <param name="converters">The list of JSON converters.</param>
        public static void AddTimeSpanConverter(this ICollection<JsonConverter> converters)
        {
            converters.Add(DynamicJsonConverter.Create((writer, ts) =>
            {
                writer.WriteStartObject();
                writer.WritePropertyName("ticks", () => DynamicJsonConverter.UseCamelCase);
                writer.WriteValue(ts.Ticks);
                writer.WritePropertyName("days", () => DynamicJsonConverter.UseCamelCase);
                writer.WriteValue(ts.Days);
                writer.WritePropertyName("hours", () => DynamicJsonConverter.UseCamelCase);
                writer.WriteValue(ts.Hours);
                writer.WritePropertyName("minutes", () => DynamicJsonConverter.UseCamelCase);
                writer.WriteValue(ts.Minutes);
                writer.WritePropertyName("seconds", () => DynamicJsonConverter.UseCamelCase);
                writer.WriteValue(ts.Seconds);
                writer.WritePropertyName("totalDays", () => DynamicJsonConverter.UseCamelCase);
                writer.WriteValue(ts.TotalDays);
                writer.WritePropertyName("totalHours", () => DynamicJsonConverter.UseCamelCase);
                writer.WriteValue(ts.TotalHours);
                writer.WritePropertyName("totalMilliseconds", () => DynamicJsonConverter.UseCamelCase);
                writer.WriteValue(ts.TotalMilliseconds);
                writer.WritePropertyName("totalMinutes", () => DynamicJsonConverter.UseCamelCase);
                writer.WriteValue(ts.TotalMinutes);
                writer.WritePropertyName("totalSeconds", () => DynamicJsonConverter.UseCamelCase);
                writer.WriteValue(ts.TotalSeconds);
                writer.WriteEndObject();
            }, (reader, ts) => reader.ToHierarchy().UseTimeSpanFormatter()));
        }

        /// <summary>
        /// Adds an <see cref="Exception" /> JSON converter to the list.
        /// </summary>
        /// <param name="converters">The list of JSON converters.</param>
        /// <param name="includeStackTrace">if set to <c>true</c> the stack of an exception is included in the converted result.</param>
        public static void AddExceptionConverter(this ICollection<JsonConverter> converters, bool includeStackTrace)
        {
            AddExceptionConverter(converters, () => includeStackTrace);
        }

        /// <summary>
        /// Adds an <see cref="Exception" /> JSON converter to the list.
        /// </summary>
        /// <param name="converters">The list of JSON converters.</param>
        /// <param name="includeStackTraceFactory">The function delegate that is invoked when it is needed to determine whether the stack of an exception is included in the converted result.</param>
        public static void AddExceptionConverter(this ICollection<JsonConverter> converters, Func<bool> includeStackTraceFactory)
        {
            converters.Add(DynamicJsonConverter.Create<Exception>((writer, exception) =>
            {
                WriteException(writer, exception, includeStackTraceFactory?.Invoke() ?? false);
            }));
        }

        /// <summary>
        /// Adds an <see cref="DataPair" /> JSON converter to the list.
        /// </summary>
        /// <param name="converters">The list of JSON converters.</param>
        public static void AddDataPairConverter(this ICollection<JsonConverter> converters)
        {
            converters.Add(DynamicJsonConverter.Create<DataPair>((writer, dp) =>
            {
                writer.WriteStartObject();
                writer.WritePropertyName("name");
                writer.WriteValue(dp.Name);
                if (dp.HasValue)
                {
                    var value = (dp.Type == typeof(Uri)) ? dp.Value.As<Uri>().OriginalString : dp.Value;
                    writer.WritePropertyName("value");
                    writer.WriteValue(value);
                }
                writer.WritePropertyName("type");
                writer.WriteValue(dp.Type.ToFriendlyName());
                writer.WriteEndObject();
            }));
        }

        private static void WriteException(JsonWriter writer, Exception exception, bool includeStackTrace)
        {
            Type exceptionType = exception.GetType();
            writer.WriteStartObject();
            writer.WritePropertyName("type", () => DynamicJsonConverter.UseCamelCase);
            writer.WriteValue(exceptionType.FullName);
            WriteExceptionCore(writer, exception, includeStackTrace);
            writer.WriteEndObject();
        }

        private static void WriteEndElement<T>(T counter, JsonWriter writer)
        {
            writer.WriteEndObject();
        }

        private static void WriteExceptionCore(JsonWriter writer, Exception exception, bool includeStackTrace)
        {
            if (!exception.Source.IsNullOrWhiteSpace())
            {
                writer.WritePropertyName("source", () => DynamicJsonConverter.UseCamelCase);
                writer.WriteValue(exception.Source);
            }

            if (!exception.Message.IsNullOrWhiteSpace())
            {
                writer.WritePropertyName("message", () => DynamicJsonConverter.UseCamelCase);
                writer.WriteValue(exception.Message);
            }

            if (exception.StackTrace != null && includeStackTrace)
            {
                writer.WritePropertyName("stack", () => DynamicJsonConverter.UseCamelCase);
                writer.WriteStartArray();
                string[] lines = exception.StackTrace.Split(new[] { StringUtility.NewLine }, StringSplitOptions.RemoveEmptyEntries);
                foreach (string line in lines)
                {
                    writer.WriteValue(line.Trim());
                }
                writer.WriteEndArray();
            }

            if (exception.Data.Count > 0)
            {
                writer.WritePropertyName("data", () => DynamicJsonConverter.UseCamelCase);
                writer.WriteStartObject();
                foreach (DictionaryEntry entry in exception.Data)
                {
                    writer.WritePropertyName(entry.Key.ToString());
                    writer.WriteObject(entry.Value);
                }
                writer.WriteEndObject();
            }

            var properties = exception.GetType().GetRuntimePropertiesExceptOf<Exception>().Where(pi => pi.PropertyType.IsSimple());
            foreach (var property in properties)
            {
                var value = property.GetValue(exception);
                if (value == null) { continue; }
                writer.WritePropertyName(property.Name, () => DynamicJsonConverter.UseCamelCase);
                writer.WriteObject(value);
            }

            WriteInnerExceptions(writer, exception, includeStackTrace);
        }

        private static void WriteInnerExceptions(JsonWriter writer, Exception exception, bool includeStackTrace)
        {
            var aggregated = exception as AggregateException;
            var innerExceptions = new List<Exception>();
            if (aggregated != null) { innerExceptions.AddRange(aggregated.InnerExceptions); }
            if (exception.InnerException != null) { innerExceptions.Add(exception.InnerException); }
            if (innerExceptions.Count > 0)
            {
                int endElementsToWrite = 0;
                foreach (var inner in innerExceptions)
                {
                    writer.WritePropertyName("inner", () => DynamicJsonConverter.UseCamelCase);
                    Type exceptionType = inner.GetType();
                    writer.WriteStartObject();
                    writer.WritePropertyName("type", () => DynamicJsonConverter.UseCamelCase);
                    writer.WriteValue(exceptionType.FullName);
                    WriteExceptionCore(writer, inner, includeStackTrace);
                    endElementsToWrite++;
                }
                LoopUtility.For(endElementsToWrite, WriteEndElement, writer);
            }
        }
    }
}