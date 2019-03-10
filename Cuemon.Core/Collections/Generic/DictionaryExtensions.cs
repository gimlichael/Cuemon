using System;
using System.Collections.Generic;

namespace Cuemon.Collections.Generic
{
    /// <summary>
    /// Extension methods for the <see cref="Dictionary{TKey,TValue}"/>.
    /// </summary>
    public static class DictionaryExtensions
    {
        /// <summary>
        /// Gets the value associated with the specified <paramref name="key"/> or <c>default(<typeparamref name="TValue"/>)</c> when the key does not exists in the <paramref name="dictionary"/>.
        /// </summary>
        /// <typeparam name="TKey">The type of the keys in the <paramref name="dictionary"/>.</typeparam>
        /// <typeparam name="TValue">The type of the values in the <paramref name="dictionary"/>.</typeparam>
        /// <param name="dictionary">The dictionary to extend.</param>
        /// <param name="key">The key of the value to get.</param>
        /// <returns>Either the value associated with the specified <paramref name="key"/> or <c>default(<typeparamref name="TValue"/>)</c> when the key does not exists.</returns>
        public static TValue GetValueOrDefault<TKey, TValue>(this IDictionary<TKey, TValue> dictionary, TKey key)
        {
            return GetValueOrDefault(dictionary, key, () => default(TValue));
        }

        /// <summary>
        /// Gets the value associated with the specified <paramref name="key"/> or a default value through <paramref name="defaultProvider"/> when the key does not exists in the <paramref name="dictionary"/>.
        /// </summary>
        /// <typeparam name="TKey">The type of the keys in the <paramref name="dictionary"/>.</typeparam>
        /// <typeparam name="TValue">The type of the values in the <paramref name="dictionary"/>.</typeparam>
        /// <param name="dictionary">The dictionary to extend.</param>
        /// <param name="key">The key of the value to get.</param>
        /// <param name="defaultProvider">The function delegate that will provide a default value when the <paramref name="key"/> does not exists in the <paramref name="dictionary"/>.</param>
        /// <returns>Either the value associated with the specified <paramref name="key"/> or a default value through <paramref name="defaultProvider"/> when the key does not exists.</returns>
        public static TValue GetValueOrDefault<TKey, TValue>(this IDictionary<TKey, TValue> dictionary, TKey key, Func<TValue> defaultProvider)
        {
            Validator.ThrowIfNull(dictionary, nameof(dictionary));
            Validator.ThrowIfNull(key, nameof(key));
            Validator.ThrowIfNull(defaultProvider, nameof(defaultProvider));
            TValue value;
            return dictionary.TryGetValue(key, out value) ? value : defaultProvider();
        }

        /// <summary>
        /// Gets the <paramref name="value"/> associated with the specified <paramref name="key"/>.
        /// </summary>
        /// <typeparam name="TKey">The type of the keys in the <paramref name="dictionary"/>.</typeparam>
        /// <typeparam name="TValue">The type of the values in the <paramref name="dictionary"/>.</typeparam>
        /// <param name="dictionary">The dictionary to extend.</param>
        /// <param name="key">The key of the value to get.</param>
        /// <param name="keySelector">The function delegate that will resolve an alternate key from the specified <paramref name="key"/>.</param>
        /// <param name="value">When this method returns, contains the value associated with the specified <paramref name="key"/> or the alternate key resolved from <paramref name="keySelector"/>, if the key is found; otherwise, the default value for the type of the value parameter. This parameter is passed uninitialized.</param>
        /// <returns><c>true</c> if the <paramref name="dictionary"/> contains an element with the specified <paramref name="key"/> or the alternate key resolved from <paramref name="keySelector"/>, <c>false</c> otherwise.</returns>
        public static bool TryGetValue<TKey, TValue>(this IDictionary<TKey, TValue> dictionary, TKey key, Func<IEnumerable<TKey>, TKey> keySelector, out TValue value)
        {
            if (!dictionary.TryGetValue(key, out value))
            {
                var alternateKey = keySelector(dictionary.Keys);
                return alternateKey != null && dictionary.TryGetValue(alternateKey, out value);
            }
            return true;
        }

