using System.Text;
using System.Threading.Tasks;
using Cuemon.Extensions.Newtonsoft.Json.Formatters;
using Microsoft.AspNetCore.Http.Internal;
using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.Net.Http.Headers;

namespace Cuemon.AspNetCore.Mvc.Formatters.Json
{
    /// <summary>
    /// This class handles deserialization of JSON to objects using <see cref="JsonFormatter"/>.
    /// </summary>
    public class JsonSerializationInputFormatter : TextInputFormatter
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="JsonSerializationInputFormatter"/> class.
        /// </summary>
        public JsonSerializationInputFormatter(JsonFormatterOptions formatterOptions)
        {
            SupportedEncodings.Add(Encoding.UTF8);
            SupportedEncodings.Add(Encoding.Unicode);
            SupportedMediaTypes.Add(new MediaTypeHeaderValue("application/json"));
            SupportedMediaTypes.Add(new MediaTypeHeaderValue("text/json"));
            FormatterOptions = formatterOptions;
        }

        /// <summary>
        /// Reads an object from the request body.
        /// </summary>
        /// <param name="context">The <see cref="InputFormatterContext" />.</param>
        /// <param name="encoding">The <see cref="Encoding" /> used to read the request body.</param>
        /// <returns>A <see cref="Task" /> that on completion deserializes the request body.</returns>
        public override Task<InputFormatterResult> ReadRequestBodyAsync(InputFormatterContext context, Encoding encoding)
        {
            Validator.ThrowIfNull(context, nameof(context));
            Validator.ThrowIfNull(encoding, nameof(encoding));
            var request = context.HttpContext.Request.EnableRewind();
            var formatter = new JsonFormatter(FormatterOptions);
            var deserializedObject = formatter.Deserialize(request.Body, context.ModelType);
            return InputFormatterResult.SuccessAsync(deserializedObject);
        }

        private JsonFormatterOptions FormatterOptions { get; }
    }
}