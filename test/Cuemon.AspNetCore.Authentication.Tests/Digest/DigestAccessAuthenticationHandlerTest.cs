using System.Net.Http;
using System.Security.Claims;
using System.Threading.Tasks;
using Cuemon.AspNetCore.Authentication.Assets;
using Cuemon.Collections.Generic;
using Cuemon.Extensions.AspNetCore.Authentication;
using Codebelt.Extensions.Xunit;
using Codebelt.Extensions.Xunit.Hosting;
using Codebelt.Extensions.Xunit.Hosting.AspNetCore;
using Cuemon.Security.Cryptography;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Microsoft.Net.Http.Headers;
using Xunit;
using Xunit.Abstractions;

namespace Cuemon.AspNetCore.Authentication.Digest
{
    public class DigestAccessAuthenticationHandlerTest : Test
    {
        public DigestAccessAuthenticationHandlerTest(ITestOutputHelper output) : base(output)
        {
        }

        [Fact]
        public async Task HandleAuthenticateAsync_ShouldReturnContent_WithQopAuthentication()
        {
            using (var webApp = WebHostTestFactory.Create(services =>
                   {
                       services.AddInMemoryDigestAuthenticationNonceTracker();
                       services.AddControllers().AddApplicationPart(typeof(FakeController).Assembly);
                       services
                           .AddAuthentication(DigestAuthorizationHeader.Scheme)
                           .AddDigestAccess(o =>
                           {
                               o.Authenticator = (string username, out string password) =>
                               {
                                   if (username == "Agent")
                                   {
                                       password = "Test";
                                       var cp = new ClaimsPrincipal();
                                       cp.AddIdentity(new ClaimsIdentity(Arguments.Yield(new Claim("Name", "Test Agent")), DigestAuthorizationHeader.Scheme));
                                       return cp;
                                   }
                                   password = null;
                                   return null;
                               };
                               o.RequireSecureConnection = false;
                           });
                   }, app =>
                   {
                       app.UseAuthentication();

                       app.UseRouting();

                       app.UseAuthorization();

                       app.UseEndpoints(routes => { routes.MapControllers(); });
                   }))
            {

                var options = webApp.ServiceProvider.GetRequiredScopedService<IOptionsSnapshot<DigestAuthenticationOptions>>().Get(DigestAuthorizationHeader.Scheme);
                var client = webApp.Host.GetTestClient();

                var result = await client.GetAsync("/fake");

                var wwwAuthenticate = result.Headers.WwwAuthenticate.ToString();

                TestOutput.WriteLine("WWW-Authenticate:");
                TestOutput.WriteLine(wwwAuthenticate);

                var db = new DigestAuthorizationHeaderBuilder(options.Algorithm)
                    .AddRealm(options.Realm)
                    .AddUserName("Agent")
                    .AddUri("/")
                    .AddNc(1)
                    .AddCnonce()
                    .AddQopAuthentication()
                    .AddFromWwwAuthenticateHeader(result.Headers);

                var ha1 = db.ComputeHash1("Test");
                var ha2 = db.ComputeHash2("GET");

                db.ComputeResponse(ha1, ha2);
                db.AddResponse("Test", "GET");

                var token = db.Build().ToString();

                TestOutput.WriteLine("Token:");
                TestOutput.WriteLine(token);

                client.DefaultRequestHeaders.Add(HeaderNames.Authorization, token);

                result = await client.GetAsync("/fake");

                Assert.Equal("Unit Test", await result.Content.ReadAsStringAsync());
                Assert.Equal(StatusCodes.Status200OK, (int)result.StatusCode);
            }
        }

