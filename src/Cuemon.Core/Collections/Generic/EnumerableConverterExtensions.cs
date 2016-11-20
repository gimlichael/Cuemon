using System;
using System.Collections.Generic;

namespace Cuemon.Collections.Generic
{
    /// <summary>
    /// This is an extension implementation of the <see cref="EnumerableConverter"/> class.
    /// </summary>
    public static class EnumerableConverterExtensions
    {
        /// <summary>
        /// Converts the specified <paramref name="source"/> to its <see cref="KeyValuePair{TKey,TValue}"/> equivalent sequence.
        /// </summary>
        /// <typeparam name="TKey">The <see cref="Type"/> of the key in the resulting <see cref="KeyValuePair{TKey,TValue}"/>.</typeparam>
        /// <typeparam name="TValue">The <see cref="Type"/> of the value in the resulting <see cref="KeyValuePair{TKey,TValue}"/>.</typeparam>
        /// <param name="source">An <see cref="IDictionary{TKey,TValue}"/> to convert into a <see cref="KeyValuePair{TKey,TValue}"/> equivalent sequence.</param>
        /// <returns>A <see cref="KeyValuePair{TKey,TValue}"/> equivalent sequence of <paramref name="source"/>.</returns>
        /// <exception cref="System.ArgumentNullException">
        /// <paramref name="source"/> is null.
        /// </exception>
        public static IEnumerable<KeyValuePair<TKey, TValue>> ToEnumerable<TKey, TValue>(this IDictionary<TKey, TValue> source)
        {
            return EnumerableConverter.FromDictionary(source);
        }

        /// <summary>
        /// Converts the elements of an <see cref="IEnumerable{T}"/> to the specified type.
        /// </summary>
        /// <typeparam name="TSource">The original source type of the elements of <paramref name="source"/>.</typeparam>
        /// <typeparam name="TResult">The converted result type of the elements of <paramref name="source"/>.</typeparam>
        /// <param name="source">The <see cref="IEnumerable{T}"/> that contains the elements to be cast to type <typeparamref name="TResult"/>.</param>
        /// <param name="converter">The function delegate that converts <typeparamref name="TSource"/> to a <typeparamref name="TResult"/> representation once per iteration.</param>
        /// <returns>An <see cref="IEnumerable{T}"/> that contains each element of the <paramref name="source"/> sequence converted to the specified <typeparamref name="TResult"/>.</returns>
        public static IEnumerable<TResult> ParseSequenceWith<TSource, TResult>(this IEnumerable<TSource> source, Func<TSource, TResult> converter)
        {
            return EnumerableConverter.Parse(source, converter);
        }

        /// <summary>
        /// Converts the elements of an <see cref="IEnumerable{T}" /> to the specified type.
        /// </summary>
        /// <typeparam name="TSource">The original source type of the elements of <paramref name="source" />.</typeparam>
        /// <typeparam name="T">The type of the parameter of the function delegate <paramref name="converter"/>.</typeparam>
        /// <typeparam name="TResult">The converted result type of the elements of <paramref name="source" />.</typeparam>
        /// <param name="source">The <see cref="IEnumerable{T}" /> that contains the elements to be cast to type <typeparamref name="TResult" />.</param>
        /// <param name="converter">The function delegate that converts <typeparamref name="TSource" /> to a <typeparamref name="TResult" /> representation once per iteration.</param>
        /// <param name="arg">The parameter of the function delegate <paramref name="converter"/>.</param>
        /// <returns>An <see cref="IEnumerable{T}" /> that contains each element of the <paramref name="source" /> sequence converted to the specified <typeparamref name="TResult" />.</returns>
        public static IEnumerable<TResult> ParseSequenceWith<TSource, T, TResult>(this IEnumerable<TSource> source, Func<TSource, T, TResult> converter, T arg)
        {
            return EnumerableConverter.Parse(source, converter, arg);
        }

