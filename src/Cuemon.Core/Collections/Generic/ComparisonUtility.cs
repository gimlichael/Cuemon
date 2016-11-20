using System;
using System.Collections.Generic;

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
            object xValue = valueSelector(x);
            object yValue = valueSelector(y);
            TypeCode code = TypeCodeConverter.FromType(typeSelector(x));
            switch (code)
            {
                case TypeCode.Boolean:
                    return Comparer<bool>.Default.Compare(Converter.FromObject<bool>(xValue), Converter.FromObject<bool>(yValue));
                case TypeCode.Byte:
                    return Comparer<byte>.Default.Compare(Converter.FromObject<byte>(xValue), Converter.FromObject<byte>(yValue));
                case TypeCode.Char:
                    return Comparer<char>.Default.Compare(Converter.FromObject<char>(xValue), Converter.FromObject<char>(yValue));
                case TypeCode.DateTime:
                    return Comparer<DateTime>.Default.Compare(Converter.FromObject<DateTime>(xValue), Converter.FromObject<DateTime>(yValue));
                case TypeCode.Decimal:
                    return Comparer<decimal>.Default.Compare(Converter.FromObject<decimal>(xValue), Converter.FromObject<decimal>(yValue));
                case TypeCode.Double:
                    return Comparer<double>.Default.Compare(Converter.FromObject<double>(xValue), Converter.FromObject<double>(yValue));
                case TypeCode.Int16:
                    return Comparer<short>.Default.Compare(Converter.FromObject<short>(xValue), Converter.FromObject<short>(yValue));
                case TypeCode.Int32:
                    return Comparer<int>.Default.Compare(Converter.FromObject<int>(xValue), Converter.FromObject<int>(yValue));
                case TypeCode.Int64:
                    return Comparer<long>.Default.Compare(Converter.FromObject<long>(xValue), Converter.FromObject<long>(yValue));
                case TypeCode.SByte:
                    return Comparer<sbyte>.Default.Compare(Converter.FromObject<sbyte>(xValue), Converter.FromObject<sbyte>(yValue));
                case TypeCode.Single:
                    return Comparer<float>.Default.Compare(Converter.FromObject<float>(xValue), Converter.FromObject<float>(yValue));
                case TypeCode.String:
                    return Comparer<string>.Default.Compare(Converter.FromObject<string>(xValue), Converter.FromObject<string>(yValue));
                case TypeCode.UInt16:
                    return Comparer<ushort>.Default.Compare(Converter.FromObject<ushort>(xValue), Converter.FromObject<ushort>(yValue));
                case TypeCode.UInt32:
                    return Comparer<uint>.Default.Compare(Converter.FromObject<uint>(xValue), Converter.FromObject<uint>(yValue));
                case TypeCode.UInt64:
                    return Comparer<ulong>.Default.Compare(Converter.FromObject<ulong>(xValue), Converter.FromObject<ulong>(yValue));
                default:
                    return Comparer<object>.Default.Compare(Converter.FromObject<object>(xValue), Converter.FromObject<object>(yValue));
            }
        }
    }
}