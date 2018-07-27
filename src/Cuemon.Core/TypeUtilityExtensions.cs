using System;

namespace Cuemon
{
    /// <summary>
    /// This is an extension implementation of the most common methods on the <see cref="TypeUtility"/> class.
    /// </summary>
    public static class TypeUtilityExtensions
    {
        /// <summary>
        /// Determines whether the specified <paramref name="source"/> is of <typeparamref name="T"/>.
        /// </summary>
        /// <typeparam name="T">The type to compare with <paramref name="source"/>.</typeparam>
        /// <param name="source">The object to compare with <typeparamref name="T"/>.</param>
        /// <returns><c>true</c> if the specified <paramref name="source"/> is of <typeparamref name="T"/>; otherwise, <c>false</c>.</returns>
        public static bool Is<T>(this object source)
        {
            return TypeUtility.Is<T>(source);
        }

        /// <summary>
        /// Determines whether the specified <paramref name="source"/> is not of <typeparamref name="T"/>.
        /// </summary>
        /// <typeparam name="T">The type to compare with <paramref name="source"/>.</typeparam>
        /// <param name="source">The object to compare with <typeparamref name="T"/>.</param>
        /// <returns><c>true</c> if the specified <paramref name="source"/> is not of <typeparamref name="T"/>; otherwise, <c>false</c>.</returns>
        public static bool IsNot<T>(this object source)
        {
            return TypeUtility.IsNot<T>(source);
        }

        /// <summary>
        /// Determines whether the specified source is a nullable <see cref="ValueType"/>.
        /// </summary>
        /// <typeparam name="T">The type of the <paramref name="source"/> of <typeparamref name="T"/>.</typeparam>
        /// <param name="source">The source type to check for nullable <see cref="ValueType"/>.</param>
        /// <returns>
        ///   <c>true</c> if the specified source is nullable; otherwise, <c>false</c>.
        /// </returns>
        public static bool IsNullable<T>(this T source) { return TypeUtility.IsNullable(source); }

        /// <summary>
        /// Determines whether the specified source is a nullable <see cref="ValueType"/>.
        /// </summary>
        /// <typeparam name="T">The type of the <paramref name="source"/> of <typeparamref name="T"/>.</typeparam>
        /// <param name="source">The source type to check for nullable <see cref="ValueType"/>.</param>
        /// <returns>
        ///   <c>true</c> if the specified source is nullable; otherwise, <c>false</c>.
        /// </returns>
        public static bool IsNullable<T>(this T? source) where T : struct { return TypeUtility.IsNullable(source); }
    }
}