using System;

namespace Cuemon
{
    /// <summary>
    /// This is an extension implementation of the most common methods on the <see cref="DateTimeUtility"/> class.
    /// </summary>
    public static class DateTimeUtilityExtension
    {
        /// <summary>
        /// Returns a <see cref="DateTime"/> value that is rounded towards negative infinity.
        /// </summary>
        /// <param name="value">A <see cref="DateTime"/> value to be rounded.</param>
        /// <param name="interval">The <see cref="TimeSpan"/> value that specifies the rounding of <paramref name="value"/>.</param>
        /// <returns>A <see cref="DateTime"/> value that is rounded towards negative infinity.</returns>
        public static DateTime Floor(this DateTime value, TimeSpan interval)
        {
            return DateTimeUtility.Floor(value, interval);
        }

        /// <summary>
        /// Returns a <see cref="DateTime"/> value that is rounded towards positive infinity.
        /// </summary>
        /// <param name="value">A <see cref="DateTime"/> value to be rounded.</param>
        /// <param name="interval">The <see cref="TimeSpan"/> value that specifies the rounding of <paramref name="value"/>.</param>
        /// <returns>A <see cref="DateTime"/> value that is rounded towards positive infinity.</returns>
        public static DateTime Ceiling(this DateTime value, TimeSpan interval)
        {
            return DateTimeUtility.Ceiling(value, interval);
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
            return DateTimeUtility.Floor(value, interval, timeUnit);
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
            return DateTimeUtility.Ceiling(value, interval, timeUnit);
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
            return DateTimeUtility.Round(value, interval, timeUnit, direction);
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
            return DateTimeUtility.Round(value, interval, direction);
        }
    }
}