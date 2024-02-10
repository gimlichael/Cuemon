using System;
using System.Collections.Generic;
using System.Net.Http;
using Cuemon.AspNetCore.Diagnostics;
using Cuemon.AspNetCore.Http;
using Cuemon.Extensions.AspNetCore.Newtonsoft.Json.Converters;
using Cuemon.Extensions.AspNetCore.Newtonsoft.Json.Formatters;
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
        /// <param name="options">The <see cref="NewtonsoftJsonFormatterOptions"/> which need to be configured.</param>
        /// <returns>A reference to <paramref name="handlers" /> so that additional calls can be chained.</returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="handlers"/> cannot be null
        /// </exception>
        [Obsolete($"This method will be removed in near future; please use extension method {nameof(ServiceCollectionExtensions.AddNewtonsoftJsonExceptionResponseFormatter)} when configuring your services.")]
        public static ICollection<HttpExceptionDescriptorResponseHandler> AddNewtonsoftJsonResponseHandler(this ICollection<HttpExceptionDescriptorResponseHandler> handlers, IOptions<NewtonsoftJsonFormatterOptions> options)
        {
            Validator.ThrowIfNull(handlers);
            return new HttpExceptionDescriptorResponseFormatter<NewtonsoftJsonFormatterOptions>(options)
                .Adjust(o => o.Settings.Converters.AddHttpExceptionDescriptorConverter(edo => edo.SensitivityDetails = o.SensitivityDetails))
                .Populate((descriptor, contentType) => new StreamContent(NewtonsoftJsonFormatter.SerializeObject(descriptor, options.Value))
                {
                    Headers = { { HttpHeaderNames.ContentType, contentType.MediaType } }
                }, handlers)
                .ExceptionDescriptorHandlers;
        }
    }
}
