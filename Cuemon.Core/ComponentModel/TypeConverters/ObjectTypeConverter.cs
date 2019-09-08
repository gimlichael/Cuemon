using System;
using System.ComponentModel;
using System.Globalization;
using System.Reflection;
using Cuemon.ComponentModel.Converters;
using Cuemon.Reflection;

namespace Cuemon.ComponentModel.TypeConverters
{
    /// <summary>
    /// Provides a converter that converts an <see cref="object"/> to an arbitrary <see cref="object"/>.
    /// </summary>
    public class ObjectTypeConverter : ITypeConverter<object, TypeConverterOptions>
    {
        /// <summary>
        /// Converts the specified <paramref name="input"/> to an arbitrary <see cref="object"/> of <typeparamref name="T"/>.
        /// </summary>
        /// <typeparam name="T">The type of the object to return.</typeparam>
        /// <param name="input">The object to convert.</param>
        /// <param name="setup">The <see cref="TypeConverterOptions"/> which may be configured.</param>
        /// <returns>An <see cref="object"/> of <typeparamref name="T"/> equivalent to <paramref name="input"/>.</returns>
        /// <exception cref="AggregateException">
        /// <paramref name="input"/> could not be converted.
        /// </exception>
        /// <remarks>What differs from the <see cref="Convert.ChangeType(object,Type)"/> is, that this converter supports generics and enums. Failover uses <see cref="TypeDescriptor"/>.</remarks>
        /// <seealso cref="Convert.ChangeType(object,Type)"/>
        /// <seealso cref="TypeDescriptor.GetConverter(Type)"/>
        public T ChangeType<T>(object input, Action<TypeConverterOptions> setup = null)
        {
            return (T)ChangeType(input, typeof(T), setup);
        }

        /// <summary>
        /// Converts the specified <paramref name="input"/> to its equivalent <typeparamref name="T"/> representation.
        /// </summary>
        /// <typeparam name="T">The type of the object to return.</typeparam>
        /// <param name="_">The marker interface of a converter having <see cref="object"/> as <paramref name="input"/>.</param>
        /// <param name="input">The <see cref="object"/> to be converted into <typeparamref name="T"/>.</param>
        /// <param name="fallbackResult">The value to return when a conversion is not possible. Default is <c>default</c> of <typeparamref name="T"/>.</param>
        /// <param name="setup">The <see cref="TypeConverterOptions"/> which may be configured.</param>
        /// <returns>An <see cref="object"/> of <typeparamref name="T"/> that is equivalent to <paramref name="input"/> when a conversion is possible; otherwise <paramref name="fallbackResult"/> is returned.</returns>
        /// <remarks>This method first checks if <paramref name="input"/> is compatible with <typeparamref name="T"/>; if incompatible the method continues with <see cref="ChangeType{T}"/> for the operation.</remarks>
        /// <seealso cref="Convert.ChangeType(object,Type)"/>
        /// <seealso cref="TypeDescriptor.GetConverter(Type)"/>
        public T ChangeTypeOrDefault<T>(object input, T fallbackResult = default, Action<TypeConverterOptions> setup = null)
        {
            if (input is T result) { return result; }
            return Patterns.InvokeOrDefault(() => ChangeType<T>(input, setup), fallbackResult);
        }

        /// <summary>
        /// Converts the specified <paramref name="input"/> to an arbitrary <see cref="object"/> of <paramref name="targetType"/>.
        /// </summary>
        /// <param name="input">The object to convert.</param>
        /// <param name="targetType">The type of the object to return.</param>
        /// <param name="setup">The <see cref="TypeConverterOptions"/> which may be configured.</param>
        /// <returns>An <see cref="object"/> of <paramref name="targetType"/> equivalent to <paramref name="input"/>.</returns>
        /// <exception cref="AggregateException">
        /// <paramref name="input"/> could not be converted.
        /// </exception>
        /// <remarks>What differs from the <see cref="Convert.ChangeType(object,Type)"/> is, that this converter supports generics and enums. Failover uses <see cref="TypeDescriptor"/> and checks if the underlying <see cref="IFormatProvider"/> of <see cref="TypeConverterOptions.FormatProvider"/> is a <see cref="CultureInfo"/>, then this will be used in the conversion together with <see cref="TypeConverterOptions.DescriptorContext"/>.</remarks>
        /// <seealso cref="Convert.ChangeType(object,Type)"/>
        /// <seealso cref="TypeDescriptor.GetConverter(Type)"/>
        public object ChangeType(object input, Type targetType, Action<TypeConverterOptions> setup = null)
        {
            Validator.ThrowIfNull(targetType, nameof(targetType));
            if (input == null) { return null; }

            var options = Patterns.Configure(setup);
            try
            {
                var isEnum = targetType.GetTypeInfo().IsEnum;
                var isNullable = TypeInsight.FromType(targetType).IsNullable();
                return System.Convert.ChangeType(isEnum ? Enum.Parse(targetType, input.ToString()) : input, isNullable ? Nullable.GetUnderlyingType(targetType) : targetType, options.FormatProvider);
            }
            catch (Exception first)
            {
                try
                {
                    if (options.FormatProvider is CultureInfo ci)
                    {
                        return TypeDescriptor.GetConverter(targetType).ConvertFrom(options.DescriptorContext, ci, input);
                    }
                    return TypeDescriptor.GetConverter(targetType).ConvertFrom(input);
                }
                catch (Exception second)
                {
                    throw new AggregateException(first, second);
                }
            }
        }
    }
}