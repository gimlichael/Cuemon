using System;
using Cuemon.Diagnostics;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Cuemon.AspNetCore.Mvc.Filters
{
    /// <summary>
    /// A filter that performs time measure profiling of an action method.
    /// </summary>
    /// <seealso cref="IActionFilter" />
    public class TimeMeasuringFilter : IActionFilter
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="TimeMeasuringFilter"/> class.
        /// </summary>
        /// <param name="setup">The <see cref="TimeMeasureOptions"/> which need to be configured.</param>
        public TimeMeasuringFilter(Action<TimeMeasureOptions> setup = null)
        {
            Options = setup.ConfigureOptions();
        }

        private TimeMeasureOptions Options { get; }

        /// <summary>
        /// Called by the ASP.NET Core MVC framework before the action method executes.
        /// </summary>
        /// <param name="context">The filter context.</param>
        public void OnActionExecuting(ActionExecutingContext context)
        {
            Infrastructure.InterceptControllerWithProfiler(context, Options);
        }

        /// <summary>
        /// Called by the ASP.NET Core MVC framework after the action method has executed.
        /// </summary>
        /// <param name="context">The filter context.</param>
        public void OnActionExecuted(ActionExecutedContext context)
        {
        }
    }
}