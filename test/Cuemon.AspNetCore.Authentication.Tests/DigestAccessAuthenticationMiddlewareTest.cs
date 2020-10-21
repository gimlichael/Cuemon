using System.IO;
using System.Security.Claims;
using System.Threading.Tasks;
using Cuemon.Collections.Generic;
using Cuemon.Extensions;
using Cuemon.Extensions.AspNetCore.Authentication;
using Cuemon.Extensions.IO;
using Cuemon.Extensions.Xunit;
using Cuemon.Extensions.Xunit.Hosting.AspNetCore;
using Cuemon.IO;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Microsoft.Net.Http.Headers;
using Xunit;
using Xunit.Abstractions;

namespace Cuemon.AspNetCore.Authentication
{
    public class DigestAccessAuthenticationMiddlewareTest : Test
    {
        public DigestAccessAuthenticationMiddlewareTest(ITestOutputHelper output) : base(output)
        {
        }

        [Fact]
        public async Task InvokeAsync_ShouldNotBeAuthenticated()
        {
            using (var middleware = MiddlewareTestFactory.CreateMiddlewareTest(app =>
            {
                app.UseFakeHttpResponseTrigger(o => o.ShortCircuitOnStarting = true);
                app.UseDigestAccessAuthentication();
            }, services =>
            {
                services.Configure<DigestAccessAuthenticationOptions>(o =>
                {
                    o.Authenticator = (string username, out string password) =>
                    {
                        if (username == "Agent")
                        {
                            password = "Test";
                            var cp = new ClaimsPrincipal();
                            cp.AddIdentity(new ClaimsIdentity(Arguments.Yield(new Claim("Name", "Test Agent"))));
                            return cp;
                        }
                        password = null;
                        return null;
                    };
                    o.Realm = "unittest";
                    o.RequireSecureConnection = false;
                });
                services.AddFakeHttpContextAccessor(ServiceLifetime.Singleton);
                services.AddDigestAccessAuthenticationNonceTracker();
            }))
            {
                var context = middleware.ServiceProvider.GetRequiredService<IHttpContextAccessor>().HttpContext;
                var options = middleware.ServiceProvider.GetRequiredService<IOptions<DigestAccessAuthenticationOptions>>();
                var pipeline = middleware.Application.Build();

                var ue = await Assert.ThrowsAsync<UnauthorizedException>(async () => await pipeline(context));

                Assert.Equal(ue.Message, options.Value.UnauthorizedMessage);
                Assert.Equal(StatusCodes.Status401Unauthorized, context.Response.StatusCode);

                var wwwAuthenticate = context.Response.Headers[HeaderNames.WWWAuthenticate];

                TestOutput.WriteLine(wwwAuthenticate);

                var encodedUsernameAndPassword = "Agent:Test".ToByteArray().ToBase64String();
                context.Request.Headers.Add(HeaderNames.Authorization, $"Digest {encodedUsernameAndPassword}");

                ue = await Assert.ThrowsAsync<UnauthorizedException>(async () => await pipeline(context));

                Assert.Equal(ue.Message, options.Value.UnauthorizedMessage);
                Assert.Equal(StatusCodes.Status401Unauthorized, context.Response.StatusCode);
            }
        }

