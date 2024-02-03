using Cuemon.AspNetCore.Mvc.Formatters;
using Cuemon.Extensions.AspNetCore.Text.Yaml.Converters;
using Cuemon.Text.Yaml.Formatters;

namespace Cuemon.Extensions.AspNetCore.Mvc.Formatters.Text.Yaml
{
    /// <summary>
    /// This class handles deserialization of YAML to objects using <see cref="YamlFormatter"/>.
    /// </summary>
    public class YamlSerializationInputFormatter : StreamInputFormatter<YamlFormatter, YamlFormatterOptions>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="YamlSerializationInputFormatter" /> class.
        /// </summary>
        /// <param name="options">The <see cref="YamlFormatterOptions"/> which need to be configured.</param>
        public YamlSerializationInputFormatter(YamlFormatterOptions options) : base(options)
        {
            options.Settings.Converters.AddHttpExceptionDescriptorConverter(o => o.SensitivityDetails = options.SensitivityDetails);
            foreach (var mediaType in options.SupportedMediaTypes)
            {
                SupportedMediaTypes.Add(mediaType.ToString());
            }
        }
    }
}
