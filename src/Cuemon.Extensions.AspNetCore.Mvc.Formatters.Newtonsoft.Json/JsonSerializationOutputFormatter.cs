using Cuemon.AspNetCore.Mvc.Formatters;
using Cuemon.Extensions.Newtonsoft.Json.Formatters;
using Microsoft.Net.Http.Headers;

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
        public JsonSerializationOutputFormatter(JsonFormatterOptions options) : base(options)
        {
            SupportedMediaTypes.Add(new MediaTypeHeaderValue("application/json"));
            SupportedMediaTypes.Add(new MediaTypeHeaderValue("text/json"));
        }
    }
}