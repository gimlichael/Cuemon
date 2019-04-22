using System;
using System.ComponentModel;
using System.Globalization;
using Cuemon.Collections.Generic;
using Cuemon.Reflection;

namespace Cuemon
{
    /// <summary>
    /// This utility class is designed to make generic conversions easier to work with.
    /// </summary>
    public static class Converter
    {
        /// <summary>
        /// Converts the specified string to its <typeparamref name="T"/> equivalent.
        /// </summary>
        /// <typeparam name="T">The type of the expected return <paramref name="value"/> after conversion.</typeparam>
        /// <param name="value">The string value to convert.</param>
        /// <returns>An object that is equivalent to <typeparamref name="T"/> contained in <paramref name="value"/>.</returns>
        /// <exception cref="ArgumentException">
        /// Invalid <paramref name="value"/> for <typeparamref name="T"/> specified.
        /// </exception>
        /// <exception cref="NotSupportedException">
        /// The conversion cannot be performed.
        /// </exception>
        public static T FromString<T>(string value)
        {
            return FromString<T>(value, CultureInfo.InvariantCulture);
        }

        /// <summary>
        /// Converts the specified string to its <typeparamref name="T"/> equivalent using the specified <paramref name="culture"/> information.
        /// </summary>
        /// <typeparam name="T">The type of the expected return <paramref name="value"/> after conversion.</typeparam>
        /// <param name="value">The string value to convert.</param>
        /// <param name="culture">The culture-specific formatting information about <paramref name="value"/>.</param>
        /// <returns>An object that is equivalent to <typeparamref name="T"/> contained in <paramref name="value"/>, as specified by <paramref name="culture"/>.</returns>
        /// <exception cref="ArgumentException">
        /// Invalid <paramref name="value"/> for <typeparamref name="T"/> specified.
        /// </exception>
        /// <exception cref="NotSupportedException">
        /// The conversion cannot be performed.
        /// </exception>
        public static T FromString<T>(string value, CultureInfo culture)
        {
            return FromString<T>(value, culture, null);
        }

        /// <summary>
        /// Converts the specified string to its <typeparamref name="T"/> equivalent using the specified <paramref name="context"/> and <paramref name="culture"/> information.
        /// </summary>
        /// <typeparam name="T">The type of the expected return <paramref name="value"/> after conversion.</typeparam>
        /// <param name="value">The string value to convert.</param>
        /// <param name="culture">The culture-specific formatting information about <paramref name="value"/>.</param>
        /// <param name="context">The type-specific formatting information about <paramref name="value"/>.</param>
        /// <returns>An object that is equivalent to <typeparamref name="T"/> contained in <paramref name="value"/>, as specified by <paramref name="culture"/> and <paramref name="context"/>.</returns>
        /// <exception cref="ArgumentException">
        /// Invalid <paramref name="value"/> for <typeparamref name="T"/> specified.
        /// </exception>
        /// <exception cref="NotSupportedException">
        /// The conversion cannot be performed.
        /// </exception>
        public static T FromString<T>(string value, CultureInfo culture, ITypeDescriptorContext context)
        {
            try
            {
                var resultType = typeof(T);
                var converter = TypeDescriptor.GetConverter(resultType);
                var result = (T)converter.ConvertFromString(context, culture, value);
                if (resultType == typeof(Uri)) // for reasons unknown to me, MS allows all sorts of string to be constructed on a Uri - check if valid (quick-fix until more knowledge of ITypeDescriptorContext)
                {
                    var resultAsUri = result as Uri;
                    var segments = resultAsUri?.Segments;
                }
                return result;
            }
            catch (Exception ex)
            {
                if (ex.GetType() == typeof(NotSupportedException)) { throw; }
                throw ExceptionUtility.Refine(new ArgumentException(nameof(value), ex.Message, ex.InnerException), MethodBaseConverter.FromType(typeof(Converter), EnumerableConverter.AsArray(typeof(string), typeof(CultureInfo), typeof(ITypeDescriptorContext))), value, culture, context).Unwrap();
            }
        }

        /// <summary>
        /// Converts the specified string to its <typeparamref name="T" /> equivalent.
        /// </summary>
        /// <typeparam name="T">The type of the expected return <paramref name="value" /> after conversion.</typeparam>
        /// <param name="value">The string value to convert.</param>
        /// <param name="result">When this method returns, contains the equivalent to <typeparamref name="T"/> of <paramref name="value"/>, or <b>default</b>(<typeparamref name="T"/>) if an exception is thrown.</param>
        /// <returns><c>true</c> if the <paramref name="value"/> parameter was successfully converted; otherwise <c>false</c>.</returns>
        public static bool TryFromString<T>(string value, out T result)
        {
            return TryFromString(value, CultureInfo.InvariantCulture, out result);
        }

