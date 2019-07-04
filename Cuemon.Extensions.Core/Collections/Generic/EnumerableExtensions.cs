using System;
using System.Collections.Generic;
using System.Linq;
using Cuemon.Collections.Generic;

namespace Cuemon.Extensions.Collections.Generic
{
    /// <summary>
    /// Extension methods for the <see cref="IEnumerable{T}"/> interface.
    /// </summary>
    public static class EnumerableExtensions
    {
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
        public static PartitionerEnumerable<TSource> Chunk<TSource>(this IEnumerable<TSource> source, int size = 128)
        {
            Validator.ThrowIfNull(source, nameof(source));
            Validator.ThrowIfLowerThanOrEqual(0, size, nameof(size));
            return new PartitionerEnumerable<TSource>(source, size);
        }

        /// <summary>
        /// Shuffles the specified <paramref name="source"/> like a deck of cards.
        /// </summary>
        /// <param name="source">The elements to be shuffled in the randomization process.</param>
        /// <returns>A sequence of <typeparamref name="TSource"/> with the shuffled <paramref name="source"/>.</returns>
        /// <remarks>Fisher–Yates shuffle: https://en.wikipedia.org/wiki/Fisher–Yates_shuffle</remarks>
        public static IEnumerable<TSource> Shuffle<TSource>(this IEnumerable<TSource> source)
        {
            return source.Shuffle(Generate.RandomNumber);
        }

        /// <summary>
        /// Shuffles the specified <paramref name="source"/> like a deck of cards.
        /// </summary>
        /// <param name="source">The elements to be shuffled in the randomization process.</param>
        /// <param name="randomizer">The function delegate that will handle the randomization of <paramref name="source"/>.</param>
        /// <returns>A sequence of <typeparamref name="TSource"/> with the shuffled <paramref name="source"/>.</returns>
        /// <remarks>Fisher–Yates shuffle: https://en.wikipedia.org/wiki/Fisher–Yates_shuffle</remarks>
        public static IEnumerable<TSource> Shuffle<TSource>(this IEnumerable<TSource> source, Func<int, int, int> randomizer)
        {
            Validator.ThrowIfNull(source, nameof(source));
            Validator.ThrowIfNull(randomizer, nameof(randomizer));
            var buffer = source.ToArray();
            var length = buffer.Length;
            while (length > 0)
            {
                length--;
                var random = randomizer(0, length + 1);
                var shuffled = buffer[random];
                yield return shuffled;
                buffer[random] = buffer[length];
            }
        }

        /// <summary>
        /// Returns ascending sorted elements from a sequence by using the default comparer to compare values.
        /// </summary>
        /// <typeparam name="TSource">The type of the elements of <paramref name="source"/>.</typeparam>
        /// <param name="source">A sequence of values to order.</param>
        /// <returns>An <see cref="IEnumerable{T}"/> that contains ascending sorted elements from the source sequence.</returns>
        public static IEnumerable<TSource> OrderBy<TSource>(this IEnumerable<TSource> source)
        {
            return source.OrderBy(Comparer<TSource>.Default);
        }

        /// <summary>
        /// Returns ascending sorted elements from a sequence by using a specified <see cref="IComparer{T}"/> to compare values.
        /// </summary>
        /// <typeparam name="TSource">The type of the elements of <paramref name="source"/>.</typeparam>
        /// <param name="source">A sequence of values to order.</param>
        /// <param name="comparer">An <see cref="IComparer{T}"/> to compare values.</param>
        /// <returns>An <see cref="IEnumerable{T}"/> that contains ascending sorted elements from the source sequence.</returns>
        public static IEnumerable<TSource> OrderBy<TSource>(this IEnumerable<TSource> source, IComparer<TSource> comparer)
        {
            Validator.ThrowIfNull(source, nameof(source));
            Validator.ThrowIfNull(comparer, nameof(comparer));
            return source.OrderBy(t => t, comparer);
        }

        /// <summary>
        /// Returns descending sorted elements from a sequence by using the default comparer to compare values.
        /// </summary>
        /// <typeparam name="TSource">The type of the elements of <paramref name="source"/>.</typeparam>
        /// <param name="source">A sequence of values to order.</param>
        /// <returns>An <see cref="IEnumerable{T}"/> that contains descending sorted elements from the source sequence.</returns>
        public static IEnumerable<TSource> OrderByDescending<TSource>(this IEnumerable<TSource> source)
        {
            return source.OrderByDescending(Comparer<TSource>.Default);
        }

        /// <summary>
        /// Returns descending sorted elements from a sequence by using a specified <see cref="IComparer{T}"/> to compare values.
        /// </summary>
        /// <typeparam name="TSource">The type of the elements of <paramref name="source"/>.</typeparam>
        /// <param name="source">A sequence of values to order.</param>
        /// <param name="comparer">An <see cref="IComparer{T}"/> to compare values.</param>
        /// <returns>An <see cref="IEnumerable{T}"/> that contains descending sorted elements from the source sequence.</returns>
        public static IEnumerable<TSource> OrderByDescending<TSource>(this IEnumerable<TSource> source, IComparer<TSource> comparer)
        {
            Validator.ThrowIfNull(source, nameof(source));
            Validator.ThrowIfNull(comparer, nameof(comparer));
            return source.OrderByDescending(t => t, comparer);
        }

