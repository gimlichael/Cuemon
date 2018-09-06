using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using Cuemon.AspNetCore.Builder;
using Cuemon.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using Microsoft.Net.Http.Headers;

namespace Cuemon.AspNetCore.Authentication
{
    /// <summary>
    /// Provides a HTTP Digest Access Authentication middleware implementation for ASP.NET Core.
    /// </summary>
    public class DigestAccessAuthenticationMiddleware : ConfigurableMiddleware<DigestAccessAuthenticationOptions>
    {
        private static readonly ConcurrentDictionary<string, Template<DateTime, string>> NonceCounter = new ConcurrentDictionary<string, Template<DateTime, string>>();
        private static Timer _nonceCounterSweeper;

        /// <summary>
        /// Initializes a new instance of the <see cref="DigestAccessAuthenticationMiddleware"/> class.
        /// </summary>
        /// <param name="next">The delegate of the request pipeline to invoke.</param>
        /// <param name="setup">The <see cref="DigestAccessAuthenticationOptions" /> which need to be configured.</param>
        public DigestAccessAuthenticationMiddleware(RequestDelegate next, IOptions<DigestAccessAuthenticationOptions> setup) : base(next, setup)
        {
            InitializeNonceCounterSweeper();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="DigestAccessAuthenticationMiddleware"/> class.
        /// </summary>
        /// <param name="next">The delegate of the request pipeline to invoke.</param>
        /// <param name="setup">The middleware <see cref="DigestAccessAuthenticationOptions"/> which need to be configured.</param>
        public DigestAccessAuthenticationMiddleware(RequestDelegate next, Action<DigestAccessAuthenticationOptions> setup) : base(next, setup)
        {
            InitializeNonceCounterSweeper();
        }

        private void InitializeNonceCounterSweeper()
        {
            _nonceCounterSweeper = new Timer(s =>
            {
                DateTime utcStaleTimestamp = DateTime.UtcNow.Subtract(TimeSpan.FromMinutes(5));
                var staledEntries = NonceCounter.Where(pair => pair.Value.Arg1 <= utcStaleTimestamp).ToList();
                foreach (var staledEntry in staledEntries)
                {
                    TesterDoer.IgnoreResult<string, Template<DateTime, string>, bool>(NonceCounter.TryRemove, staledEntry.Key);
                }
            }, null, TimeSpan.FromMinutes(1), TimeSpan.FromMinutes(2));
        }

        /// <summary>
        /// Executes the <see cref="DigestAccessAuthenticationMiddleware"/>.
        /// </summary>
        /// <param name="context">The context of the current request.</param>
        /// <returns>A task that represents the execution of this middleware.</returns>
        public override async Task InvokeAsync(HttpContext context)
        {
            if (!AuthenticationUtility.TryAuthenticate(context, Options.RequireSecureConnection, AuthorizationHeaderParser, TryAuthenticate))
            {
                context.Response.StatusCode = AuthenticationUtility.HttpNotAuthorizedStatusCode;
                string etag = context.Response.Headers[HeaderNames.ETag];
                if (string.IsNullOrEmpty(etag)) { etag = "no-entity-tag"; }
                Func<string> opaqueGenerator = Options.OpaqueGenerator;
                Func<byte[]> nonceSecret = Options.NonceSecret;
                Func<DateTime, string, byte[], string> nonceGenerator = Options.NonceGenerator;
                string staleNonce = context.Items["staleNonce"] as string ?? "FALSE";
                context.Response.Headers.Add(HeaderNames.WWWAuthenticate, "{0} realm=\"{1}\", qop=\"{2}\", nonce=\"{3}\", opaque=\"{4}\", stale=\"{5}\", algorithm=\"{6}\"".FormatWith(
                    AuthenticationScheme,
                    Options.Realm,
                    DigestAuthenticationUtility.CredentialQualityOfProtectionOptions,
                    nonceGenerator(DateTime.UtcNow, etag, nonceSecret()),
                    opaqueGenerator(),
                    staleNonce,
                    DigestAuthenticationUtility.ParseAlgorithm(Options.Algorithm)));
                await context.WriteHttpNotAuthorizedBody(Options.HttpNotAuthorizedBody).ContinueWithSuppressedContext();
                return;
            }
            await Next.Invoke(context).ContinueWithSuppressedContext();
        }

        /// <summary>
        /// Gets the name of the authentication scheme.
        /// </summary>
        /// <value>The name of the authentication scheme.</value>
        public string AuthenticationScheme => "Digest";

        private bool TryAuthenticate(HttpContext context, Dictionary<string, string> credentials, out ClaimsPrincipal result)
        {
            if (Options.Authenticator == null) { throw new InvalidOperationException("The {0} delegate cannot be null.".FormatWith(Options.Authenticator)); }
            string password, userName, clientResponse, nonce, nonceCount;
            credentials.TryGetValue(DigestAuthenticationUtility.CredentialUserName, out userName);
            credentials.TryGetValue(DigestAuthenticationUtility.CredentialResponse, out clientResponse);
            credentials.TryGetValue(DigestAuthenticationUtility.CredentialNonceCount, out nonceCount);
            if (credentials.TryGetValue(DigestAuthenticationUtility.CredentialNonce, out nonce))
            {
                result = null;
                Func<string, TimeSpan, bool> nonceExpiredParser = Options.NonceExpiredParser;
                bool staleNonce = nonceExpiredParser(nonce, TimeSpan.FromSeconds(30));
                context.Items["staleNonce"] = staleNonce.ToString().ToUpperInvariant();
                if (staleNonce) { return false; }
                Template<DateTime, string> previousNonce;
                if (NonceCounter.TryGetValue(nonce, out previousNonce))
                {
                    if (previousNonce.Arg2.Equals(nonceCount, StringComparison.Ordinal)) { return false; }
                }
                else
                {
                    NonceCounter.TryAdd(nonce, TupleUtility.CreateTwo(DateTime.UtcNow, nonceCount));
                }
            }
            result = Options.Authenticator(userName, out password);

            string serverResponse = Options?.DigestAccessSigner(new DigestAccessAuthenticationParameters(credentials.ToImmutableDictionary(), context.Request.Method, password, Options.Algorithm))?.ToHexadecimal();
            return serverResponse != null && (serverResponse.Equals(clientResponse, StringComparison.Ordinal) && Condition.IsNotNull(result));
        }

        private Dictionary<string, string> AuthorizationHeaderParser(HttpContext context, string authorizationHeader)
        {
            if (AuthenticationUtility.IsAuthenticationSchemeValid(authorizationHeader, AuthenticationScheme))
            {
                string digestCredentials = authorizationHeader.Remove(0, AuthenticationScheme.Length + 1);
                string[] credentials = digestCredentials.Split(AuthenticationUtility.DigestAuthenticationCredentialSeparator);
                if (IsDigestCredentialsValid(credentials))
                {
                    Dictionary<string, string> result = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
                    for (int i = 0; i < credentials.Length; i++)
                    {
                        string[] credentialPair = StringUtility.Split(credentials[i], "=");
                        result.Add(credentialPair[0].Trim(), Converter.Parse(credentialPair[1], QuotedStringParser));
                    }
                    return IsDigestCredentialsValid(result) ? result : null;
                }
            }
            return null;
        }

        private static bool IsDigestCredentialsValid(Dictionary<string, string> credentials)
        {
            bool valid = credentials.ContainsKey("username");
            valid |= credentials.ContainsKey("realm");
            valid |= credentials.ContainsKey("nonce");
            valid |= credentials.ContainsKey("uri");
            valid |= credentials.ContainsKey("response");
            return valid;
        }

        private string QuotedStringParser(string value)
        {
            if (value.StartsWith("\"", StringComparison.OrdinalIgnoreCase) &&
                value.EndsWith("\"", StringComparison.OrdinalIgnoreCase))
            {
                value = value.Trim('"');
            }
            return value.Trim();
        }

        private static bool IsDigestCredentialsValid(string[] credentials)
        {
            bool valid = (credentials.Length >= 5 && credentials.Length <= 10);
            for (int i = 0; i < credentials.Length; i++)
            {
                valid |= !string.IsNullOrEmpty(credentials[i]);
            }
            return valid;
        }
    }

    /// <summary>
    /// This is a factory implementation of the <see cref="DigestAccessAuthenticationMiddleware"/> class.
    /// </summary>
    public static class DigestAccessAuthenticationBuilderExtension
    {
        /// <summary>
        /// Adds a HTTP Digest Authentication scheme to the <see cref="IApplicationBuilder"/> request execution pipeline.
        /// </summary>
        /// <param name="builder">The type that provides the mechanisms to configure an application’s request pipeline.</param>
        /// <param name="setup">The HTTP <see cref="DigestAccessAuthenticationMiddleware"/> middleware which need to be configured.</param>
        /// <returns>A reference to this instance after the operation has completed.</returns>
        public static IApplicationBuilder UseDigestAccessAuthentication(this IApplicationBuilder builder, Action<DigestAccessAuthenticationOptions> setup = null)
        {
            return ApplicationBuilderFactory.UseMiddlewareConfigurable<DigestAccessAuthenticationMiddleware, DigestAccessAuthenticationOptions>(builder, setup);
        }
    }
}