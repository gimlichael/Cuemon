using System;

namespace Cuemon.Extensions.Core
{
    /// <summary>
    /// Extension methods for the <see cref="double"/> struct.
    /// </summary>
    public static class DoubleExtensions
    {
        /// <summary>
        /// Converts the specified <paramref name="value"/> to its equivalent <see cref="TimeSpan"/> representation.
        /// </summary>
        /// <param name="value">The value to be converted.</param>
        /// <param name="timeUnit">One of the enumeration values that specifies the outcome of the conversion.</param>
        /// <returns>A <see cref="TimeSpan"/> that corresponds to <paramref name="value"/> from <paramref name="timeUnit"/>.</returns>
        /// <exception cref="System.OverflowException">
        /// The <paramref name="value"/> paired with <paramref name="timeUnit"/> is outside its valid range.
        /// </exception>
        /// <exception cref="System.ArgumentOutOfRangeException">
        /// <paramref name="timeUnit"/> was outside its valid range.
        /// </exception>
        public static TimeSpan ToTimeSpan(this double value, TimeUnit timeUnit)
        {
            return TimeSpanConverter.FromDouble(value, timeUnit);
        }

        /// <summary>
        /// Converts the specified Epoc time <paramref name="value"/> to an equivalent <see cref="DateTime"/> representation.
        /// </summary>
        /// <param name="value">The <see cref="double"/> value to be converted.</param>
        /// <returns>A <see cref="DateTime"/> value that is equivalent to <paramref name="value"/>.</returns>
        public static DateTime FromEpochTime(this double value)
        {
            return DateTimeConverter.FromEpochTime(value);
        }

        /// <summary>
        /// Calculates the factorial of a positive integer <paramref name="n"/> denoted by n!.
        /// </summary>
        /// <param name="n">The positive integer to calculate a factorial number by.</param>
        /// <returns>The factorial number calculated from <paramref name="n"/>, or <see cref="double.PositiveInfinity"/> if <paramref name="n"/> is to high a value.</returns>
        public static double Factorial(this double n)
        {
            return NumberUtility.Factorial(n);
        }

        /// <summary>
        /// Rounds a double-precision floating-point value to the nearest integral value closest to the specified <paramref name="accuracy"/>.
        /// </summary>
        /// <param name="value">A double-precision floating-point number to be rounded.</param>
        /// <param name="accuracy">The accuracy to use in the rounding.</param>
        /// <returns>
        /// The integer value closest to the specified <paramref name="accuracy"/> of <paramref name="value"/>.<br/>
        /// Note that this method returns a <see cref="double"/> instead of an integral type.
        /// </returns>
        public static double RoundOff(this double value, RoundOffAccuracy accuracy)
        {
            return NumberUtility.RoundOff(value, accuracy);
        }
    }
}