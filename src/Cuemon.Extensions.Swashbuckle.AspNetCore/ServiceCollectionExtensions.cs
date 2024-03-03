﻿using System;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Swashbuckle.AspNetCore.SwaggerGen;
using Swashbuckle.AspNetCore.SwaggerUI;

namespace Cuemon.Extensions.Swashbuckle.AspNetCore
{
    /// <summary>
    /// Extension methods for the <see cref="IServiceCollection"/> interface.
    /// </summary>
    public static class ServiceCollectionExtensions
    {
        /// <summary>
        /// Adds complementary configurations for both <see cref="SwaggerGenOptions"/> and <see cref="SwaggerUIOptions"/> - optimized for RESTful APIs.
        /// </summary>
        /// <param name="services">The <see cref="IServiceCollection"/> to extend.</param>
        /// <param name="setup">The <see cref="RestfulSwaggerOptions"/> that may be configured.</param>
        /// <returns>A reference to <paramref name="services" /> so that additional calls can be chained.</returns>
        public static IServiceCollection AddRestfulSwagger(this IServiceCollection services, Action<RestfulSwaggerOptions> setup = null)
        {
            Validator.ThrowIfInvalidConfigurator(setup, out var options);
            services.AddSwaggerGen(o =>
            {
                o.DocumentFilterDescriptors = options.Settings.DocumentFilterDescriptors;
                o.OperationFilterDescriptors = options.Settings.OperationFilterDescriptors;
                o.ParameterFilterDescriptors = options.Settings.ParameterFilterDescriptors;
                o.RequestBodyFilterDescriptors = options.Settings.RequestBodyFilterDescriptors;
                o.SchemaFilterDescriptors = options.Settings.SchemaFilterDescriptors;
                o.SchemaGeneratorOptions = options.Settings.SchemaGeneratorOptions;
                o.SwaggerGeneratorOptions = options.Settings.SwaggerGeneratorOptions;
            });
            services.AddTransient<IConfigureOptions<SwaggerGenOptions>, ConfigureSwaggerGenOptions>();
            services.AddTransient<IConfigureOptions<SwaggerUIOptions>, ConfigureSwaggerUIOptions>();
            if (options.JsonSerializerOptionsFactory != null) { services.AddTransient<ISerializerDataContractResolver>(provider => new JsonSerializerDataContractResolver(options.JsonSerializerOptionsFactory(provider))); }
            services.Configure(setup ?? (o =>
            {
                o.JsonSerializerOptionsFactory = options.JsonSerializerOptionsFactory;
                o.Settings = options.Settings;
                o.IncludeControllerXmlComments = options.IncludeControllerXmlComments;
                o.OpenApiInfo = options.OpenApiInfo;
                o.XmlDocumentations = options.XmlDocumentations;
            }));
            return services;
        }
    }
}
