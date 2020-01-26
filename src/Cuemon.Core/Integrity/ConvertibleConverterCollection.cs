using System;
using System.Collections.Generic;

namespace Cuemon.Integrity
{
    /// <summary>
    /// Represents a collection of function delegates that converts an <see cref="IConvertible"/> implementation to its <see cref="T:byte[]"/> equivalent.
    /// </summary>
    public class ConvertibleConverterCollection
    {
        private readonly Dictionary<Type, Func<IConvertible, byte[]>> _converters = new Dictionary<Type, Func<IConvertible, byte[]>>();

        /// <summary>
        /// Adds the function delegate that converts an <see cref="IConvertible"/> implementation to its <see cref="T:byte[]"/> equivalent to the specified <typeparamref name="T"/>.
        /// </summary>
        /// <typeparam name="T">The type that implements <see cref="IConvertible"/>.</typeparam>
        /// <param name="converter">The function delegate that converts an <see cref="IConvertible"/> implementation to its <see cref="T:byte[]"/> equivalent.</param>
        /// <returns>An <see cref="ConvertibleConverterCollection"/> that can be used to further configure other converters.</returns>
        /// <exception cref="TypeArgumentOutOfRangeException">
        /// <typeparamref name="T"/> does not implement <see cref="IConvertible"/>.
        /// </exception>
        public ConvertibleConverterCollection Add<T>(Func<T, byte[]> converter) where T : IConvertible
        {
            Validator.ThrowIfNotContainsInterface<T>(nameof(T), typeof(IConvertible));
            Add(typeof(T), c => converter((T)c));
            return this;
        }

        /// <summary>
        /// Adds the function delegate that converts an <see cref="IConvertible"/> implementation to its <see cref="T:byte[]"/> equivalent to the specified <paramref name="type"/>.
        /// </summary>
        /// <param name="type">The type that implements <see cref="IConvertible"/>.</param>
        /// <param name="converter">The function delegate that converts an <see cref="IConvertible"/> implementation to its <see cref="T:byte[]"/> equivalent.</param>
        /// <returns>An <see cref="ConvertibleConverterCollection"/> that can be used to further configure other converters.</returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="type"/> is null.
        /// </exception>
        /// <exception cref="ArgumentOutOfRangeException">
        /// <paramref name="type"/> does not implement <see cref="IConvertible"/>.
        /// </exception>
        public ConvertibleConverterCollection Add(Type type, Func<IConvertible, byte[]> converter)
        {
            Validator.ThrowIfNotContainsInterface(type, nameof(type), typeof(IConvertible));
            _converters.Add(type, converter);
            return this;
        }

        /// <summary>
        /// Gets the function delegate that converts an <see cref="IConvertible"/> implementation to its <see cref="T:byte[]"/> equivalent from the specified <paramref name="type"/>.
        /// </summary>
        /// <param name="type">The type that implements <see cref="IConvertible"/>.</param>
        /// <returns>The function delegate associated with the specified <paramref name="type"/>; null if no association exists.</returns>
        public Func<IConvertible, byte[]> this[Type type]
        {
            get
            {
                if (type == null) { return null; }
                return _converters.TryGetValue(type, out var converter) ? converter : null;
            }
        }

        /// <summary>
        /// Gets the number of converters contained in this instance.
        /// </summary>
        /// <value>The number of converters contained in this instance.</value>
        public int Count => _converters.Count;
    }
}