using Microsoft.AspNetCore.Http;
using System;
using System.Runtime.Serialization;

namespace Cuemon.AspNetCore.Http
{
    /// <summary>
    /// The exception that is thrown when the server has encountered a situation it does not know how to handle.
    /// </summary>
    /// <seealso cref="HttpStatusCodeException" />
    [Serializable]
    public class InternalServerErrorException : HttpStatusCodeException
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="InternalServerErrorException"/> class.
        /// </summary>
        public InternalServerErrorException() : this(null)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="InternalServerErrorException"/> class.
        /// </summary>
        /// <param name="innerException">The exception that is the cause of the current exception.</param>
        public InternalServerErrorException(Exception innerException) : this(default, innerException)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="InternalServerErrorException"/> class.
        /// </summary>
        /// <param name="message">The message that describes the HTTP status code.</param>
        /// <param name="innerException">The exception that is the cause of the current exception.</param>
        public InternalServerErrorException(string message, Exception innerException = null) : base(StatusCodes.Status500InternalServerError, message ?? "The server has encountered a situation it does not know how to handle.", innerException)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="InternalServerErrorException"/> class.
        /// </summary>
        /// <param name="info">The object that holds the serialized object data.</param>
        /// <param name="context">The contextual information about the source or destination.</param>
        protected InternalServerErrorException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}
