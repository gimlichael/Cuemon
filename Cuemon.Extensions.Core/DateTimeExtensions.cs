using System;

namespace Cuemon.Extensions
{
    /// <summary>
    /// Extension methods for the <see cref="DateTime"/> struct.
    /// </summary>
    public static class DateTimeExtensions
    {
        /// <summary>
        /// Converts the specified <paramref name="value"/> to a Coordinated Universal Time (UTC) representation.
        /// </summary>
        /// <param name="value">The value to convert.</param>
        /// <returns>A new <see cref="DateTime"/> value initialized to <see cref="DateTimeKind.Utc"/> that has the same number of ticks as the object represented by the <paramref name="value"/> parameter.</returns>
        public static DateTime ToUtcKind(this DateTime value)
        {
            return ToKind(value, DateTimeKind.Utc);
        }

        /// <summary>
        /// Converts the specified <paramref name="value"/> to a local time representation.
        /// </summary>
        /// <param name="value">The value to convert.</param>
        /// <returns>A new <see cref="DateTime"/> value initialized to <see cref="DateTimeKind.Local"/> that has the same number of ticks as the object represented by the <paramref name="value"/> parameter.</returns>
        public static DateTime ToLocalKind(this DateTime value)
        {
            return ToKind(value, DateTimeKind.Local);
        }

        /// <summary>
        /// Converts the specified <paramref name="value"/> to a representation that is not specified as either local time or UTC.
        /// </summary>
        /// <param name="value">The value to convert.</param>
        /// <returns>A new <see cref="DateTime"/> value initialized to <see cref="DateTimeKind.Unspecified"/> that has the same number of ticks as the object represented by the <paramref name="value"/> parameter.</returns>
        public static DateTime ToDefaultKind(this DateTime value)
        {
            return ToKind(value, DateTimeKind.Unspecified);
        }

        private static DateTime ToKind(DateTime value, DateTimeKind kind)
        {
            if (value.Kind != kind) { value = DateTime.SpecifyKind(value, kind); }
            return value;
        }

        /// <summary>
        /// Determines whether the specified <paramref name="value"/> is within range of <paramref name="min"/> and <paramref name="max"/>.
        /// </summary>
        /// <param name="value">The <see cref="DateTime"/> to compare.</param>
        /// <param name="min">The minimum value of <paramref name="value"/>.</param>
        /// <param name="max">The maximum value of <paramref name="value"/>.</param>
        /// <returns><c>true</c> if <paramref name="value"/> is within the specified range of <paramref name="min"/> and <paramref name="max"/>; otherwise <c>false</c>.</returns>
        public static bool IsWithinRange(this DateTime value, DateTime min, DateTime max)
        {
            return Condition.IsWithinRange(value, min, max);
        }

        /// <summary>
        /// Determines whether the specified <paramref name="value"/> is within <paramref name="range"/>.
        /// </summary>
        /// <param name="value">The <see cref="DateTime"/> to compare.</param>
        /// <param name="range">The <see cref="TimeRange"/> of <paramref name="value"/>.</param>
        /// <returns><c>true</c> if <paramref name="value"/> is within the specified <paramref name="range"/>; otherwise <c>false</c>.</returns>
        public static bool IsWithinRange(this DateTime value, TimeRange range)
        {
            return value.IsWithinRange(range.Start, range.End);
        }

        /// <summary>
        /// Determines whether the specified <paramref name="value"/> is within <see cref="DayParts.Night"/>.
        /// </summary>
        /// <param name="value">The <see cref="DateTime"/> to compare.</param>
        /// <returns><c>true</c> if <paramref name="value"/> is within <see cref="DayParts.Night"/>; otherwise <c>false</c>.</returns>
        public static bool IsTimeOfDayNight(this DateTime value)
        {
            return value.ToUtcKind().IsWithinRange(DayParts.Night.Range.Start, DayParts.Night.Range.End);
        }

        /// <summary>
        /// Determines whether the specified <paramref name="value"/> is within <see cref="DayParts.Morning"/>.
        /// </summary>
        /// <param name="value">The <see cref="DateTime"/> to compare.</param>
        /// <returns><c>true</c> if <paramref name="value"/> is within <see cref="DayParts.Morning"/>; otherwise <c>false</c>.</returns>
        public static bool IsTimeOfDayMorning(this DateTime value)
        {
            return value.ToUtcKind().IsWithinRange(DayParts.Morning.Range.Start, DayParts.Morning.Range.End);
        }

