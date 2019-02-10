using System;
using System.Collections.Generic;

namespace Cuemon.Collections.Generic
{
    /// <summary>
    /// This utility class is designed to make <see cref="IList{T}"/> related conversions easier to work with.
    /// </summary>
	public static class ListUtility
	{
        /// <summary>
        /// Determines whether the <paramref name="elements"/> of the <see cref="IList{T}"/> is within the range of the <paramref name="index"/>.
        /// </summary>
        /// <typeparam name="TSource">The type of elements in the <see cref="IList{T}"/>.</typeparam>
        /// <param name="index">The index to find.</param>
        /// <param name="elements">The elements of the <see cref="IList{T}"/>.</param>
        /// <returns><c>true</c> if the specified <paramref name="index"/> is within the range of the <paramref name="elements"/>; otherwise, <c>false</c>.</returns>
        /// <exception cref="System.ArgumentNullException">
        /// <paramref name="elements"/> is null.
        /// </exception>
        public static bool HasIndex<TSource>(int index, IList<TSource> elements)
        {
            Validator.ThrowIfNull(elements, nameof(elements));
            return ((elements.Count - 1) >= index);
        }

        /// <summary>
        /// Returns the next element of <paramref name="elements"/> relative to <paramref name="index"/>, or the last element of <paramref name="elements"/> if <paramref name="index"/> is equal or greater than <see cref="ICollection{T}.Count"/>.
        /// </summary>
        /// <typeparam name="TSource">The type of elements in the <see cref="IList{T}"/>.</typeparam>
        /// <param name="index">The index of which to advance to the next element from.</param>
        /// <param name="elements">The elements, relative to <paramref name="index"/>, to return the next element of.</param>
        /// <exception cref="ArgumentOutOfRangeException">
        /// <paramref name="index"/> is less than 0.
        /// </exception>
        /// <returns>default(TSource) if <paramref name="index"/> is equal or greater than <see cref="ICollection{T}.Count"/>; otherwise the next element of <paramref name="elements"/> relative to <paramref name="index"/>.</returns>
        public static TSource Next<TSource>(int index, IList<TSource> elements)
		{
            Validator.ThrowIfNull(elements, nameof(elements));
            Validator.ThrowIfLowerThan(index, 0, nameof(index));
            int nextIndex = index + 1;
            if (nextIndex >= elements.Count) { return default(TSource); }
		    return elements[nextIndex];
		}

        /// <summary>
        /// Returns the previous element of <paramref name="elements"/> relative to <paramref name="index"/>, or the first or last element of <paramref name="elements"/> if <paramref name="index"/> is equal, greater or lower than <see cref="ICollection{T}.Count"/>.
        /// </summary>
        /// <typeparam name="TSource">The type of elements in the <see cref="IList{T}"/>.</typeparam>
        /// <param name="index">The index of which to advance to the previous element from.</param>
        /// <param name="elements">The elements, relative to <paramref name="index"/>, to return the previous element of.</param>
        /// <exception cref="ArgumentOutOfRangeException">
        /// <paramref name="index"/> is less than 0.
        /// </exception>
        /// <returns>default(TSource) if <paramref name="index"/> is equal, greater or lower than <see cref="ICollection{T}.Count"/>; otherwise the previous element of <paramref name="elements"/> relative to <paramref name="index"/>.</returns>
        public static TSource Previous<TSource>(int index, IList<TSource> elements)
        {
            Validator.ThrowIfNull(elements, nameof(elements));
            Validator.ThrowIfLowerThan(index, 0, nameof(index));
            int previousIndex = index - 1;
            if (previousIndex < 0) { return default(TSource); }
            if (previousIndex >= elements.Count) { return default(TSource); }
            return elements[previousIndex];
        }
	}
}