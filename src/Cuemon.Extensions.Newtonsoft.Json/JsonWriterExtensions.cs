using Cuemon.Extensions.Newtonsoft.Json.Serialization;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

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
        /// <param name="serializer">The calling <see cref="JsonSerializer"/>.</param>
        public static void WriteObject(this JsonWriter writer, object value, JsonSerializer serializer)
        {
            serializer.Serialize(writer, value);
        }

        /// <summary>
        /// Writes the property name of a name/value pair of a JSON object.
        /// </summary>
        /// <param name="writer">The <see cref="JsonWriter"/> used to write the JSON structure.</param>
        /// <param name="name">The name of the property.</param>
        /// <param name="serializer">The calling <see cref="JsonSerializer"/>.</param>
        /// <param name="escape">A flag to indicate whether the text should be escaped when it is written as a JSON property name.</param>
        /// <remarks>In order to support an assigned <see cref="NamingStrategy"/> to <paramref name="serializer"/>, make sure <see cref="JsonSerializer.ContractResolver"/> is assignable from <see cref="DefaultContractResolver"/>.</remarks>
        public static void WritePropertyName(this JsonWriter writer, string name, JsonSerializer serializer, bool escape = false)
        {
            var ns = serializer?.ContractResolver.ResolveNamingStrategyOrDefault();
            if (ns != null) { name = ns.GetPropertyName(name, false); }
            writer.WritePropertyName(name, escape);
        }
    }
}