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
        public static TSource RandomOrDefault<TSource>(IEnumerable<TSource> source, Doer<IEnumerable<TSource>, TSource> randomizer)
        {
            Validator.ThrowIfNull(source, nameof(source));
            Validator.ThrowIfNull(randomizer, nameof(randomizer));
            return randomizer(source);
        }

        private static TSource DefaultRandomizer<TSource>(IEnumerable<TSource> source)
        {
            if (source == null) { return default(TSource); }
            ICollection<TSource> collection = source as ICollection<TSource> ?? new List<TSource>(source);
            return collection.Count == 0 ? default(TSource) : collection.ElementAt(NumberUtility.GetRandomNumber(collection.Count));
        }

        /// <summary>
        /// Generates a sequence of <typeparamref name="T"/> within a specified range.
        /// </summary>
        /// <typeparam name="T">The type of the elements to return.</typeparam>
        /// <param name="count">The number of objects of <typeparamref name="T"/> to generate.</param>
        /// <param name="resolver">The function delegate that will resolve the value of <typeparamref name="T"/>; the parameter passed to the delegate represents the index of the element to return.</param>
        /// <returns>An <see cref="IEnumerable{T}"/> that contains a range of <typeparamref name="T"/> elements.</returns>
        /// <exception cref="System.ArgumentOutOfRangeException">
        /// <paramref name="count"/> is less than 0.
        /// </exception>
	    public static IEnumerable<T> RangeOf<T>(int count, Doer<int, T> resolver)
        {
            if (count < 0) { throw new ArgumentOutOfRangeException(nameof(count)); }
            for (int i = 0; i < count; i++) { yield return resolver(i); }

        }

        /// <summary>
        /// Returns a chunked <see cref="IEnumerable{T}"/> sequence with a maximum of 128 elements.
        /// </summary>
        /// <typeparam name="TSource">The type of the elements of <paramref name="source" />.</typeparam>
        /// <param name="source">An <see cref="IEnumerable{T}" /> to chunk into smaller slices for a batch run or similar.</param>
        /// <returns>An <see cref="IEnumerable{T}" /> that contains no more than 128 elements from the <paramref name="source" /> sequence.</returns>
        /// <exception cref="System.ArgumentNullException">
        /// <paramref name="source"/> is null.
        /// </exception>
        /// <remarks>The original <paramref name="source"/> is reduced equivalent to the number of elements in the returned sequence.</remarks>
        public static IEnumerable<TSource> Chunk<TSource>(ref IEnumerable<TSource> source)
        {
            return Chunk(ref source, 128);
        }

        /// <summary>
        /// Returns a chunked <see cref="IEnumerable{T}"/> sequence with a maximum of the specified <paramref name="size"/>. Default is 128.
        /// </summary>
        /// <typeparam name="TSource">The type of the elements of <paramref name="source" />.</typeparam>
        /// <param name="source">An <see cref="IEnumerable{T}" /> to chunk into smaller slices for a batch run or similar.</param>
        /// <param name="size">The amount of elements to process at a time.</param>
        /// <returns>An <see cref="IEnumerable{T}" /> that contains no more than the specified <paramref name="size" /> of elements from the <paramref name="source" /> sequence.</returns>
        /// <exception cref="System.ArgumentNullException">
        /// <paramref name="source"/> is null.
        /// </exception>
        /// <exception cref="System.ArgumentException">
        /// <paramref name="size"/> is less or equal to 0.
        /// </exception>
        /// <remarks>The original <paramref name="source"/> is reduced equivalent to the number of elements in the returned sequence.</remarks>
        public static IEnumerable<TSource> Chunk<TSource>(ref IEnumerable<TSource> source, int size)
        {
            int processedCount;
            return Chunk(ref source, size, out processedCount);
        }

        internal static IEnumerable<TSource> Chunk<TSource>(ref IEnumerable<TSource> source, int size, out int processedCount)
        {
            if (source == null) { throw new ArgumentNullException(nameof(source)); }
            if (size <= 0) { throw new ArgumentException("Value must be greater than 0.", nameof(size)); }
            List<TSource> pending = new List<TSource>(source);
            List<TSource> processed = new List<TSource>();
            size = size - 1;
            for (int i = 0; i < pending.Count; i++)
            {
                processed.Add(pending[i]);
                if (i >= size) { break; }
            }
            processedCount = processed.Count;
            pending.RemoveRange(0, processedCount);
            source = pending;
            return processed;
        }

        /// <summary>
        /// Retrieves the element that matches the conditions defined by the specified <paramref name="selector"/>.
        /// </summary>
        /// <typeparam name="TSource">The type of the elements of the sequence.</typeparam>
        /// <param name="source">The sequence to search.</param>
        /// <param name="selector">The function delegate that defines the condition of the element to retrieve.</param>
        /// <returns>A <typeparamref name="TSource"/> element that matched the conditions defined by the specified <paramref name="selector"/>.</returns>
        public static TSource SelectOne<TSource>(IEnumerable<TSource> source, Doer<IEnumerable<TSource>, TSource> selector)
        {
            Validator.ThrowIfNull(source, nameof(source));
            Validator.ThrowIfNull(selector, nameof(selector));
            return selector(source);
        }

        /// <summary>
        /// Retrieves the element that matches the conditions defined by the specified <paramref name="selector"/>.
        /// </summary>
        /// <typeparam name="TSource">The type of the elements of the sequence.</typeparam>
        /// <typeparam name="T">The type of the parameter of the function delegate <paramref name="selector"/>.</typeparam>
        /// <param name="source">The sequence to search.</param>
        /// <param name="selector">The function delegate that defines the condition of the element to retrieve.</param>
        /// <param name="arg">The parameter of the function delegate <paramref name="selector"/>.</param>
        /// <returns>A <typeparamref name="TSource"/> element that matched the conditions defined by the specified <paramref name="selector"/>.</returns>
        public static TSource SelectOne<TSource, T>(IEnumerable<TSource> source, Doer<IEnumerable<TSource>, T, TSource> selector, T arg)
        {
            Validator.ThrowIfNull(source, nameof(source));
            Validator.ThrowIfNull(selector, nameof(selector));
            return selector(source, arg);
        }

        /// <summary>
        /// Retrieves the element that matches the conditions defined by the specified <paramref name="selector"/>.
        /// </summary>
        /// <typeparam name="TSource">The type of the elements of the sequence.</typeparam>
        /// <typeparam name="T1">The type of the first parameter of the function delegate <paramref name="selector"/>.</typeparam>
        /// <typeparam name="T2">The type of the second parameter of the function delegate <paramref name="selector"/>.</typeparam>
        /// <param name="source">The sequence to search.</param>
        /// <param name="selector">The function delegate that defines the condition of the element to retrieve.</param>
        /// <param name="arg1">The first parameter of the function delegate <paramref name="selector"/>.</param>
        /// <param name="arg2">The second parameter of the function delegate <paramref name="selector"/>.</param>
        /// <returns>A <typeparamref name="TSource"/> element that matched the conditions defined by the specified <paramref name="selector"/>.</returns>
        public static TSource SelectOne<TSource, T1, T2>(IEnumerable<TSource> source, Doer<IEnumerable<TSource>, T1, T2, TSource> selector, T1 arg1, T2 arg2)
        {
            Validator.ThrowIfNull(source, nameof(source));
            Validator.ThrowIfNull(selector, nameof(selector));
            return selector(source, arg1, arg2);
        }

        /// <summary>
        /// Retrieves the element that matches the conditions defined by the specified <paramref name="selector"/>.
        /// </summary>
        /// <typeparam name="TSource">The type of the elements of the sequence.</typeparam>
        /// <typeparam name="T1">The type of the first parameter of the function delegate <paramref name="selector"/>.</typeparam>
        /// <typeparam name="T2">The type of the second parameter of the function delegate <paramref name="selector"/>.</typeparam>
        /// <typeparam name="T3">The type of the third parameter of the function delegate <paramref name="selector"/>.</typeparam>
        /// <param name="source">The sequence to search.</param>
        /// <param name="selector">The function delegate that defines the condition of the element to retrieve.</param>
        /// <param name="arg1">The first parameter of the function delegate <paramref name="selector"/>.</param>
        /// <param name="arg2">The second parameter of the function delegate <paramref name="selector"/>.</param>
        /// <param name="arg3">The third parameter of the function delegate <paramref name="selector"/>.</param>
        /// <returns>A <typeparamref name="TSource"/> element that matched the conditions defined by the specified <paramref name="selector"/>.</returns>
        public static TSource SelectOne<TSource, T1, T2, T3>(IEnumerable<TSource> source, Doer<IEnumerable<TSource>, T1, T2, T3, TSource> selector, T1 arg1, T2 arg2, T3 arg3)
        {
            Validator.ThrowIfNull(source, nameof(source));
            Validator.ThrowIfNull(selector, nameof(selector));
            return selector(source, arg1, arg2, arg3);
        }

        /// <summary>
        /// Retrieves the element that matches the conditions defined by the specified <paramref name="selector"/>.
        /// </summary>
        /// <typeparam name="TSource">The type of the elements of the sequence.</typeparam>
        /// <typeparam name="T1">The type of the first parameter of the function delegate <paramref name="selector"/>.</typeparam>
        /// <typeparam name="T2">The type of the second parameter of the function delegate <paramref name="selector"/>.</typeparam>
        /// <typeparam name="T3">The type of the third parameter of the function delegate <paramref name="selector"/>.</typeparam>
        /// <typeparam name="T4">The type of the fourth parameter of the function delegate <paramref name="selector"/>.</typeparam>
        /// <param name="source">The sequence to search.</param>
        /// <param name="selector">The function delegate that defines the condition of the element to retrieve.</param>
        /// <param name="arg1">The first parameter of the function delegate <paramref name="selector"/>.</param>
        /// <param name="arg2">The second parameter of the function delegate <paramref name="selector"/>.</param>
        /// <param name="arg3">The third parameter of the function delegate <paramref name="selector"/>.</param>
        /// <param name="arg4">The fourth parameter of the function delegate <paramref name="selector"/>.</param>
        /// <returns>A <typeparamref name="TSource"/> element that matched the conditions defined by the specified <paramref name="selector"/>.</returns>
        public static TSource SelectOne<TSource, T1, T2, T3, T4>(IEnumerable<TSource> source, Doer<IEnumerable<TSource>, T1, T2, T3, T4, TSource> selector, T1 arg1, T2 arg2, T3 arg3, T4 arg4)
        {
            Validator.ThrowIfNull(source, nameof(source));
            Validator.ThrowIfNull(selector, nameof(selector));
            return selector(source, arg1, arg2, arg3, arg4);
        }

        /// <summary>
        /// Retrieves the element that matches the conditions defined by the specified <paramref name="selector"/>.
        /// </summary>
        /// <typeparam name="TSource">The type of the elements of the sequence.</typeparam>
        /// <typeparam name="T1">The type of the first parameter of the function delegate <paramref name="selector"/>.</typeparam>
        /// <typeparam name="T2">The type of the second parameter of the function delegate <paramref name="selector"/>.</typeparam>
        /// <typeparam name="T3">The type of the third parameter of the function delegate <paramref name="selector"/>.</typeparam>
        /// <typeparam name="T4">The type of the fourth parameter of the function delegate <paramref name="selector"/>.</typeparam>
        /// <typeparam name="T5">The type of the fifth parameter of the function delegate <paramref name="selector"/>.</typeparam>
        /// <param name="source">The sequence to search.</param>
        /// <param name="selector">The function delegate that defines the condition of the element to retrieve.</param>
        /// <param name="arg1">The first parameter of the function delegate <paramref name="selector"/>.</param>
        /// <param name="arg2">The second parameter of the function delegate <paramref name="selector"/>.</param>
        /// <param name="arg3">The third parameter of the function delegate <paramref name="selector"/>.</param>
        /// <param name="arg4">The fourth parameter of the function delegate <paramref name="selector"/>.</param>
        /// <param name="arg5">The fifth parameter of the function delegate <paramref name="selector"/>.</param>
        /// <returns>A <typeparamref name="TSource"/> element that matched the conditions defined by the specified <paramref name="selector"/>.</returns>
        public static TSource SelectOne<TSource, T1, T2, T3, T4, T5>(IEnumerable<TSource> source, Doer<IEnumerable<TSource>, T1, T2, T3, T4, T5, TSource> selector, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5)
        {
            Validator.ThrowIfNull(source, nameof(source));
            Validator.ThrowIfNull(selector, nameof(selector));
            return selector(source, arg1, arg2, arg3, arg4, arg5);
        }

        /// <summary>
        /// Retrieves all the elements that match the conditions defined by the specified predicate.
        /// </summary>
        /// <typeparam name="TSource">The type of the elements of the sequence.</typeparam>
        /// <param name="source">The sequence to search.</param>
        /// <param name="match">The function delegate that defines the conditions of the elements to search for.</param>
        /// <returns>An <see cref="IEnumerable{T}"/> sequence containing all the elements that match the conditions defined by the specified predicate, if found; otherwise, an empty <see cref="IEnumerable{T}"/>.</returns>
        public static IEnumerable<TSource> FindAll<TSource>(IEnumerable<TSource> source, Doer<TSource, bool> match)
        {
            if (source == null) { throw new ArgumentNullException(nameof(source)); }
            if (match == null) { throw new ArgumentNullException(nameof(match)); }
            List<TSource> temp = new List<TSource>();
            using (IEnumerator<TSource> enumerator = source.GetEnumerator())
            {
                while (enumerator.MoveNext())
                {
                    TSource current = enumerator.Current;
                    if (match(current)) { temp.Add(current); }
                }
            }
            return temp;

        }

        /// <summary>
        /// Retrieves all the elements that match the conditions defined by the specified function delegate.
        /// </summary>
        /// <typeparam name="TSource">The type of the elements of the sequence.</typeparam>
        /// <typeparam name="T">The type of the first parameter of the function delegate <paramref name="match"/>.</typeparam>
        /// <param name="source">The sequence to search.</param>
        /// <param name="match">The function delegate that defines the conditions of the elements to search for.</param>
        /// <param name="arg">The first parameter of the function delegate <paramref name="match"/>.</param>
        /// <returns>An <see cref="IEnumerable{T}"/> sequence containing all the elements that match the conditions defined by the specified predicate, if found; otherwise, an empty <see cref="IEnumerable{T}"/>.</returns>
        public static IEnumerable<TSource> FindAll<TSource, T>(IEnumerable<TSource> source, Doer<TSource, T, bool> match, T arg)
        {
            var factory = DoerFactory.Create(match, default(TSource), arg);
            return FindAllCore(factory, source);
        }

        /// <summary>
        /// Retrieves all the elements that match the conditions defined by the specified function delegate.
        /// </summary>
        /// <typeparam name="TSource">The type of the elements of the sequence.</typeparam>
        /// <typeparam name="T1">The type of the first parameter of the function delegate <paramref name="match"/>.</typeparam>
        /// <typeparam name="T2">The type of the second parameter of the function delegate <paramref name="match"/>.</typeparam>
        /// <param name="source">The sequence to search.</param>
        /// <param name="match">The function delegate that defines the conditions of the elements to search for.</param>
        /// <param name="arg1">The first parameter of the function delegate <paramref name="match"/>.</param>
        /// <param name="arg2">The second parameter of the function delegate <paramref name="match"/>.</param>
        /// <returns>An <see cref="IEnumerable{T}"/> sequence containing all the elements that match the conditions defined by the specified predicate, if found; otherwise, an empty <see cref="IEnumerable{T}"/>.</returns>
        public static IEnumerable<TSource> FindAll<TSource, T1, T2>(IEnumerable<TSource> source, Doer<TSource, T1, T2, bool> match, T1 arg1, T2 arg2)
        {
            var factory = DoerFactory.Create(match, default(TSource), arg1, arg2);
            return FindAllCore(factory, source);
        }

        /// <summary>
        /// Retrieves all the elements that match the conditions defined by the specified function delegate.
        /// </summary>
        /// <typeparam name="TSource">The type of the elements of the sequence.</typeparam>
        /// <typeparam name="T1">The type of the first parameter of the function delegate <paramref name="match"/>.</typeparam>
        /// <typeparam name="T2">The type of the second parameter of the function delegate <paramref name="match"/>.</typeparam>
        /// <typeparam name="T3">The type of the third parameter of the function delegate <paramref name="match"/>.</typeparam>
        /// <param name="source">The sequence to search.</param>
        /// <param name="match">The function delegate that defines the conditions of the elements to search for.</param>
        /// <param name="arg1">The first parameter of the function delegate <paramref name="match"/>.</param>
        /// <param name="arg2">The second parameter of the function delegate <paramref name="match"/>.</param>
        /// <param name="arg3">The third parameter of the function delegate <paramref name="match"/>.</param>
        /// <returns>An <see cref="IEnumerable{T}"/> sequence containing all the elements that match the conditions defined by the specified predicate, if found; otherwise, an empty <see cref="IEnumerable{T}"/>.</returns>
        public static IEnumerable<TSource> FindAll<TSource, T1, T2, T3>(IEnumerable<TSource> source, Doer<TSource, T1, T2, T3, bool> match, T1 arg1, T2 arg2, T3 arg3)
        {
            var factory = DoerFactory.Create(match, default(TSource), arg1, arg2, arg3);
            return FindAllCore(factory, source);
        }

        /// <summary>
        /// Retrieves all the elements that match the conditions defined by the specified function delegate.
        /// </summary>
        /// <typeparam name="TSource">The type of the elements of the sequence.</typeparam>
        /// <typeparam name="T1">The type of the first parameter of the function delegate <paramref name="match"/>.</typeparam>
        /// <typeparam name="T2">The type of the second parameter of the function delegate <paramref name="match"/>.</typeparam>
        /// <typeparam name="T3">The type of the third parameter of the function delegate <paramref name="match"/>.</typeparam>
        /// <typeparam name="T4">The type of the fourth parameter of the function delegate <paramref name="match"/>.</typeparam>
        /// <param name="source">The sequence to search.</param>
        /// <param name="match">The function delegate that defines the conditions of the elements to search for.</param>
        /// <param name="arg1">The first parameter of the function delegate <paramref name="match"/>.</param>
        /// <param name="arg2">The second parameter of the function delegate <paramref name="match"/>.</param>
        /// <param name="arg3">The third parameter of the function delegate <paramref name="match"/>.</param>
        /// <param name="arg4">The fourth parameter of the function delegate <paramref name="match"/>.</param>
        /// <returns>An <see cref="IEnumerable{T}"/> sequence containing all the elements that match the conditions defined by the specified predicate, if found; otherwise, an empty <see cref="IEnumerable{T}"/>.</returns>
        public static IEnumerable<TSource> FindAll<TSource, T1, T2, T3, T4>(IEnumerable<TSource> source, Doer<TSource, T1, T2, T3, T4, bool> match, T1 arg1, T2 arg2, T3 arg3, T4 arg4)
        {
            var factory = DoerFactory.Create(match, default(TSource), arg1, arg2, arg3, arg4);
            return FindAllCore(factory, source);
        }

        /// <summary>
        /// Retrieves all the elements that match the conditions defined by the specified function delegate.
        /// </summary>
        /// <typeparam name="TSource">The type of the elements of the sequence.</typeparam>
        /// <typeparam name="T1">The type of the first parameter of the function delegate <paramref name="match"/>.</typeparam>
        /// <typeparam name="T2">The type of the second parameter of the function delegate <paramref name="match"/>.</typeparam>
        /// <typeparam name="T3">The type of the third parameter of the function delegate <paramref name="match"/>.</typeparam>
        /// <typeparam name="T4">The type of the fourth parameter of the function delegate <paramref name="match"/>.</typeparam>
        /// <typeparam name="T5">The type of the fifth parameter of the function delegate <paramref name="match"/>.</typeparam>
        /// <param name="source">The sequence to search.</param>
        /// <param name="match">The function delegate that defines the conditions of the elements to search for.</param>
        /// <param name="arg1">The first parameter of the function delegate <paramref name="match"/>.</param>
        /// <param name="arg2">The second parameter of the function delegate <paramref name="match"/>.</param>
        /// <param name="arg3">The third parameter of the function delegate <paramref name="match"/>.</param>
        /// <param name="arg4">The fourth parameter of the function delegate <paramref name="match"/>.</param>
        /// <param name="arg5">The fifth parameter of the function delegate <paramref name="match"/>.</param>
        /// <returns>An <see cref="IEnumerable{T}"/> sequence containing all the elements that match the conditions defined by the specified predicate, if found; otherwise, an empty <see cref="IEnumerable{T}"/>.</returns>
        public static IEnumerable<TSource> FindAll<TSource, T1, T2, T3, T4, T5>(IEnumerable<TSource> source, Doer<TSource, T1, T2, T3, T4, T5, bool> match, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5)
        {
            var factory = DoerFactory.Create(match, default(TSource), arg1, arg2, arg3, arg4, arg5);
            return FindAllCore(factory, source);
        }

        /// <summary>
        /// Retrieves all the elements that match the conditions defined by the specified function delegate.
        /// </summary>
        /// <typeparam name="TSource">The type of the elements of the sequence.</typeparam>
        /// <typeparam name="T1">The type of the first parameter of the function delegate <paramref name="match"/>.</typeparam>
        /// <typeparam name="T2">The type of the second parameter of the function delegate <paramref name="match"/>.</typeparam>
        /// <typeparam name="T3">The type of the third parameter of the function delegate <paramref name="match"/>.</typeparam>
        /// <typeparam name="T4">The type of the fourth parameter of the function delegate <paramref name="match"/>.</typeparam>
        /// <typeparam name="T5">The type of the fifth parameter of the function delegate <paramref name="match"/>.</typeparam>
        /// <typeparam name="T6">The type of the sixth parameter of the function delegate <paramref name="match"/>.</typeparam>
        /// <param name="source">The sequence to search.</param>
        /// <param name="match">The function delegate that defines the conditions of the elements to search for.</param>
        /// <param name="arg1">The first parameter of the function delegate <paramref name="match"/>.</param>
        /// <param name="arg2">The second parameter of the function delegate <paramref name="match"/>.</param>
        /// <param name="arg3">The third parameter of the function delegate <paramref name="match"/>.</param>
        /// <param name="arg4">The fourth parameter of the function delegate <paramref name="match"/>.</param>
        /// <param name="arg5">The fifth parameter of the function delegate <paramref name="match"/>.</param>
        /// <param name="arg6">The sixth parameter of the function delegate <paramref name="match"/>.</param>
        /// <returns>An <see cref="IEnumerable{T}"/> sequence containing all the elements that match the conditions defined by the specified predicate, if found; otherwise, an empty <see cref="IEnumerable{T}"/>.</returns>
        public static IEnumerable<TSource> FindAll<TSource, T1, T2, T3, T4, T5, T6>(IEnumerable<TSource> source, Doer<TSource, T1, T2, T3, T4, T5, T6, bool> match, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6)
        {
            var factory = DoerFactory.Create(match, default(TSource), arg1, arg2, arg3, arg4, arg5, arg6);
            return FindAllCore(factory, source);
        }

        /// <summary>
        /// Retrieves all the elements that match the conditions defined by the specified function delegate.
        /// </summary>
        /// <typeparam name="TSource">The type of the elements of the sequence.</typeparam>
        /// <typeparam name="T1">The type of the first parameter of the function delegate <paramref name="match"/>.</typeparam>
        /// <typeparam name="T2">The type of the second parameter of the function delegate <paramref name="match"/>.</typeparam>
        /// <typeparam name="T3">The type of the third parameter of the function delegate <paramref name="match"/>.</typeparam>
        /// <typeparam name="T4">The type of the fourth parameter of the function delegate <paramref name="match"/>.</typeparam>
        /// <typeparam name="T5">The type of the fifth parameter of the function delegate <paramref name="match"/>.</typeparam>
        /// <typeparam name="T6">The type of the sixth parameter of the function delegate <paramref name="match"/>.</typeparam>
        /// <typeparam name="T7">The type of the seventh parameter of the function delegate <paramref name="match"/>.</typeparam>
        /// <param name="source">The sequence to search.</param>
        /// <param name="match">The function delegate that defines the conditions of the elements to search for.</param>
        /// <param name="arg1">The first parameter of the function delegate <paramref name="match"/>.</param>
        /// <param name="arg2">The second parameter of the function delegate <paramref name="match"/>.</param>
        /// <param name="arg3">The third parameter of the function delegate <paramref name="match"/>.</param>
        /// <param name="arg4">The fourth parameter of the function delegate <paramref name="match"/>.</param>
        /// <param name="arg5">The fifth parameter of the function delegate <paramref name="match"/>.</param>
        /// <param name="arg6">The sixth parameter of the function delegate <paramref name="match"/>.</param>
        /// <param name="arg7">The seventh parameter of the function delegate <paramref name="match"/>.</param>
        /// <returns>An <see cref="IEnumerable{T}"/> sequence containing all the elements that match the conditions defined by the specified predicate, if found; otherwise, an empty <see cref="IEnumerable{T}"/>.</returns>
        public static IEnumerable<TSource> FindAll<TSource, T1, T2, T3, T4, T5, T6, T7>(IEnumerable<TSource> source, Doer<TSource, T1, T2, T3, T4, T5, T6, T7, bool> match, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7)
        {
            var factory = DoerFactory.Create(match, default(TSource), arg1, arg2, arg3, arg4, arg5, arg6, arg7);
            return FindAllCore(factory, source);
        }

        /// <summary>
        /// Retrieves all the elements that match the conditions defined by the specified function delegate.
        /// </summary>
        /// <typeparam name="TSource">The type of the elements of the sequence.</typeparam>
        /// <typeparam name="T1">The type of the first parameter of the function delegate <paramref name="match"/>.</typeparam>
        /// <typeparam name="T2">The type of the second parameter of the function delegate <paramref name="match"/>.</typeparam>
        /// <typeparam name="T3">The type of the third parameter of the function delegate <paramref name="match"/>.</typeparam>
        /// <typeparam name="T4">The type of the fourth parameter of the function delegate <paramref name="match"/>.</typeparam>
        /// <typeparam name="T5">The type of the fifth parameter of the function delegate <paramref name="match"/>.</typeparam>
        /// <typeparam name="T6">The type of the sixth parameter of the function delegate <paramref name="match"/>.</typeparam>
        /// <typeparam name="T7">The type of the seventh parameter of the function delegate <paramref name="match"/>.</typeparam>
        /// <typeparam name="T8">The type of the eighth parameter of the function delegate <paramref name="match"/>.</typeparam>
        /// <param name="source">The sequence to search.</param>
        /// <param name="match">The function delegate that defines the conditions of the elements to search for.</param>
        /// <param name="arg1">The first parameter of the function delegate <paramref name="match"/>.</param>
        /// <param name="arg2">The second parameter of the function delegate <paramref name="match"/>.</param>
        /// <param name="arg3">The third parameter of the function delegate <paramref name="match"/>.</param>
        /// <param name="arg4">The fourth parameter of the function delegate <paramref name="match"/>.</param>
        /// <param name="arg5">The fifth parameter of the function delegate <paramref name="match"/>.</param>
        /// <param name="arg6">The sixth parameter of the function delegate <paramref name="match"/>.</param>
        /// <param name="arg7">The seventh parameter of the function delegate <paramref name="match"/>.</param>
        /// <param name="arg8">The eighth parameter of the function delegate <paramref name="match"/>.</param>
        /// <returns>An <see cref="IEnumerable{T}"/> sequence containing all the elements that match the conditions defined by the specified predicate, if found; otherwise, an empty <see cref="IEnumerable{T}"/>.</returns>
        public static IEnumerable<TSource> FindAll<TSource, T1, T2, T3, T4, T5, T6, T7, T8>(IEnumerable<TSource> source, Doer<TSource, T1, T2, T3, T4, T5, T6, T7, T8, bool> match, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8)
        {
            var factory = DoerFactory.Create(match, default(TSource), arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8);
            return FindAllCore(factory, source);
        }

        /// <summary>
        /// Retrieves all the elements that match the conditions defined by the specified function delegate.
        /// </summary>
        /// <typeparam name="TSource">The type of the elements of the sequence.</typeparam>
        /// <typeparam name="T1">The type of the first parameter of the function delegate <paramref name="match"/>.</typeparam>
        /// <typeparam name="T2">The type of the second parameter of the function delegate <paramref name="match"/>.</typeparam>
        /// <typeparam name="T3">The type of the third parameter of the function delegate <paramref name="match"/>.</typeparam>
        /// <typeparam name="T4">The type of the fourth parameter of the function delegate <paramref name="match"/>.</typeparam>
        /// <typeparam name="T5">The type of the fifth parameter of the function delegate <paramref name="match"/>.</typeparam>
        /// <typeparam name="T6">The type of the sixth parameter of the function delegate <paramref name="match"/>.</typeparam>
        /// <typeparam name="T7">The type of the seventh parameter of the function delegate <paramref name="match"/>.</typeparam>
        /// <typeparam name="T8">The type of the eighth parameter of the function delegate <paramref name="match"/>.</typeparam>
        /// <typeparam name="T9">The type of the ninth parameter of the function delegate <paramref name="match"/>.</typeparam>
        /// <param name="source">The sequence to search.</param>
        /// <param name="match">The function delegate that defines the conditions of the elements to search for.</param>
        /// <param name="arg1">The first parameter of the function delegate <paramref name="match"/>.</param>
        /// <param name="arg2">The second parameter of the function delegate <paramref name="match"/>.</param>
        /// <param name="arg3">The third parameter of the function delegate <paramref name="match"/>.</param>
        /// <param name="arg4">The fourth parameter of the function delegate <paramref name="match"/>.</param>
        /// <param name="arg5">The fifth parameter of the function delegate <paramref name="match"/>.</param>
        /// <param name="arg6">The sixth parameter of the function delegate <paramref name="match"/>.</param>
        /// <param name="arg7">The seventh parameter of the function delegate <paramref name="match"/>.</param>
        /// <param name="arg8">The eighth parameter of the function delegate <paramref name="match"/>.</param>
        /// <param name="arg9">The ninth parameter of the function delegate <paramref name="match"/>.</param>
        /// <returns>An <see cref="IEnumerable{T}"/> sequence containing all the elements that match the conditions defined by the specified predicate, if found; otherwise, an empty <see cref="IEnumerable{T}"/>.</returns>
        public static IEnumerable<TSource> FindAll<TSource, T1, T2, T3, T4, T5, T6, T7, T8, T9>(IEnumerable<TSource> source, Doer<TSource, T1, T2, T3, T4, T5, T6, T7, T8, T9, bool> match, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8, T9 arg9)
        {
            var factory = DoerFactory.Create(match, default(TSource), arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9);
            return FindAllCore(factory, source);
        }

        private static IEnumerable<TSource> FindAllCore<TTuple, TSource>(DoerFactory<TTuple, bool> factory, IEnumerable<TSource> source) where TTuple : Template<TSource>
        {
            List<TSource> temp = new List<TSource>();
            using (IEnumerator<TSource> enumerator = source.GetEnumerator())
            {
                while (enumerator.MoveNext())
                {
                    TSource current = enumerator.Current;
                    factory.GenericArguments.Arg1 = current;
                    if (factory.ExecuteMethod()) { temp.Add(current); }
                }
            }
            return temp;
        }

        /// <summary>
        /// Concatenates the specified sequences in <paramref name="sources"/> into one sequence.
        /// </summary>
        /// <typeparam name="TSource">The type of the elements of the input sequences of <see paramref="sources"/>.</typeparam>
        /// <param name="sources">The sequences to concatenate into one <see cref="IEnumerable{T}"/>.</param>
        /// <returns>An <see cref="IEnumerable{T}"/> that contains the concatenated elements of the specified sequences in <paramref name="sources"/>.</returns>
        public static IEnumerable<TSource> Concat<TSource>(params IEnumerable<TSource>[] sources)
        {
            foreach (IEnumerable<TSource> source in sources)
            {
                foreach (TSource type in source)
                {
                    yield return type;
                }
            }
        }

        /// <summary>
        /// Returns ascending sorted elements from a sequence by using the default comparer to compare values.
        /// </summary>
        /// <typeparam name="TSource">The type of the elements of <paramref name="source"/>.</typeparam>
        /// <param name="source">A sequence of values to order.</param>
        /// <returns>An <see cref="IEnumerable{T}"/> that contains ascending sorted elements from the source sequence.</returns>
        public static IEnumerable<TSource> SortAscending<TSource>(IEnumerable<TSource> source)
        {
            return SortAscending(source, Comparer<TSource>.Default);
        }

        /// <summary>
        /// Returns ascending sorted elements from a sequence by using a specified <see cref="Comparer{T}"/> to compare values.
        /// </summary>
        /// <typeparam name="TSource">The type of the elements of <paramref name="source"/>.</typeparam>
        /// <param name="source">A sequence of values to order.</param>
        /// <param name="comparison">The <see cref="Comparison{T}"/> to use when comparing elements.</param>
        /// <returns>An <see cref="IEnumerable{T}"/> that contains ascending sorted elements from the source sequence.</returns>
        public static IEnumerable<TSource> SortAscending<TSource>(IEnumerable<TSource> source, Comparison<TSource> comparison)
        {
            if (source == null) { throw new ArgumentNullException(nameof(source)); }
            List<TSource> sorter = new List<TSource>(source);
            sorter.Sort(comparison);
            return sorter;
        }

        /// <summary>
        /// Returns ascending sorted elements from a sequence by using a specified <see cref="Comparer{T}"/> to compare values.
        /// </summary>
        /// <typeparam name="TSource">The type of the elements of <paramref name="source"/>.</typeparam>
        /// <param name="source">A sequence of values to order.</param>
        /// <param name="comparer">An <see cref="IComparer{T}"/> to compare values.</param>
        /// <returns>An <see cref="IEnumerable{T}"/> that contains ascending sorted elements from the source sequence.</returns>
        public static IEnumerable<TSource> SortAscending<TSource>(IEnumerable<TSource> source, IComparer<TSource> comparer)
        {
            if (source == null) { throw new ArgumentNullException(nameof(source)); }
            List<TSource> sorter = new List<TSource>(source);
            sorter.Sort(comparer);
            return sorter;
        }

        /// <summary>
        /// Returns ascending sorted elements from a sequence by using the default comparer to compare values.
        /// </summary>
        /// <typeparam name="TSource">The type of the elements of <paramref name="source"/>.</typeparam>
        /// <param name="source">A sequence of values to order.</param>
        /// <returns>An <see cref="IEnumerable{T}"/> that contains descending sorted elements from the source sequence.</returns>
        public static IEnumerable<TSource> SortDescending<TSource>(IEnumerable<TSource> source)
        {
            return SortDescending(source, Comparer<TSource>.Default);
        }

        /// <summary>
        /// Returns descending sorted elements from a sequence by using a specified <see cref="Comparer{T}"/> to compare values.
        /// </summary>
        /// <typeparam name="TSource">The type of the elements of <paramref name="source"/>.</typeparam>
        /// <param name="source">A sequence of values to order.</param>
        /// <param name="comparer">An <see cref="IComparer{T}"/> to compare values.</param>
        /// <returns>An <see cref="IEnumerable{T}"/> that contains descending sorted elements from the source sequence.</returns>
        public static IEnumerable<TSource> SortDescending<TSource>(IEnumerable<TSource> source, IComparer<TSource> comparer)
        {
            return SortAscending(source, comparer).Reverse();
        }

        /// <summary>
        /// Returns descending sorted elements from a sequence by using a specified <see cref="Comparer{T}"/> to compare values.
        /// </summary>
        /// <typeparam name="TSource">The type of the elements of <paramref name="source"/>.</typeparam>
        /// <param name="source">A sequence of values to order.</param>
        /// <param name="comparison">The <see cref="Comparison{T}"/> to use when comparing elements.</param>
        /// <returns>An <see cref="IEnumerable{T}"/> that contains descending sorted elements from the source sequence.</returns>
        public static IEnumerable<TSource> SortDescending<TSource>(IEnumerable<TSource> source, Comparison<TSource> comparison)
        {
            return SortAscending(source, comparison).Reverse();
        }

        /// <summary>
        /// Determines whether a sequence contains a specified element by using a specified <see cref="IEqualityComparer{TSource}"/>.
        /// </summary>
        /// <typeparam name="TSource">The type of the <see cref="IEnumerable{T}"/> of <see paramref="source"/>.</typeparam>
        /// <param name="source">A sequence in which to locate a value.</param>
        /// <param name="value">The value to locate in the sequence.</param>
        /// <param name="comparer">An equality comparer to compare values.</param>
        /// <returns>
        /// 	<c>true</c> if the source sequence contains an element that has the specified value; otherwise, <c>false</c>.
        /// </returns>
        public static bool Contains<TSource>(IEnumerable<TSource> source, TSource value, IEqualityComparer<TSource> comparer)
        {
            Validator.ThrowIfNull(source, nameof(source));
            Validator.ThrowIfNull(comparer, nameof(comparer));
            return Contains(source, value, comparer.Equals);
        }

        /// <summary>
        /// Determines whether a sequence contains a specified element by using a specified <see cref="IEqualityComparer{TSource}"/>.
        /// </summary>
        /// <typeparam name="TSource">The type of the <see cref="IEnumerable{T}"/> of <see paramref="source"/>.</typeparam>
        /// <param name="source">A sequence in which to locate a value.</param>
        /// <param name="value">The value to locate in the sequence.</param>
        /// <param name="condition">The function delegate that will compare values from the <paramref name="source"/> sequence with <paramref name="value"/>.</param>
        /// <returns>
        /// 	<c>true</c> if the source sequence contains an element that has the specified value; otherwise, <c>false</c>.
        /// </returns>
        public static bool Contains<TSource>(IEnumerable<TSource> source, TSource value, Doer<TSource, TSource, bool> condition)
        {
            Validator.ThrowIfNull(source, nameof(source));
            Validator.ThrowIfNull(condition, nameof(condition));
            foreach (TSource item in source)
            {
                if (condition(value, item))
                {
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// Returns an <see cref="IEnumerable{T}"/> sequence with the specified <paramref name="source"/> as the only element.
        /// </summary>
        /// <typeparam name="TSource">The type of <paramref name="source"/>.</typeparam>
        /// <param name="source">The value to yield into an <see cref="IEnumerable{T}"/> sequence.</param>
        /// <returns>An <see cref="IEnumerable{T}"/> sequence with the specified <paramref name="source"/> as the only element.</returns>
        public static IEnumerable<TSource> Yield<TSource>(TSource source)
        {
            yield return source;
        }
    }
}