using System;
using Cuemon.ComponentModel;
using Cuemon.ComponentModel.TypeConverters;

namespace Cuemon.Extensions
{
    /// <summary>
    /// Extension methods for the <see cref="TimeSpan"/> struct.
    /// </summary>
    public static class TimeSpanExtensions
    {
        /// <summary>
        /// Gets the total number of nanoseconds represented by the specified <see cref="TimeSpan"/> structure.
        /// </summary>
        /// <param name="value">The <see cref="TimeSpan"/> to extend.</param>
        /// <returns>The total number of nanoseconds represented by the specified <see cref="TimeSpan"/> structure.</returns>
        /// <seealso cref="TotalNanosecondsTimeSpanConverter"/>
        public static double GetTotalNanoseconds(this TimeSpan value)
        {
            return ConvertFactory.UseConverter<TotalNanosecondsTimeSpanConverter>().ChangeType(value);
        }

        /// <summary>
        /// Gets the total number of microseconds represented by the specified <see cref="TimeSpan"/> structure.
        /// </summary>
        /// <param name="value">The <see cref="TimeSpan"/> to extend.</param>
        /// <returns>The total number of microseconds represented by the specified <see cref="TimeSpan"/> structure.</returns>
        /// <seealso cref="TotalMicrosecondsTimeSpanConverter"/>
        public static double GetTotalMicroseconds(this TimeSpan value)
        {
            return ConvertFactory.UseConverter<TotalMicrosecondsTimeSpanConverter>().ChangeType(value);
        }

        /// <summary>
        /// Returns a <see cref="TimeSpan"/> value that is rounded towards negative infinity.
        /// </summary>
        /// <param name="value">A <see cref="TimeSpan"/> value to be rounded.</param>
        /// <param name="interval">The <see cref="TimeSpan"/> value that specifies the rounding of <paramref name="value"/>.</param>
        /// <returns>A <see cref="TimeSpan"/> value that is rounded towards negative infinity.</returns>
        public static TimeSpan Floor(this TimeSpan value, TimeSpan interval)
        {
            return Round(value, interval, VerticalDirection.Down);
        }

        /// <summary>
        /// Returns a <see cref="TimeSpan"/> value that is rounded towards positive infinity.
        /// </summary>
        /// <param name="value">A <see cref="TimeSpan"/> value to be rounded.</param>
        /// <param name="interval">The <see cref="TimeSpan"/> value that specifies the rounding of <paramref name="value"/>.</param>
        /// <returns>A <see cref="TimeSpan"/> value that is rounded towards positive infinity.</returns>
        public static TimeSpan Ceiling(this TimeSpan value, TimeSpan interval)
        {
            return Round(value, interval, VerticalDirection.Up);
        }

        /// <summary>
        /// Returns a <see cref="TimeSpan"/> value that is rounded towards negative infinity.
        /// </summary>
        /// <param name="value">A <see cref="TimeSpan"/> value to be rounded.</param>
        /// <param name="interval">The <see cref="double"/> value that in combination with <paramref name="timeUnit"/> specifies the rounding of <paramref name="value"/>.</param>
        /// <param name="timeUnit">One of the enumeration values that specifies the time unit of <paramref name="interval"/>.</param>
        /// <returns>A <see cref="TimeSpan"/> value that is rounded towards negative infinity.</returns>
        public static TimeSpan Floor(this TimeSpan value, double interval, TimeUnit timeUnit)
        {
            return Round(value, interval, timeUnit, VerticalDirection.Down);
        }

        /// <summary>
        /// Returns a <see cref="TimeSpan"/> value that is rounded towards positive infinity.
        /// </summary>
        /// <param name="value">A <see cref="TimeSpan"/> value to be rounded.</param>
        /// <param name="interval">The <see cref="double"/> value that in combination with <paramref name="timeUnit"/> specifies the rounding of <paramref name="value"/>.</param>
        /// <param name="timeUnit">One of the enumeration values that specifies the time unit of <paramref name="interval"/>.</param>
        /// <returns>A <see cref="TimeSpan"/> value that is rounded towards positive infinity.</returns>
        public static TimeSpan Ceiling(this TimeSpan value, double interval, TimeUnit timeUnit)
        {
            return Round(value, interval, timeUnit, VerticalDirection.Up);
        }

        /// <summary>
        /// Returns a <see cref="TimeSpan"/> value that is rounded either towards negative infinity or positive infinity.
        /// </summary>
        /// <param name="value">A <see cref="TimeSpan"/> value to be rounded.</param>
        /// <param name="interval">The <see cref="double"/> value that in combination with <paramref name="timeUnit"/> specifies the rounding of <paramref name="value"/>.</param>
        /// <param name="timeUnit">One of the enumeration values that specifies the time unit of <paramref name="interval"/>.</param>
        /// <param name="direction">One of the enumeration values that specifies the direction of the rounding.</param>
        /// <returns>A <see cref="TimeSpan"/> value that is rounded either towards negative infinity or positive infinity.</returns>
        /// <exception cref="ArgumentOutOfRangeException">
        /// <paramref name="direction"/> is an invalid enumeration value.
        /// </exception>
        /// <seealso cref="CompositeDoubleConverter"/>
        public static TimeSpan Round(this TimeSpan value, double interval, TimeUnit timeUnit, VerticalDirection direction)
        {
            return Round(value, ConvertFactory.UseConverter<CompositeDoubleConverter>().ChangeType(interval, o => o.TimeUnit = timeUnit), direction);
        }

        /// <summary>
        /// Returns a <see cref="TimeSpan"/> value that is rounded either towards negative infinity or positive infinity.
        /// </summary>
        /// <param name="value">A <see cref="TimeSpan"/> value to be rounded.</param>
        /// <param name="interval">The <see cref="TimeSpan"/> value that specifies the rounding of <paramref name="value"/>.</param>
        /// <param name="direction">One of the enumeration values that specifies the direction of the rounding.</param>
        /// <returns>A <see cref="TimeSpan"/> value that is rounded either towards negative infinity or positive infinity.</returns>
        /// <exception cref="ArgumentOutOfRangeException">
        /// <paramref name="direction"/> is an invalid enumeration value.
        /// </exception>
        public static TimeSpan Round(this TimeSpan value, TimeSpan interval, VerticalDirection direction)
        {
            long valueTicks = interval < TimeSpan.Zero ? value.Add(interval).Ticks : value.Ticks;
            long absoluteIntervalTicks = Math.Abs(interval.Ticks);
            long remainder = valueTicks % absoluteIntervalTicks;
            switch (direction)
            {
                case VerticalDirection.Up:
                    long adjustment = (absoluteIntervalTicks - (remainder)) % absoluteIntervalTicks;
                    return new TimeSpan(valueTicks + adjustment);
                case VerticalDirection.Down:
                    return new TimeSpan(valueTicks - remainder);
                default:
                    throw new ArgumentOutOfRangeException(nameof(direction));
            }
        }
    }
}