        [Fact]
        public async Task HandleAuthenticateAsync_ShouldReturnContent_QopAuthenticationIntegrity()
        {
            using (var webApp = WebHostTestFactory.Create(services =>
                   {
                       services.AddInMemoryDigestAuthenticationNonceTracker();
                       services.AddControllers().AddApplicationPart(typeof(FakeController).Assembly);
                       services
                           .AddAuthentication(DigestAuthorizationHeader.Scheme)
                           .AddDigestAccess(o =>
                           {
                               o.Authenticator = (string username, out string password) =>
                               {
                                   if (username == "Agent")
                                   {
                                       password = "Test";
                                       var cp = new ClaimsPrincipal();
                                       cp.AddIdentity(new ClaimsIdentity(Arguments.Yield(new Claim("Name", "Test Agent")), DigestAuthorizationHeader.Scheme));
                                       return cp;
                                   }
                                   password = null;
                                   return null;
                               };
                               o.RequireSecureConnection = false;
                           });
                   }, app =>
                   {
                       app.UseAuthentication();

                       app.UseRouting();

                       app.UseAuthorization();

                       app.UseEndpoints(routes => { routes.MapControllers(); });
                   }))
            {

                var options = webApp.ServiceProvider.GetRequiredScopedService<IOptionsSnapshot<DigestAuthenticationOptions>>().Get(DigestAuthorizationHeader.Scheme);
                var client = webApp.Host.GetTestClient();

                var result = await client.GetAsync("/fake");

                var wwwAuthenticate = result.Headers.WwwAuthenticate.ToString();

                TestOutput.WriteLine("WWW-Authenticate:");
                TestOutput.WriteLine(wwwAuthenticate);

                var db = new DigestAuthorizationHeaderBuilder(options.Algorithm)
                    .AddRealm(options.Realm)
                    .AddUserName("Agent")
                    .AddUri("/")
                    .AddNc(1)
                    .AddCnonce()
                    .AddQopAuthenticationIntegrity()
                    .AddFromWwwAuthenticateHeader(result.Headers);

                var entityBody = "test of entityBody in request";

                var ha1 = db.ComputeHash1("Test");
                var ha2 = db.ComputeHash2("POST", entityBody);
                db.ComputeResponse(ha1, ha2);

                db.AddResponse("Test", "POST", entityBody);

                var token = db.Build().ToString();

                TestOutput.WriteLine("Token:");
                TestOutput.WriteLine(token);

                client.DefaultRequestHeaders.Add(HeaderNames.Authorization, token);
                result = await client.PostAsync("/fake", new StringContent(entityBody));

                Assert.Equal("Unit Test", await result.Content.ReadAsStringAsync());
                Assert.Equal(StatusCodes.Status200OK, (int)result.StatusCode);
            }
        }

        [Fact]
        public async Task HandleAuthenticateAsync_ShouldReturnContent_WithServerSideHa1Storage()
        {
            using (var webApp = WebHostTestFactory.Create(services =>
                   {
                       services.AddInMemoryDigestAuthenticationNonceTracker();
                       services.AddControllers().AddApplicationPart(typeof(FakeController).Assembly);
                       services
                           .AddAuthentication(DigestAuthorizationHeader.Scheme)
                           .AddDigestAccess(o =>
                           {
                               o.Algorithm = UnkeyedCryptoAlgorithm.Sha512;
                               o.UseServerSideHa1Storage = true;
                               o.Authenticator = (string username, out string password) =>
                               {
                                   if (username == "Agent")
                                   {
                                       password = "64d7c739de5dc6b5149de600751c413ef74fab0419e5a656e9f5ead5b98105b8c75ba6e18850ccd7ef2a3a13517520e158181bd34a4b68e2ab3b728acd7d066b";
                                       var cp = new ClaimsPrincipal();
                                       cp.AddIdentity(new ClaimsIdentity(Arguments.Yield(new Claim("Name", "Test Agent")), DigestAuthorizationHeader.Scheme));
                                       return cp;
                                   }
                                   password = null;
                                   return null;
                               };
                               o.RequireSecureConnection = false;
                           });
                   }, app =>
                   {
                       app.UseAuthentication();

                       app.UseRouting();

                       app.UseAuthorization();

                       app.UseEndpoints(routes => { routes.MapControllers(); });
                   }))
            {

                var options = webApp.ServiceProvider.GetRequiredScopedService<IOptionsSnapshot<DigestAuthenticationOptions>>().Get(DigestAuthorizationHeader.Scheme);
                var client = webApp.Host.GetTestClient();

                var result = await client.GetAsync("/fake");

                var wwwAuthenticate = result.Headers.WwwAuthenticate.ToString();

                TestOutput.WriteLine("WWW-Authenticate:");
                TestOutput.WriteLine(wwwAuthenticate);

                var db = new DigestAuthorizationHeaderBuilder(options.Algorithm)
                    .AddRealm(options.Realm)
                    .AddUserName("Agent")
                    .AddUri("/")
                    .AddNc(1)
                    .AddCnonce()
                    .AddQopAuthentication()
                    .AddFromWwwAuthenticateHeader(result.Headers);

                var ha1 = db.ComputeHash1("Test");
                var ha2 = db.ComputeHash2("GET");
                db.ComputeResponse(ha1, ha2);

                TestOutput.WriteLine(ha1);

                db.AddResponse("Test", "GET");

                var token = db.Build().ToString();

                TestOutput.WriteLine("Token:");
                TestOutput.WriteLine(token);

                client.DefaultRequestHeaders.Add(HeaderNames.Authorization, token);

                result = await client.GetAsync("/fake");

                Assert.Equal(UnkeyedCryptoAlgorithm.Sha512, options.Algorithm);
                Assert.True(options.UseServerSideHa1Storage);
                Assert.False(options.RequireSecureConnection);

                Assert.Equal("Unit Test", await result.Content.ReadAsStringAsync());
                Assert.Equal(StatusCodes.Status200OK, (int)result.StatusCode);
            }
        }

