using System;
using System.Collections.Generic;
using Cuemon.AspNetCore.Diagnostics;
using Cuemon.Diagnostics;
using Cuemon.Extensions.Newtonsoft.Json;
using Cuemon.Extensions.Newtonsoft.Json.Converters;
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
            converters.AddExceptionDescriptorConverterOf<HttpExceptionDescriptor>(setup, (writer, descriptor, serializer) =>
            {
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
            converters.Add(DynamicJsonConverter.Create<StringValues>((writer, values, serializer) =>
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