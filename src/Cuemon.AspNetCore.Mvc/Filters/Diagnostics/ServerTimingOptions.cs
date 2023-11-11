using System;
using Cuemon.Configuration;
using Cuemon.Diagnostics;
using Microsoft.Extensions.Hosting;

namespace Cuemon.AspNetCore.Mvc.Filters.Diagnostics
{
    /// <summary>
    /// Configuration options for <see cref="ServerTimingFilter"/>.
    /// </summary>
    /// <seealso cref="TimeMeasureOptions" />
    public class ServerTimingOptions : TimeMeasureOptions, IValidatableParameterObject
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
        public Func<IHostEnvironment, bool> SuppressHeaderPredicate { get; set; }

        /// <summary>
        /// Determines whether the public read-write properties of this instance are in a valid state.
        /// </summary>
        /// <exception cref="InvalidOperationException">
        /// <see cref="SuppressHeaderPredicate"/> cannot be null.
        /// </exception>
        /// <remarks>This method is expected to throw exceptions when one or more conditions fails to be in a valid state.</remarks>
        public void ValidateOptions()
        {
            Validator.ThrowIfObjectStateInvalid(SuppressHeaderPredicate == null);
        }
    }
}
