using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using Cuemon.AspNetCore.Diagnostics;
using Cuemon.Extensions.AspNetCore.Mvc.Formatters.Newtonsoft.Json.Converters;
using Cuemon.Extensions.Newtonsoft.Json.Formatters;
using Cuemon.Net.Http;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;

namespace Cuemon.Extensions.AspNetCore.Mvc.Formatters.Newtonsoft.Json
{
    /// <summary>
    /// Extension methods for the <see cref="HttpExceptionDescriptorResponseHandler"/> class.
    /// </summary>
    public static class HttpExceptionDescriptorResponseHandlerExtensions
    {
        /// <summary>
        /// Adds an <see cref="HttpExceptionDescriptorResponseHandler"/> to the list of <paramref name="handlers"/> that uses <see cref="JsonSerializer"/> as engine of serialization.
        /// </summary>
        /// <param name="handlers">The sequence of <see cref="HttpExceptionDescriptorResponseHandler"/> to extend.</param>
        /// <param name="setup">The <see cref="NewtonsoftJsonFormatterOptions"/> which need to be configured.</param>
        /// <returns>A reference to <paramref name="handlers" /> so that additional calls can be chained.</returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="handlers"/> cannot be null
        /// </exception>
        public static ICollection<HttpExceptionDescriptorResponseHandler> AddNewtonsoftJsonResponseHandler(this ICollection<HttpExceptionDescriptorResponseHandler> handlers, IOptions<NewtonsoftJsonFormatterOptions> setup)
        {
            Validator.ThrowIfNull(handlers);
            Decorator.Enclose(handlers).AddResponseHandler(o =>
            {
                o.ContentType = MediaTypeHeaderValue.Parse("application/json");
                o.ContentFactory = ed =>
                {
                    var options = setup.Value;
                    options.Settings.Converters.AddHttpExceptionDescriptorConverter(edo => edo.SensitivityDetails = options.SensitivityDetails);
                    return new StreamContent(NewtonsoftJsonFormatter.SerializeObject(ed, Patterns.ConfigureRevert(options)))
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