        /// <summary>
        /// Returns a random element of a sequence of elements, or a default value if no element is found.
        /// </summary>
        /// <typeparam name="TSource">The type of the elements of <paramref name="source"/>.</typeparam>
        /// <param name="source">The <see cref="IEnumerable{T}"/> to return a random element of.</param>
        /// <returns><c>default</c> if <paramref name="source"/> is empty; otherwise, a random element of <paramref name="source"/>.</returns>
        public static TSource RandomOrDefault<TSource>(this IEnumerable<TSource> source)
        {
            Validator.ThrowIfNull(source, nameof(source));
            var collection = source as ICollection<TSource> ?? new List<TSource>(source);
            return collection.Count == 0 ? default : collection.ElementAt(Generate.RandomNumber(collection.Count));
        }

        /// <summary>
        /// Returns an <see cref="IEnumerable{T}"/> sequence with the specified <paramref name="value"/> as the only element.
        /// </summary>
        /// <typeparam name="T">The type of the element of <see cref="IEnumerable{T}"/>.</typeparam>
        /// <param name="value">The <typeparamref name="T"/> to extend.</param>
        /// <returns>An <see cref="IEnumerable{T}"/> sequence with the specified <paramref name="value"/> as the only element.</returns>
        public static IEnumerable<T> Yield<T>(this T value)
        {
            return Arguments.Yield(value);
        }

        /// <summary>
        /// Creates a <see cref="Dictionary{TKey,TValue}"/> from the specified <paramref name="source"/> sequence.
        /// </summary>
        /// <typeparam name="TKey">The type of keys in the <paramref name="source"/>.</typeparam>
        /// <typeparam name="TValue">The type of values in the <paramref name="source"/>.</typeparam>
        /// <param name="source">The sequence to create a <see cref="Dictionary{TKey,TValue}"/> from.</param>
        /// <returns>A <see cref="Dictionary{TKey,TValue}"/> that is equivalent to the specified <paramref name="source"/> sequence.</returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="source"/> is null.
        /// </exception>
        /// <exception cref="ArgumentException">
        /// <paramref name="source"/> contains at least one <see cref="KeyValuePair{TKey,TValue}"/> that produces duplicate keys for two elements.
        /// </exception>
        public static IDictionary<TKey, TValue> ToDictionary<TKey, TValue>(this IEnumerable<KeyValuePair<TKey, TValue>> source)
        {
            return ToDictionary(source, EqualityComparer<TKey>.Default);
        }

        /// <summary>
        /// Creates a <see cref="Dictionary{TKey,TValue}"/> from the specified <paramref name="source"/> sequence.
        /// </summary>
        /// <typeparam name="TKey">The type of keys in the <paramref name="source"/>.</typeparam>
        /// <typeparam name="TValue">The type of values in the <paramref name="source"/>.</typeparam>
        /// <param name="source">The sequence to create a <see cref="Dictionary{TKey,TValue}"/> from.</param>
        /// <param name="comparer">The <see cref="IEqualityComparer{T}"/> implementation to use when comparing keys.</param>
        /// <returns>A <see cref="Dictionary{TKey,TValue}"/> that is equivalent to the specified <paramref name="source"/> sequence.</returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="source"/> is null - or - <paramref name="comparer"/> is null.
        /// </exception>
        /// <exception cref="ArgumentException">
        /// <paramref name="source"/> contains at least one <see cref="KeyValuePair{TKey,TValue}"/> that produces duplicate keys for two elements.
        /// </exception>
        public static IDictionary<TKey, TValue> ToDictionary<TKey, TValue>(this IEnumerable<KeyValuePair<TKey, TValue>> source, IEqualityComparer<TKey> comparer)
        {
            Validator.ThrowIfNull(source, nameof(source));
            Validator.ThrowIfNull(comparer, nameof(comparer));

            var result = new Dictionary<TKey, TValue>(comparer);
            foreach (var item in source)
            {
                result.Add(item.Key, item.Value);
            }
            return result;
        }

        /// <summary>
        /// Extends the specified <paramref name="source"/> to support iterating in partitions.
        /// </summary>
        /// <typeparam name="TSource">The type of elements in the <paramref name="source"/>.</typeparam>
        /// <param name="source">The sequence to extend.</param>
        /// <param name="partitionSize">The size of the partitions.</param>
        /// <returns>An instance of <see cref="PartitionerEnumerable{T}"/>.</returns>
        public static PartitionerEnumerable<TSource> ToPartitioner<TSource>(this IEnumerable<TSource> source, int partitionSize = 128)
        {
            return new PartitionerEnumerable<TSource>(source, partitionSize);
        }

