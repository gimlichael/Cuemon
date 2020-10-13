using System.Threading.Tasks;
using Cuemon.AspNetCore;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Features;

namespace Cuemon.Extensions.Xunit.Hosting.AspNetCore.Http.Features
{
    /// <summary>
    /// Provides a fake HTTP response middleware implementation for ASP.NET Core testing.
    /// </summary>
    /// <seealso cref="Middleware" />
    public class FakeHttpResponseMiddleware : Middleware
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="FakeHttpResponseMiddleware"/> class.
        /// </summary>
        /// <param name="next">The delegate of the request pipeline to invoke.</param>
        public FakeHttpResponseMiddleware(RequestDelegate next) : base(next)
        {
        }

        /// <summary>
        /// Executes the <see cref="FakeHttpResponseMiddleware" />.
        /// </summary>
        /// <param name="context">The context of the current request.</param>
        /// <returns>A task that represents the execution of this middleware.</returns>
        public override Task InvokeAsync(HttpContext context)
        {
            var feature = context.Features.Get<IHttpResponseFeature>() as FakeHttpResponseFeature;
            return feature?.TriggerOnStarting();
        }
    }
}