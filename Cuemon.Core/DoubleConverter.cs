using System;

namespace Cuemon
{
    /// <summary>
    /// This utility class is designed to make <see cref="double"/> related conversions easier to work with.
    /// </summary>
    public static class DoubleConverter
    {
        /// <summary>
        /// Represents the number of ticks in 1 nanosecond. This field is constant.
        /// </summary>
        public const double TicksPerNanosecond = 0.01;

        /// <summary>
        /// Represents the number of ticks in 1 microsecond. This field is constant.
        /// </summary>
        public const double TicksPerMicrosecond = TicksPerNanosecond * 1000;

        /// <summary>
        /// Gets the total number of nanoseconds represented by the specified <see cref="TimeSpan"/> structure.
        /// </summary>
        /// <param name="value">The <see cref="TimeSpan"/> to extend.</param>
        /// <returns>The total number of nanoseconds represented by the specified <see cref="TimeSpan"/> structure.</returns>
        public static double FromTimeSpanToTotalNanoseconds(TimeSpan value)
        {
            return value.Ticks / TicksPerNanosecond;
        }

        /// <summary>
        /// Gets the total number of microseconds represented by the specified <see cref="TimeSpan"/> structure.
        /// </summary>
        /// <param name="value">The <see cref="TimeSpan"/> to extend.</param>
        /// <returns>The total number of microseconds represented by the specified <see cref="TimeSpan"/> structure.</returns>
        public static double FromTimeSpanToTotalMicroseconds(TimeSpan value)
        {
            return value.Ticks / TicksPerMicrosecond;
        }

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