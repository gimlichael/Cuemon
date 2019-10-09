using System;

namespace Cuemon.ComponentModel
{
    /// <summary>
    /// Provides a way to handle encoding for a given context.
    /// Implements the <see cref="IEncoder" />
    /// Implements the <see cref="IConversion" />
    /// </summary>
    /// <seealso cref="IEncoder" />
    /// <seealso cref="IConversion" />
    public interface IEncoder : IConversion
    {
    }

    /// <summary>
    /// Provides a way to handle encoding for a given context.
    /// Implements the <see cref="IEncoder" />
    /// Implements the <see cref="IConversion{TInput, TOuput}" />
    /// </summary>
    /// <typeparam name="TInput">The type of the object to encode into <typeparamref name="TResult"/>.</typeparam>
    /// <typeparam name="TResult">The type of the object to decode into <typeparamref name="TInput"/>.</typeparam>
    /// <seealso cref="IEncoder" />
    /// <seealso cref="IConversion{TInput, TOuput}" />
    public interface IEncoder<in TInput, out TResult> : IEncoder, IConversion<TInput, TResult>
    {
        /// <summary>
        /// Converts the <paramref name="input"/> to its <typeparamref name="TResult"/> encoded equivalent.
        /// </summary>
        /// <param name="input">The input.</param>
        /// <returns>An <typeparamref name="TResult"/> encoded equivalent to <paramref name="input"/>.</returns>
        TResult Encode(TInput input);
    }

    /// <summary>
    /// Provides a way to handle encoding for a given context.
    /// Implements the <see cref="IEncoder" />
    /// Implements the <see cref="IConversion{TInput, TOuput, TOptions}" />
    /// </summary>
    /// <typeparam name="TInput">The type of the object to encode into <typeparamref name="TResult"/>.</typeparam>
    /// <typeparam name="TResult">The type of the object to decode into <typeparamref name="TInput"/>.</typeparam>
    /// <typeparam name="TOptions">The type of the configuration options class having a default constructor.</typeparam>
    /// <seealso cref="IEncoder" />
    /// <seealso cref="IConversion{TInput, TOuput, TOptions}" />
    public interface IEncoder<in TInput, out TResult, out TOptions> : IEncoder, IConversion<TInput, TResult, TOptions> where TOptions : class, new()
    {
        /// <summary>
        /// Converts the <paramref name="input"/> to its <typeparamref name="TResult"/> encoded equivalent.
        /// </summary>
        /// <param name="input">The input.</param>
        /// <param name="setup">The <typeparamref name="TOptions"/> which may be configured.</param>
        /// <returns>An <typeparamref name="TResult"/> encoded equivalent to <paramref name="input"/>.</returns>
        TResult Encode(TInput input, Action<TOptions> setup = null);
    }
}