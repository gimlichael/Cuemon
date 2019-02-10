using System;

namespace Cuemon
{
    /// <summary>
    /// This utility class is designed to make <see cref="double"/> related conversions easier to work with.
    /// </summary>
    public static class DoubleConverter
    {
        /// <summary>
        /// Converts the specified <paramref name="value"/> to an equivalent Epoc time representation.
        /// </summary>
        /// <param name="value">The <see cref="DateTime"/> value to be converted.</param>
        /// <returns>A <see cref="double"/> value that is equivalent to <paramref name="value"/>.</returns>
        /// <remarks>This implementation converts the <paramref name="value"/> to an UTC representation ONLY if the <see cref="DateTime.Kind"/> equals <see cref="DateTimeKind.Local"/>.</remarks>
        public static double FromEpochTime(DateTime value)
        {
            return FromEpochTime(value, dateTime =>
            {
                if (dateTime.Kind == DateTimeKind.Local) { return dateTime.ToUniversalTime(); }
                return dateTime;
            });
        }

        /// <summary>
        /// Converts the specified <paramref name="value"/> to an equivalent Epoc time representation.
        /// </summary>
        /// <param name="value">The <see cref="DateTime"/> value to be converted.</param>
        /// <param name="utcConverter">The function delegate that will convert the given <paramref name="value"/> to its UTC equivalent.</param>
        /// <returns>A <see cref="double"/> value that is equivalent to <paramref name="value"/>.</returns>
        public static double FromEpochTime(DateTime value, Func<DateTime, DateTime> utcConverter)
        {
            Validator.ThrowIfLowerThan(value, DateTimeConverter.UnixDate, nameof(value));
            Validator.ThrowIfNull(utcConverter, nameof(utcConverter));
            return Math.Floor((utcConverter(value) - DateTimeConverter.UnixDate).TotalSeconds);
        }
    }
}