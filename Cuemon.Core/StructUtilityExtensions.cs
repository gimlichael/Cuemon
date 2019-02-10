using System;
using System.Collections.Generic;

namespace Cuemon
{
    /// <summary>
    /// This is an extension implementation of the most common methods on the <see cref="StructUtility"/> class.
    /// </summary>
    public static class StructUtilityExtensions
    {
        /// <summary>
        /// Computes a suitable hash code from the specified sequence of <paramref name="convertibles"/>.
        /// </summary>
        /// <param name="convertibles">A sequence of structs implementing the <see cref="IConvertible"/> interface.</param>
        /// <returns>A 32-bit signed integer that is the hash code of <paramref name="convertibles"/>.</returns>
        /// <exception cref="System.ArgumentNullException">
        /// <paramref name="convertibles"/> is null.
        /// </exception>
        public static int GetHashCode32<T>(this IEnumerable<T> convertibles) where T : struct, IConvertible
        {
            return StructUtility.GetHashCode32(convertibles);
        }

        /// <summary>
        /// Computes a suitable hash code from the specified sequence of <paramref name="convertibles"/>.
        /// </summary>
        /// <param name="convertibles">A sequence of structs implementing the <see cref="IConvertible"/> interface.</param>
        /// <returns>A 64-bit signed integer that is the hash code of <paramref name="convertibles"/>.</returns>
        /// <exception cref="System.ArgumentNullException">
        /// <paramref name="convertibles"/> is null.
        /// </exception>
        public static long GetHashCode64<T>(this IEnumerable<T> convertibles) where T : struct, IConvertible
        {
            return StructUtility.GetHashCode64(convertibles);
        }
    }
}