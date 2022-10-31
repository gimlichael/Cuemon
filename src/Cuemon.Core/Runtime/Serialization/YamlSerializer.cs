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
        public YamlSerializer(Action<YamlSerializerOptions> setup = null)
        {
            _options = Patterns.Configure(setup);
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
            if (_options.Preamble == PreambleSequence.Remove)
            {
                var preamble = _options.Encoding.GetPreamble();
                if (preamble.Length > 0)
                {
                    return ByteOrderMark.Remove(ms, _options.Encoding) as MemoryStream;
                }
            }
            return ms;
        }

        internal void Serialize<T>(YamlTextWriter writer, T source)
        {
            Serialize(writer, source, typeof(T));
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