        /// <summary>
        /// Converts the elements of an <see cref="IEnumerable{T}" /> to the specified type.
        /// </summary>
        /// <typeparam name="TSource">The original source type of the elements of <paramref name="source" />.</typeparam>
        /// <typeparam name="T1">The type of the first parameter of the function delegate <paramref name="converter"/>.</typeparam>
        /// <typeparam name="T2">The type of the second parameter of the function delegate <paramref name="converter"/>.</typeparam>
        /// <typeparam name="TResult">The converted result type of the elements of <paramref name="source" />.</typeparam>
        /// <param name="source">The <see cref="IEnumerable{T}" /> that contains the elements to be cast to type <typeparamref name="TResult" />.</param>
        /// <param name="converter">The function delegate that converts <typeparamref name="TSource" /> to a <typeparamref name="TResult" /> representation once per iteration.</param>
        /// <param name="arg1">The first parameter of the function delegate <paramref name="converter"/>.</param>
        /// <param name="arg2">The second parameter of the function delegate <paramref name="converter"/>.</param>
        /// <returns>An <see cref="IEnumerable{T}" /> that contains each element of the <paramref name="source" /> sequence converted to the specified <typeparamref name="TResult" />.</returns>
        public static IEnumerable<TResult> ParseSequenceWith<TSource, T1, T2, TResult>(this IEnumerable<TSource> source, Func<TSource, T1, T2, TResult> converter, T1 arg1, T2 arg2)
        {
            return EnumerableConverter.Parse(source, converter, arg1, arg2);
        }

        /// <summary>
        /// Converts the elements of an <see cref="IEnumerable{T}" /> to the specified type.
        /// </summary>
        /// <typeparam name="TSource">The original source type of the elements of <paramref name="source" />.</typeparam>
        /// <typeparam name="T1">The type of the first parameter of the function delegate <paramref name="converter"/>.</typeparam>
        /// <typeparam name="T2">The type of the second parameter of the function delegate <paramref name="converter"/>.</typeparam>
        /// <typeparam name="T3">The type of the third parameter of the function delegate <paramref name="converter"/>.</typeparam>
        /// <typeparam name="TResult">The converted result type of the elements of <paramref name="source" />.</typeparam>
        /// <param name="source">The <see cref="IEnumerable{T}" /> that contains the elements to be cast to type <typeparamref name="TResult" />.</param>
        /// <param name="converter">The function delegate that converts <typeparamref name="TSource" /> to a <typeparamref name="TResult" /> representation once per iteration.</param>
        /// <param name="arg1">The first parameter of the function delegate <paramref name="converter"/>.</param>
        /// <param name="arg2">The second parameter of the function delegate <paramref name="converter"/>.</param>
        /// <param name="arg3">The third parameter of the function delegate <paramref name="converter"/>.</param>
        /// <returns>An <see cref="IEnumerable{T}" /> that contains each element of the <paramref name="source" /> sequence converted to the specified <typeparamref name="TResult" />.</returns>
        public static IEnumerable<TResult> ParseSequenceWith<TSource, T1, T2, T3, TResult>(this IEnumerable<TSource> source, Func<TSource, T1, T2, T3, TResult> converter, T1 arg1, T2 arg2, T3 arg3)
        {
            return EnumerableConverter.Parse(source, converter, arg1, arg2, arg3);
        }

        /// <summary>
        /// Converts the elements of an <see cref="IEnumerable{T}" /> to the specified type.
        /// </summary>
        /// <typeparam name="TSource">The original source type of the elements of <paramref name="source" />.</typeparam>
        /// <typeparam name="T1">The type of the first parameter of the function delegate <paramref name="converter"/>.</typeparam>
        /// <typeparam name="T2">The type of the second parameter of the function delegate <paramref name="converter"/>.</typeparam>
        /// <typeparam name="T3">The type of the third parameter of the function delegate <paramref name="converter"/>.</typeparam>
        /// <typeparam name="T4">The type of the fourth parameter of the function delegate <paramref name="converter"/>.</typeparam>
        /// <typeparam name="TResult">The converted result type of the elements of <paramref name="source" />.</typeparam>
        /// <param name="source">The <see cref="IEnumerable{T}" /> that contains the elements to be cast to type <typeparamref name="TResult" />.</param>
        /// <param name="converter">The function delegate that converts <typeparamref name="TSource" /> to a <typeparamref name="TResult" /> representation once per iteration.</param>
        /// <param name="arg1">The first parameter of the function delegate <paramref name="converter"/>.</param>
        /// <param name="arg2">The second parameter of the function delegate <paramref name="converter"/>.</param>
        /// <param name="arg3">The third parameter of the function delegate <paramref name="converter"/>.</param>
        /// <param name="arg4">The fourth parameter of the function delegate <paramref name="converter"/>.</param>
        /// <returns>An <see cref="IEnumerable{T}" /> that contains each element of the <paramref name="source" /> sequence converted to the specified <typeparamref name="TResult" />.</returns>
        public static IEnumerable<TResult> ParseSequenceWith<TSource, T1, T2, T3, T4, TResult>(this IEnumerable<TSource> source, Func<TSource, T1, T2, T3, T4, TResult> converter, T1 arg1, T2 arg2, T3 arg3, T4 arg4)
        {
            return EnumerableConverter.Parse(source, converter, arg1, arg2, arg3, arg4);
        }

