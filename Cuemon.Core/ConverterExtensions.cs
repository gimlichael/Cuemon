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
        /// <exception cref="ArgumentNullException">
        /// <paramref name="converter"/> is null.
        /// </exception>
        public static TResult ParseWith<TSource, TResult>(this TSource source, Func<TSource, TResult> converter)
        {
            return Converter.Parse(source, converter);
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