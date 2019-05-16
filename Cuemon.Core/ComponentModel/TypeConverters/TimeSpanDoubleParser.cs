using System;
using System.ComponentModel;
using System.Globalization;

namespace Cuemon.ComponentModel.TypeConverters
{
    /// <summary>
    /// Provides a parser that converts a <see cref="string"/> to its equivalent <see cref="TimeSpan"/>.
    /// </summary>
    public class TimeSpanDoubleParser : IParser<TimeSpan, TimeSpanDoubleOptions>
    {
        /// <summary>
        /// Converts the string representation of a number to its <see cref="TimeSpan"/> equivalent.
        /// </summary>
        /// <param name="input">A string that contains the number to convert.</param>
        /// <param name="setup">The <see cref="TimeSpanDoubleOptions"/> which need to be configured.</param>
        /// <returns>A <see cref="TimeSpan"/> equivalent to <paramref name="input"/>.</returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="input"/> cannot be null.
        /// </exception>
        /// <exception cref="ArgumentException">
        /// <paramref name="input"/> cannot be empty or consist only of white-space characters.
        /// </exception>
        /// <exception cref="FormatException">
        /// <paramref name="input"/> is not a number in a valid format.
        /// </exception>
        /// <exception cref="OverflowException">
        /// <paramref name="input"/> represents a number that is less than <see cref="long.MinValue"/> or greater than <see cref="long.MaxValue"/>.
        /// </exception>
        /// <exception cref="InvalidEnumArgumentException">
        /// <paramref name="setup"/> was initialzied with an invalid <see cref="TimeUnit"/>.
        /// </exception>
        /// <seealso cref="Convert.ToDouble(string)"/>
        /// <seealso cref="TimeSpanDoubleConverter"/>
        /// <seealso cref="TimeSpanDoubleOptions"/>
        public TimeSpan Parse(string input, Action<TimeSpanDoubleOptions> setup)
        {
            Validator.ThrowIfNullOrWhitespace(input, nameof(input));
            return Converters.FromDouble.ToTimeSpan(Convert.ToDouble(input, CultureInfo.InvariantCulture), setup);
        }

        /// <summary>
        /// Converts the string representation of a number to its <see cref="TimeSpan"/> equivalent. A return value indicates whether the conversion succeeded.
        /// </summary>
        /// <param name="input">A string that contains the number to convert.</param>
        /// <param name="result">When this method returns, contains the <see cref="TimeSpan"/> equivalent of the <paramref name="input"/>, if the conversion succeeded, or <c>default</c> if the conversion failed.</param>
        /// <param name="setup">The <see cref="TimeSpanDoubleOptions"/> which need to be configured.</param>
        /// <returns><c>true</c> if <paramref name="input"/> was converted successfully; otherwise, <c>false</c>.</returns>
        public bool TryParse(string input, out TimeSpan result, Action<TimeSpanDoubleOptions> setup)
        {
            return Patterns.TryParse(() => Parse(input, setup), out result);
        }
    }
}