using System;
using Cuemon.AspNetCore.Diagnostics;

namespace Cuemon.AspNetCore.Mvc.Filters.Diagnostics
{
    /// <summary>
    /// Provides a way to evaluate an exception and provide details about it for in a developer friendly way.
    /// </summary>
    public class FaultResolver
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
        public FaultResolver(Func<Exception, bool> validator, Func<Exception, HttpExceptionDescriptor> descriptor)
        {
            Cuemon.Validator.ThrowIfNull(validator, nameof(validator));
            Cuemon.Validator.ThrowIfNull(descriptor, nameof(descriptor));
            Validator = validator;
            Descriptor = descriptor;
        }

        /// <summary>
        /// Gets the function delegate that provides details about an <see cref="Exception"/>.
        /// </summary>
        /// <value>The function delegate that provides details about an <see cref="Exception"/>.</value>
        public Func<Exception, HttpExceptionDescriptor> Descriptor { get; }

        /// <summary>
        /// Gets the function delegate that evaluates an <see cref="Exception"/>.
        /// </summary>
        /// <value>The function delegate that evaluates an <see cref="Exception"/>.</value>
        public Func<Exception, bool> Validator { get; }
    }
}