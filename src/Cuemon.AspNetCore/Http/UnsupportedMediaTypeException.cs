using System;
using System.Runtime.Serialization;
using Microsoft.AspNetCore.Http;

namespace Cuemon.AspNetCore.Http
{
    /// <summary>
    /// The exception that is thrown when the media format of the requested data is not supported by the server, so the server is rejecting the request.
    /// </summary>
    /// <seealso cref="HttpStatusCodeException" />
    [Serializable]
    public class UnsupportedMediaTypeException : HttpStatusCodeException
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="UnsupportedMediaTypeException"/> class.
        /// </summary>
        /// <param name="innerException">The exception that is the cause of the current exception.</param>
        public UnsupportedMediaTypeException(Exception innerException) : this(default, innerException)
        {
            
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="UnsupportedMediaTypeException"/> class.
        /// </summary>
        /// <param name="message">The message that describes the HTTP status code.</param>
        /// <param name="innerException">The exception that is the cause of the current exception.</param>
        public UnsupportedMediaTypeException(string message = null, Exception innerException = null) : base(StatusCodes.Status415UnsupportedMediaType, message ?? "The server is refusing to service the request because the entity of the request is in a format not supported by the requested resource for the requested method.", innerException)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="UnsupportedMediaTypeException"/> class.
        /// </summary>
        /// <param name="info">The object that holds the serialized object data.</param>
        /// <param name="context">The contextual information about the source or destination.</param>
        protected UnsupportedMediaTypeException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}