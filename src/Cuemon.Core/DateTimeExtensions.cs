using System;

namespace Cuemon
{
    /// <summary>
    /// This is an extensions implementation of the <see cref="DateTime"/> structure using methods already found in the Microsoft .NET Framework.
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
        /// Determines whether the specified <paramref name="dt"/> is within range of <paramref name="min"/> and <paramref name="max"/>.
        /// </summary>
        /// <param name="dt">The <see cref="DateTime"/> to compare.</param>
        /// <param name="min">The minimum value of <paramref name="dt"/>.</param>
        /// <param name="max">The maximum value of <paramref name="dt"/>.</param>
        /// <returns><c>true</c> if <paramref name="dt"/> is within the specified range of <paramref name="min"/> and <paramref name="max"/>; otherwise <c>false</c>.</returns>
        public static bool IsWithinRange(this DateTime dt, DateTime min, DateTime max)
        {
            return Condition.IsWithinRange(dt, min, max);
        }

        /// <summary>
        /// Determines whether the specified <paramref name="dt"/> is within <paramref name="range"/>.
        /// </summary>
        /// <param name="dt">The <see cref="DateTime"/> to compare.</param>
        /// <param name="range">The <see cref="TimeRange"/> of <paramref name="dt"/>.</param>
        /// <returns><c>true</c> if <paramref name="dt"/> is within the specified <paramref name="range"/>; otherwise <c>false</c>.</returns>
        public static bool IsWithinRange(this DateTime dt, TimeRange range)
        {
            return dt.IsWithinRange(range.Start, range.End);
        }

        /// <summary>
        /// Determines whether the specified <paramref name="dt"/> is within <see cref="DayParts.Night"/>.
        /// </summary>
        /// <param name="dt">The <see cref="DateTime"/> to compare.</param>
        /// <returns><c>true</c> if <paramref name="dt"/> is within <see cref="DayParts.Night"/>; otherwise <c>false</c>.</returns>
        public static bool IsTimeOfDayNight(this DateTime dt)
        {
            return dt.ToUtcKind().IsWithinRange(DayParts.Night.Range.Start, DayParts.Night.Range.End);
        }

        /// <summary>
        /// Determines whether the specified <paramref name="dt"/> is within <see cref="DayParts.Morning"/>.
        /// </summary>
        /// <param name="dt">The <see cref="DateTime"/> to compare.</param>
        /// <returns><c>true</c> if <paramref name="dt"/> is within <see cref="DayParts.Morning"/>; otherwise <c>false</c>.</returns>
        public static bool IsTimeOfDayMorning(this DateTime dt)
        {
            return dt.ToUtcKind().IsWithinRange(DayParts.Morning.Range.Start, DayParts.Morning.Range.End);
        }

        /// <summary>
        /// Determines whether the specified <paramref name="dt"/> is within <see cref="DayParts.Forenoon"/>.
        /// </summary>
        /// <param name="dt">The <see cref="DateTime"/> to compare.</param>
        /// <returns><c>true</c> if <paramref name="dt"/> is within <see cref="DayParts.Forenoon"/>; otherwise <c>false</c>.</returns>
        public static bool IsTimeOfDayForenoon(this DateTime dt)
        {
            return dt.ToUtcKind().IsWithinRange(DayParts.Forenoon.Range.Start, DayParts.Forenoon.Range.End);
        }

        /// <summary>
        /// Determines whether the specified <paramref name="dt"/> is within <see cref="DayParts.Afternoon"/>.
        /// </summary>
        /// <param name="dt">The <see cref="DateTime"/> to compare.</param>
        /// <returns><c>true</c> if <paramref name="dt"/> is within <see cref="DayParts.Afternoon"/>; otherwise <c>false</c>.</returns>
        public static bool IsTimeOfDayAfternoon(this DateTime dt)
        {
            return dt.ToUtcKind().IsWithinRange(DayParts.Afternoon.Range.Start, DayParts.Afternoon.Range.End);
        }

        /// <summary>
        /// Determines whether the specified <paramref name="dt"/> is within <see cref="DayParts.Evening"/>.
        /// </summary>
        /// <param name="dt">The <see cref="DateTime"/> to compare.</param>
        /// <returns><c>true</c> if <paramref name="dt"/> is within <see cref="DayParts.Evening"/>; otherwise <c>false</c>.</returns>
        public static bool IsTimeOfDayEvening(this DateTime dt)
        {
            return dt.ToUtcKind().IsWithinRange(DayParts.Evening.Range.Start, DayParts.Evening.Range.End);
        }
    }
}