        /// <summary>
        /// Converts the specified <paramref name="source"/> to a paged data sequence.
        /// </summary>
        /// <typeparam name="TSource">The type of the elements of <paramref name="source"/>.</typeparam>
        /// <param name="source">The source of the sequence to make pageable.</param>
        /// <returns>An instance of <see cref="PagedCollection{T}"/>.</returns>
        /// <remarks>The starting page is set to 1 and the page size is determined by <see cref="PagedSettings.DefaultPageSize"/>.</remarks>
        public static PagedCollection<TSource> ToPagedCollection<TSource>(this IEnumerable<TSource> source)
        {
            return ToPagedCollection(source, 1);
        }

        /// <summary>
        /// Converts the specified <paramref name="source"/> to a paged data sequence initialized with starting <paramref name="pageNumber"/>.
        /// </summary>
        /// <typeparam name="TSource">The type of the elements of <paramref name="source"/>.</typeparam>
        /// <param name="source">The source of the sequence to make pageable.</param>
        /// <param name="pageNumber">The page number to start with.</param>
        /// <returns>An instance of <see cref="PagedCollection{T}"/>.</returns>
        /// <remarks>The page size is determined by <see cref="PagedSettings.DefaultPageSize"/>.</remarks>
        public static PagedCollection<TSource> ToPagedCollection<TSource>(this IEnumerable<TSource> source, int pageNumber)
        {
            return ToPagedCollection(source, pageNumber, PagedSettings.DefaultPageSize);
        }

        /// <summary>
        /// Converts the specified <paramref name="source"/> to a paged data sequence initialized with starting <paramref name="pageNumber"/> and <paramref name="pageSize"/>.
        /// </summary>
        /// <typeparam name="TSource">The type of the elements of <paramref name="source"/>.</typeparam>
        /// <param name="source">The source of the sequence to make pageable.</param>
        /// <param name="pageNumber">The page number to start with.</param>
        /// <param name="pageSize">The number of elements a page can contain.</param>
        /// <returns>An instance of <see cref="PagedCollection{T}"/>.</returns>
        public static PagedCollection<TSource> ToPagedCollection<TSource>(this IEnumerable<TSource> source, int pageNumber, int pageSize)
        {
            return ToPagedCollection(source, new PagedSettings() { PageNumber = pageNumber, PageSize = pageSize });
        }

        /// <summary>
        /// Converts the specified <paramref name="source"/> to a paged data sequence initialized with <paramref name="settings"/>.
        /// </summary>
        /// <typeparam name="TSource">The type of the elements of <paramref name="source"/>.</typeparam>
        /// <param name="source">The source of the sequence to make pageable.</param>
        /// <param name="settings">The settings that specifies the conditions of the converted <paramref name="source"/>.</param>
        /// <returns>An instance of <see cref="PagedCollection{T}"/> initialized with <paramref name="settings"/>.</returns>
        public static PagedCollection<TSource> ToPagedCollection<TSource>(this IEnumerable<TSource> source, PagedSettings settings)
        {
            return new PagedCollection<TSource>(source, settings);
        }

        /// <summary>
        /// Converts the specified <paramref name="source"/> to a paged data sequence initialized with starting <paramref name="pageNumber"/> and <paramref name="pageSize"/>.
        /// </summary>
        /// <typeparam name="TSource">The type of the elements of <paramref name="source"/>.</typeparam>
        /// <param name="source">The source of the sequence to make pageable.</param>
        /// <param name="pageNumber">The page number to start with.</param>
        /// <param name="pageSize">The number of elements a page can contain.</param>
        /// <param name="totalElementCount">The total number of elements in the <paramref name="source"/> sequence.</param>
        /// <returns>An instance of <see cref="PagedCollection{T}"/>.</returns>
        public static PagedCollection<TSource> ToPagedCollection<TSource>(this IEnumerable<TSource> source, int pageNumber, int pageSize, int totalElementCount)
        {
            return ToPagedCollection(source, new PagedSettings() { PageNumber = pageNumber, PageSize = pageSize }, totalElementCount);
        }

        /// <summary>
        /// Converts the specified <paramref name="source"/> to a paged data sequence initialized with <paramref name="settings"/>.
        /// </summary>
        /// <typeparam name="TSource">The type of the elements of <paramref name="source"/>.</typeparam>
        /// <param name="source">The source of the sequence to make pageable.</param>
        /// <param name="settings">The settings that specifies the conditions of the converted <paramref name="source"/>.</param>
        /// <param name="totalElementCount">The total number of elements in the <paramref name="source"/> sequence.</param>
        /// <returns>An instance of <see cref="PagedCollection{T}"/> initialized with <paramref name="settings"/>.</returns>
        public static PagedCollection<TSource> ToPagedCollection<TSource>(this IEnumerable<TSource> source, PagedSettings settings, int totalElementCount)
        {
            return new PagedCollection<TSource>(source, settings, totalElementCount);
        }
    }
}