using System;

namespace Cuemon.ComponentModel
{
    /// <summary>
    /// Defines methods that converts a <typeparamref name="TInput"/> to an arbitrary <see cref="object"/> of a particular type.
    /// Implements the <see cref="IConversion{TInput}" />
    /// </summary>
    /// <typeparam name="TInput">The type of the input to convert.</typeparam>
    /// <seealso cref="IConversion{TInput}" />
    public interface ITypeConverter<in TInput> : IConversion<TInput>
    {
        /// <summary>
        /// Converts the specified <paramref name="input"/> to an object of <typeparamref name="T"/>.
        /// </summary>
        /// <typeparam name="T">The type of the object to return.</typeparam>
        /// <param name="input">The <typeparamref name="TInput"/> to convert.</param>
        /// <returns>An object of <typeparamref name="T"/> equivalent to <paramref name="input"/>.</returns>
        T ChangeType<T>(TInput input);

        /// <summary>
        /// Converts the specified <paramref name="input"/> to an object of <paramref name="targetType"/>.
        /// </summary>
        /// <param name="input">The <typeparamref name="TInput"/> to convert.</param>
        /// <param name="targetType">The type of the object to return.</param>
        /// <returns>An object of <paramref name="targetType"/> equivalent to <paramref name="input"/>.</returns>
        object ChangeType(TInput input, Type targetType);
    }

    /// <summary>
    /// Defines methods that converts a <typeparamref name="TInput"/> to an arbitrary <see cref="object"/> of a particular type.
    /// Implements the <see cref="IConversion{TInput}" />
    /// </summary>
    /// <typeparam name="TInput">The type of the input to convert.</typeparam>
    /// <typeparam name="TOptions">The type of the delegate setup.</typeparam>
    /// <seealso cref="IConversion{TInput}" />
    public interface ITypeConverter<in TInput, out TOptions> : IConversion<TInput> where TOptions : class, new()
    {
        /// <summary>
        /// Converts the specified <paramref name="input"/> to an object of <typeparamref name="T"/>.
        /// </summary>
        /// <typeparam name="T">The type of the object to return.</typeparam>
        /// <param name="input">The <typeparamref name="TInput"/> to convert.</param>
        /// <param name="setup">The <typeparamref name="TOptions"/> which may be configured.</param>
        /// <returns>An object of <typeparamref name="T"/> equivalent to <paramref name="input"/>.</returns>
        T ChangeType<T>(TInput input, Action<TOptions> setup = null);

        /// <summary>
        /// Converts the specified <paramref name="input"/> to an object of <paramref name="targetType"/>.
        /// </summary>
        /// <param name="input">The <typeparamref name="TInput"/> to convert.</param>
        /// <param name="targetType">The type of the object to return.</param>
        /// <param name="setup">The <typeparamref name="TOptions"/> which may be configured.</param>
        /// <returns>An object of <paramref name="targetType"/> equivalent to <paramref name="input"/>.</returns>
        object ChangeType(TInput input, Type targetType, Action<TOptions> setup = null);
    }
}