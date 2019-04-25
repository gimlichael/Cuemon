using System;
using System.Globalization;

namespace Cuemon
{
    /// <summary>
    /// This utility class is designed to make <see cref="TimeSpan"/> related conversions easier to work with.
    /// </summary>
    public static class TimeSpanConverter
    {
        /// <summary>
        /// Converts the specified <paramref name="value"/> to its equivalent <see cref="TimeSpan"/> representation.
        /// </summary>
        /// <param name="value">The value to be converted.</param>
        /// <param name="timeUnit">One of the enumeration values that specifies the outcome of the conversion.</param>
        /// <returns>A <see cref="TimeSpan"/> that corresponds to <paramref name="value"/> from <paramref name="timeUnit"/>.</returns>
        /// <exception cref="OverflowException">
        /// The <paramref name="value"/> paired with <paramref name="timeUnit"/> is outside its valid range.
        /// </exception>
        /// <exception cref="ArgumentOutOfRangeException">
        /// <paramref name="timeUnit"/> was outside its valid range.
        /// </exception>
        public static TimeSpan FromDouble(double value, TimeUnit timeUnit)
        {
            if (value == 0.0) { return TimeSpan.Zero; }
            switch (timeUnit)
            {
                case TimeUnit.Days:
                    return TimeSpan.FromDays(value);
                case TimeUnit.Hours:
                    return TimeSpan.FromHours(value);
                case TimeUnit.Minutes:
                    return TimeSpan.FromMinutes(value);
                case TimeUnit.Seconds:
                    return TimeSpan.FromSeconds(value);
                case TimeUnit.Milliseconds:
                    return TimeSpan.FromMilliseconds(value);
                case TimeUnit.Ticks:
                    if (value < long.MinValue || value > long.MaxValue) { throw new OverflowException(string.Format(CultureInfo.InvariantCulture, "The specified value, {0}, having a time unit specified as Ticks cannot be less than {1} or be greater than {2}.", value, long.MinValue, long.MaxValue)); }
                    return TimeSpan.FromTicks((long)value);
                default:
                    throw new ArgumentOutOfRangeException(nameof(timeUnit));
            }
        }

        /// <summary>
        /// Converts the specified <paramref name="value"/> to its equivalent <see cref="TimeSpan"/> representation.
        /// </summary>
        /// <param name="value">The string to be converted.</param>
        /// <param name="timeUnit">One of the enumeration values that specifies the outcome of the conversion.</param>
        /// <returns>A <see cref="TimeSpan"/> that corresponds to <paramref name="value"/> from <paramref name="timeUnit"/>.</returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="value"/> is null.
        /// </exception>
        /// <exception cref="OverflowException">
        /// The <paramref name="value"/> paired with <paramref name="timeUnit"/> is outside its valid range.
        /// </exception>
        /// <exception cref="ArgumentOutOfRangeException">
        /// <paramref name="timeUnit"/> was outside its valid range.
        /// </exception>
        public static TimeSpan FromString(string value, TimeUnit timeUnit)
        {
            Validator.ThrowIfNullOrWhitespace(value, nameof(value));
            return FromDouble(Convert.ToDouble(value, CultureInfo.InvariantCulture), timeUnit);
        }
    }
}