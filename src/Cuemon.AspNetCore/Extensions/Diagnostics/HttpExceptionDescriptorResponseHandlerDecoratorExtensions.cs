using System.Collections.Generic;
using System.Net.Http;
using System;

namespace Cuemon.AspNetCore.Diagnostics
{
    /// <summary>
    /// Extension methods for the <see cref="HttpExceptionDescriptorResponseHandler"/> class tailored to adhere the decorator pattern.
    /// </summary>
    /// <seealso cref="IDecorator{T}"/>
    /// <seealso cref="Decorator{T}"/>
    public static class HttpExceptionDescriptorResponseHandlerDecoratorExtensions
    {
        /// <summary>
        /// Adds the response handler.
        /// </summary>
        /// <param name="decorator">The decorator.</param>
        /// <param name="setup">The setup.</param>
        /// <returns>A reference to <see cref="P:IDecorator.Inner"/> of <paramref name="decorator" /> so that additional calls can be chained.</returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="decorator"/> cannot be null - or -
        /// <see cref="P:IDecorator.Inner"/> property of <paramref name="decorator"/> cannot be null - or -
        /// <paramref name="setup"/> cannot be null.
        /// </exception>
        /// <exception cref="ArgumentException">
        /// <paramref name="setup"/> failed to configure an instance of <see cref="HttpExceptionDescriptorResponseHandlerOptions"/> in a valid state.
        /// </exception>
        public static ICollection<HttpExceptionDescriptorResponseHandler> AddResponseHandler(this IDecorator<ICollection<HttpExceptionDescriptorResponseHandler>> decorator, Action<HttpExceptionDescriptorResponseHandlerOptions> setup)
        {
            Validator.ThrowIfNull(decorator, nameof(decorator), out var handlers);
            Validator.ThrowIfNull(setup);
            Validator.ThrowIfInvalidConfigurator(setup, nameof(setup), out var options);
            handlers.Add(new HttpExceptionDescriptorResponseHandler(options.ContentType, ed => new HttpResponseMessage()
            {
                Content = options.ContentFactory(ed),
                StatusCode = options.StatusCodeFactory(ed)
            }));
            return handlers;
        }
    }
}
