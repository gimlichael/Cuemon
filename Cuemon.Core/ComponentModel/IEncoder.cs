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
    /// <typeparam name="TInput">The type of the object to encode into <typeparamref name="TOutput"/>.</typeparam>
    /// <typeparam name="TOutput">The type of the object to decode into <typeparamref name="TInput"/>.</typeparam>
    /// <seealso cref="IEncoder" />
    /// <seealso cref="IConversion{TInput, TOuput}" />
    public interface IEncoder<in TInput, out TOutput> : IEncoder, IConversion<TInput, TOutput>
    {
        TOutput Encode(TInput input);
    }

    /// <summary>
    /// Provides a way to handle encoding for a given context.
    /// Implements the <see cref="IEncoder" />
    /// Implements the <see cref="IConversion{TInput, TOuput, TOptions}" />
    /// </summary>
    /// <typeparam name="TInput">The type of the object to encode into <typeparamref name="TOutput"/>.</typeparam>
    /// <typeparam name="TOutput">The type of the object to decode into <typeparamref name="TInput"/>.</typeparam>
    /// <typeparam name="TOptions">The type of the configuration options class having a default constructor.</typeparam>
    /// <seealso cref="IEncoder" />
    /// <seealso cref="IConversion{TInput, TOuput, TOptions}" />
    public interface IEncoder<in TInput, out TOutput, out TOptions> : IEncoder, IConversion<TInput, TOutput, TOptions> where TOptions : class, new()
    {
        TOutput Encode(TInput input, Action<TOptions> setup = null);
    }
}