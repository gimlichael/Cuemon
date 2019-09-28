using System;
using Cuemon.ComponentModel;

namespace Cuemon.Text
{
    /// <summary>
    /// Defines the contract that converts a <see cref="string"/> to its equivalent generic type.
    /// Implements the <see cref="IConversion" />
    /// </summary>
    /// <seealso cref="IConversion" />
    public interface IParser : IConversion
    {
    }

    /// <summary>
    /// Defines methods that that converts a <see cref="string"/> to its equivalent <typeparamref name="TResult"/>.
    /// Implements the <see cref="IConversion{TInput,TResult}" />
    /// </summary>
    /// <seealso cref="IConversion{TInput,TResult}" />
    /// <typeparam name="TResult">The type of the converted result.</typeparam>
    public interface IParser<TResult> : IParser, IConversion<string, TResult>
    {
        /// <summary>
        /// Converts the <paramref name="input"/> to its <typeparamref name="TResult"/> equivalent.
        /// </summary>
        /// <param name="input">The string to convert.</param>
        /// <returns>A <typeparamref name="TResult"/> equivalent to <paramref name="input"/>.</returns>
        TResult Parse(string input);

        /// <summary>
        /// Converts the <paramref name="input"/> to its <typeparamref name="TResult"/> equivalent. A return value indicates whether the conversion succeeded.
        /// </summary>
        /// <param name="input">The string to convert.</param>
        /// <param name="result">When this method returns, contains the <typeparamref name="TResult"/> equivalent of the <paramref name="input"/>, if the conversion succeeded, or <c>default</c> if the conversion failed.</param>
        /// <returns><c>true</c> if <paramref name="input"/> was converted successfully; otherwise, <c>false</c>.</returns>
        bool TryParse(string input, out TResult result);
    }

    /// <summary>
    /// Defines methods that that converts a <see cref="string"/> to its equivalent <typeparamref name="TResult"/>.
    /// Implements the <see cref="IParser" />
    /// Implements the <see cref="IConversion{TInput,TOutput,TOptions}" />
    /// </summary>
    /// <typeparam name="TResult">The type of the converted result.</typeparam>
    /// <typeparam name="TOptions">The type of the delegate setup.</typeparam>
    /// <seealso cref="IParser" />
    /// <seealso cref="IConversion{TInput,TOutput,TOptions}" />
    public interface IParser<TResult, out TOptions> : IParser, IConversion<string, TResult, TOptions> where TOptions : class, new()
    {
        /// <summary>
        /// Converts the <paramref name="input"/> to its <typeparamref name="TResult"/> equivalent.
        /// </summary>
        /// <param name="input">The string to convert.</param>
        /// <param name="setup">The <typeparamref name="TOptions"/> which may be configured.</param>
        /// <returns>A <typeparamref name="TResult"/> equivalent to <paramref name="input"/>.</returns>
        TResult Parse(string input, Action<TOptions> setup = null);

        /// <summary>
        /// Converts the <paramref name="input"/> to its <typeparamref name="TResult"/> equivalent. A return value indicates whether the conversion succeeded.
        /// </summary>
        /// <param name="input">The string to convert.</param>
        /// <param name="result">When this method returns, contains the <typeparamref name="TResult"/> equivalent of the <paramref name="input"/>, if the conversion succeeded, or <c>default</c> if the conversion failed.</param>
        /// <param name="setup">The <typeparamref name="TOptions"/> which may be configured.</param>
        /// <returns><c>true</c> if <paramref name="input"/> was converted successfully; otherwise, <c>false</c>.</returns>
        bool TryParse(string input, out TResult result, Action<TOptions> setup = null);
    }
}