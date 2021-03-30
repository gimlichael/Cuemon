using Cuemon.AspNetCore.Mvc.Formatters;
using Cuemon.Xml.Serialization.Formatters;
using Microsoft.Net.Http.Headers;

namespace Cuemon.Extensions.AspNetCore.Mvc.Formatters.Xml
{
    /// <summary>
    /// This class handles serialization of objects to XML using <see cref="XmlFormatter"/>.
    /// </summary>
    public class XmlSerializationOutputFormatter : StreamOutputFormatter<XmlFormatter, XmlFormatterOptions>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="XmlSerializationOutputFormatter"/> class.
        /// </summary>
        public XmlSerializationOutputFormatter(XmlFormatterOptions options) : base(options)
        {
            SupportedMediaTypes.Add(new MediaTypeHeaderValue("application/xml"));
            SupportedMediaTypes.Add(new MediaTypeHeaderValue("text/xml"));
        }
    }
}