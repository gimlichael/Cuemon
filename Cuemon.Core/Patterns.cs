using System;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using Cuemon.ComponentModel.Converters;

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
        public static bool TryInvoke<TResult>(Func<TResult> method, out TResult result)
        {
            try
            {
                Validator.ThrowIfNull(method, nameof(method));
                result = method();
                return true;
            }
            catch (Exception ex)
            {
                if (ex is OutOfMemoryException || 
                    ex is StackOverflowException || 
                    ex is SEHException || 
                    ex is AccessViolationException || 
                    ex is ExecutionEngineException) // fatal exceptions; re-throw for .NET "legacy" (.NET Core will handle these by a high-level catch-all handler)
                {
                    throw;
                }
                result = default;
                return false;
            }
        }

        /// <summary>
        /// Returns an object of <typeparamref name="TResult"/>, or a default value if the specified <paramref name="method"/> throws an exception.
        /// </summary>
        /// <typeparam name="TResult">The type of the return value of the <paramref name="method"/>.</typeparam>
        /// <param name="method">The function delegate that will return an instance of <typeparamref name="TResult"/>.</param>
        /// <param name="fallbackResult">The value to return when the specified <paramref name="method"/> throws an exception. Default is <c>default</c> of <typeparamref name="TResult"/>.</param>
        /// <returns>An object of <typeparamref name="TResult"/> when the specified <paramref name="method"/> can be invoked without an exception; otherwise <paramref name="fallbackResult"/> is returned.</returns>
        public static TResult InvokeOrDefault<TResult>(Func<TResult> method, TResult fallbackResult = default)
        {
            return TryInvoke(method, out var result) ? result : fallbackResult;
        }

        /// <summary>
        /// Returns the default parameterless constructed instance of <typeparamref name="TOptions"/> configured with <paramref name="setup"/> delegate.
        /// </summary>
        /// <typeparam name="TOptions">The type of the configuration options class having a default constructor.</typeparam>
        /// <param name="setup">The delegate that will configure the public read-write properties of <typeparamref name="TOptions"/>.</param>
        /// <param name="initializer">The optional delegate that will initialize the default parameterless constructed instance of <typeparamref name="TOptions"/>. Should only be used with third party libraries or for validation purposes.</param>
        /// <param name="validator">The optional delegate that will validate the <typeparamref name="TOptions"/> configured by the <paramref name="setup"/> delegate.</param>
        /// <returns>A default constructed instance of <typeparamref name="TOptions"/> initialized with the options of <paramref name="setup"/>.</returns>
        /// <remarks>Often referred to as part the Options pattern: https://docs.microsoft.com/en-us/aspnet/core/fundamentals/configuration/options</remarks>
        public static TOptions Configure<TOptions>(Action<TOptions> setup, Action<TOptions> initializer = null, Action<TOptions> validator = null) where TOptions : class, new()
        {
            var options = Activator.CreateInstance<TOptions>();
            initializer?.Invoke(options);
            setup?.Invoke(options);
            validator?.Invoke(options);
            return options;
        }

        /// <summary>
        /// Returns the delegate that will configure the public read-write properties of <typeparamref name="TExchangeOptions"/>.
        /// </summary>
        /// <typeparam name="TOptions">The type of the configuration options class having a default constructor.</typeparam>
        /// <typeparam name="TExchangeOptions">The type of the configuration options class having a default constructor.</typeparam>
        /// <param name="setup">The delegate that will configure the public read-write properties of <typeparamref name="TOptions"/>.</param>
        /// <param name="initializer">The delegate that will exchange the parameter of <paramref name="setup"/> from <typeparamref name="TOptions"/> to <typeparamref name="TExchangeOptions"/>.</param>
        /// <returns>An <see cref="Action{TExchangeOptions}"/> otherwise equivalent to <paramref name="setup"/>.</returns>
        public static Action<TExchangeOptions> ConfigureExchange<TOptions, TExchangeOptions>(Action<TOptions> setup, Action<TOptions, TExchangeOptions> initializer = null)
            where TOptions : class, new()
            where TExchangeOptions : class, new()
        {
            var io = Configure(setup);
            if (initializer == null)
            {
                initializer = (i, o) =>
                {
                    var match = false;
                    var typeOfInput = typeof(TOptions);
                    var typeOfOutput = typeof(TExchangeOptions);
                    var ips = typeOfInput.GetRuntimeProperties().Where(pi => pi.CanRead && pi.CanWrite).ToList();
                    var ops = typeOfOutput.GetRuntimeProperties().Where(pi => pi.CanRead && pi.CanWrite).ToList();
                    foreach (var ip in ips)
                    {
                        var op = ops.SingleOrDefault(opi => opi.Name == ip.Name && opi.PropertyType == ip.PropertyType);
                        if (op != null)
                        {
                            op.SetValue(o, ip.GetValue(i));
                            match = true;
                        }
                    }
                    if (!match)
                    {
                        var ds = ConvertFactory.UseConverter<DelimitedStringConverter<PropertyInfo>>();
                        throw new InvalidOperationException(FormattableString.Invariant($"Unable to use default converter for exchange of {nameof(TOptions)} ({ds.ChangeType(ips)}) with {nameof(TExchangeOptions)} ({ds.ChangeType(ops)}); no match on public read-write properties."));
                    }
                };
            }
            return oo => initializer(io, oo);
        }

        /// <summary>
        /// Returns the delegate that will configure the public read-write properties of <typeparamref name="TOptions"/>.
        /// </summary>
        /// <typeparam name="TOptions">The type of the configuration options class having a default constructor.</typeparam>
        /// <param name="options">An instance of the configured options.</param>
        /// <returns>An <see cref="Action{TOptions}"/> otherwise equivalent to <paramref name="options"/>.</returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="options"/> cannot be null.
        /// </exception>
        public static Action<TOptions> ConfigureRevert<TOptions>(TOptions options)
        {
            Validator.ThrowIfNull(options, nameof(options));
            return o =>
            {
                var to = typeof(TOptions);
                var tops= to.GetRuntimeProperties().Where(pi => pi.CanRead && pi.CanWrite).ToList();
                foreach (var p in tops) { p.SetValue(o, p.GetValue(options)); }
            };
        }
    }
}