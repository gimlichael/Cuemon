using System;
using System.IO;
using Cuemon.IO;
using Cuemon.Serialization.Formatters;
using Newtonsoft.Json;

namespace Cuemon.Serialization.Json.Formatters
{
    /// <summary>
    /// Serializes and deserializes an object, in JSON format.
    /// </summary>
    /// <seealso cref="Formatter{TFormat}" />.
    /// <seealso cref="JsonConverter"/>.
    public class JsonFormatter : Formatter<Stream>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="JsonFormatter"/> class.
        /// </summary>
        public JsonFormatter() : this((Action<JsonFormatterOptions>) null)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="JsonFormatter"/> class.
        /// </summary>
        /// <param name="setup">The <see cref="JsonFormatterOptions"/> which need to be configured.</param>
        public JsonFormatter(Action<JsonFormatterOptions> setup) : this(setup.ConfigureOptions())
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="JsonFormatter"/> class.
        /// </summary>
        /// <param name="options">The configured <see cref="JsonFormatterOptions"/>.</param>
        public JsonFormatter(JsonFormatterOptions options)
        {
            Validator.ThrowIfNull(options, nameof(options));
            Options = options;
            if (options.SynchronizeWithJsonConvert) { options.Settings.ApplyToDefaultSettings(); }
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
        /// <param name="objectType">The type of the object to serialize.</param>
        /// <returns>A string of the serialized <paramref name="source"/>.</returns>
        public override Stream Serialize(object source, Type objectType)
        {
            Validator.ThrowIfNull(source, nameof(source));
            Validator.ThrowIfNull(objectType, nameof(objectType));

            var serializer = Options.SynchronizeWithJsonConvert ? JsonSerializer.CreateDefault() : JsonSerializer.Create(Options.Settings); // there is a bug in the JsonConvert.SerializeObject, why we had to make our own implementation
            return StreamWriterUtility.CreateStream(writer =>
            {
                using (var jsonWriter = new JsonTextWriter(writer))
                {
                    jsonWriter.CloseOutput = false;
                    jsonWriter.Formatting = serializer.Formatting;
                    serializer.Serialize(jsonWriter, source, objectType);
                }
            });
        }

        /// <summary>
        /// Deserializes the specified <paramref name="value" /> into an object of <paramref name="objectType"/>.
        /// </summary>
        /// <param name="value">The string from which to deserialize the object graph.</param>
        /// <param name="objectType">The type of the deserialized object.</param>
        /// <returns>An object of <paramref name="objectType"/>.</returns>
        public override object Deserialize(Stream value, Type objectType)
        {
            Validator.ThrowIfNull(value, nameof(value));
            Validator.ThrowIfNull(objectType, nameof(objectType));
            var serializer = JsonSerializer.Create(Options.Settings);
            var sr = new StreamReader(value, true);
            using (JsonTextReader reader = new JsonTextReader(sr))
            {
                reader.CloseInput = false;
                return serializer.Deserialize(reader, objectType);
            }
        }
    }
}