        [Fact]
        public async Task InvokeAsync_ShouldAuthenticateWhenApplyingAuthorizationHeader()
        {
            using (var middleware = MiddlewareTestFactory.CreateMiddlewareTest(app =>
            {
                app.UseFakeHttpResponseTrigger(o => o.ShortCircuitOnStarting = true);
                app.UseDigestAccessAuthentication();
            }, services =>
            {
                services.Configure<DigestAccessAuthenticationOptions>(o =>
                {
                    o.Authenticator = (string username, out string password) =>
                   {
                       if (username == "Agent")
                       {
                           password = "Test";
                           var cp = new ClaimsPrincipal();
                           cp.AddIdentity(new ClaimsIdentity(Arguments.Yield(new Claim("Name", "Test Agent"))));
                           return cp;
                       }
                       password = null;
                       return null;
                   };
                    o.Realm = "unittest";
                    o.RequireSecureConnection = false;
                });
                services.AddFakeHttpContextAccessor(ServiceLifetime.Singleton);
                services.AddDigestAccessAuthenticationNonceTracker();
            }))
            {
                var context = middleware.ServiceProvider.GetRequiredService<IHttpContextAccessor>().HttpContext;
                var options = middleware.ServiceProvider.GetRequiredService<IOptions<DigestAccessAuthenticationOptions>>();
                var pipeline = middleware.Application.Build();

                var ue = await Assert.ThrowsAsync<UnauthorizedException>(async () => await pipeline(context));

                Assert.Equal(ue.Message, options.Value.UnauthorizedMessage);
                Assert.Equal(StatusCodes.Status401Unauthorized, context.Response.StatusCode);

                var wwwAuthenticate = context.Response.Headers[HeaderNames.WWWAuthenticate];


                TestOutput.WriteLine(wwwAuthenticate);

                var db = new DigestHeaderBuilder(options.Value.Algorithm)
                    .AddUserName("Agent")
                    .AddRealm("unittest")
                    .AddUri("/")
                    .AddNc(1)
                    .AddCnonce()
                    .AddQopAuthentication()
                    .AddFromWwwAuthenticateHeader(context.Response);


                var ha1 = db.ComputeHash1("Test");

                TestOutput.WriteLine(ha1);

                var ha2 = db.ComputeHash2("GET", context.Response.Body.ToEncodedString());
                var response = db.ComputeResponse(ha1, ha2);

                db.AddResponse(response);

                context.Response.Body = new MemoryStream();
                context.Request.Headers.Add(HeaderNames.Authorization, db.ToString());

                await pipeline(context);

                Assert.Equal(StatusCodes.Status200OK, context.Response.StatusCode);
            }
        }

        [Fact]
        public async Task InvokeAsync_ShouldAuthenticateWhenApplyingAuthorizationHeaderNoPlainTextPassword()
        {
            using (var middleware = MiddlewareTestFactory.CreateMiddlewareTest(app =>
            {
                app.UseFakeHttpResponseTrigger(o => o.ShortCircuitOnStarting = true);
                app.UseDigestAccessAuthentication();
            }, services =>
            {
                services.Configure<DigestAccessAuthenticationOptions>(o =>
                {
                    o.Authenticator = (string username, out string password) =>
                   {
                       if (username == "Agent")
                       {
                           password = "a69d6da3eea4fa832dc1c0534863988e550e523f1f786c238951b7ec7abf4d57";
                           var cp = new ClaimsPrincipal();
                           cp.AddIdentity(new ClaimsIdentity(Arguments.Yield(new Claim("Name", "Test Agent"))));
                           return cp;
                       }
                       password = null;
                       return null;
                   };
                    o.Realm = "unittest";
                    o.RequireSecureConnection = false;
                    o.DigestAccessSigner = parameters =>
                    {
                        var db = new DigestHeaderBuilder(parameters.Algorithm, parameters.Credentials);
                        var ha1 = parameters.Password; // password is ha1 stored in some storage
                        var ha2 = db.ComputeHash2(parameters.Method, parameters.EntityBody);
                        return db.ComputeResponse(ha1, ha2);
                    };
                });
                services.AddFakeHttpContextAccessor(ServiceLifetime.Singleton);
                services.AddDigestAccessAuthenticationNonceTracker();
            }))
            {
                var context = middleware.ServiceProvider.GetRequiredService<IHttpContextAccessor>().HttpContext;
                var options = middleware.ServiceProvider.GetRequiredService<IOptions<DigestAccessAuthenticationOptions>>();
                var pipeline = middleware.Application.Build();

                var ue = await Assert.ThrowsAsync<UnauthorizedException>(async () => await pipeline(context));

                Assert.Equal(ue.Message, options.Value.UnauthorizedMessage);
                Assert.Equal(StatusCodes.Status401Unauthorized, context.Response.StatusCode);

                var wwwAuthenticate = context.Response.Headers[HeaderNames.WWWAuthenticate];

                TestOutput.WriteLine(wwwAuthenticate);

                var db = new DigestHeaderBuilder(options.Value.Algorithm)
                    .AddUserName("Agent")
                    .AddRealm("unittest")
                    .AddUri("/")
                    .AddNc(1)
                    .AddCnonce()
                    .AddQopAuthentication()
                    .AddFromWwwAuthenticateHeader(context.Response);


                var ha1 = db.ComputeHash1("Test");

                TestOutput.WriteLine(ha1);

                var ha2 = db.ComputeHash2("GET", context.Response.Body.ToEncodedString());
                var response = db.ComputeResponse(ha1, ha2);

                db.AddResponse(response);

                context.Response.Body = new MemoryStream();
                context.Request.Headers.Add(HeaderNames.Authorization, db.ToString());

                await pipeline(context);

                Assert.Equal(StatusCodes.Status200OK, context.Response.StatusCode);
            }
        }

