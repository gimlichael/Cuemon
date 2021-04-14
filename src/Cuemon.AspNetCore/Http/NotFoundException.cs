using System;
using System.Runtime.Serialization;
using Microsoft.AspNetCore.Http;

namespace Cuemon.AspNetCore.Http
{
    /// <summary>
    /// The exception that is thrown when the server can not find the requested resource.
    /// </summary>
    /// <seealso cref="HttpStatusCodeException" />
    [Serializable]
    public class NotFoundException : HttpStatusCodeException
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="NotFoundException"/> class.
        /// </summary>
        /// <param name="innerException">The exception that is the cause of the current exception.</param>
        public NotFoundException(Exception innerException) : this(default, innerException)
        {
            
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="NotFoundException"/> class.
        /// </summary>
        /// <param name="message">The message that describes the HTTP status code.</param>
        /// <param name="innerException">The exception that is the cause of the current exception.</param>
        public NotFoundException(string message = null, Exception innerException = null) : base(StatusCodes.Status404NotFound, message ?? "The server has not found anything matching the request URI.", innerException)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="NotFoundException"/> class.
        /// </summary>
        /// <param name="info">The object that holds the serialized object data.</param>
        /// <param name="context">The contextual information about the source or destination.</param>
        protected NotFoundException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}