using System;
using Cuemon.Diagnostics;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Cuemon.AspNetCore.Mvc.Filters
{
    /// <summary>
    /// A filter that performs time measure profiling of an action method.
    /// </summary>
    /// <seealso cref="IActionFilter" />
    public class TimeMeasuringFilter : IFilterFactory
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="TimeMeasuringFilter"/> class.
        /// </summary>
        /// <param name="setup">The <see cref="TimeMeasureOptions"/> which need to be configured.</param>
        public TimeMeasuringFilter(Action<TimeMeasureOptions> setup = null)
        {
            Setup = setup;
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