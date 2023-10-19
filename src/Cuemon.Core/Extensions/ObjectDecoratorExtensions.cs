using System;
using System.ComponentModel;
using System.Globalization;
using System.Reflection;

namespace Cuemon
{
    /// <summary>
    /// Extension methods for the <see cref="object"/> class tailored to adhere the decorator pattern.
    /// </summary>
    /// <seealso cref="IDecorator{T}"/>
    /// <seealso cref="Decorator{T}"/>
    public static class ObjectDecoratorExtensions
    {
        /// <summary>
        /// Returns an <see cref="object"/> of the specified <typeparamref name="T"/> whose value is equivalent to the enclosed <see cref="object"/> of the specified <paramref name="decorator"/>.
        /// </summary>
        /// <typeparam name="T">The type of the object to return.</typeparam>
        /// <param name="decorator">The <see cref="IDecorator{String}"/> to extend.</param>
        /// <param name="fallbackResult">The value to return when a conversion is not possible. Default is <c>default</c> of <typeparamref name="T"/>.</param>
        /// <param name="setup">The <see cref="ObjectFormattingOptions"/> which may be configured.</param>
        /// <returns>An <see cref="object"/> of type <typeparamref name="T"/> equivalent to the enclosed <see cref="object"/> of the specified <paramref name="decorator"/> when a conversion is possible; otherwise <paramref name="fallbackResult"/> is returned.</returns>
        /// <remarks>This method first checks if the enclosed <see cref="object"/> of the specified <paramref name="decorator"/> is compatible with <typeparamref name="T"/>; if incompatible the method continues with <see cref="ChangeType{T}"/> for the operation.</remarks>
        public static T ChangeTypeOrDefault<T>(this IDecorator<object> decorator, T fallbackResult = default, Action<ObjectFormattingOptions> setup = null)
        {
            if (decorator.Inner is T result) { return result; }
            return Patterns.InvokeOrDefault(() => ChangeType<T>(decorator, setup), fallbackResult);
        }

        /// <summary>
        /// Returns an <see cref="object"/> of the specified <typeparamref name="T"/> whose value is equivalent to the enclosed <see cref="object"/> of the specified <paramref name="decorator"/>.
        /// </summary>
        /// <typeparam name="T">The type of the object to return.</typeparam>
        /// <param name="decorator">The <see cref="IDecorator{String}"/> to extend.</param>
        /// <param name="setup">The <see cref="ObjectFormattingOptions"/> which may be configured.</param>
        /// <returns>An <see cref="object"/> of type <typeparamref name="T"/> equivalent to the enclosed <see cref="object"/> of the specified <paramref name="decorator"/>.</returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="decorator"/> cannot be null.
        /// </exception>
        /// <exception cref="AggregateException">
        /// The enclosed <see cref="object"/> of <paramref name="decorator"/> could not be converted.
        /// </exception>
        /// <remarks>What differs from the <see cref="Convert.ChangeType(object,Type)"/> is, that this converter supports generics and enums. Fallback uses <see cref="TypeDescriptor"/> and checks if the underlying <see cref="IFormatProvider"/> of <see cref="ObjectFormattingOptions.FormatProvider"/> is a <see cref="CultureInfo"/>, then this will be used in the conversion together with <see cref="ObjectFormattingOptions.DescriptorContext"/>.</remarks>
        /// <seealso cref="Convert.ChangeType(object,Type)"/>
        /// <seealso cref="TypeDescriptor.GetConverter(Type)"/>
        public static T ChangeType<T>(this IDecorator<object> decorator, Action<ObjectFormattingOptions> setup = null)
        {
            return (T)ChangeType(decorator, typeof(T), setup);
        }

        /// <summary>
        /// Returns an <see cref="object"/> of a specified <paramref name="targetType"/> whose value is equivalent to the enclosed <see cref="object"/> of the specified <paramref name="decorator"/>.
        /// </summary>
        /// <param name="decorator">The <see cref="IDecorator{String}"/> to extend.</param>
        /// <param name="targetType">The type of the object to return.</param>
        /// <param name="setup">The <see cref="ObjectFormattingOptions"/> which may be configured.</param>
        /// <returns>An <see cref="object"/> of type <paramref name="targetType"/> equivalent to the enclosed <see cref="object"/> of the specified <paramref name="decorator"/>.</returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="decorator"/> cannot be null.
        /// </exception>
        /// <exception cref="AggregateException">
        /// The enclosed <see cref="object"/> of <paramref name="decorator"/> could not be converted.
        /// </exception>
        /// <remarks>What differs from the <see cref="Convert.ChangeType(object,Type)"/> is, that this converter supports generics and enums. Fallback uses <see cref="TypeDescriptor"/> and checks if the underlying <see cref="IFormatProvider"/> of <see cref="ObjectFormattingOptions.FormatProvider"/> is a <see cref="CultureInfo"/>, then this will be used in the conversion together with <see cref="ObjectFormattingOptions.DescriptorContext"/>.</remarks>
        /// <seealso cref="Convert.ChangeType(object,Type)"/>
        /// <seealso cref="TypeDescriptor.GetConverter(Type)"/>
        public static object ChangeType(this IDecorator<object> decorator, Type targetType, Action<ObjectFormattingOptions> setup = null)
        {
            Validator.ThrowIfNull(decorator);
            Validator.ThrowIfNull(targetType);
            if (decorator.Inner == null) { return null; }
            var options = Patterns.Configure(setup);
            try
            {
                var isEnum = targetType.GetTypeInfo().IsEnum;
                var isNullable = Decorator.Enclose(targetType).IsNullable();
                switch (targetType)
                {
                    case { } dt when dt == typeof(DateTime) && decorator.Inner is string dtValue && dtValue.EndsWith("Z", StringComparison.OrdinalIgnoreCase):
                        return DateTime.Parse(dtValue, options.FormatProvider, DateTimeStyles.AdjustToUniversal | DateTimeStyles.AssumeUniversal);
                    default:
                        return Convert.ChangeType(isEnum ? Enum.Parse(targetType, decorator.Inner.ToString()!) : decorator.Inner, (isNullable ? Nullable.GetUnderlyingType(targetType) : targetType)!, options.FormatProvider);
                }
            }
            catch (Exception first)
            {
                try
                {
                    if (options.FormatProvider is CultureInfo ci) { return TypeDescriptor.GetConverter(targetType).ConvertFrom(options.DescriptorContext, ci, decorator.Inner); }
                    return TypeDescriptor.GetConverter(targetType).ConvertFrom(decorator.Inner);
                }
                catch (Exception second)
                {
                    throw new AggregateException(first, second);
                }
            }
        }
    }
}
