using System;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Cuemon.Extensions.Net.Http;
using Cuemon.Extensions.Xunit;
using Cuemon.Threading;
using Xunit;
using Xunit.Abstractions;

namespace Cuemon.Extensions.Net.Tests.Http
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
            await ParallelFactory.ForAsync(0, expected, i =>
            {
                using (var response = uri.HttpGetAsync().GetAwaiter().GetResult())
                {
                    Interlocked.Increment(ref atomicCount);
                    Assert.Equal(HttpStatusCode.OK, response.StatusCode);
                }
            });
            Assert.Equal(expected, atomicCount);
        }
    }
}