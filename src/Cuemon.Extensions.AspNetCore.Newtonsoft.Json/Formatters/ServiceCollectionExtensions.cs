using System;
using System.Net.Http;
using Cuemon.AspNetCore.Diagnostics;
using Cuemon.Extensions.AspNetCore.Newtonsoft.Json.Converters;
using Cuemon.Extensions.DependencyInjection;
using Cuemon.Extensions.Newtonsoft.Json.Formatters;
using Cuemon.Net.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Options;

namespace Cuemon.Extensions.AspNetCore.Newtonsoft.Json.Formatters
{
    /// <summary>
    /// Extension methods for the <see cref="IServiceCollection"/> interface.
    /// </summary>
    public static class ServiceCollectionExtensions
    {
        static ServiceCollectionExtensions()
        {
            Bootstrapper.Initialize();
        }

        /// <summary>
        /// Adds configuration of <see cref="NewtonsoftJsonFormatterOptions"/> for the application.
        /// </summary>
        /// <param name="services">The <see cref="IServiceCollection"/> to extend.</param>
        /// <param name="setup">The <see cref="NewtonsoftJsonFormatterOptions"/> which may be configured.</param>
        /// <returns>A reference to <paramref name="services" /> so that additional configuration calls can be chained.</returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="services"/> cannot be null.
        /// </exception>
        /// <exception cref="ArgumentException">
        /// <paramref name="setup"/> failed to configure an instance of <see cref="NewtonsoftJsonFormatterOptions"/> in a valid state.
        /// </exception>
        public static IServiceCollection AddNewtonsoftJsonFormatterOptions(this IServiceCollection services, Action<NewtonsoftJsonFormatterOptions> setup = null)
        {
            Validator.ThrowIfNull(services);
            Validator.ThrowIfInvalidConfigurator(setup, out var options);
            services.TryConfigure(setup ?? (o =>
            {
                o.Settings = options.Settings;
                o.SensitivityDetails = options.SensitivityDetails;
                o.SupportedMediaTypes = options.SupportedMediaTypes;
                o.SynchronizeWithJsonConvert = options.SynchronizeWithJsonConvert;
            }));
            return services;
        }

        /// <summary>
        /// Adds an <see cref="IHttpExceptionDescriptorResponseFormatter"/> that uses <see cref="NewtonsoftJsonFormatter"/> as engine of serialization to the specified list of <paramref name="services"/>.
        /// </summary>
        /// <param name="services">The <see cref="IServiceCollection"/> to extend.</param>
        /// <param name="setup">The <see cref="NewtonsoftJsonFormatterOptions"/> which may be configured.</param>
        /// <returns>A reference to <paramref name="services" /> so that additional calls can be chained.</returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="services"/> cannot be null
        /// </exception>
        /// <remarks>Configuration of the <see cref="NewtonsoftJsonFormatter"/> is done through a call to <see cref="ServiceProviderServiceExtensions.GetService{T}"/> retrieving an <see cref="IOptions{TOptions}"/> implementation of <see cref="NewtonsoftJsonFormatterOptions"/>.</remarks>
        public static IServiceCollection AddNewtonsoftJsonExceptionResponseFormatter(this IServiceCollection services, Action<NewtonsoftJsonFormatterOptions> setup = null)
        {
            Validator.ThrowIfNull(services);
            services.AddNewtonsoftJsonFormatterOptions(setup);
            services.TryAddSingleton(provider =>
            {
                var options = provider.GetService<IOptions<NewtonsoftJsonFormatterOptions>>().Value;
                return new HttpExceptionDescriptorResponseFormatter<NewtonsoftJsonFormatterOptions>(options)
                    .Adjust(o => o.Settings.Converters.AddHttpExceptionDescriptorConverter(edo => edo.SensitivityDetails = o.SensitivityDetails))
                    .Populate((descriptor, contentType) => new StreamContent(NewtonsoftJsonFormatter.SerializeObject(descriptor, options))
                    {
                        Headers = { { HttpHeaderNames.ContentType, contentType.MediaType } }
                    });
            });
            return services;
        }
    }
}
