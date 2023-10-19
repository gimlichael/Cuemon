using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text.Json;
using Cuemon.AspNetCore.Diagnostics;
using Cuemon.Diagnostics;
using Cuemon.Extensions.AspNetCore.Mvc.Formatters.Text.Json.Converters;
using Cuemon.Extensions.Text.Json.Formatters;
using Cuemon.Net.Http;
using Microsoft.Extensions.Options;

namespace Cuemon.Extensions.AspNetCore.Mvc.Formatters.Text.Json
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
        /// <param name="setup">The <see cref="JsonFormatterOptions"/> which need to be configured.</param>
        /// <returns>A reference to <paramref name="handlers" /> so that additional calls can be chained.</returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="handlers"/> cannot be null.
        /// </exception>
        public static ICollection<HttpExceptionDescriptorResponseHandler> AddJsonResponseHandler(this ICollection<HttpExceptionDescriptorResponseHandler> handlers, IOptions<JsonFormatterOptions> setup)
        {
            Validator.ThrowIfNull(handlers);
            Decorator.Enclose(handlers).AddResponseHandler(o =>
            {
                o.ContentType = MediaTypeHeaderValue.Parse("application/json");
                o.ContentFactory = ed =>
                {
                    var options = setup.Value;
                    var exceptionDescriptorOptions = Patterns.ConfigureRevertExchange<JsonFormatterOptions, ExceptionDescriptorOptions>(options);
                    options.Settings.Converters.AddHttpExceptionDescriptorConverter(exceptionDescriptorOptions);
                    return new StreamContent(JsonFormatter.SerializeObject(ed, Patterns.ConfigureRevert(options)))
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