        /// <summary>
        /// Determines whether the specified <paramref name="value"/> is within <see cref="DayParts.Forenoon"/>.
        /// </summary>
        /// <param name="value">The <see cref="DateTime"/> to compare.</param>
        /// <returns><c>true</c> if <paramref name="value"/> is within <see cref="DayParts.Forenoon"/>; otherwise <c>false</c>.</returns>
        public static bool IsTimeOfDayForenoon(this DateTime value)
        {
            return value.ToUtcKind().IsWithinRange(DayParts.Forenoon.Range.Start, DayParts.Forenoon.Range.End);
        }

        /// <summary>
        /// Determines whether the specified <paramref name="value"/> is within <see cref="DayParts.Afternoon"/>.
        /// </summary>
        /// <param name="value">The <see cref="DateTime"/> to compare.</param>
        /// <returns><c>true</c> if <paramref name="value"/> is within <see cref="DayParts.Afternoon"/>; otherwise <c>false</c>.</returns>
        public static bool IsTimeOfDayAfternoon(this DateTime value)
        {
            return value.ToUtcKind().IsWithinRange(DayParts.Afternoon.Range.Start, DayParts.Afternoon.Range.End);
        }

        /// <summary>
        /// Determines whether the specified <paramref name="value"/> is within <see cref="DayParts.Evening"/>.
        /// </summary>
        /// <param name="value">The <see cref="DateTime"/> to compare.</param>
        /// <returns><c>true</c> if <paramref name="value"/> is within <see cref="DayParts.Evening"/>; otherwise <c>false</c>.</returns>
        public static bool IsTimeOfDayEvening(this DateTime value)
        {
            return value.ToUtcKind().IsWithinRange(DayParts.Evening.Range.Start, DayParts.Evening.Range.End);
        }

        /// <summary>
        /// Returns a <see cref="DateTime"/> value that is rounded towards negative infinity.
        /// </summary>
        /// <param name="value">A <see cref="DateTime"/> value to be rounded.</param>
        /// <param name="interval">The <see cref="TimeSpan"/> value that specifies the rounding of <paramref name="value"/>.</param>
        /// <returns>A <see cref="DateTime"/> value that is rounded towards negative infinity.</returns>
        public static DateTime Floor(this DateTime value, TimeSpan interval)
        {
            return Round(value, interval, VerticalDirection.Down);
        }

        /// <summary>
        /// Returns a <see cref="DateTime"/> value that is rounded towards positive infinity.
        /// </summary>
        /// <param name="value">A <see cref="DateTime"/> value to be rounded.</param>
        /// <param name="interval">The <see cref="TimeSpan"/> value that specifies the rounding of <paramref name="value"/>.</param>
        /// <returns>A <see cref="DateTime"/> value that is rounded towards positive infinity.</returns>
        public static DateTime Ceiling(this DateTime value, TimeSpan interval)
        {
            return Round(value, interval, VerticalDirection.Up);
        }

        /// <summary>
        /// Returns a <see cref="DateTime"/> value that is rounded towards negative infinity.
        /// </summary>
        /// <param name="value">A <see cref="DateTime"/> value to be rounded.</param>
        /// <param name="interval">The <see cref="double"/> value that in combination with <paramref name="timeUnit"/> specifies the rounding of <paramref name="value"/>.</param>
        /// <param name="timeUnit">One of the enumeration values that specifies the time unit of <paramref name="interval"/>.</param>
        /// <returns>A <see cref="DateTime"/> value that is rounded towards negative infinity.</returns>
        /// <exception cref="ArgumentOutOfRangeException">
        /// <paramref name="interval"/> is 0.
        /// </exception>
        public static DateTime Floor(this DateTime value, double interval, TimeUnit timeUnit)
        {
            return Round(value, interval, timeUnit, VerticalDirection.Down);
        }

        /// <summary>
        /// Returns a <see cref="DateTime"/> value that is rounded towards positive infinity.
        /// </summary>
        /// <param name="value">A <see cref="DateTime"/> value to be rounded.</param>
        /// <param name="interval">The <see cref="double"/> value that in combination with <paramref name="timeUnit"/> specifies the rounding of <paramref name="value"/>.</param>
        /// <param name="timeUnit">One of the enumeration values that specifies the time unit of <paramref name="interval"/>.</param>
        /// <returns>A <see cref="DateTime"/> value that is rounded towards positive infinity.</returns>
        /// <exception cref="ArgumentOutOfRangeException">
        /// <paramref name="interval"/> is 0.
        /// </exception>
        public static DateTime Ceiling(this DateTime value, double interval, TimeUnit timeUnit)
        {
            return Round(value, interval, timeUnit, VerticalDirection.Up);
        }

