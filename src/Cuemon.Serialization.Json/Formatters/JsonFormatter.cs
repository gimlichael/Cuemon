using System;
using System.Globalization;
using System.IO;
using System.Text;
using Cuemon.Serialization.Formatters;
using Newtonsoft.Json;

namespace Cuemon.Serialization.Json.Formatters
{
    /// <summary>
    /// Serializes and deserializes an object, in JSON format.
    /// </summary>
    /// <seealso cref="Formatter{TFormat}" />.
    /// <seealso cref="JsonConverter"/>.
    public class JsonFormatter : Formatter<string>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="JsonFormatter"/> class.
        /// </summary>
        /// <param name="setup">The <see cref="JsonFormatterOptions"/> which need to be configured.</param>
        public JsonFormatter(Action<JsonFormatterOptions> setup = null)
        {
            Options = setup.ConfigureOptions();
        }

        /// <summary>
        /// Gets the configured options of this <see cref="JsonFormatter"/>.
        /// </summary>
        /// <value>The configured options of this <see cref="JsonFormatter"/>.</value>
        protected JsonFormatterOptions Options { get; }

        /// <summary>
        /// Serializes the specified <paramref name="source"/> to an object of <see cref="string"/>.
        /// </summary>
        /// <param name="source">The object to serialize to JSON format.</param>
        /// <param name="sourceType">The type of the object to serialize.</param>
        /// <returns>A string of the serialized <paramref name="source"/>.</returns>
        /// <remarks>This method will serialize, in the order specified, using one of either:<br/>
        /// 1. the explicitly defined <see cref="JsonFormatterOptions.WriterFormatter"/> delegate<br/>
        /// 2. the implicit or explicit defined delegate in <see cref="JsonFormatterOptions.WriterFormatters"/> dictionary<br/>
        /// 3. if neither was specified, a default JSON writer implementation will be used on <see cref="JsonConverter"/>.
        /// </remarks>
        public override string Serialize(object source, Type sourceType)
        {
            Validator.ThrowIfNull(source, nameof(source));
            Validator.ThrowIfNull(sourceType, nameof(sourceType));
            var converter = Options.Converter;
            if (converter == null)
            {
                var formatter = Options.ParseWriterFormatter(sourceType);
                converter = DynamicJsonConverter.Create(sourceType, formatter);
            }

            var serializer = JsonSerializer.Create(Options.Settings); // there is a bug in the JsonConvert.SerializeObject, why we had to make our own implementation
            serializer.Converters.Add(converter);

            StringBuilder sb = new StringBuilder(256);
            StringWriter sw = new StringWriter(sb, CultureInfo.InvariantCulture);
            using (JsonTextWriter jsonWriter = new JsonTextWriter(sw))
            {
                jsonWriter.Formatting = serializer.Formatting;
                serializer.Serialize(jsonWriter, source, sourceType);
            }

            return sw.ToString();
        }

        /// <summary>
        /// Deserializes the specified <paramref name="value" /> into an object of <paramref name="valueType"/>.
        /// </summary>
        /// <param name="value">The string from which to deserialize the object graph.</param>
        /// <param name="valueType">The type of the deserialized object.</param>
        /// <returns>An object of <paramref name="valueType"/>.</returns>
        /// <remarks>This method will deserialize, in the order specified, using one of either:<br />
        /// 1. the explicitly defined <see cref="JsonFormatterOptions.ReaderFormatter" /> delegate<br />
        /// 2. the implicit or explicit defined delegate in <see cref="JsonFormatterOptions.ReaderFormatters" /> dictionary<br />
        /// 3. if neither was specified, a default JSON reader implementation will be used.</remarks>
        public override object Deserialize(string value, Type valueType)
        {
            var serializer = Options.Converter;
            if (serializer == null)
            {
                var formatter = Options.ParseReaderFormatter(valueType);
                serializer = DynamicJsonConverter.Create(valueType, null, formatter);
            }
            Options.Settings.Converters.Add(serializer);
            return JsonConvert.DeserializeObject(value, valueType, Options.Settings);
        }
    }
}