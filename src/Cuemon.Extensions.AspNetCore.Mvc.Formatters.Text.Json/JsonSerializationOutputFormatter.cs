using Cuemon.AspNetCore.Mvc.Formatters;
using Cuemon.Extensions.AspNetCore.Mvc.Formatters.Text.Json.Converters;
using Cuemon.Extensions.Text.Json.Formatters;
using Microsoft.Net.Http.Headers;

namespace Cuemon.Extensions.AspNetCore.Mvc.Formatters.Text.Json
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
