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