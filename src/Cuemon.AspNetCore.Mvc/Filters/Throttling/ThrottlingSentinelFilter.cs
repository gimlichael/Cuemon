using System.Threading.Tasks;
using Cuemon.AspNetCore.Http.Headers;
using Cuemon.AspNetCore.Http.Throttling;
using Cuemon.AspNetCore.Infrastructure;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Options;

namespace Cuemon.AspNetCore.Mvc.Filters.Throttling
{
    /// <summary>
    /// A filter that provides an API throttling of action methods.
    /// </summary>
    /// <seealso cref="ConfigurableAsyncActionFilter{TOptions}"/>
    /// <seealso cref="IAsyncActionFilter" />
    public class ThrottlingSentinelFilter : ConfigurableAsyncActionFilter<ThrottlingSentinelOptions>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ThrottlingSentinelFilter"/> class.
        /// </summary>
        /// <param name="setup">The <see cref="ThrottlingSentinelOptions" /> which need to be configured.</param>
        /// <param name="tc">The dependency injected <see cref="IThrottlingCache"/>.</param>
        public ThrottlingSentinelFilter(IOptions<ThrottlingSentinelOptions> setup, IThrottlingCache tc) : base(setup)
        {
            ThrottlingCache = tc;
        }

        private IThrottlingCache ThrottlingCache { get; }

        /// <summary>
        /// Called asynchronously before the action, after model binding is complete.
        /// </summary>
        /// <param name="context">The <see cref="ActionExecutingContext" />.</param>
        /// <param name="next">The <see cref="ActionExecutionDelegate" />. Invoked to execute the next action filter or the action itself.</param>
        /// <returns>A <see cref="Task" /> that on completion indicates the filter has executed.</returns>
        public override async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            await AspNetCoreInfrastructure.InvokeThrottlerSentinelAsync(context.HttpContext, ThrottlingCache, Options, (message, response) =>
            {
                response.StatusCode = (int) message.StatusCode;
                Decorator.Enclose(response.Headers).AddOrUpdateHeaders(message.Headers);
            }).ConfigureAwait(false);
            await next().ConfigureAwait(false);
        }
    }
}