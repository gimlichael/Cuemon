using System;
using System.Linq;
using Cuemon.Diagnostics;
using YamlDotNet.Core;

namespace Cuemon.Extensions.YamlDotNet.Converters
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
        public override void WriteYaml(IEmitter writer, ExceptionDescriptor value)
        {
            writer.WriteStartObject();
            writer.WritePropertyName(SetPropertyName("Error"));

            writer.WriteStartObject();
            writer.WriteString(SetPropertyName("Code"), value.Code);
            writer.WriteString(SetPropertyName("Message"), value.Message);
            if (value.HelpLink != null)
            {
                writer.WriteString(SetPropertyName("HelpLink"), value.HelpLink.OriginalString);
            }
            if (_options.SensitivityDetails.HasFlag(FaultSensitivityDetails.Failure))
            {
                writer.WritePropertyName(SetPropertyName("Failure"));
                new ExceptionConverter(_options.SensitivityDetails.HasFlag(FaultSensitivityDetails.StackTrace), _options.SensitivityDetails.HasFlag(FaultSensitivityDetails.Data))
                {
                    FormatterOptions = FormatterOptions
                }.WriteYaml(writer, value.Failure);

            }
            writer.WriteEndObject();

            if (_options.SensitivityDetails.HasFlag(FaultSensitivityDetails.Evidence) && value.Evidence.Any())
            {
                writer.WritePropertyName(SetPropertyName("Evidence"));
                writer.WriteStartObject();
                foreach (var evidence in value.Evidence)
                {
                    writer.WritePropertyName(evidence.Key);
                    writer.WriteObject(evidence.Value, FormatterOptions);
                }
                writer.WriteEndObject();
            }
            writer.WriteEndObject();
        }

        /// <summary>
        /// Reads and converts the YAML to <see cref="ExceptionDescriptor"/>.
        /// </summary>
        /// <param name="reader">The reader to read from.</param>
        /// <param name="typeToConvert">The type to convert.</param>
        /// <returns>The converted value.</returns>
        /// <exception cref="NotImplementedException"></exception>
        public override ExceptionDescriptor ReadYaml(IParser reader, Type typeToConvert)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Determines whether this instance can convert the specified object type.
        /// </summary>
        /// <param name="typeToConvert">The <seealso cref="T:System.Type" /> of the object.</param>
        /// <returns><c>true</c> if this instance can convert the specified object type; otherwise, <c>false</c>.</returns>
        public override bool CanConvert(Type typeToConvert)
        {
            return typeToConvert == typeof(ExceptionDescriptor);
        }
    }
}
