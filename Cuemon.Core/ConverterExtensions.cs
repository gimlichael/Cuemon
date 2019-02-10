using System;

namespace Cuemon
{
    /// <summary>
    /// This is an extension implementation of the <see cref="Converter"/> class.
    /// </summary>
    public static class ConverterExtensions
    {
        /// <summary>
        /// Converts the specified <paramref name="source"/> to a <typeparamref name="TResult"/> representation using the specified <paramref name="converter"/>.
        /// </summary>
        /// <typeparam name="TSource">The type of the <paramref name="source"/> to convert.</typeparam>
        /// <typeparam name="TResult">The type of the converted result.</typeparam>
        /// <param name="source">The source to parse and convert using the function delegate <paramref name="converter"/>.</param>
        /// <param name="converter">The function delegate that converts <typeparamref name="TSource"/> to a <typeparamref name="TResult"/> representation.</param>
        /// <returns>A <typeparamref name="TResult"/> that is equivalent to the <typeparamref name="TSource"/> contained in <paramref name="source"/>.</returns>
        /// <exception cref="System.ArgumentNullException">
        /// <paramref name="converter"/> is null.
        /// </exception>
        public static TResult ParseWith<TSource, TResult>(this TSource source, Func<TSource, TResult> converter)
        {
            return Converter.Parse(source, converter);
        }

        /// <summary>
        /// Converts the specified <paramref name="source"/> to a <typeparamref name="TResult"/> representation using the specified <paramref name="converter"/>.
        /// </summary>
        /// <typeparam name="TSource">The type of the <paramref name="source"/> to convert.</typeparam>
        /// <typeparam name="T">The type of the parameter of the function delegate <paramref name="converter"/>.</typeparam>
        /// <typeparam name="TResult">The type of the converted result.</typeparam>
        /// <param name="source">The source to parse and convert using the function delegate <paramref name="converter"/>.</param>
        /// <param name="converter">The function delegate that converts <typeparamref name="TSource"/> to a <typeparamref name="TResult"/> representation.</param>
        /// <param name="arg">The parameter of the function delegate <paramref name="converter"/>.</param>
        /// <returns>A <typeparamref name="TResult"/> that is equivalent to the <typeparamref name="TSource"/> contained in <paramref name="source"/>.</returns>
        /// <exception cref="System.ArgumentNullException">
        /// <paramref name="converter"/> is null.
        /// </exception>
        public static TResult ParseWith<TSource, T, TResult>(this TSource source, Func<TSource, T, TResult> converter, T arg)
        {
            return Converter.Parse(source, converter, arg);
        }

        /// <summary>
        /// Converts the specified <paramref name="source"/> to a <typeparamref name="TResult"/> representation using the specified <paramref name="converter"/>.
        /// </summary>
        /// <typeparam name="TSource">The type of the <paramref name="source"/> to convert.</typeparam>
        /// <typeparam name="T1">The type of the first parameter of the function delegate <paramref name="converter"/>.</typeparam>
        /// <typeparam name="T2">The type of the second parameter of the function delegate <paramref name="converter"/>.</typeparam>
        /// <typeparam name="TResult">The type of the converted result.</typeparam>
        /// <param name="source">The source to parse and convert using the function delegate <paramref name="converter"/>.</param>
        /// <param name="converter">The function delegate that converts <typeparamref name="TSource"/> to a <typeparamref name="TResult"/> representation.</param>
        /// <param name="arg1">The first parameter of the function delegate <paramref name="converter"/>.</param>
        /// <param name="arg2">The second parameter of the function delegate <paramref name="converter"/>.</param>
        /// <returns>A <typeparamref name="TResult"/> that is equivalent to the <typeparamref name="TSource"/> contained in <paramref name="source"/>.</returns>
        /// <exception cref="System.ArgumentNullException">
        /// <paramref name="converter"/> is null.
        /// </exception>
        public static TResult ParseWith<TSource, T1, T2, TResult>(this TSource source, Func<TSource, T1, T2, TResult> converter, T1 arg1, T2 arg2)
        {
            return Converter.Parse(source, converter, arg1, arg2);
        }

