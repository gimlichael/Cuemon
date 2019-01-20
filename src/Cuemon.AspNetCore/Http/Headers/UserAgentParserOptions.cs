using System;
using System.Collections.Generic;
using System.Text;

namespace Cuemon.AspNetCore.Http.Headers
{
    /// <summary>
    /// Configuration options for <see cref="UserAgentParserMiddleware"/>.
    /// </summary>
    public class UserAgentParserOptions
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="UserAgentParserOptions"/> class.
        /// </summary>
        public UserAgentParserOptions()
        {
            RequirementsFailedBody = () =>
            {
                return "The requirements of the HTTP User-Agent header was not met.".ToByteArray(o =>
                {
                    o.Encoding = Encoding.UTF8;
                    o.Preamble = PreambleSequence.Remove;
                });
            };
            AllowedUserAgents = new List<string>();
        }

        /// <summary>
        /// Gets or sets a value indicating whether a HTTP User-Agent header must be present in the request.
        /// </summary>
        /// <value><c>true</c> if the HTTP User-Agent header must be present in the request; otherwise, <c>false</c>.</value>
        public bool RequireUserAgentHeader { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether a HTTP User-Agent header must be validated against <see cref="AllowedUserAgents"/>.
        /// </summary>
        /// <value><c>true</c> if the HTTP User-Agent header must be validated against <see cref="AllowedUserAgents"/>; otherwise, <c>false</c>.</value>
        public bool ValidateUserAgentHeader { get; set; }

        /// <summary>
        /// Gets a list of whitelisted user agents.
        /// </summary>
        /// <value>A list of whitelisted user agents.</value>
        public IList<string> AllowedUserAgents { get; }

        /// <summary>
        /// Gets or sets the function delegate that retrieves the body of a request missing the requirements of a User-Agent header.
        /// </summary>
        /// <value>A <see cref="Func{TResult}"/> that retrieves the body of a request missing the requirements of a User-Agent header.</value>
        public Func<byte[]> RequirementsFailedBody { get; set; }
    }
}