using Cuemon.AspNetCore.Mvc.Formatters;
using Cuemon.Extensions.AspNetCore.Mvc.Formatters.Newtonsoft.Json.Converters;
using Cuemon.Extensions.Newtonsoft.Json.Formatters;
using Microsoft.Net.Http.Headers;

namespace Cuemon.Extensions.AspNetCore.Mvc.Formatters.Newtonsoft.Json
{
    /// <summary>
    /// This class handles deserialization of JSON to objects using <see cref="JsonFormatter"/>.
    /// </summary>
    public class JsonSerializationInputFormatter : StreamInputFormatter<JsonFormatter, JsonFormatterOptions>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="JsonSerializationInputFormatter" /> class.
        /// </summary>
        /// <param name="options">The <see cref="JsonFormatterOptions"/> which need to be configured.</param>
        public JsonSerializationInputFormatter(JsonFormatterOptions options) : base(options)
        {
            options.Settings.Converters.AddHttpExceptionDescriptorConverter(o =>
            {
                o.IncludeEvidence = options.IncludeExceptionDescriptorEvidence;
                o.IncludeFailure = options.IncludeExceptionDescriptorFailure;
                o.IncludeStackTrace = options.IncludeExceptionStackTrace;
            });
            SupportedMediaTypes.Add(new MediaTypeHeaderValue("application/json"));
            SupportedMediaTypes.Add(new MediaTypeHeaderValue("text/json"));
        }
    }
}