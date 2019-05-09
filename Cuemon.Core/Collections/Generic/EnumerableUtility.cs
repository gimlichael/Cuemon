using System;
using System.Collections.Generic;
using System.Linq;

namespace Cuemon.Collections.Generic
{
	/// <summary>
	/// This utility class provides a set of static methods for querying objects that implement <see cref="IEnumerable{T}"/>. 
	/// </summary>
	public static class EnumerableUtility
	{
        /// <summary>
        /// Returns a random element of a sequence of elements, or a default value if no element is found.
        /// </summary>
        /// <typeparam name="TSource">The type of the elements of <paramref name="source"/>.</typeparam>
        /// <param name="source">The <see cref="IEnumerable{T}"/> to return a random element of.</param>
        /// <returns>default(TSource) if source is empty; otherwise, a random element of <paramref name="source"/>.</returns>
        public static TSource RandomOrDefault<TSource>(IEnumerable<TSource> source)
	    {
            Validator.ThrowIfNull(source, nameof(source));
	        return RandomOrDefault(source, DefaultRandomizer);
	    }

        /// <summary>
        /// Returns a random element of a sequence of elements, or a default value if no element is found.
        /// </summary>
        /// <typeparam name="TSource">The type of the elements of <paramref name="source"/>.</typeparam>
        /// <param name="source">The <see cref="IEnumerable{T}"/> to return a random element of.</param>
        /// <param name="randomizer">The function delegate that will select a random element of <paramref name="source"/>.</param>
        /// <returns>default(TSource) if source is empty; otherwise, a random element of <paramref name="source"/>.</returns>
        public static TSource RandomOrDefault<TSource>(IEnumerable<TSource> source, Func<IEnumerable<TSource>, TSource> randomizer)
	    {
	        Validator.ThrowIfNull(source, nameof(source));
            Validator.ThrowIfNull(randomizer, nameof(randomizer));
            return randomizer(source);
	    }

        private static TSource DefaultRandomizer<TSource>(IEnumerable<TSource> source)
	    {
            if (source == null) { return default; }
            var collection = source as ICollection<TSource> ?? new List<TSource>(source);
            return collection.Count == 0 ? default : collection.ElementAt(NumberUtility.GetRandomNumber(collection.Count));
	    }

        /// <summary>
        /// Generates a sequence of <typeparamref name="T"/> within a specified range.
        /// </summary>
        /// <typeparam name="T">The type of the elements to return.</typeparam>
        /// <param name="count">The number of objects of <typeparamref name="T"/> to generate.</param>
        /// <param name="resolver">The function delegate that will resolve the value of <typeparamref name="T"/>; the parameter passed to the delegate represents the index of the element to return.</param>
        /// <returns>An <see cref="IEnumerable{T}"/> that contains a range of <typeparamref name="T"/> elements.</returns>
        /// <exception cref="ArgumentOutOfRangeException">
        /// <paramref name="count"/> is less than 0.
        /// </exception>
	    public static IEnumerable<T> RangeOf<T>(int count, Func<int, T> resolver) 
	    {
            if (count < 0) { throw new ArgumentOutOfRangeException(nameof(count)); }
            for (var i = 0; i < count; i++) { yield return resolver(i); }

	    }

        /// <summary>
        /// Returns a chunked <see cref="IEnumerable{T}"/> sequence with a maximum of the specified <paramref name="size"/>. Default is 128.
        /// </summary>
        /// <typeparam name="TSource">The type of the elements of <paramref name="source" />.</typeparam>
        /// <param name="source">An <see cref="IEnumerable{T}" /> to chunk into smaller slices for a batch run or similar.</param>
        /// <param name="size">The amount of elements to process at a time.</param>
        /// <returns>An <see cref="IEnumerable{T}" /> that contains no more than the specified <paramref name="size" /> of elements from the <paramref name="source" /> sequence.</returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="source"/> is null.
        /// </exception>
        /// <exception cref="ArgumentException">
        /// <paramref name="size"/> is less or equal to 0.
        /// </exception>
        /// <remarks>The original <paramref name="source"/> is reduced equivalent to the number of elements in the returned sequence.</remarks>
        public static PartitionerEnumerable<TSource> Chunk<TSource>(IEnumerable<TSource> source, int size = 128)
        {
            Validator.ThrowIfNull(source, nameof(source));
            Validator.ThrowIfLowerThanOrEqual(0, size, nameof(size));
            return new PartitionerEnumerable<TSource>(source, size);
        }

        /// <summary>
        /// Returns an <see cref="IEnumerable{T}"/> sequence with the specified <paramref name="value"/> as the only element.
        /// </summary>
        /// <typeparam name="T">The type of the element of <see cref="IEnumerable{T}"/>.</typeparam>
        /// <param name="value">The value to yield into an <see cref="IEnumerable{T}"/> sequence.</param>
        /// <returns>An <see cref="IEnumerable{T}"/> sequence with the specified <paramref name="value"/> as the only element.</returns>
        public static IEnumerable<T> Yield<T>(T value)
        {
            yield return value;
        }

        /// <summary>
        /// Returns ascending sorted elements from a sequence by using a specified <see cref="Comparison{T}"/> to compare values.
        /// </summary>
        /// <typeparam name="TSource">The type of the elements of <paramref name="source"/>.</typeparam>
        /// <param name="source">A sequence of values to order.</param>
        /// <param name="comparer">The <see cref="Comparison{T}"/> to use when comparing elements.</param>
        /// <returns>An <see cref="IEnumerable{T}"/> that contains asscending sorted elements from the source sequence.</returns>
        public static IEnumerable<TSource> OrderBy<TSource>(IEnumerable<TSource> source, Comparison<TSource> comparer)
        {
            Validator.ThrowIfNull(source, nameof(source));
            Validator.ThrowIfNull(comparer, nameof(comparer));
            var sorter = new List<TSource>(source);
            sorter.Sort(comparer);
            return sorter;
        }

        /// <summary>
        /// Returns descending sorted elements from a sequence by using a specified <see cref="Comparison{T}"/> to compare values.
        /// </summary>
        /// <typeparam name="TSource">The type of the elements of <paramref name="source"/>.</typeparam>
        /// <param name="source">A sequence of values to order.</param>
        /// <param name="comparer">The <see cref="Comparison{T}"/> to use when comparing elements.</param>
        /// <returns>An <see cref="IEnumerable{T}"/> that contains descending sorted elements from the source sequence.</returns>
        public static IEnumerable<TSource> OrderByDescending<TSource>(IEnumerable<TSource> source, Comparison<TSource> comparer)
        {
            return OrderBy(source, comparer).Reverse();
        }
    }
}