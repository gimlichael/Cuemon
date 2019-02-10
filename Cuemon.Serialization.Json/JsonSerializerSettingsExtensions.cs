using Newtonsoft.Json;

namespace Cuemon.Serialization.Json
{
    /// <summary>
    /// Extension methods for the <see cref="JsonSerializerSettings"/>.
    /// </summary>
    public static class JsonSerializerSettingsExtensions
    {
        /// <summary>
        /// Applies the specified <paramref name="settings"/> to the function delegate <see cref="JsonConvert.DefaultSettings"/>.
        /// </summary>
        /// <param name="settings">The JSON serializer settings.</param>
        public static void ApplyToDefaultSettings(this JsonSerializerSettings settings)
        {
            JsonConvert.DefaultSettings = () => settings;
        }
    }
}