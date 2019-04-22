using System;
using System.Collections.Generic;
using System.Globalization;
using System.Reflection;

namespace Cuemon
{
    /// <summary>
    /// This utility class is designed to make enum related operations easier to work with.
    /// </summary>
    public static class EnumUtility
    {
        /// <summary>
        /// Creates an <see cref="IEnumerable{T}"/> sequence from the specified <typeparamref name="TEnum"/>.
        /// </summary>
        /// <typeparam name="TEnum">The type of the enumeration.</typeparam>
        /// <returns>An <see cref="IEnumerable{T}"/> sequence equivalent to <typeparamref name="TEnum"/>.</returns>
        public static IEnumerable<KeyValuePair<int, string>> ToEnumerable<TEnum>() where TEnum : struct, IConvertible
        {
            return ToEnumerable<int, TEnum>();
        }

        /// <summary>
        /// Creates an <see cref="IEnumerable{T}"/> sequence from the specified <typeparamref name="TEnum"/>.
        /// </summary>
        /// <typeparam name="T">The integral type of the enumeration.</typeparam>
        /// <typeparam name="TEnum">The type of the enumeration.</typeparam>
        /// <returns>An <see cref="IEnumerable{T}"/> sequence equivalent to <typeparamref name="TEnum"/>.</returns>
        public static IEnumerable<KeyValuePair<T, string>> ToEnumerable<T, TEnum>() where TEnum : struct, IConvertible where T : struct, IConvertible
        {
            Validator.ThrowIfNotEnumType<TEnum>("TEnum");
            Validator.ThrowIfNotContainsType<T>("T", typeof(Int16), typeof(Int32), typeof(Int64), typeof(UInt16), typeof(UInt32), typeof(UInt64));

            Array values = Enum.GetValues(typeof(TEnum));
            foreach (var value in values)
            {
                yield return new KeyValuePair<T, string>((T)value, value.ToString());
            }
        }

        /// <summary>
        /// Determines whether the specified <paramref name="value"/> is an equivalent of <typeparamref name="TEnum"/>.
        /// </summary>
        /// <typeparam name="TEnum">The type of the enumeration to validate.</typeparam>
        /// <param name="value">A string containing the name or value to validate.</param>
        /// <returns><c>true</c> if the <paramref name="value"/> parameter is an equivalent of <typeparamref name="TEnum"/>; otherwise, <c>false</c>.</returns>
        public static bool IsStringOf<TEnum>(string value) where TEnum : struct, IConvertible
        {
            return IsStringOf<TEnum>(value, true);
        }

        /// <summary>
        /// Determines whether the specified <paramref name="value"/> is an equivalent of <typeparamref name="TEnum"/>.
        /// </summary>
        /// <typeparam name="TEnum">The type of the enumeration to validate.</typeparam>
        /// <param name="value">A string containing the name or value to validate.</param>
        /// <param name="ignoreCase"><c>true</c> to ignore case; <c>false</c> to regard case.</param>
        /// <returns><c>true</c> if the <paramref name="value"/> parameter is an equivalent of <typeparamref name="TEnum"/>; otherwise, <c>false</c>.</returns>
        public static bool IsStringOf<TEnum>(string value, bool ignoreCase) where TEnum : struct, IConvertible
        {
            TEnum result;
            if (string.IsNullOrEmpty(value)) { return false; }
            return typeof(TEnum).GetTypeInfo().IsEnum && TryParse(value, ignoreCase, out result);
        }

        /// <summary>
        /// Converts the specified <paramref name="value"/> of the name or numeric value of one or more enumerated constants to an equivalent enumerated object.
        /// The return value indicates whether the conversion succeeded.
        /// </summary>
        /// <typeparam name="TEnum">The type of the enumeration to convert.</typeparam>
        /// <param name="value">A string containing the name or value to convert.</param>
        /// <param name="result">When this method returns, <paramref name="result"/> contains an object of type <typeparamref name="TEnum"/> whose value is represented by <paramref name="value"/> if the parse operation succeeds. If the parse operation fails, result contains the default value of the underlying type of <typeparamref name="TEnum"/>.</param>
        /// <returns><c>true</c> if the <paramref name="value"/> parameter was converted successfully; otherwise, <c>false</c>.</returns>
        public static bool TryParse<TEnum>(string value, out TEnum result) where TEnum : struct, IConvertible
        {
            return TryParse(value, false, out result);
        }

