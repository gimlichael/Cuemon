using Cuemon.AspNetCore.Mvc.Formatters;
using Cuemon.Extensions.AspNetCore.Text.Yaml.Converters;
using Cuemon.Extensions.YamlDotNet.Formatters;

namespace Cuemon.Extensions.AspNetCore.Mvc.Formatters.Text.Yaml
{
    /// <summary>
    /// This class handles serialization of objects to YAML using <see cref="YamlFormatter"/>.
    /// </summary>
    public class YamlSerializationOutputFormatter : StreamOutputFormatter<YamlFormatter, YamlFormatterOptions>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="YamlSerializationOutputFormatter"/> class.
        /// </summary>
        /// <param name="options">The <see cref="YamlFormatterOptions"/> which need to be configured.</param>
        public YamlSerializationOutputFormatter(YamlFormatterOptions options) : base(options)
        {
            options.Settings.Converters.AddHttpExceptionDescriptorConverter(o => o.SensitivityDetails = options.SensitivityDetails);
            foreach (var mediaType in options.SupportedMediaTypes)
            {
                SupportedMediaTypes.Add(mediaType.ToString());
            }
        }
    }
}
