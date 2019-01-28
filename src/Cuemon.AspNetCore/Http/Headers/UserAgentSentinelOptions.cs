using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;

namespace Cuemon.AspNetCore.Http.Headers
{
    /// <summary>
    /// Configuration options for <see cref="UserAgentSentinelMiddleware"/>.
    /// </summary>
    public class UserAgentSentinelOptions
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="UserAgentSentinelOptions"/> class.
        /// </summary>
                /// <remarks>
        /// The following table shows the initial property values for an instance of <see cref="UserAgentSentinelOptions"/>.
        /// <list type="table">
        ///     <listheader>
        ///         <term>Property</term>
        ///         <description>Initial Value</description>
        ///     </listheader>
        ///     <item>
        ///         <term><see cref="AllowedUserAgents"/></term>
        ///         <description><c>new List{string{>}();</c></description>
        ///     </item>
        ///     <item>
        ///         <term><see cref="BadRequestMessage"/></term>
        ///         <description>The requirements of the HTTP User-Agent header was not met.</description>
        ///     </item>
        ///     <item>
        ///         <term><see cref="ForbiddenMessage"/></term>
        ///         <description>The HTTP User-Agent specified was rejected.</description>
        ///     </item>
        ///     <item>
        ///         <term><see cref="RequireUserAgentHeader"/></term>
        ///         <description><c>false</c></description>
        ///     </item>
        ///     <item>
        ///         <term><see cref="ValidateUserAgentHeader"/></term>
        ///         <description><c>false</c></description>
        ///     </item>
        ///     <item>
        ///         <term><see cref="ResponseBroker"/></term>
        ///         <description>A <see cref="HttpResponseMessage"/> initialized to either a HTTP status code 400 or 403 and a body of either <see cref="BadRequestMessage"/> or <see cref="ForbiddenMessage"/>.</description>
        ///     </item>
        ///     <item>
        ///         <term><see cref="UseGenericResponse"/></term>
        ///         <description><c>false</c></description>
        ///     </item>
        /// </list>
        /// </remarks>
        public UserAgentSentinelOptions()
        {
            UseGenericResponse = false;
            BadRequestMessage = "The requirements of the HTTP User-Agent header was not met.";
            ForbiddenMessage = "The HTTP User-Agent specified was rejected.";
            AllowedUserAgents = new List<string>();
            ResponseBroker = userAgent =>
            {
                var userAgentIsNullOrWhiteSpace = userAgent.IsNullOrWhiteSpace();
                var forbidden = !userAgentIsNullOrWhiteSpace &&
                                ValidateUserAgentHeader &&
                                AllowedUserAgents.Count > 0 &&
                                !AllowedUserAgents.Any(allowedUserAgent => userAgent.Equals(allowedUserAgent, StringComparison.OrdinalIgnoreCase));

                if (userAgentIsNullOrWhiteSpace || (forbidden && UseGenericResponse))
                {
                    return new HttpResponseMessage(HttpStatusCode.BadRequest)
                    {
                        Content = new StringContent(BadRequestMessage)
                    };
                }
                
                if (forbidden)
                {
                    return new HttpResponseMessage(HttpStatusCode.Forbidden)
                    {
                        Content = new StringContent(ForbiddenMessage)
                    };
                }

                return null;
            };
        }

        /// <summary>
        /// Gets or sets the function delegate that configures the response in the form of a <see cref="HttpResponseMessage"/>.
        /// </summary>
        /// <value>The function delegate that configures the response in the form of a <see cref="HttpResponseMessage"/>.</value>
        public Func<string, HttpResponseMessage> ResponseBroker { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the produced <see cref="ResponseBroker"/> should be as neutral as possible.
        /// </summary>
        /// <value><c>true</c> if the produced <see cref="ResponseBroker"/> should be as neutral as possible; otherwise, <c>false</c>.</value>
        public bool UseGenericResponse { get; set; }

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
        /// Gets or sets the message of a request missing the requirements of a User-Agent header.
        /// </summary>
        /// <value>The message of a request missing the requirements of a User-Agent header.</value>
        public string BadRequestMessage { get; set; }

        /// <summary>
        /// Gets or sets the message of a request without a valid User-Agent header.
        /// </summary>
        /// <value>The message of a request without a valid User-Agent header.</value>
        public string ForbiddenMessage { get; set; }
    }
}