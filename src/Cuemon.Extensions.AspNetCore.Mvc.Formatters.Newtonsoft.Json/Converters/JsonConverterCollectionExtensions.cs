using System;
using System.Collections.Generic;
using System.Linq;
using Cuemon.AspNetCore.Diagnostics;
using Cuemon.Diagnostics;
using Cuemon.Extensions.Newtonsoft.Json;
using Microsoft.Extensions.Primitives;
using Newtonsoft.Json;

namespace Cuemon.Extensions.AspNetCore.Mvc.Formatters.Newtonsoft.Json.Converters
{
    /// <summary>
    /// Extension methods for the <see cref="JsonConverter"/> class.
    /// </summary>
    public static class JsonConverterCollectionExtensions
    {
        /// <summary>
        /// Adds an <see cref="HttpExceptionDescriptor"/> JSON converter to the list.
        /// </summary>
        /// <param name="converters">The <see cref="T:ICollection{JsonConverter}" /> to extend.</param>
        /// <param name="setup">The <see cref="ExceptionDescriptorOptions"/> which may be configured.</param>
        /// <returns>A reference to <paramref name="converters"/> after the operation has completed.</returns>
        public static ICollection<JsonConverter> AddHttpExceptionDescriptorConverter(this ICollection<JsonConverter> converters, Action<ExceptionDescriptorOptions> setup = null)
        {
            var options = Patterns.Configure(setup);
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
                if (!string.IsNullOrWhiteSpace(descriptor.CorrelationId))
                {
                    writer.WritePropertyName("correlationId", JsonConvert.DefaultSettings.UseCamelCase);
                    writer.WriteValue(descriptor.CorrelationId);
                }
                if (!string.IsNullOrWhiteSpace(descriptor.RequestId))
                {
                    writer.WritePropertyName("requestId", JsonConvert.DefaultSettings.UseCamelCase);
                    writer.WriteValue(descriptor.RequestId);
                }
                writer.WriteEndObject();
            }));
            return converters;
        }

        /// <summary>
        /// Adds an <see cref="StringValues"/> JSON converter to the list.
        /// </summary>
        /// <param name="converters">The <see cref="T:ICollection{JsonConverter}" /> to extend.</param>
        /// <returns>A reference to <paramref name="converters"/> after the operation has completed.</returns>
        public static ICollection<JsonConverter> AddStringValuesConverter(this ICollection<JsonConverter> converters)
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
            return converters;
        }
    }
}