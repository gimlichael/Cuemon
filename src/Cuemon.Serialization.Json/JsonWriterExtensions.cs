using Newtonsoft.Json;

namespace Cuemon.Serialization.Json
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
    }
}