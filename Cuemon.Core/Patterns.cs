using System;

namespace Cuemon
{
    /// <summary>
    /// Provides a generic way to support different types of design patterns with small utility methods.
    /// </summary>
    public sealed class Patterns
    {
        private static readonly Patterns ExtendedPatterns = new Patterns();

        /// <summary>
        /// Gets the singleton instance of the Patterns functionality allowing for extensions methods like: <c>Patterns.Use.SomeIngeniousMethod()</c>.
        /// </summary>
        /// <value>The singleton instance of the Patterns functionality.</value>
        public static Patterns Use { get; } = ExtendedPatterns;

        /// <summary>
        /// Returns a value that indicates whether the specified <paramref name="method"/> can be invoked without an exception.
        /// </summary>
        /// <typeparam name="TResult">The type of the return value of the <paramref name="method"/>.</typeparam>
        /// <param name="method">The function delegate that will resolve <paramref name="result"/>.</param>
        /// <param name="result">When this method returns, contains the value returned from <paramref name="method"/>; otherwise the default value for the type of the <paramref name="result"/> parameter if an exception is thrown.</param>
        /// <returns><c>true</c> if an instance of <typeparamref name="TResult"/> has been created; otherwise <c>false</c>.</returns>
        /// <remarks>Often referred to as the Try-Parse pattern: https://docs.microsoft.com/en-us/dotnet/standard/design-guidelines/exceptions-and-performance</remarks>
        public static bool TryParse<TResult>(Func<TResult> method, out TResult result)
        {
            try
            {
                Validator.ThrowIfNull(method, nameof(method));
                result = method();
                return true;
            }
            catch (Exception)
            {
                result = default;
                return false;
            }
        }

        /// <summary>
        /// Returns the default parameterless constructed instance of <typeparamref name="TOptions"/> configured with <paramref name="setup"/> delegate.
        /// </summary>
        /// <typeparam name="TOptions">The type of the class having a default constructor.</typeparam>
        /// <param name="setup">The delegate that will configure the public read-write properties of <typeparamref name="TOptions"/>.</param>
        /// <param name="initializer">The delegate that will initialize the default parameterless constructed instance of <typeparamref name="TOptions"/>. Should only be used with third party libraries.</param>
        /// <returns>A default constructed instance of <typeparamref name="TOptions"/> initialized with the options of <paramref name="setup"/>.</returns>
        /// <remarks>Often referred to as part the Options pattern: https://docs.microsoft.com/en-us/aspnet/core/fundamentals/configuration/options</remarks>
        public static TOptions Configure<TOptions>(Action<TOptions> setup, Action<TOptions> initializer = null) where TOptions : class, new()
        {
            var options = Activator.CreateInstance<TOptions>();
            initializer?.Invoke(options);
            setup?.Invoke(options);
            return options;
        }
    }
}