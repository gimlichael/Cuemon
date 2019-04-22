using System;
using Newtonsoft.Json;

namespace Cuemon.Extensions.Newtonsoft.Json
{
    /// <summary>
    /// Extension methods for the <see cref="JsonWriter"/>.
    /// </summary>
    public static class JsonWriterExtensions
    {
        /// <summary>
        /// Serializes the specified <paramref name="value"/> and writes the JSON structure using the specified <paramref name="writer"/>.
        /// </summary>
        /// <param name="writer">The <see cref="JsonWriter"/> used to write the JSON structure.</param>
        /// <param name="value">The <see cref="object"/> to serialize.</param>
        /// <param name="settings">The settings to associate with the specified <paramref name="writer"/>. Default is <see cref="JsonConvert.DefaultSettings"/>.</param>
        public static void WriteObject(this JsonWriter writer, object value, JsonSerializerSettings settings = null)
        {
            var serializer = settings == null ? JsonSerializer.CreateDefault() : JsonSerializer.Create(settings);
            serializer.Serialize(writer, value);
        }

        /// <summary>
        /// Writes the property name of a name/value pair of a JSON object.
        /// </summary>
        /// <param name="writer">The <see cref="JsonWriter"/> used to write the JSON structure.</param>
        /// <param name="name">The name of the property.</param>
        /// <param name="useCamelCaseFactory">The function delegate that will resolve whether the <paramref name="name"/> will be converted to a camelCase representation (<c>true</c>) or a PascalCase representation.</param>
        /// <param name="escape">A flag to indicate whether the text should be escaped when it is written as a JSON property name.</param>
        public static void WritePropertyName(this JsonWriter writer, string name, Func<bool> useCamelCaseFactory, bool escape = false)
        {
            // the good JamesNK has a weird way of implementing the escape parameter .. he does not use the parameter, but just calls another method internally
            // which is why i have to call two different methods :-/
            var useCamelCase = useCamelCaseFactory?.Invoke() ?? false;
            if (escape)
            {
                writer.WritePropertyName(useCamelCase ? StringConverter.ToCamelCasing(name) : StringConverter.ToPascalCasing(name), true); 
            }
            else
            {
                writer.WritePropertyName(useCamelCase ? StringConverter.ToCamelCasing(name) : StringConverter.ToPascalCasing(name));
            }
        }
    }
}