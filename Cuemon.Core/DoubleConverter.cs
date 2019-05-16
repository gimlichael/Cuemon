using System;
using Cuemon.ComponentModel.TypeConverters;

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

    }
}