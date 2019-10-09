using System;

namespace Cuemon.ComponentModel
{
    /// <summary>
    /// Provides a converter that converts a <see cref="double"/> to its equivalent <see cref="DateTime"/> and vice versa.
    /// </summary>
    public class UnixEpochTimeConverter : IConverter<double, DateTime>, IConverter<DateTime, double>
    {
        internal static readonly DateTime UnixDate = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);

        /// <summary>
        /// Converts the specified <paramref name="input"/> of an UNIX Epoch time to its equivalent <see cref="DateTime"/> structure.
        /// </summary>
        /// <param name="input">The <see cref="double"/> to be converted into a <see cref="DateTime"/>.</param>
        /// <returns>A <see cref="DateTime"/> that is equivalent to <paramref name="input"/>.</returns>
        public DateTime ChangeType(double input)
        {
            return UnixDate.AddSeconds(input);
        }

        /// <summary>
        /// Converts the specified <paramref name="input"/> to its equivalent <see cref="double"/> UNIX Epoch time representation.
        /// </summary>
        /// <param name="input">The <see cref="DateTime"/> to be converted into a <see cref="double"/>.</param>
        /// <returns>A <see cref="double"/> UNIX Epoch time representation that is equivalent to <paramref name="input"/>.</returns>
        public double ChangeType(DateTime input)
        {
            if (input.Kind == DateTimeKind.Local) { input = input.ToUniversalTime(); }
            return Math.Floor((input - UnixDate).TotalSeconds);
        }
    }
}