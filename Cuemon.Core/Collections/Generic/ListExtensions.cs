using System;
using System.Collections.Generic;

namespace Cuemon.Collections.Generic
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
            for (int i = 0; i < list.Count; i++)
            {
                if (predicate(list[i]))
                {
                    list.RemoveAt(i);
                    return true;
                }
            }
            return false;
        }
    }
}