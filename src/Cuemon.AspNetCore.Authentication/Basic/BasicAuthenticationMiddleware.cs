using System;
using System.Collections.Generic;
using System.Globalization;
using System.Security.Claims;
using System.Threading.Tasks;
using Cuemon.Collections.Generic;
using Cuemon.IO;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using Microsoft.Net.Http.Headers;

namespace Cuemon.AspNetCore.Authentication.Basic
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
            if (!Authenticator.TryAuthenticate(context, Options.RequireSecureConnection, AuthorizationHeaderParser, TryAuthenticate))
            {
                await Decorator.Enclose(context).InvokeAuthenticationAsync(Options, dc => Decorator.Enclose(dc.Response.Headers).TryAdd(HeaderNames.WWWAuthenticate, string.Create(CultureInfo.InvariantCulture, $"{BasicAuthorizationHeader.Scheme} realm=\"{Options.Realm}\""))).ConfigureAwait(false);
            }
            await Next(context).ConfigureAwait(false);
        }

        private bool TryAuthenticate(HttpContext context, BasicAuthorizationHeader header, out ClaimsPrincipal result)
        {
            if (Options.Authenticator == null) { throw new InvalidOperationException(string.Create(CultureInfo.InvariantCulture, $"The {nameof(Options.Authenticator)} cannot be null.")); }
            result = Options.Authenticator(header.UserName, header.Password);
            return Condition.IsNotNull(result);
        }

        private BasicAuthorizationHeader AuthorizationHeaderParser(HttpContext context, string authorizationHeader)
        {
            return BasicAuthorizationHeader.Create(authorizationHeader);
        }
    }
}