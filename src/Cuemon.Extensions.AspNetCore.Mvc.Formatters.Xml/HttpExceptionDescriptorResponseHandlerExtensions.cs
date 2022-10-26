using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using Cuemon.AspNetCore.Diagnostics;
using Cuemon.Diagnostics;
using Cuemon.Extensions.AspNetCore.Mvc.Formatters.Xml.Converters;
using Cuemon.Net.Http;
using Cuemon.Xml.Serialization;
using Cuemon.Xml.Serialization.Formatters;

namespace Cuemon.Extensions.AspNetCore.Mvc.Formatters.Xml
{
    /// <summary>
    /// Extension methods for the <see cref="HttpExceptionDescriptorResponseHandler"/> class.
    /// </summary>
    public static class HttpExceptionDescriptorResponseHandlerExtensions
    {
        /// <summary>
        /// Adds an <see cref="HttpExceptionDescriptorResponseHandler"/> to the list of <paramref name="handlers"/> that uses <see cref="XmlSerializer"/> as engine of serialization.
        /// </summary>
        /// <param name="handlers">The sequence of <see cref="HttpExceptionDescriptorResponseHandler"/> to extend.</param>
        /// <param name="setup">The <see cref="ExceptionDescriptorOptions"/> which may be configured.</param>
        /// <returns>A reference to <paramref name="handlers" /> so that additional calls can be chained.</returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="handlers"/> cannot be null.
        /// </exception>
        public static ICollection<HttpExceptionDescriptorResponseHandler> AddXmlResponseHandler(this ICollection<HttpExceptionDescriptorResponseHandler> handlers, Action<ExceptionDescriptorOptions> setup = null)
        {
            Validator.ThrowIfNull(handlers);
            Decorator.Enclose(handlers).AddResponseHandler(o =>
            {
                o.ContentType = MediaTypeHeaderValue.Parse("application/xml");
                o.ContentFactory = ed =>
                {
                    return new StreamContent(XmlFormatter.SerializeObject(ed, setup: xfo =>
                    {
                        xfo.Settings.Converters.AddHttpExceptionDescriptorConverter(setup);
                    }))
                    {
                        Headers = { { HttpHeaderNames.ContentType, o.ContentType.MediaType } }
                    };
                };
                o.StatusCodeFactory = ed => (HttpStatusCode)ed.StatusCode;
            });
            return handlers;
        }
    }
}
