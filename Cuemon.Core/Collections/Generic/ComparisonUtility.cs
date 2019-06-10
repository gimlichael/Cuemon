using System;
using System.Collections.Generic;
using Cuemon.ComponentModel.Converters;
using Cuemon.ComponentModel.TypeConverters;

namespace Cuemon.Collections.Generic
{
    /// <summary>
    /// Provide a generic way to work with <see cref="Comparison{T}" /> related tasks.
    /// </summary>
    public static class ComparisonUtility
    {
        /// <summary>
        /// Performs a default comparison of two objects of the same type and returns a value indicating whether one object is less than, equal to, or greater than the other.
        /// </summary>
        /// <typeparam name="T">The type of the objects to compare.</typeparam>
        /// <param name="x">The first object to compare.</param>
        /// <param name="y">The second object to compare.</param>
        /// <param name="typeSelector">The function delegate that is used to select the <see cref="Type"/> of the objects to compare.</param>
        /// <param name="valueSelector">The function delegate that is used to select the value of <paramref name="x"/> and <paramref name="y"/>.</param>
        /// <returns>A signed integer that indicates the relative values of <paramref name="x"/> and <paramref name="y"/>.</returns>
        public static int Default<T>(T x, T y, Func<T, Type> typeSelector, Func<T, object> valueSelector)
        {
            Validator.ThrowIfNull(typeSelector, nameof(typeSelector));
            Validator.ThrowIfNull(valueSelector, nameof(valueSelector));
            var xValue = valueSelector(x);
            var yValue = valueSelector(y);
            var code = ConvertFactory.UseConverter<TypeCodeConverter>().ChangeType(typeSelector(x));
            var converter = ConvertFactory.UseConverter<ObjectTypeConverter>();
            switch (code)
            {
                case TypeCode.Boolean:
                    return Comparer<bool>.Default.Compare(converter.ChangeTypeOrDefault<bool>(xValue), converter.ChangeTypeOrDefault<bool>(yValue));
                case TypeCode.Byte:
                    return Comparer<byte>.Default.Compare(converter.ChangeTypeOrDefault<byte>(xValue), converter.ChangeTypeOrDefault<byte>(yValue));
                case TypeCode.Char:
                    return Comparer<char>.Default.Compare(converter.ChangeTypeOrDefault<char>(xValue), converter.ChangeTypeOrDefault<char>(yValue));
                case TypeCode.DateTime:
                    return Comparer<DateTime>.Default.Compare(converter.ChangeTypeOrDefault<DateTime>(xValue), converter.ChangeTypeOrDefault<DateTime>(yValue));
                case TypeCode.Decimal:
                    return Comparer<decimal>.Default.Compare(converter.ChangeTypeOrDefault<decimal>(xValue), converter.ChangeTypeOrDefault<decimal>(yValue));
                case TypeCode.Double:
                    return Comparer<double>.Default.Compare(converter.ChangeTypeOrDefault<double>(xValue), converter.ChangeTypeOrDefault<double>(yValue));
                case TypeCode.Int16:
                    return Comparer<short>.Default.Compare(converter.ChangeTypeOrDefault<short>(xValue), converter.ChangeTypeOrDefault<short>(yValue));
                case TypeCode.Int32:
                    return Comparer<int>.Default.Compare(converter.ChangeTypeOrDefault<int>(xValue), converter.ChangeTypeOrDefault<int>(yValue));
                case TypeCode.Int64:
                    return Comparer<long>.Default.Compare(converter.ChangeTypeOrDefault<long>(xValue), converter.ChangeTypeOrDefault<long>(yValue));
                case TypeCode.SByte:
                    return Comparer<sbyte>.Default.Compare(converter.ChangeTypeOrDefault<sbyte>(xValue), converter.ChangeTypeOrDefault<sbyte>(yValue));
                case TypeCode.Single:
                    return Comparer<float>.Default.Compare(converter.ChangeTypeOrDefault<float>(xValue), converter.ChangeTypeOrDefault<float>(yValue));
                case TypeCode.String:
                    return Comparer<string>.Default.Compare(converter.ChangeTypeOrDefault<string>(xValue), converter.ChangeTypeOrDefault<string>(yValue));
                case TypeCode.UInt16:
                    return Comparer<ushort>.Default.Compare(converter.ChangeTypeOrDefault<ushort>(xValue), converter.ChangeTypeOrDefault<ushort>(yValue));
                case TypeCode.UInt32:
                    return Comparer<uint>.Default.Compare(converter.ChangeTypeOrDefault<uint>(xValue), converter.ChangeTypeOrDefault<uint>(yValue));
                case TypeCode.UInt64:
                    return Comparer<ulong>.Default.Compare(converter.ChangeTypeOrDefault<ulong>(xValue), converter.ChangeTypeOrDefault<ulong>(yValue));
                default:
                    return Comparer<object>.Default.Compare(converter.ChangeTypeOrDefault<object>(xValue), converter.ChangeTypeOrDefault<object>(yValue));
            }
        }
    }
}