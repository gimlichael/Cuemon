using System;

namespace Cuemon.ComponentModel
{
    internal static class Converter<TInput, TOutput>
    {
        public static TOutput UseConverter<TConverter>(TInput input) where TConverter : class, IConverter<TInput, TOutput>, new()
        {
            return Activator.CreateInstance<TConverter>().ChangeType(input);
        }
    }

    internal static class Converter<TInput, TOutput, TOptions> where TOptions : class, new()
    {
        public static TOutput UseConverter<TConverter>(TInput input, Action<TOptions> setup) where TConverter : class, IConverter<TInput, TOutput, TOptions>, new()
        {
            return Activator.CreateInstance<TConverter>().ChangeType(input, setup);
        }
    }
}