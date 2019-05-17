using System;
using Cuemon.ComponentModel.TypeConverters;

namespace Cuemon.ComponentModel
{
    /// <summary>
    /// Extension methods for the <see cref="T:IConversion{System.TimeSpan}"/> interface.
    /// </summary>
    public static class TimeSpanTypeConverters
    {
        /// <summary>
        /// Converts the specified <paramref name="input"/> to a <see cref="double"/> that represents the total number of nanoseconds.
        /// </summary>
        /// <param name="_">The marker interface of a converter having <see cref="TimeSpan"/> as <paramref name="input"/>.</param>
        /// <param name="input">The <see cref="TimeSpan"/> to be converted into a <see cref="double"/>.</param>
        /// <returns>A <see cref="double"/> that represents the total number of nanoseconds from the specified <paramref name="input"/>.</returns>
        public static double ToTotalNanoseconds(this IConversion<TimeSpan> _, TimeSpan input)
        {
            return Converter<TimeSpan, double>.UseConverter<TotalNanosecondsTimeSpanConverter>(input);
        }

        /// <summary>
        /// Converts the specified <paramref name="input"/> to a <see cref="double"/> that represents the total number of microseconds.
        /// </summary>
        /// <param name="_">The marker interface of a converter having <see cref="TimeSpan"/> as <paramref name="input"/>.</param>
        /// <param name="input">The <see cref="TimeSpan"/> to be converted into a <see cref="double"/>.</param>
        /// <returns>A <see cref="double"/> that represents the total number of microseconds from the specified <paramref name="input"/>.</returns>
        public static double ToTotalMicroseconds(this IConversion<TimeSpan> _, TimeSpan input)
        {
            return Converter<TimeSpan, double>.UseConverter<TotalMicrosecondsTimeSpanConverter>(input);
        }
    }
}