using System;
using System.Runtime.Serialization;
using Microsoft.AspNetCore.Http;

namespace Cuemon.AspNetCore.Http
{
    /// <summary>
    /// The exception that is thrown when a request conflicts with the current state of the server.
    /// </summary>
    /// <seealso cref="HttpStatusCodeException" />
    [Serializable]
    public class ConflictException : HttpStatusCodeException
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ConflictException"/> class.
        /// </summary>
        /// <param name="message">The message that describes the HTTP status code.</param>
        public ConflictException(string message = "The request could not be completed due to a conflict with the current state of the resource.") : base(StatusCodes.Status409Conflict, message)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ConflictException"/> class.
        /// </summary>
        /// <param name="info">The object that holds the serialized object data.</param>
        /// <param name="context">The contextual information about the source or destination.</param>
        protected ConflictException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}