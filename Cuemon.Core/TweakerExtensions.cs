using System;

namespace Cuemon
{
    /// <summary>
    /// Extension methods for the <see cref="Tweaker"/> class.
    /// </summary>
    public static class TweakerExtensions
    {
        /// <summary>
        /// Adjust the specified <paramref name="value"/> with the function delegate <paramref name="tweaker"/>.
        /// </summary>
        /// <typeparam name="T">The type of the value to adjust.</typeparam>
        /// <param name="value">The value to adjust.</param>
        /// <param name="tweaker">The function delegate that will adjust the specified <paramref name="value"/>.</param>
        /// <returns>The <paramref name="value"/> in its original or adjusted form.</returns>
        public static T Adjust<T>(this T value, Func<T, T> tweaker)
        {
            return Tweaker.Adjust(value, tweaker);
        }
    }
}