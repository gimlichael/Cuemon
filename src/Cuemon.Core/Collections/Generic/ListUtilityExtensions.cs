using System;
using System.Collections.Generic;

namespace Cuemon.Collections.Generic
{
    /// <summary>
    /// This is an extension implementation of the most common methods on the <see cref="ListUtility"/> class.
    /// </summary>
    public static class ListUtilityExtensions
    {
        /// <summary>
        /// Determines whether the <paramref name="elements"/> of the <see cref="IList{T}"/> is within the range of the <paramref name="index"/>.
        /// </summary>
        /// <typeparam name="TSource">The type of elements in the <see cref="IList{T}"/>.</typeparam>
        /// <param name="elements">The elements of the <see cref="IList{T}"/>.</param>
        /// <param name="index">The index to find.</param>
        /// <returns><c>true</c> if the specified <paramref name="index"/> is within the range of the <paramref name="elements"/>; otherwise, <c>false</c>.</returns>
        /// <exception cref="System.ArgumentNullException">
        /// <paramref name="elements"/> is null.
        /// </exception>
        public static bool HasIndex<TSource>(this IList<TSource> elements, int index)
        {
            return ListUtility.HasIndex(index, elements);
        }

        /// <summary>
        /// Returns the next element of <paramref name="elements"/> relative to <paramref name="index"/>, or the last element of <paramref name="elements"/> if <paramref name="index"/> is equal or greater than <see cref="ICollection{T}.Count"/>.
        /// </summary>
        /// <typeparam name="TSource">The type of elements in the <see cref="IList{T}"/>.</typeparam>
        /// <param name="elements">The elements, relative to <paramref name="index"/>, to return the next element of.</param>
        /// <param name="index">The index of which to advance to the next element from.</param>
        /// <exception cref="ArgumentOutOfRangeException">
        /// <paramref name="index"/> is less than 0.
        /// </exception>
        /// <returns>default(TSource) if <paramref name="index"/> is equal or greater than <see cref="ICollection{T}.Count"/>; otherwise the next element of <paramref name="elements"/> relative to <paramref name="index"/>.</returns>
        public static TSource Next<TSource>(this IList<TSource> elements, int index)
        {
            return ListUtility.Next(index, elements);
        }

        /// <summary>
        /// Returns the previous element of <paramref name="elements"/> relative to <paramref name="index"/>, or the first or last element of <paramref name="elements"/> if <paramref name="index"/> is equal, greater or lower than <see cref="ICollection{T}.Count"/>.
        /// </summary>
        /// <typeparam name="TSource">The type of elements in the <see cref="IList{T}"/>.</typeparam>
        /// <param name="elements">The elements, relative to <paramref name="index"/>, to return the previous element of.</param>
        /// <param name="index">The index of which to advance to the previous element from.</param>
        /// <exception cref="ArgumentOutOfRangeException">
        /// <paramref name="index"/> is less than 0.
        /// </exception>
        /// <returns>default(TSource) if <paramref name="index"/> is equal, greater or lower than <see cref="ICollection{T}.Count"/>; otherwise the previous element of <paramref name="elements"/> relative to <paramref name="index"/>.</returns>
        public static TSource Previous<TSource>(this IList<TSource> elements, int index)
        {
            return ListUtility.Previous(index, elements);
        }
    }
}
