using System;
using Cuemon.AspNetCore.Http;

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
        public FaultResolver(Func<Exception, bool> validator, Func<Exception, HttpExceptionDescriptor> descriptor)
        {
            Cuemon.Validator.ThrowIfNull(validator, nameof(validator));
            Cuemon.Validator.ThrowIfNull(descriptor, nameof(descriptor));
            Validator = validator;
            Descriptor = descriptor;
        }

        internal Func<Exception, HttpExceptionDescriptor> Descriptor { get; private set; }

        internal Func<Exception, bool> Validator { get; set; }
    }
}