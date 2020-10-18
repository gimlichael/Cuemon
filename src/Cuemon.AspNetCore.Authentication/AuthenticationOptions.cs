using System;
using System.Net;
using System.Net.Http;

namespace Cuemon.AspNetCore.Authentication
{
    /// <summary>
    /// Base options for all authentication middleware.
    /// </summary>
    public abstract class AuthenticationOptions
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="AuthenticationOptions"/> class.
        /// </summary>
        protected AuthenticationOptions()
        {
            ResponseHandler = () => new HttpResponseMessage(HttpStatusCode.Unauthorized)
            {
                Content = new StringContent(UnauthorizedMessage)
            };
            RequireSecureConnection = true;
            UnauthorizedMessage = "The request has not been applied because it lacks valid authentication credentials for the target resource.";
        }

        /// <summary>
        /// Gets or sets a value indicating whether a HTTP connection is required to use secure sockets (that is, HTTPS).
        /// </summary>
        /// <value><c>true</c> if the HTTP connection is required to use secure sockets (that is, HTTPS); otherwise, <c>false</c>.</value>
        public bool RequireSecureConnection { get; set; }

        /// <summary>
        /// Gets or sets the function delegate that configures the unauthorized response in the form of a <see cref="HttpResponseMessage"/>.
        /// </summary>
        /// <value>The function delegate that configures the unauthorized response in the form of a <see cref="HttpResponseMessage"/>.</value>
        public Func<HttpResponseMessage> ResponseHandler { get; set; }

        /// <summary>
        /// Gets or sets the message of an unauthorized request.
        /// </summary>
        /// <value>The message of an unauthorized request.</value>
        public string UnauthorizedMessage { get; set; }
    }
}