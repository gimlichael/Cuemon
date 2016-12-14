using System;
using System.IO;
using Cuemon.Serialization.Formatters;

namespace Cuemon.Serialization.Xml.Formatters
{
    /// <summary>
    /// Serializes and deserializes an object, in XML format.
    /// </summary>
    /// <seealso cref="Formatter{TFormat}" />.
    /// <seealso cref="XmlConverter"/>.
    public class XmlFormatter : Formatter<Stream>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="XmlFormatter"/> class.
        /// </summary>
        /// <param name="setup">The <see cref="XmlFormatterOptions"/> which need to be configured.</param>
        public XmlFormatter(Action<XmlFormatterOptions> setup = null)
        {
            Options = setup.ConfigureOptions();
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
        /// <param name="sourceType">The type of the object to serialize.</param>
        /// <returns>A stream of the serialized <paramref name="source"/>.</returns>
        /// <remarks>This method will serialize, in the order specified, using one of either:<br/>
        /// 1. the explicitly defined <see cref="XmlFormatterOptions.WriterFormatter"/> delegate<br/>
        /// 2. the implicit or explicit defined delegate in <see cref="XmlFormatterOptions.WriterFormatters"/> dictionary<br/>
        /// 3. if neither was specified, a default XML writer implementation will be used on <see cref="XmlConverter"/>.
        /// </remarks>
        public override Stream Serialize(object source, Type sourceType)
        {
            Validator.ThrowIfNull(source, nameof(source));
            Validator.ThrowIfNull(sourceType, nameof(sourceType));
            var serializer = Options.Converter;
            if (serializer == null)
            {
                var formatter = Options.ParseWriterFormatter(sourceType);
                serializer = DynamicXmlConverter.Create(formatter, null, options =>
                {
                    options.WriterSettings = Options.WriterSettings;
                    options.RootName = Options.RootName;
                });
            }
            return XmlConvert.SerializeObject(source, serializer);
        }

        /// <summary>
        /// Deserializes the specified <paramref name="value" /> into an object of <paramref name="valueType"/>.
        /// </summary>
        /// <param name="value">The stream from which to deserialize the object graph.</param>
        /// <param name="valueType">The type of the deserialized object.</param>
        /// <returns>An object of <paramref name="valueType"/>.</returns>
        /// <remarks>This method will deserialize, in the order specified, using one of either:<br />
        /// 1. the explicitly defined <see cref="XmlFormatterOptions.ReaderFormatter" /> delegate<br />
        /// 2. the implicit or explicit defined delegate in <see cref="XmlFormatterOptions.ReaderFormatters" /> dictionary<br />
        /// 3. if neither was specified, a default XML reader implementation will be used.</remarks>
        public override object Deserialize(Stream value, Type valueType)
        {
            var serializer = Options.Converter;
            if (serializer == null)
            {
                var formatter = Options.ParseReaderFormatter(valueType);
                serializer = DynamicXmlConverter.Create(null, formatter, options =>
                {
                    options.WriterSettings = Options.WriterSettings;
                    options.RootName = Options.RootName;
                });
            }
            return XmlConvert.DeserializeObject(value, valueType, serializer);
        }
    }
}