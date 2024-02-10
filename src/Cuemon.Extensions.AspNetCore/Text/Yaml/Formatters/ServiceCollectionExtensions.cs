using System;
using System.Net.Http;
using Cuemon.AspNetCore.Diagnostics;
using Cuemon.Extensions.AspNetCore.Text.Yaml.Converters;
using Cuemon.Extensions.DependencyInjection;
using Cuemon.Net.Http;
using Cuemon.Text.Yaml.Formatters;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Options;

namespace Cuemon.Extensions.AspNetCore.Text.Yaml.Formatters
{
    /// <summary>
    /// Extension methods for the <see cref="IServiceCollection"/> interface.
    /// </summary>
    public static class ServiceCollectionExtensions
    {
        /// <summary>
        /// Adds configuration of <see cref="YamlFormatterOptions"/> for the application.
        /// </summary>
        /// <param name="services">The <see cref="IServiceCollection"/> to extend.</param>
        /// <param name="setup">The <see cref="YamlFormatterOptions"/> which may be configured.</param>
        /// <returns>A reference to <paramref name="services" /> so that additional configuration calls can be chained.</returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="services"/> cannot be null.
        /// </exception>
        public static IServiceCollection AddYamlFormatterOptions(this IServiceCollection services, Action<YamlFormatterOptions> setup = null)
        {
            Validator.ThrowIfNull(services);
            Validator.ThrowIfInvalidConfigurator(setup, out var options);
            services.TryConfigure(setup ?? (o =>
            {
                o.Settings = options.Settings;
                o.SensitivityDetails = options.SensitivityDetails;
                o.SupportedMediaTypes = options.SupportedMediaTypes;
            }));
            return services;
        }

        /// <summary>
        /// Adds an <see cref="IHttpExceptionDescriptorResponseFormatter"/> that uses <see cref="YamlFormatter"/> as engine of serialization to the specified list of <paramref name="services"/>.
        /// </summary>
        /// <param name="services">The <see cref="IServiceCollection"/> to extend.</param>
        /// <param name="setup">The <see cref="YamlFormatterOptions"/> which may be configured.</param>
        /// <returns>A reference to <paramref name="services" /> so that additional calls can be chained.</returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="services"/> cannot be null
        /// </exception>
        /// <remarks>Configuration of the <see cref="YamlFormatter"/> is done through a call to <see cref="ServiceProviderServiceExtensions.GetService{T}"/> retrieving an <see cref="IOptions{TOptions}"/> implementation of <see cref="YamlFormatterOptions"/>.</remarks>
        public static IServiceCollection AddYamlExceptionResponseFormatter(this IServiceCollection services, Action<YamlFormatterOptions> setup = null)
        {
            Validator.ThrowIfNull(services);
            services.AddYamlFormatterOptions(setup);
            services.TryAddSingleton(provider =>
            {
                var options = provider.GetService<IOptions<YamlFormatterOptions>>().Value;
                return new HttpExceptionDescriptorResponseFormatter<YamlFormatterOptions>(options)
                    .Adjust(o => o.Settings.Converters.AddHttpExceptionDescriptorConverter(edo => edo.SensitivityDetails = o.SensitivityDetails))
                    .Populate((descriptor, contentType) => new StreamContent(YamlFormatter.SerializeObject(descriptor, options))
                    {
                        Headers = { { HttpHeaderNames.ContentType, contentType.MediaType } }
                    });
            });
            return services;
        }
    }
}
