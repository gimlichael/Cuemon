using System;
using System.Collections.Generic;
using System.Linq;
using Cuemon.AspNetCore.Diagnostics;
using Cuemon.Diagnostics;
using Cuemon.Text.Yaml;
using Cuemon.Text.Yaml.Converters;
using Cuemon.Text.Yaml.Formatters;

namespace Cuemon.Extensions.AspNetCore.Text.Yaml.Converters
{
    /// <summary>
    /// Extension methods for the <see cref="YamlConverter"/> class.
    /// </summary>
    public static class YamlConverterExtensions
    {
        /// <summary>
        /// Adds an <see cref="ExceptionDescriptor" /> YAML converter to the list.
        /// </summary>
        /// <param name="converters">The <see cref="T:ICollection{YamlConverter}" /> to extend.</param>
        /// <param name="setup">The <see cref="ExceptionDescriptorOptions" /> which may be configured.</param>
        /// <returns>A reference to <paramref name="converters" /> after the operation has completed.</returns>
        public static ICollection<YamlConverter> AddExceptionDescriptorConverter(this ICollection<YamlConverter> converters, Action<YamlFormatterOptions> setup = null)
        {
            var options = Patterns.Configure(setup);
            converters.Add(new ExceptionDescriptorConverter(o => o.SensitivityDetails = options.SensitivityDetails));
            return converters;
        }

        /// <summary>
        /// Adds an <see cref="HttpExceptionDescriptor"/> YAML converter to the list.
        /// </summary>
        /// <param name="converters">The <see cref="T:ICollection{YamlConverter}" /> to extend.</param>
        /// <param name="setup">The <see cref="ExceptionDescriptorOptions"/> which may be configured.</param>
        /// <returns>A reference to <paramref name="converters"/> after the operation has completed.</returns>
        public static ICollection<YamlConverter> AddHttpExceptionDescriptorConverter(this ICollection<YamlConverter> converters, Action<YamlFormatterOptions> setup = null)
        {
            var options = Patterns.Configure(setup);
            converters.Add(YamlConverterFactory.Create<HttpExceptionDescriptor>(type => type == typeof(HttpExceptionDescriptor), (writer, value, so) =>
            {
                writer.WritePropertyName(so.SetPropertyName("Error"));
                writer.WriteStartObject();
                writer.WriteString(so.SetPropertyName("Status"), value.StatusCode.ToString());
                writer.WriteString(so.SetPropertyName("Code"), value.Code);
                writer.WriteString(so.SetPropertyName("Message"), value.Message);
                if (value.HelpLink != null)
                {
                    writer.WriteString(so.SetPropertyName("HelpLink"), value.HelpLink.OriginalString);
                }
                if (options.SensitivityDetails.HasFlag(FaultSensitivityDetails.Failure))
                {
                    writer.WritePropertyName(so.SetPropertyName("Failure"));
                    new ExceptionConverter(options.SensitivityDetails.HasFlag(FaultSensitivityDetails.StackTrace), options.SensitivityDetails.HasFlag(FaultSensitivityDetails.Data)).WriteYaml(writer, value.Failure, so);
                }
                writer.WriteEndObject();

                if (options.SensitivityDetails.HasFlag(FaultSensitivityDetails.Evidence) && value.Evidence.Any())
                {
                    writer.WritePropertyName(so.SetPropertyName("Evidence"));
                    writer.WriteStartObject();
                    foreach (var evidence in value.Evidence)
                    {
                        writer.WritePropertyName(so.SetPropertyName(evidence.Key));
                        writer.WriteObject(evidence.Value, so);
                    }
                    writer.WriteEndObject();
                }

                if (!string.IsNullOrWhiteSpace(value.CorrelationId))
                {
                    writer.WriteString(so.SetPropertyName("CorrelationId"), value.CorrelationId);
                }
                if (!string.IsNullOrWhiteSpace(value.RequestId))
                {
                    writer.WriteString(so.SetPropertyName("RequestId"), value.RequestId);
                }
            }));
            return converters;
        }
    }
}
