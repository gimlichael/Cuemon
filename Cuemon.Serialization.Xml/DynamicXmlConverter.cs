using System;
using System.Reflection;
using System.Xml;
using Cuemon.Serialization.Xml.Converters;
using Cuemon.Xml.Serialization;

namespace Cuemon.Serialization.Xml
{
    /// <summary>
    /// Provides a factory based way to create and wrap an <see cref="XmlConverter"/> implementation.
    /// </summary>
    public static class DynamicXmlConverter
    {
        /// <summary>
        /// Creates a dynamic instance of an <see cref="XmlConverter" /> implementation wrapping <see cref="XmlConverter.WriteXml" /> through <paramref name="writer" /> and <see cref="XmlConverter.ReadXml" /> through <paramref name="reader" />.
        /// </summary>
        /// <typeparam name="T">The type to implement an <see cref="XmlConverter" />.</typeparam>
        /// <param name="writer">The delegate that converts <typeparamref name="T" /> to its XML representation.</param>
        /// <param name="reader">The delegate that generates <typeparamref name="T" /> from its XML representation.</param>
        /// <param name="canConvertPredicate">The predicate that determines if an <see cref="XmlConverter"/> can convert.</param>
        /// <param name="rootEntity">The optional <seealso cref="XmlQualifiedEntity"/> that will provide the name of the root element.</param>
        /// <returns>An <see cref="XmlConverter" /> implementation of <typeparamref name="T" />.</returns>
        public static XmlConverter Create<T>(Action<XmlWriter, T, XmlQualifiedEntity> writer = null, Func<XmlReader, Type, T> reader = null, Func<Type, bool> canConvertPredicate = null, XmlQualifiedEntity rootEntity = null)
        {
            var castedWriter = writer == null ? (Action<XmlWriter, object, XmlQualifiedEntity>)null : (w, t, q) => writer(w, (T)t, q);
            var castedReader = reader == null ? (Func<XmlReader, Type, object>)null : (r, t) => reader(r, t);
            return Create(typeof(T), castedWriter, castedReader, canConvertPredicate, rootEntity);
        }

        /// <summary>
        /// Creates a dynamic instance of an <see cref="XmlConverter" /> implementation wrapping <see cref="XmlConverter.WriteXml" /> through <paramref name="writer" /> and <see cref="XmlConverter.ReadXml" /> through <paramref name="reader" />.
        /// </summary>
        /// <param name="objectType">The type of the object to make convertible.</param>
        /// <param name="writer">The delegate that converts an object to its XML representation.</param>
        /// <param name="reader">The delegate that generates an object from its XML representation.</param>
        /// <param name="canConvertPredicate">The predicate that determines if an <see cref="XmlConverter" /> can convert.</param>
        /// <param name="rootEntity">The optional <seealso cref="XmlQualifiedEntity"/> that will provide the name of the root element.</param>
        /// <returns>An <see cref="XmlConverter" /> implementation of an object.</returns>
        public static XmlConverter Create(Type objectType, Action<XmlWriter, object, XmlQualifiedEntity> writer = null, Func<XmlReader, Type, object> reader = null, Func<Type, bool> canConvertPredicate = null, XmlQualifiedEntity rootEntity = null)
        {
            return new DynamicXmlConverterCore(objectType, writer, reader, canConvertPredicate, rootEntity);
        }
    }

    internal class DynamicXmlConverterCore : XmlConverter
    {
        internal DynamicXmlConverterCore(Type objectType, Action<XmlWriter, object, XmlQualifiedEntity> writer, Func<XmlReader, Type, object> reader, Func<Type, bool> secondaryCanConvertPredicate, XmlQualifiedEntity rootName)
        {
            RootName = rootName;
            ObjectType = objectType;
            Writer = writer;
            Reader = reader;
            CanConvertPredicate = secondaryCanConvertPredicate;
        }

        internal XmlQualifiedEntity RootName { get; set; }

        private Func<Type, bool> CanConvertPredicate { get; }

        private Type ObjectType { get; set; }

        private Action<XmlWriter, object, XmlQualifiedEntity> Writer { get; }

        private Func<XmlReader, Type, object> Reader { get; }

        public override object ReadXml(XmlReader reader, Type objectType)
        {
            if (Reader == null) { throw new NotImplementedException("Delegate reader is null."); }
            return Reader.Invoke(reader, objectType);
        }

        public override bool CanConvert(Type objectType)
        {
            if (CanConvertPredicate != null)
            {
                return ObjectType.IsAssignableFrom(objectType) && CanConvertPredicate(objectType);
            }
            return ObjectType.IsAssignableFrom(objectType);
        }

        public override void WriteXml(XmlWriter writer, object value, XmlQualifiedEntity elementName = null)
        {
            if (Writer == null) { throw new NotImplementedException("Delegate writer is null."); }
            Writer.Invoke(writer, value, elementName ?? RootName);
        }

        public override bool CanRead => Reader != null;

        public override bool CanWrite => Writer != null;
    }
}