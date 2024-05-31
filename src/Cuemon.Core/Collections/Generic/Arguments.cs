using System;
using System.Collections.Generic;

namespace Cuemon.Collections.Generic
{
    /// <summary>
    /// Provides a set of static methods for both typing (no conversion) and converting a variable number of arguments into its equivalent <see cref="T:object[]"/>, <see cref="IEnumerable{T}"/> and <see cref="T:T[]"/>.
    /// </summary>
    public static class Arguments
    {
        /// <summary>
        /// Concatenates two arrays.
        /// </summary>
        /// <typeparam name="T">The type of the elements of the input arrays.</typeparam>
        /// <param name="args1">The first array to concatenate.</param>
        /// <param name="args2">The array to concatenate to the first array.</param>
        /// <returns>An <see cref="T:T[]"/> that contains the concatenated elements of the two input arrays.</returns>
        public static T[] Concat<T>(T[] args1, T[] args2)
        {
            if (args1 == null) { return Array.Empty<T>(); }
            if (args2 == null) { return args1; }
            if (args1.Length == 0 || args2.Length == 0) { return args1.Length == 0 ? args2 : args1; }
            var result = new T[args1.Length + args2.Length];
            args1.CopyTo(result, 0);
            args2.CopyTo(result, args2.Length);
            return result;
        }

        /// <summary>
        /// Returns the input typed as <see cref="T:T[]"/>.
        /// </summary>
        /// <typeparam name="T">The type of the elements of <paramref name="args"/>.</typeparam>
        /// <param name="args">The <see cref="T:T[]"/> to type as <see cref="T:T[]"/>.</param>
        /// <returns>The input <paramref name="args"/> typed as <see cref="T:T[]"/>.</returns>
        /// <remarks>The <see cref="ToArrayOf{T}"/> method has no effect other than to change the compile-time type of <paramref name="args"/> from <see cref="T:T[]"/> to <see cref="T:T[]"/>.</remarks>
        public static T[] ToArrayOf<T>(params T[] args)
        {
            return args;
        }

        /// <summary>
        /// Returns the input typed as <see cref="T:object[]"/>.
        /// </summary>
        /// <param name="args">The <see cref="T:object[]"/> to type as <see cref="T:object[]"/>.</param>
        /// <returns>The input <paramref name="args"/> typed as <see cref="T:object[]"/>.</returns>
        /// <remarks>The <see cref="ToArray"/> method has no effect other than to change the compile-time type of <paramref name="args"/> from <see cref="T:object[]"/> to <see cref="T:object[]"/>.</remarks>
        public static object[] ToArray(params object[] args)
        {
            return args;
        }

        /// <summary>
        /// Returns the input typed as <see cref="IEnumerable{T}"/>.
        /// </summary>
        /// <typeparam name="T">The type of the elements of <paramref name="args"/>.</typeparam>
        /// <param name="args">The <see cref="T:T[]"/> to type as <see cref="IEnumerable{T}"/>.</param>
        /// <returns>The input <paramref name="args"/> typed as <see cref="IEnumerable{T}"/>.</returns>
        /// <remarks>The <see cref="ToEnumerableOf{T}"/> method has no effect other than to change the compile-time type of <paramref name="args"/> from <see cref="T:T[]"/> to <see cref="IEnumerable{T}"/>.</remarks>
        public static IEnumerable<T> ToEnumerableOf<T>(params T[] args)
        {
            return args;
        }

        /// <summary>
        /// Returns the input typed as <see cref="IEnumerable{T}"/>.
        /// </summary>
        /// <param name="args">The <see cref="T:object[]"/> to type as <see cref="IEnumerable{T}"/>.</param>
        /// <returns>The input <paramref name="args"/> typed as <see cref="IEnumerable{T}"/>.</returns>
        /// <remarks>The <see cref="ToEnumerable"/> method has no effect other than to change the compile-time type of <paramref name="args"/> from <see cref="T:object[]"/> to <see cref="IEnumerable{T}"/>.</remarks>
        public static IEnumerable<object> ToEnumerable(params object[] args)
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