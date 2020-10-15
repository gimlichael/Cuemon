using System;
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
    public class ServerTimingAttribute : ActionFilterAttribute, IFilterFactory
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ServerTimingAttribute"/> class.
        /// </summary>
        public ServerTimingAttribute()
        {
        }

        /// <summary>
        /// Gets or sets the server-specified metric name.
        /// </summary>
        /// <value>The server-specified metric name.</value>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the server-specified metric description.
        /// </summary>
        /// <value>The server-specified metric description.</value>
        public string Description { get; set; }

        /// <summary>
        /// Gets or sets the value that in combination with <see cref="ThresholdTimeUnit" /> specifies the threshold of the action method.
        /// </summary>
        /// <value>The threshold value of the action method.</value>
        public double Threshold { get; set; } = 0;

        /// <summary>
        /// Gets or sets one of the enumeration values that specifies the time unit of <see cref="Threshold"/>.
        /// </summary>
        /// <value>The <see cref="TimeUnit"/> that defines the actual <see cref="Threshold"/>.</value>
        public TimeUnit ThresholdTimeUnit { get; set; } = TimeUnit.Ticks;

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
            var filter = new ServerTimingFilter(Options.Create(new ServerTimingOptions()
            {
                TimeMeasureCompletedThreshold = Decorator.Enclose(Threshold).ToTimeSpan(ThresholdTimeUnit)
            }), he)
            {
                Name = Name,
                Description = Description
            };
            var stOptions = serviceProvider.GetService<IOptions<ServerTimingOptions>>();
            if (stOptions?.Value?.SuppressHeaderPredicate != null)
            {
                filter.Options.SuppressHeaderPredicate = stOptions.Value.SuppressHeaderPredicate;
            }

            return filter;
        }

        /// <summary>
        /// Gets a value that indicates if the result of <see cref="IFilterFactory.CreateInstance(IServiceProvider)" /> can be reused across requests.
        /// </summary>
        /// <value><c>true</c> if this instance is reusable; otherwise, <c>false</c>.</value>
        public bool IsReusable => false;
    }
}