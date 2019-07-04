using System.Collections.Generic;
using System.Linq;
using Cuemon.ComponentModel.Converters;

namespace Cuemon.ComponentModel.TypeConverters
{
    public class TextEnumerableConverter : IConverter<IEnumerable<char>, IEnumerable<string>>
    {

        /// <summary>
        /// Converts the specified <paramref name="value"/> to its equivalent <see cref="string"/> sequence.
        /// </summary>
        /// <param name="value">The value to convert into a sequence.</param>
        /// <returns>A <see cref="string"/> sequence equivalent to the specified <paramref name="value"/>.</returns>
        public IEnumerable<string> ChangeType(IEnumerable<char> input)
        {
            Validator.ThrowIfNull(input, nameof(input));
            return input.Select(c => new string(c, 1));
        }
    }
}