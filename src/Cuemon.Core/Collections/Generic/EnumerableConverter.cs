using System;
using System.Collections.Generic;

namespace Cuemon.Collections.Generic
{
    /// <summary>
    /// This utility class is designed to make <see cref="IEnumerable{T}"/> related conversions easier to work with.
    /// </summary>
    public static class EnumerableConverter
    {
        /// <summary>
        /// Returns the input typed as <see cref="IEnumerable{T}"/>.
        /// </summary>
        /// <typeparam name="TSource">The type of the elements of <paramref name="source"/>.</typeparam>
        /// <param name="source">The array to type as <see cref="IEnumerable{T}"/>.</param>
        /// <returns>The input <paramref name="source"/> typed as <see cref="IEnumerable{T}"/>.</returns>
	    public static IEnumerable<TSource> FromArray<TSource>(params TSource[] source)
        {
            return source;
        }

        /// <summary>
        /// Converts the specified <paramref name="values"/> to a one-dimensional array of the specified type, with zero-based indexing.
        /// </summary>
        /// <typeparam name="TSource">The type of the array of <paramref name="values"/>.</typeparam>
        /// <param name="values">The values to create the <see cref="Array"/> from.</param>
        /// <returns>A one-dimensional <see cref="Array"/> of the specified <see typeparamref="TSource"/> with a length equal to the values specified.</returns>
        public static TSource[] AsArray<TSource>(params TSource[] values)
        {
            return values;
        }

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
        public static IEnumerable<KeyValuePair<TKey, TValue>> FromDictionary<TKey, TValue>(IDictionary<TKey, TValue> source)
        {
            Validator.ThrowIfNull(source, nameof(source));
            foreach (KeyValuePair<TKey, TValue> keyValuePair in source)
            {
                yield return keyValuePair;
            }
        }

        /// <summary>
        /// Converts the specified <paramref name="source"/> to a <typeparamref name="TResult"/> representation using the specified <paramref name="converter"/>.
        /// </summary>
        /// <typeparam name="TSource">The type of the <paramref name="source"/> to convert.</typeparam>
        /// <typeparam name="TResult">The type of the converted result.</typeparam>
        /// <param name="source">The <see cref="IEnumerable{T}" /> source to parse and convert using the function delegate <paramref name="converter"/>.</param>
        /// <param name="converter">The function delegate that converts <typeparamref name="TSource" /> to a <typeparamref name="TResult" /> representation once per iteration.</param>
        /// <returns>An <see cref="IEnumerable{TResult}" /> that is equivalent to the <typeparamref name="TSource"/> contained in <paramref name="source"/>.</returns>
        /// <exception cref="System.ArgumentNullException">
        /// <paramref name="source"/> is null - or - <paramref name="converter"/> is null.
        /// </exception>
        public static IEnumerable<TResult> Parse<TSource, TResult>(IEnumerable<TSource> source, Doer<TSource, TResult> converter)
        {
            Validator.ThrowIfNull(source, nameof(source));
            Validator.ThrowIfNull(converter, nameof(converter));
            var factory = DoerFactory.Create(converter, default(TSource));
            return ParseCore(factory, source);
        }

        /// <summary>
        /// Converts the specified <paramref name="source"/> to a <typeparamref name="TResult"/> representation using the specified <paramref name="converter"/>.
        /// </summary>
        /// <typeparam name="TSource">The type of the <paramref name="source"/> to convert.</typeparam>
        /// <typeparam name="T">The type of the parameter of the function delegate <paramref name="converter"/>.</typeparam>
        /// <typeparam name="TResult">The type of the converted result.</typeparam>
        /// <param name="source">The <see cref="IEnumerable{T}" /> source to parse and convert using the function delegate <paramref name="converter"/>.</param>
        /// <param name="converter">The function delegate that converts <typeparamref name="TSource" /> to a <typeparamref name="TResult" /> representation once per iteration.</param>
        /// <param name="arg">The parameter of the function delegate <paramref name="converter"/>.</param>
        /// <returns>An <see cref="IEnumerable{TResult}" /> that is equivalent to the <typeparamref name="TSource"/> contained in <paramref name="source"/>.</returns>
        /// <exception cref="System.ArgumentNullException">
        /// <paramref name="source"/> is null - or - <paramref name="converter"/> is null.
        /// </exception>
        public static IEnumerable<TResult> Parse<TSource, T, TResult>(IEnumerable<TSource> source, Doer<TSource, T, TResult> converter, T arg)
        {
            Validator.ThrowIfNull(source, nameof(source));
            Validator.ThrowIfNull(converter, nameof(converter));
            var factory = DoerFactory.Create(converter, default(TSource), arg);
            return ParseCore(factory, source);
        }

