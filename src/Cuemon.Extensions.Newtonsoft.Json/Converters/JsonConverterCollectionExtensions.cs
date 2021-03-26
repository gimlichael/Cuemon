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
        /// <param name="ns">The optional <see cref="NamingStrategy"/> to apply.</param>
        /// <returns>A reference to <paramref name="converters"/> after the operation has completed.</returns>
        public static ICollection<JsonConverter> AddStringEnumConverter(this ICollection<JsonConverter> converters, NamingStrategy ns = null)
        {
            converters.Add(ns != null ? new StringEnumConverter(ns) : DynamicJsonConverter.Create(new StringEnumConverter()));
            return converters;
        }

        /// <summary>
        /// Adds a combined <see cref="Enum" /> and <see cref="FlagsAttribute" /> JSON converter to the list.
        /// </summary>
        /// <param name="converters">The <see cref="T:ICollection{JsonConverter}" /> to extend.</param>
        /// <param name="ns">The optional <see cref="NamingStrategy"/> to apply.</param>
        /// <returns>A reference to <paramref name="converters" /> after the operation has completed.</returns>
        public static ICollection<JsonConverter> AddStringFlagsEnumConverter(this ICollection<JsonConverter> converters, NamingStrategy ns = null)
        {
            converters.Add(ns != null ? new StringFlagsEnumConverter(ns) : DynamicJsonConverter.Create(new StringFlagsEnumConverter()));
            return converters;
        }

        /// <summary>
        /// Adds an <see cref="ExceptionDescriptor" /> JSON converter to the list.
        /// </summary>
        /// <param name="converters">The <see cref="T:ICollection{JsonConverter}" /> to extend.</param>
        /// <param name="setup">The <see cref="ExceptionDescriptorOptions" /> which may be configured.</param>
        /// <param name="afterWriteErrorStartObject">The delegate that is invoked just after writing JSON start object (<c>Error</c>).</param>
        /// <param name="beforeWriteEndObject">The delegate that is invoked just before writing the JSON end object.</param>
        /// <returns>A reference to <paramref name="converters" /> after the operation has completed.</returns>
        public static ICollection<JsonConverter> AddExceptionDescriptorConverterOf<T>(this ICollection<JsonConverter> converters, Action<ExceptionDescriptorOptions> setup = null, Action<JsonWriter, T, JsonSerializer> afterWriteErrorStartObject = null, Action<JsonWriter, T, JsonSerializer> beforeWriteEndObject = null) where T : ExceptionDescriptor
        {
            converters.Add(DynamicJsonConverter.Create<T>(type => type == typeof(T), (writer, descriptor, serializer) =>
            {
                var options = Patterns.Configure(setup);
                writer.WriteStartObject();
                writer.WritePropertyName("Error", serializer);
                writer.WriteStartObject();
                afterWriteErrorStartObject?.Invoke(writer, descriptor, serializer);
                writer.WritePropertyName("Code", serializer);
                writer.WriteValue(descriptor.Code);
                writer.WritePropertyName("Message", serializer);
                writer.WriteValue(descriptor.Message);
                if (descriptor.HelpLink != null)
                {
                    writer.WritePropertyName("HelpLink", serializer);
                    writer.WriteValue(descriptor.HelpLink.OriginalString);
                }
                if (options.IncludeFailure)
                {
                    writer.WritePropertyName("Failure", serializer);
                    writer.WriteObject(descriptor.Failure, serializer);
                }
                writer.WriteEndObject();
                if (options.IncludeEvidence && descriptor.Evidence.Any())
                {
                    writer.WritePropertyName("Evidence", serializer);
                    writer.WriteStartObject();
                    foreach (var evidence in descriptor.Evidence)
                    {
                        writer.WritePropertyName(evidence.Key, serializer);
                        writer.WriteObject(evidence.Value, serializer);
                    }
                    writer.WriteEndObject();
                }
                beforeWriteEndObject?.Invoke(writer, descriptor, serializer);
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
            converters.Add(DynamicJsonConverter.Create<TimeSpan>((writer, value, serializer) =>
            {
                writer.WriteStartObject();
                writer.WritePropertyName("Ticks", serializer);
                writer.WriteValue(value.Ticks);
                writer.WritePropertyName("Days", serializer);
                writer.WriteValue(value.Days);
                writer.WritePropertyName("Hours", serializer);
                writer.WriteValue(value.Hours);
                writer.WritePropertyName("Minutes", serializer);
                writer.WriteValue(value.Minutes);
                writer.WritePropertyName("Seconds", serializer);
                writer.WriteValue(value.Seconds);
                writer.WritePropertyName("TotalDays", serializer);
                writer.WriteValue(value.TotalDays);
                writer.WritePropertyName("TotalHours", serializer);
                writer.WriteValue(value.TotalHours);
                writer.WritePropertyName("TotalMilliseconds", serializer);
                writer.WriteValue(value.TotalMilliseconds);
                writer.WritePropertyName("TotalMinutes", serializer);
                writer.WriteValue(value.TotalMinutes);
                writer.WritePropertyName("TotalSeconds", serializer);
                writer.WriteValue(value.TotalSeconds);
                writer.WriteEndObject();
            }, (reader, objectType, existingValue, serializer) => Decorator.Enclose(reader.ToHierarchy()).UseTimeSpanFormatter()));
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
            converters.Add(DynamicJsonConverter.Create<Exception>((writer, exception, serializer) =>
            {
                WriteException(writer, exception, includeStackTraceFactory?.Invoke() ?? false, serializer);
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
            converters.Add(DynamicJsonConverter.Create<DataPair>((writer, dp, serializer) =>
            {
                writer.WriteStartObject();
                writer.WritePropertyName("Name", serializer);
                writer.WriteValue(dp.Name);
                if (dp.HasValue)
                {
                    var value = (dp.Type == typeof(Uri)) ? Decorator.Enclose(dp.Value).ChangeTypeOrDefault<Uri>().OriginalString : dp.Value;
                    writer.WritePropertyName("Value", serializer);
                    writer.WriteValue(value);
                }
                writer.WritePropertyName("Type", serializer);
                writer.WriteValue(Decorator.Enclose(dp.Type).ToFriendlyName());
                writer.WriteEndObject();
            }));
            return converters;
        }

        private static void WriteException(JsonWriter writer, Exception exception, bool includeStackTrace, JsonSerializer serializer)
        {
            var exceptionType = exception.GetType();
            writer.WriteStartObject();
            writer.WritePropertyName("Type", serializer);
            writer.WriteValue(exceptionType.FullName);
            WriteExceptionCore(writer, exception, includeStackTrace, serializer);
            writer.WriteEndObject();
        }

        private static void WriteExceptionCore(JsonWriter writer, Exception exception, bool includeStackTrace, JsonSerializer serializer)
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

            if (exception.Data.Count > 0)
            {
                writer.WritePropertyName("Data", serializer);
                writer.WriteStartObject();
                foreach (DictionaryEntry entry in exception.Data)
                {
                    writer.WritePropertyName(entry.Key.ToString());
                    writer.WriteObject(entry.Value, serializer);
                }
                writer.WriteEndObject();
            }

            var properties = Decorator.Enclose(exception.GetType()).GetRuntimePropertiesExceptOf<AggregateException>().Where(pi => !Decorator.Enclose(pi.PropertyType).IsComplex());
            foreach (var property in properties)
            {
                var value = property.GetValue(exception);
                if (value == null) { continue; }
                writer.WritePropertyName(property.Name, serializer);
                writer.WriteObject(value, serializer);
            }

            WriteInnerExceptions(writer, exception, includeStackTrace, serializer);
        }

        private static void WriteInnerExceptions(JsonWriter writer, Exception exception, bool includeStackTrace, JsonSerializer serializer)
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
                    WriteExceptionCore(writer, inner, includeStackTrace, serializer);
                    endElementsToWrite++;
                }

                for (var i = 0; i < endElementsToWrite; i++) { writer.WriteEndObject(); }
            }
        }
    }
}