using System;

namespace Cuemon
{
    /// <summary>
    /// This is an extensions implementation of the <see cref="DateTime"/> structure using methods already found in the Microsoft .NET Framework.
    /// </summary>
    public static class DateTimeExtensions
    {
        /// <summary>
        /// Converts the specified <paramref name="value"/> to a Coordinated Universal Time (UTC) representation.
        /// </summary>
        /// <param name="value">The value to convert.</param>
        /// <returns>A new <see cref="DateTime"/> value initialized to <see cref="DateTimeKind.Utc"/> that has the same number of ticks as the object represented by the <paramref name="value"/> parameter.</returns>
        public static DateTime ToUtcKind(this DateTime value)
        {
            return ToKind(value, DateTimeKind.Utc);
        }

        /// <summary>
        /// Converts the specified <paramref name="value"/> to a local time representation.
        /// </summary>
        /// <param name="value">The value to convert.</param>
        /// <returns>A new <see cref="DateTime"/> value initialized to <see cref="DateTimeKind.Local"/> that has the same number of ticks as the object represented by the <paramref name="value"/> parameter.</returns>
        public static DateTime ToLocalKind(this DateTime value)
        {
            return ToKind(value, DateTimeKind.Local);
        }

        /// <summary>
        /// Converts the specified <paramref name="value"/> to a representation that is not specified as either local time or UTC.
        /// </summary>
        /// <param name="value">The value to convert.</param>
        /// <returns>A new <see cref="DateTime"/> value initialized to <see cref="DateTimeKind.Unspecified"/> that has the same number of ticks as the object represented by the <paramref name="value"/> parameter.</returns>
        public static DateTime ToDefaultKind(this DateTime value)
        {
            return ToKind(value, DateTimeKind.Unspecified);
        }

        private static DateTime ToKind(DateTime value, DateTimeKind kind)
        {
            if (value.Kind != kind) { value = DateTime.SpecifyKind(value, kind); }
            return value;
        }
    }
}