        /// <summary>
        /// Converts the specified <paramref name="source"/> to a <typeparamref name="TResult"/> representation using the specified <paramref name="converter"/>.
        /// </summary>
        /// <typeparam name="TSource">The type of the <paramref name="source"/> to convert.</typeparam>
        /// <typeparam name="T1">The type of the first parameter of the function delegate <paramref name="converter"/>.</typeparam>
        /// <typeparam name="T2">The type of the second parameter of the function delegate <paramref name="converter"/>.</typeparam>
        /// <typeparam name="TResult">The type of the converted result.</typeparam>
        /// <param name="source">The <see cref="IEnumerable{T}" /> source to parse and convert using the function delegate <paramref name="converter"/>.</param>
        /// <param name="converter">The function delegate that converts <typeparamref name="TSource" /> to a <typeparamref name="TResult" /> representation once per iteration.</param>
        /// <param name="arg1">The first parameter of the function delegate <paramref name="converter"/>.</param>
        /// <param name="arg2">The second parameter of the function delegate <paramref name="converter"/>.</param>
        /// <returns>An <see cref="IEnumerable{TResult}" /> that is equivalent to the <typeparamref name="TSource"/> contained in <paramref name="source"/>.</returns>
        /// <exception cref="System.ArgumentNullException">
        /// <paramref name="source"/> is null - or - <paramref name="converter"/> is null.
        /// </exception>
        public static IEnumerable<TResult> Parse<TSource, T1, T2, TResult>(IEnumerable<TSource> source, Doer<TSource, T1, T2, TResult> converter, T1 arg1, T2 arg2)
        {
            Validator.ThrowIfNull(source, nameof(source));
            Validator.ThrowIfNull(converter, nameof(converter));
            var factory = DoerFactory.Create(converter, default(TSource), arg1, arg2);
            return ParseCore(factory, source);
        }

        /// <summary>
        /// Converts the specified <paramref name="source"/> to a <typeparamref name="TResult"/> representation using the specified <paramref name="converter"/>.
        /// </summary>
        /// <typeparam name="TSource">The type of the <paramref name="source"/> to convert.</typeparam>
        /// <typeparam name="T1">The type of the first parameter of the function delegate <paramref name="converter"/>.</typeparam>
        /// <typeparam name="T2">The type of the second parameter of the function delegate <paramref name="converter"/>.</typeparam>
        /// <typeparam name="T3">The type of the third parameter of the function delegate <paramref name="converter"/>.</typeparam>
        /// <typeparam name="TResult">The type of the converted result.</typeparam>
        /// <param name="source">The <see cref="IEnumerable{T}" /> source to parse and convert using the function delegate <paramref name="converter"/>.</param>
        /// <param name="converter">The function delegate that converts <typeparamref name="TSource" /> to a <typeparamref name="TResult" /> representation once per iteration.</param>
        /// <param name="arg1">The first parameter of the function delegate <paramref name="converter"/>.</param>
        /// <param name="arg2">The second parameter of the function delegate <paramref name="converter"/>.</param>
        /// <param name="arg3">The third parameter of the function delegate <paramref name="converter"/>.</param>
        /// <returns>An <see cref="IEnumerable{TResult}" /> that is equivalent to the <typeparamref name="TSource"/> contained in <paramref name="source"/>.</returns>
        /// <exception cref="System.ArgumentNullException">
        /// <paramref name="source"/> is null - or - <paramref name="converter"/> is null.
        /// </exception>
        public static IEnumerable<TResult> Parse<TSource, T1, T2, T3, TResult>(IEnumerable<TSource> source, Doer<TSource, T1, T2, T3, TResult> converter, T1 arg1, T2 arg2, T3 arg3)
        {
            Validator.ThrowIfNull(source, nameof(source));
            Validator.ThrowIfNull(converter, nameof(converter));
            var factory = DoerFactory.Create(converter, default(TSource), arg1, arg2, arg3);
            return ParseCore(factory, source);
        }

