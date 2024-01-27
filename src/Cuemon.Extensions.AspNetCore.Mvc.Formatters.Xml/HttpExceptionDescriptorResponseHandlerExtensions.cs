using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using Cuemon.AspNetCore.Diagnostics;
using Cuemon.Extensions.AspNetCore.Mvc.Formatters.Xml.Converters;
using Cuemon.Net.Http;
using Cuemon.Xml.Serialization;
using Cuemon.Xml.Serialization.Formatters;
using Microsoft.Extensions.Options;

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
        /// <param name="setup">The <see cref="XmlFormatterOptions"/> which need to be configured.</param>
        /// <returns>A reference to <paramref name="handlers" /> so that additional calls can be chained.</returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="handlers"/> cannot be null.
        /// </exception>
        public static ICollection<HttpExceptionDescriptorResponseHandler> AddXmlResponseHandler(this ICollection<HttpExceptionDescriptorResponseHandler> handlers, IOptions<XmlFormatterOptions> setup)
        {
            Validator.ThrowIfNull(handlers);

            var options = setup.Value;

            foreach (var mediaType in options.SupportedMediaTypes)
            {
	            Decorator.Enclose(handlers).AddResponseHandler(o =>
	            {
		            o.ContentType = mediaType;
		            o.ContentFactory = ed =>
		            {
			            options.Settings.Converters.AddHttpExceptionDescriptorConverter(edo => edo.SensitivityDetails = options.SensitivityDetails);
			            return new StreamContent(XmlFormatter.SerializeObject(ed, Patterns.ConfigureRevert(options)))
			            {
				            Headers = { { HttpHeaderNames.ContentType, o.ContentType.MediaType } }
			            };
		            };
		            o.StatusCodeFactory = ed => (HttpStatusCode)ed.StatusCode;
	            });
            }

            return handlers;
        }
    }
}
