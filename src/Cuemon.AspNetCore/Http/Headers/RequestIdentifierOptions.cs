using System;
using Cuemon.Messaging;
using Cuemon.Net.Http;

namespace Cuemon.AspNetCore.Http.Headers
{
    /// <summary>
    /// Configuration options for <see cref="RequestIdentifierMiddleware"/>.
    /// </summary>
    public class RequestIdentifierOptions
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="RequestIdentifierOptions"/> class.
        /// </summary>
        /// <remarks>
        /// The following table shows the initial property values for an instance of <see cref="RequestIdentifierOptions"/>.
        /// <list type="table">
        ///     <listheader>
        ///         <term>Property</term>
        ///         <description>Initial Value</description>
        ///     </listheader>
        ///     <item>
        ///         <term><see cref="HeaderName"/></term>
        ///         <description><see cref="HttpHeaderNames.XRequestId"/></description>
        ///     </item>
        ///     <item>
        ///         <term><see cref="RequestProvider"/></term>
        ///         <description><c>DynamicRequest.Create(Guid.NewGuid().ToString("N")</c></description>
        ///     </item>
        /// </list>
        /// </remarks>
        public RequestIdentifierOptions()
        {
            HeaderName = HttpHeaderNames.XRequestId;
            RequestProvider = () => DynamicRequest.Create(Guid.NewGuid().ToString("N"));
        }

        /// <summary>
        /// Gets or sets the name of the correlation identifier HTTP header.
        /// </summary>
        /// <value>The name of the correlation identifier HTTP header.</value>
        public string HeaderName { get; set; }

        /// <summary>
        /// Gets or sets the function delegate that provides the correlation implementation.
        /// </summary>
        /// <value>The function delegate that provides the correlation implementation.</value>
        public Func<IRequest> RequestProvider { get; set; }
    }
}