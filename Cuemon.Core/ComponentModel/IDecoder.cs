using System;

namespace Cuemon.ComponentModel
{
    /// <summary>
    /// Provides a way to handle decoding for a given context.
    /// Implements the <see cref="IConversion" />
    /// </summary>
    /// <seealso cref="IConversion" />
    public interface IDecoder : IConversion
    {
    }

    /// <summary>
    /// Provides a way to handle decoding for a given context.
    /// Implements the <see cref="IDecoder" />
    /// Implements the <see cref="IConversion{TInput, TOuput}" />
    /// </summary>
    /// <typeparam name="TEncoded">The type of the object to decode into <typeparamref name="T"/>.</typeparam>
    /// <typeparam name="T">The type of the decoded result.</typeparam>
    /// <seealso cref="IDecoder" />
    /// <seealso cref="IConversion{TInput, TOuput}" />
    public interface IDecoder<in TEncoded, out T> : IDecoder, IConversion<TEncoded, T>
    {
        /// <summary>
        /// Converts the <paramref name="encoded"/> value to its <typeparamref name="T"/> decoded equivalent.
        /// </summary>
        /// <param name="encoded">The value to decode.</param>
        /// <returns>An <typeparamref name="T"/> decoded equivalent to <paramref name="encoded"/>.</returns>
        T Decode(TEncoded encoded);
    }

    /// <summary>
    /// Provides a way to handle decoding for a given context.
    /// Implements the <see cref="IDecoder" />
    /// Implements the <see cref="IConversion{TInput, TOuput, TOptions}" />
    /// </summary>
    /// <typeparam name="TEncoded">The type of the object to decode into <typeparamref name="T"/>.</typeparam>
    /// <typeparam name="T">The type of the decoded result.</typeparam>
    /// <typeparam name="TOptions">The type of the configuration options class having a default constructor.</typeparam>
    /// <seealso cref="IDecoder" />
    /// <seealso cref="IConversion{TInput, TOuput, TOptions}" />
    public interface IDecoder<in TEncoded, out T, out TOptions> : IDecoder, IConversion<TEncoded, T, TOptions> where TOptions : class, new()
    {
        /// <summary>
        /// Converts the <paramref name="encoded"/> value to its <typeparamref name="T"/> decoded equivalent.
        /// </summary>
        /// <param name="encoded">The value to decode.</param>
        /// <param name="setup">The <typeparamref name="TOptions"/> which may be configured.</param>
        /// <returns>An <typeparamref name="T"/> decoded equivalent to <paramref name="encoded"/>.</returns>
        T Decode(TEncoded encoded, Action<TOptions> setup = null);
    }
}