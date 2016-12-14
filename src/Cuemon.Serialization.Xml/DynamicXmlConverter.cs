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
        /// <typeparam name="T">The type to implement an <see cref="XmlConverter"/>.</typeparam>
        /// <param name="writer">The delegate that converts <typeparamref name="T"/> to its XML representation.</param>
        /// <param name="reader">The delegate that generates <typeparamref name="T"/> from its XML representation.</param>
        /// <param name="setup">The <see cref="XmlConverterOptions"/> which need to be configured.</param>
        /// <returns>An <see cref="XmlConverter"/> implementation of <typeparamref name="T"/>.</returns>
        public static XmlConverter Create<T>(Action<XmlWriter, T> writer = null, Func<XmlReader, Type, T> reader = null, Action<XmlConverterOptions> setup = null)
        {
            var castedWriter = writer == null ? (Action<XmlWriter, object>)null : (w, t) => writer(w, (T)t);
            var castedReader = reader == null ? (Func<XmlReader, Type, object>)null : (r, t) => reader(r, t);
            return Create(castedWriter, castedReader, setup);
        }

        /// <summary>
        /// Creates a dynamic instance of an <see cref="XmlConverter"/> implementation wrapping <see cref="XmlConverter.WriteXml"/> through <paramref name="writer"/> and <see cref="XmlConverter.ReadXml"/> through <paramref name="reader"/>.
        /// </summary>
        /// <param name="writer">The delegate that converts an object to its XML representation.</param>
        /// <param name="reader">The delegate that generates an object from its XML representation.</param>
        /// <param name="setup">The <see cref="XmlConverterOptions"/> which need to be configured.</param>
        /// <returns>An <see cref="XmlConverter"/> implementation of an object.</returns>
        public static XmlConverter Create(Action<XmlWriter, object> writer = null, Func<XmlReader, Type, object> reader = null, Action<XmlConverterOptions> setup = null)
        {
            return new DynamicXmlConverterCore(writer, reader, setup);
        }
    }

    internal class DynamicXmlConverterCore : XmlConverter
    {
        internal DynamicXmlConverterCore(Action<XmlWriter, object> writer, Func<XmlReader, Type, object> reader, Action<XmlConverterOptions> setup) : base(setup)
        {
            Writer = writer;
            Reader = reader;
        }

        private Action<XmlWriter, object> Writer { get; }

        private Func<XmlReader, Type, object> Reader { get; }

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
                Writer(writer, source);
            }
        }
    }
}