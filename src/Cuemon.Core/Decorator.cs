using System;

namespace Cuemon
{
    /// <summary>
    /// Provides a way to dynamically enclose/wrap an object to support the decorator pattern.
    /// </summary>
    public static class Decorator
    {
        /// <summary>
        /// Encloses the specified <paramref name="inner"/> so that it can be extended without violating SRP.
        /// </summary>
        /// <typeparam name="T">The type of the <paramref name="inner"/> to decorate.</typeparam>
        /// <param name="inner">The type to decorate.</param>
        /// <param name="throwIfNull"><c>true</c> to throw an <see cref="ArgumentNullException"/> when <paramref name="inner"/> is null; <c>false</c> to allow <paramref name="inner"/> to be null. Default is <c>true</c>.</param>
        /// <returns>An instance of <see cref="Decorator{T}"/>.</returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="inner"/> cannot be null.
        /// </exception>
        public static Decorator<T> Enclose<T>(T inner, bool throwIfNull = true)
        {
            return new Decorator<T>(inner, throwIfNull);
        }

        /// <summary>
        /// Syntactic sugar for the rare cases where retrieving properties exposed as methods is a necessity.
        /// </summary>
        /// <typeparam name="T">The type to decorate.</typeparam>
        /// <returns>An instance of <see cref="Decorator{T}"/> where the <see cref="Decorator{T}.Inner"/> defaults to <typeparamref name="T"/>.</returns>
        public static Decorator<T> Syntactic<T>()
        {
            return new Decorator<T>();
        }
    }

    /// <summary>
    /// Provides a generic way to implement the decorator pattern.
    /// Implements the <see cref="IDecorator{T}" />
    /// </summary>
    /// <typeparam name="T">The type of the inner decorated object.</typeparam>
    /// <seealso cref="IDecorator{T}" />
    public class Decorator<T> : IDecorator<T>
    {
        internal Decorator()
        {
            Inner = default;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Decorator{T}"/> class.
        /// </summary>
        /// <param name="inner">The type to decorate.</param>
        /// <param name="throwIfNull"><c>true</c> to throw an <see cref="ArgumentNullException"/> when <paramref name="inner"/> is null; <c>false</c> to allow <paramref name="inner"/> to be null..</param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="inner"/> cannot be null.
        /// </exception>
        internal Decorator(T inner, bool throwIfNull)
        {
            if (throwIfNull) { Validator.ThrowIfNull(inner); }
            Inner = inner;
        }

        /// <summary>
        /// Gets the inner object of this decorator.
        /// </summary>
        /// <value>The inner object of this decorator.</value>
        public T Inner { get; }
    }
}