using System;
using Cuemon.Diagnostics;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Cuemon.AspNetCore.Mvc.Filters
{
    internal class TimeMeasureCoreFilter : IActionFilter
    {
        public TimeMeasureCoreFilter(Action<TimeMeasureOptions> setup = null)
        {
            Options = setup.ConfigureOptions();
            Profiler = new TimeMeasureProfiler();
        }

        /// <summary>
        /// Called by the ASP.NET Core MVC framework before the action method executes.
        /// </summary>
        /// <param name="context">The filter context.</param>
        public void OnActionExecuting(ActionExecutingContext context)
        {
            Infrastructure.InterceptControllerWithProfilerOnActionExecuting(context, Options, Profiler);
        }

        /// <summary>
        /// Called by the ASP.NET Core MVC framework after the action method has executed.
        /// </summary>
        /// <param name="context">The filter context.</param>
        public void OnActionExecuted(ActionExecutedContext context)
        {
            Infrastructure.InterceptControllerWithProfilerOnActionExecuted(context, Options, Profiler);
        }

        private TimeMeasureOptions Options { get; }

        private TimeMeasureProfiler Profiler { get; }
    }
}