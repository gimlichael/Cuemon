using System;
using Cuemon.Serialization.Json.Formatters;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Options;

namespace Cuemon.AspNetCore.Mvc.Formatters.Json
{
    /// <summary>
    /// Extension methods for adding XML formatters to MVC.
    /// </summary>
    public static class JsonMvcCoreBuilderExtensions
    {
        /// <summary>
        /// Adds the JSON Serializer formatters to MVC.
        /// </summary>
        /// <param name="builder">The <see cref="IMvcCoreBuilder"/>.</param>
        /// <returns>The <see cref="IMvcCoreBuilder"/>.</returns>
        public static IMvcCoreBuilder AddJsonSerializationFormatters(this IMvcCoreBuilder builder)
        {
            Validator.ThrowIfNull(builder, nameof(builder));
            builder.Services.TryAddEnumerable(ServiceDescriptor.Transient<IConfigureOptions<MvcOptions>, JsonSerializationMvcOptionsSetup>());
            return builder;
        }

        /// <summary>
        /// Adds the JSON Serializer formatters to MVC.
        /// </summary>
        /// <param name="builder">The <see cref="IMvcBuilder"/>.</param>
        /// <returns>The <see cref="IMvcBuilder"/>.</returns>
        public static IMvcBuilder AddJsonSerializationFormatters(this IMvcBuilder builder)
        {
            Validator.ThrowIfNull(builder, nameof(builder));
            builder.Services.TryAddEnumerable(ServiceDescriptor.Transient<IConfigureOptions<MvcOptions>, JsonSerializationMvcOptionsSetup>());
            return builder;
        }

        /// <summary>
        /// Adds configuration of <see cref="JsonFormatterOptions"/> for the application.
        /// </summary>
        /// <param name="builder">The <see cref="IMvcBuilder"/>.</param>
        /// <param name="setup">The <see cref="JsonFormatterOptions"/> which need to be configured.</param>
        /// <returns>The <see cref="IMvcBuilder"/>.</returns>
        public static IMvcCoreBuilder AddJsonFormatterOptions(this IMvcCoreBuilder builder, Action<JsonFormatterOptions> setup = null)
        {
            Validator.ThrowIfNull(builder, nameof(builder));
            Validator.ThrowIfNull(setup, nameof(setup));
            builder.Services.Configure(setup);
            return builder;
        }

        /// <summary>
        /// Adds configuration of <see cref="JsonFormatterOptions"/> for the application.
        /// </summary>
        /// <param name="builder">The <see cref="IMvcBuilder"/>.</param>
        /// <param name="setup">The <see cref="JsonFormatterOptions"/> which need to be configured.</param>
        /// <returns>The <see cref="IMvcBuilder"/>.</returns>
        public static IMvcBuilder AddJsonFormatterOptions(this IMvcBuilder builder, Action<JsonFormatterOptions> setup = null)
        {
            Validator.ThrowIfNull(builder, nameof(builder));
            Validator.ThrowIfNull(setup, nameof(setup));
            builder.Services.Configure(setup);
            return builder;
        }
    }
}