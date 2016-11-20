using System;
using System.Security.Claims;
using System.Threading.Tasks;
using Cuemon.Text;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;

namespace Cuemon.Web.Security
{
    /// <summary>
    /// Provides a HTTP Basic Authentication middleware implementation for ASP.NET Core.
    /// </summary>
    public class BasicAuthenticationMiddleware
    {
        private readonly RequestDelegate _next;

        /// <summary>
        /// Initializes a new instance of the <see cref="BasicAuthenticationMiddleware"/> class.
        /// </summary>
        /// <param name="next">The delegate of the request pipeline to invoke.</param>
        /// <param name="options">The HTTP Basic Authentication middleware options.</param>
        public BasicAuthenticationMiddleware(RequestDelegate next, Func<BasicAuthenticationOptions> options)
        {
            Validator.ThrowIfNull(options, nameof(options));
            Options = options();
            _next = next;
        }

        /// <summary>
        /// Executes the <see cref="BasicAuthenticationMiddleware"/>.
        /// </summary>
        /// <param name="context">The context of the current request.</param>
        /// <returns>A task that represents the execution of this middleware.</returns>
        public async Task Invoke(HttpContext context)
        {
            if (!AuthenticationUtility.TryAuthenticate(context, Options.RequireSecureConnection, AuthorizationParser, PrincipalParser))
            {
                context.Response.StatusCode = AuthenticationUtility.HttpNotAuthorizedStatusCode;
                context.Response.Headers.Add(AuthenticationUtility.HttpWwwAuthenticateHeader, "{0} realm=\"{1}\"".FormatWith(AuthenticationSchemeName, Options.Realm));
                return;
            }

            await _next.Invoke(context);
        }

        private BasicAuthenticationOptions Options { get; }

        /// <summary>
        /// Gets the name of the authentication scheme.
        /// </summary>
        /// <value>The name of the authentication scheme.</value>
        public string AuthenticationSchemeName
        {
            get { return "Basic"; }
        }

        private bool PrincipalParser(HttpContext context, Template<string, string> credentials, out ClaimsPrincipal result)
        {
            if (Options.CredentialsValidator == null) { throw new InvalidOperationException("The CredentialsValidator delegate cannot be null."); }
            result = Options.CredentialsValidator(credentials.Arg1, credentials.Arg2);
            return Condition.IsNotNull(result);
        }

        private Template<string, string> AuthorizationParser(string authorizationHeader)
        {
            if (AuthenticationUtility.IsAuthenticationSchemeValid(authorizationHeader, AuthenticationSchemeName))
            {
                string base64Credentials = authorizationHeader.Remove(0, AuthenticationSchemeName.Length + 1);
                if (StringUtility.IsBase64(base64Credentials))
                {
                    string[] credentials = StringConverter.FromBytes(Convert.FromBase64String(base64Credentials), PreambleSequence.Remove, EncodingUtility.AsciiEncoding).Split(AuthenticationUtility.BasicAuthenticationCredentialSeparator);
                    if (credentials.Length == 2 &&
                        !string.IsNullOrEmpty(credentials[0]) &&
                        !string.IsNullOrEmpty(credentials[1]))
                    { return TupleUtility.CreateTwo(credentials[0], credentials[1]); }
                }
            }
            return null;
        }
    }

    /// <summary>
    /// This is a factory implementation of the <see cref="BasicAuthenticationMiddleware"/> class.
    /// </summary>
    public static class BasicAuthenticationBuilderExtension
    {
        /// <summary>
        /// Adds a HTTP Basic Authentication scheme to the <see cref="IApplicationBuilder"/> request execution pipeline.
        /// </summary>
        /// <param name="builder">The type that provides the mechanisms to configure an application’s request pipeline.</param>
        /// <param name="options">The HTTP Basic Authentication middleware options.</param>
        /// <returns>A reference to this instance after the operation has completed.</returns>
        public static IApplicationBuilder UseBasicAuthentication(this IApplicationBuilder builder, Func<BasicAuthenticationOptions> options)
        {
            return builder.UseMiddleware<BasicAuthenticationMiddleware>(options);
        }
    }
}