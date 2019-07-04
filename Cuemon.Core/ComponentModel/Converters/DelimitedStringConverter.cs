using System;
using System.Collections.Generic;
using System.Text;

namespace Cuemon.ComponentModel.Converters
{
    /// <summary>
    /// Provides a converter that converts a sequence of <typeparamref name="T"/> to a delimited <see cref="string" />.
    /// Implements the <see cref="IConverter{TInput,TOutput,TOptions}" />
    /// </summary>
    /// <typeparam name="T">The type of elements in the sequence to convert.</typeparam>
    /// <seealso cref="IConverter{TInput,TOutput,TOptions}" />
    public class DelimitedStringConverter<T> : IConverter<IEnumerable<T>, string, DelimitedStringOptions<T>>
    {
        /// <summary>
        /// Converts the specified <paramref name="input"/> sequence to a delimited <see cref="string"/> representation.
        /// </summary>
        /// <param name="input">The <see cref="IEnumerable{T}"/> to be converted into a <see cref="string"/>.</param>
        /// <param name="setup">The <see cref="DelimitedStringOptions{T}"/> which may be configured.</param>
        /// <returns>A <see cref="string"/> of delimited values that is a result of <paramref name="input"/>.</returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="input"/> cannot be null.
        /// </exception>
        public string ChangeType(IEnumerable<T> input, Action<DelimitedStringOptions<T>> setup = null)
        {
            Validator.ThrowIfNull(input, nameof(input));
            var options = Patterns.Configure(setup);

            var delimitedValues = new StringBuilder();
            using (var enumerator = input.GetEnumerator())
            {
                while (enumerator.MoveNext())
                {
                    delimitedValues.Append(FormattableString.Invariant($"{options.StringConverter(enumerator.Current)}{options.Delimiter}"));
                }
            }
            return delimitedValues.Length > 0 ? delimitedValues.ToString(0, delimitedValues.Length - options.Delimiter.Length) : delimitedValues.ToString();
        }
    }
}