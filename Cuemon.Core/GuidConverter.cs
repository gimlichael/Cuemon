using System;

namespace Cuemon
{
    /// <summary>
    /// This utility class is designed to make <see cref="Guid"/> related conversions easier to work with.
    /// </summary>
    public static class GuidConverter
    {
        /// <summary>
        /// Converts the specified <paramref name="value"/> of a GUID to its equivalent <see cref="Guid"/> structure.
        /// </summary>
        /// <param name="value">The GUID to be converted.</param>   
        /// <returns>A <see cref="Guid"/> that is equivalent to <paramref name="value"/>.</returns>
        /// <exception cref="System.ArgumentNullException">
        /// <paramref name="value"/> is null.
        /// </exception>
        /// <exception cref="System.FormatException">
        /// The specified <paramref name="value"/> was not recognized to be a GUID.
        /// </exception>
        public static Guid FromString(string value)
        {
            Validator.ThrowIfNull(value, nameof(value));
            Guid result;
            if (!GuidUtility.TryParse(value, out result)) { throw new FormatException("The specified value was not recognized to be a GUID."); }
            return result;
        }
    }
}