using System.Collections.Generic;
using Cuemon.Serialization.Json;
using Microsoft.Extensions.Primitives;
using Newtonsoft.Json;

namespace Cuemon.AspNetCore.Mvc.Formatters.Json.Converters
{
    /// <summary>
    /// Extension methods for the <see cref="ICollection{JsonConverter}"/>.
    /// </summary>
    public static class JsonConverterCollectionExtensions
    {
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