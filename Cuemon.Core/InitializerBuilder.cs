using System;

namespace Cuemon
{
    /// <summary>
    /// Supports the <see cref="Initializer"/> for building custom initializers.
    /// </summary>
    /// <typeparam name="T">The type of object to wrap.</typeparam>
    public class InitializerBuilder<T> where T : class
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="InitializerBuilder{T}"/> class.
        /// </summary>
        /// <param name="instance">The instance to initialize within a protective wrapping.</param>
        internal InitializerBuilder(T instance)
        {
            Validator.ThrowIfNull(instance, nameof(instance));
            Instance = instance;
        }

        /// <summary>
        /// Gets the initialized instance.
        /// </summary>
        /// <value>The initialized instance.</value>
        public T Instance { get; }

        /// <summary>
        /// Ignores any <see cref="MissingMethodException"/> that might be thrown by instance <typeparamref name="T"/>.
        /// </summary>
        /// <param name="initializer">The delegate that will continue initializing instance <typeparamref name="T"/> while ignoring any <see cref="MissingMethodException"/>.</param>
        /// <returns>A reference to this instance.</returns>
        public InitializerBuilder<T> IgnoreMissingMethod(Action<T> initializer)
        {
            return Ignore(initializer, ex => ex is MissingMethodException);
        }

        /// <summary>
        /// Ignores any exceptions that might be thrown by instance <typeparamref name="T"/>.
        /// </summary>
        /// <param name="initializer">The delegate that will continue initializing instance <typeparamref name="T"/> while ignoring any exceptions.</param>
        /// <returns>A reference to this instance.</returns>
        public InitializerBuilder<T> IgnoreAny(Action<T> initializer)
        {
            return Ignore(initializer, ex => true);
        }

        /// <summary>
        /// Ignores exceptions thrown by instance <typeparamref name="T"/> that is specified by the function delegate <paramref name="ignorer"/>.
        /// </summary>
        /// <param name="initializer">The delegate that will continue initializing instance <typeparamref name="T"/> while ignoring any exceptions specified by <paramref name="ignorer"/>.</param>
        /// <param name="ignorer">The function delegate that will parse thrown exceptions and ignore those specified.</param>
        /// <returns>A reference to this instance.</returns>
        public InitializerBuilder<T> Ignore(Action<T> initializer, Func<Exception, bool> ignorer)
        {
            Validator.ThrowIfNull(ignorer, nameof(ignorer));
            Validator.ThrowIfNull(initializer, nameof(initializer));
            try
            {
                initializer(Instance);
            }
            catch (Exception ex)
            {
                if (!ignorer(ex)) { throw; }
            }
            return this;
        }
    }
}