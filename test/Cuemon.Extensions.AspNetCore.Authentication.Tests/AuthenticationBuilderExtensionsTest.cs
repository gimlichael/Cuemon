using System.Security.Claims;
using Cuemon.AspNetCore.Authentication.Basic;
using Cuemon.AspNetCore.Authentication.Digest;
using Cuemon.Extensions.Xunit;
using Cuemon.Extensions.Xunit.Hosting.AspNetCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Xunit;
using Xunit.Abstractions;

namespace Cuemon.Extensions.AspNetCore.Authentication
{
	public class AuthenticationBuilderExtensionsTest : Test
	{
		public AuthenticationBuilderExtensionsTest(ITestOutputHelper output) : base(output)
		{
		}

		[Fact]
		public void AddBasic_ShouldAddBasicAuthenticationHandlerAndBasicAuthenticationOptions_ToHost()
		{
			using (var host = MiddlewareTestFactory.Create(services =>
				   {
					   services.AddAuthentication(BasicAuthorizationHeader.Scheme)
						   .AddBasic(o =>
						   {
							   o.Authenticator = (username, password) => ClaimsPrincipal.Current;
						   });
				   }))
			{
				var options = host.ServiceProvider.GetRequiredService<IOptions<BasicAuthenticationOptions>>();
				var handler = host.ServiceProvider.GetRequiredService<BasicAuthenticationHandler>();

				Assert.NotNull(options);
				Assert.NotNull(handler);
				Assert.IsType<BasicAuthenticationOptions>(options.Value);
				Assert.IsType<BasicAuthenticationHandler>(handler);
			}
		}

		[Fact]
		public void AddDigestAccess_ShouldAddDigestAuthenticationHandlerAndDigestAuthenticationOptions_ToHost()
		{
			using (var host = MiddlewareTestFactory.Create(services =>
				   {
					   services.AddAuthentication(DigestAuthorizationHeader.Scheme)
						   .AddDigestAccess(o =>
						   {
							   o.Authenticator = (string username, out string password) =>
							   {
								   password = "";
								   return ClaimsPrincipal.Current;
							   };
						   });
				   }))
			{
				var options = host.ServiceProvider.GetRequiredService<IOptions<DigestAuthenticationOptions>>();
				var handler = host.ServiceProvider.GetRequiredService<DigestAuthenticationHandler>();

				Assert.NotNull(options);
				Assert.NotNull(handler);
				Assert.IsType<DigestAuthenticationOptions>(options.Value);
				Assert.IsType<DigestAuthenticationHandler>(handler);
			}
		}
	}
}
