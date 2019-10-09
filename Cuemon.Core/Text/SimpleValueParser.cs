using System;
using System.Globalization;

namespace Cuemon.Text
{
    /// <summary>
    /// Provides a parser that converts a <see cref="string"/>, represented as a simple value type, to its equivalent <see cref="bool"/>, <see cref="byte"/>, <see cref="int"/>, <see cref="long"/>, <see cref="double"/>, <see cref="float"/>, <see cref="DateTime"/> or <see cref="Guid"/>.
    /// </summary>
    public class SimpleValueParser : IParser<object, FormattingOptions<CultureInfo>>
    {
        /// <summary>
        /// Converts the string representation of a simple value type to its <see cref="bool"/>, <see cref="byte"/>, <see cref="int"/>, <see cref="long"/>, <see cref="double"/>, <see cref="float"/>, <see cref="DateTime"/> or <see cref="Guid"/> equivalent.
        /// </summary>
        /// <param name="value">The string to convert.</param>
        /// <param name="setup">The <see cref="FormattingOptions{CultureInfo}"/> which may be configured.</param>
        /// <returns>A <see cref="bool"/>, <see cref="byte"/>, <see cref="int"/>, <see cref="long"/>, <see cref="double"/>, <see cref="float"/>, <see cref="DateTime"/> or <see cref="Guid"/> equivalent to <paramref name="value"/>.</returns>
        /// <remarks>If unable to parse the <paramref name="value"/> for a simple value type, the original value is returned.</remarks>
        public object Parse(string value, Action<FormattingOptions<CultureInfo>> setup = null)
        {
            if (value == null) { return null; }
            var options = Patterns.Configure(setup);

            if (bool.TryParse(value, out var boolinput)) { return boolinput; }
            if (byte.TryParse(value, NumberStyles.None, options.FormatProvider, out var byteinput)) { return byteinput; }
            if (int.TryParse(value, NumberStyles.None, options.FormatProvider, out var intinput)) { return intinput; }
            if (long.TryParse(value, NumberStyles.None, options.FormatProvider, out var longinput)) { return longinput; }
            if (double.TryParse(value, NumberStyles.Number, options.FormatProvider, out var doubleinput)) { return doubleinput; }
            if (float.TryParse(value, NumberStyles.Float, options.FormatProvider, out var floatinput)) { return floatinput; }
            if (value.Length > 6 && DateTime.TryParse(value, CultureInfo.InvariantCulture, DateTimeStyles.AdjustToUniversal, out var dateTimeinput)) { return dateTimeinput; }
            if (value.Length > 31 && value.Length < 69 && Guid.TryParse(value, out var guidinput)) { return guidinput; }

            return value;
        }

        /// <summary>
        /// Converts the string representation of a simple value type to its <see cref="bool"/>, <see cref="byte"/>, <see cref="int"/>, <see cref="long"/>, <see cref="double"/>, <see cref="float"/>, <see cref="DateTime"/> or <see cref="Guid"/> equivalent. A return value indicates whether the conversion succeeded.
        /// </summary>
        /// <param name="value">The string to convert.</param>
        /// <param name="result">When this method returns, contains the <see cref="bool"/>, <see cref="byte"/>, <see cref="int"/>, <see cref="long"/>, <see cref="double"/>, <see cref="float"/>, <see cref="DateTime"/> or <see cref="Guid"/> equivalent of the <paramref name="value"/>, if the conversion succeeded, or <paramref name="value"/> if the conversion failed.</param>
        /// <param name="setup">The <see cref="FormattingOptions{CultureInfo}"/> which may be configured.</param>
        /// <returns><c>true</c> if <paramref name="value"/> was converted successfully; otherwise, <c>false</c>.</returns>
        public bool TryParse(string value, out object result, Action<FormattingOptions<CultureInfo>> setup = null)
        {
            return Patterns.TryInvoke(() => Parse(value, setup), out result);
        }
    }
}