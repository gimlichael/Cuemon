using System;

namespace Cuemon.ComponentModel
{
    /// <summary>
    /// Provides a way to handle encoding for a given context.
    /// Implements the <see cref="IConversion" />
    /// </summary>
    /// <seealso cref="IConversion" />
    public interface IEncoder : IConversion
    {
    }

    /// <summary>
    /// Provides a way to handle encoding for a given context.
    /// Implements the <see cref="IEncoder" />
    /// Implements the <see cref="IConversion{TInput, TOuput}" />
    /// </summary>
    /// <typeparam name="T">The type of the object to encode into <typeparamref name="TEncoded"/>.</typeparam>
    /// <typeparam name="TEncoded">The type of the encoded result.</typeparam>
    /// <seealso cref="IEncoder" />
    /// <seealso cref="IConversion{TInput, TOuput}" />
    public interface IEncoder<in T, out TEncoded> : IEncoder, IConversion<T, TEncoded>
    {
        /// <summary>
        /// Converts the <paramref name="value"/> to its <typeparamref name="TEncoded"/> encoded equivalent.
        /// </summary>
        /// <param name="value">The value to encode.</param>
        /// <returns>An <typeparamref name="TEncoded"/> encoded equivalent to <paramref name="value"/>.</returns>
        TEncoded Encode(T value);
    }

    /// <summary>
    /// Provides a way to handle encoding for a given context.
    /// Implements the <see cref="IEncoder" />
    /// Implements the <see cref="IConversion{TInput, TOuput, TOptions}" />
    /// </summary>
    /// <typeparam name="T">The type of the object to encode into <typeparamref name="TEncoded"/>.</typeparam>
    /// <typeparam name="TEncoded">The type of the encoded result.</typeparam>
    /// <typeparam name="TOptions">The type of the configuration options class having a default constructor.</typeparam>
    /// <seealso cref="IEncoder" />
    /// <seealso cref="IConversion{TInput, TOuput, TOptions}" />
    public interface IEncoder<in T, out TEncoded, out TOptions> : IEncoder, IConversion<T, TEncoded, TOptions> where TOptions : class, new()
    {
        /// <summary>
        /// Converts the <paramref name="value"/> to its <typeparamref name="TEncoded"/> encoded equivalent.
        /// </summary>
        /// <param name="value">The value to encode.</param>
        /// <param name="setup">The <typeparamref name="TOptions"/> which may be configured.</param>
        /// <returns>An <typeparamref name="TEncoded"/> encoded equivalent to <paramref name="value"/>.</returns>
        TEncoded Encode(T value, Action<TOptions> setup = null);
    }
}