using System;
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
                if (options.SensitivityDetails.HasFlag(FaultSensitivityDetails.Failure))
                {
                    writer.WritePropertyName("Failure", serializer);
                    new ExceptionConverter(options.SensitivityDetails.HasFlag(FaultSensitivityDetails.StackTrace), options.SensitivityDetails.HasFlag(FaultSensitivityDetails.Data)).WriteJson(writer, descriptor.Failure, serializer);
                }
                writer.WriteEndObject();
                if (options.SensitivityDetails.HasFlag(FaultSensitivityDetails.Evidence) && descriptor.Evidence.Any())
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
        /// Adds an <see cref="Exception" /> JSON converter to the list.
        /// </summary>
        /// <param name="converters">The <see cref="T:ICollection{JsonConverter}" /> to extend.</param>
        /// <param name="includeStackTrace">The value that determine whether the stack of an exception is included in the converted result.</param>
        /// <param name="includeData">The value that determine whether the data of an exception is included in the converted result.</param>
        /// <returns>A reference to <paramref name="converters"/> after the operation has completed.</returns>
        public static ICollection<JsonConverter> AddExceptionConverter(this ICollection<JsonConverter> converters, bool includeStackTrace, bool includeData)
        {
            converters.Add(new ExceptionConverter(includeStackTrace, includeData));
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
    }
}
