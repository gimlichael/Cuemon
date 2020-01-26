using System;
using Microsoft.AspNetCore.Http;

namespace Cuemon.AspNetCore.Http
{
    /// <summary>
    /// Provides a base-class for exceptions based on an HTTP status code.
    /// </summary>
    /// <seealso cref="Exception" />
    public abstract class HttpStatusCodeException : Exception
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="HttpStatusCodeException"/> class.
        /// </summary>
        /// <param name="statusCode">The HTTP status code to associate with this exception.</param>
        /// <param name="message">The message that describes the HTTP status code.</param>
        protected HttpStatusCodeException(int statusCode, string message) : base(message)
        {
            Validator.ThrowIfLowerThan(statusCode, StatusCodes.Status100Continue, nameof(statusCode), FormattableString.Invariant($"{nameof(statusCode)} cannot be less than {StatusCodes.Status100Continue}."));
            Validator.ThrowIfGreaterThan(statusCode, StatusCodes.Status511NetworkAuthenticationRequired, nameof(statusCode), FormattableString.Invariant($"{nameof(statusCode)} cannot be greater than {StatusCodes.Status511NetworkAuthenticationRequired}."));
            StatusCode = statusCode;
        }

        /// <summary>
        /// Gets the HTTP status code associated with this exception.
        /// </summary>
        /// <value>The HTTP status code associated with this exception.</value>
        public int StatusCode { get; }
    }
}