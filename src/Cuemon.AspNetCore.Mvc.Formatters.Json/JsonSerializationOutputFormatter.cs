using System.IO;
using System.Text;
using System.Threading.Tasks;
using Cuemon.Extensions.Newtonsoft.Json.Formatters;
using Cuemon.IO;
using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.Net.Http.Headers;

namespace Cuemon.Extensions.AspNetCore.Mvc.Formatters.Json
{
    /// <summary>
    /// This class handles serialization of objects to JSON using <see cref="JsonFormatter"/>.
    /// </summary>
    public class JsonSerializationOutputFormatter : TextOutputFormatter
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="JsonSerializationOutputFormatter"/> class.
        /// </summary>
        public JsonSerializationOutputFormatter(JsonFormatterOptions formatterOptions)
        {
            SupportedEncodings.Add(Encoding.UTF8);
            SupportedEncodings.Add(Encoding.Unicode);
            SupportedMediaTypes.Add(new MediaTypeHeaderValue("application/json"));
            SupportedMediaTypes.Add(new MediaTypeHeaderValue("text/json"));
            FormatterOptions = formatterOptions;
        }

        /// <summary>
        /// Write response body as an asynchronous operation.
        /// </summary>
        /// <param name="context">The formatter context associated with the call.</param>
        /// <param name="selectedEncoding">The <see cref="Encoding" /> that should be used to write the response.</param>
        /// <returns>A task which can write the response body.</returns>
        public override async Task WriteResponseBodyAsync(OutputFormatterWriteContext context, Encoding selectedEncoding)
        {
            Validator.ThrowIfNull(context, nameof(context));
            Validator.ThrowIfNull(selectedEncoding, nameof(selectedEncoding));
            var value = context.Object;
            using (var textWriter = context.WriterFactory(context.HttpContext.Response.Body, selectedEncoding))
            {
                JsonFormatter formatter = new JsonFormatter(FormatterOptions);
                using (var streamReader = new StreamReader(formatter.Serialize(value)))
                {
                    await Decorator.Enclose(streamReader).CopyToAsync(textWriter).ConfigureAwait(false);
                }
                await textWriter.FlushAsync().ConfigureAwait(false);
            }
        }

        private JsonFormatterOptions FormatterOptions { get; }
    }
}