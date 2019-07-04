using System;

namespace Cuemon.ComponentModel
{
    public interface ITypeConverter : IConverter, ITypeConversion
    {
    }

    public interface ITypeConverter<in TInput> : IConverter, ITypeConversion<TInput>
    {
        T Convert<T>(TInput input);

        object Convert(TInput input, Type targetType);
    }

    public interface ITypeConverter<in TInput, out TOptions> : IConverter, ITypeConversion<TInput> where TOptions : class, new()
    {
        T ChangeType<T>(TInput input, Action<TOptions> setup = null);

        object ChangeType(TInput input, Type targetType, Action<TOptions> setup = null);
    }
}