using System;
using System.Runtime.Serialization;
using Microsoft.AspNetCore.Http;

namespace Cuemon.AspNetCore.Http
{
    /// <summary>
    /// The exception that is thrown when the origin server requires the request to be conditional.
    /// </summary>
    /// <seealso cref="HttpStatusCodeException" />
    [Serializable]
    public class PreconditionRequiredException : HttpStatusCodeException
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="PreconditionRequiredException"/> class.
        /// </summary>
        /// <param name="message">The message that describes the HTTP status code.</param>
        public PreconditionRequiredException(string message = "No conditional request-header fields was supplied to the server.") : base(StatusCodes.Status428PreconditionRequired, message)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="PreconditionRequiredException"/> class.
        /// </summary>
        /// <param name="info">The object that holds the serialized object data.</param>
        /// <param name="context">The contextual information about the source or destination.</param>
        protected PreconditionRequiredException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}