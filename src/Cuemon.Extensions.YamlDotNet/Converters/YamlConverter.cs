using System;
using Cuemon.Extensions.YamlDotNet.Formatters;
using YamlDotNet.Core;
using YamlDotNet.Serialization;

namespace Cuemon.Extensions.YamlDotNet.Converters
{
    /// <summary>
    /// Converts an object to and from YAML (YAML ain't markup language).
    /// </summary>
    public abstract class YamlConverter : IYamlTypeConverter
    {
        internal abstract void WriteYamlCore(IEmitter writer, object value);

        internal abstract object ReadYamlCore(IParser reader, Type typeToConvert);

        /// <summary>
        /// Determines whether this instance can convert the specified object type.
        /// </summary>
        /// <param name="typeToConvert">The <seealso cref="Type"/> of the object.</param>
        /// <returns><c>true</c> if this instance can convert the specified object type; otherwise, <c>false</c>.</returns>
        public abstract bool CanConvert(Type typeToConvert);

        /// <summary>
        /// Gets or sets the <see cref="YamlFormatterOptions"/> associated with this instance. Normally this is done from <see cref="Formatters.YamlFormatter"/>
        /// </summary>
        /// <value>The <see cref="YamlFormatterOptions"/> associated with this instance.</value>
        public YamlFormatterOptions FormatterOptions { get; set; }

        /// <summary>
        /// Returns the specified <paramref name="name"/> adhering to the underlying <see cref="YamlSerializerOptions.NamingConvention"/> policy on <see cref="FormatterOptions"/>.
        /// </summary>
        /// <param name="name">The name of the property.</param>
        /// <returns>If <see cref="FormatterOptions"/> is null, the specified <paramref name="name"/> is returned unaltered; otherwise it is converted according to the <see cref="YamlSerializerOptions.NamingConvention"/>.</returns>
        public string SetPropertyName(string name)
        {
            Validator.ThrowIfNull(name);
            return FormatterOptions?.Settings.NamingConvention.Apply(name) ?? name;
        }

        bool IYamlTypeConverter.Accepts(Type type)
        {
            return CanConvert(type);
        }

        object IYamlTypeConverter.ReadYaml(IParser parser, Type type)
        {
            return ReadYamlCore(parser, type);
        }

        void IYamlTypeConverter.WriteYaml(IEmitter emitter, object value, Type type) // odd decision with type parameter
        {
            WriteYamlCore(emitter, value);
        }
    }

    /// <summary>
    /// Converts an object to or from YAML (YAML ain't markup language).
    /// </summary>
    /// <typeparam name="T">The type of object or value handled by the converter.</typeparam>
    /// <seealso cref="YamlConverter" />
    public abstract class YamlConverter<T> : YamlConverter
    {
        /// <summary>
        /// Writes a specified <paramref name="value"/> as YAML.
        /// </summary>
        /// <param name="writer">The writer to write to.</param>
        /// <param name="value">The value to convert to YAML.</param>
        public abstract void WriteYaml(IEmitter writer, T value);

        /// <summary>
        /// Reads and converts the YAML to type <typeparamref name="T"/>.
        /// </summary>
        /// <param name="reader">The reader to read from.</param>
        /// <param name="typeToConvert">The type to convert.</param>
        /// <returns>The converted value.</returns>
        public abstract T ReadYaml(IParser reader, Type typeToConvert);

        internal override object ReadYamlCore(IParser reader, Type typeToConvert)
        {
            return ReadYaml(reader, typeToConvert);
        }

        internal override void WriteYamlCore(IEmitter writer, object value)
        {
            WriteYaml(writer, (T)value);
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
}
