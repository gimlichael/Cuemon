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
    }
}