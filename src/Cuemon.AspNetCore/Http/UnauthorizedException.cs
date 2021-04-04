using System;
using System.Runtime.Serialization;
using Microsoft.AspNetCore.Http;

namespace Cuemon.AspNetCore.Http
{
    /// <summary>
    /// The exception that is thrown when the requirements of an HTTP WWW-Authenticate header is not meet.
    /// </summary>
    /// <seealso cref="HttpStatusCodeException" />
    [Serializable]
    public class UnauthorizedException : HttpStatusCodeException
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="UnauthorizedException"/> class.
        /// </summary>
        /// <param name="message">The message that describes the HTTP status code.</param>
        public UnauthorizedException(string message = "The request requires user authentication.") : base(StatusCodes.Status401Unauthorized, message)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="UnauthorizedException"/> class.
        /// </summary>
        /// <param name="info">The object that holds the serialized object data.</param>
        /// <param name="context">The contextual information about the source or destination.</param>
        protected UnauthorizedException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}