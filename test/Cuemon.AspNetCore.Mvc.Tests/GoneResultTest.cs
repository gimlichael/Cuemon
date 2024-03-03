using System.Threading.Tasks;
using Cuemon.Extensions.Xunit;
using Cuemon.Extensions.Xunit.Hosting.AspNetCore;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Abstractions;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.DependencyInjection;
using Xunit;
using Xunit.Abstractions;

namespace Cuemon.AspNetCore.Mvc
{
    public class GoneResultTest : Test
    {
        public GoneResultTest(ITestOutputHelper output) : base(output)
        {
        }

        [Fact]
        public async Task ExecuteResultAsync_ShouldReturnStatusCode303AndLocationUri()
        {
            using (var host = MiddlewareTestFactory.Create())
            {
                var context = host.ServiceProvider.GetRequiredService<IHttpContextAccessor>().HttpContext;
                var sut = new GoneResult();
                var ac = new ActionContext(context, new RouteData(), new ActionDescriptor());

                Assert.Equal(StatusCodes.Status410Gone, sut.StatusCode);

                await sut.ExecuteResultAsync(ac);

                Assert.Equal(sut.StatusCode, context.Response.StatusCode);
            }

        }
    }
}
