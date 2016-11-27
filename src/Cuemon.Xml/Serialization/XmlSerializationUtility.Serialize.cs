using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Xml;
using System.Xml.Serialization;
using Cuemon.Collections.Generic;
using Cuemon.Reflection;

namespace Cuemon.Xml.Serialization
{
    public static partial class XmlSerializationUtility
    {
        /// <summary>
        /// Creates and returns a XML stream representation of the specified <paramref name="serializable"/> object.
        /// </summary>
        /// <param name="serializable">The object to serialize.</param>
        /// <returns>A <see cref="Stream"/> holding the serialized version of <paramref name="serializable"/>.</returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="serializable"/> is null.
        /// </exception>
        public static Stream Serialize(object serializable)
        {
            return Serialize(serializable, false);
        }

        /// <summary>
        /// Creates and returns a XML stream representation of the specified <paramref name="serializable"/> object.
        /// </summary>
        /// <param name="serializable">The object to serialize.</param>
        /// <param name="omitXmlDeclaration">if set to <c>true</c> omit the XML declaration; otherwise <c>false</c>. The default is false.</param>
        /// <returns>A <see cref="Stream"/> holding the serialized version of <paramref name="serializable"/>.</returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="serializable"/> is null.
        /// </exception>
        public static Stream Serialize(object serializable, bool omitXmlDeclaration)
        {
            return Serialize(serializable, omitXmlDeclaration, null);
        }

        /// <summary>
        /// Creates and returns a XML stream representation of the specified <paramref name="serializable"/> object.
        /// </summary>
        /// <param name="serializable">The object to serialize.</param>
        /// <param name="omitXmlDeclaration">if set to <c>true</c> omit the XML declaration; otherwise <c>false</c>. The default is false.</param>
        /// <param name="qualifiedRootEntity">A <see cref="XmlQualifiedEntity"/> that overrides and represents the fully qualified name of the XML root element.</param>
        /// <returns>A <see cref="Stream"/> holding the serialized version of <paramref name="serializable"/>.</returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="serializable"/> is null.
        /// </exception>
        public static Stream Serialize(object serializable, bool omitXmlDeclaration, XmlQualifiedEntity qualifiedRootEntity)
        {
            return Serialize(serializable, SerializableOrder.Append, omitXmlDeclaration, qualifiedRootEntity, null);
        }

        /// <summary>
        /// Creates and returns a XML stream representation of the specified <paramref name="serializable"/> object while complementary XML is applied to <paramref name="serializable"/>.
        /// </summary>
        /// <param name="serializable">The object to serialize.</param>
        /// <param name="order">A <see cref="SerializableOrder"/> that specifies the order (append or prepend) in which the additional serialization information is applied to the <paramref name="serializable"/> object.</param>
        /// <param name="writer">The delegate that will either prepend or append complementary XML to <paramref name="serializable"/>.</param>
        /// <returns>A <see cref="Stream"/> holding the serialized version of <paramref name="serializable"/>.</returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="serializable"/> is null.
        /// </exception>
        public static Stream Serialize(object serializable, SerializableOrder order, Action<XmlWriter> writer)
        {
            return Serialize(serializable, order, false, writer);
        }

        /// <summary>
        /// Creates and returns a XML stream representation of the specified <paramref name="serializable"/> object while complementary XML is applied to <paramref name="serializable"/>.
        /// </summary>
        /// <param name="serializable">The object to serialize.</param>
        /// <param name="order">A <see cref="SerializableOrder"/> that specifies the order (append or prepend) in which the additional serialization information is applied to the <paramref name="serializable"/> object.</param>
        /// <param name="omitXmlDeclaration">if set to <c>true</c> omit the XML declaration; otherwise <c>false</c>. The default is false.</param>
        /// <param name="writer">The delegate that will either prepend or append complementary XML to <paramref name="serializable"/>.</param>
        /// <returns>A <see cref="Stream"/> holding the serialized version of <paramref name="serializable"/>.</returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="serializable"/> is null.
        /// </exception>
        public static Stream Serialize(object serializable, SerializableOrder order, bool omitXmlDeclaration, Action<XmlWriter> writer)
        {
            return Serialize(serializable, order, omitXmlDeclaration, (XmlQualifiedEntity)null, writer);
        }

        /// <summary>
        /// Creates and returns a XML stream representation of the specified <paramref name="serializable"/> object while complementary XML is applied to <paramref name="serializable"/>.
        /// </summary>
        /// <param name="serializable">The object to serialize.</param>
        /// <param name="order">A <see cref="SerializableOrder"/> that specifies the order (append or prepend) in which the additional serialization information is applied to the <paramref name="serializable"/> object.</param>
        /// <param name="omitXmlDeclaration">if set to <c>true</c> omit the XML declaration; otherwise <c>false</c>. The default is false.</param>
        /// <param name="qualifiedRootEntity">A <see cref="XmlQualifiedEntity"/> that overrides and represents the fully qualified name of the XML root element.</param>
        /// <param name="writer">The delegate that will either prepend or append complementary XML to <paramref name="serializable"/>.</param>
        /// <returns>A <see cref="Stream"/> holding the serialized version of <paramref name="serializable"/>.</returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="serializable"/> is null.
        /// </exception>
        public static Stream Serialize(object serializable, SerializableOrder order, bool omitXmlDeclaration, XmlQualifiedEntity qualifiedRootEntity, Action<XmlWriter> writer)
        {
            Validator.ThrowIfNull(serializable, nameof(serializable));
            var factory = ActionFactory.Create(writer, null);
            return SerializeCore(factory, serializable, order, omitXmlDeclaration, qualifiedRootEntity);
        }

        /// <summary>
        /// Creates and returns a XML stream representation of the specified <paramref name="serializable"/> object while complementary XML is applied to <paramref name="serializable"/>.
        /// </summary>
        /// <typeparam name="T">The type of the first parameter of the delegate <paramref name="writer"/>.</typeparam>
        /// <param name="serializable">The object to serialize.</param>
        /// <param name="order">A <see cref="SerializableOrder"/> that specifies the order (append or prepend) in which the additional serialization information is applied to the <paramref name="serializable"/> object.</param>
        /// <param name="writer">The delegate that will either prepend or append complementary XML to <paramref name="serializable"/>.</param>
        /// <param name="arg">The first parameter of the delegate <paramref name="writer"/>.</param>
        /// <returns>A <see cref="Stream"/> holding the serialized version of <paramref name="serializable"/>.</returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="serializable"/> is null.
        /// </exception>
        public static Stream Serialize<T>(object serializable, SerializableOrder order, Action<XmlWriter, T> writer, T arg)
        {
            return Serialize(serializable, order, false, writer, arg);
        }

        /// <summary>
        /// Creates and returns a XML stream representation of the specified <paramref name="serializable"/> object while complementary XML is applied to <paramref name="serializable"/>.
        /// </summary>
        /// <typeparam name="T">The type of the first parameter of the delegate <paramref name="writer"/>.</typeparam>
        /// <param name="serializable">The object to serialize.</param>
        /// <param name="order">A <see cref="SerializableOrder"/> that specifies the order (append or prepend) in which the additional serialization information is applied to the <paramref name="serializable"/> object.</param>
        /// <param name="omitXmlDeclaration">if set to <c>true</c> omit the XML declaration; otherwise <c>false</c>. The default is false.</param>
        /// <param name="writer">The delegate that will either prepend or append complementary XML to <paramref name="serializable"/>.</param>
        /// <param name="arg">The first parameter of the delegate <paramref name="writer"/>.</param>
        /// <returns>A <see cref="Stream"/> holding the serialized version of <paramref name="serializable"/>.</returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="serializable"/> is null.
        /// </exception>
        public static Stream Serialize<T>(object serializable, SerializableOrder order, bool omitXmlDeclaration, Action<XmlWriter, T> writer, T arg)
        {
            return Serialize(serializable, order, omitXmlDeclaration, (XmlQualifiedEntity)null, writer, arg);
        }

        /// <summary>
        /// Creates and returns a XML stream representation of the specified <paramref name="serializable"/> object while complementary XML is applied to <paramref name="serializable"/>.
        /// </summary>
        /// <typeparam name="T">The type of the first parameter of the delegate <paramref name="writer"/>.</typeparam>
        /// <param name="serializable">The object to serialize.</param>
        /// <param name="order">A <see cref="SerializableOrder"/> that specifies the order (append or prepend) in which the additional serialization information is applied to the <paramref name="serializable"/> object.</param>
        /// <param name="omitXmlDeclaration">if set to <c>true</c> omit the XML declaration; otherwise <c>false</c>. The default is false.</param>
        /// <param name="qualifiedRootEntity">A <see cref="XmlQualifiedEntity"/> that overrides and represents the fully qualified name of the XML root element.</param>
        /// <param name="writer">The delegate that will either prepend or append complementary XML to <paramref name="serializable"/>.</param>
        /// <param name="arg">The first parameter of the delegate <paramref name="writer"/>.</param>
        /// <returns>A <see cref="Stream"/> holding the serialized version of <paramref name="serializable"/>.</returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="serializable"/> is null.
        /// </exception>
        public static Stream Serialize<T>(object serializable, SerializableOrder order, bool omitXmlDeclaration, XmlQualifiedEntity qualifiedRootEntity, Action<XmlWriter, T> writer, T arg)
        {
            Validator.ThrowIfNull(serializable, nameof(serializable));
            var factory = ActionFactory.Create(writer, null, arg);
            return SerializeCore(factory, serializable, order, omitXmlDeclaration, qualifiedRootEntity);
        }

        /// <summary>
        /// Creates and returns a XML stream representation of the specified <paramref name="serializable"/> object while complementary XML is applied to <paramref name="serializable"/>.
        /// </summary>
        /// <typeparam name="T1">The type of the first parameter of the delegate <paramref name="writer"/>.</typeparam>
        /// <typeparam name="T2">The type of the second parameter of the delegate <paramref name="writer"/>.</typeparam>
        /// <param name="serializable">The object to serialize.</param>
        /// <param name="order">A <see cref="SerializableOrder"/> that specifies the order (append or prepend) in which the additional serialization information is applied to the <paramref name="serializable"/> object.</param>
        /// <param name="writer">The delegate that will either prepend or append complementary XML to <paramref name="serializable"/>.</param>
        /// <param name="arg1">The first parameter of the delegate <paramref name="writer"/>.</param>
        /// <param name="arg2">The second parameter of the delegate <paramref name="writer"/>.</param>
        /// <returns>A <see cref="Stream"/> holding the serialized version of <paramref name="serializable"/>.</returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="serializable"/> is null.
        /// </exception>
        public static Stream Serialize<T1, T2>(object serializable, SerializableOrder order, Action<XmlWriter, T1, T2> writer, T1 arg1, T2 arg2)
        {
            return Serialize(serializable, order, false, writer, arg1, arg2);
        }

