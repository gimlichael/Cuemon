using System;
using System.Runtime.Serialization;
using Microsoft.AspNetCore.Http;

namespace Cuemon.AspNetCore.Http
{
    /// <summary>
    /// The exception that is thrown when the user has sent too many requests in a given amount of time ("rate limiting").
    /// </summary>
    /// <seealso cref="HttpStatusCodeException" />
    [Serializable]
    public class TooManyRequestsException : HttpStatusCodeException
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="TooManyRequestsException"/> class.
        /// </summary>
        /// <param name="message">The message that describes the HTTP status code.</param>
        public TooManyRequestsException(string message = "The allowed number of requests has been exceeded.") : base(StatusCodes.Status429TooManyRequests, message)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="TooManyRequestsException"/> class.
        /// </summary>
        /// <param name="info">The object that holds the serialized object data.</param>
        /// <param name="context">The contextual information about the source or destination.</param>
        protected TooManyRequestsException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}