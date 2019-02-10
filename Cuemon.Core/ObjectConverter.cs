using System;
using System.ComponentModel;
using System.Globalization;
using System.Reflection;

namespace Cuemon
{
    /// <summary>
    /// This utility class is designed to make <see cref="Object"/> related conversions easier to work with.
    /// </summary>
    public static class ObjectConverter
    {
        /// <summary>
        /// Returns an object of the specified type and whose value is equivalent to the specified object.
        /// </summary>
        /// <param name="value">The object to convert the underlying type.</param>
        /// <param name="conversionType">The <see cref="Type"/> of object to return.</param>
        /// <returns>An object whose type is <paramref name="conversionType"/> and whose value is equivalent to <paramref name="value"/>.</returns>
        /// <remarks>What differs from the <see cref="Convert.ChangeType(object,System.Type)"/> is, that this converter supports generics and enums somewhat automated.</remarks>
        public static object ChangeType(object value, Type conversionType)
        {
            return ChangeType(value, conversionType, CultureInfo.InvariantCulture);
        }

        /// <summary>
        /// Returns an object of the specified type and whose value is equivalent to the specified object.
        /// </summary>
        /// <param name="value">The object to convert the underlying type.</param>
        /// <param name="conversionType">The <see cref="Type"/> of object to return.</param>
        /// <param name="provider">An object that supplies culture-specific formatting information.</param>
        /// <returns>An object whose type is <paramref name="conversionType"/> and whose value is equivalent to <paramref name="value"/>.</returns>
        /// <remarks>What differs from the <see cref="Convert.ChangeType(object,System.Type)"/> is, that this converter supports generics and enums. Failover uses <see cref="TypeDescriptor"/>.</remarks>
        public static object ChangeType(object value, Type conversionType, IFormatProvider provider)
        {
            Validator.ThrowIfNull(conversionType, nameof(conversionType));
            if (value == null) { return null; }

            try
            {
                bool isEnum = conversionType.GetTypeInfo().IsEnum;
                bool isNullable = TypeUtility.IsNullable(conversionType);
                return Convert.ChangeType(isEnum ? Enum.Parse(conversionType, value.ToString()) : value, isNullable ? Nullable.GetUnderlyingType(conversionType) : conversionType, provider);
            }
            catch (Exception first)
            {
                try
                {
                    return TypeDescriptor.GetConverter(conversionType).ConvertFrom(value);
                }
                catch (Exception second)
                {
                    throw new AggregateException(first, second);
                }
            }
        }

        /// <summary>
        /// Returns a primitive object whose value is equivalent to the specified <paramref name="value"/>.
        /// </summary>
        /// <param name="value">The string to convert the underlying type.</param>
        /// <returns>An object whose type is primitive (either <see cref="bool"/>, <see cref="long"/> or <see cref="double"/>) and whose value is equivalent to <paramref name="value"/>. If conversion is unsuccessful, the original <paramref name="value"/> is returned.</returns>
        public static object FromString(string value)
        {
            return FromString(value, CultureInfo.InvariantCulture);
        }

        /// <summary>
        /// Returns a primitive object whose value is equivalent to the specified <paramref name="value"/>.
        /// </summary>
        /// <param name="value">The string to convert the underlying type.</param>
        /// <param name="provider">An object that supplies culture-specific formatting information.</param>
        /// <returns>An object whose type is primitive (either <see cref="bool"/>, <see cref="long"/> or <see cref="double"/>) and whose value is equivalent to <paramref name="value"/>. If conversion is unsuccessful, the original <paramref name="value"/> is returned.</returns>
        public static object FromString(string value, IFormatProvider provider)
        {
            if (value == null) { return null; }

            bool boolValue;
            byte byteValue;
            int intValue;
            long longValue;
            double doubleValue;
            DateTime dateTimeValue;
            Guid guidValue;

            if (Boolean.TryParse(value, out boolValue)) { return boolValue; }
            if (Byte.TryParse(value, NumberStyles.None, provider, out byteValue)) { return byteValue; }
            if (Int32.TryParse(value, NumberStyles.None, provider, out intValue)) { return intValue; }
            if (Int64.TryParse(value, NumberStyles.None, provider, out longValue)) { return longValue; }
            if (Double.TryParse(value, NumberStyles.Number, provider, out doubleValue)) { return doubleValue; }
            if (value.Length > 6 && DateTime.TryParse(value, CultureInfo.InvariantCulture, DateTimeStyles.AdjustToUniversal, out dateTimeValue)) { return dateTimeValue; }
            if (value.Length > 31 && GuidUtility.TryParse(value, out guidValue)) { return guidValue; }

            return value;
        }
    }
}