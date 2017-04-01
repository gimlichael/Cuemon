using System;
using Cuemon.Diagnostics;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Cuemon.AspNetCore.Mvc.Filters
{
    /// <summary>
    /// Represents an attribute that is used to mark an action method for time measure profiling.
    /// </summary>
    /// <seealso cref="ActionFilterAttribute" />
    public class TimeMeasureAttribute : ActionFilterAttribute, IFilterFactory
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="TimeMeasureAttribute"/> class.
        /// </summary>
        public TimeMeasureAttribute() : this(0, TimeUnit.Ticks)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="TimeMeasureAttribute" /> class.
        /// </summary>
        /// <param name="threshold">The <see cref="double" /> value that in combination with <paramref name="thresholdTimeUnit" /> specifies the threshold of the action method.</param>
        /// <param name="thresholdTimeUnit">One of the enumeration values that specifies the time unit of <paramref name="threshold" />.</param>
        public TimeMeasureAttribute(double threshold, TimeUnit thresholdTimeUnit)
        {
            Setup = options =>
            {
                options.TimeMeasureCompletedThreshold = TimeSpanConverter.FromDouble(threshold, thresholdTimeUnit);
            };
        }

        /// <summary>
        /// Creates an instance of the executable filter.
        /// </summary>
        /// <param name="serviceProvider">The request <see cref="T:System.IServiceProvider" />.</param>
        /// <returns>An instance of the executable filter.</returns>
        public IFilterMetadata CreateInstance(IServiceProvider serviceProvider)
        {
            return new TimeMeasureCoreFilter(Setup);
        }

        private Action<TimeMeasureOptions> Setup { get; }

        /// <summary>
        /// Gets a value that indicates if the result of <see cref="M:Microsoft.AspNetCore.Mvc.Filters.IFilterFactory.CreateInstance(System.IServiceProvider)" /> can be reused across requests.
        /// </summary>
        /// <value><c>true</c> if this instance is reusable; otherwise, <c>false</c>.</value>
        public bool IsReusable => false;
    }
}