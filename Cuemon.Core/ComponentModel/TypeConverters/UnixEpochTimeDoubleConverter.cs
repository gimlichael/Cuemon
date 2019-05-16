using System;

namespace Cuemon.ComponentModel.TypeConverters
{
    /// <summary>
    /// Provides a converter that converts a <see cref="double"/> to its equivalent <see cref="DateTime"/>.
    /// </summary>
    public class UnixEpochTimeDoubleConverter : IConverter<double, DateTime>
    {
        /// <summary>
        /// Converts the specified <paramref name="input"/> of an UNIX Epoch time to its equivalent <see cref="DateTime"/> structure.
        /// </summary>
        /// <param name="input">The <see cref="double"/> to be converted into a <see cref="DateTime"/>.</param>
        /// <returns>A <see cref="DateTime"/> that is equivalent to <paramref name="input"/>.</returns>
        public DateTime ChangeType(double input)
        {
            return UnixEpochTimeDateTimeConverter.UnixDate.AddSeconds(input);
        }
    }
}