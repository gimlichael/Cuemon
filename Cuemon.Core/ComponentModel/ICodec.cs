using Cuemon.ComponentModel.Decoders;
using Cuemon.ComponentModel.Encoders;

namespace Cuemon.ComponentModel
{
    /// <summary>
    /// Provides a way to handle both encoding and decoding for a given context.
    /// Implements the <see cref="IEncoder" />
    /// Implements the <see cref="IDecoder" />
    /// </summary>
    /// <seealso cref="IEncoder" />
    /// <seealso cref="IDecoder" />
    /// <remarks>Codec is a portmanteau of encoding (or coding) and decoding.</remarks>
    public interface ICodec : IEncoder, IDecoder
    {
    }

    /// <summary>
    /// Provides a way to handle both encoding and decoding for a given context.
    /// Implements the <see cref="ICodec" />
    /// Implements the <see cref="IEncoder{TInput, TOutput}" />
    /// Implements the <see cref="IDecoder{TInput, TOutput}" />
    /// </summary>
    /// <typeparam name="TInput">The type of the object to encode into <typeparamref name="TOutput"/>.</typeparam>
    /// <typeparam name="TOutput">The type of the object to decode into <typeparamref name="TInput"/>.</typeparam>
    /// <seealso cref="ICodec" />
    /// <seealso cref="IEncoder{TInput, TOutput}" />
    /// <seealso cref="IDecoder{TInput, TOutput}" />
    public interface ICodec<TInput, TOutput> : ICodec, IEncoder<TInput, TOutput>, IDecoder<TInput, TOutput>
    {
    }

    /// <summary>
    /// Provides a way to handle both encoding and decoding for a given context.
    /// Implements the <see cref="ICodec" />
    /// Implements the <see cref="IEncoder{TInput, TOutput, TOptions}" />
    /// Implements the <see cref="IDecoder{TInput, TOutput, TOptions}" />
    /// </summary>
    /// <typeparam name="TInput">The type of the object to encode into <typeparamref name="TOutput"/>.</typeparam>
    /// <typeparam name="TOutput">The type of the object to decode into <typeparamref name="TInput"/>.</typeparam>
    /// <typeparam name="TOptions">The type of the configuration options class having a default constructor.</typeparam>
    /// <seealso cref="ICodec" />
    /// <seealso cref="IEncoder{TInput, TOutput, TOptions}" />
    /// <seealso cref="IDecoder{TInput, TOutput, TOptions}" />
    public interface ICodec<TInput, TOutput, out TOptions> : ICodec, IEncoder<TInput, TOutput, TOptions>, IDecoder<TInput, TOutput, TOptions> where TOptions : class, new()
    {
    }
}