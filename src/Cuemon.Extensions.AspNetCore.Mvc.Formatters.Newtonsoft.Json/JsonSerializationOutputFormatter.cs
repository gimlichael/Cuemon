using Cuemon.AspNetCore.Mvc.Formatters;
using Cuemon.Extensions.AspNetCore.Mvc.Formatters.Newtonsoft.Json.Converters;
using Cuemon.Extensions.Newtonsoft.Json.Formatters;

namespace Cuemon.Extensions.AspNetCore.Mvc.Formatters.Newtonsoft.Json
{
    /// <summary>
    /// This class handles serialization of objects to JSON using <see cref="JsonFormatter"/>.
    /// </summary>
    public class JsonSerializationOutputFormatter : StreamOutputFormatter<JsonFormatter, JsonFormatterOptions>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="JsonSerializationOutputFormatter"/> class.
        /// </summary>
        /// <param name="options">The <see cref="JsonFormatterOptions"/> which need to be configured.</param>
        public JsonSerializationOutputFormatter(JsonFormatterOptions options) : base(options)
        {
            options.Settings.Converters.AddHttpExceptionDescriptorConverter(o => o.SensitivityDetails = options.SensitivityDetails);
            foreach (var mediaType in options.SupportedMediaTypes)
            {
                SupportedMediaTypes.Add(mediaType.ToString());
            }
        }
    }
}
