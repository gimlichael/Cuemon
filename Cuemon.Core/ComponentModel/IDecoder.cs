using System;

namespace Cuemon.ComponentModel
{
    public interface IDecoder
    {
    }

    public interface IDecoder<TInput, TOutput> : IDecoder, IConversion<TInput, TOutput>
    {
        TInput Decode(TOutput input);
    }

    public interface IDecoder<TInput, TOuput, out TOptions> : IDecoder, IConversion<TInput, TOuput, TOptions> where TOptions : class, new()
    {
        TInput Decode(TOuput input, Action<TOptions> setup = null);
    }
}