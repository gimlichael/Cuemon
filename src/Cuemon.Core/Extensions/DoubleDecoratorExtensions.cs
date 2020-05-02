using System;
using Cuemon.Diagnostics;

namespace Cuemon
{
    /// <summary>
    /// Extension methods for the <see cref="double"/> struct tailored to adhere the decorator pattern.
    /// </summary>
    /// <seealso cref="IDecorator{T}"/>
    /// <seealso cref="Decorator{T}"/>
    public static class DoubleDecoratorExtensions
    {
        /// <summary>
        /// Converts the enclosed <see cref="double"/> of the <paramref name="decorator"/> to its equivalent <see cref="TimeSpan"/> representation.
        /// </summary>
        /// <param name="decorator">The <see cref="IDecorator{T}"/> to extend.</param>
        /// <param name="timeUnit">One of the enumeration values that specifies the outcome of the conversion.</param>
        /// <returns>A <see cref="TimeSpan"/> that corresponds to the enclosed <see cref="double"/> of the <paramref name="decorator"/> and <paramref name="timeUnit"/>.</returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="decorator"/> cannot be null.
        /// </exception>
        /// <exception cref="OverflowException">
        /// The enclosed <see cref="double"/> of the <paramref name="decorator"/> paired with <paramref name="timeUnit"/> is outside its valid range.
        /// </exception>
        /// <exception cref="ArgumentOutOfRangeException">
        /// <paramref name="timeUnit"/> was outside its valid range.
        /// </exception>
        public static TimeSpan ToTimeSpan(this IDecorator<double> decorator, TimeUnit timeUnit)
        {
            return TimeMeasure.CreateTimeSpan(decorator.Inner, timeUnit);
        }
    }
}