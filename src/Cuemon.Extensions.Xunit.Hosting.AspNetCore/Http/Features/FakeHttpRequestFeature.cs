using System.Net.Http;
using Microsoft.AspNetCore.Http.Features;

namespace Cuemon.Extensions.Xunit.Hosting.AspNetCore.Http.Features
{
    /// <summary>
    /// Represents a way to support some default values for Request context..
    /// </summary>
    /// <seealso cref="HttpRequestFeature" />
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