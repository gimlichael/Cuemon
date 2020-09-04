using System;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Cuemon.AspNetCore.Builder;
using Cuemon.Security.Cryptography;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using Microsoft.Net.Http.Headers;

namespace Cuemon.AspNetCore.Authentication
{
    /// <summary>
    /// Provides a HTTP HMAC Authentication middleware implementation for ASP.NET Core.
    /// </summary>
    public class HmacAuthenticationMiddleware : ConfigurableMiddleware<HmacAuthenticationOptions>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="HmacAuthenticationMiddleware"/> class.
        /// </summary>
        /// <param name="next">The delegate of the request pipeline to invoke.</param>
        /// <param name="setup">The <see cref="HmacAuthenticationOptions" /> which need to be configured.</param>
        public HmacAuthenticationMiddleware(RequestDelegate next, IOptions<HmacAuthenticationOptions> setup) : base(next, setup)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="HmacAuthenticationMiddleware"/> class.
        /// </summary>
        /// <param name="next">The delegate of the request pipeline to invoke.</param>
        /// <param name="setup">The middleware <see cref="HmacAuthenticationOptions"/> which need to be configured.</param>
        public HmacAuthenticationMiddleware(RequestDelegate next, Action<HmacAuthenticationOptions> setup)  : base(next, setup)
        {
        }

        /// <summary>
        /// Executes the <see cref="HmacAuthenticationMiddleware" />.
        /// </summary>
        /// <param name="context">The context of the current request.</param>
        /// <returns>A task that represents the execution of this middleware.</returns>
        public override async Task InvokeAsync(HttpContext context)
        {
            if (!AuthenticationUtility.TryAuthenticate(context, Options.RequireSecureConnection, AuthorizationHeaderParser, TryAuthenticate))
            {
                context.Response.StatusCode = AuthenticationUtility.HttpNotAuthorizedStatusCode;
                context.Response.Headers.Add(HeaderNames.WWWAuthenticate, Options.AuthenticationScheme);
                await context.WriteHttpNotAuthorizedBody(Options.HttpNotAuthorizedBody).ConfigureAwait(false);
                return;
            }
            await Next.Invoke(context).ConfigureAwait(false);
        }

        private bool TryAuthenticate(HttpContext context, Template<string, string> credentials, out ClaimsPrincipal result)
        {
            if (Options.Authenticator == null) { throw new InvalidOperationException(FormattableString.Invariant($"The {nameof(Options.Authenticator)} cannot be null.")); }
            var requestBodyMd5 = context.Request.Headers[HeaderNames.ContentMD5].FirstOrDefault()?.ToLowerInvariant();
            if (!string.IsNullOrWhiteSpace(requestBodyMd5) && !UnkeyedHashFactory.CreateCrypto(UnkeyedCryptoAlgorithm.Md5).ComputeHash(context.Request.Body).ToHexadecimalString().Equals(requestBodyMd5, StringComparison.Ordinal))
            {
                result = null;
                return false;
            }
            var publicKey = credentials.Arg1;
            var signature = credentials.Arg2;
            var stringToSign = Options.MessageDescriptor(context);
            var privateKey = new byte[0];
            result = Options?.Authenticator(publicKey, out privateKey);
            if (privateKey == null)
            {
                result = null;
                return false;
            }
            var computedSignature = Options?.HmacSigner(new HmacAuthenticationParameters(Options.Algorithm, privateKey, stringToSign));
            return computedSignature != null && signature.Equals(Convert.ToBase64String(computedSignature), StringComparison.Ordinal) && Condition.IsNotNull(result);
        }

        private Template<string, string> AuthorizationHeaderParser(HttpContext context, string authorizationHeader)
        {
            if (AuthenticationUtility.IsAuthenticationSchemeValid(authorizationHeader, Options.AuthenticationScheme) && authorizationHeader.Length > Options.AuthenticationScheme.Length)
            {
                var credentials = authorizationHeader.Remove(0, Options.AuthenticationScheme.Length + 1).Split(':');
                if (credentials.Length == 2)
                {
                    var publicKey = credentials[0];
                    var signature = credentials[1];
                    if (!string.IsNullOrWhiteSpace(publicKey) && !string.IsNullOrWhiteSpace(signature)) { return Template.CreateTwo(publicKey, signature); }
                }
            }
            return null;
        }
    }

    /// <summary>
    /// This is a factory implementation of the <see cref="HmacAuthenticationMiddleware"/> class.
    /// </summary>
    public static class HmacAuthenticationBuilderExtension
    {
        /// <summary>
        /// Adds a HTTP HMAC Authentication scheme to the <see cref="IApplicationBuilder"/> request execution pipeline.
        /// </summary>
        /// <param name="builder">The type that provides the mechanisms to configure an application’s request pipeline.</param>
        /// <param name="setup">The HTTP <see cref="HmacAuthenticationOptions"/> middleware which need to be configured.</param>
        /// <returns>A reference to this instance after the operation has completed.</returns>
        public static IApplicationBuilder UseHmacAuthentication(this IApplicationBuilder builder, Action<HmacAuthenticationOptions> setup = null)
        {
            return ApplicationBuilderFactory.UseMiddlewareConfigurable<HmacAuthenticationMiddleware, HmacAuthenticationOptions>(builder, setup);
        }
    }
}