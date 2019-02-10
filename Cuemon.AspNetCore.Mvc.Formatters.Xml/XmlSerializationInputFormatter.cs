using System.Text;
using System.Threading.Tasks;
using Cuemon.Serialization.Xml.Formatters;
using Microsoft.AspNetCore.Http.Internal;
using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.Net.Http.Headers;

namespace Cuemon.AspNetCore.Mvc.Formatters.Xml
{
    /// <summary>
    /// This class handles deserialization of JSON to objects using <see cref="XmlFormatter"/>.
    /// </summary>
    public class XmlSerializationInputFormatter : TextInputFormatter
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="XmlSerializationInputFormatter"/> class.
        /// </summary>
        public XmlSerializationInputFormatter(XmlFormatterOptions formatterOptions)
        {
            SupportedEncodings.Add(Encoding.UTF8);
            SupportedEncodings.Add(Encoding.Unicode);
            SupportedMediaTypes.Add(new MediaTypeHeaderValue("application/xml"));
            SupportedMediaTypes.Add(new MediaTypeHeaderValue("text/xml"));
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
            var formatter = new XmlFormatter(FormatterOptions);
            var deserializedObject = formatter.Deserialize(request.Body, context.ModelType);
            return InputFormatterResult.SuccessAsync(deserializedObject);
        }

        private XmlFormatterOptions FormatterOptions { get; }
    }
}