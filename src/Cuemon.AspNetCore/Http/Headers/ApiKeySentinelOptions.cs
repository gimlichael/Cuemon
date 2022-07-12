using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using Cuemon.Net.Http;

namespace Cuemon.AspNetCore.Http.Headers
{
    /// <summary>
    /// Configuration options for <see cref="ApiKeySentinelMiddleware"/>.
    /// </summary>
    public class ApiKeySentinelOptions
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ApiKeySentinelOptions"/> class.
        /// </summary>
        /// <remarks>
        /// The following table shows the initial property values for an instance of <see cref="UserAgentSentinelOptions"/>.
        /// <list type="table">
        ///     <listheader>
        ///         <term>Property</term>
        ///         <description>Initial Value</description>
        ///     </listheader>
        ///     <item>
        ///         <term><see cref="HeaderName"/></term>
        ///         <description><see cref="HttpHeaderNames.XApiKey"/></description>
        ///     </item>
        ///     <item>
        ///         <term><see cref="AllowedKeys"/></term>
        ///         <description><c>new List{string}();</c></description>
        ///     </item>
        ///     <item>
        ///         <term><see cref="BadRequestMessage"/></term>
        ///         <description>The requirements of the request was not met.</description>
        ///     </item>
        ///     <item>
        ///         <term><see cref="ForbiddenMessage"/></term>
        ///         <description>The API key specified was rejected.</description>
        ///     </item>
        ///     <item>
        ///         <term><see cref="ResponseHandler"/></term>
        ///         <description>A <see cref="HttpResponseMessage"/> initialized to either a HTTP status code 400 or 403 and a body of either <see cref="BadRequestMessage"/> or <see cref="ForbiddenMessage"/>.</description>
        ///     </item>
        ///     <item>
        ///         <term><see cref="UseGenericResponse"/></term>
        ///         <description><c>false</c></description>
        ///     </item>
        /// </list>
        /// </remarks>
        public ApiKeySentinelOptions()
        {
            HeaderName = HttpHeaderNames.XApiKey;
            BadRequestMessage = "The requirements of the request was not met.";
            ForbiddenMessage = "The API key specified was rejected.";
            AllowedKeys = new List<string>();
            ResponseHandler = apiKey =>
            {
                var apiKeyIsNullOrWhiteSpace = string.IsNullOrWhiteSpace(apiKey);
                var forbidden = !apiKeyIsNullOrWhiteSpace &&
                                AllowedKeys.Count > 0 &&
                                !AllowedKeys.Any(allowedApiKey => apiKey.Equals(allowedApiKey, StringComparison.OrdinalIgnoreCase));

                if (apiKeyIsNullOrWhiteSpace || forbidden && UseGenericResponse)
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
        /// Gets or sets the name of the API key HTTP header.
        /// </summary>
        /// <value>The name of the API key HTTP header.</value>
        public string HeaderName { get; set; }

        /// <summary>
        /// Gets or sets the function delegate that configures the response in the form of a <see cref="HttpResponseMessage"/>.
        /// </summary>
        /// <value>The function delegate that configures the response in the form of a <see cref="HttpResponseMessage"/>.</value>
        public Func<string, HttpResponseMessage> ResponseHandler { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the produced <see cref="ResponseHandler"/> should be as neutral as possible.
        /// </summary>
        /// <value><c>true</c> if the produced <see cref="ResponseHandler"/> should be as neutral as possible; otherwise, <c>false</c>.</value>
        public bool UseGenericResponse { get; set; }

        /// <summary>
        /// Gets a list of whitelisted API keys.
        /// </summary>
        /// <value>A list of whitelisted API keys.</value>
        public IList<string> AllowedKeys { get; }

        /// <summary>
        /// Gets or sets the generic message of a request without a valid <see cref="HeaderName"/>.
        /// </summary>
        /// <value>The generic message of a request without a valid <see cref="HeaderName"/>.</value>
        public string BadRequestMessage { get; set; }

        /// <summary>
        /// Gets or sets the message of a request without a valid <see cref="HeaderName"/>.
        /// </summary>
        /// <value>The message of a request without a valid <see cref="HeaderName"/>.</value>
        public string ForbiddenMessage { get; set; }
    }
}
