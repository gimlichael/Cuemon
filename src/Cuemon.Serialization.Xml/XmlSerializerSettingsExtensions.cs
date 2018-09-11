namespace Cuemon.Serialization.Xml
{
    /// <summary>
    /// Extension methods for the <see cref="XmlSerializerSettings"/>.
    /// </summary>
    public static class XmlSerializerSettingsExtensions
    {
        /// <summary>
        /// Applies the specified <paramref name="settings"/> to the function delegate <see cref="XmlConvert.DefaultSettings"/>.
        /// </summary>
        /// <param name="settings">The XML serializer settings.</param>
        public static void ApplyToDefaultSettings(this XmlSerializerSettings settings)
        {
            XmlConvert.DefaultSettings = () => settings;
        }
    }
}