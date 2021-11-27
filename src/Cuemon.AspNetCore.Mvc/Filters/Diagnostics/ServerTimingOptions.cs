using System;
using Cuemon.Diagnostics;
using Microsoft.Extensions.Hosting;

namespace Cuemon.AspNetCore.Mvc.Filters.Diagnostics
{
    /// <summary>
    /// Configuration options for <see cref="ServerTimingFilter"/>.
    /// </summary>
    /// <seealso cref="TimeMeasureOptions" />
    public class ServerTimingOptions : TimeMeasureOptions
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ServerTimingOptions"/> class.
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
        ///         <term><see cref="SuppressHeaderPredicate"/></term>
        ///         <description><c>he => he.IsProduction()</c></description>
        ///     </item>
        /// </list>
        /// </remarks>
        public ServerTimingOptions()
        {
            SuppressHeaderPredicate = he => he.IsProduction();
        }

        /// <summary>
        /// Gets or sets the predicate that can suppress the Server-Timing HTTP header(s).
        /// </summary>
        /// <value>The function delegate that can determine if the Server-Timing HTTP header(s) should be suppressed.</value>
        #if NETSTANDARD
        public Func<IHostingEnvironment, bool> SuppressHeaderPredicate { get; set; }
        #else
        public Func<IHostEnvironment, bool> SuppressHeaderPredicate { get; set; }
        #endif
    }
}