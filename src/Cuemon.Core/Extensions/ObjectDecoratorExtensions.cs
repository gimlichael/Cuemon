using System;
using System.Collections.Generic;

namespace Cuemon
{
    /// <summary>
    /// Extension methods for an arbitrary <see cref="object"/> class tailored to adhere the decorator pattern.
    /// </summary>
    /// <seealso cref="IDecorator{T}"/>
    /// <seealso cref="Decorator{T}"/>
    public static class ObjectDecoratorExtensions
    {
        /// <summary>
        /// Converts the enclosed <see cref="IEnumerable{T}"/> of the <paramref name="decorator"/> to a string of <paramref name="delimiter"/> delimited values.
        /// </summary>
        /// <typeparam name="T">The type of the elements of the sequence to convert.</typeparam>
        /// <param name="decorator">The <see cref="IDecorator{T}"/> to extend.</param>
        /// <param name="delimiter">The delimiter specification.</param>
        /// <param name="converter">The function delegate that converts <typeparamref name="T"/> to a string representation once per iteration.</param>
        /// <returns>A <see cref="string"/> of delimited values from the by parameter specified <paramref name="delimiter"/>.</returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="decorator"/> cannot be null -or-
        /// <paramref name="delimiter"/> cannot be null.
        /// </exception>
        /// <exception cref="ArgumentException">
        /// <paramref name="delimiter"/> cannot be empty or consist only of white-space characters.
        /// </exception>
        public static string ToDelimitedString<T>(this IDecorator<IEnumerable<T>> decorator, string delimiter = ",", Func<T, string> converter = null)
        {
            Validator.ThrowIfNull(decorator, nameof(decorator));
            Validator.ThrowIfNullOrWhitespace(delimiter, nameof(delimiter));
            return DelimitedString.Create(decorator.Inner, o =>
            {
                o.Delimiter = delimiter;
                if (converter != null) { o.StringConverter = converter; }
            });
        }

        /// <summary>
        /// Adjust the enclosed <typeparamref name="T"/> of the <paramref name="decorator"/> with the function delegate <paramref name="tweaker"/>.
        /// </summary>
        /// <typeparam name="T">The type of the value to adjust.</typeparam>
        /// <param name="decorator">The <see cref="IDecorator{T}"/> to extend.</param>
        /// <param name="tweaker">The function delegate that will adjust the enclosed <typeparamref name="T"/> of the <paramref name="decorator"/>.</param>
        /// <returns>The enclosed <typeparamref name="T"/> of the <paramref name="decorator"/> in its original or adjusted form.</returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="decorator"/> cannot be null.
        /// </exception>
        public static T Adjust<T>(this IDecorator<T> decorator, Func<T, T> tweaker)
        {
            Validator.ThrowIfNull(decorator, nameof(decorator));
            return Tweaker.Adjust(decorator.Inner, tweaker);
        }
    }
}