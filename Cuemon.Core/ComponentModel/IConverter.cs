using System;

namespace Cuemon.ComponentModel
{
    public interface IConverter : IConversion
    {
    }

    public interface IConverter<in TInput, out TOutput> : IConverter, IConversion<TInput, TOutput>
    {
        TOutput ChangeType(TInput input);
    }

    public interface IConverter<in TInput, out TOutput, out TOptions> : IConverter, IConversion<TInput, TOutput, TOptions> where TOptions : class, new()
    {
        TOutput ChangeType(TInput input, Action<TOptions> setup = null);
    }
}