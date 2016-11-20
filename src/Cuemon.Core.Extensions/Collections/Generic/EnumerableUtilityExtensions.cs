using System;
using System.Collections.Generic;

namespace Cuemon.Collections.Generic
{
    /// <summary>
    /// This is an extension implementation of the most common methods on the <see cref="EnumerableUtility"/> class.
    /// </summary>
    public static class EnumerableUtilityExtensions
    {
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
        /// Retrieves the element that matches the conditions defined by the specified <paramref name="selector"/>.
        /// </summary>
        /// <typeparam name="TSource">The type of the elements of the sequence.</typeparam>
        /// <param name="source">The sequence to search.</param>
        /// <param name="selector">The function delegate that defines the condition of the element to retrieve.</param>
        /// <returns>A <see cref="TSource"/> element that matched the conditions defined by the specified <paramref name="selector"/>.</returns>
        public static TSource SelectOne<TSource>(this IEnumerable<TSource> source, Func<IEnumerable<TSource>, TSource> selector)
        {
            return EnumerableUtility.SelectOne(source, selector);
        }

        /// <summary>
        /// Retrieves the element that matches the conditions defined by the specified <paramref name="selector"/>.
        /// </summary>
        /// <typeparam name="TSource">The type of the elements of the sequence.</typeparam>
        /// <typeparam name="T">The type of the parameter of the function delegate <paramref name="selector"/>.</typeparam>
        /// <param name="source">The sequence to search.</param>
        /// <param name="selector">The function delegate that defines the condition of the element to retrieve.</param>
        /// <param name="arg">The parameter of the function delegate <paramref name="selector"/>.</param>
        /// <returns>A <see cref="TSource"/> element that matched the conditions defined by the specified <paramref name="selector"/>.</returns>
        public static TSource SelectOne<TSource, T>(this IEnumerable<TSource> source, Func<IEnumerable<TSource>, T, TSource> selector, T arg)
        {
            return EnumerableUtility.SelectOne(source, selector, arg);
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
        /// <returns>A <see cref="TSource"/> element that matched the conditions defined by the specified <paramref name="selector"/>.</returns>
        public static TSource SelectOne<TSource, T1, T2>(this IEnumerable<TSource> source, Func<IEnumerable<TSource>, T1, T2, TSource> selector, T1 arg1, T2 arg2)
        {
            return EnumerableUtility.SelectOne(source, selector, arg1, arg2);
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
        /// <returns>A <see cref="TSource"/> element that matched the conditions defined by the specified <paramref name="selector"/>.</returns>
        public static TSource SelectOne<TSource, T1, T2, T3>(this IEnumerable<TSource> source, Func<IEnumerable<TSource>, T1, T2, T3, TSource> selector, T1 arg1, T2 arg2, T3 arg3)
        {
            return EnumerableUtility.SelectOne(source, selector, arg1, arg2, arg3);
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
        /// <returns>A <see cref="TSource"/> element that matched the conditions defined by the specified <paramref name="selector"/>.</returns>
        public static TSource SelectOne<TSource, T1, T2, T3, T4>(this IEnumerable<TSource> source, Func<IEnumerable<TSource>, T1, T2, T3, T4, TSource> selector, T1 arg1, T2 arg2, T3 arg3, T4 arg4)
        {
            return EnumerableUtility.SelectOne(source, selector, arg1, arg2, arg3, arg4);
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
        /// <returns>A <see cref="TSource"/> element that matched the conditions defined by the specified <paramref name="selector"/>.</returns>
        public static TSource SelectOne<TSource, T1, T2, T3, T4, T5>(IEnumerable<TSource> source, Func<IEnumerable<TSource>, T1, T2, T3, T4, T5, TSource> selector, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5)
        {
            return EnumerableUtility.SelectOne(source, selector, arg1, arg2, arg3, arg4, arg5);
        }

        /// <summary>
        /// Retrieves all the elements that match the conditions defined by the specified predicate.
        /// </summary>
        /// <typeparam name="TSource">The type of the elements of the sequence.</typeparam>
        /// <param name="source">The sequence to search.</param>
        /// <param name="match">The function delegate that defines the conditions of the elements to search for.</param>
        /// <returns>An <see cref="IEnumerable{T}"/> sequence containing all the elements that match the conditions defined by the specified predicate, if found; otherwise, an empty <see cref="IEnumerable{T}"/>.</returns>
        public static IEnumerable<TSource> FindAll<TSource>(this IEnumerable<TSource> source, Func<TSource, bool> match)
        {
            return EnumerableUtility.FindAll(source, match);
        }

        /// <summary>
        /// Retrieves all the elements that match the conditions defined by the specified function delegate.
        /// </summary>
        /// <typeparam name="TSource">The type of the elements of the sequence.</typeparam>
        /// <typeparam name="T">The type of the first parameter of the delegate <paramref name="match"/>.</typeparam>
        /// <param name="source">The sequence to search.</param>
        /// <param name="match">The function delegate that defines the conditions of the elements to search for.</param>
        /// <param name="arg">The first parameter of the delegate <paramref name="match"/>.</param>
        /// <returns>An <see cref="IEnumerable{T}"/> sequence containing all the elements that match the conditions defined by the specified predicate, if found; otherwise, an empty <see cref="IEnumerable{T}"/>.</returns>
        public static IEnumerable<TSource> FindAll<TSource, T>(this IEnumerable<TSource> source, Func<TSource, T, bool> match, T arg)
        {
            return EnumerableUtility.FindAll(source, match, arg);
        }

        /// <summary>
        /// Retrieves all the elements that match the conditions defined by the specified function delegate.
        /// </summary>
        /// <typeparam name="TSource">The type of the elements of the sequence.</typeparam>
        /// <typeparam name="T1">The type of the first parameter of the delegate <paramref name="match"/>.</typeparam>
        /// <typeparam name="T2">The type of the second parameter of the delegate <paramref name="match"/>.</typeparam>
        /// <param name="source">The sequence to search.</param>
        /// <param name="match">The function delegate that defines the conditions of the elements to search for.</param>
        /// <param name="arg1">The first parameter of the delegate <paramref name="match"/>.</param>
        /// <param name="arg2">The second parameter of the delegate <paramref name="match"/>.</param>
        /// <returns>An <see cref="IEnumerable{T}"/> sequence containing all the elements that match the conditions defined by the specified predicate, if found; otherwise, an empty <see cref="IEnumerable{T}"/>.</returns>
        public static IEnumerable<TSource> FindAll<TSource, T1, T2>(this IEnumerable<TSource> source, Func<TSource, T1, T2, bool> match, T1 arg1, T2 arg2)
        {
            return EnumerableUtility.FindAll(source, match, arg1, arg2);
        }

        /// <summary>
        /// Retrieves all the elements that match the conditions defined by the specified function delegate.
        /// </summary>
        /// <typeparam name="TSource">The type of the elements of the sequence.</typeparam>
        /// <typeparam name="T1">The type of the first parameter of the delegate <paramref name="match"/>.</typeparam>
        /// <typeparam name="T2">The type of the second parameter of the delegate <paramref name="match"/>.</typeparam>
        /// <typeparam name="T3">The type of the third parameter of the delegate <paramref name="match"/>.</typeparam>
        /// <param name="source">The sequence to search.</param>
        /// <param name="match">The function delegate that defines the conditions of the elements to search for.</param>
        /// <param name="arg1">The first parameter of the delegate <paramref name="match"/>.</param>
        /// <param name="arg2">The second parameter of the delegate <paramref name="match"/>.</param>
        /// <param name="arg3">The third parameter of the delegate <paramref name="match"/>.</param>
        /// <returns>An <see cref="IEnumerable{T}"/> sequence containing all the elements that match the conditions defined by the specified predicate, if found; otherwise, an empty <see cref="IEnumerable{T}"/>.</returns>
        public static IEnumerable<TSource> FindAll<TSource, T1, T2, T3>(this IEnumerable<TSource> source, Func<TSource, T1, T2, T3, bool> match, T1 arg1, T2 arg2, T3 arg3)
        {
            return EnumerableUtility.FindAll(source, match, arg1, arg2, arg3);
        }

        /// <summary>
        /// Retrieves all the elements that match the conditions defined by the specified function delegate.
        /// </summary>
        /// <typeparam name="TSource">The type of the elements of the sequence.</typeparam>
        /// <typeparam name="T1">The type of the first parameter of the delegate <paramref name="match"/>.</typeparam>
        /// <typeparam name="T2">The type of the second parameter of the delegate <paramref name="match"/>.</typeparam>
        /// <typeparam name="T3">The type of the third parameter of the delegate <paramref name="match"/>.</typeparam>
        /// <typeparam name="T4">The type of the fourth parameter of the delegate <paramref name="match"/>.</typeparam>
        /// <param name="source">The sequence to search.</param>
        /// <param name="match">The function delegate that defines the conditions of the elements to search for.</param>
        /// <param name="arg1">The first parameter of the delegate <paramref name="match"/>.</param>
        /// <param name="arg2">The second parameter of the delegate <paramref name="match"/>.</param>
        /// <param name="arg3">The third parameter of the delegate <paramref name="match"/>.</param>
        /// <param name="arg4">The fourth parameter of the delegate <paramref name="match"/>.</param>
        /// <returns>An <see cref="IEnumerable{T}"/> sequence containing all the elements that match the conditions defined by the specified predicate, if found; otherwise, an empty <see cref="IEnumerable{T}"/>.</returns>
        public static IEnumerable<TSource> FindAll<TSource, T1, T2, T3, T4>(this IEnumerable<TSource> source, Func<TSource, T1, T2, T3, T4, bool> match, T1 arg1, T2 arg2, T3 arg3, T4 arg4)
        {
            return EnumerableUtility.FindAll(source, match, arg1, arg2, arg3, arg4);
        }

        /// <summary>
        /// Retrieves all the elements that match the conditions defined by the specified function delegate.
        /// </summary>
        /// <typeparam name="TSource">The type of the elements of the sequence.</typeparam>
        /// <typeparam name="T1">The type of the first parameter of the delegate <paramref name="match"/>.</typeparam>
        /// <typeparam name="T2">The type of the second parameter of the delegate <paramref name="match"/>.</typeparam>
        /// <typeparam name="T3">The type of the third parameter of the delegate <paramref name="match"/>.</typeparam>
        /// <typeparam name="T4">The type of the fourth parameter of the delegate <paramref name="match"/>.</typeparam>
        /// <typeparam name="T5">The type of the fifth parameter of the delegate <paramref name="match"/>.</typeparam>
        /// <param name="source">The sequence to search.</param>
        /// <param name="match">The function delegate that defines the conditions of the elements to search for.</param>
        /// <param name="arg1">The first parameter of the delegate <paramref name="match"/>.</param>
        /// <param name="arg2">The second parameter of the delegate <paramref name="match"/>.</param>
        /// <param name="arg3">The third parameter of the delegate <paramref name="match"/>.</param>
        /// <param name="arg4">The fourth parameter of the delegate <paramref name="match"/>.</param>
        /// <param name="arg5">The fifth parameter of the delegate <paramref name="match"/>.</param>
        /// <returns>An <see cref="IEnumerable{T}"/> sequence containing all the elements that match the conditions defined by the specified predicate, if found; otherwise, an empty <see cref="IEnumerable{T}"/>.</returns>
        public static IEnumerable<TSource> FindAll<TSource, T1, T2, T3, T4, T5>(this IEnumerable<TSource> source, Func<TSource, T1, T2, T3, T4, T5, bool> match, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5)
        {
            return EnumerableUtility.FindAll(source, match, arg1, arg2, arg3, arg4, arg5);
        }

        /// <summary>
        /// Retrieves all the elements that match the conditions defined by the specified function delegate.
        /// </summary>
        /// <typeparam name="TSource">The type of the elements of the sequence.</typeparam>
        /// <typeparam name="T1">The type of the first parameter of the delegate <paramref name="match"/>.</typeparam>
        /// <typeparam name="T2">The type of the second parameter of the delegate <paramref name="match"/>.</typeparam>
        /// <typeparam name="T3">The type of the third parameter of the delegate <paramref name="match"/>.</typeparam>
        /// <typeparam name="T4">The type of the fourth parameter of the delegate <paramref name="match"/>.</typeparam>
        /// <typeparam name="T5">The type of the fifth parameter of the delegate <paramref name="match"/>.</typeparam>
        /// <typeparam name="T6">The type of the sixth parameter of the delegate <paramref name="match"/>.</typeparam>
        /// <param name="source">The sequence to search.</param>
        /// <param name="match">The function delegate that defines the conditions of the elements to search for.</param>
        /// <param name="arg1">The first parameter of the delegate <paramref name="match"/>.</param>
        /// <param name="arg2">The second parameter of the delegate <paramref name="match"/>.</param>
        /// <param name="arg3">The third parameter of the delegate <paramref name="match"/>.</param>
        /// <param name="arg4">The fourth parameter of the delegate <paramref name="match"/>.</param>
        /// <param name="arg5">The fifth parameter of the delegate <paramref name="match"/>.</param>
        /// <param name="arg6">The sixth parameter of the delegate <paramref name="match"/>.</param>
        /// <returns>An <see cref="IEnumerable{T}"/> sequence containing all the elements that match the conditions defined by the specified predicate, if found; otherwise, an empty <see cref="IEnumerable{T}"/>.</returns>
        public static IEnumerable<TSource> FindAll<TSource, T1, T2, T3, T4, T5, T6>(this IEnumerable<TSource> source, Func<TSource, T1, T2, T3, T4, T5, T6, bool> match, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6)
        {
            return EnumerableUtility.FindAll(source, match, arg1, arg2, arg3, arg4, arg5, arg6);
        }

        /// <summary>
        /// Retrieves all the elements that match the conditions defined by the specified function delegate.
        /// </summary>
        /// <typeparam name="TSource">The type of the elements of the sequence.</typeparam>
        /// <typeparam name="T1">The type of the first parameter of the delegate <paramref name="match"/>.</typeparam>
        /// <typeparam name="T2">The type of the second parameter of the delegate <paramref name="match"/>.</typeparam>
        /// <typeparam name="T3">The type of the third parameter of the delegate <paramref name="match"/>.</typeparam>
        /// <typeparam name="T4">The type of the fourth parameter of the delegate <paramref name="match"/>.</typeparam>
        /// <typeparam name="T5">The type of the fifth parameter of the delegate <paramref name="match"/>.</typeparam>
        /// <typeparam name="T6">The type of the sixth parameter of the delegate <paramref name="match"/>.</typeparam>
        /// <typeparam name="T7">The type of the seventh parameter of the delegate <paramref name="match"/>.</typeparam>
        /// <param name="source">The sequence to search.</param>
        /// <param name="match">The function delegate that defines the conditions of the elements to search for.</param>
        /// <param name="arg1">The first parameter of the delegate <paramref name="match"/>.</param>
        /// <param name="arg2">The second parameter of the delegate <paramref name="match"/>.</param>
        /// <param name="arg3">The third parameter of the delegate <paramref name="match"/>.</param>
        /// <param name="arg4">The fourth parameter of the delegate <paramref name="match"/>.</param>
        /// <param name="arg5">The fifth parameter of the delegate <paramref name="match"/>.</param>
        /// <param name="arg6">The sixth parameter of the delegate <paramref name="match"/>.</param>
        /// <param name="arg7">The seventh parameter of the delegate <paramref name="match"/>.</param>
        /// <returns>An <see cref="IEnumerable{T}"/> sequence containing all the elements that match the conditions defined by the specified predicate, if found; otherwise, an empty <see cref="IEnumerable{T}"/>.</returns>
        public static IEnumerable<TSource> FindAll<TSource, T1, T2, T3, T4, T5, T6, T7>(this IEnumerable<TSource> source, Func<TSource, T1, T2, T3, T4, T5, T6, T7, bool> match, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7)
        {
            return EnumerableUtility.FindAll(source, match, arg1, arg2, arg3, arg4, arg5, arg6, arg7);
        }

        /// <summary>
        /// Retrieves all the elements that match the conditions defined by the specified function delegate.
        /// </summary>
        /// <typeparam name="TSource">The type of the elements of the sequence.</typeparam>
        /// <typeparam name="T1">The type of the first parameter of the delegate <paramref name="match"/>.</typeparam>
        /// <typeparam name="T2">The type of the second parameter of the delegate <paramref name="match"/>.</typeparam>
        /// <typeparam name="T3">The type of the third parameter of the delegate <paramref name="match"/>.</typeparam>
        /// <typeparam name="T4">The type of the fourth parameter of the delegate <paramref name="match"/>.</typeparam>
        /// <typeparam name="T5">The type of the fifth parameter of the delegate <paramref name="match"/>.</typeparam>
        /// <typeparam name="T6">The type of the sixth parameter of the delegate <paramref name="match"/>.</typeparam>
        /// <typeparam name="T7">The type of the seventh parameter of the delegate <paramref name="match"/>.</typeparam>
        /// <typeparam name="T8">The type of the eighth parameter of the delegate <paramref name="match"/>.</typeparam>
        /// <param name="source">The sequence to search.</param>
        /// <param name="match">The function delegate that defines the conditions of the elements to search for.</param>
        /// <param name="arg1">The first parameter of the delegate <paramref name="match"/>.</param>
        /// <param name="arg2">The second parameter of the delegate <paramref name="match"/>.</param>
        /// <param name="arg3">The third parameter of the delegate <paramref name="match"/>.</param>
        /// <param name="arg4">The fourth parameter of the delegate <paramref name="match"/>.</param>
        /// <param name="arg5">The fifth parameter of the delegate <paramref name="match"/>.</param>
        /// <param name="arg6">The sixth parameter of the delegate <paramref name="match"/>.</param>
        /// <param name="arg7">The seventh parameter of the delegate <paramref name="match"/>.</param>
        /// <param name="arg8">The eighth parameter of the delegate <paramref name="match"/>.</param>
        /// <returns>An <see cref="IEnumerable{T}"/> sequence containing all the elements that match the conditions defined by the specified predicate, if found; otherwise, an empty <see cref="IEnumerable{T}"/>.</returns>
        public static IEnumerable<TSource> FindAll<TSource, T1, T2, T3, T4, T5, T6, T7, T8>(this IEnumerable<TSource> source, Func<TSource, T1, T2, T3, T4, T5, T6, T7, T8, bool> match, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8)
        {
            return EnumerableUtility.FindAll(source, match, arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8);
        }

        /// <summary>
        /// Retrieves all the elements that match the conditions defined by the specified function delegate.
        /// </summary>
        /// <typeparam name="TSource">The type of the elements of the sequence.</typeparam>
        /// <typeparam name="T1">The type of the first parameter of the delegate <paramref name="match"/>.</typeparam>
        /// <typeparam name="T2">The type of the second parameter of the delegate <paramref name="match"/>.</typeparam>
        /// <typeparam name="T3">The type of the third parameter of the delegate <paramref name="match"/>.</typeparam>
        /// <typeparam name="T4">The type of the fourth parameter of the delegate <paramref name="match"/>.</typeparam>
        /// <typeparam name="T5">The type of the fifth parameter of the delegate <paramref name="match"/>.</typeparam>
        /// <typeparam name="T6">The type of the sixth parameter of the delegate <paramref name="match"/>.</typeparam>
        /// <typeparam name="T7">The type of the seventh parameter of the delegate <paramref name="match"/>.</typeparam>
        /// <typeparam name="T8">The type of the eighth parameter of the delegate <paramref name="match"/>.</typeparam>
        /// <typeparam name="T9">The type of the ninth parameter of the delegate <paramref name="match"/>.</typeparam>
        /// <param name="source">The sequence to search.</param>
        /// <param name="match">The function delegate that defines the conditions of the elements to search for.</param>
        /// <param name="arg1">The first parameter of the delegate <paramref name="match"/>.</param>
        /// <param name="arg2">The second parameter of the delegate <paramref name="match"/>.</param>
        /// <param name="arg3">The third parameter of the delegate <paramref name="match"/>.</param>
        /// <param name="arg4">The fourth parameter of the delegate <paramref name="match"/>.</param>
        /// <param name="arg5">The fifth parameter of the delegate <paramref name="match"/>.</param>
        /// <param name="arg6">The sixth parameter of the delegate <paramref name="match"/>.</param>
        /// <param name="arg7">The seventh parameter of the delegate <paramref name="match"/>.</param>
        /// <param name="arg8">The eighth parameter of the delegate <paramref name="match"/>.</param>
        /// <param name="arg9">The ninth parameter of the delegate <paramref name="match"/>.</param>
        /// <returns>An <see cref="IEnumerable{T}"/> sequence containing all the elements that match the conditions defined by the specified predicate, if found; otherwise, an empty <see cref="IEnumerable{T}"/>.</returns>
        public static IEnumerable<TSource> FindAll<TSource, T1, T2, T3, T4, T5, T6, T7, T8, T9>(this IEnumerable<TSource> source, Func<TSource, T1, T2, T3, T4, T5, T6, T7, T8, T9, bool> match, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8, T9 arg9)
        {
            return EnumerableUtility.FindAll(source, match, arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9);
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
        public static bool Contains<TSource>(this IEnumerable<TSource> source, TSource value, Func<TSource, TSource, bool> condition)
        {
            return EnumerableUtility.Contains(source, value, condition);
        }

        /// <summary>
        /// Returns an <see cref="IEnumerable{T}"/> sequence with the specified <paramref name="source"/> as the only element.
        /// </summary>
        /// <typeparam name="TSource">The type of <paramref name="source"/>.</typeparam>
        /// <param name="source">The value to yield into an <see cref="IEnumerable{T}"/> sequence.</param>
        /// <returns>An <see cref="IEnumerable{T}"/> sequence with the specified <paramref name="source"/> as the only element.</returns>
        public static IEnumerable<TSource> Yield<TSource>(this TSource source)
        {
            return EnumerableUtility.Yield(source);
        }
    }
}
