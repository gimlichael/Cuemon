using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Cuemon.Diagnostics;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Serialization;

namespace Cuemon.Extensions.Newtonsoft.Json.Converters
{
    /// <summary>
    /// Extension methods for the <see cref="JsonConverter"/> class.
    /// </summary>
    public static class JsonConverterCollectionExtensions
    {
        /// <summary>
        /// Adds an <see cref="Enum"/> JSON converter to the list.
        /// </summary>
        /// <param name="converters">The <see cref="T:ICollection{JsonConverter}" /> to extend.</param>
        /// <returns>A reference to <paramref name="converters"/> after the operation has completed.</returns>
        public static ICollection<JsonConverter> AddStringEnumConverter(this ICollection<JsonConverter> converters)
        {
            converters.Add(new StringEnumConverter());
            return converters;
        }

        /// <summary>
        /// Adds a combined <see cref="Enum"/> and <see cref="FlagsAttribute"/> JSON converter to the list.
        /// </summary>
        /// <param name="converters">The <see cref="T:ICollection{JsonConverter}" /> to extend.</param>
        /// <returns>A reference to <paramref name="converters"/> after the operation has completed.</returns>
        public static ICollection<JsonConverter> AddStringFlagsEnumConverter(this ICollection<JsonConverter> converters)
        {
            converters.Add(new StringFlagsEnumConverter(new CamelCaseNamingStrategy()));
            return converters;
        }

        /// <summary>
        /// Adds an <see cref="ExceptionDescriptor"/> JSON converter to the list.
        /// </summary>
        /// <param name="converters">The <see cref="T:ICollection{JsonConverter}" /> to extend.</param>
        /// <param name="setup">The <see cref="ExceptionDescriptorOptions"/> which may be configured.</param>
        /// <returns>A reference to <paramref name="converters"/> after the operation has completed.</returns>
        public static ICollection<JsonConverter> AddExceptionDescriptorConverter(this ICollection<JsonConverter> converters, Action<ExceptionDescriptorOptions> setup = null)
        {
            converters.Add(DynamicJsonConverter.Create<ExceptionDescriptor>((writer, descriptor) =>
            {
                var options = Patterns.Configure(setup);
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
            return converters;
        }

        /// <summary>
        /// Adds a <see cref="TimeSpan"/> JSON converter to the list.
        /// </summary>
        /// <param name="converters">The <see cref="T:ICollection{JsonConverter}" /> to extend.</param>
        /// <returns>A reference to <paramref name="converters"/> after the operation has completed.</returns>
        public static ICollection<JsonConverter> AddTimeSpanConverter(this ICollection<JsonConverter> converters)
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
            }, (reader, ts) => Decorator.Enclose(reader.ToHierarchy()).UseTimeSpanFormatter()));
            return converters;
        }

        /// <summary>
        /// Adds an <see cref="Exception" /> JSON converter to the list.
        /// </summary>
        /// <param name="converters">The <see cref="T:ICollection{JsonConverter}" /> to extend.</param>
        /// <param name="includeStackTraceFactory">The function delegate that is invoked when it is needed to determine whether the stack of an exception is included in the converted result.</param>
        /// <returns>A reference to <paramref name="converters"/> after the operation has completed.</returns>
        public static ICollection<JsonConverter> AddExceptionConverter(this ICollection<JsonConverter> converters, Func<bool> includeStackTraceFactory)
        {
            converters.Add(DynamicJsonConverter.Create<Exception>((writer, exception) =>
            {
                WriteException(writer, exception, includeStackTraceFactory?.Invoke() ?? false);
            }));
            return converters;
        }

        /// <summary>
        /// Adds an <see cref="DataPair" /> JSON converter to the list.
        /// </summary>
        /// <param name="converters">The <see cref="T:ICollection{JsonConverter}" /> to extend.</param>
        /// <returns>A reference to <paramref name="converters"/> after the operation has completed.</returns>
        public static ICollection<JsonConverter> AddDataPairConverter(this ICollection<JsonConverter> converters)
        {
            converters.Add(DynamicJsonConverter.Create<DataPair>((writer, dp) =>
            {
                writer.WriteStartObject();
                writer.WritePropertyName("name");
                writer.WriteValue(dp.Name);
                if (dp.HasValue)
                {
                    var value = (dp.Type == typeof(Uri)) ? Decorator.Enclose(dp.Value).ChangeTypeOrDefault<Uri>().OriginalString : dp.Value;
                    writer.WritePropertyName("value");
                    writer.WriteValue(value);
                }
                writer.WritePropertyName("type");
                writer.WriteValue(Decorator.Enclose(dp.Type).ToFriendlyName());
                writer.WriteEndObject();
            }));
            return converters;
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
            if (!string.IsNullOrWhiteSpace(exception.Source))
            {
                writer.WritePropertyName("source", JsonConvert.DefaultSettings.UseCamelCase);
                writer.WriteValue(exception.Source);
            }

            if (!string.IsNullOrWhiteSpace(exception.Message))
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

            var properties = Decorator.Enclose(exception.GetType()).GetRuntimePropertiesExceptOf<AggregateException>().Where(pi => !Decorator.Enclose(pi.PropertyType).IsComplex());
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