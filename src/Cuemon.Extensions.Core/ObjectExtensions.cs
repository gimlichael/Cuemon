using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Cuemon.Extensions
{
    /// <summary>
    /// Extension methods for the <see cref="object"/> class.
    /// </summary>
    public static class ObjectExtensions
    {
        /// <summary>
        /// Wrap and extend an existing object of <typeparamref name="T"/> with additional data.
        /// </summary>
        /// <typeparam name="T">The type of the object to extend.</typeparam>
        /// <param name="instance">The instance to wrap and extend.</param>
        /// <param name="extender">The delegate that provides an easy way of supplying additional data to an object.</param>
        /// <returns>An implementation of <see cref="IWrapper{T}"/> encapsulating the specified <paramref name="instance"/>.</returns>
        public static IWrapper<T> UseWrapper<T>(this T instance, Action<IDictionary<string, object>> extender = null)
        {
            return UseWrapper(instance, null, extender);
        }

        /// <summary>
        /// Wrap and extend an existing object of <typeparamref name="T"/> with additional data.
        /// </summary>
        /// <typeparam name="T">The type of the object to extend.</typeparam>
        /// <param name="instance">The instance to wrap and extend.</param>
        /// <param name="memberReference">The optional member reference to assign <see cref="IWrapper{T}.MemberReference"/>.</param>
        /// <param name="extender">The delegate that provides an easy way of supplying additional data to an object.</param>
        /// <returns>An implementation of <see cref="IWrapper{T}"/> encapsulating the specified <paramref name="instance"/>.</returns>
        public static IWrapper<T> UseWrapper<T>(this T instance, MemberInfo memberReference, Action<IDictionary<string, object>> extender = null)
        {
            var wrapper = new Wrapper<T>(instance, memberReference);
            extender?.Invoke(wrapper.Data);
            return wrapper;
        }

        /// <summary>
        /// Attempts to converts the specified <paramref name="value"/> to a given type. If the conversion is not possible the result is set to <b>default(TResult)</b>.
        /// </summary>
        /// <typeparam name="TResult">The type of the result.</typeparam>
        /// <param name="value">The object to convert the underlying type.</param>
        /// <returns>The <paramref name="value"/> converted to the specified <typeparamref name="TResult"/>.</returns>
        public static TResult As<TResult>(this object value)
        {
            return ConvertFactory.FromObject().ChangeTypeOrDefault<TResult>(value);
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
            return ConvertFactory.FromObject().ChangeTypeOrDefault(value, resultOnConversionNotPossible);
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
            return ConvertFactory.FromObject().ChangeTypeOrDefault(value, resultOnConversionNotPossible, o => o.FormatProvider = provider);
        }

        /// <summary>
        /// Computes a suitable hash code from the specified sequence of <paramref name="convertibles"/>.
        /// </summary>
        /// <param name="convertibles">A sequence of objects implementing the <see cref="IConvertible"/> interface.</param>
        /// <returns>A 32-bit signed integer that is the hash code of <paramref name="convertibles"/>.</returns>
        public static int GetHashCode32<T>(this IEnumerable<T> convertibles) where T : IConvertible
        {
            return Generate.HashCode32(convertibles.Cast<IConvertible>());
        }

        /// <summary>
        /// Computes a suitable hash code from the specified sequence of <paramref name="convertibles"/>.
        /// </summary>
        /// <param name="convertibles">A sequence of objects implementing the <see cref="IConvertible"/> interface.</param>
        /// <returns>A 64-bit signed integer that is the hash code of <paramref name="convertibles"/>.</returns>
        public static long GetHashCode64<T>(this IEnumerable<T> convertibles) where T : struct, IConvertible
        {
            return Generate.HashCode64(convertibles.Cast<IConvertible>());
        }

        /// <summary>
        /// Converts the specified <paramref name="source"/> to a string of <paramref name="delimiter"/> delimited values.
        /// </summary>
        /// <typeparam name="T">The type of the elements of the sequence to convert.</typeparam>
        /// <param name="source">A sequence of elements to be converted.</param>
        /// <param name="delimiter">The delimiter specification.</param>
        /// <param name="converter">The function delegate that converts <typeparamref name="T"/> to a string representation once per iteration.</param>
        /// <returns>A <see cref="string"/> of delimited values from the by parameter specified delimiter.</returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="source"/> cannot be null -or-
        /// <paramref name="delimiter"/> cannot be null.
        /// </exception>
        /// <exception cref="ArgumentException">
        /// <paramref name="delimiter"/> cannot be empty or consist only of white-space characters.
        /// </exception>
        public static string ToDelimitedString<T>(this IEnumerable<T> source, string delimiter = ",", Func<T, string> converter = null)
        {
            Validator.ThrowIfNull(source, nameof(source));
            return Decorator.Enclose(source).ToDelimitedString(delimiter, converter);
        }

        /// <summary>
        /// Adjust the specified <paramref name="value"/> with the function delegate <paramref name="tweaker"/>.
        /// </summary>
        /// <typeparam name="T">The type of the value to adjust.</typeparam>
        /// <param name="value">The value to adjust.</param>
        /// <param name="tweaker">The function delegate that will adjust the specified <paramref name="value"/>.</param>
        /// <returns>The <paramref name="value"/> in its original or adjusted form.</returns>
        public static T Adjust<T>(this T value, Func<T, T> tweaker)
        {
            return Decorator.Enclose(value).Adjust(tweaker);
        }

        /// <summary>
        /// Determines whether the specified source is a nullable <see cref="ValueType"/>.
        /// </summary>
        /// <typeparam name="T">The type of the <paramref name="source"/> of <typeparamref name="T"/>.</typeparam>
        /// <param name="source">The source type to check for nullable <see cref="ValueType"/>.</param>
        /// <returns>
        ///   <c>true</c> if the specified source is nullable; otherwise, <c>false</c>.
        /// </returns>
        public static bool IsNullable<T>(this T source) { return false; }

        /// <summary>
        /// Determines whether the specified source is a nullable <see cref="ValueType"/>.
        /// </summary>
        /// <typeparam name="T">The type of the <paramref name="source"/> of <typeparamref name="T"/>.</typeparam>
        /// <param name="source">The source type to check for nullable <see cref="ValueType"/>.</param>
        /// <returns>
        ///   <c>true</c> if the specified source is nullable; otherwise, <c>false</c>.
        /// </returns>
        public static bool IsNullable<T>(this T? source) where T : struct { return true; }
    }
}