using System;
using System.Collections.Generic;
using Cuemon.Collections.Generic;

namespace Cuemon.Extensions.Collections.Generic
{
    /// <summary>
    /// Extension methods for the <see cref="IDictionary{TKey,TValue}"/> interface.
    /// </summary>
    public static class DictionaryExtensions
    {
        /// <summary>
        /// Gets the value associated with the specified <paramref name="key"/> or <c>default(<typeparamref name="TValue"/>)</c> when the key does not exists in the <paramref name="dictionary"/>.
        /// </summary>
        /// <typeparam name="TKey">The type of the keys in the <paramref name="dictionary"/>.</typeparam>
        /// <typeparam name="TValue">The type of the values in the <paramref name="dictionary"/>.</typeparam>
        /// <param name="dictionary">The <see cref="IDictionary{TKey,TValue}"/> to extend.</param>
        /// <param name="key">The key of the value to get.</param>
        /// <returns>Either the value associated with the specified <paramref name="key"/> or <c>default(<typeparamref name="TValue"/>)</c> when the key does not exists.</returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="dictionary"/> cannot be null -or-
        /// <paramref name="key"/> cannot be null.
        /// </exception>
        public static TValue GetValueOrDefault<TKey, TValue>(this IDictionary<TKey, TValue> dictionary, TKey key)
        {
            Validator.ThrowIfNull(dictionary, nameof(dictionary));
            return Decorator.Enclose(dictionary).GetValueOrDefault(key);
        }

        /// <summary>
        /// Gets the value associated with the specified <paramref name="key"/> or a default value through <paramref name="defaultProvider"/> when the key does not exists in the <paramref name="dictionary"/>.
        /// </summary>
        /// <typeparam name="TKey">The type of the keys in the <paramref name="dictionary"/>.</typeparam>
        /// <typeparam name="TValue">The type of the values in the <paramref name="dictionary"/>.</typeparam>
        /// <param name="dictionary">The <see cref="IDictionary{TKey,TValue}"/> to extend.</param>
        /// <param name="key">The key of the value to get.</param>
        /// <param name="defaultProvider">The function delegate that will provide a default value when the <paramref name="key"/> does not exists in the <paramref name="dictionary"/>.</param>
        /// <returns>Either the value associated with the specified <paramref name="key"/> or a default value through <paramref name="defaultProvider"/> when the key does not exists.</returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="dictionary"/> cannot be null -or-
        /// <paramref name="key"/> cannot be null -or-
        /// <paramref name="defaultProvider"/> cannot be null.
        /// </exception>
        public static TValue GetValueOrDefault<TKey, TValue>(this IDictionary<TKey, TValue> dictionary, TKey key, Func<TValue> defaultProvider)
        {
            Validator.ThrowIfNull(dictionary, nameof(dictionary));
            return Decorator.Enclose(dictionary).GetValueOrDefault(key, defaultProvider);
        }

        /// <summary>
        /// Gets the <paramref name="value"/> associated with the specified <paramref name="key"/>.
        /// </summary>
        /// <typeparam name="TKey">The type of the keys in the <paramref name="dictionary"/>.</typeparam>
        /// <typeparam name="TValue">The type of the values in the <paramref name="dictionary"/>.</typeparam>
        /// <param name="dictionary">The <see cref="IDictionary{TKey,TValue}"/> to extend.</param>
        /// <param name="key">The key of the value to get.</param>
        /// <param name="fallbackKeySelector">The function delegate that will resolve an alternate key from the specified <paramref name="key"/>.</param>
        /// <param name="value">When this method returns, contains the value associated with the specified <paramref name="key"/> or the alternate key resolved from <paramref name="fallbackKeySelector"/>, if the key is found; otherwise, the default value for the type of the value parameter. This parameter is passed uninitialized.</param>
        /// <returns><c>true</c> if the <paramref name="dictionary"/> contains an element with the specified <paramref name="key"/> or the alternate key resolved from <paramref name="fallbackKeySelector"/>, <c>false</c> otherwise.</returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="dictionary"/> cannot be null.
        /// </exception>
        public static bool TryGetValueOrFallback<TKey, TValue>(this IDictionary<TKey, TValue> dictionary, TKey key, Func<IEnumerable<TKey>, TKey> fallbackKeySelector, out TValue value)
        {
            Validator.ThrowIfNull(dictionary, nameof(dictionary));
            return Decorator.Enclose(dictionary).TryGetValueOrFallback(key, fallbackKeySelector, out value);
        }

