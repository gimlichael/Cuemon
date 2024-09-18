using System;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Codebelt.Extensions.Xunit;
using Cuemon.Threading;
using Xunit;
using Xunit.Abstractions;

namespace Cuemon.Extensions.Net.Http
{
    public class UriExtensionsTest : Test
    {
        public UriExtensionsTest(ITestOutputHelper output) : base(output)
        {
            UriExtensions.DefaultHttpClientFactory = new SlimHttpClientFactory(() => new HttpClientHandler()
            {
                AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate,
                MaxAutomaticRedirections = 10
            }, o => o.HandlerLifetime = TimeSpan.MinValue);
        }

        [Fact]
        public async Task HttpGetAsync_ShouldGetResponseFromUri()
        {
            var uri = new Uri("https://www.cuemon.net/");
            var expected = 125;
            var atomicCount = 0;
            await ParallelFactory.ForAsync(0, expected, async (i, ct) =>
            {
                using (var response = await uri.HttpGetAsync(ct))
                {
                    Interlocked.Increment(ref atomicCount);
                    Assert.Equal(HttpStatusCode.OK, response.StatusCode);
                }
            });
            Assert.Equal(expected, atomicCount);
        }
    }
}