using System;
using System.Collections.Generic;
using Cuemon.Collections.Generic;

namespace Cuemon.Extensions.Collections.Generic
{
    /// <summary>
    /// Extension methods for the <see cref="IList{T}"/> interface.
    /// </summary>
    public static class ListExtensions
    {
        /// <summary>
        /// Removes the first occurrence of a specific object from the <paramref name="list"/>.
        /// </summary>
        /// <typeparam name="T">The type of elements in the <see cref="IList{T}"/>.</typeparam>
        /// <param name="list">The extended list.</param>
        /// <param name="predicate">The function delegate that defines the conditions of the element to remove.</param>
        /// <returns><c>true</c> if item was successfully removed from the <paramref name="list"/>, <c>false</c> otherwise.</returns>
        public static bool Remove<T>(this IList<T> list, Func<T, bool> predicate)
        {
            for (var i = 0; i < list.Count; i++)
            {
                if (predicate(list[i]))
                {
                    list.RemoveAt(i);
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// Determines whether the <paramref name="elements"/> of the <see cref="IList{T}"/> is within the range of the <paramref name="index"/>.
        /// </summary>
        /// <typeparam name="TSource">The type of elements in the <see cref="IList{T}"/>.</typeparam>
        /// <param name="elements">The elements of the <see cref="IList{T}"/>.</param>
        /// <param name="index">The index to find.</param>
        /// <returns><c>true</c> if the specified <paramref name="index"/> is within the range of the <paramref name="elements"/>; otherwise, <c>false</c>.</returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="elements"/> is null.
        /// </exception>
        public static bool HasIndex<TSource>(this IList<TSource> elements, int index)
        {
            Validator.ThrowIfNull(elements, nameof(elements));
            return ((elements.Count - 1) >= index);
        }

        /// <summary>
        /// Returns the next element of <paramref name="elements"/> relative to <paramref name="index"/>, or the last element of <paramref name="elements"/> if <paramref name="index"/> is equal or greater than <see cref="ICollection{T}.Count"/>.
        /// </summary>
        /// <typeparam name="TSource">The type of elements in the <see cref="IList{T}"/>.</typeparam>
        /// <param name="elements">The elements, relative to <paramref name="index"/>, to return the next element of.</param>
        /// <param name="index">The index of which to advance to the next element from.</param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="elements"/> is null.
        /// </exception>
        /// <exception cref="ArgumentOutOfRangeException">
        /// <paramref name="index"/> is less than 0.
        /// </exception>
        /// <returns>default(TSource) if <paramref name="index"/> is equal or greater than <see cref="ICollection{T}.Count"/>; otherwise the next element of <paramref name="elements"/> relative to <paramref name="index"/>.</returns>
        public static TSource Next<TSource>(this IList<TSource> elements, int index)
        {
            Validator.ThrowIfNull(elements, nameof(elements));
            Validator.ThrowIfLowerThan(index, 0, nameof(index));
            var nextIndex = index + 1;
            if (nextIndex >= elements.Count) { return default; }
            return elements[nextIndex];
        }

        /// <summary>
        /// Returns the previous element of <paramref name="elements"/> relative to <paramref name="index"/>, or the first or last element of <paramref name="elements"/> if <paramref name="index"/> is equal, greater or lower than <see cref="ICollection{T}.Count"/>.
        /// </summary>
        /// <typeparam name="TSource">The type of elements in the <see cref="IList{T}"/>.</typeparam>
        /// <param name="elements">The elements, relative to <paramref name="index"/>, to return the previous element of.</param>
        /// <param name="index">The index of which to advance to the previous element from.</param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="elements"/> is null.
        /// </exception>
        /// <exception cref="ArgumentOutOfRangeException">
        /// <paramref name="index"/> is less than 0.
        /// </exception>
        /// <returns>default(TSource) if <paramref name="index"/> is equal, greater or lower than <see cref="ICollection{T}.Count"/>; otherwise the previous element of <paramref name="elements"/> relative to <paramref name="index"/>.</returns>
        public static TSource Previous<TSource>(this IList<TSource> elements, int index)
        {
            Validator.ThrowIfNull(elements, nameof(elements));
            Validator.ThrowIfLowerThan(index, 0, nameof(index));
            var previousIndex = index - 1;
            if (previousIndex < 0) { return default; }
            if (previousIndex >= elements.Count) { return default; }
            return elements[previousIndex];
        }
    }
}