        /// <summary>
        /// Creates and returns a XML stream representation of the specified <paramref name="serializable"/> object while complementary XML is applied to <paramref name="serializable"/>.
        /// </summary>
        /// <typeparam name="T1">The type of the first parameter of the delegate <paramref name="writer"/>.</typeparam>
        /// <typeparam name="T2">The type of the second parameter of the delegate <paramref name="writer"/>.</typeparam>
        /// <param name="serializable">The object to serialize.</param>
        /// <param name="order">A <see cref="SerializableOrder"/> that specifies the order (append or prepend) in which the additional serialization information is applied to the <paramref name="serializable"/> object.</param>
        /// <param name="omitXmlDeclaration">if set to <c>true</c> omit the XML declaration; otherwise <c>false</c>. The default is false.</param>
        /// <param name="writer">The delegate that will either prepend or append complementary XML to <paramref name="serializable"/>.</param>
        /// <param name="arg1">The first parameter of the delegate <paramref name="writer"/>.</param>
        /// <param name="arg2">The second parameter of the delegate <paramref name="writer"/>.</param>
        /// <returns>A <see cref="Stream"/> holding the serialized version of <paramref name="serializable"/>.</returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="serializable"/> is null.
        /// </exception>
        public static Stream Serialize<T1, T2>(object serializable, SerializableOrder order, bool omitXmlDeclaration, Action<XmlWriter, T1, T2> writer, T1 arg1, T2 arg2)
        {
            return Serialize(serializable, order, omitXmlDeclaration, (XmlQualifiedEntity)null, writer, arg1, arg2);
        }

        /// <summary>
        /// Creates and returns a XML stream representation of the specified <paramref name="serializable"/> object while complementary XML is applied to <paramref name="serializable"/>.
        /// </summary>
        /// <typeparam name="T1">The type of the first parameter of the delegate <paramref name="writer"/>.</typeparam>
        /// <typeparam name="T2">The type of the second parameter of the delegate <paramref name="writer"/>.</typeparam>
        /// <param name="serializable">The object to serialize.</param>
        /// <param name="order">A <see cref="SerializableOrder"/> that specifies the order (append or prepend) in which the additional serialization information is applied to the <paramref name="serializable"/> object.</param>
        /// <param name="omitXmlDeclaration">if set to <c>true</c> omit the XML declaration; otherwise <c>false</c>. The default is false.</param>
        /// <param name="qualifiedRootEntity">A <see cref="XmlQualifiedEntity"/> that overrides and represents the fully qualified name of the XML root element.</param>
        /// <param name="writer">The delegate that will either prepend or append complementary XML to <paramref name="serializable"/>.</param>
        /// <param name="arg1">The first parameter of the delegate <paramref name="writer"/>.</param>
        /// <param name="arg2">The second parameter of the delegate <paramref name="writer"/>.</param>
        /// <returns>A <see cref="Stream"/> holding the serialized version of <paramref name="serializable"/>.</returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="serializable"/> is null.
        /// </exception>
        public static Stream Serialize<T1, T2>(object serializable, SerializableOrder order, bool omitXmlDeclaration, XmlQualifiedEntity qualifiedRootEntity, Action<XmlWriter, T1, T2> writer, T1 arg1, T2 arg2)
        {
            Validator.ThrowIfNull(serializable, nameof(serializable));
            var factory = ActionFactory.Create(writer, null, arg1, arg2);
            return SerializeCore(factory, serializable, order, omitXmlDeclaration, qualifiedRootEntity);
        }

        /// <summary>
        /// Creates and returns a XML stream representation of the specified <paramref name="serializable"/> object while complementary XML is applied to <paramref name="serializable"/>.
        /// </summary>
        /// <typeparam name="T1">The type of the first parameter of the delegate <paramref name="writer"/>.</typeparam>
        /// <typeparam name="T2">The type of the second parameter of the delegate <paramref name="writer"/>.</typeparam>
        /// <typeparam name="T3">The type of the third parameter of the delegate <paramref name="writer"/>.</typeparam>
        /// <param name="serializable">The object to serialize.</param>
        /// <param name="order">A <see cref="SerializableOrder"/> that specifies the order (append or prepend) in which the additional serialization information is applied to the <paramref name="serializable"/> object.</param>
        /// <param name="writer">The delegate that will either prepend or append complementary XML to <paramref name="serializable"/>.</param>
        /// <param name="arg1">The first parameter of the delegate <paramref name="writer"/>.</param>
        /// <param name="arg2">The second parameter of the delegate <paramref name="writer"/>.</param>
        /// <param name="arg3">The third parameter of the delegate <paramref name="writer"/>.</param>
        /// <returns>A <see cref="Stream"/> holding the serialized version of <paramref name="serializable"/>.</returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="serializable"/> is null.
        /// </exception>
        public static Stream Serialize<T1, T2, T3>(object serializable, SerializableOrder order, Action<XmlWriter, T1, T2, T3> writer, T1 arg1, T2 arg2, T3 arg3)
        {
            return Serialize(serializable, order, false, writer, arg1, arg2, arg3);
        }

        /// <summary>
        /// Creates and returns a XML stream representation of the specified <paramref name="serializable"/> object while complementary XML is applied to <paramref name="serializable"/>.
        /// </summary>
        /// <typeparam name="T1">The type of the first parameter of the delegate <paramref name="writer"/>.</typeparam>
        /// <typeparam name="T2">The type of the second parameter of the delegate <paramref name="writer"/>.</typeparam>
        /// <typeparam name="T3">The type of the third parameter of the delegate <paramref name="writer"/>.</typeparam>
        /// <param name="serializable">The object to serialize.</param>
        /// <param name="order">A <see cref="SerializableOrder"/> that specifies the order (append or prepend) in which the additional serialization information is applied to the <paramref name="serializable"/> object.</param>
        /// <param name="omitXmlDeclaration">if set to <c>true</c> omit the XML declaration; otherwise <c>false</c>. The default is false.</param>
        /// <param name="writer">The delegate that will either prepend or append complementary XML to <paramref name="serializable"/>.</param>
        /// <param name="arg1">The first parameter of the delegate <paramref name="writer"/>.</param>
        /// <param name="arg2">The second parameter of the delegate <paramref name="writer"/>.</param>
        /// <param name="arg3">The third parameter of the delegate <paramref name="writer"/>.</param>
        /// <returns>A <see cref="Stream"/> holding the serialized version of <paramref name="serializable"/>.</returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="serializable"/> is null.
        /// </exception>
        public static Stream Serialize<T1, T2, T3>(object serializable, SerializableOrder order, bool omitXmlDeclaration, Action<XmlWriter, T1, T2, T3> writer, T1 arg1, T2 arg2, T3 arg3)
        {
            return Serialize(serializable, order, omitXmlDeclaration, (XmlQualifiedEntity)null, writer, arg1, arg2, arg3);
        }

        /// <summary>
        /// Creates and returns a XML stream representation of the specified <paramref name="serializable"/> object while complementary XML is applied to <paramref name="serializable"/>.
        /// </summary>
        /// <typeparam name="T1">The type of the first parameter of the delegate <paramref name="writer"/>.</typeparam>
        /// <typeparam name="T2">The type of the second parameter of the delegate <paramref name="writer"/>.</typeparam>
        /// <typeparam name="T3">The type of the third parameter of the delegate <paramref name="writer"/>.</typeparam>
        /// <param name="serializable">The object to serialize.</param>
        /// <param name="order">A <see cref="SerializableOrder"/> that specifies the order (append or prepend) in which the additional serialization information is applied to the <paramref name="serializable"/> object.</param>
        /// <param name="omitXmlDeclaration">if set to <c>true</c> omit the XML declaration; otherwise <c>false</c>. The default is false.</param>
        /// <param name="qualifiedRootEntity">A <see cref="XmlQualifiedEntity"/> that overrides and represents the fully qualified name of the XML root element.</param>
        /// <param name="writer">The delegate that will either prepend or append complementary XML to <paramref name="serializable"/>.</param>
        /// <param name="arg1">The first parameter of the delegate <paramref name="writer"/>.</param>
        /// <param name="arg2">The second parameter of the delegate <paramref name="writer"/>.</param>
        /// <param name="arg3">The third parameter of the delegate <paramref name="writer"/>.</param>
        /// <returns>A <see cref="Stream"/> holding the serialized version of <paramref name="serializable"/>.</returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="serializable"/> is null.
        /// </exception>
        public static Stream Serialize<T1, T2, T3>(object serializable, SerializableOrder order, bool omitXmlDeclaration, XmlQualifiedEntity qualifiedRootEntity, Action<XmlWriter, T1, T2, T3> writer, T1 arg1, T2 arg2, T3 arg3)
        {
            Validator.ThrowIfNull(serializable, nameof(serializable));
            var factory = ActionFactory.Create(writer, null, arg1, arg2, arg3);
            return SerializeCore(factory, serializable, order, omitXmlDeclaration, qualifiedRootEntity);
        }

        /// <summary>
        /// Creates and returns a XML stream representation of the specified <paramref name="serializable"/> object while complementary XML is applied to <paramref name="serializable"/>.
        /// </summary>
        /// <typeparam name="T1">The type of the first parameter of the delegate <paramref name="writer"/>.</typeparam>
        /// <typeparam name="T2">The type of the second parameter of the delegate <paramref name="writer"/>.</typeparam>
        /// <typeparam name="T3">The type of the third parameter of the delegate <paramref name="writer"/>.</typeparam>
        /// <typeparam name="T4">The type of the fourth parameter of the delegate <paramref name="writer"/>.</typeparam>
        /// <param name="serializable">The object to serialize.</param>
        /// <param name="order">A <see cref="SerializableOrder"/> that specifies the order (append or prepend) in which the additional serialization information is applied to the <paramref name="serializable"/> object.</param>
        /// <param name="writer">The delegate that will either prepend or append complementary XML to <paramref name="serializable"/>.</param>
        /// <param name="arg1">The first parameter of the delegate <paramref name="writer"/>.</param>
        /// <param name="arg2">The second parameter of the delegate <paramref name="writer"/>.</param>
        /// <param name="arg3">The third parameter of the delegate <paramref name="writer"/>.</param>
        /// <param name="arg4">The fourth parameter of the delegate <paramref name="writer"/>.</param>
        /// <returns>A <see cref="Stream"/> holding the serialized version of <paramref name="serializable"/>.</returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="serializable"/> is null.
        /// </exception>
        public static Stream Serialize<T1, T2, T3, T4>(object serializable, SerializableOrder order, Action<XmlWriter, T1, T2, T3, T4> writer, T1 arg1, T2 arg2, T3 arg3, T4 arg4)
        {
            return Serialize(serializable, order, false, writer, arg1, arg2, arg3, arg4);
        }

