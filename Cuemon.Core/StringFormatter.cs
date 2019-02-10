using System;
using System.Globalization;

namespace Cuemon
{
    /// <summary>
    /// This utility class is designed to make <see cref="string"/> related formating operations easier to work with.
    /// </summary>
    public static class StringFormatter
    {
        /// <summary>
        /// Returns a string expression representing a standardized date- and time value.
        /// </summary>
        /// <param name="value">The <see cref="DateTime"/> value to be formatted.</param>
        /// <param name="pattern">The standardized patterns to apply on <paramref name="value"/>.</param>
        /// <returns>Returns a string expression representing a date- and time value.</returns>
        public static string FromDateTime(DateTime value, StandardizedDateTimeFormatPattern pattern)
        {
            return FromDateTime(value, pattern, 0);
        }

        /// <summary>
        /// Returns a string expression representing a standardized date- and time value.
        /// </summary>
        /// <param name="value">The <see cref="DateTime"/> value to be formatted.</param>
        /// <param name="pattern">The standardized patterns to apply on <paramref name="value"/>.</param>
        /// <param name="fractionalDecimalPlaces">The amount of fractional decimal places to apply to the string expression.</param>
        /// <returns>Returns a string expression representing a date- and time value.</returns>
        public static string FromDateTime(DateTime value, StandardizedDateTimeFormatPattern pattern, byte fractionalDecimalPlaces)
        {
            switch (pattern)
            {
                case StandardizedDateTimeFormatPattern.Iso8601CompleteDateBasic:
                    return value.ToString("yyyyMMdd", CultureInfo.InvariantCulture);
                case StandardizedDateTimeFormatPattern.Iso8601CompleteDateExtended:
                    return value.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture);
                case StandardizedDateTimeFormatPattern.Iso8601CompleteDateTimeBasic:
                    return value.ToString(string.Format(CultureInfo.InvariantCulture, "yyyyMMddTHHmmss{0}{1}", fractionalDecimalPlaces == 0 ? "" : string.Format(CultureInfo.InvariantCulture, ".{0}", StringUtility.CreateFixedString('f', fractionalDecimalPlaces)), value.Kind == DateTimeKind.Utc ? "Z" : "zz"), CultureInfo.InvariantCulture);
                case StandardizedDateTimeFormatPattern.Iso8601CompleteDateTimeExtended:
                    return value.ToString(string.Format(CultureInfo.InvariantCulture, "yyyy-MM-ddTHH:mm:ss{0}{1}", fractionalDecimalPlaces == 0 ? "" : string.Format(CultureInfo.InvariantCulture, ".{0}", StringUtility.CreateFixedString('f', fractionalDecimalPlaces)), value.Kind == DateTimeKind.Utc ? "Z" : "zz"), CultureInfo.InvariantCulture);
                default:
                    throw new ArgumentOutOfRangeException(nameof(pattern));
            }
        }

        /// <summary>
        /// Returns a string expression representing a standardized date- and time value.
        /// </summary>
        /// <param name="value">The <see cref="DateTime"/> value to be formatted.</param>
        /// <param name="pattern">The standardized patterns to apply on <paramref name="value"/>.</param>
        /// <returns>Returns a string expression representing a date- and time value.</returns>
        public static string FromDateTime(DateTime value, DateTimeFormatPattern pattern)
        {
            return FromDateTime(value, pattern, CultureInfo.InvariantCulture);
        }

        /// <summary>
        /// Returns a string expression representing a standardized date- and time value.
        /// </summary>
        /// <param name="value">The <see cref="DateTime"/> value to be formatted.</param>
        /// <param name="pattern">The standardized patterns to apply on <paramref name="value"/>.</param>
        /// <param name="provider">An <see cref="IFormatProvider"/> that supplies culture-specific formatting information.</param>
        /// <returns>Returns a string expression representing a date- and time value.</returns>
        public static string FromDateTime(DateTime value, DateTimeFormatPattern pattern, IFormatProvider provider)
        {
            DateTimeFormatInfo formatInfo = DateTimeFormatInfo.GetInstance(provider);
            switch (pattern)
            {
                case DateTimeFormatPattern.LongDate:
                    return value.ToString(formatInfo.LongDatePattern, formatInfo);
                case DateTimeFormatPattern.LongDateTime:
                    return value.ToString(string.Format(formatInfo, "{0} {1}", formatInfo.LongDatePattern, formatInfo.LongTimePattern), formatInfo);
                case DateTimeFormatPattern.LongTime:
                    return value.ToString(formatInfo.LongTimePattern, formatInfo);
                case DateTimeFormatPattern.ShortDate:
                    return value.ToString(formatInfo.ShortDatePattern, formatInfo);
                case DateTimeFormatPattern.ShortDateTime:
                    return value.ToString(string.Format(formatInfo, "{0} {1}", formatInfo.ShortDatePattern, formatInfo.ShortTimePattern), formatInfo);
                case DateTimeFormatPattern.ShortTime:
                    return value.ToString(formatInfo.ShortTimePattern, formatInfo);
                default:
                    throw new ArgumentOutOfRangeException(nameof(pattern));
            }
        }
    }
}