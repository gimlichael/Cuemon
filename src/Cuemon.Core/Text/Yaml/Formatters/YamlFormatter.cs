using System;
using System.IO;
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
    [Obsolete("All YAML marshalling has been moved to its own assembly; Cuemon.Extensions.YamlDotNet. This member will be removed with next major version.")]
    public class YamlFormatter : StreamFormatter<YamlFormatterOptions>
    {
	    /// <summary>
	    /// Initializes a new instance of the <see cref="YamlFormatter"/> class.
	    /// </summary>
	    public YamlFormatter() : this((Action<YamlFormatterOptions>) null)
	    {
	    }

	    /// <summary>
	    /// Initializes a new instance of the <see cref="YamlFormatter"/> class.
	    /// </summary>
	    /// <param name="setup">The <see cref="YamlFormatterOptions"/> which need to be configured.</param>
	    public YamlFormatter(Action<YamlFormatterOptions> setup) : this(Patterns.Configure(setup))
	    {
	    }

	    /// <summary>
	    /// Initializes a new instance of the <see cref="YamlFormatter"/> class.
	    /// </summary>
	    /// <param name="options">The configured <see cref="YamlFormatterOptions"/>.</param>
	    public YamlFormatter(YamlFormatterOptions options) : base(options)
	    {
		    Options.RefreshWithConverterDependencies();
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
            var serializer = new YamlSerializer(Options.Settings);
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
            var serializer = new YamlSerializer(Options.Settings);
            return serializer.Deserialize(value, objectType);
        }
    }
}
