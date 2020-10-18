using System;
using System.Threading.Tasks;
using Cuemon.AspNetCore;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.Extensions.Options;

namespace Cuemon.Extensions.Xunit.Hosting.AspNetCore.Http.Features
{
    /// <summary>
    /// Provides a fake HTTP response middleware implementation for ASP.NET Core testing.
    /// </summary>
    /// <seealso cref="Middleware" />
    public class FakeHttpResponseMiddleware : ConfigurableMiddleware<FakeHttpResponseOptions>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="FakeHttpResponseMiddleware"/> class.
        /// </summary>
        /// <param name="next">The delegate of the request pipeline to invoke.</param>
        /// <param name="setup">The <see cref="FakeHttpResponseOptions" /> which need to be configured.</param>
        public FakeHttpResponseMiddleware(RequestDelegate next, IOptions<FakeHttpResponseOptions> setup) : base(next, setup)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="FakeHttpResponseMiddleware"/> class.
        /// </summary>
        /// <param name="next">The delegate of the request pipeline to invoke.</param>
        /// <param name="setup">The <see cref="FakeHttpResponseOptions" /> which need to be configured.</param>
        public FakeHttpResponseMiddleware(RequestDelegate next, Action<FakeHttpResponseOptions> setup) : base(next, setup)
        {
        }

        /// <summary>
        /// Executes the <see cref="FakeHttpResponseMiddleware" />.
        /// </summary>
        /// <param name="context">The context of the current request.</param>
        /// <returns>A task that represents the execution of this middleware.</returns>
        public override async Task InvokeAsync(HttpContext context)
        {
            if (context.Features.Get<IHttpResponseFeature>() is FakeHttpResponseFeature feature)
            {
                if (Options.ShortCircuitOnStarting)
                {
                    feature.ShortCircuitOnStarting = Options.ShortCircuitOnStarting;
                    await Next(context).ConfigureAwait(false);
                }
                else
                {
                    await feature.TriggerOnStartingAsync().ConfigureAwait(false);    
                }
                feature.StatusCode = Options.StatusCode;
            }
        }
    }
}