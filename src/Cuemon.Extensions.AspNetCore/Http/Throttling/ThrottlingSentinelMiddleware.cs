using System;
using System.Threading.Tasks;
using Cuemon.AspNetCore;
using Cuemon.AspNetCore.Infrastructure;
using Cuemon.Extensions.AspNetCore.Infrastructure;
using Cuemon.Extensions.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;

#if NETSTANDARD
using Cuemon.Extensions.IO;
#endif

namespace Cuemon.Extensions.AspNetCore.Http.Throttling
{
    /// <summary>
    /// Provides an API throttling middleware implementation for ASP.NET Core.
    /// </summary>
    public class ThrottlingSentinelMiddleware : ConfigurableMiddleware<IThrottlingCache, ThrottlingSentinelOptions>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ThrottlingSentinelMiddleware"/> class.
        /// </summary>
        /// <param name="next">The delegate of the request pipeline to invoke.</param>
        /// <param name="setup">The <see cref="ThrottlingSentinelOptions" /> which need to be configured.</param>
        public ThrottlingSentinelMiddleware(RequestDelegate next, IOptions<ThrottlingSentinelOptions> setup) : base(next, setup)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ThrottlingSentinelMiddleware"/> class.
        /// </summary>
        /// <param name="next">The delegate of the request pipeline to invoke.</param>
        /// <param name="setup">The <see cref="ThrottlingSentinelOptions" /> which need to be configured.</param>
        public ThrottlingSentinelMiddleware(RequestDelegate next, Action<ThrottlingSentinelOptions> setup) : base(next, setup)
        {
        }

        /// <summary>
        /// Executes the <see cref="ThrottlingSentinelMiddleware" />.
        /// </summary>
        /// <param name="context">The context of the current request.</param>
        /// <param name="tc">The dependency injected <see cref="IThrottlingCache"/> of <see cref="InvokeAsync"/>.</param>
        /// <returns>A task that represents the execution of this middleware.</returns>
        public override async Task InvokeAsync(HttpContext context, IThrottlingCache tc)
        {
            var exception = false;
            try
            {
                await AspNetCoreInfrastructure.InvokeThrottlerSentinelAsync(context, tc, Options, async (message, response) =>
                {
                    response.StatusCode = (int) message.StatusCode;
                    response.Headers.AddOrUpdateHeaders(message.Headers);
                    await response.Body.WriteAsync(await message.Content.ReadAsByteArrayAsync());
                });
            }
            catch (ThrottlingException)
            {
                exception = true;
            }
            if (!exception) { await Next(context).ContinueWithSuppressedContext(); }
        }
    }
}