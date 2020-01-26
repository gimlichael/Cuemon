namespace Cuemon.Extensions.Xml.Serialization
{
    /// <summary>
    /// Extension methods for the <see cref="XmlSerializerOptions"/>.
    /// </summary>
    public static class XmlSerializerSettingsExtensions
    {
        /// <summary>
        /// Applies the specified <paramref name="options"/> to the function delegate <see cref="XmlConvert.DefaultSettings"/>.
        /// </summary>
        /// <param name="options">The XML serializer settings.</param>
        public static void ApplyToDefaultSettings(this XmlSerializerOptions options)
        {
            XmlConvert.DefaultSettings = () => options;
        }
    }
}