        /// <summary>
        /// Converts the specified <paramref name="source"/> to a <typeparamref name="TResult"/> representation using the specified <paramref name="converter"/>.
        /// </summary>
        /// <typeparam name="TSource">The type of the <paramref name="source"/> to convert.</typeparam>
        /// <typeparam name="T1">The type of the first parameter of the function delegate <paramref name="converter"/>.</typeparam>
        /// <typeparam name="T2">The type of the second parameter of the function delegate <paramref name="converter"/>.</typeparam>
        /// <typeparam name="T3">The type of the third parameter of the function delegate <paramref name="converter"/>.</typeparam>
        /// <typeparam name="T4">The type of the fourth parameter of the function delegate <paramref name="converter"/>.</typeparam>
        /// <typeparam name="TResult">The type of the converted result.</typeparam>
        /// <param name="source">The <see cref="IEnumerable{T}" /> source to parse and convert using the function delegate <paramref name="converter"/>.</param>
        /// <param name="converter">The function delegate that converts <typeparamref name="TSource" /> to a <typeparamref name="TResult" /> representation once per iteration.</param>
        /// <param name="arg1">The first parameter of the function delegate <paramref name="converter"/>.</param>
        /// <param name="arg2">The second parameter of the function delegate <paramref name="converter"/>.</param>
        /// <param name="arg3">The third parameter of the function delegate <paramref name="converter"/>.</param>
        /// <param name="arg4">The fourth parameter of the function delegate <paramref name="converter"/>.</param>
        /// <returns>An <see cref="IEnumerable{TResult}" /> that is equivalent to the <typeparamref name="TSource"/> contained in <paramref name="source"/>.</returns>
        /// <exception cref="System.ArgumentNullException">
        /// <paramref name="source"/> is null - or - <paramref name="converter"/> is null.
        /// </exception>
        public static IEnumerable<TResult> Parse<TSource, T1, T2, T3, T4, TResult>(IEnumerable<TSource> source, Doer<TSource, T1, T2, T3, T4, TResult> converter, T1 arg1, T2 arg2, T3 arg3, T4 arg4)
        {
            Validator.ThrowIfNull(source, nameof(source));
            Validator.ThrowIfNull(converter, nameof(converter));
            var factory = DoerFactory.Create(converter, default(TSource), arg1, arg2, arg3, arg4);
            return ParseCore(factory, source);
        }

        /// <summary>
        /// Converts the specified <paramref name="source"/> to a <typeparamref name="TResult"/> representation using the specified <paramref name="converter"/>.
        /// </summary>
        /// <typeparam name="TSource">The type of the <paramref name="source"/> to convert.</typeparam>
        /// <typeparam name="T1">The type of the first parameter of the function delegate <paramref name="converter"/>.</typeparam>
        /// <typeparam name="T2">The type of the second parameter of the function delegate <paramref name="converter"/>.</typeparam>
        /// <typeparam name="T3">The type of the third parameter of the function delegate <paramref name="converter"/>.</typeparam>
        /// <typeparam name="T4">The type of the fourth parameter of the function delegate <paramref name="converter"/>.</typeparam>
        /// <typeparam name="T5">The type of the fifth parameter of the function delegate <paramref name="converter"/>.</typeparam>
        /// <typeparam name="TResult">The type of the converted result.</typeparam>
        /// <param name="source">The <see cref="IEnumerable{T}" /> source to parse and convert using the function delegate <paramref name="converter"/>.</param>
        /// <param name="converter">The function delegate that converts <typeparamref name="TSource" /> to a <typeparamref name="TResult" /> representation once per iteration.</param>
        /// <param name="arg1">The first parameter of the function delegate <paramref name="converter"/>.</param>
        /// <param name="arg2">The second parameter of the function delegate <paramref name="converter"/>.</param>
        /// <param name="arg3">The third parameter of the function delegate <paramref name="converter"/>.</param>
        /// <param name="arg4">The fourth parameter of the function delegate <paramref name="converter"/>.</param>
        /// <param name="arg5">The fifth parameter of the function delegate <paramref name="converter"/>.</param>
        /// <returns>An <see cref="IEnumerable{TResult}" /> that is equivalent to the <typeparamref name="TSource"/> contained in <paramref name="source"/>.</returns>
        /// <exception cref="System.ArgumentNullException">
        /// <paramref name="source"/> is null - or - <paramref name="converter"/> is null.
        /// </exception>
        public static IEnumerable<TResult> Parse<TSource, T1, T2, T3, T4, T5, TResult>(IEnumerable<TSource> source, Doer<TSource, T1, T2, T3, T4, T5, TResult> converter, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5)
        {
            Validator.ThrowIfNull(source, nameof(source));
            Validator.ThrowIfNull(converter, nameof(converter));
            var factory = DoerFactory.Create(converter, default(TSource), arg1, arg2, arg3, arg4, arg5);
            return ParseCore(factory, source);
        }

