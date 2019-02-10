using Cuemon.AspNetCore.Http;

namespace Cuemon.AspNetCore.Authentication
{
    /// <summary>
    /// The exception that is thrown when the requirements of an HTTP WWW-Authenticated header is not meet.
    /// </summary>
    /// <seealso cref="HttpStatusCodeException" />
    public class UnauthorizedException : HttpStatusCodeException
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="UnauthorizedException"/> class.
        /// </summary>
        /// <param name="statusCode">The HTTP status code to associate with this exception.</param>
        /// <param name="message">The message that describes the HTTP status code.</param>
        public UnauthorizedException(int statusCode, string message) : base(statusCode, message)
        {
        }
    }
}