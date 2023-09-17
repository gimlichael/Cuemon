using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.Net.Http.Headers;

namespace Cuemon.Extensions.Xunit.Hosting.AspNetCore.Http.Features
{
    /// <summary>
    /// Represents a way to trigger <see cref="HttpResponseFeature.OnStarting"/>.
    /// </summary>
    /// <seealso cref="HttpResponseFeature" />
    public class FakeHttpResponseFeature : HttpResponseFeature
    {
        private bool _hasStarted;

        /// <summary>
        /// Initializes a new instance of the <see cref="FakeHttpResponseFeature"/> class.
        /// </summary>
        public FakeHttpResponseFeature()
        {
            Headers.Append(HeaderNames.Date, DateTime.UtcNow.ToString("R"));
        }

        /// <summary>
        /// Registers a callback to be invoked just before the response starts. This is the last chance to modify the <see cref="P:Microsoft.AspNetCore.Http.Features.IHttpResponseFeature.Headers" />, <see cref="P:Microsoft.AspNetCore.Http.Features.IHttpResponseFeature.StatusCode" />, or <see cref="P:Microsoft.AspNetCore.Http.Features.IHttpResponseFeature.ReasonPhrase" />.
        /// </summary>
        /// <param name="callback">The callback to invoke when starting the response.</param>
        /// <param name="state">The state to pass into the callback.</param>
        public override void OnStarting(Func<object, Task> callback, object state)
        {
            if (_hasStarted) { return; }
            _hasStarted = true;
            callback?.Invoke(state);
        }

        /// <summary>
        /// Indicates if the response has started. If true, the <see cref="P:Microsoft.AspNetCore.Http.Features.IHttpResponseFeature.StatusCode" />,
        /// <see cref="P:Microsoft.AspNetCore.Http.Features.IHttpResponseFeature.ReasonPhrase" />, and <see cref="P:Microsoft.AspNetCore.Http.Features.IHttpResponseFeature.Headers" /> are now immutable, and
        /// OnStarting should no longer be called.
        /// </summary>
        /// <value><c>true</c> if this instance has started; otherwise, <c>false</c>.</value>
        public override bool HasStarted => _hasStarted;
    }
}