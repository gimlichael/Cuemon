using System.Collections.Generic;

namespace Cuemon.Collections.Generic
{
    /// <summary>
    /// This is an extension implementation that provides ways to partition <see cref="IEnumerable{T}"/> sequences.
    /// </summary>
    public static class PartitionCollectionExtension
    {
        /// <summary>
        /// Converts the specified <paramref name="source"/> to a partitioned data sequence. Default partition size is 128.
        /// </summary>
        /// <typeparam name="T">The type of the elements of <paramref name="source"/>.</typeparam>
        /// <param name="source">The source of the sequence.</param>
        /// <returns>An instance of <see cref="PartitionCollection{T}"/>.</returns>
        public static PartitionCollection<T> ToPartitionCollection<T>(this IEnumerable<T> source)
        {
            return new PartitionCollection<T>(source);
        }

        /// <summary>
        /// Converts the specified <paramref name="source"/> to a partitioned data sequence.
        /// </summary>
        /// <typeparam name="T">The type of the elements of <paramref name="source"/>.</typeparam>
        /// <param name="source">The source of the sequence.</param>
        /// <param name="size">The number of elements a partition can hold.</param>
        /// <returns>An instance of <see cref="PartitionCollection{T}"/>.</returns>
        public static PartitionCollection<T> ToPartitionCollection<T>(this IEnumerable<T> source, int size)
        {
            return new PartitionCollection<T>(source, size);
        }
    }
}