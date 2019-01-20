using System;
using System.Linq;
using System.Threading.Tasks;
using Cuemon.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using Microsoft.Net.Http.Headers;

namespace Cuemon.AspNetCore.Http.Headers
{
    /// <summary>
    /// Provides a HTTP User-Agent parser middleware implementation for ASP.NET Core.
    /// </summary>
    public class UserAgentParserMiddleware : ConfigurableMiddleware<UserAgentParserOptions>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="UserAgentParserMiddleware"/> class.
        /// </summary>
        /// <param name="next">The delegate of the request pipeline to invoke.</param>
        /// <param name="setup">The <see cref="UserAgentParserOptions" /> which need to be configured.</param>
        public UserAgentParserMiddleware(RequestDelegate next, IOptions<UserAgentParserOptions> setup) : base(next, setup)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="UserAgentParserMiddleware"/> class.
        /// </summary>
        /// <param name="next">The delegate of the request pipeline to invoke.</param>
        /// <param name="setup">The <see cref="UserAgentParserOptions" /> which need to be configured.</param>
        public UserAgentParserMiddleware(RequestDelegate next, Action<UserAgentParserOptions> setup) : base(next, setup)
        {
        }

        /// <summary>
        /// Executes the <see cref="UserAgentParserMiddleware" />.
        /// </summary>
        /// <param name="context">The context of the current request.</param>
        /// <returns>A task that represents the execution of this middleware.</returns>
        public override async Task InvokeAsync(HttpContext context)
        {
            var userAgent = context.Request.Headers[HeaderNames.UserAgent].FirstOrDefault();
            if (Options.RequireUserAgentHeader)
            {
                var requirementsFailed = userAgent.IsNullOrWhiteSpace();
                if (!requirementsFailed && Options.ValidateUserAgentHeader && Options.AllowedUserAgents.Count > 0)
                {
                    requirementsFailed |= !Options.AllowedUserAgents.Any(allowedUserAgent => userAgent.Equals(allowedUserAgent, StringComparison.OrdinalIgnoreCase));
                }

                if (requirementsFailed)
                {
                    context.Response.StatusCode = StatusCodes.Status400BadRequest;
                    await context.Response.WriteBodyAsync(Options.RequirementsFailedBody);
                    return;
                }
            }
            await Next(context).ContinueWithSuppressedContext();
        }
    }
}