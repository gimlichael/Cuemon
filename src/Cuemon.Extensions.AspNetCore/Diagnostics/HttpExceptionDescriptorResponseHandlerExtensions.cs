﻿using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using Cuemon.AspNetCore.Diagnostics;
using Cuemon.Diagnostics;
using Cuemon.Extensions.AspNetCore.Text.Yaml.Converters;
using Cuemon.Net.Http;
using Cuemon.Runtime.Serialization;
using Cuemon.Text.Yaml.Formatters;

namespace Cuemon.Extensions.AspNetCore.Diagnostics
{
    /// <summary>
    /// Extension methods for the <see cref="HttpExceptionDescriptorResponseHandler"/> class.
    /// </summary>
    public static class HttpExceptionDescriptorResponseHandlerExtensions
    {
        /// <summary>
        /// Adds the response handler.
        /// </summary>
        /// <param name="handlers">The sequence of <see cref="HttpExceptionDescriptorResponseHandler"/> to extend.</param>
        /// <param name="setup">The <see cref="HttpExceptionDescriptorResponseHandlerOptions"/> that needs to be configured.</param>
        /// <returns>A reference to <paramref name="handlers" /> so that additional calls can be chained.</returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="handlers"/> cannot be null - or -
        /// <paramref name="setup"/> cannot be null.
        /// </exception>
        public static ICollection<HttpExceptionDescriptorResponseHandler> AddResponseHandler(this ICollection<HttpExceptionDescriptorResponseHandler> handlers, Action<HttpExceptionDescriptorResponseHandlerOptions> setup)
        {
            Validator.ThrowIfNull(handlers, nameof(handlers));
            return Decorator.Enclose(handlers).AddResponseHandler(setup);
        }

        /// <summary>
        /// Adds an <see cref="HttpExceptionDescriptorResponseHandler"/> to the list of <paramref name="handlers"/> that uses <see cref="YamlSerializer"/> as engine of serialization.
        /// </summary>
        /// <param name="handlers">The sequence of <see cref="HttpExceptionDescriptorResponseHandler"/> to extend.</param>
        /// <param name="setup">The <see cref="ExceptionDescriptorOptions"/> which may be configured.</param>
        /// <returns>A reference to <paramref name="handlers" /> so that additional calls can be chained.</returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="handlers"/> cannot be null.
        /// </exception>
        /// <remarks>This is also the fallback response handler for <see cref="M:ApplicationBuilderExtensions.UseFaultDescriptorExceptionHandler"/> found on <see cref="ApplicationBuilderExtensions"/>.</remarks>
        public static ICollection<HttpExceptionDescriptorResponseHandler> AddYamlResponseHandler(this ICollection<HttpExceptionDescriptorResponseHandler> handlers, Action<ExceptionDescriptorOptions> setup = null)
        {
            Validator.ThrowIfNull(handlers, nameof(handlers));
            Decorator.Enclose(handlers).AddResponseHandler(o =>
            {
                o.ContentType = MediaTypeHeaderValue.Parse("text/plain");
                o.ContentFactory = ed =>
                {
                    return new StreamContent(YamlFormatter.SerializeObject(ed, setup: yfo =>
                    {
                        yfo.Settings.Converters.AddHttpExceptionDescriptorConverter(setup);
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