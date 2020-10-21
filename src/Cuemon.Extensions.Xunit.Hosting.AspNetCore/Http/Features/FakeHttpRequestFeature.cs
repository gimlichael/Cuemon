using System.Net.Http;
using Microsoft.AspNetCore.Http.Features;

namespace Cuemon.Extensions.Xunit.Hosting.AspNetCore.Http.Features
{
    public class FakeHttpRequestFeature : HttpRequestFeature
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="FakeHttpRequestFeature"/> class.
        /// </summary>
        public FakeHttpRequestFeature()
        {
            Method = HttpMethod.Get.ToString();
            Path = "/";
            Scheme = "http";
            Protocol = "HTTP/1.1";
        }
    }
}