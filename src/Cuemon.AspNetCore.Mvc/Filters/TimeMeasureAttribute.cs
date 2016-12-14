using System;
using Cuemon.Diagnostics;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Cuemon.AspNetCore.Mvc.Filters
{
    /// <summary>
    /// Represents an attribute that is used to mark an action method for time measure profiling.
    /// </summary>
    /// <seealso cref="ActionFilterAttribute" />
    public class TimeMeasureAttribute : ActionFilterAttribute
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="TimeMeasureAttribute"/> class.
        /// </summary>
        public TimeMeasureAttribute()
            : this(0, TimeUnit.Ticks)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="TimeMeasureAttribute" /> class.
        /// </summary>
        /// <param name="threshold">The <see cref="double" /> value that in combination with <paramref name="thresholdTimeUnit" /> specifies the threshold of the action method.</param>
        /// <param name="thresholdTimeUnit">One of the enumeration values that specifies the time unit of <paramref name="threshold" />.</param>
        public TimeMeasureAttribute(double threshold, TimeUnit thresholdTimeUnit)
        {
            Setup = options => options.TimeMeasureCompletedThreshold = TimeSpanConverter.FromDouble(threshold, thresholdTimeUnit);
        }

        private Action<TimeMeasureOptions> Setup { get; }

        /// <summary>
        /// Called by the ASP.NET Core MVC framework before the action method executes.
        /// </summary>
        /// <param name="context">The filter context.</param>
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            Infrastructure.InterceptControllerWithProfiler(context, Setup.ConfigureOptions());
        }
    }
}