        /// <summary>
        /// Converts the elements of an <see cref="IEnumerable{T}" /> to the specified type.
        /// </summary>
        /// <typeparam name="TSource">The original source type of the elements of <paramref name="source" />.</typeparam>
        /// <typeparam name="T1">The type of the first parameter of the function delegate <paramref name="converter"/>.</typeparam>
        /// <typeparam name="T2">The type of the second parameter of the function delegate <paramref name="converter"/>.</typeparam>
        /// <typeparam name="T3">The type of the third parameter of the function delegate <paramref name="converter"/>.</typeparam>
        /// <typeparam name="T4">The type of the fourth parameter of the function delegate <paramref name="converter"/>.</typeparam>
        /// <typeparam name="T5">The type of the fifth parameter of the function delegate <paramref name="converter"/>.</typeparam>
        /// <typeparam name="TResult">The converted result type of the elements of <paramref name="source" />.</typeparam>
        /// <param name="source">The <see cref="IEnumerable{T}" /> that contains the elements to be cast to type <typeparamref name="TResult" />.</param>
        /// <param name="converter">The function delegate that converts <typeparamref name="TSource" /> to a <typeparamref name="TResult" /> representation once per iteration.</param>
        /// <param name="arg1">The first parameter of the function delegate <paramref name="converter"/>.</param>
        /// <param name="arg2">The second parameter of the function delegate <paramref name="converter"/>.</param>
        /// <param name="arg3">The third parameter of the function delegate <paramref name="converter"/>.</param>
        /// <param name="arg4">The fourth parameter of the function delegate <paramref name="converter"/>.</param>
        /// <param name="arg5">The fifth parameter of the function delegate <paramref name="converter"/>.</param>
        /// <returns>An <see cref="IEnumerable{T}" /> that contains each element of the <paramref name="source" /> sequence converted to the specified <typeparamref name="TResult" />.</returns>
        public static IEnumerable<TResult> ParseSequenceWith<TSource, T1, T2, T3, T4, T5, TResult>(this IEnumerable<TSource> source, Func<TSource, T1, T2, T3, T4, T5, TResult> converter, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5)
        {
            return EnumerableConverter.Parse(source, converter, arg1, arg2, arg3, arg4, arg5);
        }

        /// <summary>
        /// Converts the elements of an <see cref="IEnumerable{T}" /> to the specified type.
        /// </summary>
        /// <typeparam name="TSource">The original source type of the elements of <paramref name="source" />.</typeparam>
        /// <typeparam name="T1">The type of the first parameter of the function delegate <paramref name="converter"/>.</typeparam>
        /// <typeparam name="T2">The type of the second parameter of the function delegate <paramref name="converter"/>.</typeparam>
        /// <typeparam name="T3">The type of the third parameter of the function delegate <paramref name="converter"/>.</typeparam>
        /// <typeparam name="T4">The type of the fourth parameter of the function delegate <paramref name="converter"/>.</typeparam>
        /// <typeparam name="T5">The type of the fifth parameter of the function delegate <paramref name="converter"/>.</typeparam>
        /// <typeparam name="T6">The type of the sixth parameter of the function delegate <paramref name="converter"/>.</typeparam>
        /// <typeparam name="TResult">The converted result type of the elements of <paramref name="source" />.</typeparam>
        /// <param name="source">The <see cref="IEnumerable{T}" /> that contains the elements to be cast to type <typeparamref name="TResult" />.</param>
        /// <param name="converter">The function delegate that converts <typeparamref name="TSource" /> to a <typeparamref name="TResult" /> representation once per iteration.</param>
        /// <param name="arg1">The first parameter of the function delegate <paramref name="converter"/>.</param>
        /// <param name="arg2">The second parameter of the function delegate <paramref name="converter"/>.</param>
        /// <param name="arg3">The third parameter of the function delegate <paramref name="converter"/>.</param>
        /// <param name="arg4">The fourth parameter of the function delegate <paramref name="converter"/>.</param>
        /// <param name="arg5">The fifth parameter of the function delegate <paramref name="converter"/>.</param>
        /// <param name="arg6">The sixth parameter of the function delegate <paramref name="converter"/>.</param>
        /// <returns>An <see cref="IEnumerable{T}" /> that contains each element of the <paramref name="source" /> sequence converted to the specified <typeparamref name="TResult" />.</returns>
        public static IEnumerable<TResult> ParseSequenceWith<TSource, T1, T2, T3, T4, T5, T6, TResult>(this IEnumerable<TSource> source, Func<TSource, T1, T2, T3, T4, T5, T6, TResult> converter, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6)
        {
            return EnumerableConverter.Parse(source, converter, arg1, arg2, arg3, arg4, arg5, arg6);
        }

