using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;
using Cuemon.AspNetCore.Diagnostics;
using Cuemon.Diagnostics;
using Cuemon.Extensions.Text.Json;
using Cuemon.Extensions.Text.Json.Converters;
using Microsoft.Extensions.Primitives;

namespace Cuemon.Extensions.AspNetCore.Text.Json.Converters
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
            converters.AddExceptionDescriptorConverterOf<HttpExceptionDescriptor>(setup, (writer, descriptor, options) =>
            {
                writer.WriteNumber(options.SetPropertyName("Status"), descriptor.StatusCode);
            }, (writer, descriptor, options) =>
            {
                if (!string.IsNullOrWhiteSpace(descriptor.CorrelationId))
                {
                    writer.WriteString(options.SetPropertyName("CorrelationId"), descriptor.CorrelationId);
                }
                if (!string.IsNullOrWhiteSpace(descriptor.RequestId))
                {
                    writer.WriteString(options.SetPropertyName("RequestId"), descriptor.RequestId);
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
                    writer.WriteStringValue(values.ToString());
                }
                else
                {
                    writer.WriteStartArray();
                    foreach (var value in values) { writer.WriteStringValue(value); }
                    writer.WriteEndArray();
                }
            }));
            return converters;
        }
    }
}