        [Fact]
        public async Task InvokeAsync_ShouldAuthenticateWhenApplyingAuthorizationHeaderWithQopIntegrity()
        {
            using (var middleware = MiddlewareTestFactory.CreateMiddlewareTest(app =>
            {
                app.UseFakeHttpResponseTrigger(o => o.ShortCircuitOnStarting = true);
                app.UseDigestAccessAuthentication();
            }, services =>
            {
                services.Configure<DigestAccessAuthenticationOptions>(o =>
                {
                    o.Authenticator = (string username, out string password) =>
                   {
                       if (username == "Agent")
                       {
                           password = "Test";
                           var cp = new ClaimsPrincipal();
                           cp.AddIdentity(new ClaimsIdentity(Arguments.Yield(new Claim("Name", "Test Agent"))));
                           return cp;
                       }
                       password = null;
                       return null;
                   };
                    o.Realm = "unittest";
                    o.RequireSecureConnection = false;
                });
                services.AddFakeHttpContextAccessor(ServiceLifetime.Singleton);
                services.AddDigestAccessAuthenticationNonceTracker();
            }))
            {
                var context = middleware.ServiceProvider.GetRequiredService<IHttpContextAccessor>().HttpContext;
                var options = middleware.ServiceProvider.GetRequiredService<IOptions<DigestAccessAuthenticationOptions>>();
                var pipeline = middleware.Application.Build();

                var ue = await Assert.ThrowsAsync<UnauthorizedException>(async () => await pipeline(context));

                Assert.Equal(ue.Message, options.Value.UnauthorizedMessage);
                Assert.Equal(StatusCodes.Status401Unauthorized, context.Response.StatusCode);

                var wwwAuthenticate = context.Response.Headers[HeaderNames.WWWAuthenticate];
                
                TestOutput.WriteLine(wwwAuthenticate);

                var db = new DigestHeaderBuilder(options.Value.Algorithm)
                    .AddUserName("Agent")
                    .AddRealm("unittest")
                    .AddUri("/")
                    .AddNc(1)
                    .AddCnonce()
                    .AddQopAuthenticationIntegrity()
                    .AddFromWwwAuthenticateHeader(context.Response);

                TestOutput.WriteLine("Body:");
                var entityBody = context.Response.Body.ToEncodedString(o => o.LeaveOpen = true);

                var ha1 = db.ComputeHash1("Test");

                TestOutput.WriteLine("HA1:");
                TestOutput.WriteLine(ha1);

                var ha2 = db.ComputeHash2("GET", context.Response.Body.ToEncodedString());
                var response = db.ComputeResponse(ha1, ha2);

                TestOutput.WriteLine("HA2:");
                TestOutput.WriteLine(ha2);

                db.AddResponse(response);

                context.Response.Body = StreamFactory.Create(writer => writer.Write(entityBody));
                context.Request.Headers.Add(HeaderNames.Authorization, db.ToString());

                await pipeline(context);

                Assert.Equal(StatusCodes.Status200OK, context.Response.StatusCode);
            }
        }
    }
}