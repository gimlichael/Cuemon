using System;
using System.Collections.Generic;

namespace Cuemon.Collections.Generic
{
    /// <summary>
    /// This utility class is designed to make <see cref="IDictionary{TKey,TValue}"/> related conversions easier to work with.
    /// </summary>
    public static class DictionaryConverter
    {
        /// <summary>
        /// Creates a <see cref="Dictionary{TKey,TValue}"/> from the specified <paramref name="source"/> sequence.
        /// </summary>
        /// <typeparam name="TKey">The type of keys in the <paramref name="source"/>.</typeparam>
        /// <typeparam name="TValue">The type of values in the <paramref name="source"/>.</typeparam>
        /// <param name="source">The sequence to create a <see cref="Dictionary{TKey,TValue}"/> from.</param>
        /// <returns>A <see cref="Dictionary{TKey,TValue}"/> that is equivalent to the specified <paramref name="source"/> sequence.</returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="source"/> is null.
        /// </exception>
        /// <exception cref="ArgumentException">
        /// <paramref name="source"/> contains at least one <see cref="KeyValuePair{TKey,TValue}"/> that produces duplicate keys for two elements.
        /// </exception>
        public static IDictionary<TKey, TValue> FromEnumerable<TKey, TValue>(IEnumerable<KeyValuePair<TKey, TValue>> source)
        {
            return FromEnumerable(source, EqualityComparer<TKey>.Default);
        }

        /// <summary>
        /// Creates a <see cref="Dictionary{TKey,TValue}"/> from the specified <paramref name="source"/> sequence.
        /// </summary>
        /// <typeparam name="TKey">The type of keys in the <paramref name="source"/>.</typeparam>
        /// <typeparam name="TValue">The type of values in the <paramref name="source"/>.</typeparam>
        /// <param name="source">The sequence to create a <see cref="Dictionary{TKey,TValue}"/> from.</param>
        /// <param name="comparer">The <see cref="IEqualityComparer{T}"/> implementation to use when comparing keys.</param>
        /// <returns>A <see cref="Dictionary{TKey,TValue}"/> that is equivalent to the specified <paramref name="source"/> sequence.</returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="source"/> is null - or - <paramref name="comparer"/> is null.
        /// </exception>
        /// <exception cref="ArgumentException">
        /// <paramref name="source"/> contains at least one <see cref="KeyValuePair{TKey,TValue}"/> that produces duplicate keys for two elements.
        /// </exception>
        public static IDictionary<TKey, TValue> FromEnumerable<TKey, TValue>(IEnumerable<KeyValuePair<TKey, TValue>> source, IEqualityComparer<TKey> comparer)
        {
            Validator.ThrowIfNull(source, nameof(source));
            Validator.ThrowIfNull(comparer, nameof(comparer));

            Dictionary<TKey, TValue> result = new Dictionary<TKey, TValue>(comparer);
            foreach (KeyValuePair<TKey, TValue> item in source)
            {
                result.Add(item.Key, item.Value);
            }
            return result;
        }
    }
}