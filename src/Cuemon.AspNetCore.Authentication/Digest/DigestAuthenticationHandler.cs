using Microsoft.AspNetCore.Authentication;
using System;
using System.Collections.Generic;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.Net.Http.Headers;
using System.Globalization;
using Cuemon.Collections.Generic;
using Microsoft.AspNetCore.Http;

namespace Cuemon.AspNetCore.Authentication.Digest
{
	/// <summary>
	/// Provides a HTTP Digest Access Authentication implementation of <see cref="AuthenticationHandler{TOptions}"/> for ASP.NET Core.
	/// </summary>
	/// <seealso cref="AuthenticationHandler{TOptions}" />
	public class DigestAuthenticationHandler : AuthenticationHandler<DigestAuthenticationOptions>
	{
		private readonly INonceTracker _nonceTracker;

		/// <summary>
		/// Initializes a new instance of the <see cref="DigestAuthenticationHandler"/> class.
		/// </summary>
		/// <param name="options">The monitor for the options instance.</param>
		/// <param name="logger">The <see cref="T:Microsoft.Extensions.Logging.ILoggerFactory" />.</param>
		/// <param name="encoder">The <see cref="T:System.Text.Encodings.Web.UrlEncoder" />.</param>
		/// <param name="clock">The <see cref="T:Microsoft.AspNetCore.Authentication.ISystemClock" />.</param>
		/// <param name="nonceTracker">The dependency injected implementation of an <see cref="INonceTracker"/>.</param>
		public DigestAuthenticationHandler(IOptionsMonitor<DigestAuthenticationOptions> options, ILoggerFactory logger, UrlEncoder encoder, ISystemClock clock, INonceTracker nonceTracker) : base(options, logger, encoder, clock)
		{
			_nonceTracker = nonceTracker;
		}

		/// <summary>
		/// Handle authenticate as an asynchronous operation.
		/// </summary>
		/// <returns>A <see cref="Task{TResult}"/> representing the asynchronous operation.</returns>
		protected override Task<AuthenticateResult> HandleAuthenticateAsync()
		{
			Context.Items.TryAdd(nameof(DigestAuthenticationOptions), Options);
			Context.Items.TryAdd(nameof(INonceTracker), _nonceTracker);

			if (!Authenticator.TryAuthenticate(Context, Options.RequireSecureConnection, DigestAuthenticationMiddleware.AuthorizationHeaderParser, DigestAuthenticationMiddleware.TryAuthenticate, out var principal))
			{
				return Task.FromResult(AuthenticateResult.Fail(Options.UnauthorizedMessage));
			}

			var ticket = new AuthenticationTicket(principal, DigestAuthorizationHeader.Scheme);
			return Task.FromResult(AuthenticateResult.Success(ticket));
		}
		
		/// <summary>
		/// Handle challenge as an asynchronous operation.
		/// </summary>
		/// <param name="properties">The properties.</param>
		/// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
		/// <remarks><c>qop</c> is included and supported to be compliant with RFC 2617 (hence, this implementation cannot revert to reduced legacy RFC 2069 mode).</remarks>
		protected override async Task HandleChallengeAsync(AuthenticationProperties properties)
		{
			string etag = Response.Headers[HeaderNames.ETag];
			if (string.IsNullOrEmpty(etag)) { etag = "no-entity-tag"; }
			var opaqueGenerator = Options.OpaqueGenerator;
			var nonceSecret = Options.NonceSecret;
			var nonceGenerator = Options.NonceGenerator;
			var staleNonce = Context.Items[DigestFields.Stale] as string ?? "false";
			Decorator.Enclose(Response.Headers).TryAdd(HeaderNames.WWWAuthenticate, string.Create(CultureInfo.InvariantCulture, $"{DigestAuthorizationHeader.Scheme} realm=\"{Options.Realm}\", qop=\"auth, auth-int\", nonce=\"{nonceGenerator(DateTime.UtcNow, etag, nonceSecret())}\", opaque=\"{opaqueGenerator()}\", stale=\"{staleNonce}\", algorithm=\"{DigestAuthenticationMiddleware.ParseAlgorithm(Options.Algorithm)}\""));
			Response.StatusCode = StatusCodes.Status401Unauthorized;
			Response.ContentType = "text/plain";
			Response.ContentLength = Options.UnauthorizedMessage.Length;
			await Response.Body.WriteAsync(Decorator.Enclose(Options.UnauthorizedMessage).ToByteArray()).ConfigureAwait(false);
		}
	}
}