        /// <summary>
        /// Converts the specified <paramref name="source"/> to a <typeparamref name="TResult"/> representation using the specified <paramref name="converter"/>.
        /// </summary>
        /// <typeparam name="TSource">The type of the <paramref name="source"/> to convert.</typeparam>
        /// <typeparam name="T1">The type of the first parameter of the function delegate <paramref name="converter"/>.</typeparam>
        /// <typeparam name="T2">The type of the second parameter of the function delegate <paramref name="converter"/>.</typeparam>
        /// <typeparam name="T3">The type of the third parameter of the function delegate <paramref name="converter"/>.</typeparam>
        /// <typeparam name="T4">The type of the fourth parameter of the function delegate <paramref name="converter"/>.</typeparam>
        /// <typeparam name="T5">The type of the fifth parameter of the function delegate <paramref name="converter"/>.</typeparam>
        /// <typeparam name="T6">The type of the sixth parameter of the function delegate <paramref name="converter"/>.</typeparam>
        /// <typeparam name="TResult">The type of the converted result.</typeparam>
        /// <param name="source">The <see cref="IEnumerable{T}" /> source to parse and convert using the function delegate <paramref name="converter"/>.</param>
        /// <param name="converter">The function delegate that converts <typeparamref name="TSource" /> to a <typeparamref name="TResult" /> representation once per iteration.</param>
        /// <param name="arg1">The first parameter of the function delegate <paramref name="converter"/>.</param>
        /// <param name="arg2">The second parameter of the function delegate <paramref name="converter"/>.</param>
        /// <param name="arg3">The third parameter of the function delegate <paramref name="converter"/>.</param>
        /// <param name="arg4">The fourth parameter of the function delegate <paramref name="converter"/>.</param>
        /// <param name="arg5">The fifth parameter of the function delegate <paramref name="converter"/>.</param>
        /// <param name="arg6">The sixth parameter of the function delegate <paramref name="converter"/>.</param>
        /// <returns>An <see cref="IEnumerable{TResult}" /> that is equivalent to the <typeparamref name="TSource"/> contained in <paramref name="source"/>.</returns>
        /// <exception cref="System.ArgumentNullException">
        /// <paramref name="source"/> is null - or - <paramref name="converter"/> is null.
        /// </exception>
        public static IEnumerable<TResult> Parse<TSource, T1, T2, T3, T4, T5, T6, TResult>(IEnumerable<TSource> source, Doer<TSource, T1, T2, T3, T4, T5, T6, TResult> converter, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6)
        {
            Validator.ThrowIfNull(source, nameof(source));
            Validator.ThrowIfNull(converter, nameof(converter));
            var factory = DoerFactory.Create(converter, default(TSource), arg1, arg2, arg3, arg4, arg5, arg6);
            return ParseCore(factory, source);
        }

