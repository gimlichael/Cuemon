using System;

namespace Cuemon
{
    /// <summary>
    /// This utility class is designed to make <see cref="DateTime"/> related conversions easier to work with.
    /// </summary>
    public static class DateTimeConverter
    {
        internal static readonly DateTime UnixDate = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);

        /// <summary>
        /// Converts the specified Epoc time <paramref name="value"/> to an equivalent <see cref="DateTime"/> representation.
        /// </summary>
        /// <param name="value">The <see cref="double"/> value to be converted.</param>
        /// <returns>A <see cref="DateTime"/> value that is equivalent to <paramref name="value"/>.</returns>
        public static DateTime FromEpochTime(double value)
        {
            Validator.ThrowIfLowerThan(value, 0, nameof(value));
            return UnixDate.AddSeconds(value);
        }
    }
}