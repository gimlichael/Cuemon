using System;
using System.ComponentModel;
using System.Globalization;
using Cuemon.ComponentModel;

namespace Cuemon.Text
{
    /// <summary>
    /// Provides a parser that converts a <see cref="string"/> to an arbitrary <see cref="object"/> of a particular type.
    /// </summary>
    public class ComplexValueParser : ITypeParser<ObjectConverterOptions>
    {
        /// <summary>
        /// Converts the specified string to an object of <typeparamref name="T"/>.
        /// </summary>
        /// <typeparam name="T">The type of the object to return.</typeparam>
        /// <param name="input">The string to convert.</param>
        /// <param name="setup">The <see cref="ObjectConverterOptions"/> which may be configured.</param>
        /// <returns>An object of <typeparamref name="T"/> equivalent to <paramref name="input"/>.</returns>
        /// <exception cref="NotSupportedException">
        /// <paramref name="input"/> cannot be converted to the specified <typeparamref name="T"/>.
        /// </exception>
        /// <remarks>If the underlying <see cref="IFormatProvider"/> of <see cref="ObjectConverterOptions.FormatProvider"/> is a <see cref="CultureInfo"/>, then this will be used in the conversion.</remarks>
        public T Parse<T>(string input, Action<ObjectConverterOptions> setup = null)
        {
            return (T)Parse(input, typeof(T), setup);
        }

        /// <summary>
        /// Converts the specified string to an object of <paramref name="targetType"/>.
        /// </summary>
        /// <param name="input">The string to convert.</param>
        /// <param name="targetType">The type of the object to return.</param>
        /// <param name="setup">The <see cref="ObjectConverterOptions"/> which may be configured.</param>
        /// <returns>An object of <paramref name="targetType"/> equivalent to <paramref name="input"/>.</returns>
        /// <exception cref="NotSupportedException">
        /// <paramref name="input"/> cannot be converted to the specified <paramref name="targetType"/>.
        /// </exception>
        /// <remarks>If the underlying <see cref="IFormatProvider"/> of <see cref="ObjectConverterOptions.FormatProvider"/> is a <see cref="CultureInfo"/>, then this will be used in the conversion.</remarks>
        public object Parse(string input, Type targetType, Action<ObjectConverterOptions> setup = null)
        {
            if (input == null) { return default; }
            var options = Patterns.Configure(setup);
            var converter = TypeDescriptor.GetConverter(targetType);
            if (options.FormatProvider is CultureInfo ci)
            {
                return converter.ConvertFromString(options.DescriptorContext, ci, input);
            }
            return converter.ConvertFromString(options.DescriptorContext, input);
        }

        /// <summary>
        /// Converts the specified string to an object of <typeparamref name="T"/>. A return value indicates whether the conversion succeeded.
        /// </summary>
        /// <typeparam name="T">The type of the object to return.</typeparam>
        /// <param name="input">The string to convert.</param>
        /// <param name="result">When this method returns, contains the object of <typeparamref name="T"/> equivalent to <paramref name="input"/>, if the conversion succeeded, or <c>default</c> if the conversion failed.</param>
        /// <param name="setup">The <see cref="ObjectConverterOptions"/> which may be configured.</param>
        /// <returns><c>true</c> if <paramref name="input"/> was converted successfully; otherwise, <c>false</c>.</returns>
        /// <remarks>If the underlying <see cref="IFormatProvider"/> of <see cref="ObjectConverterOptions.FormatProvider"/> is a <see cref="CultureInfo"/>, then this will be used in the conversion.</remarks>
        public bool TryParse<T>(string input, out T result, Action<ObjectConverterOptions> setup = null)
        {
            return Patterns.TryInvoke(() => Parse<T>(input, setup), out result);
        }

        /// <summary>
        /// Converts the specified string to an object of <paramref name="targetType"/>. A return value indicates whether the conversion succeeded.
        /// </summary>
        /// <param name="input">The string to convert.</param>
        /// <param name="targetType">The type of the object to return.</param>
        /// <param name="result">When this method returns, contains the object of <paramref name="targetType"/> equivalent to <paramref name="input"/>, if the conversion succeeded, or <c>default</c> if the conversion failed.</param>
        /// <param name="setup">The <see cref="ObjectConverterOptions"/> which may be configured.</param>
        /// <returns><c>true</c> if <paramref name="input"/> was converted successfully; otherwise, <c>false</c>.</returns>
        /// <remarks>If the underlying <see cref="IFormatProvider"/> of <see cref="ObjectConverterOptions.FormatProvider"/> is a <see cref="CultureInfo"/>, then this will be used in the conversion.</remarks>
        public bool TryParse(string input, Type targetType, out object result, Action<ObjectConverterOptions> setup = null)
        {
            return Patterns.TryInvoke(() => Parse(input, targetType, setup), out result);
        }
    }
}