        /// <summary>
        /// Converts the specified string to its <typeparamref name="T" /> equivalent using the specified <paramref name="culture" /> information.
        /// </summary>
        /// <typeparam name="T">The type of the expected return <paramref name="value" /> after conversion.</typeparam>
        /// <param name="value">The string value to convert.</param>
        /// <param name="culture">The culture-specific formatting information about <paramref name="value" />.</param>
        /// <param name="result">When this method returns, contains the equivalent to <typeparamref name="T"/> of <paramref name="value"/>, as specified by <paramref name="culture"/>, or <b>default</b>(<typeparamref name="T"/>) if an exception is thrown.</param>
        /// <returns><c>true</c> if the <paramref name="value"/> parameter was successfully converted; otherwise <c>false</c>.</returns>
        public static bool TryFromString<T>(string value, CultureInfo culture, out T result)
        {
            return TryFromString(value, culture, null, out result);
        }

        /// <summary>
        /// Converts the specified string to its <typeparamref name="T" /> equivalent using the specified <paramref name="context" /> and <paramref name="culture" /> information.
        /// </summary>
        /// <typeparam name="T">The type of the expected return <paramref name="value" /> after conversion.</typeparam>
        /// <param name="value">The string value to convert.</param>
        /// <param name="culture">The culture-specific formatting information about <paramref name="value" />.</param>
        /// <param name="context">The type-specific formatting information about <paramref name="value" />.</param>
        /// <param name="result">When this method returns, contains the equivalent to <typeparamref name="T"/> of <paramref name="value"/>, as specified by <paramref name="culture"/> and <paramref name="context"/>, or <b>default</b>(<typeparamref name="T"/>) if an exception is thrown.</param>
        /// <returns><c>true</c> if the <paramref name="value"/> parameter was successfully converted; otherwise <c>false</c>.</returns>
        public static bool TryFromString<T>(string value, CultureInfo culture, ITypeDescriptorContext context, out T result)
        {
            try
            {
                result = FromString<T>(value, culture, context);
                return true;
            }
            catch (Exception)
            {
                result = default(T);
                return false;
            }
        }

        /// <summary>
        /// Attempts to convert the specified <paramref name="value"/> to a given type. If the conversion is not possible the result is set to <b>default(TResult)</b>.
        /// </summary>
        /// <typeparam name="TResult">The type of the result.</typeparam>
        /// <param name="value">The object to convert the underlying type.</param>
        /// <returns>The <paramref name="value"/> converted to the specified <typeparamref name="TResult"/>.</returns>
        /// <remarks>This method first checks if <paramref name="value"/> is compatible with <typeparamref name="TResult"/>; if not compatible the method continues with <see cref="ObjectConverter.ChangeType(object,System.Type,IFormatProvider)"/> for the operation.</remarks>
        public static TResult FromObject<TResult>(object value)
        {
            return FromObject(value, default(TResult));
        }

        /// <summary>
        /// Attempts to convert the specified <paramref name="value"/> to a given type. If the conversion is not possible the result is set to <paramref name="resultOnConversionNotPossible"/>.
        /// </summary>
        /// <typeparam name="TResult">The type of the result.</typeparam>
        /// <param name="value">The object to convert the underlying type.</param>
        /// <param name="resultOnConversionNotPossible">The value to return if the conversion is not possible.</param>
        /// <returns>The <paramref name="value"/> converted to the specified <typeparamref name="TResult"/>.</returns>
        /// <remarks>This method first checks if <paramref name="value"/> is compatible with <typeparamref name="TResult"/>; if not compatible the method continues with <see cref="ObjectConverter.ChangeType(object,System.Type,IFormatProvider)"/> for the operation.</remarks>
        public static TResult FromObject<TResult>(object value, TResult resultOnConversionNotPossible)
        {
            return FromObject(value, resultOnConversionNotPossible, CultureInfo.InvariantCulture);
        }