        /// <summary>
        /// Creates and returns a XML stream representation of the specified <paramref name="serializable"/> object while complementary XML is applied to <paramref name="serializable"/>.
        /// </summary>
        /// <typeparam name="T1">The type of the first parameter of the delegate <paramref name="writer"/>.</typeparam>
        /// <typeparam name="T2">The type of the second parameter of the delegate <paramref name="writer"/>.</typeparam>
        /// <typeparam name="T3">The type of the third parameter of the delegate <paramref name="writer"/>.</typeparam>
        /// <typeparam name="T4">The type of the fourth parameter of the delegate <paramref name="writer"/>.</typeparam>
        /// <param name="serializable">The object to serialize.</param>
        /// <param name="order">A <see cref="SerializableOrder"/> that specifies the order (append or prepend) in which the additional serialization information is applied to the <paramref name="serializable"/> object.</param>
        /// <param name="omitXmlDeclaration">if set to <c>true</c> omit the XML declaration; otherwise <c>false</c>. The default is false.</param>
        /// <param name="writer">The delegate that will either prepend or append complementary XML to <paramref name="serializable"/>.</param>
        /// <param name="arg1">The first parameter of the delegate <paramref name="writer"/>.</param>
        /// <param name="arg2">The second parameter of the delegate <paramref name="writer"/>.</param>
        /// <param name="arg3">The third parameter of the delegate <paramref name="writer"/>.</param>
        /// <param name="arg4">The fourth parameter of the delegate <paramref name="writer"/>.</param>
        /// <returns>A <see cref="Stream"/> holding the serialized version of <paramref name="serializable"/>.</returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="serializable"/> is null.
        /// </exception>
        public static Stream Serialize<T1, T2, T3, T4>(object serializable, SerializableOrder order, bool omitXmlDeclaration, Action<XmlWriter, T1, T2, T3, T4> writer, T1 arg1, T2 arg2, T3 arg3, T4 arg4)
        {
            return Serialize(serializable, order, omitXmlDeclaration, (XmlQualifiedEntity)null, writer, arg1, arg2, arg3, arg4);
        }

        /// <summary>
        /// Creates and returns a XML stream representation of the specified <paramref name="serializable"/> object while complementary XML is applied to <paramref name="serializable"/>.
        /// </summary>
        /// <typeparam name="T1">The type of the first parameter of the delegate <paramref name="writer"/>.</typeparam>
        /// <typeparam name="T2">The type of the second parameter of the delegate <paramref name="writer"/>.</typeparam>
        /// <typeparam name="T3">The type of the third parameter of the delegate <paramref name="writer"/>.</typeparam>
        /// <typeparam name="T4">The type of the fourth parameter of the delegate <paramref name="writer"/>.</typeparam>
        /// <param name="serializable">The object to serialize.</param>
        /// <param name="order">A <see cref="SerializableOrder"/> that specifies the order (append or prepend) in which the additional serialization information is applied to the <paramref name="serializable"/> object.</param>
        /// <param name="omitXmlDeclaration">if set to <c>true</c> omit the XML declaration; otherwise <c>false</c>. The default is false.</param>
        /// <param name="qualifiedRootEntity">A <see cref="XmlQualifiedEntity"/> that overrides and represents the fully qualified name of the XML root element.</param>
        /// <param name="writer">The delegate that will either prepend or append complementary XML to <paramref name="serializable"/>.</param>
        /// <param name="arg1">The first parameter of the delegate <paramref name="writer"/>.</param>
        /// <param name="arg2">The second parameter of the delegate <paramref name="writer"/>.</param>
        /// <param name="arg3">The third parameter of the delegate <paramref name="writer"/>.</param>
        /// <param name="arg4">The fourth parameter of the delegate <paramref name="writer"/>.</param>
        /// <returns>A <see cref="Stream"/> holding the serialized version of <paramref name="serializable"/>.</returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="serializable"/> is null.
        /// </exception>
        public static Stream Serialize<T1, T2, T3, T4>(object serializable, SerializableOrder order, bool omitXmlDeclaration, XmlQualifiedEntity qualifiedRootEntity, Action<XmlWriter, T1, T2, T3, T4> writer, T1 arg1, T2 arg2, T3 arg3, T4 arg4)
        {
            Validator.ThrowIfNull(serializable, nameof(serializable));
            var factory = ActionFactory.Create(writer, null, arg1, arg2, arg3, arg4);
            return SerializeCore(factory, serializable, order, omitXmlDeclaration, qualifiedRootEntity);
        }

        /// <summary>
        /// Creates and returns a XML stream representation of the specified <paramref name="serializable"/> object while complementary XML is applied to <paramref name="serializable"/>.
        /// </summary>
        /// <typeparam name="T1">The type of the first parameter of the delegate <paramref name="writer"/>.</typeparam>
        /// <typeparam name="T2">The type of the second parameter of the delegate <paramref name="writer"/>.</typeparam>
        /// <typeparam name="T3">The type of the third parameter of the delegate <paramref name="writer"/>.</typeparam>
        /// <typeparam name="T4">The type of the fourth parameter of the delegate <paramref name="writer"/>.</typeparam>
        /// <typeparam name="T5">The type of the fifth parameter of the delegate <paramref name="writer"/>.</typeparam>
        /// <param name="serializable">The object to serialize.</param>
        /// <param name="order">A <see cref="SerializableOrder"/> that specifies the order (append or prepend) in which the additional serialization information is applied to the <paramref name="serializable"/> object.</param>
        /// <param name="writer">The delegate that will either prepend or append complementary XML to <paramref name="serializable"/>.</param>
        /// <param name="arg1">The first parameter of the delegate <paramref name="writer"/>.</param>
        /// <param name="arg2">The second parameter of the delegate <paramref name="writer"/>.</param>
        /// <param name="arg3">The third parameter of the delegate <paramref name="writer"/>.</param>
        /// <param name="arg4">The fourth parameter of the delegate <paramref name="writer"/>.</param>
        /// <param name="arg5">The fifth parameter of the delegate <paramref name="writer"/>.</param>
        /// <returns>A <see cref="Stream"/> holding the serialized version of <paramref name="serializable"/>.</returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="serializable"/> is null.
        /// </exception>
        public static Stream Serialize<T1, T2, T3, T4, T5>(object serializable, SerializableOrder order, Action<XmlWriter, T1, T2, T3, T4, T5> writer, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5)
        {
            return Serialize(serializable, order, false, writer, arg1, arg2, arg3, arg4, arg5);
        }

        /// <summary>
        /// Creates and returns a XML stream representation of the specified <paramref name="serializable"/> object while complementary XML is applied to <paramref name="serializable"/>.
        /// </summary>
        /// <typeparam name="T1">The type of the first parameter of the delegate <paramref name="writer"/>.</typeparam>
        /// <typeparam name="T2">The type of the second parameter of the delegate <paramref name="writer"/>.</typeparam>
        /// <typeparam name="T3">The type of the third parameter of the delegate <paramref name="writer"/>.</typeparam>
        /// <typeparam name="T4">The type of the fourth parameter of the delegate <paramref name="writer"/>.</typeparam>
        /// <typeparam name="T5">The type of the fifth parameter of the delegate <paramref name="writer"/>.</typeparam>
        /// <param name="serializable">The object to serialize.</param>
        /// <param name="order">A <see cref="SerializableOrder"/> that specifies the order (append or prepend) in which the additional serialization information is applied to the <paramref name="serializable"/> object.</param>
        /// <param name="omitXmlDeclaration">if set to <c>true</c> omit the XML declaration; otherwise <c>false</c>. The default is false.</param>
        /// <param name="writer">The delegate that will either prepend or append complementary XML to <paramref name="serializable"/>.</param>
        /// <param name="arg1">The first parameter of the delegate <paramref name="writer"/>.</param>
        /// <param name="arg2">The second parameter of the delegate <paramref name="writer"/>.</param>
        /// <param name="arg3">The third parameter of the delegate <paramref name="writer"/>.</param>
        /// <param name="arg4">The fourth parameter of the delegate <paramref name="writer"/>.</param>
        /// <param name="arg5">The fifth parameter of the delegate <paramref name="writer"/>.</param>
        /// <returns>A <see cref="Stream"/> holding the serialized version of <paramref name="serializable"/>.</returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="serializable"/> is null.
        /// </exception>
        public static Stream Serialize<T1, T2, T3, T4, T5>(object serializable, SerializableOrder order, bool omitXmlDeclaration, Action<XmlWriter, T1, T2, T3, T4, T5> writer, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5)
        {
            return Serialize(serializable, order, omitXmlDeclaration, (XmlQualifiedEntity)null, writer, arg1, arg2, arg3, arg4, arg5);
        }

        /// <summary>
        /// Creates and returns a XML stream representation of the specified <paramref name="serializable"/> object while complementary XML is applied to <paramref name="serializable"/>.
        /// </summary>
        /// <typeparam name="T1">The type of the first parameter of the delegate <paramref name="writer"/>.</typeparam>
        /// <typeparam name="T2">The type of the second parameter of the delegate <paramref name="writer"/>.</typeparam>
        /// <typeparam name="T3">The type of the third parameter of the delegate <paramref name="writer"/>.</typeparam>
        /// <typeparam name="T4">The type of the fourth parameter of the delegate <paramref name="writer"/>.</typeparam>
        /// <typeparam name="T5">The type of the fifth parameter of the delegate <paramref name="writer"/>.</typeparam>
        /// <param name="serializable">The object to serialize.</param>
        /// <param name="order">A <see cref="SerializableOrder"/> that specifies the order (append or prepend) in which the additional serialization information is applied to the <paramref name="serializable"/> object.</param>
        /// <param name="omitXmlDeclaration">if set to <c>true</c> omit the XML declaration; otherwise <c>false</c>. The default is false.</param>
        /// <param name="qualifiedRootEntity">A <see cref="XmlQualifiedEntity"/> that overrides and represents the fully qualified name of the XML root element.</param>
        /// <param name="writer">The delegate that will either prepend or append complementary XML to <paramref name="serializable"/>.</param>
        /// <param name="arg1">The first parameter of the delegate <paramref name="writer"/>.</param>
        /// <param name="arg2">The second parameter of the delegate <paramref name="writer"/>.</param>
        /// <param name="arg3">The third parameter of the delegate <paramref name="writer"/>.</param>
        /// <param name="arg4">The fourth parameter of the delegate <paramref name="writer"/>.</param>
        /// <param name="arg5">The fifth parameter of the delegate <paramref name="writer"/>.</param>
        /// <returns>A <see cref="Stream"/> holding the serialized version of <paramref name="serializable"/>.</returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="serializable"/> is null.
        /// </exception>
        public static Stream Serialize<T1, T2, T3, T4, T5>(object serializable, SerializableOrder order, bool omitXmlDeclaration, XmlQualifiedEntity qualifiedRootEntity, Action<XmlWriter, T1, T2, T3, T4, T5> writer, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5)
        {
            Validator.ThrowIfNull(serializable, nameof(serializable));
            var factory = ActionFactory.Create(writer, null, arg1, arg2, arg3, arg4, arg5);
            return SerializeCore(factory, serializable, order, omitXmlDeclaration, qualifiedRootEntity);
        }

