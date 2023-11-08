using System.Linq;
using System.Threading.Tasks;
using Cuemon.AspNetCore.Http.Headers;
using Cuemon.AspNetCore.Mvc.Assets;
using Cuemon.AspNetCore.Mvc.Filters.Diagnostics;
using Cuemon.Extensions.AspNetCore.Mvc.Formatters.Newtonsoft.Json;
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
    public class ApiKeySentinelFilterTest : Test
    {
        public ApiKeySentinelFilterTest(ITestOutputHelper output) : base(output)
        {
        }

        [Fact]
        public async Task OnActionExecutionAsync_ShouldCaptureApiKeyException_BadRequest()
        {
            using (var filter = WebApplicationTestFactory.Create(services =>
                   {
                       services.AddControllers(o =>
                       {
                           o.Filters.Add<FaultDescriptorFilter>();
                           o.Filters.Add<ApiKeySentinelFilter>();
                       }).AddApplicationPart(typeof(FakeController).Assembly)
                           .AddNewtonsoftJson()
                           .AddNewtonsoftJsonFormatters();
                   }, app =>
                   {
                       app.UseRouting();
                       app.UseEndpoints(routes => { routes.MapControllers(); });
                   }))
            {
                var options = filter.ServiceProvider.GetRequiredService<IOptions<ApiKeySentinelOptions>>();
                var client = filter.Host.GetTestClient();

                var result = await client.GetAsync("/fake/it");

                Assert.Contains(options.Value.BadRequestMessage, await result.Content.ReadAsStringAsync());
                Assert.Equal((int)result.StatusCode, StatusCodes.Status400BadRequest);
            }
        }

        [Fact]
        public async Task OnActionExecutionAsync_ShouldCaptureApiKeyException_Forbidden()
        {
            using (var filter = WebApplicationTestFactory.Create(services =>
                   {
                       services.Configure<ApiKeySentinelOptions>(o =>
                       {
                           o.AllowedKeys.Add("Cuemon-Key");
                       });
                       services.AddControllers(o =>
                           {
                               o.Filters.Add<FaultDescriptorFilter>();
                               o.Filters.Add<ApiKeySentinelFilter>();
                           }).AddApplicationPart(typeof(FakeController).Assembly)
                           .AddNewtonsoftJson()
                           .AddNewtonsoftJsonFormatters();
                   }, app =>
                   {
                       app.UseRouting();
                       app.UseEndpoints(routes => { routes.MapControllers(); });
                   }))
            {
                var options = filter.ServiceProvider.GetRequiredService<IOptions<ApiKeySentinelOptions>>();
                var client = filter.Host.GetTestClient();
                client.DefaultRequestHeaders.Add(options.Value.HeaderName, "Invalid-Key");

                var result = await client.GetAsync("/fake/it");

                Assert.Contains(options.Value.ForbiddenMessage, await result.Content.ReadAsStringAsync());
                Assert.Equal(StatusCodes.Status403Forbidden, (int)result.StatusCode);

                Assert.True(options.Value.AllowedKeys.Any());
            }
        }

        [Fact]
        public async Task OnActionExecutionAsync_ShouldThrowApiKeyException_BadRequest()
        {
            using (var filter = WebApplicationTestFactory.Create(services =>
            {
                services.AddControllers(o => { o.Filters.Add<ApiKeySentinelFilter>(); }).AddApplicationPart(typeof(FakeController).Assembly);
            }, app =>
                   {
                       app.UseRouting();
                       app.UseEndpoints(routes => { routes.MapControllers(); });
                   }))
            {
                var options = filter.ServiceProvider.GetRequiredService<IOptions<ApiKeySentinelOptions>>();
                var client = filter.Host.GetTestClient();

                var uae = await Assert.ThrowsAsync<ApiKeyException>(async () =>
                {
                    var result = await client.GetAsync("/fake/it");
                });


                Assert.Equal(uae.Message, options.Value.BadRequestMessage);
                Assert.Equal(uae.StatusCode, StatusCodes.Status400BadRequest);
            }
        }

        [Fact]
        public async Task OnActionExecutionAsync_ShouldThrowApiKeyException_Forbidden()
        {
            using (var filter = WebApplicationTestFactory.Create(services =>
            {
                services.Configure<ApiKeySentinelOptions>(o =>
                {
                    o.AllowedKeys.Add("Cuemon-Key");
                });
                services.AddControllers(o => { o.Filters.Add<ApiKeySentinelFilter>(); }).AddApplicationPart(typeof(FakeController).Assembly);
            }, app =>
                   {
                       app.UseRouting();
                       app.UseEndpoints(routes => { routes.MapControllers(); });
                   }))
            {
                var options = filter.ServiceProvider.GetRequiredService<IOptions<ApiKeySentinelOptions>>();
                var client = filter.Host.GetTestClient();
                client.DefaultRequestHeaders.Add(options.Value.HeaderName, "Invalid-Key");

                var uae = await Assert.ThrowsAsync<ApiKeyException>(async () =>
                {
                    var result = await client.GetAsync("/fake/it");
                });


                Assert.Equal(uae.Message, options.Value.ForbiddenMessage);
                Assert.Equal(uae.StatusCode, StatusCodes.Status403Forbidden);

                Assert.True(options.Value.AllowedKeys.Any());
            }
        }

        [Fact]
        public async Task OnActionExecutionAsync_ShouldThrowApiKeyException_BadRequest_BecauseOfUseGenericResponse()
        {
            using (var filter = WebApplicationTestFactory.Create(services =>
            {
                services.Configure<ApiKeySentinelOptions>(o =>
                {
                    o.UseGenericResponse = true;
                    o.AllowedKeys.Add("Cuemon-Key");
                });
                services.AddControllers(o => { o.Filters.Add<ApiKeySentinelFilter>(); }).AddApplicationPart(typeof(FakeController).Assembly);
            }, app =>
                   {
                       app.UseRouting();
                       app.UseEndpoints(routes => { routes.MapControllers(); });
                   }))
            {
                var options = filter.ServiceProvider.GetRequiredService<IOptions<ApiKeySentinelOptions>>();
                var client = filter.Host.GetTestClient();
                client.DefaultRequestHeaders.Add(HeaderNames.UserAgent, "Invalid-Key");

                var uae = await Assert.ThrowsAsync<ApiKeyException>(async () =>
                {
                    var result = await client.GetAsync("/fake/it");
                });

                Assert.Equal(uae.Message, options.Value.BadRequestMessage);
                Assert.Equal(uae.StatusCode, StatusCodes.Status400BadRequest);

                Assert.True(options.Value.UseGenericResponse);
                Assert.True(options.Value.AllowedKeys.Any());
            }
        }

        [Fact]
        public async Task OnActionExecutionAsync_ShouldAllowRequestAfterBeingValidated()
        {
            using (var filter = WebApplicationTestFactory.Create(services =>
            {
                services.Configure<ApiKeySentinelOptions>(o =>
                {
                    o.AllowedKeys.Add("Cuemon-Key");
                });
                services.AddControllers(o => { o.Filters.Add<ApiKeySentinelFilter>(); }).AddApplicationPart(typeof(FakeController).Assembly);
            }, app =>
                   {
                       app.UseRouting();
                       app.UseEndpoints(routes => { routes.MapControllers(); });
                   }))
            {
                var options = filter.ServiceProvider.GetRequiredService<IOptions<ApiKeySentinelOptions>>();
                var client = filter.Host.GetTestClient();
                client.DefaultRequestHeaders.Add(options.Value.HeaderName, "Cuemon-Key");
                
                var result = await client.GetAsync("/fake/it");

                Assert.Equal(StatusCodes.Status200OK, (int) result.StatusCode);
            }
        }
    }
}