using System;
using Cuemon.Diagnostics;
using Microsoft.Extensions.Hosting;

namespace Cuemon.AspNetCore.Mvc.Filters.Diagnostics
{
    /// <summary>
    /// Configuration options for <see cref="TimeMeasuringFilter"/>.
    /// </summary>
    /// <seealso cref="TimeMeasureOptions" />
    public class TimeMeasuringOptions : TimeMeasureOptions
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="TimeMeasuringOptions"/> class.
        /// </summary>
        /// <remarks>
        /// The following table shows the initial property values for an instance of <see cref="TimeMeasureOptions"/>.
        /// <list type="table">
        ///     <listheader>
        ///         <term>Property</term>
        ///         <description>Initial Value</description>
        ///     </listheader>
        ///     <item>
        ///         <term><see cref="TimeMeasureOptions.TimeMeasureCompletedThreshold"/></term>
        ///         <description><see cref="TimeSpan.Zero"/></description>
        ///     </item>
        ///     <item>
        ///         <term><see cref="HeaderName"/></term>
        ///         <description><c>X-Action-Profiler</c></description>
        ///     </item>
        ///     <item>
        ///         <term><see cref="SuppressHeaderPredicate"/></term>
        ///         <description><c>_ => false</c></description>
        ///     </item>
        ///     <item>
        ///         <term><see cref="UseServerTimingHeader"/></term>
        ///         <description><c>true</c></description>
        ///     </item>
        /// </list>
        /// </remarks>
        public TimeMeasuringOptions()
        {
            HeaderName = "X-Action-Profiler";
            SuppressHeaderPredicate = _ => false;
            UseServerTimingHeader = true;
            UseCustomHeader = true;
        }

        /// <summary>
        /// Gets or sets the name of the custom time-measured HTTP header.
        /// </summary>
        /// <value>The name of the custom time-measured HTTP header.</value>
        public string HeaderName { get; set; }

        /// <summary>
        /// Gets or sets the predicate that can suppress either of the time-measured HTTP headers.
        /// </summary>
        /// <value>The function delegate that can determine if either of the time-measured HTTP headers should be suppressed.</value>
        public Func<IHostingEnvironment, bool> SuppressHeaderPredicate { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether to include a Server-Timing HTTP header specifying how long an action took to execute.
        /// </summary>
        /// <value><c>true</c> to include a Server-Timing HTTP header specifying how long an action took to execute; otherwise, <c>false</c>.</value>
        public bool UseServerTimingHeader { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether to include the custom <see cref="HeaderName"/> specifying how long an action took to execute.
        /// </summary>
        /// <value><c>true</c> to include the custom <see cref="HeaderName"/> specifying how long an action took to execute; otherwise, <c>false</c>.</value>
        public bool UseCustomHeader { get; set; }
    }
}