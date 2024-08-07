﻿using System;
using Cuemon.Configuration;
using Cuemon.Messaging;
using Cuemon.Net.Http;

namespace Cuemon.AspNetCore.Http.Headers
{
    /// <summary>
    /// Configuration options for <see cref="RequestIdentifierMiddleware"/>.
    /// </summary>
    public class RequestIdentifierOptions : IValidatableParameterObject
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
        ///         <term><see cref="Token"/></term>
        ///         <description><c>DynamicRequest.Create(Guid.NewGuid().ToString("N")</c></description>
        ///     </item>
        /// </list>
        /// </remarks>
        public RequestIdentifierOptions()
        {
            HeaderName = HttpHeaderNames.XRequestId;
            Token = new RequestToken();
        }

        /// <summary>
        /// Gets or sets the name of the correlation identifier HTTP header.
        /// </summary>
        /// <value>The name of the correlation identifier HTTP header.</value>
        public string HeaderName { get; set; }

        /// <summary>
        /// Gets or sets the <see cref="IRequestToken"/> that provides a Request ID implementation.
        /// </summary>
        /// <value>The <see cref="IRequestToken"/> that provides a Request ID implementation.</value>
        public IRequestToken Token { get; set; }

        /// <summary>
        /// Determines whether the public read-write properties of this instance are in a valid state.
        /// </summary>
        /// <exception cref="InvalidOperationException">
        /// <see cref="HeaderName"/> cannot be null, empty or consist only of white-space characters - or -
        /// <see cref="Token"/> cannot be null.
        /// </exception>
        /// <remarks>This method is expected to throw exceptions when one or more conditions fails to be in a valid state.</remarks>
        public void ValidateOptions()
        {
            Validator.ThrowIfInvalidState(Condition.IsNull(HeaderName) || Condition.IsEmpty(HeaderName) || Condition.IsWhiteSpace(HeaderName));
            Validator.ThrowIfInvalidState(Token == null);
        }
    }
}
