using System;

namespace Cuemon.ComponentModel
{
    public interface ITypeConversion
    {
    }

    public interface ITypeConversion<in TInput> : ITypeConversion
    {
    }

    public interface ITypeConversion<in TInput, out TOutput> : ITypeConversion<TInput>
    {
    }

    public interface ITypeConversion<in TInput, out TOutput, out TOptions> : ITypeConversion<TInput, TOutput> where TOptions : class, new()
    {
    }
}