        /// <summary>
        /// Converts the specified <paramref name="source"/> to its <see cref="KeyValuePair{TKey,TValue}"/> equivalent sequence.
        /// </summary>
        /// <typeparam name="TKey">The <see cref="Type"/> of the key in the resulting <see cref="KeyValuePair{TKey,TValue}"/>.</typeparam>
        /// <typeparam name="TValue">The <see cref="Type"/> of the value in the resulting <see cref="KeyValuePair{TKey,TValue}"/>.</typeparam>
        /// <param name="source">An <see cref="IDictionary{TKey,TValue}"/> to convert into a <see cref="KeyValuePair{TKey,TValue}"/> equivalent sequence.</param>
        /// <returns>A <see cref="KeyValuePair{TKey,TValue}"/> equivalent sequence of <paramref name="source"/>.</returns>
        /// <exception cref="System.ArgumentNullException">
        /// <paramref name="source"/> is null.
        /// </exception>
        public static IEnumerable<KeyValuePair<TKey, TValue>> ToEnumerable<TKey, TValue>(this IDictionary<TKey, TValue> source)
        {
            return EnumerableConverter.FromDictionary(source);
        }

        /// <summary>
        /// Returns the first <typeparamref name="TValue"/> matching one of the specified <paramref name="keys"/> in a <see cref="IDictionary{TKey,TValue}"/>, or a default value if the <paramref name="source"/> contains no elements or no match was found.
        /// </summary>
        /// <typeparam name="TKey">The <see cref="Type"/> of the key.</typeparam>
        /// <typeparam name="TValue">The <see cref="Type"/> of the value.</typeparam>
        /// <param name="source">The <see cref="IDictionary{TKey, TValue}"/> to return a matching <typeparamref name="TValue"/> from.</param>
        /// <param name="keys">A variable number of keys to match in the specified <paramref name="source"/>.</param>
        /// <returns>default(TValue) if source is empty or no match was found; otherwise, the matching element in <paramref name="source"/>.</returns>
        /// <remarks>The default value for reference and nullable types is null.</remarks>
        public static TValue FirstMatchOrDefault<TKey, TValue>(this IDictionary<TKey, TValue> source, params TKey[] keys)
        {
            return DictionaryUtility.FirstMatchOrDefault(source, keys);
        }

        /// <summary>
        /// Adds an element with the provided <paramref name="key"/> and <paramref name="value"/> to the <see cref="IDictionary{TKey,TValue}"/>, if the <paramref name="key"/> is not contained within the <paramref name="source"/>.
        /// </summary>
        /// <typeparam name="TKey">The type of keys in the dictionary.</typeparam>
        /// <typeparam name="TValue">The type of values in the dictionary.</typeparam>
        /// <param name="source">The <see cref="IDictionary{TKey, TValue}"/> to perform the operation.</param>
        /// <param name="key">The object to use as the key of the element to add.</param>
        /// <param name="value">The object to use as the value of the element to add.</param>
        public static void AddIfNotContainsKey<TKey, TValue>(this IDictionary<TKey, TValue> source, TKey key, TValue value)
        {
            if (source.ContainsKey(key)) { return; }
            source.Add(key, value);
        }

        /// <summary>
        /// Adds or updates an existing element with the provided <paramref name="key"/> in the <see cref="IDictionary{TKey,TValue}"/> with the specified <paramref name="value"/>.
        /// </summary>
        /// <typeparam name="TKey">The type of keys in the dictionary.</typeparam>
        /// <typeparam name="TValue">The type of values in the dictionary.</typeparam>
        /// <param name="source">The <see cref="IDictionary{TKey, TValue}"/> to perform the operation.</param>
        /// <param name="key">The object to use as the key of the element to add.</param>
        /// <param name="value">The object to use as the value of the element to add or update.</param>
        public static void AddOrUpdate<TKey, TValue>(this IDictionary<TKey, TValue> source, TKey key, TValue value)
        {
            if (source.ContainsKey(key))
            {
                source[key] = value;
            }
            else
            {
                source.Add(key, value);
            }
        }
    }
}