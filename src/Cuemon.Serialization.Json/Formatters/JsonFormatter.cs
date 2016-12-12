using System;
using Cuemon.Serialization.Formatters;
using Newtonsoft.Json;

namespace Cuemon.Serialization.Json.Formatters
{
    /// <summary>
    /// Serializes and deserializes an object, in JSON format.
    /// </summary>
    /// <seealso cref="Formatter{TFormat}" />
    /// <seealso cref="JsonConverter"/>
    public class JsonFormatter : Formatter<string>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="JsonFormatter"/> class.
        /// </summary>
        /// <param name="source">The object to serialize and deserialize to XML format.</param>
        /// <param name="setup">The <see cref="JsonFormatterOptions"/> which need to be configured.</param>
        public JsonFormatter(object source, Action<JsonFormatterOptions> setup = null)
            : base(ValidateBase(source), ValidateBase(source.GetType()))
        {
            Options = setup.ConfigureOptions();
        }

        /// <summary>
        /// Gets the configured options of this <see cref="JsonFormatter"/>.
        /// </summary>
        /// <value>The configured options of this <see cref="JsonFormatter"/>.</value>
        protected JsonFormatterOptions Options { get; }

        /// <summary>
        /// Serializes the object of this instance to a <see cref="string"/>.
        /// </summary>
        /// <returns>A string of the serialized <see cref="Formatter{TFormat}.Source"/>.</returns>
        /// <remarks>This method will serialize, in the order specified, using one of either:<br/>
        /// 1. the explicitly defined <see cref="JsonFormatterOptions.WriterFormatter"/> delegate<br/>
        /// 2. the implicit or explicit defined delegate in <see cref="JsonFormatterOptions.WriterFormatters"/> dictionary<br/>
        /// 3. if neither was specified, a default JSON writer implementation will be used on <see cref="JsonConverter"/>.
        /// </remarks>
        public override string Serialize()
        {
            var serializer = Options.Converter;
            if (serializer == null)
            {
                var writerFormatter = Options.ParseWriterFormatter(SourceType);
                serializer = DynamicJsonConverter.Create(Source, writerFormatter, Options.ReaderFormatter);
            }
            Options.Settings.Converters.Add(serializer);
            return JsonConvert.SerializeObject(Source, Options.Settings);
        }

        /// <summary>
        /// Deserializes the specified <paramref name="value" /> into an object of <typeparamref name="T" />.
        /// </summary>
        /// <typeparam name="T">The type of the object to return.</typeparam>
        /// <param name="value">The string from which to deserialize the object graph.</param>
        /// <returns>An object of <typeparamref name="T" />.</returns>
        /// <remarks>This method will deserialize, in the order specified, using one of either:<br />
        /// 1. the explicitly defined <see cref="JsonFormatterOptions.ReaderFormatter" /> delegate<br />
        /// 2. the implicit or explicit defined delegate in <see cref="JsonFormatterOptions.ReaderFormatters" /> dictionary<br />
        /// 3. if neither was specified, a default JSON reader implementation will be used.</remarks>
        public override T Deserialize<T>(string value)
        {
            var serializer = Options.Converter;
            if (serializer == null)
            {
                var readerFormatter = Options.ParseReaderFormatter(SourceType);
                serializer = DynamicJsonConverter.Create(Source, Options.WriterFormatter, readerFormatter);
            }
            Options.Settings.Converters.Add(serializer);
            return JsonConvert.DeserializeObject<T>(value, Options.Settings);
        }

        /// <summary>
        /// Returns a <see cref="string" /> that represents this instance.
        /// </summary>
        /// <returns>A <see cref="string" /> that represents this instance.</returns>
        public override string ToString()
        {
            return Serialize();
        }
    }
}