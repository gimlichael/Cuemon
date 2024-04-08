using System;
using Cuemon.Extensions.YamlDotNet.Formatters;
using YamlDotNet.Core;
using YamlDotNet.Core.Events;

namespace Cuemon.Extensions.YamlDotNet
{
    /// <summary>
    /// Extension methods for the <see cref="IEmitter"/> interface.
    /// </summary>
    public static class EmitterExtensions
    {
        /// <summary>
        /// Writes the beginning of a YAML object.
        /// </summary>
        /// <param name="writer">The <see cref="IEmitter"/> to extend.</param>
        public static void WriteStartObject(this IEmitter writer)
        {
            writer.Emit(new MappingStart(AnchorName.Empty, TagName.Empty, true, MappingStyle.Block));
        }

        /// <summary>
        /// Writes the <paramref name="propertyName"/> and <paramref name="value"/> as part of a name/value pair of an YAML object.
        /// </summary>
        /// <param name="writer">The <see cref="IEmitter"/> to extend.</param>
        /// <param name="propertyName">The name of the YAML object.</param>
        /// <param name="value">The value to be written as part of the name/value pair of an YAML object.</param>
        /// <param name="setup">The <see cref="ScalarOptions"/> which may be configured.</param>
        public static void WriteString(this IEmitter writer, string propertyName, string value, Action<ScalarOptions> setup = null)
        {
            Validator.ThrowIfNull(writer);
            Validator.ThrowIfNullOrWhitespace(propertyName);
            writer.WritePropertyName(propertyName);
            writer.WriteValue(value, setup);
        }

        /// <summary>
        /// Writes the <paramref name="propertyName"/> of an YAML object.
        /// </summary>
        /// <param name="writer">The <see cref="IEmitter"/> to extend.</param>
        /// <param name="propertyName">The name of the YAML object.</param>
        public static void WritePropertyName(this IEmitter writer, string propertyName)
        {
            Validator.ThrowIfNull(writer);
            Validator.ThrowIfNullOrWhitespace(propertyName);
            writer.Emit(new Scalar(propertyName));
        }

        /// <summary>
        /// Writes the <paramref name="value"/> of an YAML object.
        /// </summary>
        /// <param name="writer">The <see cref="IEmitter"/> to extend.</param>
        /// <param name="value">The value to be written as part of the name/value pair of an YAML object.</param>
        /// <param name="setup">The <see cref="ScalarOptions"/> which may be configured.</param>
        public static void WriteValue(this IEmitter writer, string value, Action<ScalarOptions> setup = null)
        {
            Validator.ThrowIfNull(writer);
            Validator.ThrowIfInvalidConfigurator(setup ?? (o => o.Style = value.Contains(Environment.NewLine) ? ScalarStyle.Literal : ScalarStyle.Any), out var options);
            writer.Emit(new Scalar(options.Anchor, options.Tag, value, options.Style, options.IsPlainImplicit, options.IsQuotedImplicit));
        }

        /// <summary>
        /// Denotes the end of a YAML object.
        /// </summary>
        /// <param name="writer">The <see cref="IEmitter"/> to extend.</param>
        public static void WriteEndObject(this IEmitter writer)
        {
            Validator.ThrowIfNull(writer);
            writer.Emit(new MappingEnd());
        }

        /// <summary>
        /// Writes the beginning of a YAML array.
        /// </summary>
        /// <param name="writer">The <see cref="IEmitter"/> to extend.</param>
        public static void WriteStartArray(this IEmitter writer)
        {
            Validator.ThrowIfNull(writer);
            writer.Emit(new SequenceStart(AnchorName.Empty, TagName.Empty, false, SequenceStyle.Block));
        }

        /// <summary>
        /// Denotes the end of a YAML array.
        /// </summary>
        /// <param name="writer">The <see cref="IEmitter"/> to extend.</param>
        public static void WriteEndArray(this IEmitter writer)
        {
            Validator.ThrowIfNull(writer);
            writer.Emit(new SequenceEnd());
        }

        /// <summary>
        /// Serializes the specified <paramref name="value"/> into a YAML format.
        /// </summary>
        /// <param name="writer">The <see cref="IEmitter"/> to extend.</param>
        /// <param name="value">The <see cref="object"/> to serialize.</param>
        /// <param name="options">Options to control the conversion behavior.</param>
        public static void WriteObject(this IEmitter writer, object value, YamlFormatterOptions options)
        {
            Validator.ThrowIfNull(writer);
            WriteObject(writer, value, value?.GetType(), options);
        }

        /// <summary>
        /// Serializes the specified <paramref name="value"/> into a YAML format.
        /// </summary>
        /// <param name="writer">The <see cref="IEmitter"/> to extend.</param>
        /// <param name="value">The <see cref="object"/> to serialize.</param>
        /// <param name="valueType">The type of the <paramref name="value"/> to convert.</param>
        /// <param name="options">Options to control the conversion behavior.</param>
        public static void WriteObject(this IEmitter writer, object value, Type valueType, YamlFormatterOptions options)
        {
            Validator.ThrowIfNull(writer);
            if (value == null) { return; }
            var serializer = new YamlFormatter(options);
            serializer.Serialize(writer, value, valueType);
        }
    }
}
