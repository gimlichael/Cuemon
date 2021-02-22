using System.Security.Claims;
using System.Threading.Tasks;
using Cuemon.AspNetCore.Authentication.Basic;
using Cuemon.Collections.Generic;
using Cuemon.Extensions.AspNetCore.Authentication;
using Cuemon.Extensions.Xunit;
using Cuemon.Extensions.Xunit.Hosting.AspNetCore;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Microsoft.Net.Http.Headers;
using Xunit;
using Xunit.Abstractions;
using Cuemon.Extensions;

namespace Cuemon.AspNetCore.Authentication
{
    public class BasicAuthenticationMiddlewareTest : Test
    {
        public BasicAuthenticationMiddlewareTest(ITestOutputHelper output) : base(output)
        {
        }

        
        [Fact]
        public async Task InvokeAsync_ShouldNotBeAuthenticated()
        {
            using (var middleware = MiddlewareTestFactory.CreateMiddlewareTest(app =>
            {
                app.UseFakeHttpResponseTrigger(o => o.ShortCircuitOnStarting = true);
                app.UseBasicAuthentication();
            }, services =>
            {
                services.Configure<BasicAuthenticationOptions>(o =>
                {
                    o.Authenticator = (username, password) =>
                    {
                        return null;
                    };
                    o.RequireSecureConnection = false;
                });
                services.AddFakeHttpContextAccessor(ServiceLifetime.Singleton);
            }))
            {
                var context = middleware.ServiceProvider.GetRequiredService<IHttpContextAccessor>().HttpContext;
                var options = middleware.ServiceProvider.GetRequiredService<IOptions<BasicAuthenticationOptions>>();
                var pipeline = middleware.Application.Build();
                
                var ue = await Assert.ThrowsAsync<UnauthorizedException>(async () => await pipeline(context));

                Assert.Equal(ue.Message, options.Value.UnauthorizedMessage);
                Assert.Equal(StatusCodes.Status401Unauthorized, context.Response.StatusCode);

                var wwwAuthenticate = context.Response.Headers[HeaderNames.WWWAuthenticate];

                TestOutput.WriteLine(wwwAuthenticate);

                var encodedUsernameAndPassword = "Agent:Test".ToByteArray().ToBase64String();
                context.Request.Headers.Add(HeaderNames.Authorization, $"Basic {encodedUsernameAndPassword}");

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
                app.UseBasicAuthentication();
            }, services =>
            {
                services.Configure<BasicAuthenticationOptions>(o =>
                {
                    o.Authenticator = (username, password) =>
                    {
                        if (username == "Agent" && password == "Test")
                        {
                            var cp = new ClaimsPrincipal();
                            cp.AddIdentity(new ClaimsIdentity(Arguments.Yield(new Claim("Name", "Test Agent"))));
                            return cp;
                        }
                        return null;
                    };
                    o.RequireSecureConnection = false;
                });
                services.AddFakeHttpContextAccessor(ServiceLifetime.Singleton);
            }))
            {
                var context = middleware.ServiceProvider.GetRequiredService<IHttpContextAccessor>().HttpContext;
                var options = middleware.ServiceProvider.GetRequiredService<IOptions<BasicAuthenticationOptions>>();
                var pipeline = middleware.Application.Build();
                
                var ue = await Assert.ThrowsAsync<UnauthorizedException>(async () => await pipeline(context));

                Assert.Equal(ue.Message, options.Value.UnauthorizedMessage);
                Assert.Equal(StatusCodes.Status401Unauthorized, context.Response.StatusCode);

                var wwwAuthenticate = context.Response.Headers[HeaderNames.WWWAuthenticate];

                TestOutput.WriteLine(wwwAuthenticate);

                var encodedUsernameAndPassword = "Agent:Test".ToByteArray().ToBase64String();
                context.Request.Headers.Add(HeaderNames.Authorization, $"Basic {encodedUsernameAndPassword}");

                await pipeline(context);

                Assert.Equal(StatusCodes.Status200OK, context.Response.StatusCode);
            }
        }
    }
}