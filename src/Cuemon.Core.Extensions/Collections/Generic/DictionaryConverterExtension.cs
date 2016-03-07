using System;
using System.Collections.Generic;

namespace Cuemon.Collections.Generic
{
    /// <summary>
    /// This is an extension implementation of the <see cref="DictionaryConverter"/> class.
    /// </summary>
    public static class DictionaryConverterExtension
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
        public static IDictionary<TKey, TValue> ToDictionary<TKey, TValue>(this IEnumerable<KeyValuePair<TKey, TValue>> source)
        {
            return DictionaryConverter.FromEnumerable(source);
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
        public static IDictionary<TKey, TValue> ToDictionary<TKey, TValue>(this IEnumerable<KeyValuePair<TKey, TValue>> source, IEqualityComparer<TKey> comparer)
        {
            return DictionaryConverter.FromEnumerable(source, comparer);
        }
    }
}