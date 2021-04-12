using System;
using System.Runtime.Serialization;
using Microsoft.AspNetCore.Http;

namespace Cuemon.AspNetCore.Http
{
    /// <summary>
    /// The exception that is thrown when the client has indicated preconditions in its headers which the server does not meet.
    /// </summary>
    /// <seealso cref="HttpStatusCodeException" />
    [Serializable]
    public class PreconditionFailedException : HttpStatusCodeException
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="PreconditionFailedException"/> class.
        /// </summary>
        /// <param name="message">The message that describes the HTTP status code.</param>
        public PreconditionFailedException(string message = "The precondition given in one or more of the request-header fields evaluated to false when it was tested on the server.") : base(StatusCodes.Status412PreconditionFailed, message)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="PreconditionFailedException"/> class.
        /// </summary>
        /// <param name="info">The object that holds the serialized object data.</param>
        /// <param name="context">The contextual information about the source or destination.</param>
        protected PreconditionFailedException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}