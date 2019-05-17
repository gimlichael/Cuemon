using System;

namespace Cuemon.ComponentModel.TypeConverters
{
    /// <summary>
    /// Provides a converter that converts a <see cref="TimeSpan"/> to its equivalent <see cref="double"/>.
    /// </summary>
    public class TotalMicrosecondsTimeSpanConverter : IConverter<TimeSpan, double>
    {
        /// <summary>
        /// Represents the number of ticks in 1 microsecond. This field is constant.
        /// </summary>
        public const double TicksPerMicrosecond = TotalNanosecondsTimeSpanConverter.TicksPerNanosecond * 1000;

        /// <summary>
        /// Converts the specified <paramref name="input"/> to a <see cref="double"/> that represents the total number of microseconds.
        /// </summary>
        /// <param name="input">The <see cref="TimeSpan"/> to be converted into a <see cref="double"/>.</param>
        /// <returns>A <see cref="double"/> that represents the total number of microseconds from the specified <paramref name="input"/>.</returns>
        public double ChangeType(TimeSpan input)
        {
            return input.Ticks / TicksPerMicrosecond;
        }
    }
}