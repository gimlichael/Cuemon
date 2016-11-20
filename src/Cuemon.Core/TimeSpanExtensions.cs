using System;

namespace Cuemon
{
    /// <summary>
    /// Extension methods for the <see cref="TimeSpan"/> structure.
    /// </summary>
    public static class TimeSpanExtensions
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
        /// <param name="ts">The <see cref="TimeSpan"/> to extend.</param>
        /// <returns>The total number of nanoseconds represented by the specified <see cref="TimeSpan"/> structure.</returns>
        public static double GetTotalNanoseconds(this TimeSpan ts)
        {
            return ts.Ticks / TicksPerNanosecond;
        }

        /// <summary>
        /// Gets the nanoseconds of the time interval represented by the specified <see cref="TimeSpan"/> structure.
        /// </summary>
        /// <param name="ts">The <see cref="TimeSpan"/> to extend with a nanoseconds component.</param>
        /// <returns>The nanoseconds component by the specified <see cref="TimeSpan"/> structure. The return value ranges from -999 through 999.</returns>
        public static int GetNanoseconds(this TimeSpan ts)
        {
            return (int)(GetTotalNanoseconds(ts) % 1000);
        }

        /// <summary>
        /// Gets the total number of microseconds represented by the specified <see cref="TimeSpan"/> structure.
        /// </summary>
        /// <param name="ts">The <see cref="TimeSpan"/> to extend.</param>
        /// <returns>The total number of microseconds represented by the specified <see cref="TimeSpan"/> structure.</returns>
        public static double GetTotalMicroseconds(this TimeSpan ts)
        {
            return ts.Ticks / TicksPerMicrosecond;
        }

        /// <summary>
        /// Gets the microseconds of the time interval represented by the specified <see cref="TimeSpan"/> structure.
        /// </summary>
        /// <param name="ts">The <see cref="TimeSpan"/> to extend with a microseconds component.</param>
        /// <returns>The millisecond component by the specified <see cref="TimeSpan"/> structure. The return value ranges from -999 through 999.</returns>
        public static int GetMicroseconds(this TimeSpan ts)
        {
            return (int)(GetTotalMicroseconds(ts) % 1000);
        }
    }
}