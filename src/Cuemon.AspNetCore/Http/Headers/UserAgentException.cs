using System;
using System.Runtime.Serialization;

namespace Cuemon.AspNetCore.Http.Headers
{
    /// <summary>
    /// The exception that is thrown when the requirements of an HTTP User-Agent header is not meet.
    /// </summary>
    /// <seealso cref="HttpStatusCodeException" />
    [Serializable]
    public class UserAgentException : HttpStatusCodeException
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="UserAgentException"/> class.
        /// </summary>
        /// <param name="statusCode">The HTTP status code to associate with this exception.</param>
        /// <param name="message">The message that describes the HTTP status code.</param>
        public UserAgentException(int statusCode, string message) : base(statusCode, message)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="UserAgentException"/> class.
        /// </summary>
        /// <param name="info">The object that holds the serialized object data.</param>
        /// <param name="context">The contextual information about the source or destination.</param>
        protected UserAgentException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}