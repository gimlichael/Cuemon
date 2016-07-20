using System;

namespace Cuemon
{
    /// <summary>
    /// This is an extension implementation of the <see cref="DoubleConverter"/> class.
    /// </summary>
    public static class DoubleConverterExtensions
    {
        /// <summary>
        /// Converts the specified <paramref name="value"/> to an equivalent Epoc time representation.
        /// </summary>
        /// <param name="value">The <see cref="DateTime"/> value to be converted.</param>
        /// <returns>A <see cref="double"/> value that is equivalent to <paramref name="value"/>.</returns>
        /// <remarks>This implementation converts the <paramref name="value"/> to an UTC representation ONLY if the <see cref="DateTime.Kind"/> eqauls <see cref="DateTimeKind.Local"/>.</remarks>
        public static double ToEpochTime(this DateTime value)
        {
            return DoubleConverter.FromEpochTime(value);
        }

        /// <summary>
        /// Converts the specified <paramref name="value"/> to an equivalent Epoc time representation.
        /// </summary>
        /// <param name="value">The <see cref="DateTime"/> value to be converted.</param>
        /// <param name="utcConverter">The function delegate that will convert the given <paramref name="value"/> to its UTC equivalent.</param>
        /// <returns>A <see cref="double"/> value that is equivalent to <paramref name="value"/>.</returns>
        public static double ToEpochTime(this DateTime value, Doer<DateTime, DateTime> utcConverter)
        {
            return DoubleConverter.FromEpochTime(value, utcConverter);
        }
    }
}