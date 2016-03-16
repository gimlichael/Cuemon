using System;

namespace Cuemon
{
    /// <summary>
    /// This is an extension implementation of the <see cref="DateTimeConverter"/> class.
    /// </summary>
    public static class DateTimeConverterExtension
    {
        /// <summary>
        /// Converts the specified Epoc time <paramref name="value"/> to an equivalent <see cref="DateTime"/> representation.
        /// </summary>
        /// <param name="value">The <see cref="double"/> value to be converted.</param>
        /// <returns>A <see cref="DateTime"/> value that is equivalent to <paramref name="value"/>.</returns>
        public static DateTime FromEpochTime(this double value)
        {
            return DateTimeConverter.FromEpochTime(value);
        }
    }
}