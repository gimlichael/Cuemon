using System;
using System.IO;
using Cuemon.Configuration;
using Cuemon.IO;
using Cuemon.Runtime.Serialization.Formatters;
using Newtonsoft.Json;

namespace Cuemon.Extensions.Newtonsoft.Json.Formatters
{
    /// <summary>
    /// Serializes and deserializes an object, in JSON format.
    /// </summary>
    /// <seealso cref="Formatter{TFormat}" />.
    /// <seealso cref="JsonConverter"/>.
    public class JsonFormatter : Formatter<Stream>, IConfigurable<JsonFormatterOptions>
    {
        /// <summary>
        /// Serializes the specified <paramref name="source"/> to an object of <see cref="string"/>.
        /// </summary>
        /// <param name="source">The object to serialize to JSON format.</param>
        /// <param name="objectType">The type of the object to serialize.</param>
        /// <param name="setup">The <see cref="JsonFormatterOptions"/> which may be configured.</param>
        /// <returns>A string of the serialized <paramref name="source"/>.</returns>
        public static Stream SerializeObject(object source, Type objectType = null, Action<JsonFormatterOptions> setup = null)
        {
            var formatter = new JsonFormatter(setup);
            return formatter.Serialize(source, objectType ?? source?.GetType());
        }

        /// <summary>
        /// Deserializes the specified <paramref name="value"/> into an object of <typeparamref name="T"/>.
        /// </summary>
        /// <typeparam name="T">The type of the object to return.</typeparam>
        /// <param name="value">The object from which to deserialize the object graph.</param>
        /// <param name="setup">The <see cref="JsonFormatterOptions"/> which may be configured.</param>
        /// <returns>An object of <typeparamref name="T" />.</returns>
        public static T DeserializeObject<T>(Stream value, Action<JsonFormatterOptions> setup = null)
        {
            return (T)DeserializeObject(value, typeof(T), setup);
        }

        /// <summary>
        /// Deserializes the specified <paramref name="value" /> into an object of <paramref name="objectType"/>.
        /// </summary>
        /// <param name="value">The string from which to deserialize the object graph.</param>
        /// <param name="objectType">The type of the deserialized object.</param>
        /// <param name="setup">The <see cref="JsonFormatterOptions"/> which may be configured.</param>
        /// <returns>An object of <paramref name="objectType"/>.</returns>
        public static object DeserializeObject(Stream value, Type objectType, Action<JsonFormatterOptions> setup = null)
        {
            var formatter = new JsonFormatter(setup);
            return formatter.Deserialize(value, objectType);
        }

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
        public JsonFormatter(Action<JsonFormatterOptions> setup) : this(Patterns.Configure(setup))
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
        public JsonFormatterOptions Options { get; }

        /// <summary>
        /// Serializes the specified <paramref name="source"/> to an object of <see cref="Stream"/>.
        /// </summary>
        /// <param name="source">The object to serialize to JSON format.</param>
        /// <param name="objectType">The type of the object to serialize.</param>
        /// <returns>A string of the serialized <paramref name="source"/>.</returns>
        public override Stream Serialize(object source, Type objectType)
        {
            Validator.ThrowIfNull(source, nameof(source));
            Validator.ThrowIfNull(objectType, nameof(objectType));

            return StreamFactory.Create(writer =>
            {
                var serializer = Options.SynchronizeWithJsonConvert ? JsonSerializer.CreateDefault() : JsonSerializer.Create(Options.Settings);
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
            var serializer = Options.SynchronizeWithJsonConvert ? JsonSerializer.CreateDefault() : JsonSerializer.Create(Options.Settings);
            var sr = new StreamReader(value, true);
            using (var reader = new JsonTextReader(sr))
            {
                reader.CloseInput = false;
                return serializer.Deserialize(reader, objectType);
            }
        }
    }
}