        /// <summary>
        /// Creates and returns a XML stream representation of the specified <paramref name="serializable"/> object while complementary XML is applied to <paramref name="serializable"/>.
        /// </summary>
        /// <typeparam name="T1">The type of the first parameter of the delegate <paramref name="writer"/>.</typeparam>
        /// <typeparam name="T2">The type of the second parameter of the delegate <paramref name="writer"/>.</typeparam>
        /// <typeparam name="T3">The type of the third parameter of the delegate <paramref name="writer"/>.</typeparam>
        /// <typeparam name="T4">The type of the fourth parameter of the delegate <paramref name="writer"/>.</typeparam>
        /// <typeparam name="T5">The type of the fifth parameter of the delegate <paramref name="writer"/>.</typeparam>
        /// <typeparam name="T6">The type of the sixth parameter of the delegate <paramref name="writer"/>.</typeparam>
        /// <param name="serializable">The object to serialize.</param>
        /// <param name="order">A <see cref="SerializableOrder"/> that specifies the order (append or prepend) in which the additional serialization information is applied to the <paramref name="serializable"/> object.</param>
        /// <param name="writer">The delegate that will either prepend or append complementary XML to <paramref name="serializable"/>.</param>
        /// <param name="arg1">The first parameter of the delegate <paramref name="writer"/>.</param>
        /// <param name="arg2">The second parameter of the delegate <paramref name="writer"/>.</param>
        /// <param name="arg3">The third parameter of the delegate <paramref name="writer"/>.</param>
        /// <param name="arg4">The fourth parameter of the delegate <paramref name="writer"/>.</param>
        /// <param name="arg5">The fifth parameter of the delegate <paramref name="writer"/>.</param>
        /// <param name="arg6">The sixth parameter of the delegate <paramref name="writer"/>.</param>
        /// <returns>A <see cref="Stream"/> holding the serialized version of <paramref name="serializable"/>.</returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="serializable"/> is null.
        /// </exception>
        public static Stream Serialize<T1, T2, T3, T4, T5, T6>(object serializable, SerializableOrder order, Action<XmlWriter, T1, T2, T3, T4, T5, T6> writer, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6)
        {
            return Serialize(serializable, order, false, writer, arg1, arg2, arg3, arg4, arg5, arg6);
        }

        /// <summary>
        /// Creates and returns a XML stream representation of the specified <paramref name="serializable"/> object while complementary XML is applied to <paramref name="serializable"/>.
        /// </summary>
        /// <typeparam name="T1">The type of the first parameter of the delegate <paramref name="writer"/>.</typeparam>
        /// <typeparam name="T2">The type of the second parameter of the delegate <paramref name="writer"/>.</typeparam>
        /// <typeparam name="T3">The type of the third parameter of the delegate <paramref name="writer"/>.</typeparam>
        /// <typeparam name="T4">The type of the fourth parameter of the delegate <paramref name="writer"/>.</typeparam>
        /// <typeparam name="T5">The type of the fifth parameter of the delegate <paramref name="writer"/>.</typeparam>
        /// <typeparam name="T6">The type of the sixth parameter of the delegate <paramref name="writer"/>.</typeparam>
        /// <param name="serializable">The object to serialize.</param>
        /// <param name="order">A <see cref="SerializableOrder"/> that specifies the order (append or prepend) in which the additional serialization information is applied to the <paramref name="serializable"/> object.</param>
        /// <param name="omitXmlDeclaration">if set to <c>true</c> omit the XML declaration; otherwise <c>false</c>. The default is false.</param>
        /// <param name="writer">The delegate that will either prepend or append complementary XML to <paramref name="serializable"/>.</param>
        /// <param name="arg1">The first parameter of the delegate <paramref name="writer"/>.</param>
        /// <param name="arg2">The second parameter of the delegate <paramref name="writer"/>.</param>
        /// <param name="arg3">The third parameter of the delegate <paramref name="writer"/>.</param>
        /// <param name="arg4">The fourth parameter of the delegate <paramref name="writer"/>.</param>
        /// <param name="arg5">The fifth parameter of the delegate <paramref name="writer"/>.</param>
        /// <param name="arg6">The sixth parameter of the delegate <paramref name="writer"/>.</param>
        /// <returns>A <see cref="Stream"/> holding the serialized version of <paramref name="serializable"/>.</returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="serializable"/> is null.
        /// </exception>
        public static Stream Serialize<T1, T2, T3, T4, T5, T6>(object serializable, SerializableOrder order, bool omitXmlDeclaration, Action<XmlWriter, T1, T2, T3, T4, T5, T6> writer, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6)
        {
            return Serialize(serializable, order, omitXmlDeclaration, (XmlQualifiedEntity)null, writer, arg1, arg2, arg3, arg4, arg5, arg6);
        }

        /// <summary>
        /// Creates and returns a XML stream representation of the specified <paramref name="serializable"/> object while complementary XML is applied to <paramref name="serializable"/>.
        /// </summary>
        /// <typeparam name="T1">The type of the first parameter of the delegate <paramref name="writer"/>.</typeparam>
        /// <typeparam name="T2">The type of the second parameter of the delegate <paramref name="writer"/>.</typeparam>
        /// <typeparam name="T3">The type of the third parameter of the delegate <paramref name="writer"/>.</typeparam>
        /// <typeparam name="T4">The type of the fourth parameter of the delegate <paramref name="writer"/>.</typeparam>
        /// <typeparam name="T5">The type of the fifth parameter of the delegate <paramref name="writer"/>.</typeparam>
        /// <typeparam name="T6">The type of the sixth parameter of the delegate <paramref name="writer"/>.</typeparam>
        /// <param name="serializable">The object to serialize.</param>
        /// <param name="order">A <see cref="SerializableOrder"/> that specifies the order (append or prepend) in which the additional serialization information is applied to the <paramref name="serializable"/> object.</param>
        /// <param name="omitXmlDeclaration">if set to <c>true</c> omit the XML declaration; otherwise <c>false</c>. The default is false.</param>
        /// <param name="qualifiedRootEntity">A <see cref="XmlQualifiedEntity"/> that overrides and represents the fully qualified name of the XML root element.</param>
        /// <param name="writer">The delegate that will either prepend or append complementary XML to <paramref name="serializable"/>.</param>
        /// <param name="arg1">The first parameter of the delegate <paramref name="writer"/>.</param>
        /// <param name="arg2">The second parameter of the delegate <paramref name="writer"/>.</param>
        /// <param name="arg3">The third parameter of the delegate <paramref name="writer"/>.</param>
        /// <param name="arg4">The fourth parameter of the delegate <paramref name="writer"/>.</param>
        /// <param name="arg5">The fifth parameter of the delegate <paramref name="writer"/>.</param>
        /// <param name="arg6">The sixth parameter of the delegate <paramref name="writer"/>.</param>
        /// <returns>A <see cref="Stream"/> holding the serialized version of <paramref name="serializable"/>.</returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="serializable"/> is null.
        /// </exception>
        public static Stream Serialize<T1, T2, T3, T4, T5, T6>(object serializable, SerializableOrder order, bool omitXmlDeclaration, XmlQualifiedEntity qualifiedRootEntity, Action<XmlWriter, T1, T2, T3, T4, T5, T6> writer, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6)
        {
            var factory = ActionFactory.Create(writer, null, arg1, arg2, arg3, arg4, arg5, arg6);
            return SerializeCore(factory, serializable, order, omitXmlDeclaration, qualifiedRootEntity);
        }

        /// <summary>
        /// Creates and returns a XML stream representation of the specified <paramref name="serializable"/> object while complementary XML is applied to <paramref name="serializable"/>.
        /// </summary>
        /// <typeparam name="T1">The type of the first parameter of the delegate <paramref name="writer"/>.</typeparam>
        /// <typeparam name="T2">The type of the second parameter of the delegate <paramref name="writer"/>.</typeparam>
        /// <typeparam name="T3">The type of the third parameter of the delegate <paramref name="writer"/>.</typeparam>
        /// <typeparam name="T4">The type of the fourth parameter of the delegate <paramref name="writer"/>.</typeparam>
        /// <typeparam name="T5">The type of the fifth parameter of the delegate <paramref name="writer"/>.</typeparam>
        /// <typeparam name="T6">The type of the sixth parameter of the delegate <paramref name="writer"/>.</typeparam>
        /// <typeparam name="T7">The type of the seventh parameter of the delegate <paramref name="writer"/>.</typeparam>
        /// <param name="serializable">The object to serialize.</param>
        /// <param name="order">A <see cref="SerializableOrder"/> that specifies the order (append or prepend) in which the additional serialization information is applied to the <paramref name="serializable"/> object.</param>
        /// <param name="writer">The delegate that will either prepend or append complementary XML to <paramref name="serializable"/>.</param>
        /// <param name="arg1">The first parameter of the delegate <paramref name="writer"/>.</param>
        /// <param name="arg2">The second parameter of the delegate <paramref name="writer"/>.</param>
        /// <param name="arg3">The third parameter of the delegate <paramref name="writer"/>.</param>
        /// <param name="arg4">The fourth parameter of the delegate <paramref name="writer"/>.</param>
        /// <param name="arg5">The fifth parameter of the delegate <paramref name="writer"/>.</param>
        /// <param name="arg6">The sixth parameter of the delegate <paramref name="writer"/>.</param>
        /// <param name="arg7">The seventh parameter of the delegate <paramref name="writer"/>.</param>
        /// <returns>A <see cref="Stream"/> holding the serialized version of <paramref name="serializable"/>.</returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="serializable"/> is null.
        /// </exception>
        public static Stream Serialize<T1, T2, T3, T4, T5, T6, T7>(object serializable, SerializableOrder order, Action<XmlWriter, T1, T2, T3, T4, T5, T6, T7> writer, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7)
        {
            return Serialize(serializable, order, false, writer, arg1, arg2, arg3, arg4, arg5, arg6, arg7);
        }

