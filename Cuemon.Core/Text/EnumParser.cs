using System;
using System.Reflection;

namespace Cuemon.Text
{
    /// <summary>
    /// Provides a parser that converts a <see cref="string"/> to its equivalent <see cref="Enum"/>.
    /// </summary>
    public class EnumParser : ITypeParser<EnumOptions>
    {
        /// <summary>
        /// Converts the string representation of an enumeration to its <see cref="Enum"/> equivalent.
        /// </summary>
        /// <typeparam name="T">The type of the enumeration to return.</typeparam>
        /// <param name="input">The <see cref="string"/> to convert.</param>
        /// <param name="setup">The <see cref="EnumOptions"/> which may be configured.</param>
        /// <returns>An enumeration of <typeparamref name="T"/> equivalent to <paramref name="input"/>.</returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="input"/> cannot be null.
        /// </exception>
        /// <exception cref="ArgumentException">
        /// <paramref name="input"/> cannot be empty or consist only of white-space characters.
        /// </exception>
        /// <exception cref="TypeArgumentException">
        /// <typeparamref name="T"/> does not represents an enumeration.
        /// </exception>
        /// <exception cref="OverflowException">
        /// <paramref name="input"/> is outside the range of the underlying type of <typeparamref name="T"/>.
        /// </exception>
        /// <exception cref="InvalidOperationException">
        /// <paramref name="input"/> is not type SByte, Int16, Int32, Int64, Byte, UInt16, UInt32, or UInt64, or String.
        /// </exception>
        public T Parse<T>(string input, Action<EnumOptions> setup = null)
        {
            Validator.ThrowIfNotEnumType<T>(nameof(T));
            return (T)Parse(input, typeof(T), setup);
        }

        /// <summary>
        /// Converts the string representation of an enumeration to its <see cref="Enum"/> equivalent.
        /// </summary>
        /// <param name="input">The <see cref="string"/> to convert.</param>
        /// <param name="targetType">The <see cref="Type"/> of the <see cref="Enum"/> to return.</param>
        /// <param name="setup">The <see cref="EnumOptions"/> which may be configured.</param>
        /// <returns>An enumeration of <paramref name="targetType"/> equivalent to <paramref name="input"/>.</returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="input"/> cannot be null -or-
        /// <paramref name="targetType"/> cannot be null.
        /// </exception>
        /// <exception cref="ArgumentException">
        /// <paramref name="input"/> cannot be empty or consist only of white-space characters -or-
        /// <paramref name="targetType"/> does not represents an enumeration.
        /// </exception>
        /// <exception cref="OverflowException">
        /// <paramref name="input"/> is outside the range of the underlying type of <paramref name="targetType"/>.
        /// </exception>
        /// <exception cref="InvalidOperationException">
        /// <paramref name="input"/> is not type SByte, Int16, Int32, Int64, Byte, UInt16, UInt32, or UInt64, or String.
        /// </exception>
        public object Parse(string input, Type targetType, Action<EnumOptions> setup = null)
        {
            Validator.ThrowIfNullOrWhitespace(input, nameof(input));
            Validator.ThrowIfNull(targetType, nameof(targetType));
            Validator.ThrowIfNotEnumType(targetType, nameof(targetType));

            var options = Patterns.Configure(setup);
            var enumType = targetType;
            var hasFlags = enumType.GetTypeInfo().IsDefined(typeof(FlagsAttribute), false);
            var result = Enum.Parse(targetType, input, options.IgnoreCase);
            if (hasFlags && input.IndexOf(',') != -1) { return result; }
            if (Enum.IsDefined(targetType, result)) { return result; }
            throw new ArgumentException("Value does not represents an enumeration.");
        }

        /// <summary>
        /// Converts the string representation of an enumeration to its <see cref="Enum"/> equivalent. A return value indicates whether the conversion succeeded.
        /// </summary>
        /// <typeparam name="T">The type of the enumeration to return.</typeparam>
        /// <param name="input">The <see cref="string"/> to convert.</param>
        /// <param name="result">When this method returns, contains the enumeration of <typeparamref name="T"/> equivalent to <paramref name="input"/>, if the conversion succeeded, or <c>default</c> if the conversion failed.</param>
        /// <param name="setup">The <see cref="EnumOptions"/> which may be configured.</param>
        /// <returns><c>true</c> if <paramref name="input"/> was converted successfully; otherwise, <c>false</c>.</returns>
        public bool TryParse<T>(string input, out T result, Action<EnumOptions> setup = null)
        {
            return Patterns.TryInvoke(() => Parse<T>(input, setup), out result);
        }

        /// <summary>
        /// Converts the string representation of an enumeration to its <see cref="Enum"/> equivalent. A return value indicates whether the conversion succeeded.
        /// </summary>
        /// <param name="input">The <see cref="string"/> to convert.</param>
        /// <param name="targetType">The <see cref="Type"/> of the <see cref="Enum"/> to return.</param>
        /// <param name="result">When this method returns, contains the enumeration of <paramref name="targetType"/> equivalent to <paramref name="input"/>, if the conversion succeeded, or <c>default</c> if the conversion failed.</param>
        /// <param name="setup">The <see cref="EnumOptions"/> which may be configured.</param>
        /// <returns><c>true</c> if <paramref name="input"/> was converted successfully; otherwise, <c>false</c>.</returns>
        public bool TryParse(string input, Type targetType, out object result, Action<EnumOptions> setup = null)
        {
            return Patterns.TryInvoke(() => Parse(input, targetType, setup), out result);
        }
    }
}