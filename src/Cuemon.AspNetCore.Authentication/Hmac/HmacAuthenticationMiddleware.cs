using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Cuemon.Collections.Generic;
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
	        if (Options.Authenticator == null) { throw new InvalidOperationException(string.Create(CultureInfo.InvariantCulture, $"The {nameof(Options.Authenticator)} cannot be null.")); }

	        context.Items.TryAdd(nameof(HmacAuthenticationOptions), Options);

            if (!Authenticator.TryAuthenticate(context, Options.RequireSecureConnection, AuthorizationHeaderParser, TryAuthenticate, out var principal))
            {
                await Decorator.Enclose(context).InvokeUnauthorizedExceptionAsync(Options, dc => Decorator.Enclose(dc.Response.Headers).TryAdd(HeaderNames.WWWAuthenticate, Options.AuthenticationScheme)).ConfigureAwait(false);
            }

            context.User = principal;
            await Next.Invoke(context).ConfigureAwait(false);
        }

        internal static bool TryAuthenticate(HttpContext context, HmacAuthorizationHeader header, out ClaimsPrincipal result)
        {
	        var options = context.Items[nameof(HmacAuthenticationOptions)] as HmacAuthenticationOptions;
	        if (options?.Authenticator == null)
	        {
		        result = null;
		        return false;
	        }

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
            result = options.Authenticator(clientId, out var clientSecret);
            if (clientSecret == null)
            {
                result = null;
                return false;
            }
            var signature = header.Signature;

            var hb = new HmacAuthorizationHeaderBuilder(options.AuthenticationScheme)
                .AddCredentialScope(header.CredentialScope)
                .AddClientId(clientId)
                .AddClientSecret(clientSecret)
                .AddSignedHeaders(header.SignedHeaders)
                .AddFromRequest(context.Request);

            Debug.WriteLine(hb.ToString());

            var computedSignature = hb.ComputeSignature();
            return computedSignature != null && signature.Equals(computedSignature, StringComparison.Ordinal) && Condition.IsNotNull(result);
        }

        internal static HmacAuthorizationHeader AuthorizationHeaderParser(HttpContext context, string authorizationHeader)
        {
	        var options = context.Items[nameof(HmacAuthenticationOptions)] as HmacAuthenticationOptions;
            return HmacAuthorizationHeader.Create(options!.AuthenticationScheme, authorizationHeader);
        }
    }
}