        /// <summary>
        /// Creates and returns a XML stream representation of the specified <paramref name="serializable"/> object while complementary XML is applied to <paramref name="serializable"/>.
        /// </summary>
        /// <typeparam name="T1">The type of the first parameter of the delegate <paramref name="writer"/>.</typeparam>
        /// <typeparam name="T2">The type of the second parameter of the delegate <paramref name="writer"/>.</typeparam>
        /// <typeparam name="T3">The type of the third parameter of the delegate <paramref name="writer"/>.</typeparam>
        /// <typeparam name="T4">The type of the fourth parameter of the delegate <paramref name="writer"/>.</typeparam>
        /// <typeparam name="T5">The type of the fifth parameter of the delegate <paramref name="writer"/>.</typeparam>
        /// <typeparam name="T6">The type of the sixth parameter of the delegate <paramref name="writer"/>.</typeparam>
        /// <typeparam name="T7">The type of the seventh parameter of the delegate <paramref name="writer"/>.</typeparam>
        /// <param name="serializable">The object to serialize.</param>
        /// <param name="order">A <see cref="SerializableOrder"/> that specifies the order (append or prepend) in which the additional serialization information is applied to the <paramref name="serializable"/> object.</param>
        /// <param name="omitXmlDeclaration">if set to <c>true</c> omit the XML declaration; otherwise <c>false</c>. The default is false.</param>
        /// <param name="writer">The delegate that will either prepend or append complementary XML to <paramref name="serializable"/>.</param>
        /// <param name="arg1">The first parameter of the delegate <paramref name="writer"/>.</param>
        /// <param name="arg2">The second parameter of the delegate <paramref name="writer"/>.</param>
        /// <param name="arg3">The third parameter of the delegate <paramref name="writer"/>.</param>
        /// <param name="arg4">The fourth parameter of the delegate <paramref name="writer"/>.</param>
        /// <param name="arg5">The fifth parameter of the delegate <paramref name="writer"/>.</param>
        /// <param name="arg6">The sixth parameter of the delegate <paramref name="writer"/>.</param>
        /// <param name="arg7">The seventh parameter of the delegate <paramref name="writer"/>.</param>
        /// <returns>A <see cref="Stream"/> holding the serialized version of <paramref name="serializable"/>.</returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="serializable"/> is null.
        /// </exception>
        public static Stream Serialize<T1, T2, T3, T4, T5, T6, T7>(object serializable, SerializableOrder order, bool omitXmlDeclaration, Action<XmlWriter, T1, T2, T3, T4, T5, T6, T7> writer, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7)
        {
            return Serialize(serializable, order, omitXmlDeclaration, (XmlQualifiedEntity)null, writer, arg1, arg2, arg3, arg4, arg5, arg6, arg7);
        }

        /// <summary>
        /// Creates and returns a XML stream representation of the specified <paramref name="serializable"/> object while complementary XML is applied to <paramref name="serializable"/>.
        /// </summary>
        /// <typeparam name="T1">The type of the first parameter of the delegate <paramref name="writer"/>.</typeparam>
        /// <typeparam name="T2">The type of the second parameter of the delegate <paramref name="writer"/>.</typeparam>
        /// <typeparam name="T3">The type of the third parameter of the delegate <paramref name="writer"/>.</typeparam>
        /// <typeparam name="T4">The type of the fourth parameter of the delegate <paramref name="writer"/>.</typeparam>
        /// <typeparam name="T5">The type of the fifth parameter of the delegate <paramref name="writer"/>.</typeparam>
        /// <typeparam name="T6">The type of the sixth parameter of the delegate <paramref name="writer"/>.</typeparam>
        /// <typeparam name="T7">The type of the seventh parameter of the delegate <paramref name="writer"/>.</typeparam>
        /// <param name="serializable">The object to serialize.</param>
        /// <param name="order">A <see cref="SerializableOrder"/> that specifies the order (append or prepend) in which the additional serialization information is applied to the <paramref name="serializable"/> object.</param>
        /// <param name="omitXmlDeclaration">if set to <c>true</c> omit the XML declaration; otherwise <c>false</c>. The default is false.</param>
        /// <param name="qualifiedRootEntity">A <see cref="XmlQualifiedEntity"/> that overrides and represents the fully qualified name of the XML root element.</param>
        /// <param name="writer">The delegate that will either prepend or append complementary XML to <paramref name="serializable"/>.</param>
        /// <param name="arg1">The first parameter of the delegate <paramref name="writer"/>.</param>
        /// <param name="arg2">The second parameter of the delegate <paramref name="writer"/>.</param>
        /// <param name="arg3">The third parameter of the delegate <paramref name="writer"/>.</param>
        /// <param name="arg4">The fourth parameter of the delegate <paramref name="writer"/>.</param>
        /// <param name="arg5">The fifth parameter of the delegate <paramref name="writer"/>.</param>
        /// <param name="arg6">The sixth parameter of the delegate <paramref name="writer"/>.</param>
        /// <param name="arg7">The seventh parameter of the delegate <paramref name="writer"/>.</param>
        /// <returns>A <see cref="Stream"/> holding the serialized version of <paramref name="serializable"/>.</returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="serializable"/> is null.
        /// </exception>
        public static Stream Serialize<T1, T2, T3, T4, T5, T6, T7>(object serializable, SerializableOrder order, bool omitXmlDeclaration, XmlQualifiedEntity qualifiedRootEntity, Action<XmlWriter, T1, T2, T3, T4, T5, T6, T7> writer, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7)
        {
            Validator.ThrowIfNull(serializable, nameof(serializable));
            var factory = ActionFactory.Create(writer, null, arg1, arg2, arg3, arg4, arg5, arg6, arg7);
            return SerializeCore(factory, serializable, order, omitXmlDeclaration, qualifiedRootEntity);
        }

        /// <summary>
        /// Creates and returns a XML stream representation of the specified <paramref name="serializable"/> object while complementary XML is applied to <paramref name="serializable"/>.
        /// </summary>
        /// <typeparam name="T1">The type of the first parameter of the delegate <paramref name="writer"/>.</typeparam>
        /// <typeparam name="T2">The type of the second parameter of the delegate <paramref name="writer"/>.</typeparam>
        /// <typeparam name="T3">The type of the third parameter of the delegate <paramref name="writer"/>.</typeparam>
        /// <typeparam name="T4">The type of the fourth parameter of the delegate <paramref name="writer"/>.</typeparam>
        /// <typeparam name="T5">The type of the fifth parameter of the delegate <paramref name="writer"/>.</typeparam>
        /// <typeparam name="T6">The type of the sixth parameter of the delegate <paramref name="writer"/>.</typeparam>
        /// <typeparam name="T7">The type of the seventh parameter of the delegate <paramref name="writer"/>.</typeparam>
        /// <typeparam name="T8">The type of the eighth parameter of the delegate <paramref name="writer"/>.</typeparam>
        /// <param name="serializable">The object to serialize.</param>
        /// <param name="order">A <see cref="SerializableOrder"/> that specifies the order (append or prepend) in which the additional serialization information is applied to the <paramref name="serializable"/> object.</param>
        /// <param name="writer">The delegate that will either prepend or append complementary XML to <paramref name="serializable"/>.</param>
        /// <param name="arg1">The first parameter of the delegate <paramref name="writer"/>.</param>
        /// <param name="arg2">The second parameter of the delegate <paramref name="writer"/>.</param>
        /// <param name="arg3">The third parameter of the delegate <paramref name="writer"/>.</param>
        /// <param name="arg4">The fourth parameter of the delegate <paramref name="writer"/>.</param>
        /// <param name="arg5">The fifth parameter of the delegate <paramref name="writer"/>.</param>
        /// <param name="arg6">The sixth parameter of the delegate <paramref name="writer"/>.</param>
        /// <param name="arg7">The seventh parameter of the delegate <paramref name="writer"/>.</param>
        /// <param name="arg8">The eighth parameter of the delegate <paramref name="writer"/>.</param>
        /// <returns>A <see cref="Stream"/> holding the serialized version of <paramref name="serializable"/>.</returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="serializable"/> is null.
        /// </exception>
        public static Stream Serialize<T1, T2, T3, T4, T5, T6, T7, T8>(object serializable, SerializableOrder order, Action<XmlWriter, T1, T2, T3, T4, T5, T6, T7, T8> writer, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8)
        {
            return Serialize(serializable, order, false, writer, arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8);
        }

        /// <summary>
        /// Creates and returns a XML stream representation of the specified <paramref name="serializable"/> object while complementary XML is applied to <paramref name="serializable"/>.
        /// </summary>
        /// <typeparam name="T1">The type of the first parameter of the delegate <paramref name="writer"/>.</typeparam>
        /// <typeparam name="T2">The type of the second parameter of the delegate <paramref name="writer"/>.</typeparam>
        /// <typeparam name="T3">The type of the third parameter of the delegate <paramref name="writer"/>.</typeparam>
        /// <typeparam name="T4">The type of the fourth parameter of the delegate <paramref name="writer"/>.</typeparam>
        /// <typeparam name="T5">The type of the fifth parameter of the delegate <paramref name="writer"/>.</typeparam>
        /// <typeparam name="T6">The type of the sixth parameter of the delegate <paramref name="writer"/>.</typeparam>
        /// <typeparam name="T7">The type of the seventh parameter of the delegate <paramref name="writer"/>.</typeparam>
        /// <typeparam name="T8">The type of the eighth parameter of the delegate <paramref name="writer"/>.</typeparam>
        /// <param name="serializable">The object to serialize.</param>
        /// <param name="order">A <see cref="SerializableOrder"/> that specifies the order (append or prepend) in which the additional serialization information is applied to the <paramref name="serializable"/> object.</param>
        /// <param name="omitXmlDeclaration">if set to <c>true</c> omit the XML declaration; otherwise <c>false</c>. The default is false.</param>
        /// <param name="writer">The delegate that will either prepend or append complementary XML to <paramref name="serializable"/>.</param>
        /// <param name="arg1">The first parameter of the delegate <paramref name="writer"/>.</param>
        /// <param name="arg2">The second parameter of the delegate <paramref name="writer"/>.</param>
        /// <param name="arg3">The third parameter of the delegate <paramref name="writer"/>.</param>
        /// <param name="arg4">The fourth parameter of the delegate <paramref name="writer"/>.</param>
        /// <param name="arg5">The fifth parameter of the delegate <paramref name="writer"/>.</param>
        /// <param name="arg6">The sixth parameter of the delegate <paramref name="writer"/>.</param>
        /// <param name="arg7">The seventh parameter of the delegate <paramref name="writer"/>.</param>
        /// <param name="arg8">The eighth parameter of the delegate <paramref name="writer"/>.</param>
        /// <returns>A <see cref="Stream"/> holding the serialized version of <paramref name="serializable"/>.</returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="serializable"/> is null.
        /// </exception>
        public static Stream Serialize<T1, T2, T3, T4, T5, T6, T7, T8>(object serializable, SerializableOrder order, bool omitXmlDeclaration, Action<XmlWriter, T1, T2, T3, T4, T5, T6, T7, T8> writer, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8)
        {
            return Serialize(serializable, order, omitXmlDeclaration, (XmlQualifiedEntity)null, writer, arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8);
        }

