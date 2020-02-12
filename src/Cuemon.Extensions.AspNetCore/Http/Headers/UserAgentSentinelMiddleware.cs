using System;
using System.Threading.Tasks;
using Cuemon.AspNetCore;
using Cuemon.AspNetCore.Http.Headers;
using Cuemon.Extensions.AspNetCore.Infrastructure;
using Cuemon.Extensions.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;

#if NETSTANDARD
using Cuemon.Extensions.IO;
#endif

namespace Cuemon.Extensions.AspNetCore.Http.Headers
{
    /// <summary>
    /// Provides a HTTP User-Agent sentinel middleware implementation for ASP.NET Core.
    /// </summary>
    public class UserAgentSentinelMiddleware : ConfigurableMiddleware<UserAgentSentinelOptions>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="UserAgentSentinelMiddleware"/> class.
        /// </summary>
        /// <param name="next">The delegate of the request pipeline to invoke.</param>
        /// <param name="setup">The <see cref="UserAgentSentinelOptions" /> which need to be configured.</param>
        public UserAgentSentinelMiddleware(RequestDelegate next, IOptions<UserAgentSentinelOptions> setup) : base(next, setup)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="UserAgentSentinelMiddleware"/> class.
        /// </summary>
        /// <param name="next">The delegate of the request pipeline to invoke.</param>
        /// <param name="setup">The <see cref="UserAgentSentinelOptions" /> which need to be configured.</param>
        public UserAgentSentinelMiddleware(RequestDelegate next, Action<UserAgentSentinelOptions> setup) : base(next, setup)
        {
        }

        /// <summary>
        /// Executes the <see cref="UserAgentSentinelMiddleware" />.
        /// </summary>
        /// <param name="context">The context of the current request.</param>
        /// <returns>A task that represents the execution of this middleware.</returns>
        public override async Task InvokeAsync(HttpContext context)
        {
            var exception = false;
            try
            {
                await AspNetCoreInfrastructure.InvokeUserAgentSentinelAsync(context, Options, async (message, response) =>
                {
                    response.StatusCode = (int) message.StatusCode;
                    response.Headers.AddOrUpdateHeaders(message.Headers);
                    await response.Body.WriteAsync(await message.Content.ReadAsByteArrayAsync());
                }).ContinueWithSuppressedContext();
            }
            catch (UserAgentException)
            {
                exception = true;
            }
            if (!exception) { await Next(context).ContinueWithSuppressedContext(); }
        }
    }
}