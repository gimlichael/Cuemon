using System;
using Cuemon.ComponentModel.Converters;

namespace Cuemon.ComponentModel.TypeConverters
{
    /// <summary>
    /// Provides a converter that converts a <see cref="TimeSpan"/> to its equivalent <see cref="double"/>.
    /// </summary>
    public class TotalNanosecondsTimeSpanConverter : IConverter<TimeSpan, double>
    {
        /// <summary>
        /// Represents the number of ticks in 1 nanosecond. This field is constant.
        /// </summary>
        public const double TicksPerNanosecond = 0.01;

        /// <summary>
        /// Converts the specified <paramref name="input"/> to a <see cref="double"/> that represents the total number of nanoseconds.
        /// </summary>
        /// <param name="input">The <see cref="TimeSpan"/> to be converted into a <see cref="double"/>.</param>
        /// <returns>A <see cref="double"/> that represents the total number of nanoseconds from the specified <paramref name="input"/>.</returns>
        public double ChangeType(TimeSpan input)
        {
            return input.Ticks / TicksPerNanosecond;
        }
    }
}