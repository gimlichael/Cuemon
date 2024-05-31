using System;
using System.Collections.Generic;
using System.Net.Http;
using Cuemon.AspNetCore.Diagnostics;
using Cuemon.Diagnostics;
using Cuemon.Extensions.AspNetCore.Text.Yaml.Converters;
using Cuemon.Extensions.YamlDotNet.Formatters;
using Cuemon.Net.Http;
using Microsoft.Extensions.Options;

namespace Cuemon.Extensions.AspNetCore.Diagnostics
{
    /// <summary>
    /// Extension methods for the <see cref="HttpExceptionDescriptorResponseHandler"/> class.
    /// </summary>
    public static class HttpExceptionDescriptorResponseHandlerExtensions
    {
        /// <summary>
        /// Adds an <see cref="HttpExceptionDescriptorResponseHandler"/> to the list of <paramref name="handlers"/>.
        /// </summary>
        /// <param name="handlers">The sequence of <see cref="HttpExceptionDescriptorResponseHandler"/> to extend.</param>
        /// <param name="setup">The <see cref="HttpExceptionDescriptorResponseHandlerOptions"/> that needs to be configured.</param>
        /// <returns>A reference to <paramref name="handlers" /> so that additional calls can be chained.</returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="handlers"/> cannot be null - or -
        /// <paramref name="setup"/> cannot be null.
        /// </exception>
        [Obsolete($"This method will be removed in near future.")]
        public static ICollection<HttpExceptionDescriptorResponseHandler> AddResponseHandler(this ICollection<HttpExceptionDescriptorResponseHandler> handlers, Action<HttpExceptionDescriptorResponseHandlerOptions> setup)
        {
            Validator.ThrowIfNull(handlers);
            return Decorator.Enclose(handlers).AddResponseHandler(setup);
        }

        /// <summary>
        /// Adds an <see cref="HttpExceptionDescriptorResponseHandler"/> to the list of <paramref name="handlers"/> that uses <see cref="YamlFormatter"/> as engine of serialization.
        /// </summary>
        /// <param name="handlers">The sequence of <see cref="HttpExceptionDescriptorResponseHandler"/> to extend.</param>
        /// <param name="options">The <see cref="ExceptionDescriptorOptions"/> which need to be configured.</param>
        /// <returns>A reference to <paramref name="handlers" /> so that additional calls can be chained.</returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="handlers"/> cannot be null.
        /// </exception>
        [Obsolete($"This method will be removed in near future; please use extension method {nameof(Text.Yaml.Formatters.ServiceCollectionExtensions.AddYamlExceptionResponseFormatter)} when configuring your services.")]
        public static ICollection<HttpExceptionDescriptorResponseHandler> AddYamlResponseHandler(this ICollection<HttpExceptionDescriptorResponseHandler> handlers, IOptions<YamlFormatterOptions> options)
        {
            Validator.ThrowIfNull(handlers);

            return new HttpExceptionDescriptorResponseFormatter<YamlFormatterOptions>(options)
                .Adjust(o => o.Settings.Converters.AddHttpExceptionDescriptorConverter(edo => edo.SensitivityDetails = o.SensitivityDetails))
                .Populate((descriptor, contentType) => new StreamContent(YamlFormatter.SerializeObject(descriptor, options.Value))
                {
                    Headers = { { HttpHeaderNames.ContentType, contentType.MediaType } }
                }, handlers)
                .ExceptionDescriptorHandlers;
        }
    }
}
