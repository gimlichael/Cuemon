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
        /// <param name="format">A bitmask comprised of one or more <see cref="GuidFormats"/> that specify how the GUID parsing is conducted.</param>
        /// <returns>A <see cref="Guid"/> that is equivalent to <paramref name="value"/>.</returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="value"/> is null.
        /// </exception>
        /// <exception cref="FormatException">
        /// The specified <paramref name="value"/> was not recognized to be a GUID.
        /// </exception>
        public static Guid FromString(string value, GuidFormats format = GuidFormats.BraceFormat | GuidFormats.DigitFormat | GuidFormats.ParenthesisFormat)
        {
            Validator.ThrowIfNull(value, nameof(value));
            if (!GuidUtility.TryParse(value, format, out var result)) { throw new FormatException("The specified value was not recognized to be a GUID."); }
            return result;
        }
    }
}