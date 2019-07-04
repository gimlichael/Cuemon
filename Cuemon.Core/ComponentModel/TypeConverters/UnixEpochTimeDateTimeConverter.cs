using System;
using Cuemon.ComponentModel.Converters;

namespace Cuemon.ComponentModel.TypeConverters
{
    /// <summary>
    /// Provides a converter that converts a <see cref="DateTime"/> to its equivalent <see cref="double"/> UNIX Epoch time represenation.
    /// </summary>
    public class UnixEpochTimeDateTimeConverter : IConverter<DateTime, double>
    {
        internal static readonly DateTime UnixDate = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);

        /// <summary>
        /// Converts the specified <paramref name="input"/> to its equivalent <see cref="double"/> UNIX Epoch time represenation.
        /// </summary>
        /// <param name="input">The <see cref="DateTime"/> to be converted into a <see cref="double"/>.</param>
        /// <returns>A <see cref="double"/> UNIX Epoch time represenation that is equivalent to <paramref name="input"/>.</returns>
        public double ChangeType(DateTime input)
        {
            if (input.Kind == DateTimeKind.Local) { input = input.ToUniversalTime(); }
            return Math.Floor((input - UnixDate).TotalSeconds);
        }
    }
}