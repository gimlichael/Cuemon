using System;
using System.IO;
using Cuemon.Serialization.Formatters;

namespace Cuemon.Serialization.Xml.Formatters
{
    /// <summary>
    /// Serializes and deserializes an object, in XML format.
    /// </summary>
    /// <seealso cref="Formatter{TFormat}" />
    /// <seealso cref="XmlConverter"/>
    public class XmlFormatter : Formatter<Stream>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="XmlFormatter"/> class.
        /// </summary>
        /// <param name="source">The object to serialize and deserialize to XML format.</param>
        /// <param name="setup">The <see cref="XmlFormatterOptions"/> which need to be configured.</param>
        public XmlFormatter(object source, Action<XmlFormatterOptions> setup = null)
            : base(ValidateBase(source), ValidateBase(source.GetType()))
        {
            Options = setup.ConfigureOptions();
        }

        /// <summary>
        /// Gets the configured options of this <see cref="XmlFormatter"/>.
        /// </summary>
        /// <value>The configured options of this <see cref="XmlFormatter"/>.</value>
        protected XmlFormatterOptions Options { get; }

        /// <summary>
        /// Serializes the object of this instance to a <see cref="Stream"/>.
        /// </summary>
        /// <returns>A stream of the serialized <see cref="Formatter{TFormat}.Source"/>.</returns>
        /// <remarks>This method will serialize, in the order specified, using one of either:<br/>
        /// 1. the explicitly defined <see cref="XmlFormatterOptions.WriterFormatter"/> delegate<br/>
        /// 2. the implicit or explicit defined delegate in <see cref="XmlFormatterOptions.WriterFormatters"/> dictionary<br/>
        /// 3. if neither was specified, a default XML writer implementation will be used on <see cref="XmlConverter"/>.
        /// </remarks>
        public override Stream Serialize()
        {
            var serializer = Options.Converter;
            if (serializer == null)
            {
                var writerFormatter = Options.ParseWriterFormatter(SourceType);
                serializer = DynamicXmlConverter.Create(Source, writerFormatter, Options.ReaderFormatter, options =>
                {
                    options.WriterSettings = Options.WriterSettings;
                    options.RootName = Options.RootName;
                });
            }
            return XmlConvert.SerializeObject(Source, serializer);
        }

        /// <summary>
        /// Deserializes the specified <paramref name="value" /> into an object of <typeparamref name="T" />.
        /// </summary>
        /// <typeparam name="T">The type of the object to return.</typeparam>
        /// <param name="value">The stream from which to deserialize the object graph.</param>
        /// <returns>An object of <typeparamref name="T" />.</returns>
        /// <remarks>This method will deserialize, in the order specified, using one of either:<br />
        /// 1. the explicitly defined <see cref="XmlFormatterOptions.ReaderFormatter" /> delegate<br />
        /// 2. the implicit or explicit defined delegate in <see cref="XmlFormatterOptions.ReaderFormatters" /> dictionary<br />
        /// 3. if neither was specified, a default XML reader implementation will be used.</remarks>
        public override T Deserialize<T>(Stream value)
        {
            var serializer = Options.Converter;
            if (serializer == null)
            {
                var readerFormatter = Options.ParseReaderFormatter(SourceType);
                serializer = DynamicXmlConverter.Create(Source, Options.WriterFormatter, readerFormatter, options =>
                {
                    options.WriterSettings = Options.WriterSettings;
                    options.RootName = Options.RootName;
                });
            }
            return XmlConvert.DeserializeObject<T>(value, serializer);
        }
    }
}