using Cuemon.Extensions.Xunit.Hosting.AspNetCore.Http.Features;
using Cuemon.IO;
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

        /// <summary>
        /// Initializes a new instance of the <see cref="FakeHttpContextAccessor"/> class.
        /// </summary>
        public FakeHttpContextAccessor()
        {
            var fc = new FeatureCollection();
            fc.Set<IHttpResponseFeature>(new FakeHttpResponseFeature());
            fc.Set<IHttpRequestFeature>(new FakeHttpRequestFeature());
            fc.Set<IHttpResponseBodyFeature>(new StreamResponseBodyFeature(StreamFactory.Create(writer => writer.Write("Hello awesome developers!"))));
            HttpContext = new DefaultHttpContext(fc);
        }

        /// <summary>
        /// Gets or sets the HTTP context.
        /// </summary>
        /// <value>The HTTP context.</value>
        public HttpContext HttpContext { get; set; }
    }
}