        /// <summary>
        /// Converts the elements of an <see cref="IEnumerable{T}" /> to the specified type.
        /// </summary>
        /// <typeparam name="TSource">The original source type of the elements of <paramref name="source" />.</typeparam>
        /// <typeparam name="T1">The type of the first parameter of the function delegate <paramref name="converter"/>.</typeparam>
        /// <typeparam name="T2">The type of the second parameter of the function delegate <paramref name="converter"/>.</typeparam>
        /// <typeparam name="T3">The type of the third parameter of the function delegate <paramref name="converter"/>.</typeparam>
        /// <typeparam name="T4">The type of the fourth parameter of the function delegate <paramref name="converter"/>.</typeparam>
        /// <typeparam name="T5">The type of the fifth parameter of the function delegate <paramref name="converter"/>.</typeparam>
        /// <typeparam name="T6">The type of the sixth parameter of the function delegate <paramref name="converter"/>.</typeparam>
        /// <typeparam name="T7">The type of the seventh parameter of the function delegate <paramref name="converter"/>.</typeparam>
        /// <typeparam name="TResult">The converted result type of the elements of <paramref name="source" />.</typeparam>
        /// <param name="source">The <see cref="IEnumerable{T}" /> that contains the elements to be cast to type <typeparamref name="TResult" />.</param>
        /// <param name="converter">The function delegate that converts <typeparamref name="TSource" /> to a <typeparamref name="TResult" /> representation once per iteration.</param>
        /// <param name="arg1">The first parameter of the function delegate <paramref name="converter"/>.</param>
        /// <param name="arg2">The second parameter of the function delegate <paramref name="converter"/>.</param>
        /// <param name="arg3">The third parameter of the function delegate <paramref name="converter"/>.</param>
        /// <param name="arg4">The fourth parameter of the function delegate <paramref name="converter"/>.</param>
        /// <param name="arg5">The fifth parameter of the function delegate <paramref name="converter"/>.</param>
        /// <param name="arg6">The sixth parameter of the function delegate <paramref name="converter"/>.</param>
        /// <param name="arg7">The seventh parameter of the function delegate <paramref name="converter"/>.</param>
        /// <returns>An <see cref="IEnumerable{T}" /> that contains each element of the <paramref name="source" /> sequence converted to the specified <typeparamref name="TResult" />.</returns>
        public static IEnumerable<TResult> ParseSequenceWith<TSource, T1, T2, T3, T4, T5, T6, T7, TResult>(this IEnumerable<TSource> source, Func<TSource, T1, T2, T3, T4, T5, T6, T7, TResult> converter, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7)
        {
            return EnumerableConverter.Parse(source, converter, arg1, arg2, arg3, arg4, arg5, arg6, arg7);
        }

        /// <summary>
        /// Converts the elements of an <see cref="IEnumerable{T}" /> to the specified type.
        /// </summary>
        /// <typeparam name="TSource">The original source type of the elements of <paramref name="source" />.</typeparam>
        /// <typeparam name="T1">The type of the first parameter of the function delegate <paramref name="converter"/>.</typeparam>
        /// <typeparam name="T2">The type of the second parameter of the function delegate <paramref name="converter"/>.</typeparam>
        /// <typeparam name="T3">The type of the third parameter of the function delegate <paramref name="converter"/>.</typeparam>
        /// <typeparam name="T4">The type of the fourth parameter of the function delegate <paramref name="converter"/>.</typeparam>
        /// <typeparam name="T5">The type of the fifth parameter of the function delegate <paramref name="converter"/>.</typeparam>
        /// <typeparam name="T6">The type of the sixth parameter of the function delegate <paramref name="converter"/>.</typeparam>
        /// <typeparam name="T7">The type of the seventh parameter of the function delegate <paramref name="converter"/>.</typeparam>
        /// <typeparam name="T8">The type of the eighth parameter of the function delegate <paramref name="converter"/>.</typeparam>
        /// <typeparam name="TResult">The converted result type of the elements of <paramref name="source" />.</typeparam>
        /// <param name="source">The <see cref="IEnumerable{T}" /> that contains the elements to be cast to type <typeparamref name="TResult" />.</param>
        /// <param name="converter">The function delegate that converts <typeparamref name="TSource" /> to a <typeparamref name="TResult" /> representation once per iteration.</param>
        /// <param name="arg1">The first parameter of the function delegate <paramref name="converter"/>.</param>
        /// <param name="arg2">The second parameter of the function delegate <paramref name="converter"/>.</param>
        /// <param name="arg3">The third parameter of the function delegate <paramref name="converter"/>.</param>
        /// <param name="arg4">The fourth parameter of the function delegate <paramref name="converter"/>.</param>
        /// <param name="arg5">The fifth parameter of the function delegate <paramref name="converter"/>.</param>
        /// <param name="arg6">The sixth parameter of the function delegate <paramref name="converter"/>.</param>
        /// <param name="arg7">The seventh parameter of the function delegate <paramref name="converter"/>.</param>
        /// <param name="arg8">The eighth parameter of the function delegate <paramref name="converter"/>.</param>
        /// <returns>An <see cref="IEnumerable{T}" /> that contains each element of the <paramref name="source" /> sequence converted to the specified <typeparamref name="TResult" />.</returns>
        public static IEnumerable<TResult> ParseSequenceWith<TSource, T1, T2, T3, T4, T5, T6, T7, T8, TResult>(this IEnumerable<TSource> source, Func<TSource, T1, T2, T3, T4, T5, T6, T7, T8, TResult> converter, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8)
        {
            return EnumerableConverter.Parse(source, converter, arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8);
        }

