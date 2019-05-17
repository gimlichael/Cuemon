using System;
using Cuemon.ComponentModel.TypeConverters;

namespace Cuemon.ComponentModel
{
    /// <summary>
    /// Extension methods for the <see cref="T:ITypeConversion{System.Object}"/> interface.
    /// </summary>
    public static class ObjectTypeConverters
    {
        /// <summary>
        /// Converts the specified object to an arbitrary object of <typeparamref name="T"/>.
        /// </summary>
        /// <typeparam name="T">The type of the object to return.</typeparam>
        /// <param name="_">The marker interface of a converter having <see cref="object"/> as <paramref name="input"/>.</param>
        /// <param name="input">The object to convert.</param>
        /// <param name="setup">The <see cref="TypeConverterOptions"/> which may be configured.</param>
        /// <returns>An object of <typeparamref name="T"/> equivalent to <paramref name="input"/>.</returns>
        /// <exception cref="AggregateException">
        /// <paramref name="input"/> could not be converted.
        /// </exception>
        /// <seealso cref="ObjectTypeConverter"/>
        /// <seealso cref="TypeConverterOptions"/>
        public static T ChangeType<T>(this ITypeConversion<object> _, object input, Action<TypeConverterOptions> setup = null)
        {
            return TypeConverter<object, TypeConverterOptions>.UseConverter<ObjectTypeConverter, T>(input, setup);
        }

        /// <summary>
        /// Converts the specified object to an arbitrary object of <paramref name="targetType"/>.
        /// </summary>
        /// <param name="_">The marker interface of a converter having <see cref="object"/> as <paramref name="input"/>.</param>
        /// <param name="input">The object to convert.</param>
        /// <param name="targetType">The type of the object to return.</param>
        /// <param name="setup">The <see cref="TypeConverterOptions"/> which may be configured.</param>
        /// <returns>An object of <paramref name="targetType"/> equivalent to <paramref name="input"/>.</returns>
        /// <exception cref="AggregateException">
        /// <paramref name="input"/> could not be converted.
        /// </exception>
        /// <seealso cref="ObjectTypeConverter"/>
        /// <seealso cref="TypeConverterOptions"/>
        public static object ChangeType(this ITypeConversion<object> _, object input, Type targetType, Action<TypeConverterOptions> setup = null)
        {
            return TypeConverter<object, TypeConverterOptions>.UseConverter<ObjectTypeConverter>(input, targetType, setup);
        }

        /// <summary>
        /// Converts the specified <paramref name="input"/> to its equivalent <typeparamref name="T"/> representation.
        /// </summary>
        /// <typeparam name="T">The type of the object to return.</typeparam>
        /// <param name="_">The marker interface of a converter having <see cref="object"/> as <paramref name="input"/>.</param>
        /// <param name="input">The <see cref="object"/> to be converted into <typeparamref name="T"/>.</param>
        /// <param name="fallbackResult">The value to return when a conversion is not possible. Default is <c>default</c> of <typeparamref name="T"/>.</param>
        /// <param name="setup">The <see cref="TypeConverterOptions"/> which may be configured.</param>
        /// <returns>An object of <typeparamref name="T"/> that is equivalent to <paramref name="input"/> when a conversion is possible; otherwise <paramref name="fallbackResult"/> is returned.</returns>
        /// <remarks>This method first checks if <paramref name="input"/> is compatible with <typeparamref name="T"/>; if incompatible the method continues with <see cref="ChangeType{T}"/> for the operation.</remarks>
        /// <seealso cref="ObjectTypeConverter"/>
        /// <seealso cref="TypeConverterOptions"/>
        public static T ToTypeOrDefault<T>(this ITypeConversion<object> _, object input, T fallbackResult = default, Action<TypeConverterOptions> setup = null)
        {
            if (input is T result) { return result; }
            var converted = Patterns.TryInvoke(() => ChangeType<T>(_, input, setup), out result);
            return converted ? result : fallbackResult;
        }
    }
}