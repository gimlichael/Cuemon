using System;

namespace Cuemon.Text
{
    /// <summary>
    /// Defines methods that converts a <see cref="string"/> to an arbitrary <see cref="object"/> of a particular type.
    /// Implements the <see cref="IParser" />
    /// </summary>
    /// <seealso cref="IParser" />
    public interface ITypeParser : IParser
    {
        /// <summary>
        /// Converts the specified <paramref name="input"/> to an object of <typeparamref name="T"/>.
        /// </summary>
        /// <typeparam name="T">The type of the object to return.</typeparam>
        /// <param name="input">The string to convert.</param>
        /// <returns>An object of <typeparamref name="T"/> equivalent to <paramref name="input"/>.</returns>
        T Parse<T>(string input);
        
        /// <summary>
        /// Converts the specified <paramref name="input"/> to an object of <paramref name="targetType"/>.
        /// </summary>
        /// <param name="input">The string to convert.</param>
        /// <param name="targetType">The type of the object to return.</param>
        /// <returns>An object of <paramref name="targetType"/> equivalent to <paramref name="input"/>.</returns>
        object Parse(string input, Type targetType);

        /// <summary>
        /// Converts the specified <paramref name="input"/> to an object of <typeparamref name="T"/>. A return value indicates whether the conversion succeeded.
        /// </summary>
        /// <typeparam name="T">The type of the object to return.</typeparam>
        /// <param name="input">The string to convert.</param>
        /// <param name="result">When this method returns, contains the object of <typeparamref name="T"/> equivalent to <paramref name="input"/>, if the conversion succeeded, or <c>default</c> if the conversion failed.</param>
        /// <returns><c>true</c> if <paramref name="input"/> was converted successfully; otherwise, <c>false</c>.</returns>
        bool TryParse<T>(string input, out T result);

        /// <summary>
        /// Converts the specified <paramref name="input"/> to an object of <paramref name="targetType"/>. A return value indicates whether the conversion succeeded.
        /// </summary>
        /// <param name="input">The string to convert.</param>
        /// <param name="targetType">The type of the object to return.</param>
        /// <param name="result">When this method returns, contains the object of <paramref name="targetType"/> equivalent to <paramref name="input"/>, if the conversion succeeded, or <c>default</c> if the conversion failed.</param>
        /// <returns><c>true</c> if <paramref name="input"/> was converted successfully; otherwise, <c>false</c>.</returns>
        bool TryParse(string input, Type targetType, out object result);
    }

    /// <summary>
    /// Defines methods that converts a <see cref="string"/> to an arbitrary <see cref="object"/> of a particular type.
    /// Implements the <see cref="IParser" />
    /// </summary>
    /// <typeparam name="TOptions">The type of the delegate setup.</typeparam>
    /// <seealso cref="IParser" />
    public interface ITypeParser<out TOptions> : IParser where TOptions : class, new()
    {
        /// <summary>
        /// Converts the specified <paramref name="input"/> to an object of <typeparamref name="T"/>.
        /// </summary>
        /// <typeparam name="T">The type of the object to return.</typeparam>
        /// <param name="input">The string to convert.</param>
        /// <param name="setup">The <typeparamref name="TOptions"/> which may be configured.</param>
        /// <returns>An object of <typeparamref name="T"/> equivalent to <paramref name="input"/>.</returns>
        T Parse<T>(string input, Action<TOptions> setup = null);

        /// <summary>
        /// Converts the specified <paramref name="input"/> to an object of <paramref name="targetType"/>.
        /// </summary>
        /// <param name="input">The string to convert.</param>
        /// <param name="targetType">The type of the object to return.</param>
        /// <param name="setup">The <typeparamref name="TOptions"/> which may be configured.</param>
        /// <returns>An object of <paramref name="targetType"/> equivalent to <paramref name="input"/>.</returns>
        object Parse(string input, Type targetType, Action<TOptions> setup = null);

        /// <summary>
        /// Converts the specified <paramref name="input"/> to an object of <typeparamref name="T"/>. A return value indicates whether the conversion succeeded.
        /// </summary>
        /// <typeparam name="T">The type of the object to return.</typeparam>
        /// <param name="input">The string to convert.</param>
        /// <param name="result">When this method returns, contains the object of <typeparamref name="T"/> equivalent to <paramref name="input"/>, if the conversion succeeded, or <c>default</c> if the conversion failed.</param>
        /// <param name="setup">The <typeparamref name="TOptions"/> which may be configured.</param>
        /// <returns><c>true</c> if <paramref name="input"/> was converted successfully; otherwise, <c>false</c>.</returns>
        bool TryParse<T>(string input, out T result, Action<TOptions> setup = null);

        /// <summary>
        /// Converts the specified <paramref name="input"/> to an object of <paramref name="targetType"/>. A return value indicates whether the conversion succeeded.
        /// </summary>
        /// <param name="input">The string to convert.</param>
        /// <param name="targetType">The type of the object to return.</param>
        /// <param name="result">When this method returns, contains the object of <paramref name="targetType"/> equivalent to <paramref name="input"/>, if the conversion succeeded, or <c>default</c> if the conversion failed.</param>
        /// <param name="setup">The <typeparamref name="TOptions"/> which may be configured.</param>
        /// <returns><c>true</c> if <paramref name="input"/> was converted successfully; otherwise, <c>false</c>.</returns>
        bool TryParse(string input, Type targetType, out object result, Action<TOptions> setup = null);
    }
}