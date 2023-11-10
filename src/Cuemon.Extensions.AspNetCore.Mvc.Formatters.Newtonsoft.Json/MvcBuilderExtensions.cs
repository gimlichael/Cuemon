using System;
using Cuemon.Extensions.Newtonsoft.Json.Formatters;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Options;

namespace Cuemon.Extensions.AspNetCore.Mvc.Formatters.Newtonsoft.Json
{
    /// <summary>
    /// Extension methods for the <see cref="IMvcBuilder"/> interface.
    /// </summary>
    public static class MvcBuilderExtensions
    {
        static MvcBuilderExtensions()
        {
            Bootstrapper.Initialize();
        }

        /// <summary>
        /// Adds the JSON serializer formatters to MVC.
        /// </summary>
        /// <param name="builder">The <see cref="IMvcBuilder"/>.</param>
        /// <returns>A reference to <paramref name="builder"/> after the operation has completed.</returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="builder"/> cannot be null.
        /// </exception>
        public static IMvcBuilder AddNewtonsoftJsonFormatters(this IMvcBuilder builder)
        {
            Validator.ThrowIfNull(builder);
            builder.Services.TryAddEnumerable(ServiceDescriptor.Transient<IConfigureOptions<MvcOptions>, JsonSerializationMvcOptionsSetup>());
            return builder;
        }

        /// <summary>
        /// Adds the JSON serializer formatters to MVC.
        /// </summary>
        /// <param name="builder">The <see cref="IMvcBuilder"/>.</param>
        /// <param name="setup">The <see cref="NewtonsoftJsonFormatterOptions"/> which need to be configured.</param>
        /// <returns>A reference to <paramref name="builder"/> after the operation has completed.</returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="builder"/> cannot be null -or-
        /// <paramref name="setup"/> cannot be null.
        /// </exception>
        public static IMvcBuilder AddNewtonsoftJsonFormatters(this IMvcBuilder builder, Action<NewtonsoftJsonFormatterOptions> setup)
        {
            Validator.ThrowIfNull(builder);
            AddNewtonsoftJsonFormatters(builder);
            AddNewtonsoftJsonFormattersOptions(builder, setup);
            return builder;
        }

        /// <summary>
        /// Adds configuration of <see cref="NewtonsoftJsonFormatterOptions"/> for the application.
        /// </summary>
        /// <param name="builder">The <see cref="IMvcBuilder"/>.</param>
        /// <param name="setup">The <see cref="NewtonsoftJsonFormatterOptions"/> which need to be configured.</param>
        /// <returns>A reference to <paramref name="builder"/> after the operation has completed.</returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="builder"/> cannot be null -or-
        /// <paramref name="setup"/> cannot be null.
        /// </exception>
        public static IMvcBuilder AddNewtonsoftJsonFormattersOptions(this IMvcBuilder builder, Action<NewtonsoftJsonFormatterOptions> setup)
        {
            Validator.ThrowIfNull(builder);
            Validator.ThrowIfNull(setup);
            builder.Services.Configure(setup);
            return builder;
        }
    }
}