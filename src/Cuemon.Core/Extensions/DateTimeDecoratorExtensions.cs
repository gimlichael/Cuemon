using System;

namespace Cuemon
{
    /// <summary>
    /// Extension methods for the <see cref="DateTime"/> struct tailored to adhere the decorator pattern.
    /// </summary>
    /// <seealso cref="IDecorator{T}"/>
    /// <seealso cref="Decorator{T}"/>
    public static class DateTimeDecoratorExtensions
    {
        /// <summary>
        /// A <see cref="DateTime"/> initialized to midnight, January 1st, 1970 in Coordinated Universal Time (UTC).
        /// </summary>
        private static readonly DateTime UnixEpoch = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);

        /// <summary>
        /// Gets a <see cref="DateTime"/> initialized to midnight, January 1st, 1970 in Coordinated Universal Time (UTC).
        /// </summary>
        /// <param name="_">The <see cref="IDecorator{T}"/> to extend.</param>
        /// <returns>A a <see cref="DateTime"/> initialized to midnight, January 1st, 1970 in Coordinated Universal Time (UTC).</returns>
        public static DateTime GetUnixEpoch(this IDecorator<DateTime> _)
        {
            return UnixEpoch;
        }

        /// <summary>
        /// Converts the enclosed <see cref="DateTime"/> of the <paramref name="decorator"/> to an equivalent UNIX Epoch time representation.
        /// </summary>
        /// <param name="decorator">The <see cref="IDecorator{T}"/> to extend.</param>
        /// <returns>A <see cref="double"/> value that is equivalent to the enclosed <see cref="DateTime"/> of the <paramref name="decorator"/>.</returns>
        /// <remarks>This implementation converts the enclosed <see cref="DateTime"/> of the <paramref name="decorator"/> to an UTC representation ONLY if the <see cref="DateTime.Kind"/> equals <see cref="DateTimeKind.Local"/>.</remarks>
        public static double ToUnixEpochTime(this IDecorator<DateTime> decorator)
        {
            Validator.ThrowIfNull(decorator, nameof(decorator));
            var value = decorator.Inner.Kind == DateTimeKind.Local ? decorator.Inner.ToUniversalTime() : decorator.Inner;
            return Math.Floor((value - UnixEpoch).TotalSeconds);
        }
    }
}