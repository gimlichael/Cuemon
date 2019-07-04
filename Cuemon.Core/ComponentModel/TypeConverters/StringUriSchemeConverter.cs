using System.Collections.Generic;
using System.Linq;
using Cuemon.Collections.Generic;
using Cuemon.ComponentModel.Converters;

namespace Cuemon.ComponentModel.TypeConverters
{
    /// <summary>
    /// Provides a converter that converts a <see cref="UriScheme"/> to its equivalent <see cref="string"/>.
    /// </summary>
    public class StringUriSchemeConverter : IConverter<UriScheme, string>
    {
        internal static readonly IDictionary<UriScheme, string> UriSchemeToStringLookupTable = UriSchemeStringConverter.StringToUriSchemeLookupTable.ToDictionary(pair => pair.Value, pair => pair.Key);

        /// <summary>
        /// Converts the specified <paramref name="input"/> to its equivalent <see cref="string"/> representation.
        /// </summary>
        /// <param name="input">The <see cref="UriScheme"/> to be converted into a <see cref="string"/>.</param>
        /// <returns>A <see cref="string"/> that is equivalent to <paramref name="input"/>.</returns>
        public string ChangeType(UriScheme input)
        {
            if (!UriSchemeToStringLookupTable.TryGetValue(input, out var result))
            {
                result = UriScheme.Undefined.ToString();
            }
            return result;
        }
    }
}