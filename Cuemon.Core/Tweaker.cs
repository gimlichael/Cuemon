using System;

namespace Cuemon
{
    /// <summary>
    /// Provides a way to change any instance of the same generic type.
    /// </summary>
    public static class Tweaker
    {
        /// <summary>
        /// Adjust the specified <paramref name="value"/> with the function delegate <paramref name="tweaker"/>.
        /// </summary>
        /// <typeparam name="T">The type of the value to adjust.</typeparam>
        /// <param name="value">The value to adjust.</param>
        /// <param name="tweaker">The function delegate that will adjust the specified <paramref name="value"/>.</param>
        /// <returns>The <paramref name="value"/> in its original or adjusted form.</returns>
        public static T Adjust<T>(T value, Func<T, T> tweaker)
        {
            if (tweaker == null) { return value; }
            return tweaker(value);
        }
    }
}