        /// <summary>
        /// Returns a <see cref="DateTime"/> value that is rounded either towards negative infinity or positive infinity.
        /// </summary>
        /// <param name="value">A <see cref="DateTime"/> value to be rounded.</param>
        /// <param name="interval">The <see cref="double"/> value that in combination with <paramref name="timeUnit"/> specifies the rounding of <paramref name="value"/>.</param>
        /// <param name="timeUnit">One of the enumeration values that specifies the time unit of <paramref name="interval"/>.</param>
        /// <param name="direction">One of the enumeration values that specifies the direction of the rounding.</param>
        /// <returns>A <see cref="DateTime"/> value that is rounded either towards negative infinity or positive infinity.</returns>
        /// <exception cref="ArgumentOutOfRangeException">
        /// <paramref name="direction"/> is an invalid enumeration value.
        /// </exception>
        /// <exception cref="ArgumentOutOfRangeException">
        /// <paramref name="interval"/> is <see cref="TimeSpan.Zero"/>.
        /// </exception>
        public static DateTime Round(this DateTime value, double interval, TimeUnit timeUnit, VerticalDirection direction)
        {
            return Round(value, TimeSpanConverter.FromDouble(interval, timeUnit), direction);
        }

        /// <summary>
        /// Returns a <see cref="DateTime"/> value that is rounded either towards negative infinity or positive infinity.
        /// </summary>
        /// <param name="value">A <see cref="DateTime"/> value to be rounded.</param>
        /// <param name="interval">The <see cref="TimeSpan"/> value that specifies the rounding of <paramref name="value"/>.</param>
        /// <param name="direction">One of the enumeration values that specifies the direction of the rounding.</param>
        /// <returns>A <see cref="DateTime"/> value that is rounded either towards negative infinity or positive infinity.</returns>
        /// <exception cref="ArgumentOutOfRangeException">
        /// <paramref name="direction"/> is an invalid enumeration value.
        /// </exception>
        /// <exception cref="ArgumentOutOfRangeException">
        /// <paramref name="interval"/> is <see cref="TimeSpan.Zero"/>.
        /// </exception>
        public static DateTime Round(this DateTime value, TimeSpan interval, VerticalDirection direction)
        {
            Validator.ThrowIfEqual(interval, TimeSpan.Zero, nameof(interval));
            long datetTimeTicks = interval < TimeSpan.Zero ? value.Add(interval).Ticks : value.Ticks;
            long absoluteIntervalTicks = Math.Abs(interval.Ticks);
            long remainder = datetTimeTicks % absoluteIntervalTicks;
            switch (direction)
            {
                case VerticalDirection.Up:
                    long adjustment = (absoluteIntervalTicks - (remainder)) % absoluteIntervalTicks;
                    return new DateTime(datetTimeTicks + adjustment, value.Kind);
                case VerticalDirection.Down:
                    return new DateTime(datetTimeTicks - remainder, value.Kind);
                default:
                    throw new ArgumentOutOfRangeException(nameof(direction));
            }
        }

        /// <summary>
        /// Converts the specified <paramref name="value"/> to an equivalent Epoc time representation.
        /// </summary>
        /// <param name="value">The <see cref="DateTime"/> value to be converted.</param>
        /// <returns>A <see cref="double"/> value that is equivalent to <paramref name="value"/>.</returns>
        /// <remarks>This implementation converts the <paramref name="value"/> to an UTC representation ONLY if the <see cref="DateTime.Kind"/> equals <see cref="DateTimeKind.Local"/>.</remarks>
        public static double ToEpochTime(this DateTime value)
        {
            return DoubleConverter.FromEpochTime(value);
        }

        /// <summary>
        /// Converts the specified <paramref name="value"/> to an equivalent Epoc time representation.
        /// </summary>
        /// <param name="value">The <see cref="DateTime"/> value to be converted.</param>
        /// <param name="utcConverter">The function delegate that will convert the given <paramref name="value"/> to its UTC equivalent.</param>
        /// <returns>A <see cref="double"/> value that is equivalent to <paramref name="value"/>.</returns>
        public static double ToEpochTime(this DateTime value, Func<DateTime, DateTime> utcConverter)
        {
            return DoubleConverter.FromEpochTime(value, utcConverter);
        }
    }
}