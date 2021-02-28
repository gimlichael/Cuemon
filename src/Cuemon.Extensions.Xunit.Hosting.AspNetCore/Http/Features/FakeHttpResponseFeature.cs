using System;
using System.Threading.Tasks;
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
        private Func<object, Task> _callback;
        private object _state;

        /// <summary>
        /// Initializes a new instance of the <see cref="FakeHttpResponseFeature"/> class.
        /// </summary>
        public FakeHttpResponseFeature()
        {
            Headers.Add(HeaderNames.Date, DateTime.UtcNow.ToString("R"));
        }

        /// <summary>
        /// Registers a callback to be invoked just before the response starts. This is the
        /// last chance to modify the <see cref="P:Microsoft.AspNetCore.Http.Features.IHttpResponseFeature.Headers" />, <see cref="P:Microsoft.AspNetCore.Http.Features.IHttpResponseFeature.StatusCode" />, or
        /// <see cref="P:Microsoft.AspNetCore.Http.Features.IHttpResponseFeature.ReasonPhrase" />.
        /// </summary>
        /// <param name="callback">The callback to invoke when starting the response.</param>
        /// <param name="state">The state to pass into the callback.</param>
        public override void OnStarting(Func<object, Task> callback, object state)
        {
            if (_hasStarted) { return; }
            if (ShortCircuitOnStarting)
            {
                _hasStarted = true;
                callback?.Invoke(state);
            }
            _callback = callback;
            _state = state;
        }

        /// <summary>
        /// Gets a value indicating whether this instance has <see cref="OnStarting"/> callback.
        /// </summary>
        /// <value><c>true</c> if this instance has <see cref="OnStarting"/> callback; otherwise, <c>false</c>.</value>
        public bool HasOnStartingCallback => _callback != null;

        /// <summary>
        /// Indicates if the response has started. If true, the <see cref="P:Microsoft.AspNetCore.Http.Features.IHttpResponseFeature.StatusCode" />,
        /// <see cref="P:Microsoft.AspNetCore.Http.Features.IHttpResponseFeature.ReasonPhrase" />, and <see cref="P:Microsoft.AspNetCore.Http.Features.IHttpResponseFeature.Headers" /> are now immutable, and
        /// OnStarting should no longer be called.
        /// </summary>
        /// <value><c>true</c> if this instance has started; otherwise, <c>false</c>.</value>
        public override bool HasStarted => _hasStarted;

        /// <summary>
        /// Gets or sets a value indicating whether <see cref="OnStarting"/> is invoked immediately upon initialization.
        /// </summary>
        /// <value><c>true</c> if <see cref="OnStarting"/> is invoked immediately upon initialization; otherwise, <c>false</c>.</value>
        public bool ShortCircuitOnStarting { get; set; }

        /// <summary>
        /// Executes the function delegate assigned by <see cref="OnStarting"/>.
        /// </summary>
        /// <returns>A task that represents the asynchronous operation.</returns>
        public Task TriggerOnStartingAsync()
        {
            _hasStarted = true;
            return HasOnStartingCallback ? _callback(_state) : Task.CompletedTask;
        }
    }
}