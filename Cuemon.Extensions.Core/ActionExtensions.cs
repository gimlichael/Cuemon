using System;

namespace Cuemon
{
    /// <summary>
    /// Extension methods for the <see cref="Action"/> delegates.
    /// </summary>
    public static class ActionExtensions
    {
        /// <summary>
        /// Provides a generic way to support the options pattern which enables using custom options classes to represent a group of related settings.
        /// </summary>
        /// <typeparam name="TOptions">The type of the custom options class.</typeparam>
        /// <param name="setup">The delegate that will configure the public read-write properties of <typeparamref name="TOptions"/>.</param>
        /// <returns>A default constructed instance of <typeparamref name="TOptions"/> initialized with the options of <paramref name="setup"/>.</returns>
        public static TOptions Configure<TOptions>(this Action<TOptions> setup) where TOptions : class, new()
        {
            return Patterns.Configure(setup);
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
            return Patterns.Configure(setup);
        }
    }
}