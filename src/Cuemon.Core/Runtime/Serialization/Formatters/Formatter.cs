using System;

namespace Cuemon.Runtime.Serialization.Formatters
{
    /// <summary>
    /// An abstract class that supports serialization and deserialization of an object, in a given format.
    /// </summary>
    /// <typeparam name="TFormat">The type of format which serialization and deserialization is invoked.</typeparam>
    public abstract class Formatter<TFormat>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Formatter{TFormat}"/> class.
        /// </summary>
        protected Formatter()
        {
        }

        /// <summary>
        /// Serializes the specified <paramref name="source"/> to an object of <typeparamref name="TFormat" />.
        /// </summary>
        /// <param name="source">The object to serialize to a given format.</param>
        /// <returns>An object of the serialized <paramref name="source" />.</returns>
        public TFormat Serialize(object source)
        {
            Validator.ThrowIfNull(source);
            return Serialize(source, source.GetType());
        }

        /// <summary>
        /// Serializes the object of this instance to an object of <typeparamref name="TFormat" />.
        /// </summary>
        /// <param name="source">The object to serialize to a given format.</param>
        /// <param name="objectType">The type of the object to serialize.</param>
        /// <returns>An object of the serialized <paramref name="source"/>.</returns>
        public abstract TFormat Serialize(object source, Type objectType);

        /// <summary>
        /// Deserializes the specified <paramref name="value"/> of <typeparamref name="TFormat"/> into an object of <typeparamref name="T"/>.
        /// </summary>
        /// <typeparam name="T">The type of the object to return.</typeparam>
        /// <param name="value">The object from which to deserialize the object graph.</param>
        /// <returns>An object of <typeparamref name="T" />.</returns>
        public T Deserialize<T>(TFormat value)
        {
            Validator.ThrowIfNull(value);
            return (T)Deserialize(value, typeof(T));
        }

        /// <summary>
        /// Deserializes the specified <paramref name="value"/> of <typeparamref name="TFormat"/> into an object of <paramref name="objectType"/>.
        /// </summary>
        /// <param name="value">The object from which to deserialize the object graph.</param>
        /// <param name="objectType">The type of the deserialized object.</param>
        /// <returns>An object of <paramref name="objectType"/>.</returns>
        public abstract object Deserialize(TFormat value, Type objectType);
    }
}