        /// <summary>
        /// Converts the specified <paramref name="source"/> to a <typeparamref name="TResult"/> representation using the specified <paramref name="converter"/>.
        /// </summary>
        /// <typeparam name="TSource">The type of the <paramref name="source"/> to convert.</typeparam>
        /// <typeparam name="T1">The type of the first parameter of the function delegate <paramref name="converter"/>.</typeparam>
        /// <typeparam name="T2">The type of the second parameter of the function delegate <paramref name="converter"/>.</typeparam>
        /// <typeparam name="T3">The type of the third parameter of the function delegate <paramref name="converter"/>.</typeparam>
        /// <typeparam name="TResult">The type of the converted result.</typeparam>
        /// <param name="source">The source to parse and convert using the function delegate <paramref name="converter"/>.</param>
        /// <param name="converter">The function delegate that converts <typeparamref name="TSource"/> to a <typeparamref name="TResult"/> representation.</param>
        /// <param name="arg1">The first parameter of the function delegate <paramref name="converter"/>.</param>
        /// <param name="arg2">The second parameter of the function delegate <paramref name="converter"/>.</param>
        /// <param name="arg3">The third parameter of the function delegate <paramref name="converter"/>.</param>
        /// <returns>A <typeparamref name="TResult"/> that is equivalent to the <typeparamref name="TSource"/> contained in <paramref name="source"/>.</returns>
        /// <exception cref="System.ArgumentNullException">
        /// <paramref name="converter"/> is null.
        /// </exception>
        public static TResult ParseWith<TSource, T1, T2, T3, TResult>(this TSource source, Func<TSource, T1, T2, T3, TResult> converter, T1 arg1, T2 arg2, T3 arg3)
        {
            return Converter.Parse(source, converter, arg1, arg2, arg3);
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
        /// <param name="source">The source to parse and convert using the function delegate <paramref name="converter"/>.</param>
        /// <param name="converter">The function delegate that converts <typeparamref name="TSource"/> to a <typeparamref name="TResult"/> representation.</param>
        /// <param name="arg1">The first parameter of the function delegate <paramref name="converter"/>.</param>
        /// <param name="arg2">The second parameter of the function delegate <paramref name="converter"/>.</param>
        /// <param name="arg3">The third parameter of the function delegate <paramref name="converter"/>.</param>
        /// <param name="arg4">The fourth parameter of the function delegate <paramref name="converter"/>.</param>
        /// <returns>A <typeparamref name="TResult"/> that is equivalent to the <typeparamref name="TSource"/> contained in <paramref name="source"/>.</returns>
        /// <exception cref="System.ArgumentNullException">
        /// <paramref name="converter"/> is null.
        /// </exception>
        public static TResult ParseWith<TSource, T1, T2, T3, T4, TResult>(this TSource source, Func<TSource, T1, T2, T3, T4, TResult> converter, T1 arg1, T2 arg2, T3 arg3, T4 arg4)
        {
            return Converter.Parse(source, converter, arg1, arg2, arg3, arg4);
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
        /// <param name="source">The source to parse and convert using the function delegate <paramref name="converter"/>.</param>
        /// <param name="converter">The function delegate that converts <typeparamref name="TSource"/> to a <typeparamref name="TResult"/> representation.</param>
        /// <param name="arg1">The first parameter of the function delegate <paramref name="converter"/>.</param>
        /// <param name="arg2">The second parameter of the function delegate <paramref name="converter"/>.</param>
        /// <param name="arg3">The third parameter of the function delegate <paramref name="converter"/>.</param>
        /// <param name="arg4">The fourth parameter of the function delegate <paramref name="converter"/>.</param>
        /// <param name="arg5">The fifth parameter of the function delegate <paramref name="converter"/>.</param>
        /// <returns>A <typeparamref name="TResult"/> that is equivalent to the <typeparamref name="TSource"/> contained in <paramref name="source"/>.</returns>
        /// <exception cref="System.ArgumentNullException">
        /// <paramref name="converter"/> is null.
        /// </exception>
        public static TResult ParseWith<TSource, T1, T2, T3, T4, T5, TResult>(this TSource source, Func<TSource, T1, T2, T3, T4, T5, TResult> converter, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5)
        {
            return Converter.Parse(source, converter, arg1, arg2, arg3, arg4, arg5);
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
        /// <param name="source">The source to parse and convert using the function delegate <paramref name="converter"/>.</param>
        /// <param name="converter">The function delegate that converts <typeparamref name="TSource"/> to a <typeparamref name="TResult"/> representation.</param>
        /// <param name="arg1">The first parameter of the function delegate <paramref name="converter"/>.</param>
        /// <param name="arg2">The second parameter of the function delegate <paramref name="converter"/>.</param>
        /// <param name="arg3">The third parameter of the function delegate <paramref name="converter"/>.</param>
        /// <param name="arg4">The fourth parameter of the function delegate <paramref name="converter"/>.</param>
        /// <param name="arg5">The fifth parameter of the function delegate <paramref name="converter"/>.</param>
        /// <param name="arg6">The sixth parameter of the function delegate <paramref name="converter"/>.</param>
        /// <returns>A <typeparamref name="TResult"/> that is equivalent to the <typeparamref name="TSource"/> contained in <paramref name="source"/>.</returns>
        /// <exception cref="System.ArgumentNullException">
        /// <paramref name="converter"/> is null.
        /// </exception>
        public static TResult ParseWith<TSource, T1, T2, T3, T4, T5, T6, TResult>(this TSource source, Func<TSource, T1, T2, T3, T4, T5, T6, TResult> converter, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6)
        {
            return Converter.Parse(source, converter, arg1, arg2, arg3, arg4, arg5, arg6);
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
        /// <param name="source">The source to parse and convert using the function delegate <paramref name="converter"/>.</param>
        /// <param name="converter">The function delegate that converts <typeparamref name="TSource"/> to a <typeparamref name="TResult"/> representation.</param>
        /// <param name="arg1">The first parameter of the function delegate <paramref name="converter"/>.</param>
        /// <param name="arg2">The second parameter of the function delegate <paramref name="converter"/>.</param>
        /// <param name="arg3">The third parameter of the function delegate <paramref name="converter"/>.</param>
        /// <param name="arg4">The fourth parameter of the function delegate <paramref name="converter"/>.</param>
        /// <param name="arg5">The fifth parameter of the function delegate <paramref name="converter"/>.</param>
        /// <param name="arg6">The sixth parameter of the function delegate <paramref name="converter"/>.</param>
        /// <param name="arg7">The seventh parameter of the function delegate <paramref name="converter"/>.</param>
        /// <returns>A <typeparamref name="TResult"/> that is equivalent to the <typeparamref name="TSource"/> contained in <paramref name="source"/>.</returns>
        /// <exception cref="System.ArgumentNullException">
        /// <paramref name="converter"/> is null.
        /// </exception>
        public static TResult ParseWith<TSource, T1, T2, T3, T4, T5, T6, T7, TResult>(this TSource source, Func<TSource, T1, T2, T3, T4, T5, T6, T7, TResult> converter, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7)
        {
            return Converter.Parse(source, converter, arg1, arg2, arg3, arg4, arg5, arg6, arg7);
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
        /// <param name="source">The source to parse and convert using the function delegate <paramref name="converter"/>.</param>
        /// <param name="converter">The function delegate that converts <typeparamref name="TSource"/> to a <typeparamref name="TResult"/> representation.</param>
        /// <param name="arg1">The first parameter of the function delegate <paramref name="converter"/>.</param>
        /// <param name="arg2">The second parameter of the function delegate <paramref name="converter"/>.</param>
        /// <param name="arg3">The third parameter of the function delegate <paramref name="converter"/>.</param>
        /// <param name="arg4">The fourth parameter of the function delegate <paramref name="converter"/>.</param>
        /// <param name="arg5">The fifth parameter of the function delegate <paramref name="converter"/>.</param>
        /// <param name="arg6">The sixth parameter of the function delegate <paramref name="converter"/>.</param>
        /// <param name="arg7">The seventh parameter of the function delegate <paramref name="converter"/>.</param>
        /// <param name="arg8">The eighth parameter of the function delegate <paramref name="converter"/>.</param>
        /// <returns>A <typeparamref name="TResult"/> that is equivalent to the <typeparamref name="TSource"/> contained in <paramref name="source"/>.</returns>
        /// <exception cref="System.ArgumentNullException">
        /// <paramref name="converter"/> is null.
        /// </exception>
        public static TResult ParseWith<TSource, T1, T2, T3, T4, T5, T6, T7, T8, TResult>(this TSource source, Func<TSource, T1, T2, T3, T4, T5, T6, T7, T8, TResult> converter, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8)
        {
            return Converter.Parse(source, converter, arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8);
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
        /// <param name="source">The source to parse and convert using the function delegate <paramref name="converter"/>.</param>
        /// <param name="converter">The function delegate that converts <typeparamref name="TSource"/> to a <typeparamref name="TResult"/> representation.</param>
        /// <param name="arg1">The first parameter of the function delegate <paramref name="converter"/>.</param>
        /// <param name="arg2">The second parameter of the function delegate <paramref name="converter"/>.</param>
        /// <param name="arg3">The third parameter of the function delegate <paramref name="converter"/>.</param>
        /// <param name="arg4">The fourth parameter of the function delegate <paramref name="converter"/>.</param>
        /// <param name="arg5">The fifth parameter of the function delegate <paramref name="converter"/>.</param>
        /// <param name="arg6">The sixth parameter of the function delegate <paramref name="converter"/>.</param>
        /// <param name="arg7">The seventh parameter of the function delegate <paramref name="converter"/>.</param>
        /// <param name="arg8">The eighth parameter of the function delegate <paramref name="converter"/>.</param>
        /// <param name="arg9">The ninth parameter of the function delegate <paramref name="converter"/>.</param>
        /// <returns>A <typeparamref name="TResult"/> that is equivalent to the <typeparamref name="TSource"/> contained in <paramref name="source"/>.</returns>
        /// <exception cref="System.ArgumentNullException">
        /// <paramref name="converter"/> is null.
        /// </exception>
        public static TResult ParseWith<TSource, T1, T2, T3, T4, T5, T6, T7, T8, T9, TResult>(this TSource source, Func<TSource, T1, T2, T3, T4, T5, T6, T7, T8, T9, TResult> converter, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8, T9 arg9)
        {
            return Converter.Parse(source, converter, arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9);
        }

        /// <summary>
        /// Attempts to converts the specified <paramref name="value"/> to a given type. If the conversion is not possible the result is set to <b>default(TResult)</b>.
        /// </summary>
        /// <typeparam name="TResult">The type of the result.</typeparam>
        /// <param name="value">The object to convert the underlying type.</param>
        /// <returns>The <paramref name="value"/> converted to the specified <typeparamref name="TResult"/>.</returns>
        public static TResult As<TResult>(this object value)
        {
            return Converter.FromObject<TResult>(value);
        }

        /// <summary>
        /// Attempts to converts the specified <paramref name="value"/> to a given type. If the conversion is not possible the result is set to <paramref name="resultOnConversionNotPossible"/>.
        /// </summary>
        /// <typeparam name="TResult">The type of the result.</typeparam>
        /// <param name="value">The object to convert the underlying type.</param>
        /// <param name="resultOnConversionNotPossible">The value to return if the conversion is not possible.</param>
        /// <returns>The <paramref name="value"/> converted to the specified <typeparamref name="TResult"/>.</returns>
        public static TResult As<TResult>(this object value, TResult resultOnConversionNotPossible)
        {
            return Converter.FromObject(value, resultOnConversionNotPossible);
        }

        /// <summary>
        /// Attempts to converts the specified <paramref name="value"/> to a given type. If the conversion is not possible the result is set to <paramref name="resultOnConversionNotPossible"/>.
        /// </summary>
        /// <typeparam name="TResult">The type of the result.</typeparam>
        /// <param name="value">The object to convert the underlying type.</param>
        /// <param name="resultOnConversionNotPossible">The value to return if the conversion is not possible.</param>
        /// <param name="provider">An object that supplies culture-specific formatting information.</param>
        /// <returns>The <paramref name="value"/> converted to the specified <typeparamref name="TResult"/>.</returns>
        public static TResult As<TResult>(this object value, TResult resultOnConversionNotPossible, IFormatProvider provider)
        {
            return Converter.FromObject(value, resultOnConversionNotPossible, provider);
        }
    }
}