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
        /// <returns>A sequence of <typeparamref name="T"/> with the shuffled <paramref name="source"/>.</returns>
        /// <remarks>Fisher–Yates shuffle: https://en.wikipedia.org/wiki/Fisher–Yates_shuffle</remarks>
        public static IEnumerable<T> Shuffle<T>(this IEnumerable<T> source)
        {
            return source.Shuffle(NumberUtility.GetRandomNumber);
        }

        /// <summary>
        /// Shuffles the specified <paramref name="source"/> like a deck of cards.
        /// </summary>
        /// <param name="source">The elements to be shuffled in the randomization process.</param>
        /// <param name="randomizer">The function delegate that will handle the randomization of <paramref name="source"/>.</param>
        /// <returns>A sequence of <typeparamref name="T"/> with the shuffled <paramref name="source"/>.</returns>
        /// <remarks>Fisher–Yates shuffle: https://en.wikipedia.org/wiki/Fisher–Yates_shuffle</remarks>
        public static IEnumerable<T> Shuffle<T>(this IEnumerable<T> source, Func<int, int, int> randomizer)
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
    }
}