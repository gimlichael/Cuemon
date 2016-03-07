using System;

namespace Cuemon
{
    /// <summary>
    /// This utility class is designed to make <see cref="DateTime"/> operations easier to work with.
    /// </summary>
    public static class DateTimeUtility
    {
        /// <summary>
        /// Returns a <see cref="DateTime"/> value that is rounded towards negative infinity.
        /// </summary>
        /// <param name="value">A <see cref="DateTime"/> value to be rounded.</param>
        /// <param name="interval">The <see cref="TimeSpan"/> value that specifies the rounding of <paramref name="value"/>.</param>
        /// <returns>A <see cref="DateTime"/> value that is rounded towards negative infinity.</returns>
        public static DateTime Floor(DateTime value, TimeSpan interval)
        {
            return Round(value, interval, VerticalDirection.Down);
        }

        /// <summary>
        /// Returns a <see cref="DateTime"/> value that is rounded towards positive infinity.
        /// </summary>
        /// <param name="value">A <see cref="DateTime"/> value to be rounded.</param>
        /// <param name="interval">The <see cref="TimeSpan"/> value that specifies the rounding of <paramref name="value"/>.</param>
        /// <returns>A <see cref="DateTime"/> value that is rounded towards positive infinity.</returns>
        public static DateTime Ceiling(DateTime value, TimeSpan interval)
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
        public static DateTime Floor(DateTime value, double interval, TimeUnit timeUnit)
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
        public static DateTime Ceiling(DateTime value, double interval, TimeUnit timeUnit)
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
        public static DateTime Round(DateTime value, double interval, TimeUnit timeUnit, VerticalDirection direction)
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
        public static DateTime Round(DateTime value, TimeSpan interval, VerticalDirection direction)
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
    }
}