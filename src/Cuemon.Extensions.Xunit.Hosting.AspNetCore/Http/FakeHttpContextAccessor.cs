using System.IO;
using Cuemon.Extensions.Xunit.Hosting.AspNetCore.Http.Features;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Features;

namespace Cuemon.Extensions.Xunit.Hosting.AspNetCore.Http
{
    /// <summary>
    /// Provides a unit test implementation of <see cref="IHttpContextAccessor"/>.
    /// </summary>
    /// <seealso cref="IHttpContextAccessor" />
    public class FakeHttpContextAccessor : IHttpContextAccessor
    {
        private HttpContext _httpContextCurrent;

        /// <summary>
        /// Initializes a new instance of the <see cref="FakeHttpContextAccessor"/> class.
        /// </summary>
        public FakeHttpContextAccessor()
        {
            var fc = new FeatureCollection();
            fc.Set<IHttpResponseFeature>(new FakeHttpResponseFeature());
            fc.Set<IHttpRequestFeature>(new HttpRequestFeature());
            _httpContextCurrent = new DefaultHttpContext(fc);
            _httpContextCurrent.Response.Body = new MemoryStream();
        }

        /// <summary>
        /// Gets or sets the HTTP context.
        /// </summary>
        /// <value>The HTTP context.</value>
        public HttpContext HttpContext
        {
            get => _httpContextCurrent;
            set => _httpContextCurrent = value;
        }
    }
}