using System;
using System.Net.Http;
using System.Net.Http.Headers;

namespace Cuemon.AspNetCore.Diagnostics
{
    /// <summary>
    /// Provides a way to support content negotiation for <see cref="HttpExceptionDescriptor" />.
    /// </summary>
    public class HttpExceptionDescriptorResponseHandler
    {
        private readonly Func<HttpExceptionDescriptor, HttpResponseMessage> _responseMessageFactory;

        /// <summary>
        /// Initializes a new instance of the <see cref="HttpExceptionDescriptorResponseHandler"/> class.
        /// </summary>
        /// <param name="contentType">The media type that this handler supports.</param>
        /// <param name="responseMessageFactory">The function delegate that produces the <see cref="HttpResponseMessage"/>.</param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="contentType"/> cannot be null -or-
        /// <paramref name="responseMessageFactory"/> cannot be null.
        /// </exception>
        public HttpExceptionDescriptorResponseHandler(MediaTypeHeaderValue contentType, Func<HttpExceptionDescriptor, HttpResponseMessage> responseMessageFactory)
        {
            Validator.ThrowIfNull(contentType, nameof(contentType));
            Validator.ThrowIfNull(responseMessageFactory, nameof(responseMessageFactory));

            ContentType = contentType;

            _responseMessageFactory = responseMessageFactory;
        }

        /// <summary>
        /// Gets the media type that this handler supports.
        /// </summary>
        /// <value>The media type that this handler supports.</value>
        public MediaTypeHeaderValue ContentType { get; }

        /// <summary>
        /// Converts this instance into an <see cref="HttpResponseMessage"/> from the by constructor provided factory.
        /// </summary>
        /// <param name="descriptor">The exception descriptor tailored for HTTP requests.</param>
        /// <returns>An <see cref="HttpResponseMessage"/> from the by constructor provided factory.</returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="descriptor"/> cannot be null.
        /// </exception>
        public HttpResponseMessage ToHttpResponseMessage(HttpExceptionDescriptor descriptor)
        {
            return _responseMessageFactory(descriptor);
        }
    }
}   
