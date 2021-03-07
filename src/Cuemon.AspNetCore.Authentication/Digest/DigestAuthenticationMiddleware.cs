using System;
using System.Security.Claims;
using System.Threading.Tasks;
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
        private INonceTracker _nonceTracker;

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
        public override async Task InvokeAsync(HttpContext context, INonceTracker di)
        {
            _nonceTracker = di;
            if (!Authenticator.TryAuthenticate(context, Options.RequireSecureConnection, AuthorizationHeaderParser, TryAuthenticate))
            {
                await Decorator.Enclose(context).InvokeAuthenticationAsync(Options, async (message, response) =>
                {
                    context.Response.OnStarting(() =>
                    {
                        string etag = context.Response.Headers[HeaderNames.ETag];
                        if (string.IsNullOrEmpty(etag)) { etag = "no-entity-tag"; }
                        var opaqueGenerator = Options.OpaqueGenerator;
                        var nonceSecret = Options.NonceSecret;
                        var nonceGenerator = Options.NonceGenerator;
                        var staleNonce = context.Items[DigestFields.Stale] as string ?? "false";
                        context.Response.Headers.Add(HeaderNames.WWWAuthenticate, FormattableString.Invariant($"{DigestAuthorizationHeader.Scheme} realm=\"{Options.Realm}\", qop=\"auth, auth-int\", nonce=\"{nonceGenerator(DateTime.UtcNow, etag, nonceSecret())}\", opaque=\"{opaqueGenerator()}\", stale=\"{staleNonce}\", algorithm=\"{ParseAlgorithm(Options.Algorithm)}\""));
                        return Task.CompletedTask;
                    });
                    response.StatusCode = (int)message.StatusCode;
                    await Decorator.Enclose(response.Body).WriteAsync(await message.Content.ReadAsByteArrayAsync().ConfigureAwait(false)).ConfigureAwait(false);
                }).ConfigureAwait(false);
            }
            await Next.Invoke(context).ConfigureAwait(false);
        }

        private bool TryAuthenticate(HttpContext context, DigestAuthorizationHeader header, out ClaimsPrincipal result)
        {
            if (Options.Authenticator == null) { throw new InvalidOperationException(FormattableString.Invariant($"The {nameof(Options.Authenticator)} delegate cannot be null.")); }

            if (header == null)
            {
                result = null;
                return false;
            }

            result = null;
            var nonceExpiredParser = Options.NonceExpiredParser;
            var staleNonce = nonceExpiredParser(header.Nonce, TimeSpan.FromSeconds(30));
            if (staleNonce)
            {
                context.Items.Add(DigestFields.Stale, "true");
                return false;
            }

            if (_nonceTracker != null)
            {
                var nc = Convert.ToInt32(header.NC, 16);
                if (_nonceTracker.TryGetEntry(header.Nonce, out var previousNonce))
                {
                    if (previousNonce.Count == nc)
                    {
                        context.Items.Add(DigestFields.Stale, "true");
                        return false;
                    }
                }
                else
                {
                    _nonceTracker.TryAddEntry(header.Nonce, nc);
                }
            }

            result = Options.Authenticator(header.UserName, out var password);

            var db = new DigestAuthorizationHeaderBuilder().AddFromDigestAuthorizationHeader(header);
            var ha1 = db.ComputeHash1(password);
            var ha2 = db.ComputeHash2(context.Request.Method, Decorator.Enclose(context.Request.Body).ToEncodedString(o => o.LeaveOpen = true));
            var serverResponse = db.ComputeResponse(ha1, ha2);

            return serverResponse != null && serverResponse.Equals(header.Response, StringComparison.Ordinal) && Condition.IsNotNull(result);
        }

        private DigestAuthorizationHeader AuthorizationHeaderParser(HttpContext context, string authorizationHeader)
        {
            return DigestAuthorizationHeader.Create(authorizationHeader);
        }

        private static string ParseAlgorithm(UnkeyedCryptoAlgorithm algorithm)
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