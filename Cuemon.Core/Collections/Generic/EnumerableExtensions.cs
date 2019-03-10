using System;
using System.Collections.Generic;
using System.Linq;

namespace Cuemon.Collections.Generic
{
    /// <summary>
    /// Extension methods for the <see cref="IEnumerable{T}"/> interface.
    /// </summary>
    public static class EnumerableExtensions
    {
        /// <summary>
        /// Shuffles the specified <paramref name="source"/> like a deck of cards.
        /// </summary>
        /// <param name="source">The elements to be shuffled in the randomization process.</param>
        /// <returns>A sequence of <typeparamref name="TSource"/> with the shuffled <paramref name="source"/>.</returns>
        /// <remarks>Fisher–Yates shuffle: https://en.wikipedia.org/wiki/Fisher–Yates_shuffle</remarks>
        public static IEnumerable<TSource> Shuffle<TSource>(this IEnumerable<TSource> source)
        {
            return source.Shuffle(NumberUtility.GetRandomNumber);
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
        /// Returns ascending sorted elements from a sequence by using a specified <see cref="Comparison{T}"/> to compare values.
        /// </summary>
        /// <typeparam name="TSource">The type of the elements of <paramref name="source"/>.</typeparam>
        /// <param name="source">A sequence of values to order.</param>
        /// <param name="comparer">The <see cref="Comparison{T}"/> to use when comparing elements.</param>
        /// <returns>An <see cref="IEnumerable{T}"/> that contains asscending sorted elements from the source sequence.</returns>
        public static IEnumerable<TSource> OrderBy<TSource>(this IEnumerable<TSource> source, Comparison<TSource> comparer)
        {
            Validator.ThrowIfNull(source, nameof(source));
            Validator.ThrowIfNull(comparer, nameof(comparer));
            var sorter = new List<TSource>(source);
            sorter.Sort(comparer);
            return sorter;
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
        /// Returns descending sorted elements from a sequence by using a specified <see cref="Comparison{T}"/> to compare values.
        /// </summary>
        /// <typeparam name="TSource">The type of the elements of <paramref name="source"/>.</typeparam>
        /// <param name="source">A sequence of values to order.</param>
        /// <param name="comparer">The <see cref="Comparison{T}"/> to use when comparing elements.</param>
        /// <returns>An <see cref="IEnumerable{T}"/> that contains descending sorted elements from the source sequence.</returns>
        public static IEnumerable<TSource> OrderByDescending<TSource>(this IEnumerable<TSource> source, Comparison<TSource> comparer)
        {
            return source.OrderBy(comparer).Reverse();
        }

        /// <summary>
        /// Returns a random element of a sequence of elements, or a default value if no element is found.
        /// </summary>
        /// <typeparam name="TSource">The type of the elements of <paramref name="source"/>.</typeparam>
        /// <param name="source">The <see cref="IEnumerable{T}"/> to return a random element of.</param>
        /// <returns>default(TSource) if source is empty; otherwise, a random element of <paramref name="source"/>.</returns>
        public static TSource RandomOrDefault<TSource>(this IEnumerable<TSource> source)
        {
            return EnumerableUtility.RandomOrDefault(source);
        }

        /// <summary>
        /// Returns a random element of a sequence of elements, or a default value if no element is found.
        /// </summary>
        /// <typeparam name="TSource">The type of the elements of <paramref name="source"/>.</typeparam>
        /// <param name="source">The <see cref="IEnumerable{T}"/> to return a random element of.</param>
        /// <param name="randomizer">The function delegate that will select a random element of <paramref name="source"/>.</param>
        /// <returns>default(TSource) if source is empty; otherwise, a random element of <paramref name="source"/>.</returns>
        public static TSource RandomOrDefault<TSource>(this IEnumerable<TSource> source, Func<IEnumerable<TSource>, TSource> randomizer)
        {
            return EnumerableUtility.RandomOrDefault(source, randomizer);
        }

        /// <summary>
        /// Returns an <see cref="IEnumerable{T}"/> sequence with the specified <paramref name="source"/> as the only element.
        /// </summary>
        /// <typeparam name="TSource">The type of <paramref name="source"/>.</typeparam>
        /// <param name="source">The value to yield into an <see cref="IEnumerable{T}"/> sequence.</param>
        /// <returns>An <see cref="IEnumerable{T}"/> sequence with the specified <paramref name="source"/> as the only element.</returns>
        public static IEnumerable<TSource> Yield<TSource>(this TSource source)
        {
            yield return source;
        }
    }
}