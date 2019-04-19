using System;
using System.Collections.Generic;

namespace Cuemon
{
    /// <summary>
    /// Extension methods for the <see cref="char"/> struct.
    /// </summary>
    public static class CharExtensions
    {
        /// <summary>
        /// Converts the specified <paramref name="value"/> to its equivalent <see cref="String"/> sequence.
        /// </summary>
        /// <param name="value">The value to convert into a sequence.</param>
        /// <returns>A <see cref="String"/> sequence equivalent to the specified <paramref name="value"/>.</returns>
        public static IEnumerable<string> ToEnumerable(this IEnumerable<char> value)
        {
            return StringConverter.ToEnumerable(value);
        }

        /// <summary>
        /// Converts the specified <paramref name="value"/> to its equivalent <see cref="String"/> representation.
        /// </summary>
        /// <param name="value">The <see cref="Char"/> sequence to convert.</param>
        /// <returns>A <see cref="String"/> equivalent to the specified <paramref name="value"/>.</returns>
        public static string FromChars(this IEnumerable<char> value)
        {
            return StringConverter.FromChars(value);
        }
    }
}