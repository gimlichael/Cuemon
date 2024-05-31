using System;
using System.Globalization;

namespace Cuemon.Extensions.AspNetCore.Authentication.AwsSignature4
{
    /// <summary>
    /// Extension methods for the <see cref="DateTime"/> struct.
    /// </summary>
    public static class DateTimeExtensions
    {
        /// <summary>
        /// Converts the value of the specified <paramref name="dt"/> to its equivalent string representation using <see cref="Aws4HmacFields.DateStampFormat"/> format and the formatting conventions of <see cref="CultureInfo.InvariantCulture"/>.
        /// </summary>
        /// <param name="dt">The <see cref="DateTime"/> to extend.</param>
        /// <returns>A string representation of the specified <paramref name="dt"/> as <c>yyyyMMdd</c>.</returns>
        public static string ToAwsDateString(this DateTime dt)
        {
            return dt.ToString(Aws4HmacFields.DateStampFormat, CultureInfo.InvariantCulture);
        }

        /// <summary>
        /// Converts the value of the specified <paramref name="dt"/> to its equivalent string representation using <see cref="Aws4HmacFields.DateTimeStampFormat"/> format and the formatting conventions of <see cref="CultureInfo.InvariantCulture"/>.
        /// </summary>
        /// <param name="dt">The <see cref="DateTime"/> to extend.</param>
        /// <returns>A string representation of the specified <paramref name="dt"/> as <c>yyyyMMddTHHmmssZ</c>.</returns>
        public static string ToAwsDateTimeString(this DateTime dt)
        {
            return dt.ToString(Aws4HmacFields.DateTimeStampFormat, CultureInfo.InvariantCulture);
        }
    }
}
