namespace Cuemon.ComponentModel
{
    /// <summary>
    /// Defines a generic contract from which all kinds of conversions must implement.
    /// </summary>
    public interface IConversion
    {
    }

    /// <summary>
    /// Defines a generic conversion that handles <typeparamref name="TInput"/>.
    /// Implements the <see cref="IConversion" />
    /// </summary>
    /// <typeparam name="TInput">The type of the input to convert.</typeparam>
    /// <seealso cref="IConversion" />
    public interface IConversion<in TInput> : IConversion
    {
    }

    /// <summary>
    /// Defines a generic conversion that handles <typeparamref name="TInput"/> and <typeparamref name="TResult"/>.
    /// Implements the <see cref="IConversion{TInput}" />
    /// </summary>
    /// <typeparam name="TInput">The type of the input to convert.</typeparam>
    /// <typeparam name="TResult">The type of the converted result.</typeparam>
    /// <seealso cref="IConversion{TInput}" />
    public interface IConversion<in TInput, out TResult> : IConversion<TInput>
    {
    }

    /// <summary>
    /// Defines a generic conversion that handles <typeparamref name="TInput"/>, <typeparamref name="TResult"/> and <typeparamref name="TOptions"/>.
    /// Implements the <see cref="IConversion{TInput, TOutput}" />
    /// </summary>
    /// <typeparam name="TInput">The type of the input to convert.</typeparam>
    /// <typeparam name="TResult">The type of the converted result.</typeparam>
    /// <typeparam name="TOptions">The type of the delegate setup.</typeparam>
    /// <seealso cref="IConversion{TInput, TResult}" />
    public interface IConversion<in TInput, out TResult, out TOptions> : IConversion<TInput, TResult> where TOptions : class, new()
    {
    }
}