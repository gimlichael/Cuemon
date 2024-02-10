using System;
using Cuemon.Extensions.AspNetCore.Text.Yaml.Formatters;
using Cuemon.Text.Yaml.Formatters;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Options;

namespace Cuemon.Extensions.AspNetCore.Mvc.Formatters.Text.Yaml
{
    /// <summary>
    /// Extension methods for the <see cref="IMvcBuilder"/> interface.
    /// </summary>
    public static class MvcBuilderExtensions
    {
        /// <summary>
        /// Adds the YAML serializer formatters to MVC.
        /// </summary>
        /// <param name="builder">The <see cref="IMvcBuilder"/>.</param>
        /// <param name="setup">The <see cref="YamlFormatterOptions"/> which may be configured.</param>
        /// <returns>A reference to <paramref name="builder"/> after the operation has completed.</returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="builder"/> cannot be null -or-
        /// <paramref name="setup"/> cannot be null.
        /// </exception>
        public static IMvcBuilder AddYamlFormatters(this IMvcBuilder builder, Action<YamlFormatterOptions> setup = null)
        {
            Validator.ThrowIfNull(builder);
            builder.Services.TryAddEnumerable(ServiceDescriptor.Transient<IConfigureOptions<MvcOptions>, YamlSerializationMvcOptionsSetup>());
            AddYamlFormattersOptions(builder, setup);
            return builder;
        }

        /// <summary>
        /// Adds configuration of <see cref="YamlFormatterOptions"/> for the application.
        /// </summary>
        /// <param name="builder">The <see cref="IMvcBuilder"/>.</param>
        /// <param name="setup">The <see cref="YamlFormatterOptions"/> which need to be configured.</param>
        /// <returns>A reference to <paramref name="builder"/> after the operation has completed.</returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="builder"/> cannot be null.
        /// </exception>
        public static IMvcBuilder AddYamlFormattersOptions(this IMvcBuilder builder, Action<YamlFormatterOptions> setup = null)
        {
            Validator.ThrowIfNull(builder);
            builder.Services.AddYamlFormatterOptions(setup);
            builder.Services.AddYamlExceptionResponseFormatter();
            return builder;
        }
    }
}
