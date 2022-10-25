using System;
using System.IO;
using Cuemon.Configuration;
using Cuemon.Runtime.Serialization;
using Cuemon.Runtime.Serialization.Formatters;
using Cuemon.Text.Yaml.Converters;

namespace Cuemon.Text.Yaml.Formatters
{
    /// <summary>
    /// Serializes and deserializes an object, in YAML format.
    /// </summary>
    /// <seealso cref="Formatter{TFormat}" />.
    /// <seealso cref="YamlConverter"/>.
    public sealed class YamlFormatter : Formatter<Stream>, IConfigurable<YamlFormatterOptions>
    {
        /// <summary>
        /// Serializes the specified <paramref name="source"/> to an object of <see cref="Stream"/>.
        /// </summary>
        /// <param name="source">The object to serialize to YAML format.</param>
        /// <param name="objectType">The type of the object to serialize.</param>
        /// <param name="setup">The <see cref="YamlFormatterOptions"/> which may be configured.</param>
        /// <returns>A <see cref="Stream"/> of the serialized <paramref name="source"/>.</returns>
        public static Stream SerializeObject(object source, Type objectType = null, Action<YamlFormatterOptions> setup = null)
        {
            var formatter = new YamlFormatter(setup);
            return formatter.Serialize(source, objectType ?? source?.GetType());
        }

        /// <summary>
        /// Deserializes the specified <paramref name="value"/> into an object of <typeparamref name="T"/>.
        /// </summary>
        /// <typeparam name="T">The type of the object to return.</typeparam>
        /// <param name="value">The object from which to deserialize the object graph.</param>
        /// <param name="setup">The <see cref="YamlFormatterOptions"/> which may be configured.</param>
        /// <returns>An object of <typeparamref name="T" />.</returns>
        public static T DeserializeObject<T>(Stream value, Action<YamlFormatterOptions> setup = null)
        {
            return (T)DeserializeObject(value, typeof(T), setup);
        }

        /// <summary>
        /// Deserializes the specified <paramref name="value" /> into an object of <paramref name="objectType"/>.
        /// </summary>
        /// <param name="value">The string from which to deserialize the object graph.</param>
        /// <param name="objectType">The type of the deserialized object.</param>
        /// <param name="setup">The <see cref="YamlFormatterOptions"/> which may be configured.</param>
        /// <returns>An object of <paramref name="objectType"/>.</returns>
        public static object DeserializeObject(Stream value, Type objectType, Action<YamlFormatterOptions> setup = null)
        {
            var formatter = new YamlFormatter(setup);
            return formatter.Deserialize(value, objectType);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="YamlFormatter"/> class.
        /// </summary>
        /// <param name="setup">The <see cref="YamlFormatterOptions"/> which may be configured.</param>
        public YamlFormatter(Action<YamlFormatterOptions> setup = null)
        {
            Options = Patterns.Configure(setup);
        }

        /// <summary>
        /// Serializes the specified <paramref name="source"/> to an object of <see cref="Stream"/>.
        /// </summary>
        /// <param name="source">The object to serialize to YAML format.</param>
        /// <param name="objectType">The type of the object to serialize.</param>
        /// <returns>A stream of the serialized <paramref name="source"/>.</returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="source"/> is null -or-
        /// <paramref name="objectType"/> is null.
        /// </exception>
        public override Stream Serialize(object source, Type objectType)
        {
            Validator.ThrowIfNull(source);
            Validator.ThrowIfNull(objectType);
            var serializer = new YamlSerializer(Patterns.ConfigureRevert(Options.Settings));
            return serializer.Serialize(source, objectType);
        }

        /// <summary>
        /// Deserializes the specified <paramref name="value" /> into an object of <paramref name="objectType"/>.
        /// </summary>
        /// <param name="value">The object from which to deserialize the object graph.</param>
        /// <param name="objectType">The type of the deserialized object.</param>
        /// <returns>An object of <paramref name="objectType" />.</returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="value"/> is null -or-
        /// <paramref name="objectType"/> is null.
        /// </exception>
        public override object Deserialize(Stream value, Type objectType)
        {
            Validator.ThrowIfNull(value);
            Validator.ThrowIfNull(objectType);
            var serializer = new YamlSerializer(Patterns.ConfigureRevert(Options.Settings));
            return serializer.Deserialize(value, objectType);
        }

        /// <summary>
        /// Gets the configured options of this instance.
        /// </summary>
        /// <value>The configured options of this instance.</value>
        public YamlFormatterOptions Options { get; }
    }
}