        [Fact]
        public async Task HandleAuthenticateAsync_ShouldReturn401WithUnauthorizedMessage_WhenAuthenticatorIsNotProbablySetup()
        {
            using (var webApp = WebHostTestFactory.Create(services =>
                   {
                       services.AddAuthorizationResponseHandler();
                       services.AddInMemoryDigestAuthenticationNonceTracker();
                       services.AddControllers().AddApplicationPart(typeof(FakeController).Assembly);
                       services
                           .AddAuthentication(DigestAuthorizationHeader.Scheme)
                           .AddDigestAccess(o =>
                           {
                               o.Authenticator = (string username, out string password) =>
                               {
                                   password = "";
                                   return ClaimsPrincipal.Current;
                               };
                           });
                   }, app =>
                   {
                       app.UseAuthentication();

                       app.UseRouting();

                       app.UseAuthorization();

                       app.UseEndpoints(routes => { routes.MapControllers(); });
                   }))
            {
                var options = webApp.ServiceProvider.GetRequiredScopedService<IOptionsSnapshot<DigestAuthenticationOptions>>().Get(DigestAuthorizationHeader.Scheme);
                var client = webApp.Host.GetTestClient();

                var result = await client.GetAsync("/fake");

                Assert.Equal(options.UnauthorizedMessage, await result.Content.ReadAsStringAsync());
                Assert.Equal(StatusCodes.Status401Unauthorized, (int)result.StatusCode);

                var wwwAuthenticate = result.Headers.WwwAuthenticate;

                TestOutput.WriteLine(wwwAuthenticate.ToString());

                result = await client.GetAsync("/fake");

                Assert.Equal(options.UnauthorizedMessage, await result.Content.ReadAsStringAsync());
                Assert.Equal(StatusCodes.Status401Unauthorized, (int)result.StatusCode);
            }
        }

        [Fact]
        public async Task HandleAuthenticateAsync_EnsureAnonymousIsWorking()
        {
            using (var webApp = WebHostTestFactory.Create(services =>
                   {
                       services.AddControllers().AddApplicationPart(typeof(FakeController).Assembly);
                       services
                           .AddAuthentication(DigestAuthorizationHeader.Scheme)
                           .AddBasic(o =>
                           {
                               o.Authenticator = (username, password) => ClaimsPrincipal.Current;
                           });
                   }, app =>
                   {
                       app.UseAuthentication();

                       app.UseRouting();

                       app.UseAuthorization();

                       app.UseEndpoints(routes => { routes.MapControllers(); });
                   }))
            {
                var client = webApp.Host.GetTestClient();

                var result = await client.GetAsync("/fake/anonymous");

                Assert.Equal("Unit Test", await result.Content.ReadAsStringAsync());
                Assert.Equal(StatusCodes.Status200OK, (int)result.StatusCode);
            }
        }
    }
}