        /// <summary>
        /// Converts the specified <paramref name="source"/> to a <typeparamref name="TResult"/> representation using the specified <paramref name="converter"/>.
        /// </summary>
        /// <typeparam name="TSource">The type of the <paramref name="source"/> to convert.</typeparam>
        /// <typeparam name="T1">The type of the first parameter of the function delegate <paramref name="converter"/>.</typeparam>
        /// <typeparam name="T2">The type of the second parameter of the function delegate <paramref name="converter"/>.</typeparam>
        /// <typeparam name="T3">The type of the third parameter of the function delegate <paramref name="converter"/>.</typeparam>
        /// <typeparam name="T4">The type of the fourth parameter of the function delegate <paramref name="converter"/>.</typeparam>
        /// <typeparam name="T5">The type of the fifth parameter of the function delegate <paramref name="converter"/>.</typeparam>
        /// <typeparam name="T6">The type of the sixth parameter of the function delegate <paramref name="converter"/>.</typeparam>
        /// <typeparam name="T7">The type of the seventh parameter of the function delegate <paramref name="converter"/>.</typeparam>
        /// <typeparam name="TResult">The type of the converted result.</typeparam>
        /// <param name="source">The <see cref="IEnumerable{T}" /> source to parse and convert using the function delegate <paramref name="converter"/>.</param>
        /// <param name="converter">The function delegate that converts <typeparamref name="TSource" /> to a <typeparamref name="TResult" /> representation once per iteration.</param>
        /// <param name="arg1">The first parameter of the function delegate <paramref name="converter"/>.</param>
        /// <param name="arg2">The second parameter of the function delegate <paramref name="converter"/>.</param>
        /// <param name="arg3">The third parameter of the function delegate <paramref name="converter"/>.</param>
        /// <param name="arg4">The fourth parameter of the function delegate <paramref name="converter"/>.</param>
        /// <param name="arg5">The fifth parameter of the function delegate <paramref name="converter"/>.</param>
        /// <param name="arg6">The sixth parameter of the function delegate <paramref name="converter"/>.</param>
        /// <param name="arg7">The seventh parameter of the function delegate <paramref name="converter"/>.</param>
        /// <returns>An <see cref="IEnumerable{TResult}" /> that is equivalent to the <typeparamref name="TSource"/> contained in <paramref name="source"/>.</returns>
        /// <exception cref="System.ArgumentNullException">
        /// <paramref name="source"/> is null - or - <paramref name="converter"/> is null.
        /// </exception>
        public static IEnumerable<TResult> Parse<TSource, T1, T2, T3, T4, T5, T6, T7, TResult>(IEnumerable<TSource> source, Doer<TSource, T1, T2, T3, T4, T5, T6, T7, TResult> converter, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7)
        {
            Validator.ThrowIfNull(source, nameof(source));
            Validator.ThrowIfNull(converter, nameof(converter));
            var factory = DoerFactory.Create(converter, default(TSource), arg1, arg2, arg3, arg4, arg5, arg6, arg7);
            return ParseCore(factory, source);
        }

        /// <summary>
        /// Converts the specified <paramref name="source"/> to a <typeparamref name="TResult"/> representation using the specified <paramref name="converter"/>.
        /// </summary>
        /// <typeparam name="TSource">The type of the <paramref name="source"/> to convert.</typeparam>
        /// <typeparam name="T1">The type of the first parameter of the function delegate <paramref name="converter"/>.</typeparam>
        /// <typeparam name="T2">The type of the second parameter of the function delegate <paramref name="converter"/>.</typeparam>
        /// <typeparam name="T3">The type of the third parameter of the function delegate <paramref name="converter"/>.</typeparam>
        /// <typeparam name="T4">The type of the fourth parameter of the function delegate <paramref name="converter"/>.</typeparam>
        /// <typeparam name="T5">The type of the fifth parameter of the function delegate <paramref name="converter"/>.</typeparam>
        /// <typeparam name="T6">The type of the sixth parameter of the function delegate <paramref name="converter"/>.</typeparam>
        /// <typeparam name="T7">The type of the seventh parameter of the function delegate <paramref name="converter"/>.</typeparam>
        /// <typeparam name="T8">The type of the eighth parameter of the function delegate <paramref name="converter"/>.</typeparam>
        /// <typeparam name="TResult">The type of the converted result.</typeparam>
        /// <param name="source">The <see cref="IEnumerable{T}" /> source to parse and convert using the function delegate <paramref name="converter"/>.</param>
        /// <param name="converter">The function delegate that converts <typeparamref name="TSource" /> to a <typeparamref name="TResult" /> representation once per iteration.</param>
        /// <param name="arg1">The first parameter of the function delegate <paramref name="converter"/>.</param>
        /// <param name="arg2">The second parameter of the function delegate <paramref name="converter"/>.</param>
        /// <param name="arg3">The third parameter of the function delegate <paramref name="converter"/>.</param>
        /// <param name="arg4">The fourth parameter of the function delegate <paramref name="converter"/>.</param>
        /// <param name="arg5">The fifth parameter of the function delegate <paramref name="converter"/>.</param>
        /// <param name="arg6">The sixth parameter of the function delegate <paramref name="converter"/>.</param>
        /// <param name="arg7">The seventh parameter of the function delegate <paramref name="converter"/>.</param>
        /// <param name="arg8">The eighth parameter of the function delegate <paramref name="converter"/>.</param>
        /// <returns>An <see cref="IEnumerable{TResult}" /> that is equivalent to the <typeparamref name="TSource"/> contained in <paramref name="source"/>.</returns>
        /// <exception cref="System.ArgumentNullException">
        /// <paramref name="source"/> is null - or - <paramref name="converter"/> is null.
        /// </exception>
        public static IEnumerable<TResult> Parse<TSource, T1, T2, T3, T4, T5, T6, T7, T8, TResult>(IEnumerable<TSource> source, Doer<TSource, T1, T2, T3, T4, T5, T6, T7, T8, TResult> converter, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8)
        {
            Validator.ThrowIfNull(source, nameof(source));
            Validator.ThrowIfNull(converter, nameof(converter));
            var factory = DoerFactory.Create(converter, default(TSource), arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8);
            return ParseCore(factory, source);
        }

