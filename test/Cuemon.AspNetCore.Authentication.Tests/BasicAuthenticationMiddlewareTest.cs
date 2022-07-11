using System;
using System.Security.Claims;
using System.Threading.Tasks;
using Cuemon.AspNetCore.Authentication.Basic;
using Cuemon.AspNetCore.Http;
using Cuemon.Collections.Generic;
using Cuemon.Extensions.AspNetCore.Authentication;
using Cuemon.Extensions.AspNetCore.Diagnostics;
using Cuemon.Extensions.IO;
using Cuemon.Extensions.Xunit;
using Cuemon.Extensions.Xunit.Hosting.AspNetCore;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Microsoft.Net.Http.Headers;
using Xunit;
using Xunit.Abstractions;

namespace Cuemon.AspNetCore.Authentication
{
    public class BasicAuthenticationMiddlewareTest : Test
    {
        public BasicAuthenticationMiddlewareTest(ITestOutputHelper output) : base(output)
        {
        }

        [Fact]
        public async Task InvokeAsync_ShouldThrowUnauthorizedException_InvalidCredentials()
        {
            using (var middleware = MiddlewareTestFactory.Create(app =>
            {
                app.UseBasicAuthentication();
            }, services =>
            {
                services.Configure<BasicAuthenticationOptions>(o =>
                {
                    o.Authenticator = (username, password) => null;
                    o.RequireSecureConnection = false;
                });
            }))
            {
                var context = middleware.ServiceProvider.GetRequiredService<IHttpContextAccessor>().HttpContext;
                var options = middleware.ServiceProvider.GetRequiredService<IOptions<BasicAuthenticationOptions>>();
                var pipeline = middleware.Application.Build();

                var ue = await Assert.ThrowsAsync<UnauthorizedException>(async () => await pipeline(context));

                Assert.Equal(options.Value.UnauthorizedMessage, ue.Message);
                Assert.Equal(StatusCodes.Status401Unauthorized, ue.StatusCode);

                var wwwAuthenticate = context.Response.Headers[HeaderNames.WWWAuthenticate];
                
                TestOutput.WriteLine(wwwAuthenticate);

                var bb = new BasicAuthorizationHeaderBuilder()
                    .AddUserName("Agent")
                    .AddPassword("Test");

                context.Request.Headers.Add(HeaderNames.Authorization, bb.Build().ToString());

                ue = await Assert.ThrowsAsync<UnauthorizedException>(async () => await pipeline(context));

                Assert.Equal(options.Value.UnauthorizedMessage, ue.Message);
                Assert.Equal(StatusCodes.Status401Unauthorized, ue.StatusCode);
            }
        }

        [Fact]
        public async Task InvokeAsync_ShouldCaptureUnauthorizedException_InvalidCredentials()
        {
            using (var middleware = MiddlewareTestFactory.Create(app =>
                   {
                       app.UseFaultDescriptorExceptionHandler();
                       app.UseBasicAuthentication();
                   }, services =>
                   {
                       services.Configure<BasicAuthenticationOptions>(o =>
                       {
                           o.Authenticator = (username, password) => null;
                           o.RequireSecureConnection = false;
                       });
                   }))
            {
                var context = middleware.ServiceProvider.GetRequiredService<IHttpContextAccessor>().HttpContext;
                var options = middleware.ServiceProvider.GetRequiredService<IOptions<BasicAuthenticationOptions>>();
                var pipeline = middleware.Application.Build();

                var bb = new BasicAuthorizationHeaderBuilder()
                    .AddUserName("Agent")
                    .AddPassword("Test");

                context.Request.Headers.Add(HeaderNames.Authorization, bb.Build().ToString());

                await pipeline(context);

                Assert.EndsWith(options.Value.UnauthorizedMessage, context.Response.Body.ToEncodedString());
                Assert.Equal(StatusCodes.Status401Unauthorized, context.Response.StatusCode);

                var wwwAuthenticate = context.Response.Headers[HeaderNames.WWWAuthenticate];

                TestOutput.WriteLine(wwwAuthenticate);
            }
        }

        [Fact]
        public async Task InvokeAsync_ShouldAuthenticateWhenApplyingAuthorizationHeader()
        {
            using (var middleware = MiddlewareTestFactory.Create(app =>
            {
                app.UseBasicAuthentication();
                app.Run(context =>
                {
                    context.Response.StatusCode = 200;
                    return Task.CompletedTask;
                });
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
            }))
            {
                var context = middleware.ServiceProvider.GetRequiredService<IHttpContextAccessor>().HttpContext;
                var options = middleware.ServiceProvider.GetRequiredService<IOptions<BasicAuthenticationOptions>>();
                var pipeline = middleware.Application.Build();

                var ue = await Assert.ThrowsAsync<UnauthorizedException>(async () => await pipeline(context));

                Assert.Equal(options.Value.UnauthorizedMessage, ue.Message);
                Assert.Equal(StatusCodes.Status401Unauthorized, ue.StatusCode);

                var wwwAuthenticate = context.Response.Headers[HeaderNames.WWWAuthenticate];

                TestOutput.WriteLine(wwwAuthenticate);

                var bb = new BasicAuthorizationHeaderBuilder()
                    .AddUserName("Agent")
                    .AddPassword("Test");

                context.Request.Headers.Add(HeaderNames.Authorization, bb.Build().ToString());

                await pipeline(context);

                Assert.Equal(StatusCodes.Status200OK, context.Response.StatusCode);
            }
        }
    }
}