        /// <summary>
        /// Creates and returns a XML stream representation of the specified <paramref name="serializable"/> object while complementary XML is applied to <paramref name="serializable"/>.
        /// </summary>
        /// <typeparam name="T1">The type of the first parameter of the delegate <paramref name="writer"/>.</typeparam>
        /// <typeparam name="T2">The type of the second parameter of the delegate <paramref name="writer"/>.</typeparam>
        /// <typeparam name="T3">The type of the third parameter of the delegate <paramref name="writer"/>.</typeparam>
        /// <typeparam name="T4">The type of the fourth parameter of the delegate <paramref name="writer"/>.</typeparam>
        /// <typeparam name="T5">The type of the fifth parameter of the delegate <paramref name="writer"/>.</typeparam>
        /// <typeparam name="T6">The type of the sixth parameter of the delegate <paramref name="writer"/>.</typeparam>
        /// <typeparam name="T7">The type of the seventh parameter of the delegate <paramref name="writer"/>.</typeparam>
        /// <typeparam name="T8">The type of the eighth parameter of the delegate <paramref name="writer"/>.</typeparam>
        /// <param name="serializable">The object to serialize.</param>
        /// <param name="order">A <see cref="SerializableOrder"/> that specifies the order (append or prepend) in which the additional serialization information is applied to the <paramref name="serializable"/> object.</param>
        /// <param name="omitXmlDeclaration">if set to <c>true</c> omit the XML declaration; otherwise <c>false</c>. The default is false.</param>
        /// <param name="qualifiedRootEntity">A <see cref="XmlQualifiedEntity"/> that overrides and represents the fully qualified name of the XML root element.</param>
        /// <param name="writer">The delegate that will either prepend or append complementary XML to <paramref name="serializable"/>.</param>
        /// <param name="arg1">The first parameter of the delegate <paramref name="writer"/>.</param>
        /// <param name="arg2">The second parameter of the delegate <paramref name="writer"/>.</param>
        /// <param name="arg3">The third parameter of the delegate <paramref name="writer"/>.</param>
        /// <param name="arg4">The fourth parameter of the delegate <paramref name="writer"/>.</param>
        /// <param name="arg5">The fifth parameter of the delegate <paramref name="writer"/>.</param>
        /// <param name="arg6">The sixth parameter of the delegate <paramref name="writer"/>.</param>
        /// <param name="arg7">The seventh parameter of the delegate <paramref name="writer"/>.</param>
        /// <param name="arg8">The eighth parameter of the delegate <paramref name="writer"/>.</param>
        /// <returns>A <see cref="Stream"/> holding the serialized version of <paramref name="serializable"/>.</returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="serializable"/> is null.
        /// </exception>
        public static Stream Serialize<T1, T2, T3, T4, T5, T6, T7, T8>(object serializable, SerializableOrder order, bool omitXmlDeclaration, XmlQualifiedEntity qualifiedRootEntity, Action<XmlWriter, T1, T2, T3, T4, T5, T6, T7, T8> writer, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8)
        {
            Validator.ThrowIfNull(serializable, nameof(serializable));
            var factory = ActionFactory.Create(writer, null, arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8);
            return SerializeCore(factory, serializable, order, omitXmlDeclaration, qualifiedRootEntity);
        }

        /// <summary>
        /// Creates and returns a XML stream representation of the specified <paramref name="serializable"/> object while complementary XML is applied to <paramref name="serializable"/>.
        /// </summary>
        /// <typeparam name="T1">The type of the first parameter of the delegate <paramref name="writer"/>.</typeparam>
        /// <typeparam name="T2">The type of the second parameter of the delegate <paramref name="writer"/>.</typeparam>
        /// <typeparam name="T3">The type of the third parameter of the delegate <paramref name="writer"/>.</typeparam>
        /// <typeparam name="T4">The type of the fourth parameter of the delegate <paramref name="writer"/>.</typeparam>
        /// <typeparam name="T5">The type of the fifth parameter of the delegate <paramref name="writer"/>.</typeparam>
        /// <typeparam name="T6">The type of the sixth parameter of the delegate <paramref name="writer"/>.</typeparam>
        /// <typeparam name="T7">The type of the seventh parameter of the delegate <paramref name="writer"/>.</typeparam>
        /// <typeparam name="T8">The type of the eighth parameter of the delegate <paramref name="writer"/>.</typeparam>
        /// <typeparam name="T9">The type of the ninth parameter of the delegate <paramref name="writer"/>.</typeparam>
        /// <param name="serializable">The object to serialize.</param>
        /// <param name="order">A <see cref="SerializableOrder"/> that specifies the order (append or prepend) in which the additional serialization information is applied to the <paramref name="serializable"/> object.</param>
        /// <param name="writer">The delegate that will either prepend or append complementary XML to <paramref name="serializable"/>.</param>
        /// <param name="arg1">The first parameter of the delegate <paramref name="writer"/>.</param>
        /// <param name="arg2">The second parameter of the delegate <paramref name="writer"/>.</param>
        /// <param name="arg3">The third parameter of the delegate <paramref name="writer"/>.</param>
        /// <param name="arg4">The fourth parameter of the delegate <paramref name="writer"/>.</param>
        /// <param name="arg5">The fifth parameter of the delegate <paramref name="writer"/>.</param>
        /// <param name="arg6">The sixth parameter of the delegate <paramref name="writer"/>.</param>
        /// <param name="arg7">The seventh parameter of the delegate <paramref name="writer"/>.</param>
        /// <param name="arg8">The eighth parameter of the delegate <paramref name="writer"/>.</param>
        /// <param name="arg9">The ninth parameter of the delegate <paramref name="writer"/>.</param>
        /// <returns>A <see cref="Stream"/> holding the serialized version of <paramref name="serializable"/>.</returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="serializable"/> is null.
        /// </exception>
        public static Stream Serialize<T1, T2, T3, T4, T5, T6, T7, T8, T9>(object serializable, SerializableOrder order, Action<XmlWriter, T1, T2, T3, T4, T5, T6, T7, T8, T9> writer, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8, T9 arg9)
        {
            return Serialize(serializable, order, false, writer, arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9);
        }

        /// <summary>
        /// Creates and returns a XML stream representation of the specified <paramref name="serializable"/> object while complementary XML is applied to <paramref name="serializable"/>.
        /// </summary>
        /// <typeparam name="T1">The type of the first parameter of the delegate <paramref name="writer"/>.</typeparam>
        /// <typeparam name="T2">The type of the second parameter of the delegate <paramref name="writer"/>.</typeparam>
        /// <typeparam name="T3">The type of the third parameter of the delegate <paramref name="writer"/>.</typeparam>
        /// <typeparam name="T4">The type of the fourth parameter of the delegate <paramref name="writer"/>.</typeparam>
        /// <typeparam name="T5">The type of the fifth parameter of the delegate <paramref name="writer"/>.</typeparam>
        /// <typeparam name="T6">The type of the sixth parameter of the delegate <paramref name="writer"/>.</typeparam>
        /// <typeparam name="T7">The type of the seventh parameter of the delegate <paramref name="writer"/>.</typeparam>
        /// <typeparam name="T8">The type of the eighth parameter of the delegate <paramref name="writer"/>.</typeparam>
        /// <typeparam name="T9">The type of the ninth parameter of the delegate <paramref name="writer"/>.</typeparam>
        /// <param name="serializable">The object to serialize.</param>
        /// <param name="order">A <see cref="SerializableOrder"/> that specifies the order (append or prepend) in which the additional serialization information is applied to the <paramref name="serializable"/> object.</param>
        /// <param name="omitXmlDeclaration">if set to <c>true</c> omit the XML declaration; otherwise <c>false</c>. The default is false.</param>
        /// <param name="writer">The delegate that will either prepend or append complementary XML to <paramref name="serializable"/>.</param>
        /// <param name="arg1">The first parameter of the delegate <paramref name="writer"/>.</param>
        /// <param name="arg2">The second parameter of the delegate <paramref name="writer"/>.</param>
        /// <param name="arg3">The third parameter of the delegate <paramref name="writer"/>.</param>
        /// <param name="arg4">The fourth parameter of the delegate <paramref name="writer"/>.</param>
        /// <param name="arg5">The fifth parameter of the delegate <paramref name="writer"/>.</param>
        /// <param name="arg6">The sixth parameter of the delegate <paramref name="writer"/>.</param>
        /// <param name="arg7">The seventh parameter of the delegate <paramref name="writer"/>.</param>
        /// <param name="arg8">The eighth parameter of the delegate <paramref name="writer"/>.</param>
        /// <param name="arg9">The ninth parameter of the delegate <paramref name="writer"/>.</param>
        /// <returns>A <see cref="Stream"/> holding the serialized version of <paramref name="serializable"/>.</returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="serializable"/> is null.
        /// </exception>
        public static Stream Serialize<T1, T2, T3, T4, T5, T6, T7, T8, T9>(object serializable, SerializableOrder order, bool omitXmlDeclaration, Action<XmlWriter, T1, T2, T3, T4, T5, T6, T7, T8, T9> writer, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8, T9 arg9)
        {
            return Serialize(serializable, order, omitXmlDeclaration, (XmlQualifiedEntity)null, writer, arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9);
        }

