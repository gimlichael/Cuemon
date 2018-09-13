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
        /// </list>
        /// </remarks>
        public TimeMeasuringOptions()
        {
            HeaderName = "X-Action-Profiler";
            SuppressHeaderPredicate = _ => false;
        }

        /// <summary>
        /// Gets or sets the name of the time-measured HTTP header.
        /// </summary>
        /// <value>The name of the time-measured HTTP header.</value>
        public string HeaderName { get; set; }

        /// <summary>
        /// Gets or sets the predicate that can suppress the time-measured HTTP header.
        /// </summary>
        /// <value>The function delegate that can determine if the time-measured HTTP header should be suppressed.</value>
        public Func<IHostingEnvironment, bool> SuppressHeaderPredicate { get; set; }
    }
}