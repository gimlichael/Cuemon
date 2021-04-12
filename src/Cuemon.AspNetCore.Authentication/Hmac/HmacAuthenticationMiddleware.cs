using System;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Cuemon.IO;
using Cuemon.Security.Cryptography;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using Microsoft.Net.Http.Headers;

namespace Cuemon.AspNetCore.Authentication.Hmac
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
        public HmacAuthenticationMiddleware(RequestDelegate next, Action<HmacAuthenticationOptions> setup) : base(next, setup)
        {
        }

        /// <summary>
        /// Executes the <see cref="HmacAuthenticationMiddleware" />.
        /// </summary>
        /// <param name="context">The context of the current request.</param>
        /// <returns>A task that represents the execution of this middleware.</returns>
        public override async Task InvokeAsync(HttpContext context)
        {
            if (!Authenticator.TryAuthenticate(context, Options.RequireSecureConnection, AuthorizationHeaderParser, TryAuthenticate))
            {
                await Decorator.Enclose(context).InvokeAuthenticationAsync(Options, async (message, response) =>
                {
                    context.Response.OnStarting(() =>
                    {
                        context.Response.Headers.Add(HeaderNames.WWWAuthenticate, Options.AuthenticationScheme);
                        return Task.CompletedTask;
                    });
                    response.StatusCode = (int)message.StatusCode;
                    await Decorator.Enclose(response.Body).WriteAllAsync(await message.Content.ReadAsByteArrayAsync().ConfigureAwait(false)).ConfigureAwait(false);
                }).ConfigureAwait(false);
            }
            await Next.Invoke(context).ConfigureAwait(false);
        }

        private bool TryAuthenticate(HttpContext context, HmacAuthorizationHeader header, out ClaimsPrincipal result)
        {
            if (Options.Authenticator == null) { throw new InvalidOperationException(FormattableString.Invariant($"The {nameof(Options.Authenticator)} cannot be null.")); }

            if (header == null)
            {
                result = null;
                return false;
            }

            var requestBodyMd5 = context.Request.Headers[HeaderNames.ContentMD5].FirstOrDefault()?.ToLowerInvariant();
            if (!string.IsNullOrWhiteSpace(requestBodyMd5) && !UnkeyedHashFactory.CreateCrypto(UnkeyedCryptoAlgorithm.Md5).ComputeHash(context.Request.Body).ToHexadecimalString().Equals(requestBodyMd5, StringComparison.Ordinal))
            {
                result = null;
                return false;
            }
            
            var clientId = header.ClientId;
            result = Options.Authenticator(clientId, out var clientSecret);
            if (clientSecret == null)
            {
                result = null;
                return false;
            }
            var signature = header.Signature;

            var hb = new HmacAuthorizationHeaderBuilder(Options.AuthenticationScheme)
                .AddCredentialScope(header.CredentialScope)
                .AddClientId(clientId)
                .AddClientSecret(clientSecret)
                .AddSignedHeaders(header.SignedHeaders)
                .AddFromRequest(context.Request);

            var computedSignature = hb.ComputeSignature();
            return computedSignature != null && signature.Equals(computedSignature, StringComparison.Ordinal) && Condition.IsNotNull(result);
        }

        private HmacAuthorizationHeader AuthorizationHeaderParser(HttpContext context, string authorizationHeader)
        {
            return HmacAuthorizationHeader.Create(Options.AuthenticationScheme, authorizationHeader);
        }
    }
}