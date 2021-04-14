using System;
using System.Runtime.Serialization;
using Microsoft.AspNetCore.Http;

namespace Cuemon.AspNetCore.Http
{
    /// <summary>
    /// The exception that is thrown when the client does not have access rights to the content; that is, it is unauthorized, so the server is refusing to give the requested resource. Unlike 401, the client's identity is known to the server.
    /// </summary>
    /// <seealso cref="HttpStatusCodeException" />
    [Serializable]
    public class ForbiddenException : HttpStatusCodeException
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ForbiddenException"/> class.
        /// </summary>
        /// <param name="innerException">The exception that is the cause of the current exception.</param>
        public ForbiddenException(Exception innerException) : this(default, innerException)
        {
            
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ForbiddenException"/> class.
        /// </summary>
        /// <param name="message">The message that describes the HTTP status code.</param>
        /// <param name="innerException">The exception that is the cause of the current exception.</param>
        public ForbiddenException(string message = null, Exception innerException = null) : base(StatusCodes.Status403Forbidden, message ?? "The server understood the request, but is refusing to fulfill it.", innerException)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ForbiddenException"/> class.
        /// </summary>
        /// <param name="info">The object that holds the serialized object data.</param>
        /// <param name="context">The contextual information about the source or destination.</param>
        protected ForbiddenException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}