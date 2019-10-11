using System;
using Cuemon.AspNetCore.Mvc.Formatters.Xml.Converters;
using Cuemon.Extensions;
using Cuemon.Extensions.Collections.Generic;
using Cuemon.Extensions.Xml.Serialization.Formatters;
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
        public static IMvcCoreBuilder AddXmlFormatterOptions(this IMvcCoreBuilder builder, Action<XmlFormatterOptions> setup)
        {
            Validator.ThrowIfNull(builder, nameof(builder));
            Validator.ThrowIfNull(setup, nameof(setup));
            builder.Services.Configure(DefaultXmlFormatterOptions(setup));
            return builder;
        }

        /// <summary>
        /// Adds configuration of <see cref="XmlFormatterOptions"/> for the application.
        /// </summary>
        /// <param name="builder">The <see cref="IMvcBuilder"/>.</param>
        /// <param name="setup">The <see cref="XmlFormatterOptions"/> which need to be configured.</param>
        /// <returns>The <see cref="IMvcBuilder"/>.</returns>
        public static IMvcBuilder AddXmlFormatterOptions(this IMvcBuilder builder, Action<XmlFormatterOptions> setup)
        {
            Validator.ThrowIfNull(builder, nameof(builder));
            Validator.ThrowIfNull(setup, nameof(setup));
            builder.Services.Configure(DefaultXmlFormatterOptions(setup));
            return builder;
        }

        private static Action<XmlFormatterOptions> DefaultXmlFormatterOptions(Action<XmlFormatterOptions> setup)
        {
            var options = setup.Configure();
            return o =>
            {
                o.IncludeExceptionStackTrace = options.IncludeExceptionStackTrace;
                o.SynchronizeWithXmlConvert = options.SynchronizeWithXmlConvert;
                o.Settings.Writer = options.Settings.Writer;
                o.Settings.RootName = options.Settings.RootName;
                o.Settings.Reader = options.Settings.Reader;
                o.Settings.Converters.AddRange(options.Settings.Converters);
                o.Settings.Converters.AddStringValuesConverter();
                o.Settings.Converters.AddHeaderDictionaryConverter();
                o.Settings.Converters.AddQueryCollectionConverter();
                o.Settings.Converters.AddFormCollectionConverter();
                o.Settings.Converters.AddCookieCollectionConverter();
                o.Settings.Converters.AddHttpExceptionDescriptorConverter();
            };
        }
    }
}