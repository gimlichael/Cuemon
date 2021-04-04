using System;
using System.Runtime.Serialization;
using Microsoft.AspNetCore.Http;

namespace Cuemon.AspNetCore.Http
{
    /// <summary>
    /// The exception that is thrown when the request entity is larger than limits defined by server.
    /// </summary>
    /// <seealso cref="HttpStatusCodeException" />
    [Serializable]
    public class PayloadTooLargeException : HttpStatusCodeException
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="PayloadTooLargeException"/> class.
        /// </summary>
        /// <param name="message">The message that describes the HTTP status code.</param>
        public PayloadTooLargeException(string message = "The server is refusing to process a request because the request entity is larger than the server is willing or able to process.") : base(StatusCodes.Status413PayloadTooLarge, message)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="PayloadTooLargeException"/> class.
        /// </summary>
        /// <param name="info">The object that holds the serialized object data.</param>
        /// <param name="context">The contextual information about the source or destination.</param>
        protected PayloadTooLargeException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}