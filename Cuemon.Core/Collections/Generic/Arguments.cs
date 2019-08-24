using System.Collections.Generic;

namespace Cuemon.Collections.Generic
{
    /// <summary>
    /// Provides a set of static methods for typing (no conversion) a variable number of arguments into its equivalent <see cref="IEnumerable{T}"/>.
    /// </summary>
    public static class Arguments
    {
        /// <summary>
        /// Returns the input typed as <see cref="IEnumerable{T}"/>.
        /// </summary>
        /// <typeparam name="T">The type of the elements of <paramref name="args"/>.</typeparam>
        /// <param name="args">The <see cref="T:T[]"/> to type as <see cref="IEnumerable{T}"/>.</param>
        /// <returns>The input <paramref name="args"/> typed as <see cref="IEnumerable{T}"/>.</returns>
        /// <remarks>The <see cref="FromParams{T}"/> method has no effect other than to change the compile-time type of <paramref name="args"/> from <see cref="T:T[]"/> to <see cref="IEnumerable{T}"/>.</remarks>
        public static IEnumerable<T> FromParams<T>(params T[] args)
        {
            return args;
        }

        /// <summary>
        /// Returns an <see cref="IEnumerable{T}"/> with the specified <paramref name="arg"/> as the only element.
        /// </summary>
        /// <typeparam name="T">The type of the element of <paramref name="arg"/>.</typeparam>
        /// <param name="arg">The <typeparamref name="T"/> to type as <see cref="IEnumerable{T}"/>.</param>
        /// <returns>An <see cref="IEnumerable{T}"/> with the specified <paramref name="arg"/> as the only element.</returns>
        /// <remarks>The <see cref="Yield{T}"/> method has no effect other than to change the compile-time type of <paramref name="arg"/> from <typeparamref name="T"/> to <see cref="IEnumerable{T}"/>.</remarks>
        public static IEnumerable<T> Yield<T>(T arg)
        {
            yield return arg;
        }
    }
}