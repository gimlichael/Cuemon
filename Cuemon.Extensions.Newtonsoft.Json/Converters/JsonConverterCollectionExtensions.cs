using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Cuemon.Diagnostics;
using Cuemon.Extensions.Reflection;
using Cuemon.Extensions.Runtime.Serialization;
using Cuemon.Runtime.Serialization;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Serialization;

namespace Cuemon.Extensions.Newtonsoft.Json.Converters
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
            converters.Add(new StringFlagsEnumConverter(new CamelCaseNamingStrategy()));
        }

        /// <summary>
        /// Adds an <see cref="ExceptionDescriptor"/> JSON converter to the list.
        /// </summary>
        /// <param name="converters">The list of JSON converters.</param>
        /// <param name="setup">The <see cref="ExceptionDescriptorSerializationOptions"/> which need to be configured.</param>
        public static void AddExceptionDescriptorConverter(this ICollection<JsonConverter> converters, Action<ExceptionDescriptorSerializationOptions> setup = null)
        {
            converters.Add(DynamicJsonConverter.Create<ExceptionDescriptor>((writer, descriptor) =>
            {
                var options = setup.Configure();
                writer.WriteStartObject();
                writer.WritePropertyName("error", JsonConvert.DefaultSettings.UseCamelCase);
                writer.WriteStartObject();
                writer.WritePropertyName("code", JsonConvert.DefaultSettings.UseCamelCase);
                writer.WriteValue(descriptor.Code);
                writer.WritePropertyName("message", JsonConvert.DefaultSettings.UseCamelCase);
                writer.WriteValue(descriptor.Message);
                if (descriptor.HelpLink != null)
                {
                    writer.WritePropertyName("helpLink", JsonConvert.DefaultSettings.UseCamelCase);
                    writer.WriteValue(descriptor.HelpLink.OriginalString);
                }
                if (options.IncludeFailure)
                {
                    writer.WritePropertyName("failure", JsonConvert.DefaultSettings.UseCamelCase);
                    writer.WriteObject(descriptor.Failure);
                }
                writer.WriteEndObject();
                if (options.IncludeEvidence && descriptor.Evidence.Any())
                {
                    writer.WritePropertyName("evidence", JsonConvert.DefaultSettings.UseCamelCase);
                    writer.WriteStartObject();
                    foreach (var evidence in descriptor.Evidence)
                    {
                        writer.WritePropertyName(evidence.Key, JsonConvert.DefaultSettings.UseCamelCase);
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
                writer.WritePropertyName("ticks", JsonConvert.DefaultSettings.UseCamelCase);
                writer.WriteValue(ts.Ticks);
                writer.WritePropertyName("days", JsonConvert.DefaultSettings.UseCamelCase);
                writer.WriteValue(ts.Days);
                writer.WritePropertyName("hours", JsonConvert.DefaultSettings.UseCamelCase);
                writer.WriteValue(ts.Hours);
                writer.WritePropertyName("minutes", JsonConvert.DefaultSettings.UseCamelCase);
                writer.WriteValue(ts.Minutes);
                writer.WritePropertyName("seconds", JsonConvert.DefaultSettings.UseCamelCase);
                writer.WriteValue(ts.Seconds);
                writer.WritePropertyName("totalDays", JsonConvert.DefaultSettings.UseCamelCase);
                writer.WriteValue(ts.TotalDays);
                writer.WritePropertyName("totalHours", JsonConvert.DefaultSettings.UseCamelCase);
                writer.WriteValue(ts.TotalHours);
                writer.WritePropertyName("totalMilliseconds", JsonConvert.DefaultSettings.UseCamelCase);
                writer.WriteValue(ts.TotalMilliseconds);
                writer.WritePropertyName("totalMinutes", JsonConvert.DefaultSettings.UseCamelCase);
                writer.WriteValue(ts.TotalMinutes);
                writer.WritePropertyName("totalSeconds", JsonConvert.DefaultSettings.UseCamelCase);
                writer.WriteValue(ts.TotalSeconds);
                writer.WriteEndObject();
            }, (reader, ts) => reader.ToHierarchy().UseTimeSpanFormatter()));
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
            var exceptionType = exception.GetType();
            writer.WriteStartObject();
            writer.WritePropertyName("type", JsonConvert.DefaultSettings.UseCamelCase);
            writer.WriteValue(exceptionType.FullName);
            WriteExceptionCore(writer, exception, includeStackTrace);
            writer.WriteEndObject();
        }

        private static void WriteExceptionCore(JsonWriter writer, Exception exception, bool includeStackTrace)
        {
            if (!exception.Source.IsNullOrWhiteSpace())
            {
                writer.WritePropertyName("source", JsonConvert.DefaultSettings.UseCamelCase);
                writer.WriteValue(exception.Source);
            }

            if (!exception.Message.IsNullOrWhiteSpace())
            {
                writer.WritePropertyName("message", JsonConvert.DefaultSettings.UseCamelCase);
                writer.WriteValue(exception.Message);
            }

            if (exception.StackTrace != null && includeStackTrace)
            {
                writer.WritePropertyName("stack", JsonConvert.DefaultSettings.UseCamelCase);
                writer.WriteStartArray();
                var lines = exception.StackTrace.Split(new[] { Alphanumeric.NewLine }, StringSplitOptions.RemoveEmptyEntries);
                foreach (var line in lines)
                {
                    writer.WriteValue(line.Trim());
                }
                writer.WriteEndArray();
            }

            if (exception.Data.Count > 0)
            {
                writer.WritePropertyName("data", JsonConvert.DefaultSettings.UseCamelCase);
                writer.WriteStartObject();
                foreach (DictionaryEntry entry in exception.Data)
                {
                    writer.WritePropertyName(entry.Key.ToString());
                    writer.WriteObject(entry.Value);
                }
                writer.WriteEndObject();
            }

            var properties = exception.GetType().GetRuntimePropertiesExceptOf<AggregateException>().Where(pi => pi.PropertyType.IsSimple());
            foreach (var property in properties)
            {
                var value = property.GetValue(exception);
                if (value == null) { continue; }
                writer.WritePropertyName(property.Name, JsonConvert.DefaultSettings.UseCamelCase);
                writer.WriteObject(value);
            }

            WriteInnerExceptions(writer, exception, includeStackTrace);
        }

        private static void WriteInnerExceptions(JsonWriter writer, Exception exception, bool includeStackTrace)
        {
            var aggregated = exception as AggregateException;
            var innerExceptions = new List<Exception>();
            if (aggregated != null) { innerExceptions.AddRange(aggregated.Flatten().InnerExceptions); }
            if (exception.InnerException != null) { innerExceptions.Add(exception.InnerException); }
            if (innerExceptions.Count > 0)
            {
                var endElementsToWrite = 0;
                foreach (var inner in innerExceptions)
                {
                    writer.WritePropertyName("inner", JsonConvert.DefaultSettings.UseCamelCase);
                    var exceptionType = inner.GetType();
                    writer.WriteStartObject();
                    writer.WritePropertyName("type", JsonConvert.DefaultSettings.UseCamelCase);
                    writer.WriteValue(exceptionType.FullName);
                    WriteExceptionCore(writer, inner, includeStackTrace);
                    endElementsToWrite++;
                }

                for (var i = 0; i < endElementsToWrite; i++) { writer.WriteEndObject(); }
            }
        }
    }
}