using System;
using System.IO;
using Cuemon.Serialization.Formatters;
using Cuemon.Serialization.Xml.Converters;

namespace Cuemon.Serialization.Xml.Formatters
{
    /// <summary>
    /// Serializes and deserializes an object in XML format.
    /// </summary>
    /// <seealso cref="Formatter{TFormat}" />.
    /// <seealso cref="DefaultXmlConverter"/>.
    public class XmlFormatter : Formatter<Stream>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="XmlFormatter"/> class.
        /// </summary>
        public XmlFormatter() : this((Action<XmlFormatterOptions>) null)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="XmlFormatter"/> class.
        /// </summary>
        /// <param name="setup">The <see cref="XmlFormatterOptions"/> which need to be configured.</param>
        public XmlFormatter(Action<XmlFormatterOptions> setup) : this(setup.ConfigureOptions())
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="XmlFormatter"/> class.
        /// </summary>
        /// <param name="options">The configured <see cref="XmlFormatterOptions"/>.</param>
        public XmlFormatter(XmlFormatterOptions options)
        {
            Validator.ThrowIfNull(options, nameof(options));
            Options = options;
            if (options.SynchronizeWithXmlConvert) { options.Settings.ApplyToDefaultSettings(); }
        }

        /// <summary>
        /// Gets the configured options of this <see cref="XmlFormatter"/>.
        /// </summary>
        /// <value>The configured options of this <see cref="XmlFormatter"/>.</value>
        protected XmlFormatterOptions Options { get; }

        /// <summary>
        /// Serializes the specified <paramref name="source"/> to an object of <see cref="Stream"/>.
        /// </summary>
        /// <param name="source">The object to serialize to XML format.</param>
        /// <param name="objectType">The type of the object to serialize.</param>
        /// <returns>A stream of the serialized <paramref name="source"/>.</returns>
        public override Stream Serialize(object source, Type objectType)
        {
            Validator.ThrowIfNull(source, nameof(source));
            Validator.ThrowIfNull(objectType, nameof(objectType));
            var serializer = XmlSerializer.Create(Options.Settings);
            return serializer.Serialize(source, objectType);
        }

        /// <summary>
        /// Deserializes the specified <paramref name="value" /> into an object of <paramref name="objectType"/>.
        /// </summary>
        /// <param name="value">The stream from which to deserialize the object graph.</param>
        /// <param name="objectType">The type of the deserialized object.</param>
        /// <returns>An object of <paramref name="objectType"/>.</returns>
        public override object Deserialize(Stream value, Type objectType)
        {
            Validator.ThrowIfNull(value, nameof(value));
            Validator.ThrowIfNull(objectType, nameof(objectType));
            var serializer = XmlSerializer.Create(Options.Settings);
            return serializer.Deserialize(value, objectType);
        }
    }
}