        /// <summary>
        /// Creates and returns a XML stream representation of the specified <paramref name="serializable"/> object while complementary XML is applied to <paramref name="serializable"/>.
        /// </summary>
        /// <typeparam name="T1">The type of the first parameter of the delegate <paramref name="writer"/>.</typeparam>
        /// <typeparam name="T2">The type of the second parameter of the delegate <paramref name="writer"/>.</typeparam>
        /// <typeparam name="T3">The type of the third parameter of the delegate <paramref name="writer"/>.</typeparam>
        /// <typeparam name="T4">The type of the fourth parameter of the delegate <paramref name="writer"/>.</typeparam>
        /// <typeparam name="T5">The type of the fifth parameter of the delegate <paramref name="writer"/>.</typeparam>
        /// <typeparam name="T6">The type of the sixth parameter of the delegate <paramref name="writer"/>.</typeparam>
        /// <typeparam name="T7">The type of the seventh parameter of the delegate <paramref name="writer"/>.</typeparam>
        /// <typeparam name="T8">The type of the eighth parameter of the delegate <paramref name="writer"/>.</typeparam>
        /// <typeparam name="T9">The type of the ninth parameter of the delegate <paramref name="writer"/>.</typeparam>
        /// <param name="serializable">The object to serialize.</param>
        /// <param name="order">A <see cref="SerializableOrder"/> that specifies the order (append or prepend) in which the additional serialization information is applied to the <paramref name="serializable"/> object.</param>
        /// <param name="omitXmlDeclaration">if set to <c>true</c> omit the XML declaration; otherwise <c>false</c>. The default is false.</param>
        /// <param name="qualifiedRootEntity">A <see cref="XmlQualifiedEntity"/> that overrides and represents the fully qualified name of the XML root element.</param>
        /// <param name="writer">The delegate that will either prepend or append complementary XML to <paramref name="serializable"/>.</param>
        /// <param name="arg1">The first parameter of the delegate <paramref name="writer"/>.</param>
        /// <param name="arg2">The second parameter of the delegate <paramref name="writer"/>.</param>
        /// <param name="arg3">The third parameter of the delegate <paramref name="writer"/>.</param>
        /// <param name="arg4">The fourth parameter of the delegate <paramref name="writer"/>.</param>
        /// <param name="arg5">The fifth parameter of the delegate <paramref name="writer"/>.</param>
        /// <param name="arg6">The sixth parameter of the delegate <paramref name="writer"/>.</param>
        /// <param name="arg7">The seventh parameter of the delegate <paramref name="writer"/>.</param>
        /// <param name="arg8">The eighth parameter of the delegate <paramref name="writer"/>.</param>
        /// <param name="arg9">The ninth parameter of the delegate <paramref name="writer"/>.</param>
        /// <returns>A <see cref="Stream"/> holding the serialized version of <paramref name="serializable"/>.</returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="serializable"/> is null.
        /// </exception>
        public static Stream Serialize<T1, T2, T3, T4, T5, T6, T7, T8, T9>(object serializable, SerializableOrder order, bool omitXmlDeclaration, XmlQualifiedEntity qualifiedRootEntity, Action<XmlWriter, T1, T2, T3, T4, T5, T6, T7, T8, T9> writer, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8, T9 arg9)
        {
            Validator.ThrowIfNull(serializable, nameof(serializable));
            var factory = ActionFactory.Create(writer, null, arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9);
            return SerializeCore(factory, serializable, order, omitXmlDeclaration, qualifiedRootEntity);
        }

        /// <summary>
        /// Creates and returns a XML stream representation of the specified <paramref name="serializable"/> object while complementary XML is applied to <paramref name="serializable"/>.
        /// </summary>
        /// <typeparam name="T1">The type of the first parameter of the delegate <paramref name="writer"/>.</typeparam>
        /// <typeparam name="T2">The type of the second parameter of the delegate <paramref name="writer"/>.</typeparam>
        /// <typeparam name="T3">The type of the third parameter of the delegate <paramref name="writer"/>.</typeparam>
        /// <typeparam name="T4">The type of the fourth parameter of the delegate <paramref name="writer"/>.</typeparam>
        /// <typeparam name="T5">The type of the fifth parameter of the delegate <paramref name="writer"/>.</typeparam>
        /// <typeparam name="T6">The type of the sixth parameter of the delegate <paramref name="writer"/>.</typeparam>
        /// <typeparam name="T7">The type of the seventh parameter of the delegate <paramref name="writer"/>.</typeparam>
        /// <typeparam name="T8">The type of the eighth parameter of the delegate <paramref name="writer"/>.</typeparam>
        /// <typeparam name="T9">The type of the ninth parameter of the delegate <paramref name="writer"/>.</typeparam>
        /// <typeparam name="T10">The type of the tenth parameter of the delegate <paramref name="writer"/>.</typeparam>
        /// <param name="serializable">The object to serialize.</param>
        /// <param name="order">A <see cref="SerializableOrder"/> that specifies the order (append or prepend) in which the additional serialization information is applied to the <paramref name="serializable"/> object.</param>
        /// <param name="writer">The delegate that will either prepend or append complementary XML to <paramref name="serializable"/>.</param>
        /// <param name="arg1">The first parameter of the delegate <paramref name="writer"/>.</param>
        /// <param name="arg2">The second parameter of the delegate <paramref name="writer"/>.</param>
        /// <param name="arg3">The third parameter of the delegate <paramref name="writer"/>.</param>
        /// <param name="arg4">The fourth parameter of the delegate <paramref name="writer"/>.</param>
        /// <param name="arg5">The fifth parameter of the delegate <paramref name="writer"/>.</param>
        /// <param name="arg6">The sixth parameter of the delegate <paramref name="writer"/>.</param>
        /// <param name="arg7">The seventh parameter of the delegate <paramref name="writer"/>.</param>
        /// <param name="arg8">The eighth parameter of the delegate <paramref name="writer"/>.</param>
        /// <param name="arg9">The ninth parameter of the delegate <paramref name="writer"/>.</param>
        /// <param name="arg10">The tenth parameter of the delegate <paramref name="writer"/>.</param>
        /// <returns>A <see cref="Stream"/> holding the serialized version of <paramref name="serializable"/>.</returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="serializable"/> is null.
        /// </exception>
        public static Stream Serialize<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>(object serializable, SerializableOrder order, Action<XmlWriter, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10> writer, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8, T9 arg9, T10 arg10)
        {
            return Serialize(serializable, order, false, writer, arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10);
        }

        /// <summary>
        /// Creates and returns a XML stream representation of the specified <paramref name="serializable"/> object while complementary XML is applied to <paramref name="serializable"/>.
        /// </summary>
        /// <typeparam name="T1">The type of the first parameter of the delegate <paramref name="writer"/>.</typeparam>
        /// <typeparam name="T2">The type of the second parameter of the delegate <paramref name="writer"/>.</typeparam>
        /// <typeparam name="T3">The type of the third parameter of the delegate <paramref name="writer"/>.</typeparam>
        /// <typeparam name="T4">The type of the fourth parameter of the delegate <paramref name="writer"/>.</typeparam>
        /// <typeparam name="T5">The type of the fifth parameter of the delegate <paramref name="writer"/>.</typeparam>
        /// <typeparam name="T6">The type of the sixth parameter of the delegate <paramref name="writer"/>.</typeparam>
        /// <typeparam name="T7">The type of the seventh parameter of the delegate <paramref name="writer"/>.</typeparam>
        /// <typeparam name="T8">The type of the eighth parameter of the delegate <paramref name="writer"/>.</typeparam>
        /// <typeparam name="T9">The type of the ninth parameter of the delegate <paramref name="writer"/>.</typeparam>
        /// <typeparam name="T10">The type of the tenth parameter of the delegate <paramref name="writer"/>.</typeparam>
        /// <param name="serializable">The object to serialize.</param>
        /// <param name="omitXmlDeclaration">if set to <c>true</c> omit the XML declaration; otherwise <c>false</c>. The default is false.</param>
        /// <param name="order">A <see cref="SerializableOrder"/> that specifies the order (append or prepend) in which the additional serialization information is applied to the <paramref name="serializable"/> object.</param>
        /// <param name="writer">The delegate that will either prepend or append complementary XML to <paramref name="serializable"/>.</param>
        /// <param name="arg1">The first parameter of the delegate <paramref name="writer"/>.</param>
        /// <param name="arg2">The second parameter of the delegate <paramref name="writer"/>.</param>
        /// <param name="arg3">The third parameter of the delegate <paramref name="writer"/>.</param>
        /// <param name="arg4">The fourth parameter of the delegate <paramref name="writer"/>.</param>
        /// <param name="arg5">The fifth parameter of the delegate <paramref name="writer"/>.</param>
        /// <param name="arg6">The sixth parameter of the delegate <paramref name="writer"/>.</param>
        /// <param name="arg7">The seventh parameter of the delegate <paramref name="writer"/>.</param>
        /// <param name="arg8">The eighth parameter of the delegate <paramref name="writer"/>.</param>
        /// <param name="arg9">The ninth parameter of the delegate <paramref name="writer"/>.</param>
        /// <param name="arg10">The tenth parameter of the delegate <paramref name="writer"/>.</param>
        /// <returns>A <see cref="Stream"/> holding the serialized version of <paramref name="serializable"/>.</returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="serializable"/> is null.
        /// </exception>
        public static Stream Serialize<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>(object serializable, SerializableOrder order, bool omitXmlDeclaration, Action<XmlWriter, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10> writer, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8, T9 arg9, T10 arg10)
        {
            return Serialize(serializable, order, omitXmlDeclaration, null, writer, arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10);
        }

