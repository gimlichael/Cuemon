using System;
using System.Collections.Immutable;
using System.Security.Claims;
using System.Threading.Tasks;
using Cuemon.IO;
using Cuemon.Security.Cryptography;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using Microsoft.Net.Http.Headers;

namespace Cuemon.AspNetCore.Authentication
{
    /// <summary>
    /// Provides a HTTP Digest Access Authentication middleware implementation for ASP.NET Core.
    /// </summary>
    public class DigestAccessAuthenticationMiddleware : ConfigurableMiddleware<INonceTracker, DigestAccessAuthenticationOptions>
    {
        private INonceTracker _nonceTracker;

        /// <summary>
        /// Initializes a new instance of the <see cref="DigestAccessAuthenticationMiddleware"/> class.
        /// </summary>
        /// <param name="next">The delegate of the request pipeline to invoke.</param>
        /// <param name="setup">The <see cref="DigestAccessAuthenticationOptions" /> which need to be configured.</param>
        public DigestAccessAuthenticationMiddleware(RequestDelegate next, IOptions<DigestAccessAuthenticationOptions> setup) : base(next, setup)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="DigestAccessAuthenticationMiddleware"/> class.
        /// </summary>
        /// <param name="next">The delegate of the request pipeline to invoke.</param>
        /// <param name="setup">The middleware <see cref="DigestAccessAuthenticationOptions"/> which need to be configured.</param>
        public DigestAccessAuthenticationMiddleware(RequestDelegate next, Action<DigestAccessAuthenticationOptions> setup) : base(next, setup)
        {
        }

        /// <summary>
        /// Executes the <see cref="DigestAccessAuthenticationMiddleware"/>.
        /// </summary>
        /// <param name="context">The context of the current request.</param>
        /// <param name="nonceTracker">The dependency injected implementation of an <see cref="INonceTracker"/>.</param>
        /// <returns>A task that represents the execution of this middleware.</returns>
        public override async Task InvokeAsync(HttpContext context, INonceTracker nonceTracker)
        {
            _nonceTracker = nonceTracker;
            if (!AuthenticationUtility.TryAuthenticate(context, Options.RequireSecureConnection, AuthorizationHeaderParser, TryAuthenticate))
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
                        var staleNonce = context.Items["staleNonce"] as string ?? "false";
                        context.Response.Headers.Add(HeaderNames.WWWAuthenticate, FormattableString.Invariant($"{AuthenticationScheme} realm=\"{Options.Realm}\", qop=\"auth, auth-int\", nonce=\"{nonceGenerator(DateTime.UtcNow, etag, nonceSecret())}\", opaque=\"{opaqueGenerator()}\", stale=\"{staleNonce}\", algorithm=\"{ParseAlgorithm(Options.Algorithm)}\""));
                        return Task.CompletedTask;
                    });
                    response.StatusCode = (int)message.StatusCode;
                    await Decorator.Enclose(response.Body).WriteAsync(await message.Content.ReadAsByteArrayAsync().ConfigureAwait(false)).ConfigureAwait(false);
                }).ConfigureAwait(false);
            }
            await Next.Invoke(context).ConfigureAwait(false);
        }

        /// <summary>
        /// Gets the name of the authentication scheme.
        /// </summary>
        /// <value>The name of the authentication scheme.</value>
        public string AuthenticationScheme => "Digest";

        private bool TryAuthenticate(HttpContext context, ImmutableDictionary<string, string> credentials, out ClaimsPrincipal result)
        {
            if (Options.Authenticator == null) { throw new InvalidOperationException(FormattableString.Invariant($"The {nameof(Options.Authenticator)} delegate cannot be null.")); }
            credentials.TryGetValue(DigestHeaders.UserName, out var userName);
            credentials.TryGetValue(DigestHeaders.Response, out var clientResponse);
            credentials.TryGetValue(DigestHeaders.NonceCount, out var nonceCount);
            if (credentials.TryGetValue(DigestHeaders.Nonce, out var nonce))
            {
                result = null;
                var nonceExpiredParser = Options.NonceExpiredParser;
                var staleNonce = nonceExpiredParser(nonce, TimeSpan.FromSeconds(30));
                context.Items["staleNonce"] = staleNonce.ToString().ToUpperInvariant();
                if (staleNonce) { return false; }

                if (_nonceTracker != null)
                {
                    var nc = Convert.ToInt32(nonceCount, 16);
                    if (_nonceTracker.TryGetEntry(nonce, out var previousNonce))
                    {
                        if (previousNonce.Count == nc) { return false; }
                    }
                    else
                    {
                        _nonceTracker.TryAddEntry(nonce, nc);
                    }
                }
            }
            result = Options.Authenticator(userName, out var password);
            var serverResponse = Options?.DigestAccessSigner(new DigestAccessAuthenticationParameters(credentials, context.Request.Method, password, Decorator.Enclose(context.Response.Body).ToEncodedString(o => o.LeaveOpen = true), Options.Algorithm));
            return serverResponse != null && serverResponse.Equals(clientResponse, StringComparison.Ordinal) && Condition.IsNotNull(result);
        }

        internal ImmutableDictionary<string, string> AuthorizationHeaderParser(HttpContext context, string authorizationHeader)
        {
            var id = new DigestHeaderBuilder(Options.Algorithm)
                .AddFromDigestHeader(authorizationHeader)
                .ToImmutableDictionary();
            return IsDigestCredentialsValid(id) ? id : null;
        }

        private static bool IsDigestCredentialsValid(ImmutableDictionary<string, string> credentials)
        {
            var valid = credentials.ContainsKey("username");
            valid |= credentials.ContainsKey("realm");
            valid |= credentials.ContainsKey("nonce");
            valid |= credentials.ContainsKey("uri");
            valid |= credentials.ContainsKey("response");
            return valid;
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