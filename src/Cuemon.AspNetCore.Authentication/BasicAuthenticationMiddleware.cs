using System;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Cuemon.IO;
using Cuemon.Text;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using Microsoft.Net.Http.Headers;

namespace Cuemon.AspNetCore.Authentication
{
    /// <summary>
    /// Provides a HTTP Basic Authentication middleware implementation for ASP.NET Core.
    /// </summary>
    public class BasicAuthenticationMiddleware : ConfigurableMiddleware<BasicAuthenticationOptions>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="BasicAuthenticationMiddleware"/> class.
        /// </summary>
        /// <param name="next">The delegate of the request pipeline to invoke.</param>
        /// <param name="setup">The <see cref="BasicAuthenticationOptions" /> which need to be configured.</param>
        public BasicAuthenticationMiddleware(RequestDelegate next, IOptions<BasicAuthenticationOptions> setup) : base(next, setup)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="BasicAuthenticationMiddleware"/> class.
        /// </summary>
        /// <param name="next">The delegate of the request pipeline to invoke.</param>
        /// <param name="setup">The middleware <see cref="BasicAuthenticationOptions"/> which need to be configured.</param>
        public BasicAuthenticationMiddleware(RequestDelegate next, Action<BasicAuthenticationOptions> setup)  : base(next, setup)
        {
        }

        /// <summary>
        /// Executes the <see cref="BasicAuthenticationMiddleware"/>.
        /// </summary>
        /// <param name="context">The context of the current request.</param>
        /// <returns>A task that represents the execution of this middleware.</returns>
        public override async Task InvokeAsync(HttpContext context)
        {
            if (!AuthenticationUtility.TryAuthenticate(context, Options.RequireSecureConnection, AuthorizationHeaderParser, TryAuthenticate))
            {
                await Decorator.Enclose(context).InvokeAuthenticationAsync(Options, async (message, response) =>
                {
                    context.Response.OnStarting(() =>
                    {
                        context.Response.Headers.Add(HeaderNames.WWWAuthenticate, FormattableString.Invariant($"{AuthenticationScheme} realm=\"{Options.Realm}\""));
                        return Task.CompletedTask;
                    });
                    response.StatusCode = (int)message.StatusCode;
                    await Decorator.Enclose(response.Body).WriteAsync(await message.Content.ReadAsByteArrayAsync().ConfigureAwait(false)).ConfigureAwait(false);
                }).ConfigureAwait(false);
            }
            await Next(context).ConfigureAwait(false);
        }

        /// <summary>
        /// Gets the name of the authentication scheme.
        /// </summary>
        /// <value>The name of the authentication scheme.</value>
        public string AuthenticationScheme => "Basic";

        private bool TryAuthenticate(HttpContext context, Template<string, string> credentials, out ClaimsPrincipal result)
        {
            if (Options.Authenticator == null) { throw new InvalidOperationException(FormattableString.Invariant($"The {nameof(Options.Authenticator)} cannot be null.")); }
            result = Options.Authenticator(credentials.Arg1, credentials.Arg2);
            return Condition.IsNotNull(result);
        }

        private Template<string, string> AuthorizationHeaderParser(HttpContext context, string authorizationHeader)
        {
            if (AuthenticationUtility.IsAuthenticationSchemeValid(authorizationHeader, AuthenticationScheme))
            {
                var base64Credentials = authorizationHeader.Remove(0, AuthenticationScheme.Length + 1);
                if (Condition.IsBase64(base64Credentials))
                {
                    var credentials = Convertible.ToString(Convert.FromBase64String(base64Credentials), options =>
                    {
                        options.Encoding = Encoding.ASCII;
                        options.Preamble = PreambleSequence.Remove;
                    }).Split(AuthenticationUtility.BasicAuthenticationCredentialSeparator);
                    if (credentials.Length == 2 &&
                        !string.IsNullOrEmpty(credentials[0]) &&
                        !string.IsNullOrEmpty(credentials[1]))
                    { return Template.CreateTwo(credentials[0], credentials[1]); }
                }
            }
            return null;
        }
    }
}