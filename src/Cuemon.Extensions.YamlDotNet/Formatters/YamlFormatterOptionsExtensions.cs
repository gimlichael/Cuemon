namespace Cuemon.Extensions.YamlDotNet.Formatters
{
    /// <summary>
    /// Extension methods for the <see cref="YamlFormatterOptions"/> class.
    /// </summary>
    public static class YamlFormatterOptionsExtensions
    {
        /// <summary>
        /// Returns the specified <paramref name="name"/> adhering to the underlying <see cref="YamlSerializerOptions.NamingConvention"/> policy on <paramref name="options"/>.
        /// </summary>
        /// <param name="options">The <see cref="YamlFormatterOptions"/> to extend.</param>
        /// <param name="name">The name of the property.</param>
        /// <returns>If <paramref name="options"/>. is null, the specified <paramref name="name"/> is returned unaltered; otherwise it is converted according to the <see cref="YamlSerializerOptions.NamingConvention"/>.</returns>
        public static string SetPropertyName(this YamlFormatterOptions options, string name)
        {
            return options?.Settings.NamingConvention.Apply(name) ?? name;
        }
    }
}