        /// <summary>
        /// Converts the specified <paramref name="dictionary"/> to its <see cref="KeyValuePair{TKey,TValue}"/> equivalent sequence.
        /// </summary>
        /// <typeparam name="TKey">The <see cref="Type"/> of the key in the resulting <see cref="KeyValuePair{TKey,TValue}"/>.</typeparam>
        /// <typeparam name="TValue">The <see cref="Type"/> of the value in the resulting <see cref="KeyValuePair{TKey,TValue}"/>.</typeparam>
        /// <param name="dictionary">The <see cref="IDictionary{TKey,TValue}"/> to extend.</param>
        /// <returns>A <see cref="KeyValuePair{TKey,TValue}"/> equivalent sequence of <paramref name="dictionary"/>.</returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="dictionary"/> is null.
        /// </exception>
        public static IEnumerable<KeyValuePair<TKey, TValue>> ToEnumerable<TKey, TValue>(this IDictionary<TKey, TValue> dictionary)
        {
            Validator.ThrowIfNull(dictionary, nameof(dictionary));
            return Decorator.Enclose(dictionary).ToEnumerable();
        }

        /// <summary>
        /// Attempts to add the specified <paramref name="key"/> and <paramref name="value"/> to the <paramref name="dictionary"/>.
        /// </summary>
        /// <param name="dictionary">The <see cref="IDictionary{TKey,TValue}"/> to extend.</param>
        /// <param name="key">The key of the element to add.</param>
        /// <param name="value">The value of the element to add.</param>
        /// <param name="condition">The function delegate that specifies the condition for adding the element.</param>
        /// <returns><c>true</c> if the key/value pair was added to the <paramref name="dictionary"/> successfully; otherwise, <c>false</c>.</returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="dictionary"/> cannot be null -or-
        /// <paramref name="key"/> cannot be null -or-
        /// <paramref name="condition"/> cannot be null.
        /// </exception>
        public static bool TryAdd<TKey, TValue>(this IDictionary<TKey, TValue> dictionary, TKey key, TValue value, Func<IDictionary<TKey, TValue>, bool> condition)
        {
            Validator.ThrowIfNull(dictionary, nameof(dictionary));
            return Decorator.Enclose(dictionary).TryAdd(key, value, condition);
        }

        /// <summary>
        /// Attempts to add the specified <paramref name="key"/> and <paramref name="value"/> to the <paramref name="dictionary"/>.
        /// </summary>
        /// <param name="dictionary">The <see cref="IDictionary{TKey,TValue}"/> to extend.</param>
        /// <param name="key">The key of the element to add.</param>
        /// <param name="value">The value of the element to add.</param>
        /// <returns><c>true</c> if the key/value pair was added to the enclosed <see cref="IDictionary{TKey,TValue}"/> of the <paramref name="dictionary"/> successfully; otherwise, <c>false</c>.</returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="dictionary"/> cannot be null -or-
        /// <paramref name="key"/> cannot be null.
        /// </exception>
        public static bool TryAdd<TKey, TValue>(this IDictionary<TKey, TValue> dictionary, TKey key, TValue value)
        {
            Validator.ThrowIfNull(dictionary, nameof(dictionary));
            return Decorator.Enclose(dictionary).TryAdd(key, value);
        }

        /// <summary>
        /// Attempts to add or update an existing element with the provided <paramref name="key"/> to the <paramref name="dictionary"/> with the specified <paramref name="value"/>.
        /// </summary>
        /// <param name="dictionary">The <see cref="IDictionary{TKey,TValue}"/> to extend.</param>
        /// <param name="key">The key of the element to add or update.</param>
        /// <param name="value">The value of the element to add or update.</param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="dictionary"/> cannot be null -or-
        /// <paramref name="key"/> cannot be null.
        /// </exception>
        public static void AddOrUpdate<TKey, TValue>(this IDictionary<TKey, TValue> dictionary, TKey key, TValue value)
        {
            Validator.ThrowIfNull(dictionary, nameof(dictionary));
            Decorator.Enclose(dictionary).AddOrUpdate(key, value);
        }
    }
}