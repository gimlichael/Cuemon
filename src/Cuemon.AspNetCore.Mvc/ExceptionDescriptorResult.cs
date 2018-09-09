using System;
using Cuemon.Diagnostics;
using Microsoft.AspNetCore.Mvc;

namespace Cuemon.AspNetCore.Mvc
{
    /// <summary>
    /// An <see cref="ObjectResult"/> that when executed will produce a response that varies depending on the encapsulated <see cref="Exception"/>.
    /// </summary>
    /// <seealso cref="ObjectResult" />
    public class ExceptionDescriptorResult : ObjectResult
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ExceptionDescriptorResult"/> class.
        /// </summary>
        /// <param name="value">The descriptor value to return.</param>
        public ExceptionDescriptorResult(ExceptionDescriptor value) : base(value)
        {
        }
    }
}