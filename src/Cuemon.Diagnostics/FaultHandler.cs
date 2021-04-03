using System;

namespace Cuemon.Diagnostics
{
    /// <summary>
    /// Provides a generic way to implement a fault resolver that evaluate an exception and provide details about it in a developer friendly way.
    /// </summary>
    public abstract class FaultHandler<TDescriptor> where TDescriptor : ExceptionDescriptor
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="FaultResolver"/> class.
        /// </summary>
        /// <param name="validator">The function delegate that evaluates an <see cref="Exception"/>.</param>
        /// <param name="descriptor">The function delegate that provides details about an <see cref="Exception"/>.</param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="validator"/> cannot be null -or-
        /// <paramref name="descriptor"/> cannot be null.
        /// </exception>
        protected FaultHandler(Func<Exception, bool> validator, Func<Exception, TDescriptor> descriptor)
        {
            Validator.ThrowIfNull(validator, nameof(validator));
            Validator.ThrowIfNull(descriptor, nameof(descriptor));
            ValidatorCallback = validator;
            DescriptorCallback = descriptor;
        }

        /// <summary>
        /// Gets the function delegate that provides details about an <see cref="Exception"/>.
        /// </summary>
        /// <value>The function delegate that provides details about an <see cref="Exception"/>.</value>
        public Func<Exception, TDescriptor> DescriptorCallback { get; }

        /// <summary>
        /// Gets the function delegate that evaluates an <see cref="Exception"/>.
        /// </summary>
        /// <value>The function delegate that evaluates an <see cref="Exception"/>.</value>
        public Func<Exception, bool> ValidatorCallback { get; }
    }
}