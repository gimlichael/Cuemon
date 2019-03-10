using System;

namespace Cuemon
{
    /// <summary>
    /// This is an extension implementation of the most common methods on the <see cref="DelegateUtility"/> class.
    /// </summary>
    public static class DelegateUtilityExtensions
    {
        /// <summary>
        /// Provides an easy and reflection less way to get a value from a property that is delegate compatible (such as <see cref="Func{TResult}"/> and the likes thereof).
        /// </summary>
        /// <typeparam name="TResult">The type of the return value of <paramref name="output"/>.</typeparam>
        /// <param name="output">The return value of a member to be routed as output through this Wrap{TResult} method.</param>
        /// <returns>The value from <paramref name="output"/>.</returns>
        public static TResult Wrap<TResult>(this TResult output)
        {
            return DelegateUtility.Wrap(output);
        }

        /// <summary>
        /// Provides a way to dynamically wrap a return value (typically from a property or field) inside an anonymous method.
        /// </summary>
        /// <typeparam name="TResult">The type of the return value of <paramref name="output" />.</typeparam>
        /// <param name="output">The value to dynamically wrap as a return value (typically from a property of field) inside an anonymous method.</param>
        /// <returns>An anonymous method that returns the value of <paramref name="output" />.</returns>
        public static Func<TResult> DynamicWrap<TResult>(this TResult output)
        {
            return DelegateUtility.DynamicWrap(output);
        }

        /// <summary>
        /// Provides a generic way to support the options pattern which enables using custom options classes to represent a group of related settings.
        /// </summary>
        /// <typeparam name="TOptions">The type of the custom options class.</typeparam>
        /// <param name="setup">The delegate that will configure the public read-write properties of <typeparamref name="TOptions"/>.</param>
        /// <returns>A default constructed instance of <typeparamref name="TOptions"/> initialized with the options of <paramref name="setup"/>.</returns>
        public static TOptions Configure<TOptions>(this Action<TOptions> setup) where TOptions : class, new()
        {
            return DelegateUtility.ConfigureAction(setup);
        }

        /// <summary>
        /// Provides a generic way to initialize the default, parameterless constructed instance of <typeparamref name="T"/>.
        /// </summary>
        /// <typeparam name="T">The type of the class having a default constructor.</typeparam>
        /// <param name="setup">The delegate that will initialize the public read-write properties of <typeparamref name="T"/>.</param>
        /// <returns>A default constructed instance of <typeparamref name="T"/> initialized with <paramref name="setup"/>.</returns>
        /// <remarks>This method is equivalent to <see cref="Configure{TOptions}"/>; but duplicated for more clear intend of use.</remarks>
        public static T CreateInstance<T>(this Action<T> setup) where T : class, new()
        {
            return DelegateUtility.ConfigureAction(setup);
        }
    }
}