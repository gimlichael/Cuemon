using System;
namespace Cuemon
{
    /// <summary>
    /// This utility class is designed to make common <see cref="Version"/> related operations easier to work with.
    /// </summary>
    public static class VersionUtility
    {
        /// <summary>
        /// Represents the default <see cref="Version"/> value of 1.0.0.0. This field is read-only.
        /// </summary>
        public static readonly Version DefaultValue = new Version(1, 0, 0, 0);

        /// <summary>
        /// Represents the minimum <see cref="Version"/> value of 0.0.0.0. This field is read-only.
        /// </summary>
        public static readonly Version MinValue = new Version(0, 0, 0, 0);

        /// <summary>
        /// Represents the maximum <see cref="Version"/> value of 2147483647.2147483647.2147483647.2147483647. This field is read-only.
        /// </summary>
        public static readonly Version MaxValue = new Version(int.MaxValue, int.MaxValue, int.MaxValue, int.MaxValue);
    }
}