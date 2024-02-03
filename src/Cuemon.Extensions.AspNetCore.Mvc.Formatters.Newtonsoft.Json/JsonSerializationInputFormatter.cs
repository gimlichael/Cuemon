using Cuemon.AspNetCore.Mvc.Formatters;
using Cuemon.Extensions.AspNetCore.Newtonsoft.Json.Converters;
using Cuemon.Extensions.Newtonsoft.Json.Formatters;

namespace Cuemon.Extensions.AspNetCore.Mvc.Formatters.Newtonsoft.Json
{
    /// <summary>
    /// This class handles deserialization of JSON to objects using <see cref="NewtonsoftJsonFormatter"/>.
    /// </summary>
    public class JsonSerializationInputFormatter : StreamInputFormatter<NewtonsoftJsonFormatter, NewtonsoftJsonFormatterOptions>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="JsonSerializationInputFormatter" /> class.
        /// </summary>
        /// <param name="options">The <see cref="NewtonsoftJsonFormatterOptions"/> which need to be configured.</param>
        public JsonSerializationInputFormatter(NewtonsoftJsonFormatterOptions options) : base(options)
        {
            options.Settings.Converters.AddHttpExceptionDescriptorConverter(o => o.SensitivityDetails = options.SensitivityDetails);
            foreach (var mediaType in options.SupportedMediaTypes)
            {
                SupportedMediaTypes.Add(mediaType.ToString());
            }
        }
    }
}
