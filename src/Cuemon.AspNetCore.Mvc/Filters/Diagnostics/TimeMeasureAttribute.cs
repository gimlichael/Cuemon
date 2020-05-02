using System;
using Cuemon.Diagnostics;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;

namespace Cuemon.AspNetCore.Mvc.Filters.Diagnostics
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
            Threshold = threshold;
            ThresholdTimeUnit = thresholdTimeUnit;
        }

        /// <summary>
        /// Gets or sets the value that in combination with <see cref="ThresholdTimeUnit" /> specifies the threshold of the action method.
        /// </summary>
        /// <value>The threshold value of the action method.</value>
        public double Threshold { get; set; }

        /// <summary>
        /// Gets or sets one of the enumeration values that specifies the time unit of <see cref="Threshold"/>.
        /// </summary>
        /// <value>The <see cref="TimeUnit"/> that defines the actual <see cref="Threshold"/>.</value>
        public TimeUnit ThresholdTimeUnit { get; set; }

        /// <summary>
        /// Creates an instance of the executable filter.
        /// </summary>
        /// <param name="serviceProvider">The request <see cref="IServiceProvider" />.</param>
        /// <returns>An instance of the executable filter.</returns>
        public IFilterMetadata CreateInstance(IServiceProvider serviceProvider)
        {
            #if NETSTANDARD
            var he = serviceProvider.GetRequiredService<IHostingEnvironment>();
            #elif NETCOREAPP
            var he = serviceProvider.GetRequiredService<IHostEnvironment>();
            #endif
            return new TimeMeasuringFilter(Options.Create(new TimeMeasuringOptions()
            {
                TimeMeasureCompletedThreshold = TimeMeasure.CreateTimeSpan(Threshold, ThresholdTimeUnit)
            }), he);
        }

        /// <summary>
        /// Gets a value that indicates if the result of <see cref="IFilterFactory.CreateInstance(IServiceProvider)" /> can be reused across requests.
        /// </summary>
        /// <value><c>true</c> if this instance is reusable; otherwise, <c>false</c>.</value>
        public bool IsReusable => false;
    }
}