using System;
using System.Collections.Generic;
using System.Linq;
using Cuemon.AspNetCore.Diagnostics;
using Cuemon.Diagnostics;
using Cuemon.Extensions.Newtonsoft.Json;
using Cuemon.Extensions.Newtonsoft.Json.Converters;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Primitives;
using Newtonsoft.Json;

namespace Cuemon.Extensions.AspNetCore.Newtonsoft.Json.Converters
{
    /// <summary>
    /// Extension methods for the <see cref="JsonConverter"/> class.
    /// </summary>
    public static class JsonConverterCollectionExtensions
    {
        /// <summary>
        /// Adds a <see cref="ProblemDetails"/> JSON converter to the list.
        /// </summary>
        /// <param name="converters">The <see cref="T:ICollection{JsonConverter}" /> to extend.</param>
        /// <returns>A reference to <paramref name="converters"/> after the operation has completed.</returns>
        public static ICollection<JsonConverter> AddProblemDetailsConverter(this ICollection<JsonConverter> converters)
        {
            converters.Add(DynamicJsonConverter.Create<ProblemDetails>(WriteProblemDetails));
            converters.Add(DynamicJsonConverter.Create<IDecorator<ProblemDetails>>((writer, dpd, serializer) => WriteProblemDetails(writer, dpd.Inner, serializer)));
            return converters;
        }

        private static void WriteProblemDetails(JsonWriter writer, ProblemDetails pd, JsonSerializer serializer)
        {
            writer.WriteStartObject();
            if (pd.Type != null)
            {
                writer.WritePropertyName(nameof(ProblemDetails.Type), serializer);
                writer.WriteValue(pd.Type);
            }

            if (pd.Title != null)
            {
                writer.WritePropertyName(nameof(ProblemDetails.Title), serializer);
                writer.WriteValue(pd.Title);
            }

            if (pd.Status.HasValue)
            {
                writer.WritePropertyName(nameof(ProblemDetails.Status), serializer);
                writer.WriteValue(pd.Status.Value);
            }

            if (pd.Detail != null)
            {
                writer.WritePropertyName(nameof(ProblemDetails.Detail), serializer);
                writer.WriteValue(pd.Detail);
            }

            if (pd.Instance != null)
            {
                writer.WritePropertyName(nameof(ProblemDetails.Instance), serializer);
                writer.WriteValue(pd.Instance);
            }

            foreach (var extension in pd.Extensions.Where(kvp => kvp.Value != null))
            {
                writer.WritePropertyName(extension.Key, serializer);
                writer.WriteObject(extension.Value, serializer);
            }

            writer.WriteEndObject();
        }

        /// <summary>
        /// Adds an <see cref="HttpExceptionDescriptor"/> JSON converter to the list.
        /// </summary>
        /// <param name="converters">The <see cref="T:ICollection{JsonConverter}" /> to extend.</param>
        /// <param name="setup">The <see cref="ExceptionDescriptorOptions"/> which may be configured.</param>
        /// <returns>A reference to <paramref name="converters"/> after the operation has completed.</returns>
        public static ICollection<JsonConverter> AddHttpExceptionDescriptorConverter(this ICollection<JsonConverter> converters, Action<ExceptionDescriptorOptions> setup = null)
        {
            converters.AddExceptionDescriptorConverterOf<HttpExceptionDescriptor>(setup, (writer, descriptor, serializer) =>
            {
                if (descriptor.Instance != null)
                {
                    writer.WritePropertyName("Instance", serializer);
                    writer.WriteValue(descriptor.Instance.OriginalString);
                }
                writer.WritePropertyName("Status", serializer);
                writer.WriteValue(descriptor.StatusCode);
            }, (writer, descriptor, serializer) =>
            {
                if (!string.IsNullOrWhiteSpace(descriptor.CorrelationId))
                {
                    writer.WritePropertyName("CorrelationId", serializer);
                    writer.WriteValue(descriptor.CorrelationId);
                }
                if (!string.IsNullOrWhiteSpace(descriptor.RequestId))
                {
                    writer.WritePropertyName("RequestId", serializer);
                    writer.WriteValue(descriptor.RequestId);
                }
                if (!string.IsNullOrWhiteSpace(descriptor.TraceId))
                {
                    writer.WritePropertyName("TraceId", serializer);
                    writer.WriteValue(descriptor.TraceId);
                }
            });
            return converters;
        }

        /// <summary>
        /// Adds an <see cref="StringValues"/> JSON converter to the list.
        /// </summary>
        /// <param name="converters">The <see cref="T:ICollection{JsonConverter}" /> to extend.</param>
        /// <returns>A reference to <paramref name="converters"/> after the operation has completed.</returns>
        public static ICollection<JsonConverter> AddStringValuesConverter(this ICollection<JsonConverter> converters)
        {
            converters.Add(DynamicJsonConverter.Create<StringValues>((writer, values, _) =>
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
