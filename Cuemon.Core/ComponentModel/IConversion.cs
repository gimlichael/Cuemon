namespace Cuemon.ComponentModel
{
    public interface IConversion
    {
    }

    public interface IConversion<in TInput> : IConversion
    {
    }

    public interface IConversion<in TInput, out TOutput> : IConversion<TInput>
    {
    }

    public interface IConversion<in TInput, out TOutput, out TOptions> : IConversion<TInput, TOutput> where TOptions : class, new()
    {
    }
}