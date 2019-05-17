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
        /// Attempts to convert the specified <paramref name="value"/> to a given type. If the conversion is not possible the result is set to <b>default(TResult)</b>.
        /// </summary>
        /// <typeparam name="TResult">The type of the result.</typeparam>
        /// <param name="value">The object to convert the underlying type.</param>
        /// <returns>The <paramref name="value"/> converted to the specified <typeparamref name="TResult"/>.</returns>
        /// <remarks>This method first checks if <paramref name="value"/> is compatible with <typeparamref name="TResult"/>; if not compatible the method continues with <see cref="ObjectConverter.ChangeType(object,Type,IFormatProvider)"/> for the operation.</remarks>
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
        /// <remarks>This method first checks if <paramref name="value"/> is compatible with <typeparamref name="TResult"/>; if not compatible the method continues with <see cref="ObjectConverter.ChangeType(object,Type,IFormatProvider)"/> for the operation.</remarks>
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
        /// <remarks>This method first checks if <paramref name="value"/> is compatible with <typeparamref name="TResult"/>; if not compatible the method continues with <see cref="ObjectConverter.ChangeType(object,Type,IFormatProvider)"/> for the operation.</remarks>
        public static TResult FromObject<TResult>(object value, TResult resultOnConversionNotPossible, IFormatProvider provider)
        {
            if (value is TResult) { return (TResult)value; }
            object o;
            var success = Patterns.TryParse(() => ObjectConverter.ChangeType(value, typeof(TResult), provider), out o);
            return success ? (TResult)o : resultOnConversionNotPossible;
        }
    }
}