        /// <summary>
        /// Converts the specified <paramref name="value"/> of the name or numeric value of one or more enumerated constants to an equivalent enumerated object.
        /// A parameter specifies whether the operation is case-sensitive. The return value indicates whether the conversion succeeded.
        /// </summary>
        /// <typeparam name="TEnum">The type of the enumeration to convert.</typeparam>
        /// <param name="value">A string containing the name or value to convert.</param>
        /// <param name="ignoreCase"><c>true</c> to ignore case; <c>false</c> to regard case.</param>
        /// <param name="result">When this method returns, <paramref name="result"/> contains an object of type <typeparamref name="TEnum"/> whose value is represented by <paramref name="value"/> if the parse operation succeeds. If the parse operation fails, result contains the default value of the underlying type of <typeparamref name="TEnum"/>.</param>
        /// <returns><c>true</c> if the <paramref name="value"/> parameter was converted successfully; otherwise, <c>false</c>.</returns>
        public static bool TryParse<TEnum>(string value, bool ignoreCase, out TEnum result) where TEnum : struct, IConvertible
        {
            return Patterns.TryParse(() => Parse<TEnum>(value, ignoreCase), out result);
        }

        /// <summary>
        /// Converts the string representation of the name or numeric <paramref name="value"/> of one or more enumerated constants to an equivalent enumerated <typeparamref name="TEnum"/>.
        /// </summary>
        /// <typeparam name="TEnum">The type of the enumeration to convert.</typeparam>
        /// <param name="value">A string containing the name or value to convert.</param>
        /// <returns>An enum of type <typeparamref name="TEnum"/> whose value is represented by <paramref name="value"/>.</returns>
        public static TEnum Parse<TEnum>(string value) where TEnum : struct, IConvertible
        {
            return Parse<TEnum>(value, false);
        }

        /// <summary>
        /// Converts the string representation of the name or numeric <paramref name="value"/> of one or more enumerated constants to an equivalent enumerated <typeparamref name="TEnum"/>.
        /// </summary>
        /// <typeparam name="TEnum">The type of the enumeration to convert.</typeparam>
        /// <param name="value">A string containing the name or value to convert.</param>
        /// <param name="ignoreCase"><c>true</c> to ignore case; <c>false</c> to regard case.</param>
        /// <returns>An enum of type <typeparamref name="TEnum" /> whose value is represented by <paramref name="value" />.</returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="value"/> is null.
        /// </exception>
        /// <exception cref="TypeArgumentException">
        /// <typeparamref name="TEnum"/> does not represents an enumeration.
        /// </exception>
        /// <exception cref="ArgumentException">
        /// <paramref name="value"/> does not represents an enumeration.
        /// </exception>
        public static TEnum Parse<TEnum>(string value, bool ignoreCase) where TEnum : struct, IConvertible
        {
            Validator.ThrowIfNotEnumType<TEnum>("TEnum");
            Validator.ThrowIfNullOrEmpty(value, nameof(value));
            Type enumType = typeof(TEnum);
            bool hasFlags = enumType.GetTypeInfo().IsDefined(typeof(FlagsAttribute), false);
            TEnum result = (TEnum)Enum.Parse(typeof(TEnum), value, ignoreCase);
            if (hasFlags && value.IndexOf(',') > 0) { return result; }
            if (Enum.IsDefined(typeof(TEnum), result)) { return result; }
            throw new ArgumentException("Value does not represents an enumeration.");
        }

        /// <summary>
        /// Determines whether one or more bit fields are set in the specified <paramref name="source"/>.
        /// </summary>
        /// <typeparam name="TEnum">The type of the enumeration.</typeparam>
        /// <param name="source">The bit field or bit fields to test if set in <paramref name="value"/>.</param>
        /// <param name="value">An enumeration value.</param>
        /// <returns><c>true</c> if the bit field or bit fields that are set in <paramref name="source"/> are also set in the <paramref name="value"/>; otherwise, <c>false</c>.</returns>
        public static bool HasFlag<TEnum>(TEnum source, TEnum value) where TEnum : struct, IConvertible
        {
            Validator.ThrowIfNotEnumType<TEnum>("TEnum");

            try
            {
                long signedSource = source.ToInt64(CultureInfo.InvariantCulture);
                long signedValue = value.ToInt64(CultureInfo.InvariantCulture);
                return ((signedSource & signedValue) == signedValue);
            }
            catch (OverflowException)
            {
                ulong unsignedSource = source.ToUInt64(CultureInfo.InvariantCulture);
                ulong unsignedValue = value.ToUInt64(CultureInfo.InvariantCulture);
                return ((unsignedSource & unsignedValue) == unsignedValue);
            }
        }
    }
}