using System;
using System.Security;
using System.Security.Claims;
using System.Threading.Tasks;
using Cuemon.Extensions.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Net.Http.Headers;

namespace Cuemon.AspNetCore.Authentication
{
    /// <summary>
    /// Provides a set of generic ways to work with HTTP based authentication.
    /// </summary>
    public static class AuthenticationUtility
    {
        /// <summary>
        /// The value of the header credential separator of a HTTP Basic access authentication.
        /// </summary>
        public const char BasicAuthenticationCredentialSeparator = ':';

        /// <summary>
        /// The value of the header credential separator of a HTTP Digest access authentication.
        /// </summary>
        public const char DigestAuthenticationCredentialSeparator = ',';

        /// <summary>
        /// The value of the header credential separator of a HTTP JSON Web Token authentication.
        /// </summary>
        public const char JwtAuthenticationCredentialSeparator = '.';

        /// <summary>
        /// The value of the status description associated with <see cref="HttpNotAuthorizedStatusCode"/>.
        /// </summary>
        public const string HttpNotAuthorizedStatus = "401 Unauthorized";

        /// <summary>
        /// Equivalent to HTTP status 401. Unauthorized indicates that the requested resource requires authentication.
        /// </summary>
        public const int HttpNotAuthorizedStatusCode = 401;

        /// <summary>
        /// Provides a generic way to make authentication requests using the specified <paramref name="context"/>.
        /// </summary>
        /// <typeparam name="T">The type of the credentials returned from <paramref name="authorizationParser"/> and passed to <paramref name="principalParser"/>.</typeparam>
        /// <param name="context">The context of the ASP.NET application.</param>
        /// <param name="requireSecureConnection">When <c>true</c>, the HTTP connection is required to use secure sockets (that is, HTTPS); when <c>false</c> no requirement is enforced.</param>
        /// <param name="authorizationParser">The function delegate that will parse the authorization header of a web request and return the credentials of <typeparamref name="T"/>.</param>
        /// <param name="principalParser">The function delegate that will parse the credentials of <typeparamref name="T"/> returned from <paramref name="authorizationParser"/> and if successful returns a <see cref="ClaimsPrincipal"/> object.</param>
        /// <returns><c>true</c> if the specified parameters triggers a successful authentication; otherwise, <c>false</c>.</returns>
        public static bool TryAuthenticate<T>(HttpContext context, bool requireSecureConnection, Func<HttpContext, string, T> authorizationParser, TesterFunc<HttpContext, T, ClaimsPrincipal, bool> principalParser)
        {
            try
            {
                Authenticate(context, requireSecureConnection, authorizationParser, principalParser);
                return true;
            }
            catch (SecurityException)
            {
                return false;
            }
        }

        /// <summary>
        /// Provides a generic way to make authentication requests using the specified <paramref name="context"/>.
        /// </summary>
        /// <typeparam name="T">The type of the credentials returned from <paramref name="authorizationParser"/> and passed to <paramref name="principalParser"/>.</typeparam>
        /// <param name="context">The context of the ASP.NET application.</param>
        /// <param name="requireSecureConnection">When <c>true</c>, the HTTP connection is required to use secure sockets (that is, HTTPS); when <c>false</c> no requirement is enforced.</param>
        /// <param name="authorizationParser">The function delegate that will parse the authorization header of a web request and return the credentials of <typeparamref name="T"/>.</param>
        /// <param name="principalParser">The function delegate that will parse the credentials of <typeparamref name="T"/> returned from <paramref name="authorizationParser"/> and if successful returns a <see cref="ClaimsPrincipal"/> object.</param>
        /// <exception cref="SecurityException">
        /// Authorized failed for the request.
        /// </exception>
        public static void Authenticate<T>(HttpContext context, bool requireSecureConnection, Func<HttpContext, string, T> authorizationParser, TesterFunc<HttpContext, T, ClaimsPrincipal, bool> principalParser)
        {
            Validator.ThrowIfNull(context, nameof(context));
            if (requireSecureConnection && !context.Request.IsHttps) { throw new SecurityException("An SSL connection is required for the request."); }

            if (TryGetPrincipal(context, authorizationParser, principalParser, out var principal))
            {
                context.User = principal;
                return;
            }
            throw new SecurityException("Authentication failed for the request.");
        }

        private static bool TryGetPrincipal<T>(HttpContext context, Func<HttpContext, string, T> authorizationParser, TesterFunc<HttpContext, T, ClaimsPrincipal, bool> principalParser, out ClaimsPrincipal principal)
        {
            principal = null;
            string authorizationHeader = context.Request.Headers[HeaderNames.Authorization];
            if (string.IsNullOrEmpty(authorizationHeader)) { return false; }
            var credentials = authorizationParser(context, authorizationHeader);
            if (credentials != null && principalParser(context, credentials, out principal)) { return true; }
            return false;
        }

        internal static bool IsAuthenticationSchemeValid(string authorizationHeader, string authenticationSchemeName)
        {
            return (!string.IsNullOrEmpty(authorizationHeader) && authorizationHeader.StartsWith(authenticationSchemeName, StringComparison.Ordinal));
        }

        internal static async Task WriteHttpNotAuthorizedBody(this HttpContext context, Func<byte[]> httpNotAuthorizedBody)
        {
            var bodyContent = httpNotAuthorizedBody?.Invoke();
            if (bodyContent != null)
            {
                await context.Response.Body.WriteAsync(bodyContent, 0, bodyContent.Length).ContinueWithSuppressedContext();
            }
        }
    }
}