using System.Threading.Tasks;
using Cuemon.AspNetCore.Http;
using Cuemon.AspNetCore.Http.Headers;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Options;

namespace Cuemon.AspNetCore.Mvc.Filters.Headers
{
    /// <summary>
    /// A filter that provides an API key sentinel on action methods.
    /// </summary>
    /// <seealso cref="ConfigurableAsyncActionFilter{TOptions}"/>
    /// <seealso cref="IAsyncActionFilter" />
    public class ApiKeySentinelFilter : ConfigurableAsyncActionFilter<ApiKeySentinelOptions>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ApiKeySentinelFilter"/> class.
        /// </summary>
        /// <param name="setup">The <see cref="ApiKeySentinelOptions" /> which need to be configured.</param>
        public ApiKeySentinelFilter(IOptions<ApiKeySentinelOptions> setup) : base(setup)
        {
        }

        /// <summary>
        /// Called asynchronously before the action, after model binding is complete.
        /// </summary>
        /// <param name="context">The <see cref="ActionExecutingContext" />.</param>
        /// <param name="next">The <see cref="ActionExecutionDelegate" />. Invoked to execute the next action filter or the action itself.</param>
        /// <returns>A <see cref="Task" /> that on completion indicates the filter has executed.</returns>
        public override async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            await Decorator.Enclose(context.HttpContext).InvokeApiKeySentinelAsync(Options).ConfigureAwait(false);
            await next().ConfigureAwait(false);
        }
    }
}
