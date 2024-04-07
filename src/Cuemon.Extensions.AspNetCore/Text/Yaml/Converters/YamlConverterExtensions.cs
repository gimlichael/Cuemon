using System;
using System.Collections.Generic;
using System.Linq;
using Cuemon.AspNetCore.Diagnostics;
using Cuemon.Diagnostics;
using Cuemon.Extensions.YamlDotNet;
using Cuemon.Extensions.YamlDotNet.Converters;
using Cuemon.Extensions.YamlDotNet.Formatters;
using YamlDotNet.Core;

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
            var converter = YamlConverterFactory.Create<HttpExceptionDescriptor>(type => type == typeof(HttpExceptionDescriptor), (writer, value) =>
            {
                writer.WriteStartObject();
                writer.WritePropertyName("Error");
                
                writer.WriteStartObject();
                writer.WriteString("Status", value.StatusCode.ToString());
                writer.WriteString("Code", value.Code);
                writer.WriteString("Message", value.Message, o => o.Style = value.Message.Contains(Environment.NewLine) ? ScalarStyle.Literal : ScalarStyle.Any);
                if (value.HelpLink != null)
                {
                    writer.WriteString("HelpLink", value.HelpLink.OriginalString);
                }
                if (options.SensitivityDetails.HasFlag(FaultSensitivityDetails.Failure))
                {
                    writer.WritePropertyName("Failure");
                    new ExceptionConverter(options.SensitivityDetails.HasFlag(FaultSensitivityDetails.StackTrace), options.SensitivityDetails.HasFlag(FaultSensitivityDetails.Data))
                    {
                        FormatterOptions = options
                    }.WriteYaml(writer, value.Failure);
                }
                writer.WriteEndObject();

                if (options.SensitivityDetails.HasFlag(FaultSensitivityDetails.Evidence) && value.Evidence.Any())
                {
                    writer.WritePropertyName("Evidence");
                    writer.WriteStartObject();
                    foreach (var evidence in value.Evidence)
                    {
                        writer.WritePropertyName(evidence.Key);
                        writer.WriteObject(evidence.Value, options);
                    }
                    writer.WriteEndObject();
                }

                if (!string.IsNullOrWhiteSpace(value.CorrelationId))
                {
                    writer.WriteString("CorrelationId", value.CorrelationId);
                }
                if (!string.IsNullOrWhiteSpace(value.RequestId))
                {
                    writer.WriteString("RequestId", value.RequestId);
                }

                writer.WriteEndObject();
            });
            converter.FormatterOptions = options;
            converters.Add(converter);
            return converters;
        }
    }
}
