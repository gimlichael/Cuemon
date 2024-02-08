using System;
using System.IO;
using System.Linq;
using Cuemon.Runtime.Serialization.Converters;
using Cuemon.Text;

namespace Cuemon.Runtime.Serialization
{
    /// <summary>
    /// Provides functionality to serialize objects to YAML and to deserialize YAML into objects.
    /// </summary>
    public class YamlSerializer
    {
        private readonly YamlSerializerOptions _options;

        /// <summary>
        /// Initializes a new instance of the <see cref="YamlSerializer"/> class.
        /// </summary>
        /// <param name="setup">The <see cref="YamlSerializerOptions"/> which may be configured.</param>
        /// <exception cref="ArgumentException">
        /// <paramref name="setup"/> was unable to configure a valid state of the public read-write properties.
        /// </exception>
        public YamlSerializer(Action<YamlSerializerOptions> setup = null) : this(Validator.CheckParameter(() =>
        {
	        Validator.ThrowIfInvalidConfigurator(setup, out var options);
	        return options;
        }))
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="YamlSerializer"/> class.
        /// </summary>
        /// <param name="options">The configured <see cref="YamlSerializerOptions"/>.</param>
        /// <exception cref="ArgumentException">
        /// <paramref name="options"/> are not in a valid state.
        /// </exception>
        public YamlSerializer(YamlSerializerOptions options)
        {
	        Validator.ThrowIfInvalidOptions(options);
	        _options = options;
        }

        /// <summary>
        /// Converts the value of a specified type into a YAML string.
        /// </summary>
        /// <typeparam name="T">The type of the value to serialize.</typeparam>
        /// <param name="source">The value to convert.</param>
        /// <returns>The YAML stream representation of the value.</returns>
        public Stream Serialize<T>(T source)
        {
            return Serialize(source, typeof(T));
        }

        /// <summary>
        /// Converts the value of a specified type into a YAML string.
        /// </summary>
        /// <param name="source">The value to convert.</param>
        /// <param name="inputType">The type of the value to convert.</param>
        /// <returns>The YAML stream representation of the value.</returns>
        public Stream Serialize(object source, Type inputType)
        {
            var ms = new MemoryStream();
            var sw = new StreamWriter(ms, _options.Encoding);
            using (var itw = new YamlTextWriter(sw, Generate.FixedString(' ', _options.WhiteSpaceIndentation)))
            {
                Serialize(itw, source, inputType);
                itw.Flush();
            }
            sw.Flush();
            ms.Flush();
            ms.Position = 0;
            var intermediate = Eradicate.TrailingBytes(ms.ToArray(), Decorator.Enclose(Environment.NewLine).ToByteArray());
            if (_options.Preamble == PreambleSequence.Remove)
            {
                var preamble = _options.Encoding.GetPreamble();
                if (preamble.Length > 0)
                {
                    intermediate = ByteOrderMark.Remove(intermediate, _options.Encoding);
                }
            }
            return new MemoryStream(intermediate);
        }

        internal void Serialize(YamlTextWriter writer, object source)
        {
            Serialize(writer, source, source.GetType());
        }

        internal void Serialize(YamlTextWriter writer, object source, Type inputType)
        {
            (_options.Converters.FirstOrDefault(c => c.CanConvert(inputType)) ?? new DefaultYamlConverter(_options.Converters)).WriteYamlCore(writer, source, _options);
        }

        /// <summary>
        /// Deserializes the specified <paramref name="value"/> into an object of <typeparamref name="T"/>.
        /// </summary>
        /// <typeparam name="T">The type of the object to deserialize.</typeparam>
        /// <param name="value">The object to deserialize from YAML format.</param>
        /// <returns>An object of <typeparamref name="T"/>.</returns>
        public T Deserialize<T>(Stream value)
        {
            return (T)Deserialize(value, typeof(T));
        }

        /// <summary>
        /// Deserializes the specified <paramref name="value" /> into an object of <paramref name="objectType"/>.
        /// </summary>
        /// <param name="value">The object to deserialize from YAML format.</param>
        /// <param name="objectType">The type of the object to deserialize.</param>
        /// <returns>An object of <paramref name="objectType"/>.</returns>
        public object Deserialize(Stream value, Type objectType)
        {
            using (var itr = new YamlTextReader(value, _options.Encoding))
            {
                return (_options.Converters.FirstOrDefault(c => c.CanConvert(objectType)) ?? new DefaultYamlConverter(_options.Converters)).ReadYamlCore(itr, objectType, _options);
            }
        }
    }
}
