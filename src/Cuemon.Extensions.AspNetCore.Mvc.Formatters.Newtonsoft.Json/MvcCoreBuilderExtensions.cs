using System;
using Cuemon.Extensions.Newtonsoft.Json.Formatters;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Options;

namespace Cuemon.Extensions.AspNetCore.Mvc.Formatters.Newtonsoft.Json
{
    /// <summary>
    /// Extension methods for the <see cref="IMvcCoreBuilder"/> interface.
    /// </summary>
    public static class MvcCoreBuilderExtensions
    {
        static MvcCoreBuilderExtensions()
        {
            Bootstrapper.Initialize();
        }

        /// <summary>
        /// Adds the JSON serializer formatters to MVC.
        /// </summary>
        /// <param name="builder">The <see cref="IMvcCoreBuilder"/>.</param>
        /// <returns>A reference to <paramref name="builder"/> after the operation has completed.</returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="builder"/> cannot be null.
        /// </exception>
        public static IMvcCoreBuilder AddJsonSerializationFormatters(this IMvcCoreBuilder builder)
        {
            Validator.ThrowIfNull(builder, nameof(builder));
            builder.Services.TryAddEnumerable(ServiceDescriptor.Transient<IConfigureOptions<MvcOptions>, JsonSerializationMvcOptionsSetup>());
            return builder;
        }

        /// <summary>
        /// Adds the JSON serializer formatters to MVC.
        /// </summary>
        /// <param name="builder">The <see cref="IMvcCoreBuilder"/>.</param>
        /// <param name="setup">The <see cref="JsonFormatterOptions"/> which need to be configured.</param>
        /// <returns>A reference to <paramref name="builder"/> after the operation has completed.</returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="builder"/> cannot be null -or-
        /// <paramref name="setup"/> cannot be null.
        /// </exception>
        public static IMvcCoreBuilder AddJsonSerializationFormatters(this IMvcCoreBuilder builder, Action<JsonFormatterOptions> setup)
        {
            Validator.ThrowIfNull(builder, nameof(builder));
            AddJsonSerializationFormatters(builder);
            AddJsonFormatterOptions(builder, setup);
            return builder;
        }

        /// <summary>
        /// Adds configuration of <see cref="JsonFormatterOptions"/> for the application.
        /// </summary>
        /// <param name="builder">The <see cref="IMvcBuilder"/>.</param>
        /// <param name="setup">The <see cref="JsonFormatterOptions"/> which need to be configured.</param>
        /// <returns>A reference to <paramref name="builder"/> after the operation has completed.</returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="builder"/> cannot be null -or-
        /// <paramref name="setup"/> cannot be null.
        /// </exception>
        public static IMvcCoreBuilder AddJsonFormatterOptions(this IMvcCoreBuilder builder, Action<JsonFormatterOptions> setup)
        {
            Validator.ThrowIfNull(builder, nameof(builder));
            Validator.ThrowIfNull(setup, nameof(setup));
            builder.Services.Configure(setup);
            return builder;
        }
    }
}
