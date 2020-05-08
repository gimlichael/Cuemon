using System;
using Cuemon.ComponentModel;
using Cuemon.Text;

namespace Cuemon
{
    /// <summary>
    /// Provides access to factory methods that are tailored for convert operations following the patterns defined in <see cref="IConverter{TInput,TResult}"/> and <see cref="ITypeConverter{TInput,TOptions}"/>.
    /// </summary>
    public static class ConvertFactory
    {
        /// <summary>
        /// Uses the encoder.
        /// </summary>
        /// <typeparam name="TEncoder">The type of the t encoder.</typeparam>
        /// <returns>TEncoder.</returns>
        /// <seealso cref="StringEncoder"/>
        public static TEncoder UseEncoder<TEncoder>() where TEncoder : class, IEncoder, new()
        {
            return Activator.CreateInstance<TEncoder>();
        }
    }
}