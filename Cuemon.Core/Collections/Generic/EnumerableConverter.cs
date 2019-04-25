using System;
using System.Collections.Generic;
using System.Linq;

namespace Cuemon.Collections.Generic
{
    /// <summary>
    /// This utility class is designed to make <see cref="IEnumerable{T}"/> related conversions easier to work with.
    /// </summary>
    public static class EnumerableConverter
    {
        /// <summary>
        /// Returns the input typed as <see cref="IEnumerable{T}"/>.
        /// </summary>
        /// <typeparam name="TSource">The type of the elements of <paramref name="source"/>.</typeparam>
        /// <param name="source">The array to type as <see cref="IEnumerable{T}"/>.</param>
        /// <returns>The input <paramref name="source"/> typed as <see cref="IEnumerable{T}"/>.</returns>
	    public static IEnumerable<TSource> FromArray<TSource>(params TSource[] source)
        {
            return source;
        }

        /// <summary>
        /// Converts the specified <paramref name="values"/> to a one-dimensional array of the specified type, with zero-based indexing.
        /// </summary>
        /// <typeparam name="TSource">The type of the array of <paramref name="values"/>.</typeparam>
        /// <param name="values">The values to create the <see cref="Array"/> from.</param>
        /// <returns>A one-dimensional <see cref="Array"/> of the specified <see typeparamref="TSource"/> with a length equal to the values specified.</returns>
        public static TSource[] AsArray<TSource>(params TSource[] values)
        {
            return values;
        }

        /// <summary>
        /// Converts the specified <paramref name="source"/> to its <see cref="KeyValuePair{TKey,TValue}"/> equivalent sequence.
        /// </summary>
        /// <typeparam name="TKey">The <see cref="Type"/> of the key in the resulting <see cref="KeyValuePair{TKey,TValue}"/>.</typeparam>
        /// <typeparam name="TValue">The <see cref="Type"/> of the value in the resulting <see cref="KeyValuePair{TKey,TValue}"/>.</typeparam>
        /// <param name="source">An <see cref="IDictionary{TKey,TValue}"/> to convert into a <see cref="KeyValuePair{TKey,TValue}"/> equivalent sequence.</param>
        /// <returns>A <see cref="KeyValuePair{TKey,TValue}"/> equivalent sequence of <paramref name="source"/>.</returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="source"/> is null.
        /// </exception>
        public static IEnumerable<KeyValuePair<TKey, TValue>> FromDictionary<TKey, TValue>(IDictionary<TKey, TValue> source)
        {
            Validator.ThrowIfNull(source, nameof(source));
            foreach (var keyValuePair in source)
            {
                yield return keyValuePair;
            }
        }
    }
}