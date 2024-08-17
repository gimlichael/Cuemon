using System;
using System.Collections;
using System.Collections.Generic;
using Cuemon.Collections.Generic;

namespace Cuemon
{
    /// <summary>
    /// Represents a collection of function delegates that converts an <see cref="IConvertible"/> implementation to its <see cref="T:byte[]"/> equivalent.
    /// </summary>
    public class ConvertibleConverterDictionary : IReadOnlyDictionary<Type, Func<IConvertible, byte[]>>
    {
        private readonly Dictionary<Type, Func<IConvertible, byte[]>> _converters = new();

        /// <summary>
        /// Adds the function delegate that converts an <see cref="IConvertible"/> implementation to its <see cref="T:byte[]"/> equivalent to the specified <typeparamref name="T"/>.
        /// </summary>
        /// <typeparam name="T">The type that implements <see cref="IConvertible"/>.</typeparam>
        /// <param name="converter">The function delegate that converts an <see cref="IConvertible"/> implementation to its <see cref="T:byte[]"/> equivalent.</param>
        /// <returns>An <see cref="ConvertibleConverterDictionary"/> that can be used to further configure other converters.</returns>
        /// <exception cref="TypeArgumentOutOfRangeException">
        /// <typeparamref name="T"/> does not implement <see cref="IConvertible"/>.
        /// </exception>
        public ConvertibleConverterDictionary Add<T>(Func<T, byte[]> converter) where T : IConvertible
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
        /// <returns>An <see cref="ConvertibleConverterDictionary"/> that can be used to further configure other converters.</returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="type"/> is null.
        /// </exception>
        /// <exception cref="ArgumentOutOfRangeException">
        /// <paramref name="type"/> does not implement <see cref="IConvertible"/>.
        /// </exception>
        public ConvertibleConverterDictionary Add(Type type, Func<IConvertible, byte[]> converter)
        {
            Validator.ThrowIfNotContainsInterface(type, Arguments.ToArrayOf(typeof(IConvertible)));
            _converters.Add(type, converter);
            return this;
        }

        /// <summary>
        /// Determines whether the dictionary contains an element that has the specified key.
        /// </summary>
        /// <param name="key">The key to locate.</param>
        /// <returns><c>true</c> if the dictionary contains an element that has the specified key; otherwise, <c>false</c>.</returns>
        public bool ContainsKey(Type key)
        {
            return _converters.ContainsKey(key);
        }

        /// <summary>
        /// Gets the value that is associated with the specified key.
        /// </summary>
        /// <param name="key">The key to locate.</param>
        /// <param name="value">When this method returns, the value associated with the specified key, if the key is found; otherwise, the default value for the type of the value parameter. This parameter is passed uninitialized.</param>
        /// <returns><c>true</c> if the dictionary contains an element that has the specified key; otherwise, <c>false</c>.</returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="key"/> is null.
        /// </exception>
        public bool TryGetValue(Type key, out Func<IConvertible, byte[]> value)
        {
            return _converters.TryGetValue(key, out value);
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
        /// Gets an enumerable collection that contains the keys in the dictionary.
        /// </summary>
        /// <value>An enumerable collection that contains the keys in the dictionary.</value>
        public IEnumerable<Type> Keys => _converters.Keys;

        /// <summary>
        /// Gets an enumerable collection that contains the values in the dictionary.
        /// </summary>
        /// <value>An enumerable collection that contains the values in the dictionary.</value>
        public IEnumerable<Func<IConvertible, byte[]>> Values => _converters.Values;

        /// <summary>
        /// Gets the number of converters contained in this instance.
        /// </summary>
        /// <value>The number of converters contained in this instance.</value>
        public int Count => _converters.Count;

        /// <summary>
        /// Returns an enumerator that iterates through the collection.
        /// </summary>
        /// <returns>An enumerator that can be used to iterate through the collection.</returns>
        public IEnumerator<KeyValuePair<Type, Func<IConvertible, byte[]>>> GetEnumerator()
        {
            return _converters.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}