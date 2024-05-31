using System;
using Cuemon.Runtime.Serialization;

namespace Cuemon.Text.Yaml.Converters
{
    /// <summary>
    /// Converts an object to or from YAML (YAML ain't markup language).
    /// </summary>
    [Obsolete("All YAML marshalling has been moved to its own assembly; Cuemon.Extensions.YamlDotNet. This member will be removed with next major version.")]
    public abstract class YamlConverter
    {
        internal abstract void WriteYamlCore(YamlTextWriter writer, object value, YamlSerializerOptions so);

        internal abstract object ReadYamlCore(YamlTextReader reader, Type typeToConvert, YamlSerializerOptions so);

        /// <summary>
        /// Determines whether this instance can convert the specified object type.
        /// </summary>
        /// <param name="typeToConvert">The <seealso cref="Type"/> of the object.</param>
        /// <returns><c>true</c> if this instance can convert the specified object type; otherwise, <c>false</c>.</returns>
        public abstract bool CanConvert(Type typeToConvert);
    }

    /// <summary>
    /// Converts an object to or from YAML (YAML ain't markup language).
    /// </summary>
    /// <typeparam name="T">The type of object or value handled by the converter.</typeparam>
    /// <seealso cref="YamlConverter" />
    [Obsolete("All YAML marshalling has been moved to its own assembly; Cuemon.Extensions.YamlDotNet. This member will be removed with next major version.")]
    public abstract class YamlConverter<T> : YamlConverter
    {
        /// <summary>
        /// Writes a specified <paramref name="value"/> as YAML.
        /// </summary>
        /// <param name="writer">The writer to write to.</param>
        /// <param name="value">The value to convert to YAML.</param>
        /// <param name="so">An object that specifies serialization options to use.</param>
        public abstract void WriteYaml(YamlTextWriter writer, T value, YamlSerializerOptions so);

        /// <summary>
        /// Reads and converts the YAML to type <typeparamref name="T"/>.
        /// </summary>
        /// <param name="reader">The reader to read from.</param>
        /// <param name="typeToConvert">The type to convert.</param>
        /// <param name="so">An object that specifies serialization options to use.</param>
        /// <returns>The converted value.</returns>
        public abstract T ReadYaml(YamlTextReader reader, Type typeToConvert, YamlSerializerOptions so);

        internal override object ReadYamlCore(YamlTextReader reader, Type typeToConvert, YamlSerializerOptions so)
        {
            return ReadYaml(reader, typeToConvert, so);
        }

        internal override void WriteYamlCore(YamlTextWriter writer, object value, YamlSerializerOptions so)
        {
            WriteYaml(writer, (T)value, so);
        }

        /// <summary>
        /// Determines whether this instance can convert the specified object type.
        /// </summary>
        /// <param name="typeToConvert">The <seealso cref="Type" /> of the object.</param>
        /// <returns><c>true</c> if this instance can convert the specified object type; otherwise, <c>false</c>.</returns>
        public override bool CanConvert(Type typeToConvert)
        {
            return typeof(T).IsAssignableFrom(typeToConvert);
        }
    }

    /// <summary>
    /// Extension methods for the <see cref="YamlConverter"/> class.
    /// </summary>
    [Obsolete("All YAML marshalling has been moved to its own assembly; Cuemon.Extensions.YamlDotNet. This member will be removed with next major version.")]
    public static class YamlConverterExtensions
    {
        /// <summary>
        /// Reads and converts the YAML to type <typeparamref name="T"/>.
        /// </summary>
        /// <typeparam name="T">The type of object or value handled by the converter.</typeparam>
        /// <param name="yc">The <see cref="YamlConverter"/> to extend.</param>
        /// <param name="reader">The reader to read from.</param>
        /// <param name="typeToConvert">The type to convert.</param>
        /// <param name="so">An object that specifies serialization options to use.</param>
        /// <returns>The converted value.</returns>
        public static T ReadYaml<T>(this YamlConverter yc, YamlTextReader reader, Type typeToConvert, YamlSerializerOptions so)
        {
            return (T)yc.ReadYamlCore(reader, typeToConvert, so);
        }

        /// <summary>
        /// Writes a specified <paramref name="value"/> as YAML.
        /// </summary>
        /// <typeparam name="T">The type of object or value handled by the converter.</typeparam>
        /// <param name="yc">The <see cref="YamlConverter"/> to extend.</param>
        /// <param name="writer">The writer to write to.</param>
        /// <param name="value">The value to convert to YAML.</param>
        /// <param name="so">An object that specifies serialization options to use.</param>
        public static void WriteYaml<T>(this YamlConverter yc, YamlTextWriter writer, T value, YamlSerializerOptions so)
        {
            yc.WriteYamlCore(writer, value, so);
        }
    }
}
