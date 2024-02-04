using System;
using Cuemon.Configuration;
using Cuemon.Diagnostics;
using Cuemon.Threading;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authorization.Policy;

namespace Cuemon.Extensions.AspNetCore.Authentication
{
    /// <summary>
    /// Specifies options that is related to <see cref="AuthorizationResponseHandler"/> operations.
    /// </summary>
    public class AuthorizationResponseHandlerOptions : AsyncOptions, IExceptionDescriptorOptions, IValidatableParameterObject
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="AuthorizationResponseHandlerOptions"/> class.
        /// </summary>
        /// <remarks>
        /// The following table shows the initial property values for an instance of <see cref="AuthorizationResponseHandlerOptions"/>.
        /// <list type="table">
        ///     <listheader>
        ///         <term>Property</term>
        ///         <description>Initial Value</description>
        ///     </listheader>
        ///     <item>
        ///         <term><see cref="FallbackResponseHandler"/></term>
        ///         <description><c>new AuthorizationMiddlewareResultHandler()</c></description>
        ///     </item>
        ///     <item>
        ///         <term><see cref="SensitivityDetails"/></term>
        ///         <description><see cref="FaultSensitivityDetails.None"/></description>
        ///     </item>
        /// </list>
        /// </remarks>
        public AuthorizationResponseHandlerOptions()
        {
            FallbackResponseHandler = new AuthorizationMiddlewareResultHandler();
            SensitivityDetails = FaultSensitivityDetails.None;
        }
        
        /// <summary>
        /// Gets or sets a bitwise combination of the enumeration values that specify which sensitive details to include in the serialized result.
        /// </summary>
        /// <value>The enumeration values that specify which sensitive details to include in the serialized result.</value>
        public FaultSensitivityDetails SensitivityDetails { get; set; }

        /// <summary>
        /// Gets or sets the mandatory <see cref="IAuthorizationMiddlewareResultHandler"/> implementation of a fallback response handler.
        /// </summary>
        /// <value>The mandatory <see cref="IAuthorizationMiddlewareResultHandler"/> implementation of a fallback response handler.</value>
        /// <remarks>If everything else fails; this is safeguard to keep the AuthN/AuthZ flow intact.</remarks>
        public IAuthorizationMiddlewareResultHandler FallbackResponseHandler { get; set; }

        /// <summary>
        /// Determines whether the public read-write properties of this instance are in a valid state.
        /// </summary>
        /// <exception cref="InvalidOperationException">
        /// <see cref="FallbackResponseHandler"/> cannot be null.
        /// </exception>
        /// <remarks>This method is expected to throw exceptions when one or more conditions fails to be in a valid state.</remarks>
        public void ValidateOptions()
        {
            Validator.ThrowIfInvalidState(FallbackResponseHandler == null);
        }
    }
}
