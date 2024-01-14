using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Security.Claims;
using System.Threading.Tasks;
using Cuemon.Collections.Generic;
using Cuemon.IO;
using Cuemon.Security.Cryptography;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using Microsoft.Net.Http.Headers;

namespace Cuemon.AspNetCore.Authentication.Digest
{
    /// <summary>
    /// Provides a HTTP Digest Access Authentication middleware implementation for ASP.NET Core.
    /// </summary>
    public class DigestAuthenticationMiddleware : ConfigurableMiddleware<INonceTracker, DigestAuthenticationOptions>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DigestAuthenticationMiddleware"/> class.
        /// </summary>
        /// <param name="next">The delegate of the request pipeline to invoke.</param>
        /// <param name="setup">The <see cref="DigestAuthenticationOptions" /> which need to be configured.</param>
        public DigestAuthenticationMiddleware(RequestDelegate next, IOptions<DigestAuthenticationOptions> setup) : base(next, setup)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="DigestAuthenticationMiddleware"/> class.
        /// </summary>
        /// <param name="next">The delegate of the request pipeline to invoke.</param>
        /// <param name="setup">The middleware <see cref="DigestAuthenticationOptions"/> which need to be configured.</param>
        public DigestAuthenticationMiddleware(RequestDelegate next, Action<DigestAuthenticationOptions> setup) : base(next, setup)
        {
        }

        /// <summary>
        /// Executes the <see cref="DigestAuthenticationMiddleware"/>.
        /// </summary>
        /// <param name="context">The context of the current request.</param>
        /// <param name="di">The dependency injected implementation of an <see cref="INonceTracker"/>.</param>
        /// <returns>A task that represents the execution of this middleware.</returns>
        /// <remarks><c>qop</c> is included and supported to be compliant with RFC 2617 (hence, this implementation cannot revert to reduced legacy RFC 2069 mode).</remarks>
        public override async Task InvokeAsync(HttpContext context, INonceTracker di)
        {
	        if (Options.Authenticator == null) { throw new InvalidOperationException(string.Create(CultureInfo.InvariantCulture, $"The {nameof(Options.Authenticator)} delegate cannot be null.")); }
	        
	        context.Items.TryAdd(nameof(DigestAuthenticationOptions), Options);
	        context.Items.TryAdd(nameof(INonceTracker), di);

	        if (!Authenticator.TryAuthenticate(context, Options.RequireSecureConnection, AuthorizationHeaderParser, TryAuthenticate, out var principal))
            {
                await Decorator.Enclose(context).InvokeUnauthorizedExceptionAsync(Options, dc =>
                {
                    string etag = dc.Response.Headers[HeaderNames.ETag];
                    if (string.IsNullOrEmpty(etag)) { etag = "no-entity-tag"; }
                    var opaqueGenerator = Options.OpaqueGenerator;
                    var nonceSecret = Options.NonceSecret;
                    var nonceGenerator = Options.NonceGenerator;
                    var staleNonce = dc.Items[DigestFields.Stale] as string ?? "false";
                    Decorator.Enclose(dc.Response.Headers).TryAdd(HeaderNames.WWWAuthenticate, string.Create(CultureInfo.InvariantCulture, $"{DigestAuthorizationHeader.Scheme} realm=\"{Options.Realm}\", qop=\"auth, auth-int\", nonce=\"{nonceGenerator(DateTime.UtcNow, etag, nonceSecret())}\", opaque=\"{opaqueGenerator()}\", stale=\"{staleNonce}\", algorithm=\"{ParseAlgorithm(Options.Algorithm)}\""));
                }).ConfigureAwait(false);
            }

            context.User = principal;
			await Next.Invoke(context).ConfigureAwait(false);
        }

        internal static bool TryAuthenticate(HttpContext context, DigestAuthorizationHeader header, out ClaimsPrincipal result)
        {
	        var options = context.Items[nameof(DigestAuthenticationOptions)] as DigestAuthenticationOptions;
            var nonceTracker = context.Items[nameof(INonceTracker)] as INonceTracker;
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

            result = null;
            var nonceExpiredParser = options.NonceExpiredParser;
            var staleNonce = nonceExpiredParser(header.Nonce, TimeSpan.FromSeconds(30));
            if (staleNonce)
            {
                context.Items.Add(DigestFields.Stale, "true");
                return false;
            }

            if (nonceTracker != null)
            {
                var nc = Convert.ToInt32(header.NC, 16);
                if (nonceTracker.TryGetEntry(header.Nonce, out var previousNonce))
                {
                    if (previousNonce.Count == nc)
                    {
                        context.Items.Add(DigestFields.Stale, "true");
                        return false;
                    }
                }
                else
                {
                    nonceTracker.TryAddEntry(header.Nonce, nc);
                }
            }

            result = options.Authenticator(header.UserName, out var password);

            context.Request.EnableBuffering();

            using var body = new MemoryStream();
            context.Request.Body.CopyToAsync(body).GetAwaiter().GetResult();
            var db = new DigestAuthorizationHeaderBuilder().AddFromDigestAuthorizationHeader(header);
            var ha1 = options.UseServerSideHa1Storage ? password : db.ComputeHash1(password);
            var ha2 = db.ComputeHash2(context.Request.Method, Decorator.Enclose(body).ToEncodedString());
            var serverResponse = db.ComputeResponse(ha1, ha2);
            return serverResponse != null && serverResponse.Equals(header.Response, StringComparison.Ordinal) && Condition.IsNotNull(result);
        }

        internal static DigestAuthorizationHeader AuthorizationHeaderParser(HttpContext context, string authorizationHeader)
        {
            return DigestAuthorizationHeader.Create(authorizationHeader);
        }

        internal static string ParseAlgorithm(UnkeyedCryptoAlgorithm algorithm)
        {
            switch (algorithm)
            {
                case UnkeyedCryptoAlgorithm.Sha256:
                    return "SHA-256";
                case UnkeyedCryptoAlgorithm.Sha512:
                    return "SHA-512-256";
                default:
                    return "MD5";
            }
        }
    }
}
