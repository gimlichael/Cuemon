using System;
using System.IO;
using Cuemon.IO;
using Cuemon.Runtime.Serialization.Formatters;
using Newtonsoft.Json;

namespace Cuemon.Extensions.Newtonsoft.Json.Formatters
{
    /// <summary>
    /// Serializes and deserializes an object, in JSON format.
    /// </summary>
    /// <seealso cref="StreamFormatter{TOptions}" />.
    /// <seealso cref="JsonConverter"/>.
    public class NewtonsoftJsonFormatter : StreamFormatter<NewtonsoftJsonFormatterOptions>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="NewtonsoftJsonFormatter"/> class.
        /// </summary>
        public NewtonsoftJsonFormatter() : this((Action<NewtonsoftJsonFormatterOptions>) null)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="NewtonsoftJsonFormatter"/> class.
        /// </summary>
        /// <param name="setup">The <see cref="NewtonsoftJsonFormatterOptions"/> which need to be configured.</param>
        public NewtonsoftJsonFormatter(Action<NewtonsoftJsonFormatterOptions> setup) : this(Patterns.Configure(setup))
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="NewtonsoftJsonFormatter"/> class.
        /// </summary>
        /// <param name="options">The configured <see cref="NewtonsoftJsonFormatterOptions"/>.</param>
        public NewtonsoftJsonFormatter(NewtonsoftJsonFormatterOptions options) : base(options)
        {
            if (options.SynchronizeWithJsonConvert) { options.RefreshWithConverterDependencies().ApplyToDefaultSettings(); }
        }

        /// <summary>
        /// Serializes the specified <paramref name="source"/> to an object of <see cref="Stream"/>.
        /// </summary>
        /// <param name="source">The object to serialize to JSON format.</param>
        /// <param name="objectType">The type of the object to serialize.</param>
        /// <returns>A string of the serialized <paramref name="source"/>.</returns>
        public override Stream Serialize(object source, Type objectType)
        {
            Validator.ThrowIfNull(source);
            Validator.ThrowIfNull(objectType);

            return StreamFactory.Create(writer =>
            {
                var serializer = Options.SynchronizeWithJsonConvert ? JsonSerializer.CreateDefault() : JsonSerializer.Create(Options.RefreshWithConverterDependencies());
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
            Validator.ThrowIfNull(value);
            Validator.ThrowIfNull(objectType);
            var serializer = Options.SynchronizeWithJsonConvert ? JsonSerializer.CreateDefault() : JsonSerializer.Create(Options.RefreshWithConverterDependencies());
            var sr = new StreamReader(value, true);
            using (var reader = new JsonTextReader(sr))
            {
                reader.CloseInput = false;
                return serializer.Deserialize(reader, objectType);
            }
        }
    }
}
