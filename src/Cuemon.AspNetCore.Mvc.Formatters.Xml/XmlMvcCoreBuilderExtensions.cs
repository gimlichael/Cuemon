using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Options;

namespace Cuemon.AspNetCore.Mvc
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
    }
}