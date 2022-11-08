using System;
using System.Linq;
using Cuemon.Runtime.Serialization;
using Cuemon.Text.Yaml.Converters;

namespace Cuemon.Diagnostics.Text.Yaml
{
    /// <summary>
    /// Converts an <see cref="ExceptionDescriptor"/> to or from YAML.
    /// </summary>
    /// <seealso cref="YamlConverter{Exception}" />
    public class ExceptionDescriptorConverter : YamlConverter<ExceptionDescriptor>
    {
        private readonly ExceptionDescriptorOptions _options;

        /// <summary>
        /// Initializes a new instance of the <see cref="ExceptionDescriptorConverter"/> class.
        /// </summary>
        /// <param name="setup">The <see cref="ExceptionDescriptorOptions"/> which may be configured.</param>
        public ExceptionDescriptorConverter(Action<ExceptionDescriptorOptions> setup = null)
        {
            _options = Patterns.Configure(setup);
        }

        /// <summary>
        /// Writes a specified <paramref name="value" /> as YAML.
        /// </summary>
        /// <param name="writer">The writer to write to.</param>
        /// <param name="value">The value to convert to YAML.</param>
        /// <param name="so">An object that specifies serialization options to use.</param>
        public override void WriteYaml(YamlTextWriter writer, ExceptionDescriptor value, YamlSerializerOptions so)
        {
            writer.WritePropertyName(so.SetPropertyName("Error"));
            
            writer.WriteStartObject();
            writer.WriteString(so.SetPropertyName("Code"), value.Code);
            writer.WriteString(so.SetPropertyName("Message"), value.Message);
            if (value.HelpLink != null)
            {
                writer.WriteString(so.SetPropertyName("HelpLink"), value.HelpLink.OriginalString);
            }
            if (_options.SensitivityDetails.HasFlag(FaultSensitivityDetails.Failure))
            {
                writer.WritePropertyName(so.SetPropertyName("Failure"));
                new ExceptionConverter(_options.SensitivityDetails.HasFlag(FaultSensitivityDetails.StackTrace), _options.SensitivityDetails.HasFlag(FaultSensitivityDetails.Data)).WriteYaml(writer, value.Failure, so);
            }
            writer.WriteEndObject();

            if (_options.SensitivityDetails.HasFlag(FaultSensitivityDetails.Evidence) && value.Evidence.Any())
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
        }

        /// <summary>
        /// Reads and converts the YAML to <see cref="ExceptionDescriptor"/>.
        /// </summary>
        /// <param name="reader">The reader to read from.</param>
        /// <param name="typeToConvert">The type to convert.</param>
        /// <param name="so">An object that specifies serialization options to use.</param>
        /// <returns>The converted value.</returns>
        /// <exception cref="System.NotImplementedException"></exception>
        public override ExceptionDescriptor ReadYaml(YamlTextReader reader, Type typeToConvert, YamlSerializerOptions so)
        {
            throw new NotImplementedException();
        }
    }
}