        /// <summary>
        /// Creates and returns a XML stream representation of the specified <paramref name="serializable"/> object while complementary XML is applied to <paramref name="serializable"/>.
        /// </summary>
        /// <typeparam name="T1">The type of the first parameter of the delegate <paramref name="writer"/>.</typeparam>
        /// <typeparam name="T2">The type of the second parameter of the delegate <paramref name="writer"/>.</typeparam>
        /// <typeparam name="T3">The type of the third parameter of the delegate <paramref name="writer"/>.</typeparam>
        /// <typeparam name="T4">The type of the fourth parameter of the delegate <paramref name="writer"/>.</typeparam>
        /// <typeparam name="T5">The type of the fifth parameter of the delegate <paramref name="writer"/>.</typeparam>
        /// <typeparam name="T6">The type of the sixth parameter of the delegate <paramref name="writer"/>.</typeparam>
        /// <typeparam name="T7">The type of the seventh parameter of the delegate <paramref name="writer"/>.</typeparam>
        /// <typeparam name="T8">The type of the eighth parameter of the delegate <paramref name="writer"/>.</typeparam>
        /// <typeparam name="T9">The type of the ninth parameter of the delegate <paramref name="writer"/>.</typeparam>
        /// <typeparam name="T10">The type of the tenth parameter of the delegate <paramref name="writer"/>.</typeparam>
        /// <param name="serializable">The object to serialize.</param>
        /// <param name="order">A <see cref="SerializableOrder"/> that specifies the order (append or prepend) in which the additional serialization information is applied to the <paramref name="serializable"/> object.</param>
        /// <param name="omitXmlDeclaration">if set to <c>true</c> omit the XML declaration; otherwise <c>false</c>. The default is false.</param>
        /// <param name="qualifiedRootEntity">A <see cref="XmlQualifiedEntity"/> that overrides and represents the fully qualified name of the XML root element.</param>
        /// <param name="writer">The delegate that will either prepend or append complementary XML to <paramref name="serializable"/>.</param>
        /// <param name="arg1">The first parameter of the delegate <paramref name="writer"/>.</param>
        /// <param name="arg2">The second parameter of the delegate <paramref name="writer"/>.</param>
        /// <param name="arg3">The third parameter of the delegate <paramref name="writer"/>.</param>
        /// <param name="arg4">The fourth parameter of the delegate <paramref name="writer"/>.</param>
        /// <param name="arg5">The fifth parameter of the delegate <paramref name="writer"/>.</param>
        /// <param name="arg6">The sixth parameter of the delegate <paramref name="writer"/>.</param>
        /// <param name="arg7">The seventh parameter of the delegate <paramref name="writer"/>.</param>
        /// <param name="arg8">The eighth parameter of the delegate <paramref name="writer"/>.</param>
        /// <param name="arg9">The ninth parameter of the delegate <paramref name="writer"/>.</param>
        /// <param name="arg10">The tenth parameter of the delegate <paramref name="writer"/>.</param>
        /// <returns>A <see cref="Stream"/> holding the serialized version of <paramref name="serializable"/>.</returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="serializable"/> is null.
        /// </exception>
        public static Stream Serialize<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>(object serializable, SerializableOrder order, bool omitXmlDeclaration, XmlQualifiedEntity qualifiedRootEntity, Action<XmlWriter, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10> writer, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8, T9 arg9, T10 arg10)
        {
            Validator.ThrowIfNull(serializable, nameof(serializable));
            var factory = ActionFactory.Create(writer, null, arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10);
            return SerializeCore(factory, serializable, order, omitXmlDeclaration, qualifiedRootEntity);
        }

        /// <summary>
        /// Gets or sets the maximum serialization object depth. Default is 10.
        /// </summary>
        /// <value>The maximum serialization object depth.</value>
        /// <remarks>This is an experimental property.</remarks>
        public static int MaxSerializationDepth { get; set; } = 10;

        /// <summary>
        /// Gets or sets the callback implementation that evaluates which public properties of an object should be skipped from either of the Serialize methods on this <see cref="XmlSerializationUtility"/> class.
        /// </summary>
        /// <value>The callback method that evaluates which public properties of an object should be skipped.</value>
        public static Func<Type, bool> SkipPropertyType { get; set; } = DefaultXmlSkipPropertiesCallback;

        /// <summary>
        /// Gets or sets the callback implementation that evaluates if a given property from either of the Serialize methods on this <see cref="XmlSerializationUtility"/> class should be skipped for further processing.
        /// </summary>
        /// <value>The callback method that evaluates if a given property of an object should be skipped.</value>
        public static Func<PropertyInfo, bool> SkipProperty { get; set; } = DefaultXmlSkipPropertyCallback;

        private static Stream SerializeCore<TTuple>(ActionFactory<TTuple> factory, object serializable, SerializableOrder order, bool omitXmlDeclaration, XmlQualifiedEntity qualifiedRootEntity) where TTuple : Template<XmlWriter>
        {
            Validator.ThrowIfNull(serializable, nameof(serializable));
            Stream output = null;
            MemoryStream tempOutput = null;
            IHierarchy<object> serializableNode = ReflectionUtility.GetObjectHierarchy(serializable, options =>
            {
                options.MaxDepth = MaxSerializationDepth;
                options.SkipPropertyType = SkipPropertyType;
                options.SkipProperty = SkipProperty;
            });
            bool useFactory = factory.HasDelegate;
            try
            {
                tempOutput = new MemoryStream();
                using (XmlWriter writer = XmlWriter.Create(tempOutput, XmlWriterUtility.CreateSettings(Encoding.Unicode, omitXmlDeclaration)))
                {
                    if (useFactory) { factory.GenericArguments.Arg1 = writer; }
                    IEnumerable<IHierarchy<object>> ancestorAndSelf = Hierarchy.AncestorsAndSelf(serializableNode);
                    IHierarchy<object> rootNode = ancestorAndSelf.FirstOrDefault();
                    MethodInfo writeXmlMethod = rootNode.InstanceType.GetMethod("WriteXml", ReflectionUtility.BindingInstancePublicAndPrivate);
                    bool useXmlWriter = (writeXmlMethod != null) && TypeUtility.ContainsInterface(rootNode.InstanceType, typeof(IXmlSerializable));

                    try
                    {
                        bool isEnumerable = IsEnumerable(rootNode);
                        if (isEnumerable && (qualifiedRootEntity == null || string.IsNullOrEmpty(qualifiedRootEntity.LocalName)) && !TypeUtility.ContainsAttributeType(rootNode.InstanceType, typeof(XmlRootAttribute)))
                        {
                            qualifiedRootEntity = GetXmlStartElementQualifiedName(rootNode);
                        }

                        XmlQualifiedEntity qualifiedEntity = GetXmlStartElementQualifiedName(rootNode, qualifiedRootEntity);
                        writer.WriteStartElement(qualifiedEntity.Prefix, qualifiedEntity.LocalName, qualifiedEntity.Namespace);

                        if (useFactory && order == SerializableOrder.Prepend) { factory.ExecuteMethod(); }

                        if (!useXmlWriter)
                        {
                            WriteEnumerable(writer, rootNode, true);
                            if (!rootNode.HasChildren && !isEnumerable) { WriteValue(writer, rootNode, ParseInstanceForXml); }
                            foreach (IHierarchy<object> childNode in SortChildren(rootNode.GetChildren()))
                            {
                                bool hasIgnoreAttribute = childNode.HasMemberReference && TypeUtility.ContainsAttributeType(childNode.MemberReference, typeof(XmlIgnoreAttribute));
                                if (hasIgnoreAttribute) { continue; }
                                if (childNode.HasChildren)
                                {
                                    qualifiedEntity = GetXmlStartElementQualifiedName(childNode);
                                    writer.WriteStartElement(qualifiedEntity.Prefix, qualifiedEntity.LocalName, qualifiedEntity.Namespace);
                                }
                                WriteXml(writer, childNode);
                                if (childNode.HasChildren) { writer.WriteEndElement(); }
                            }
                        }
                        else
                        {
                            if (writeXmlMethod != null) { writeXmlMethod.Invoke(rootNode.Instance, new object[] { writer }); }
                        }

                        if (useFactory && order == SerializableOrder.Append) { factory.ExecuteMethod(); }
                    }
                    catch (Exception ex)
                    {
                        Exception innerException = ex;
                        if (innerException is OutOfMemoryException)
                        { throw; }

                        if (innerException is TargetInvocationException) { innerException = innerException.InnerException; }

                        if (innerException is InvalidOperationException)
                        {
                            if (useFactory)
                            {
                                switch (order)
                                {
                                    case SerializableOrder.Append:
                                        throw new InvalidOperationException(string.Format(CultureInfo.InvariantCulture, "Serializable append operation failed. Check that your serializable append method, {0}, does not invalidate the original serialization.", factory), innerException);
                                    case SerializableOrder.Prepend:
                                        throw new InvalidOperationException(string.Format(CultureInfo.InvariantCulture, "Serializable prepend operation failed. Check that your serializable prepend method, {0}, does not invalidate the original serialization.", factory), innerException);
                                }
                            }
                        }

                        throw ExceptionUtility.Refine(new InvalidOperationException("There is an error in the XML document.", innerException), MethodBaseConverter.FromType(typeof(XmlSerializationUtility)), serializable, order, omitXmlDeclaration, qualifiedRootEntity);
                    }

                    writer.WriteEndElement();
                    writer.Flush();
                }
                tempOutput.Flush();
                tempOutput.Position = 0;
                output = tempOutput;
                tempOutput = null;
            }
            finally
            {
                if (tempOutput != null) { tempOutput.Dispose(); }
            }
            return output;
        }

        private static IEnumerable<IHierarchy<T>> SortChildren<T>(IEnumerable<IHierarchy<T>> sequence)
        {
            List<IHierarchy<T>> attributes = new List<IHierarchy<T>>();
            List<IHierarchy<T>> rest = new List<IHierarchy<T>>();

            foreach (IHierarchy<T> value in sequence)
            {
                XmlAttributeAttribute attribute = value.MemberReference.GetCustomAttribute<XmlAttributeAttribute>();
                if (attribute != null)
                {
                    attributes.Add(value);
                }
                else
                {
                    rest.Add(value);
                }
            }

            return EnumerableUtility.Concat(attributes, rest);
        }

        private static bool IsEnumerable(IHierarchy<object> hierarchy)
        {
            bool hasWrapperAttribute = TypeUtility.ContainsAttributeType(Hierarchy.Root(hierarchy).InstanceType, true, typeof(XmlWrapperAttribute));
            if (hasWrapperAttribute)
            {
                XmlWrapper wrapper = hierarchy.Instance as XmlWrapper;
                if (wrapper != null)
                {
                    return TypeUtility.IsEnumerable(wrapper.InstanceType) && (wrapper.InstanceType != typeof(string));
                }
            }
            return TypeUtility.IsEnumerable(hierarchy.InstanceType) && (hierarchy.InstanceType != typeof(string));
        }

        private static string ParseInstanceForXml<T>(IWrapper<T> wrapper)
        {
            return Wrapper.ParseInstance(wrapper);
        }

        private static bool DefaultXmlSkipPropertiesCallback(Type source)
        {
            if (TypeUtility.ContainsInterface(source, typeof(IXmlSerializable)) && (source.GetMethod("WriteXml", ReflectionUtility.BindingInstancePublicAndPrivate) != null)) { return true; }
            return ReflectionUtility.DefaultSkipPropertiesCallback(source);
        }

        private static bool DefaultXmlSkipPropertyCallback(PropertyInfo propertyToEvaluate)
        {
            if (propertyToEvaluate.Name == "InstanceName" && TypeUtility.ContainsType(propertyToEvaluate.PropertyType, typeof(XmlQualifiedEntity))) { return true; }
            if (TypeUtility.ContainsInterface(propertyToEvaluate.PropertyType, typeof(IXmlSerializable)) && (propertyToEvaluate.PropertyType.GetMethod("WriteXml", ReflectionUtility.BindingInstancePublicAndPrivate) != null)) { return true; }
            return ReflectionUtility.DefaultSkipPropertyCallback(propertyToEvaluate);
        }
    }
}