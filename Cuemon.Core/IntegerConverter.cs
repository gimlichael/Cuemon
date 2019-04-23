using System;

namespace Cuemon
{
    /// <summary>
    /// This utility class is designed to make <see cref="short"/>, <see cref="int"/> and <see cref="long"/> related conversions easier to work with.
    /// </summary>
    public static class IntegerConverter
    {
        /// <summary>
        /// Gets the nanoseconds of the time interval represented by the specified <see cref="TimeSpan"/> structure.
        /// </summary>
        /// <param name="value">The <see cref="TimeSpan"/> to extend with a nanoseconds component.</param>
        /// <returns>The nanoseconds component by the specified <see cref="TimeSpan"/> structure. The return value ranges from -999 through 999.</returns>
        public static int FromTimeSpanToNanoseconds(TimeSpan value)
        {
            return (int)(DoubleConverter.FromTimeSpanToTotalNanoseconds(value) % 1000);
        }

        /// <summary>
        /// Gets the microseconds of the time interval represented by the specified <see cref="TimeSpan"/> structure.
        /// </summary>
        /// <param name="value">The <see cref="TimeSpan"/> to extend with a microseconds component.</param>
        /// <returns>The millisecond component by the specified <see cref="TimeSpan"/> structure. The return value ranges from -999 through 999.</returns>
        public static int FromTimeSpanToMicroseconds(TimeSpan value)
        {
            return (int)(DoubleConverter.FromTimeSpanToTotalMicroseconds(value) % 1000);
        }
    }
}