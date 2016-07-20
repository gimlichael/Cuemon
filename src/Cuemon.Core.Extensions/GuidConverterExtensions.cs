using System;

namespace Cuemon
{
    /// <summary>
    /// This is an extension implementation of the <see cref="GuidConverter"/> class.
    /// </summary>
    public static class GuidConverterExtensions
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
        public static Guid ToGuid(this string value)
        {
            return GuidConverter.FromString(value);
        }
    }
}