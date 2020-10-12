using System;
using System.Threading.Tasks;
using Cuemon.Extensions.AspNetCore.Builder;
using Cuemon.Extensions.AspNetCore.Http.Throttling;
using Cuemon.Extensions.IO;
using Cuemon.Extensions.Xunit;
using Cuemon.Extensions.Xunit.Hosting.AspNetCore;
using Cuemon.Extensions.Xunit.Hosting.AspNetCore.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Xunit;
using Xunit.Abstractions;

namespace Cuemon.AspNetCore.Http.Throttling
{
    public class ThrottlingSentinelMiddlewareTest : Test
    {
        public ThrottlingSentinelMiddlewareTest(ITestOutputHelper output) : base(output)
        {
        }

        [Fact]
        public async Task InvokeAsync_ShouldThrowThrottlingException_TooManyRequests()
        {
            var middleware = MiddlewareTestFactory.CreateMiddlewareTest(app =>
            {
                app.UseThrottlingSentinel();
                app.UseFakeHttpResponseTrigger();
            }, services =>
            {
                services.Configure<ThrottlingSentinelOptions>(o =>
                {
                    o.Quota = new ThrottleQuota(10, TimeSpan.FromMinutes(5));
                    o.ContextResolver = cr => nameof(ThrottlingSentinelMiddlewareTest);
                });
                services.AddSingleton<IHttpContextAccessor, FakeHttpContextAccessor>();
                services.AddMemoryThrottlingCache();
            });

            var context = middleware.ServiceProvider.GetRequiredService<IHttpContextAccessor>().HttpContext;
            var options = middleware.ServiceProvider.GetRequiredService<IOptions<ThrottlingSentinelOptions>>();
            var cache = middleware.ServiceProvider.GetRequiredService<IThrottlingCache>();
            var pipeline = middleware.Application.Build();

            var te = await Assert.ThrowsAsync<ThrottlingException>(async () =>
            {
                for (var i = 0; i < 15; i++)
                {
                    await pipeline(context);
                }
            });

            var ce = cache[nameof(ThrottlingSentinelMiddlewareTest)];
            Assert.InRange(ce.Total, te.RateLimit, 15);

            Assert.Equal(te.RateLimit, options.Value.Quota.RateLimit);
            Assert.Equal(te.Message, options.Value.TooManyRequestsMessage);
            Assert.Equal(te.StatusCode, StatusCodes.Status429TooManyRequests);

            Assert.True(options.Value.UseRetryAfterHeader);
            Assert.Equal(StatusCodes.Status429TooManyRequests, context.Response.StatusCode);
            Assert.Equal(options.Value.TooManyRequestsMessage, context.Response.Body.ToEncodedString());
        }

        [Fact]
        public async Task InvokeAsync_ShouldRehydrate()
        {
            var middleware = MiddlewareTestFactory.CreateMiddlewareTest(app =>
            {
                app.UseThrottlingSentinel();
                app.UseFakeHttpResponseTrigger();
            }, services =>
            {
                services.Configure<ThrottlingSentinelOptions>(o =>
                {
                    o.Quota = new ThrottleQuota(10, TimeSpan.FromSeconds(5));
                    o.ContextResolver = cr => nameof(ThrottlingSentinelMiddlewareTest);
                });
                services.AddSingleton<IHttpContextAccessor, FakeHttpContextAccessor>();
                services.AddMemoryThrottlingCache();
            });

            var context = middleware.ServiceProvider.GetRequiredService<IHttpContextAccessor>().HttpContext;
            var options = middleware.ServiceProvider.GetRequiredService<IOptions<ThrottlingSentinelOptions>>();
            var cache = middleware.ServiceProvider.GetRequiredService<IThrottlingCache>();
            var pipeline = middleware.Application.Build();

            for (var i = 0; i < 10; i++)
            {
                await pipeline(context);
            }

            var te = await Assert.ThrowsAsync<ThrottlingException>(async () => await pipeline(context));

            TestOutput.WriteLine(te.Delta.ToString());

            await Task.Delay(te.Delta);

            await pipeline(context);

            var ce = cache[nameof(ThrottlingSentinelMiddlewareTest)];


            Assert.True(options.Value.UseRetryAfterHeader);
            Assert.Equal(StatusCodes.Status200OK, context.Response.StatusCode);
        }
    }
}