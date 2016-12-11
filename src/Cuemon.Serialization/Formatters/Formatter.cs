using System;

namespace Cuemon.Serialization.Formatters
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
        /// <param name="source">The object to serialize and deserialize to and from a given format.</param>
        /// <param name="sourceType">The type of the object to serialize and deserialize.</param>
        protected Formatter(object source, Type sourceType)
        {
            Validator.ThrowIfNull(source, nameof(source));
            Validator.ThrowIfNull(sourceType, nameof(sourceType));
            Source = source;
            SourceType = sourceType;
        }

        /// <summary>
        /// Gets the object to serialize and deserialize to and from a given format.
        /// </summary>
        /// <value>The object to be formatted.</value>
        protected object Source { get; }

        /// <summary>
        /// Gets the type of the object to serialize and deserialize.
        /// </summary>
        /// <value>The type of the object to be formatted.</value>
        protected Type SourceType { get; }

        /// <summary>
        /// Validates the first parameter for the base class <see cref="Formatter{TFormat}"/>.
        /// </summary>
        /// <param name="source">The object to serialize and deserialize to and from a given format.</param>
        protected static object ValidateBase(object source)
        {
            Validator.ThrowIfNull(source, nameof(source));
            return source;
        }

        /// <summary>
        /// Validates the second parameter for the base class <see cref="Formatter{TFormat}"/>.
        /// </summary>
        /// <param name="sourceType">The type of the object to serialize and deserialize.</param>
        protected static Type ValidateBase(Type sourceType)
        {
            Validator.ThrowIfNull(sourceType, nameof(sourceType));
            return sourceType;
        }

        /// <summary>
        /// Serializes the object of this instance to an object of <typeparamref name="TFormat"/>.
        /// </summary>
        /// <returns>An object of the serialized <see cref="Formatter{TFormat}.Source"/>.</returns>
        public abstract TFormat Serialize();

        /// <summary>
        /// Deserializes the specified <paramref name="value"/> of <typeparamref name="TFormat"/> into an object of <typeparamref name="T"/>.
        /// </summary>
        /// <typeparam name="T">The type of the object to return.</typeparam>
        /// <param name="value">The object from which to deserialize the object graph.</param>
        /// <returns>An object of <typeparamref name="T" />.</returns>
        public abstract T Deserialize<T>(TFormat value);
    }
}