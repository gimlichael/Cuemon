using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Cuemon.AspNetCore.Http.Throttling;
using Cuemon.AspNetCore.Mvc.Assets;
using Cuemon.AspNetCore.Mvc.Filters.Diagnostics;
using Cuemon.Extensions;
using Cuemon.Extensions.AspNetCore.Http.Throttling;
using Cuemon.Extensions.AspNetCore.Mvc.Formatters.Newtonsoft.Json;
using Cuemon.Extensions.Xunit;
using Cuemon.Extensions.Xunit.Hosting.AspNetCore.Mvc;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Xunit;
using Xunit.Abstractions;

namespace Cuemon.AspNetCore.Mvc.Filters.Throttling
{
    public class ThrottlingSentinelFilterTest : Test
    {
        public ThrottlingSentinelFilterTest(ITestOutputHelper output) : base(output)
        {
        }

        [Fact]
        public async Task OnActionExecutionAsync_ShouldCaptureThrottlingException()
        {
            using (var filter = WebApplicationTestFactory.CreateWebApplicationTest(app =>
            {
                app.UseRouting();
                app.UseEndpoints(routes => { routes.MapControllers(); });
            }, services =>
            {
                services.AddControllers(o =>
                    {
                        o.Filters.Add<FaultDescriptorFilter>();
                        o.Filters.Add<ThrottlingSentinelFilter>();
                    }).AddApplicationPart(typeof(FakeController).Assembly)
                    .AddNewtonsoftJson()
                    .AddNewtonsoftJsonFormatters(o => o.IncludeExceptionDescriptorFailure = false);
                services.Configure<ThrottlingSentinelOptions>(o =>
                {
                    o.ContextResolver = context => nameof(OnActionExecutionAsync_ShouldCaptureThrottlingException);
                    o.Quota = new ThrottleQuota(10, 5, TimeUnit.Seconds);
                });
                services.AddMemoryThrottlingCache();
            }))
            {
                var client = filter.Host.GetTestClient();

                HttpResponseMessage result = null;
                for (var i = 0; i < 10; i++)
                {
                    result = await client.GetAsync("/fake/it");
                    Assert.Equal(StatusCodes.Status200OK, (int)result.StatusCode);
                    Assert.Equal("\"Unit Test\"", await result.Content.ReadAsStringAsync());
                }

                result = await client.GetAsync("/fake/it");

                var retryAfter = result.Headers.RetryAfter.Delta.Value.TotalSeconds;
                var ratelimitReset = result.Headers.GetValues("RateLimit-Reset").Single().As<int>();

                Assert.Equal(StatusCodes.Status429TooManyRequests, (int)result.StatusCode);
                Assert.Equal("{\r\n  \"error\": {\r\n    \"status\": 429,\r\n    \"code\": \"TooManyRequests\",\r\n    \"message\": \"Throttling rate limit quota violation. Quota limit exceeded.\"\r\n  }\r\n}", await result.Content.ReadAsStringAsync(), ignoreLineEndingDifferences: true);
                Assert.Contains("Retry-After", result.Headers.Select(pair => pair.Key));
                Assert.Contains("RateLimit-Limit", result.Headers.Select(pair => pair.Key));
                Assert.Contains("RateLimit-Remaining", result.Headers.Select(pair => pair.Key));
                Assert.Contains("RateLimit-Reset", result.Headers.Select(pair => pair.Key));
                Assert.Equal(retryAfter, ratelimitReset);
            }
        }

        [Fact]
        public async Task OnActionExecutionAsync_ShouldThrowThrottlingException()
        {
            using (var filter = WebApplicationTestFactory.CreateWebApplicationTest(app =>
            {
                app.UseRouting();
                app.UseEndpoints(routes => { routes.MapControllers(); });
            }, services =>
            {
                services.AddControllers(o =>
                    {
                        o.Filters.Add<ThrottlingSentinelFilter>();
                    }).AddApplicationPart(typeof(FakeController).Assembly)
                    .AddNewtonsoftJson()
                    .AddNewtonsoftJsonFormatters(o => o.IncludeExceptionDescriptorFailure = false);
                services.Configure<ThrottlingSentinelOptions>(o =>
                {
                    o.ContextResolver = context => nameof(OnActionExecutionAsync_ShouldCaptureThrottlingException);
                    o.Quota = new ThrottleQuota(10, 5, TimeUnit.Seconds);
                });
                services.AddMemoryThrottlingCache();
            }))
            {
                var client = filter.Host.GetTestClient();

                HttpResponseMessage result = null;
                for (var i = 0; i < 10; i++)
                {
                    result = await client.GetAsync("/fake/it");
                    Assert.Equal(StatusCodes.Status200OK, (int)result.StatusCode);
                    Assert.Equal("\"Unit Test\"", await result.Content.ReadAsStringAsync());
                }

                var te = await Assert.ThrowsAsync<ThrottlingException>(async () =>
                {
                    result = await client.GetAsync("/fake/it");
                });

                var options = filter.ServiceProvider.GetRequiredService<IOptions<ThrottlingSentinelOptions>>().Value;

                var ratelimitReset = result.Headers.GetValues("RateLimit-Reset").Single().As<int>();
                Assert.Null(result.Headers.RetryAfter);
                Assert.Equal(StatusCodes.Status200OK, (int)result.StatusCode);
                Assert.Equal(StatusCodes.Status429TooManyRequests, te.StatusCode);
                Assert.Equal(10, te.RateLimit);
                Assert.Equal("\"Unit Test\"", await result.Content.ReadAsStringAsync());
                Assert.Contains("RateLimit-Limit", result.Headers.Select(pair => pair.Key));
                Assert.Contains("RateLimit-Remaining", result.Headers.Select(pair => pair.Key));
                Assert.Contains("RateLimit-Reset", result.Headers.Select(pair => pair.Key));
                Assert.Equal(10, result.Headers.Single(pair => pair.Key == options.RateLimitHeaderName).Value.Single().As<int>());
                Assert.Equal(0, result.Headers.Single(pair => pair.Key == options.RateLimitRemainingHeaderName).Value.Single().As<int>());
                Assert.Equal(4, result.Headers.Single(pair => pair.Key == options.RateLimitResetHeaderName).Value.Single().As<int>());
            }
        }
    }
}
