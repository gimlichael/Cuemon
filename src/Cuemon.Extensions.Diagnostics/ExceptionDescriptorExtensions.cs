using System;
using Cuemon.Diagnostics;
using Cuemon.Diagnostics.Text.Yaml;
using Cuemon.Extensions.IO;
using Cuemon.Text.Yaml.Formatters;

namespace Cuemon.Extensions.Diagnostics
{
    /// <summary>
    /// Extension methods for the <see cref="ExceptionDescriptor"/> class.
    /// </summary>
    public static class ExceptionDescriptorExtensions
    {
        /// <summary>
        /// Converts the specified <paramref name="descriptor"/> to its equivalent string representation.
        /// </summary>
        /// <param name="descriptor">The <see cref="ExceptionDescriptor"/> to extend.</param>
        /// <param name="setup">The <see cref="ExceptionDescriptorOptions"/> which may be configured.</param>
        /// <returns>A string that represents the specified <paramref name="descriptor"/>.</returns>
        public static string ToYaml(this ExceptionDescriptor descriptor, Action<ExceptionDescriptorOptions> setup = null)
        {
            Validator.ThrowIfNull(descriptor);
            var formatter = new YamlFormatter(o => o.Settings.Converters.Add(new ExceptionDescriptorConverter(setup)));
            return formatter.Serialize(descriptor).ToEncodedString();
        }
    }
}
