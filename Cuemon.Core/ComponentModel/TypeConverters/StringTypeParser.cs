using System;
using System.ComponentModel;

namespace Cuemon.ComponentModel.TypeConverters
{
    /// <summary>
    /// Provides a parser that converts a <see cref="string"/> to an arbitrary <see cref="object"/>.
    /// </summary>
    public class StringTypeParser : ITypeParser<StringTypeOptions>
    {
        /// <summary>
        /// Converts the specified string to an arbitrary object of <typeparamref name="T"/>.
        /// </summary>
        /// <typeparam name="T">The type of the arbitrary object.</typeparam>
        /// <param name="input">The string to convert.</param>
        /// <param name="setup">The <see cref="StringTypeOptions"/> which may be configured.</param>
        /// <returns>An object of <typeparamref name="T"/> equivalent to <paramref name="input"/>.</returns>
        /// <exception cref="NotSupportedException">
        /// <paramref name="input"/> cannot be converted to the specified <typeparamref name="T"/>.
        /// </exception>
        public T Parse<T>(string input, Action<StringTypeOptions> setup = null)
        {
            if (input == null) { return default; }
            var options = Patterns.Configure(setup);
            var converter = TypeDescriptor.GetConverter(typeof(T));
            return (T)converter.ConvertFromString(options.DescriptorContext, options.Culture, input);
        }

        /// <summary>
        /// Converts the specified string to an arbitrary object of <typeparamref name="T"/>. A return value indicates whether the conversion succeeded.
        /// </summary>
        /// <typeparam name="T">The type of the arbitrary object.</typeparam>
        /// <param name="input">The string to convert.</param>
        /// <param name="result">When this method returns, contains the object of <typeparamref name="T"/> equivalent to <paramref name="input"/>, if the conversion succeeded, or <c>default</c> if the conversion failed.</param>
        /// <param name="setup">The <see cref="StringTypeOptions"/> which may be configured.</param>
        /// <returns><c>true</c> if <paramref name="input"/> was converted successfully; otherwise, <c>false</c>.</returns>
        public bool TryParse<T>(string input, out T result, Action<StringTypeOptions> setup = null)
        {
            return Patterns.TryParse(() => Parse<T>(input, setup), out result);
        }
    }
}