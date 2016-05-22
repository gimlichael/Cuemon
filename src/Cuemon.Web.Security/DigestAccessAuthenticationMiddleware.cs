using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;

namespace Cuemon.Web.Security
{
    /// <summary>
    /// Provides a HTTP Digest Access Authentication middleware implementation for ASP.NET Core.
    /// </summary>
    public class DigestAccessAuthenticationMiddleware
    {
        private static readonly Dictionary<string, Template<DateTime, string>> NonceCounter = new Dictionary<string, Template<DateTime, string>>();
        private readonly RequestDelegate _next;

        /// <summary>
        /// Initializes a new instance of the <see cref="DigestAccessAuthenticationMiddleware"/> class.
        /// </summary>
        /// <param name="next">The delegate of the request pipeline to invoke.</param>
        /// <param name="options">The HTTP Digest Access Authentication middleware options.</param>
        public DigestAccessAuthenticationMiddleware(RequestDelegate next, Doer<DigestAccessAuthenticationOptions> options)
        {
            Validator.ThrowIfNull(options, nameof(options));
            Options = options();
            _next = next;
        }

        /// <summary>
        /// Executes the <see cref="DigestAccessAuthenticationMiddleware"/>.
        /// </summary>
        /// <param name="context">The context of the current request.</param>
        /// <returns>A task that represents the execution of this middleware.</returns>
        public async Task Invoke(HttpContext context)
        {

            if (!AuthenticationUtility.TryAuthenticate(context, Options.RequireSecureConnection, AuthorizationParser, PrincipalParser))
            {
                context.Response.StatusCode = AuthenticationUtility.HttpNotAuthorizedStatusCode;
                string etag = context.Response.Headers[AuthenticationUtility.HttpEtagHeader];
                if (string.IsNullOrEmpty(etag)) { etag = "no-entity-tag"; }
                Doer<string> opaqueGenerator = Options.OpaqueGenerator;
                Doer<byte[]> nonceSecret = Options.NonceSecret;
                Doer<DateTime, string, byte[], string> nonceGenerator = Options.NonceGenerator;
                string staleNonce = context.Items["staleNonce"] as string ?? "FALSE";
                context.Response.Headers.Add(AuthenticationUtility.HttpWwwAuthenticateHeader, "{0} realm=\"{1}\", qop=\"{2}\", nonce=\"{3}\", opaque=\"{4}\", stale=\"{5}\", algorithm=\"{6}\"".FormatWith(
                    AuthenticationSchemeName,
                    Options.Realm,
                    DigestAuthenticationUtility.CredentialQualityOfProtectionOptions,
                    nonceGenerator(DateTime.UtcNow, etag, nonceSecret()),
                    opaqueGenerator(),
                    staleNonce,
                    DigestAuthenticationUtility.ParseAlgorithm(Options.Algorithm)));
                return;
            }

            await _next.Invoke(context);
        }

        private DigestAccessAuthenticationOptions Options { get; }

        /// <summary>
        /// Gets the name of the authentication scheme.
        /// </summary>
        /// <value>The name of the authentication scheme.</value>
        public string AuthenticationSchemeName
        {
            get { return "Digest"; }
        }

        private bool PrincipalParser(HttpContext context, Dictionary<string, string> credentials, out ClaimsPrincipal result)
        {
            if (Options.CredentialsValidator == null) { throw new InvalidOperationException("The CredentialsValidator delegate cannot be null."); }
            string password, userName, clientResponse, nonce, nonceCount;
            credentials.TryGetValue(DigestAuthenticationUtility.CredentialUserName, out userName);
            credentials.TryGetValue(DigestAuthenticationUtility.CredentialResponse, out clientResponse);
            credentials.TryGetValue(DigestAuthenticationUtility.CredentialNonceCount, out nonceCount);
            if (credentials.TryGetValue(DigestAuthenticationUtility.CredentialNonce, out nonce))
            {
                result = null;
                Doer<string, TimeSpan, bool> nonceExpiredParser = Options.NonceExpiredParser;
                bool staleNonce = nonceExpiredParser(nonce, TimeSpan.FromSeconds(30));
                context.Items["staleNonce"] = staleNonce.ToString().ToUpperInvariant();
                if (staleNonce) { return false; }

                lock (NonceCounter)
                {
                    Template<DateTime, string> previousNonce;
                    if (NonceCounter.TryGetValue(nonce, out previousNonce))
                    {
                        if (previousNonce.Arg2.Equals(nonceCount, StringComparison.Ordinal)) { return false; }
                    }
                    else
                    {
                        NonceCounter.Add(nonce, TupleUtility.CreateTwo(DateTime.UtcNow, nonceCount));
                    }
                }
            }
            result = Options.CredentialsValidator(userName, out password);
            string ha1 = DigestAuthenticationUtility.ComputeHash1(credentials, password, Options.Algorithm);
            string ha2 = DigestAuthenticationUtility.ComputeHash2(credentials, context.Request.Method, Options.Algorithm);
            string serverResponse = DigestAuthenticationUtility.ComputeResponse(credentials, ha1, ha2, Options.Algorithm);
            return serverResponse.Equals(clientResponse, StringComparison.Ordinal) && Condition.IsNotNull(result);
        }

        private Dictionary<string, string> AuthorizationParser(string authorizationHeader)
        {
            if (AuthenticationUtility.IsAuthenticationSchemeValid(authorizationHeader, AuthenticationSchemeName))
            {
                string digestCredentials = authorizationHeader.Remove(0, AuthenticationSchemeName.Length + 1);
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
        /// Adds a HTTP Basic Authentication scheme to the <see cref="IApplicationBuilder"/> request execution pipeline.
        /// </summary>
        /// <param name="builder">The type that provides the mechanisms to configure an application’s request pipeline.</param>
        /// <param name="options">The HTTP Digest Access Authentication middleware options.</param>
        /// <returns>A reference to this instance after the operation has completed.</returns>
        public static IApplicationBuilder UseDigestAccessAuthentication(this IApplicationBuilder builder, Doer<DigestAccessAuthenticationOptions> options)
        {
            return builder.UseMiddleware<DigestAccessAuthenticationMiddleware>(options);
        }
    }
}