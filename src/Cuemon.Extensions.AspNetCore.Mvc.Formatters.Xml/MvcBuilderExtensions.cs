using System;
using Cuemon.Xml.Serialization.Formatters;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Options;

namespace Cuemon.Extensions.AspNetCore.Mvc.Formatters.Xml
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
        /// Adds the XML serializer formatters to MVC.
        /// </summary>
        /// <param name="builder">The <see cref="IMvcBuilder"/> to extend.</param>
        /// <returns>A reference to <paramref name="builder"/> after the operation has completed.</returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="builder"/> cannot be null.
        /// </exception>
        public static IMvcBuilder AddXmlFormatters(this IMvcBuilder builder)
        {
            Validator.ThrowIfNull(builder, nameof(builder));
            builder.Services.TryAddEnumerable(ServiceDescriptor.Transient<IConfigureOptions<MvcOptions>, XmlSerializationMvcOptionsSetup>());
            return builder;
        }

        /// <summary>
        /// Adds the XML serializer formatters to MVC.
        /// </summary>
        /// <param name="builder">The <see cref="IMvcBuilder"/> to extend.</param>
        /// <param name="setup">The <see cref="XmlFormatterOptions"/> which need to be configured.</param>
        /// <returns>A reference to <paramref name="builder"/> after the operation has completed.</returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="builder"/> cannot be null -or-
        /// <paramref name="setup"/> cannot be null.
        /// </exception>
        public static IMvcBuilder AddXmlFormatters(this IMvcBuilder builder, Action<XmlFormatterOptions> setup)
        {
            Validator.ThrowIfNull(builder, nameof(builder));
            AddXmlFormatters(builder);
            AddXmlFormattersOptions(builder, setup);
            return builder;
        }


        /// <summary>
        /// Adds configuration of <see cref="XmlFormatterOptions"/> for the application.
        /// </summary>
        /// <param name="builder">The <see cref="IMvcBuilder"/> to extend.</param>
        /// <param name="setup">The <see cref="XmlFormatterOptions"/> which need to be configured.</param>
        /// <returns>A reference to <paramref name="builder"/> after the operation has completed.</returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="builder"/> cannot be null -or-
        /// <paramref name="setup"/> cannot be null.
        /// </exception>
        public static IMvcBuilder AddXmlFormattersOptions(this IMvcBuilder builder, Action<XmlFormatterOptions> setup)
        {
            Validator.ThrowIfNull(builder, nameof(builder));
            Validator.ThrowIfNull(setup, nameof(setup));
            builder.Services.Configure(setup);
            return builder;
        }
    }
}