        /// <summary>
        /// Converts the elements of an <see cref="IEnumerable{T}" /> to the specified type.
        /// </summary>
        /// <typeparam name="TSource">The original source type of the elements of <paramref name="source" />.</typeparam>
        /// <typeparam name="T1">The type of the first parameter of the function delegate <paramref name="converter"/>.</typeparam>
        /// <typeparam name="T2">The type of the second parameter of the function delegate <paramref name="converter"/>.</typeparam>
        /// <typeparam name="T3">The type of the third parameter of the function delegate <paramref name="converter"/>.</typeparam>
        /// <typeparam name="T4">The type of the fourth parameter of the function delegate <paramref name="converter"/>.</typeparam>
        /// <typeparam name="T5">The type of the fifth parameter of the function delegate <paramref name="converter"/>.</typeparam>
        /// <typeparam name="T6">The type of the sixth parameter of the function delegate <paramref name="converter"/>.</typeparam>
        /// <typeparam name="T7">The type of the seventh parameter of the function delegate <paramref name="converter"/>.</typeparam>
        /// <typeparam name="T8">The type of the eighth parameter of the function delegate <paramref name="converter"/>.</typeparam>
        /// <typeparam name="T9">The type of the ninth parameter of the function delegate <paramref name="converter"/>.</typeparam>
        /// <typeparam name="TResult">The converted result type of the elements of <paramref name="source" />.</typeparam>
        /// <param name="source">The <see cref="IEnumerable{T}" /> that contains the elements to be cast to type <typeparamref name="TResult" />.</param>
        /// <param name="converter">The function delegate that converts <typeparamref name="TSource" /> to a <typeparamref name="TResult" /> representation once per iteration.</param>
        /// <param name="arg1">The first parameter of the function delegate <paramref name="converter"/>.</param>
        /// <param name="arg2">The second parameter of the function delegate <paramref name="converter"/>.</param>
        /// <param name="arg3">The third parameter of the function delegate <paramref name="converter"/>.</param>
        /// <param name="arg4">The fourth parameter of the function delegate <paramref name="converter"/>.</param>
        /// <param name="arg5">The fifth parameter of the function delegate <paramref name="converter"/>.</param>
        /// <param name="arg6">The sixth parameter of the function delegate <paramref name="converter"/>.</param>
        /// <param name="arg7">The seventh parameter of the function delegate <paramref name="converter"/>.</param>
        /// <param name="arg8">The eighth parameter of the function delegate <paramref name="converter"/>.</param>
        /// <param name="arg9">The ninth parameter of the function delegate <paramref name="converter"/>.</param>
        /// <returns>An <see cref="IEnumerable{T}" /> that contains each element of the <paramref name="source" /> sequence converted to the specified <typeparamref name="TResult" />.</returns>
        public static IEnumerable<TResult> ParseSequenceWith<TSource, T1, T2, T3, T4, T5, T6, T7, T8, T9, TResult>(this IEnumerable<TSource> source, Func<TSource, T1, T2, T3, T4, T5, T6, T7, T8, T9, TResult> converter, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8, T9 arg9)
        {
            return EnumerableConverter.Parse(source, converter, arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9);
        }
    }
}