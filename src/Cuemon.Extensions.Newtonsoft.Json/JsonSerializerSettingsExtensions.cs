using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

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

        /// <summary>
        /// Retrieves a value indicating whether implementations of <see cref="JsonConverter"/> should use camelCase casing for property names.
        /// </summary>
        /// <param name="setup">The <see cref="T:Func{JsonSerializerSettings}"/> to extend.</param>
        /// <returns><c>true</c> if implementations of <see cref="JsonConverter"/> should use camelCase casing for property names; otherwise, <c>false</c>.</returns>
        public static bool UseCamelCase(this Func<JsonSerializerSettings> setup)
        {
            var settings = setup?.Invoke();
            var contractResolver = settings?.ContractResolver;
            var namingStrategyProperty = contractResolver?.GetType().GetProperty("NamingStrategy");
            var namingStrategy = namingStrategyProperty?.GetValue(contractResolver) is CamelCaseNamingStrategy;
            return namingStrategy || contractResolver is CamelCasePropertyNamesContractResolver;
        }
    }
}