        /// <summary>
        /// Converts the specified <paramref name="source"/> to a <typeparamref name="TResult"/> representation using the specified <paramref name="converter"/>.
        /// </summary>
        /// <typeparam name="TSource">The type of the <paramref name="source"/> to convert.</typeparam>
        /// <typeparam name="T1">The type of the first parameter of the function delegate <paramref name="converter"/>.</typeparam>
        /// <typeparam name="T2">The type of the second parameter of the function delegate <paramref name="converter"/>.</typeparam>
        /// <typeparam name="T3">The type of the third parameter of the function delegate <paramref name="converter"/>.</typeparam>
        /// <typeparam name="T4">The type of the fourth parameter of the function delegate <paramref name="converter"/>.</typeparam>
        /// <typeparam name="T5">The type of the fifth parameter of the function delegate <paramref name="converter"/>.</typeparam>
        /// <typeparam name="T6">The type of the sixth parameter of the function delegate <paramref name="converter"/>.</typeparam>
        /// <typeparam name="T7">The type of the seventh parameter of the function delegate <paramref name="converter"/>.</typeparam>
        /// <typeparam name="T8">The type of the eighth parameter of the function delegate <paramref name="converter"/>.</typeparam>
        /// <typeparam name="T9">The type of the ninth parameter of the function delegate <paramref name="converter"/>.</typeparam>
        /// <typeparam name="TResult">The type of the converted result.</typeparam>
        /// <param name="source">The <see cref="IEnumerable{T}" /> source to parse and convert using the function delegate <paramref name="converter"/>.</param>
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
        /// <returns>An <see cref="IEnumerable{TResult}" /> that is equivalent to the <typeparamref name="TSource"/> contained in <paramref name="source"/>.</returns>
        /// <exception cref="System.ArgumentNullException">
        /// <paramref name="source"/> is null - or - <paramref name="converter"/> is null.
        /// </exception>
        public static IEnumerable<TResult> Parse<TSource, T1, T2, T3, T4, T5, T6, T7, T8, T9, TResult>(IEnumerable<TSource> source, Doer<TSource, T1, T2, T3, T4, T5, T6, T7, T8, T9, TResult> converter, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8, T9 arg9)
        {
            Validator.ThrowIfNull(source, nameof(source));
            Validator.ThrowIfNull(converter, nameof(converter));
            var factory = DoerFactory.Create(converter, default(TSource), arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9);
            return ParseCore(factory, source);
        }

        private static IEnumerable<TResult> ParseCore<TTuple, TSource, TResult>(DoerFactory<TTuple, TResult> factory, IEnumerable<TSource> source) where TTuple : Template<TSource>
        {
            foreach (TSource obj in source)
            {
                yield return Converter.ParseCore(factory, obj);
            }
        }
    }
}