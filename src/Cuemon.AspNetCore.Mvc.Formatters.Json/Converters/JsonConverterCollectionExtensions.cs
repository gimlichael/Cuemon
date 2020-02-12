using System;
using System.Collections.Generic;
using System.Linq;
using Cuemon.AspNetCore.Http;
using Cuemon.Extensions.Newtonsoft.Json;
using Cuemon.Runtime.Serialization;
using Microsoft.Extensions.Primitives;
using Newtonsoft.Json;

namespace Cuemon.Extensions.AspNetCore.Mvc.Formatters.Json.Converters
{
    /// <summary>
    /// Extension methods for the <see cref="ICollection{JsonConverter}"/>.
    /// </summary>
    public static class JsonConverterCollectionExtensions
    {
        /// <summary>
        /// Adds an <see cref="HttpExceptionDescriptor"/> JSON converter to the list.
        /// </summary>
        /// <param name="converters">The list of JSON converters.</param>
        /// <param name="setup">The <see cref="ExceptionDescriptorSerializationOptions"/> which need to be configured.</param>
        public static void AddHttpExceptionDescriptorConverter(this ICollection<JsonConverter> converters, Action<ExceptionDescriptorSerializationOptions> setup = null)
        {
            var options = setup.Configure();
            converters.Add(DynamicJsonConverter.Create<HttpExceptionDescriptor>((writer, descriptor) =>
            {
                writer.WriteStartObject();
                writer.WritePropertyName("error", JsonConvert.DefaultSettings.UseCamelCase);
                writer.WriteStartObject();
                writer.WritePropertyName("status", JsonConvert.DefaultSettings.UseCamelCase);
                writer.WriteValue(descriptor.StatusCode);
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
                if (!descriptor.CorrelationId.IsNullOrWhiteSpace())
                {
                    writer.WritePropertyName("correlationId", JsonConvert.DefaultSettings.UseCamelCase);
                    writer.WriteValue(descriptor.CorrelationId);
                }
                if (!descriptor.RequestId.IsNullOrWhiteSpace())
                {
                    writer.WritePropertyName("requestId", JsonConvert.DefaultSettings.UseCamelCase);
                    writer.WriteValue(descriptor.RequestId);
                }
                writer.WriteEndObject();
            }));
        }

        /// <summary>
        /// Adds an <see cref="StringValues"/> JSON converter to the list.
        /// </summary>
        /// <param name="converters">The list of JSON converters.</param>
        public static void AddStringValuesConverter(this ICollection<JsonConverter> converters)
        {
            converters.Add(DynamicJsonConverter.Create<StringValues>((writer, values) =>
            {
                if (values.Count <= 1)
                {
                    writer.WriteValue(values.ToString());
                }
                else
                {
                    writer.WriteStartArray();
                    foreach (var value in values) { writer.WriteValue(value); }
                    writer.WriteEndArray();
                }
            }));
        }
    }
}