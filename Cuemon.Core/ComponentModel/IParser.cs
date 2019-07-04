using System;

namespace Cuemon.ComponentModel
{
    public interface IParser : IConversion
    {
    }

    public interface IParser<TResult> : IParser, IConversion<string, TResult>
    {
        TResult Parse(string input);

        bool TryParse(string input, out TResult result);
    }

    public interface IParser<TResult, out TOptions> : IParser, IConversion<string, TResult, TOptions> where TOptions : class, new()
    {
        TResult Parse(string input, Action<TOptions> setup = null);

        bool TryParse(string input, out TResult result, Action<TOptions> setup = null);
    }
}