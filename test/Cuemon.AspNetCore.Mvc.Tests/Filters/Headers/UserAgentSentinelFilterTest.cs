using System.Linq;
using System.Threading.Tasks;
using Cuemon.AspNetCore.Http.Headers;
using Cuemon.AspNetCore.Mvc.Assets;
using Cuemon.Extensions.Xunit;
using Cuemon.Extensions.Xunit.Hosting.AspNetCore.Mvc;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Microsoft.Net.Http.Headers;
using Xunit;
using Xunit.Abstractions;

namespace Cuemon.AspNetCore.Mvc.Filters.Headers
{
    public class UserAgentSentinelFilterTest : Test
    {
        public UserAgentSentinelFilterTest(ITestOutputHelper output) : base(output)
        {
        }

        [Fact]
        public async Task OnActionExecutionAsync_ShouldThrowUserAgentException_BadRequest()
        {
            using (var filter = FilterTestFactory.CreateFilterTest(app =>
            {
                app.UseRouting();
                app.UseEndpoints(routes => { routes.MapControllers(); });
            }, services =>
            {
                services.AddControllers(o => { o.Filters.Add<UserAgentSentinelFilter>(); }).AddApplicationPart(typeof(FakeController).Assembly);
                services.Configure<UserAgentSentinelOptions>(o => { o.RequireUserAgentHeader = true; });
            }))
            {
                var options = filter.ServiceProvider.GetRequiredService<IOptions<UserAgentSentinelOptions>>();
                var client = filter.Host.GetTestClient();

                var uae = await Assert.ThrowsAsync<UserAgentException>(async () =>
                {
                    var result = await client.GetAsync("/fake/it");
                });


                Assert.Equal(uae.Message, options.Value.BadRequestMessage);
                Assert.Equal(uae.StatusCode, StatusCodes.Status400BadRequest);

                Assert.True(options.Value.RequireUserAgentHeader);
            }
        }

        [Fact]
        public async Task OnActionExecutionAsync_ShouldThrowUserAgentException_Forbidden()
        {
            using (var filter = FilterTestFactory.CreateFilterTest(app =>
            {
                app.UseRouting();
                app.UseEndpoints(routes => { routes.MapControllers(); });
            }, services =>
            {
                services.Configure<UserAgentSentinelOptions>(o =>
                {
                    o.RequireUserAgentHeader = true;
                    o.ValidateUserAgentHeader = true;
                    o.AllowedUserAgents.Add("Cuemon-Agent");
                });
                services.AddControllers(o => { o.Filters.Add<UserAgentSentinelFilter>(); }).AddApplicationPart(typeof(FakeController).Assembly);
            }))
            {
                var options = filter.ServiceProvider.GetRequiredService<IOptions<UserAgentSentinelOptions>>();
                var client = filter.Host.GetTestClient();
                client.DefaultRequestHeaders.Add(HeaderNames.UserAgent, "Invalid-Agent");

                var uae = await Assert.ThrowsAsync<UserAgentException>(async () =>
                {
                    var result = await client.GetAsync("/fake/it");
                });


                Assert.Equal(uae.Message, options.Value.ForbiddenMessage);
                Assert.Equal(uae.StatusCode, StatusCodes.Status403Forbidden);

                Assert.True(options.Value.RequireUserAgentHeader);
                Assert.True(options.Value.ValidateUserAgentHeader);
                Assert.True(options.Value.AllowedUserAgents.Any());
            }
        }

        [Fact]
        public async Task OnActionExecutionAsync_ShouldThrowUserAgentException_BadRequest_BecauseOfUseGenericResponse()
        {
            using (var filter = FilterTestFactory.CreateFilterTest(app =>
            {
                app.UseRouting();
                app.UseEndpoints(routes => { routes.MapControllers(); });
            }, services =>
            {
                services.Configure<UserAgentSentinelOptions>(o =>
                {
                    o.RequireUserAgentHeader = true;
                    o.ValidateUserAgentHeader = true;
                    o.UseGenericResponse = true;
                    o.AllowedUserAgents.Add("Cuemon-Agent");
                });
                services.AddControllers(o => { o.Filters.Add<UserAgentSentinelFilter>(); }).AddApplicationPart(typeof(FakeController).Assembly);
            }))
            {
                var options = filter.ServiceProvider.GetRequiredService<IOptions<UserAgentSentinelOptions>>();
                var client = filter.Host.GetTestClient();
                client.DefaultRequestHeaders.Add(HeaderNames.UserAgent, "Invalid-Agent");

                var uae = await Assert.ThrowsAsync<UserAgentException>(async () =>
                {
                    var result = await client.GetAsync("/fake/it");
                });

                Assert.Equal(uae.Message, options.Value.BadRequestMessage);
                Assert.Equal(uae.StatusCode, StatusCodes.Status400BadRequest);

                Assert.True(options.Value.RequireUserAgentHeader);
                Assert.True(options.Value.ValidateUserAgentHeader);
                Assert.True(options.Value.UseGenericResponse);
                Assert.True(options.Value.AllowedUserAgents.Any());
            }
        }

        [Fact]
        public async Task OnActionExecutionAsync_ShouldAllowRequestUnconditional()
        {
            using (var filter = FilterTestFactory.CreateFilterTest(app =>
            {
                app.UseRouting();
                app.UseEndpoints(routes => { routes.MapControllers(); });
            }, services => services.AddControllers(o => { o.Filters.Add<UserAgentSentinelFilter>(); }).AddApplicationPart(typeof(FakeController).Assembly)))
            {
                var options = filter.ServiceProvider.GetRequiredService<IOptions<UserAgentSentinelOptions>>();

                var client = filter.Host.GetTestClient();
                var result = await client.GetAsync("/fake/it");

                Assert.Equal(StatusCodes.Status200OK, (int) result.StatusCode);
                Assert.False(options.Value.RequireUserAgentHeader);
            }
        }

        [Fact]
        public async Task OnActionExecutionAsync_ShouldAllowRequestAfterBeingValidated()
        {
            using (var filter = FilterTestFactory.CreateFilterTest(app =>
            {
                app.UseRouting();
                app.UseEndpoints(routes => { routes.MapControllers(); });
            }, services =>
            {
                services.Configure<UserAgentSentinelOptions>(o =>
                {
                    o.RequireUserAgentHeader = true;
                    o.ValidateUserAgentHeader = true;
                    o.AllowedUserAgents.Add("Cuemon-Agent");
                });
                services.AddControllers(o => { o.Filters.Add<UserAgentSentinelFilter>(); }).AddApplicationPart(typeof(FakeController).Assembly);
            }))
            {
                var options = filter.ServiceProvider.GetRequiredService<IOptions<UserAgentSentinelOptions>>();
                var client = filter.Host.GetTestClient();
                client.DefaultRequestHeaders.Add(HeaderNames.UserAgent, "Cuemon-Agent");
                
                var result = await client.GetAsync("/fake/it");

                Assert.True(options.Value.RequireUserAgentHeader);
                Assert.True(options.Value.ValidateUserAgentHeader);
                Assert.Equal(StatusCodes.Status200OK, (int) result.StatusCode);
            }
        }
    }
}