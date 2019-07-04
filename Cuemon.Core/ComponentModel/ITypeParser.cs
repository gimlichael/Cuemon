using System;

namespace Cuemon.ComponentModel
{
    public interface ITypeParser : IParser
    {
        T Parse<T>(string input);

        object Parse(string input, Type targetType);

        bool TryParse<T>(string input, out T result);

        bool TryParse(string input, Type targetType, out object result);
    }

    public interface ITypeParser<out TOptions> : IParser where TOptions : class, new()
    {
        T Parse<T>(string input, Action<TOptions> setup = null);

        object Parse(string input, Type targetType, Action<TOptions> setup = null);

        bool TryParse<T>(string input, out T result, Action<TOptions> setup = null);

        bool TryParse(string input, Type targetType, out object result, Action<TOptions> setup = null);
    }
}