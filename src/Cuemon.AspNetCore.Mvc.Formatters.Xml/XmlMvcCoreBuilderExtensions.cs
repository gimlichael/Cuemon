using System;
using Cuemon.Serialization.Xml.Formatters;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Options;

namespace Cuemon.AspNetCore.Mvc.Formatters.Xml
{
    /// <summary>
    /// Extension methods for adding XML formatters to MVC.
    /// </summary>
    public static class XmlMvcCoreBuilderExtensions
    {
        /// <summary>
        /// Adds the XML Serializer formatters to MVC.
        /// </summary>
        /// <param name="builder">The <see cref="IMvcCoreBuilder"/>.</param>
        /// <returns>The <see cref="IMvcCoreBuilder"/>.</returns>
        public static IMvcCoreBuilder AddXmlSerializationFormatters(this IMvcCoreBuilder builder)
        {
            Validator.ThrowIfNull(builder, nameof(builder));
            builder.Services.TryAddEnumerable(ServiceDescriptor.Transient<IConfigureOptions<MvcOptions>, XmlSerializationMvcOptionsSetup>());
            return builder;
        }

        /// <summary>
        /// Adds the XML Serializer formatters to MVC.
        /// </summary>
        /// <param name="builder">The <see cref="IMvcBuilder"/>.</param>
        /// <returns>The <see cref="IMvcBuilder"/>.</returns>
        public static IMvcBuilder AddXmlSerializationFormatters(this IMvcBuilder builder)
        {
            Validator.ThrowIfNull(builder, nameof(builder));
            builder.Services.TryAddEnumerable(ServiceDescriptor.Transient<IConfigureOptions<MvcOptions>, XmlSerializationMvcOptionsSetup>());
            return builder;
        }

        /// <summary>
        /// Adds configuration of <see cref="XmlFormatterOptions"/> for the application.
        /// </summary>
        /// <param name="builder">The <see cref="IMvcBuilder"/>.</param>
        /// <param name="setup">The <see cref="XmlFormatterOptions"/> which need to be configured.</param>
        /// <returns>The <see cref="IMvcBuilder"/>.</returns>
        public static IMvcCoreBuilder AddXmlFormatterOptions(this IMvcCoreBuilder builder, Action<XmlFormatterOptions> setup = null)
        {
            Validator.ThrowIfNull(builder, nameof(builder));
            Validator.ThrowIfNull(setup, nameof(setup));
            builder.Services.Configure(setup);
            return builder;
        }

        /// <summary>
        /// Adds configuration of <see cref="XmlFormatterOptions"/> for the application.
        /// </summary>
        /// <param name="builder">The <see cref="IMvcBuilder"/>.</param>
        /// <param name="setup">The <see cref="XmlFormatterOptions"/> which need to be configured.</param>
        /// <returns>The <see cref="IMvcBuilder"/>.</returns>
        public static IMvcBuilder AddXmlFormatterOptions(this IMvcBuilder builder, Action<XmlFormatterOptions> setup = null)
        {
            Validator.ThrowIfNull(builder, nameof(builder));
            Validator.ThrowIfNull(setup, nameof(setup));
            builder.Services.Configure(setup);
            return builder;
        }
    }
}