        /// <summary>
        /// Attempts to convert the specified <paramref name="value"/> to a given type. If the conversion is not possible the result is set to <paramref name="resultOnConversionNotPossible"/>.
        /// </summary>
        /// <typeparam name="TResult">The type of the result.</typeparam>
        /// <param name="value">The object to convert the underlying type.</param>
        /// <param name="resultOnConversionNotPossible">The value to return if the conversion is not possible.</param>
        /// <param name="provider">An object that supplies culture-specific formatting information.</param>
        /// <returns>The <paramref name="value"/> converted to the specified <typeparamref name="TResult"/>.</returns>
        /// <remarks>This method first checks if <paramref name="value"/> is compatible with <typeparamref name="TResult"/>; if not compatible the method continues with <see cref="ObjectConverter.ChangeType(object,System.Type,IFormatProvider)"/> for the operation.</remarks>
        public static TResult FromObject<TResult>(object value, TResult resultOnConversionNotPossible, IFormatProvider provider)
        {
            if (value is TResult) { return (TResult)value; }
            object o;
            var success = Patterns.TryParse(() => ObjectConverter.ChangeType(value, typeof(TResult), provider), out o);
            return success ? (TResult)o : resultOnConversionNotPossible;
        }

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
        public static TResult Parse<TSource, TResult>(TSource source, Func<TSource, TResult> converter)
        {
            Validator.ThrowIfNull(converter, nameof(converter));
            var factory = FuncFactory.Create(converter, default(TSource));
            return ParseCore(factory, source);
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
        public static TResult Parse<TSource, T, TResult>(TSource source, Func<TSource, T, TResult> converter, T arg)
        {
            Validator.ThrowIfNull(converter, nameof(converter));
            var factory = FuncFactory.Create(converter, default(TSource), arg);
            return ParseCore(factory, source);
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
        public static TResult Parse<TSource, T1, T2, TResult>(TSource source, Func<TSource, T1, T2, TResult> converter, T1 arg1, T2 arg2)
        {
            Validator.ThrowIfNull(converter, nameof(converter));
            var factory = FuncFactory.Create(converter, default(TSource), arg1, arg2);
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
        /// <param name="source">The source to parse and convert using the function delegate <paramref name="converter"/>.</param>
        /// <param name="converter">The function delegate that converts <typeparamref name="TSource"/> to a <typeparamref name="TResult"/> representation.</param>
        /// <param name="arg1">The first parameter of the function delegate <paramref name="converter"/>.</param>
        /// <param name="arg2">The second parameter of the function delegate <paramref name="converter"/>.</param>
        /// <param name="arg3">The third parameter of the function delegate <paramref name="converter"/>.</param>
        /// <returns>A <typeparamref name="TResult"/> that is equivalent to the <typeparamref name="TSource"/> contained in <paramref name="source"/>.</returns>
        /// <exception cref="System.ArgumentNullException">
        /// <paramref name="converter"/> is null.
        /// </exception>
        public static TResult Parse<TSource, T1, T2, T3, TResult>(TSource source, Func<TSource, T1, T2, T3, TResult> converter, T1 arg1, T2 arg2, T3 arg3)
        {
            Validator.ThrowIfNull(converter, nameof(converter));
            var factory = FuncFactory.Create(converter, default(TSource), arg1, arg2, arg3);
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
        public static TResult Parse<TSource, T1, T2, T3, T4, TResult>(TSource source, Func<TSource, T1, T2, T3, T4, TResult> converter, T1 arg1, T2 arg2, T3 arg3, T4 arg4)
        {
            Validator.ThrowIfNull(converter, nameof(converter));
            var factory = FuncFactory.Create(converter, default(TSource), arg1, arg2, arg3, arg4);
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
        public static TResult Parse<TSource, T1, T2, T3, T4, T5, TResult>(TSource source, Func<TSource, T1, T2, T3, T4, T5, TResult> converter, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5)
        {
            Validator.ThrowIfNull(converter, nameof(converter));
            var factory = FuncFactory.Create(converter, default(TSource), arg1, arg2, arg3, arg4, arg5);
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
        public static TResult Parse<TSource, T1, T2, T3, T4, T5, T6, TResult>(TSource source, Func<TSource, T1, T2, T3, T4, T5, T6, TResult> converter, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6)
        {
            Validator.ThrowIfNull(converter, nameof(converter));
            var factory = FuncFactory.Create(converter, default(TSource), arg1, arg2, arg3, arg4, arg5, arg6);
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
        public static TResult Parse<TSource, T1, T2, T3, T4, T5, T6, T7, TResult>(TSource source, Func<TSource, T1, T2, T3, T4, T5, T6, T7, TResult> converter, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7)
        {
            Validator.ThrowIfNull(converter, nameof(converter));
            var factory = FuncFactory.Create(converter, default(TSource), arg1, arg2, arg3, arg4, arg5, arg6, arg7);
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
        public static TResult Parse<TSource, T1, T2, T3, T4, T5, T6, T7, T8, TResult>(TSource source, Func<TSource, T1, T2, T3, T4, T5, T6, T7, T8, TResult> converter, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8)
        {
            Validator.ThrowIfNull(converter, nameof(converter));
            var factory = FuncFactory.Create(converter, default(TSource), arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8);
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
        public static TResult Parse<TSource, T1, T2, T3, T4, T5, T6, T7, T8, T9, TResult>(TSource source, Func<TSource, T1, T2, T3, T4, T5, T6, T7, T8, T9, TResult> converter, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8, T9 arg9)
        {
            Validator.ThrowIfNull(converter, nameof(converter));
            var factory = FuncFactory.Create(converter, source, arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9);
            return ParseCore(factory, source);
        }

        internal static TResult ParseCore<TTuple, TSource, TResult>(FuncFactory<TTuple, TResult> factory, TSource source) where TTuple : Template<TSource>
        {
            factory.GenericArguments.Arg1 = source;
            return factory.ExecuteMethod();
        }
    }
}