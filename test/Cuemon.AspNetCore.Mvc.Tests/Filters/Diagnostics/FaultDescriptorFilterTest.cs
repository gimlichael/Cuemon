using System.Threading.Tasks;
using Cuemon.AspNetCore.Mvc.Assets;
using Cuemon.Extensions.Xunit;
using Cuemon.Extensions.Xunit.Hosting.AspNetCore.Mvc;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using Xunit;
using Xunit.Abstractions;

namespace Cuemon.AspNetCore.Mvc.Filters.Diagnostics
{
    public class FaultDescriptorFilterTest : Test
    {
        public FaultDescriptorFilterTest(ITestOutputHelper output) : base(output)
        {
        }

        [Fact]
        public async Task GetBadRequest_ShouldReturnBadRequest()
        {
            using (var filter = MvcFilterTestFactory.CreateMvcFilterTest(app =>
            {
                app.UseRouting();
                app.UseEndpoints(routes => { routes.MapControllers(); });
            }, services =>
            {
                services.AddControllers(o => { o.Filters.Add<FaultDescriptorFilter>(); }).AddApplicationPart(typeof(FakeController).Assembly).AddJsonOptions(o =>
                {
                    o.JsonSerializerOptions.MaxDepth = 0;
                    o.JsonSerializerOptions.IgnoreNullValues = true;
                    o.JsonSerializerOptions.IgnoreReadOnlyProperties = true;
                });
            }))
            {
                var client = filter.Host.GetTestClient();
                var result = await client.GetAsync("/fake/getResponse400");

                Assert.Equal(StatusCodes.Status400BadRequest, (int) result.StatusCode);
            }
        }
    }
}