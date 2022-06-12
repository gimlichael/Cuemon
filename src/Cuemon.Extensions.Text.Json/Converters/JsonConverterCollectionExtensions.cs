using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Text.Json.Serialization;
using Cuemon.Diagnostics;
using Cuemon.Extensions.Text.Json.Formatters;

namespace Cuemon.Extensions.Text.Json.Converters
{
    /// <summary>
    /// Extension methods for the <see cref="JsonConverter"/> class.
    /// </summary>
    public static class JsonConverterCollectionExtensions
    {
        /// <summary>
        /// Adds an <see cref="Enum" /> JSON converter to the list.
        /// </summary>
        /// <param name="converters">The <see cref="T:ICollection{JsonConverter}" /> to extend.</param>
        /// <param name="namingPolicy">The optional naming policy for writing enum values.</param>
        /// <returns>A reference to <paramref name="converters" /> after the operation has completed.</returns>
        /// <remarks>Default implementation will, just like Newtonsoft.Json variant, favor <see cref="StringEnumConverter"/> with a fallback to <see cref="JsonStringEnumConverter"/> using default naming policy from <see cref="JsonFormatterOptions"/>.</remarks>
        public static ICollection<JsonConverter> AddStringEnumConverter(this ICollection<JsonConverter> converters, JsonNamingPolicy namingPolicy = null)
        {
            if (namingPolicy != null)
            {
                converters.Add(new JsonStringEnumConverter(namingPolicy, false));
            }
            else
            {
                try
                {
                    converters.Add(new StringEnumConverter());
                }
                catch (NotSupportedException)
                {
                    var options = new JsonFormatterOptions().Settings;
                    converters.Add(new JsonStringEnumConverter(options.PropertyNamingPolicy, false));
                }
            }
            return converters;
        }

        /// <summary>
        /// Adds a combined <see cref="Enum" /> and <see cref="FlagsAttribute" /> JSON converter to the list.
        /// </summary>
        /// <param name="converters">The <see cref="T:ICollection{JsonConverter}" /> to extend.</param>
        /// <returns>A reference to <paramref name="converters" /> after the operation has completed.</returns>
        public static ICollection<JsonConverter> AddStringFlagsEnumConverter(this ICollection<JsonConverter> converters)
        {
            converters.Add(new StringFlagsEnumConverter());
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
        public static ICollection<JsonConverter> AddExceptionDescriptorConverterOf<T>(this ICollection<JsonConverter> converters, Action<ExceptionDescriptorOptions> setup = null, Utf8JsonWriterAction<T> afterWriteErrorStartObject = null, Utf8JsonWriterAction<T> beforeWriteEndObject = null) where T : ExceptionDescriptor
        {
            converters.Add(DynamicJsonConverter.Create<T>(type => type == typeof(T), (writer, descriptor, serializerOptions) =>
            {
                var options = Patterns.Configure(setup);
                writer.WriteStartObject();
                writer.WritePropertyName(serializerOptions.SetPropertyName("Error"));
                writer.WriteStartObject();
                afterWriteErrorStartObject?.Invoke(writer, descriptor, serializerOptions);
                writer.WriteString(serializerOptions.SetPropertyName("Code"), descriptor.Code);
                writer.WriteString(serializerOptions.SetPropertyName("Message"), descriptor.Message);
                if (descriptor.HelpLink != null)
                {
                    writer.WriteString(serializerOptions.SetPropertyName("HelpLink"), descriptor.HelpLink.OriginalString);
                }
                if (options.IncludeFailure)
                {
                    writer.WritePropertyName(serializerOptions.SetPropertyName("Failure"));
                    writer.WriteObject(descriptor.Failure, serializerOptions);
                }
                writer.WriteEndObject();
                if (options.IncludeEvidence && descriptor.Evidence.Any())
                {
                    writer.WritePropertyName(serializerOptions.SetPropertyName("Evidence"));
                    writer.WriteStartObject();
                    foreach (var evidence in descriptor.Evidence)
                    {
                        writer.WritePropertyName(serializerOptions.SetPropertyName(evidence.Key));
                        writer.WriteObject(evidence.Value, serializerOptions);
                    }
                    writer.WriteEndObject();
                }
                beforeWriteEndObject?.Invoke(writer, descriptor, serializerOptions);
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
            converters.Add(DynamicJsonConverter.Create((writer, value, options) =>
            {
                writer.WriteStartObject();
                writer.WriteNumber(options.SetPropertyName("Ticks"), value.Ticks);
                writer.WriteNumber(options.SetPropertyName("Days"), value.Days);
                writer.WriteNumber(options.SetPropertyName("Hours"), value.Hours);
                writer.WriteNumber(options.SetPropertyName("Minutes"), value.Minutes);
                writer.WriteNumber(options.SetPropertyName("Seconds"), value.Seconds);
                writer.WriteNumber(options.SetPropertyName("TotalDays"), value.TotalDays);
                writer.WriteNumber(options.SetPropertyName("TotalHours"), value.TotalHours);
                writer.WriteNumber(options.SetPropertyName("TotalMilliseconds"), value.TotalMilliseconds);
                writer.WriteNumber(options.SetPropertyName("TotalMinutes"), value.TotalMinutes);
                writer.WriteNumber(options.SetPropertyName("TotalSeconds"), value.TotalSeconds);
                writer.WriteEndObject();
            }, (ref Utf8JsonReader reader, Type _, JsonSerializerOptions _) => Decorator.Enclose(reader.ToHierarchy()).UseTimeSpanFormatter()));
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
            converters.Add(DynamicJsonConverter.Create<Exception>((writer, exception, serializerOptions) =>
            {
                WriteException(writer, exception, includeStackTraceFactory?.Invoke() ?? false, serializerOptions);
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
            converters.Add(DynamicJsonConverter.Create<DataPair>((writer, dp, options) =>
            {
                writer.WriteStartObject();
                writer.WriteString(options.SetPropertyName("Name"), dp.Name);
                if (dp.HasValue)
                {
                    var value = (dp.Type == typeof(Uri)) ? Decorator.Enclose(dp.Value).ChangeTypeOrDefault<Uri>().OriginalString : dp.Value;
                    writer.WritePropertyName(options.SetPropertyName("Value"));
                    writer.WriteObject(value, options);
                }
                writer.WriteString(options.SetPropertyName("Type"), Decorator.Enclose(dp.Type).ToFriendlyName());
                writer.WriteEndObject();
            }));
            return converters;
        }

        private static void WriteException(Utf8JsonWriter writer, Exception exception, bool includeStackTrace, JsonSerializerOptions options)
        {
            var exceptionType = exception.GetType();
            writer.WriteStartObject();
            writer.WriteString(options.SetPropertyName("Type"), exceptionType.FullName);
            WriteExceptionCore(writer, exception, includeStackTrace, options);
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
