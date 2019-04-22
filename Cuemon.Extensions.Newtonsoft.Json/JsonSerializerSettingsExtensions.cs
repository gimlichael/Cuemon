using Newtonsoft.Json;

namespace Cuemon.Extensions.Newtonsoft.Json
{
    /// <summary>
    /// Extension methods for the <see cref="JsonSerializerSettings"/> class.
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