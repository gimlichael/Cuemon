using System;
using System.Security.Claims;

namespace Cuemon.AspNetCore.Authentication
{
    /// <summary>
    /// Configuration options for <see cref="BasicAuthenticationMiddleware"/>. This class cannot be inherited.
    /// </summary>
    /// <seealso cref="AuthenticationOptions" />
    public sealed class BasicAuthenticationOptions : AuthenticationOptions
    {
        /// <summary>
        /// Gets the function delegate callback for credentials validation.
        /// </summary>
        /// <value>The function delegate callback for credentials validation.</value>
        public Func<string, string, ClaimsPrincipal> CredentialsValidator { get; set; }
    }
}