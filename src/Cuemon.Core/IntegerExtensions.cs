using System;

namespace Cuemon
{
    /// <summary>
    /// Provides extension methods for <see cref="short"/>, <see cref="int"/> and <see cref="long"/>.
    /// </summary>
    public static class IntegerExtensions
    {
        /// <summary>
        /// Returns the smaller of two 32-bit signed integers.
        /// </summary>
        /// <param name="value">The first of two 32-bit signed integers to compare.</param>
        /// <param name="maximum">The second of two 32-bit signed integers to compare.</param>
        /// <returns>Parameter <paramref name="value"/> or <paramref name="maximum"/>, whichever is smaller.</returns>
        public static int Min(this int value, int maximum) => Math.Min(value, maximum);

        /// <summary>
        /// Returns the smaller of two 64-bit signed integers.
        /// </summary>
        /// <param name="value">The first of two 64-bit signed integers to compare.</param>
        /// <param name="maximum">The second of two 64-bit signed integers to compare.</param>
        /// <returns>Parameter <paramref name="value"/> or <paramref name="maximum"/>, whichever is smaller.</returns>
        public static long Min(this long value, long maximum) => Math.Min(value, maximum);

        /// <summary>
        /// Returns the smaller of two 16-bit signed integers.
        /// </summary>
        /// <param name="value">The first of two 16-bit signed integers to compare.</param>
        /// <param name="maximum">The second of two 16-bit signed integers to compare.</param>
        /// <returns>Parameter <paramref name="value"/> or <paramref name="maximum"/>, whichever is smaller.</returns>
        public static short Min(this short value, short maximum) => Math.Min(value, maximum);

        /// <summary>
        /// Returns the larger of two 32-bit signed integers.
        /// </summary>
        /// <param name="value">The first of two 32-bit signed integers to compare.</param>
        /// <param name="minimum">The second of two 32-bit signed integers to compare.</param>
        /// <returns>Parameter <paramref name="value"/> or <paramref name="minimum"/>, whichever is larger.</returns>
        public static int Max(this int value, int minimum) => Math.Max(value, minimum);

        /// <summary>
        /// Returns the larger of two 64-bit signed integers.
        /// </summary>
        /// <param name="value">The first of two 64-bit signed integers to compare.</param>
        /// <param name="minimum">The second of two 64-bit signed integers to compare.</param>
        /// <returns>Parameter <paramref name="value"/> or <paramref name="minimum"/>, whichever is larger.</returns>
        public static long Max(this long value, long minimum) => Math.Max(value, minimum);

        /// <summary>
        /// Returns the larger of two 16-bit signed integers.
        /// </summary>
        /// <param name="value">The first of two 16-bit signed integers to compare.</param>
        /// <param name="minimum">The second of two 16-bit signed integers to compare.</param>
        /// <returns>Parameter <paramref name="value"/> or <paramref name="minimum"/>, whichever is larger.</returns>
        public static short Max(this short value, short minimum) => Math.Max(value, minimum);
    }
}