using System;

namespace Cuemon.ComponentModel
{
    /// <summary>
    /// Defines methods that converts a <typeparamref name="TInput"/> to its equivalent <typeparamref name="TResult"/>.
    /// Implements the <see cref="IConversion{TInput, TOutput}" />
    /// </summary>
    /// <typeparam name="TInput">The type of the input to convert.</typeparam>
    /// <typeparam name="TResult">The type of the converted result.</typeparam>
    /// <seealso cref="IConversion{TInput, TResult}" />
    public interface IConverter<in TInput, out TResult> : IConversion<TInput, TResult>
    {
        /// <summary>
        /// Converts the <paramref name="input"/> to its <typeparamref name="TResult"/> equivalent.
        /// </summary>
        /// <param name="input">The <typeparamref name="TInput"/> to convert.</param>
        /// <returns>A <typeparamref name="TResult"/> equivalent to <paramref name="input"/>.</returns>
        TResult ChangeType(TInput input);
    }

    /// <summary>
    /// Defines methods that converts a <typeparamref name="TInput"/> to its equivalent <typeparamref name="TResult"/>.
    /// Implements the <see cref="IConversion{TInput, TOutput, TOptions}" />
    /// </summary>
    /// <typeparam name="TInput">The type of the input to convert.</typeparam>
    /// <typeparam name="TResult">The type of the converted result.</typeparam>
    /// <typeparam name="TOptions">The type of the delegate setup.</typeparam>
    /// <seealso cref="IConversion{TInput, TResult, TOptions}" />
    public interface IConverter<in TInput, out TResult, out TOptions> : IConversion<TInput, TResult, TOptions> where TOptions : class, new()
    {
        /// <summary>
        /// Converts the <paramref name="input"/> to its <typeparamref name="TResult"/> equivalent.
        /// </summary>
        /// <param name="input">The <typeparamref name="TInput"/> to convert.</param>
        /// <param name="setup">The <typeparamref name="TOptions"/> which may be configured.</param>
        /// <returns>A <typeparamref name="TResult"/> equivalent to <paramref name="input"/>.</returns>
        TResult ChangeType(TInput input, Action<TOptions> setup = null);
    }
}