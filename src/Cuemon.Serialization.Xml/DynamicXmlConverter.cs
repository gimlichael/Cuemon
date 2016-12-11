using System;
using System.Xml;

namespace Cuemon.Serialization.Xml
{
    /// <summary>
    /// Provides a factory based way to create and wrap an <see cref="XmlConverter"/> implementation.
    /// </summary>
    public static class DynamicXmlConverter
    {
        /// <summary>
        /// Creates a dynamic instance of an <see cref="XmlConverter"/> implementation wrapping <see cref="XmlConverter.WriteXml"/> through <paramref name="writer"/> and <see cref="XmlConverter.ReadXml"/> through <paramref name="reader"/>.
        /// </summary>
        /// <typeparam name="T">The type of the <paramref name="source"/> to implement an <see cref="XmlConverter"/>.</typeparam>
        /// <param name="source">The object that needs support for an <see cref="XmlConverter"/> implementation.</param>
        /// <param name="writer">The delegate that converts <paramref name="source"/> to its JSON representation.</param>
        /// <param name="reader">The delegate that generates <paramref name="source"/> from its JSON representation.</param>
        /// <param name="setup">The <see cref="XmlConverterOptions"/> which need to be configured.</param>
        /// <returns>An <see cref="XmlConverter"/> implementation of <paramref name="source"/>.</returns>
        public static XmlConverter Create<T>(T source, Action<XmlWriter, T> writer = null, Func<XmlReader, Type, T> reader = null, Action<XmlConverterOptions> setup = null)
        {
            Validator.ThrowIfNull(source, nameof(source));
            return new DynamicXmlConverter<T>(source, writer, reader, setup);
        }
    }

    internal class DynamicXmlConverter<T> : XmlConverter
    {
        internal DynamicXmlConverter(T source, Action<XmlWriter, T> writer, Func<XmlReader, Type, T> reader, Action<XmlConverterOptions> setup) : base(setup)
        {
            Source = source;
            Writer = writer;
            Reader = reader;
        }

        private T Source { get; }

        private Action<XmlWriter, T> Writer { get; }

        private Func<XmlReader, Type, T> Reader { get; }

        public override object ReadXml(XmlReader reader, Type valueType)
        {
            return Reader == null ? base.ReadXml(reader, valueType) : Reader.Invoke(reader, valueType);
        }

        public override void WriteXml(XmlWriter writer, object source)
        {
            if (Writer == null)
            {
                base.WriteXml(writer, source);
            }
            else
            {
                Writer(writer, Source);
            }
        }
    }
}