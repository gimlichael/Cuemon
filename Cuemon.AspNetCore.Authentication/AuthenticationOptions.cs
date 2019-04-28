using System;
using System.Text;
using Cuemon.Extensions;

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
            HttpNotAuthorizedBody = () =>
            {
                return "401 Unauthorized".ToByteArray(o =>
                {
                    o.Encoding = Encoding.UTF8;
                    o.Preamble = PreambleSequence.Remove;
                });
            };
        }

        /// <summary>
        /// Gets or sets a value indicating whether a HTTP connection is required to use secure sockets (that is, HTTPS).
        /// </summary>
        /// <value><c>true</c> if the HTTP connection is required to use secure sockets (that is, HTTPS); otherwise, <c>false</c>.</value>
        public bool RequireSecureConnection { get; set; }

        /// <summary>
        /// Gets or sets the function delegate for retrieving content for the body of an unauthorized request.
        /// </summary>
        /// <value>A <see cref="Func{TResult}"/> for retrieving content for the body of an unauthorized request.</value>
        public Func<byte[]> HttpNotAuthorizedBody { get; set; }
    }
}