using System.Security.Claims;
using Cuemon.AspNetCore.Authentication.Basic;
using Cuemon.AspNetCore.Authentication.Digest;
using Cuemon.AspNetCore.Authentication.Hmac;
using Cuemon.Extensions.Xunit;
using Cuemon.Extensions.Xunit.Hosting;
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
            using (var host = WebHostTestFactory.Create(services =>
                   {
                       services.AddAuthentication(BasicAuthorizationHeader.Scheme)
                           .AddBasic(o =>
                           {
                               o.Authenticator = (username, password) => ClaimsPrincipal.Current;
                           });
                   }))
            {
                var options = host.ServiceProvider.GetRequiredScopedService<IOptionsSnapshot<BasicAuthenticationOptions>>().Get(BasicAuthorizationHeader.Scheme);
                var handler = host.ServiceProvider.GetRequiredService<BasicAuthenticationHandler>();

                Assert.NotNull(options);
                Assert.NotNull(handler);
                Assert.IsType<BasicAuthenticationOptions>(options);
                Assert.IsType<BasicAuthenticationHandler>(handler);
            }
        }

        [Fact]
        public void AddDigestAccess_ShouldAddDigestAuthenticationHandlerAndDigestAuthenticationOptions_ToHost()
        {
            using (var host = WebHostTestFactory.Create(services =>
                   {
                       services.AddInMemoryDigestAuthenticationNonceTracker();
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
                var options = host.ServiceProvider.GetRequiredScopedService<IOptionsSnapshot<DigestAuthenticationOptions>>().Get(DigestAuthorizationHeader.Scheme);
                var handler = host.ServiceProvider.GetRequiredService<DigestAuthenticationHandler>();

                Assert.NotNull(options);
                Assert.NotNull(handler);
                Assert.IsType<DigestAuthenticationOptions>(options);
                Assert.IsType<DigestAuthenticationHandler>(handler);
            }
        }

        [Fact]
        public void AddHmac_ShouldAddHmacAuthenticationHandlerAndHmacAuthenticationOptions_ToHost()
        {
            using (var host = WebHostTestFactory.Create(services =>
                   {
                       services.AddAuthentication(HmacFields.Scheme)
                           .AddHmac(o =>
                           {
                               o.Authenticator = (string clientId, out string clientSecret) =>
                               {
                                   clientSecret = "";
                                   return ClaimsPrincipal.Current;
                               };
                           });
                   }))
            {
                var options = host.ServiceProvider.GetRequiredScopedService<IOptionsSnapshot<HmacAuthenticationOptions>>().Get(HmacFields.Scheme);
                var handler = host.ServiceProvider.GetRequiredService<HmacAuthenticationHandler>();

                Assert.NotNull(options);
                Assert.NotNull(handler);
                Assert.IsType<HmacAuthenticationOptions>(options);
                Assert.IsType<HmacAuthenticationHandler>(handler);
            }
        }
    }
}
