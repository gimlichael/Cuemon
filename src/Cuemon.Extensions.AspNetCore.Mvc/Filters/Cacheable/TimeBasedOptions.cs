using System;

namespace Cuemon.Extensions.AspNetCore.Mvc.Filters.Cacheable
{
    /// <summary>
    /// Specifies options that is related to the <see cref="TimeBasedObjectResult{T}" />.
    /// </summary>
    public class TimeBasedOptions
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="TimeBasedOptions"/> class.
        /// </summary>
        /// <remarks>
        /// The following table shows the initial property values for an instance of <see cref="TimeBasedOptions"/>.
        /// <list type="table">
        ///     <listheader>
        ///         <term>Property</term>
        ///         <description>Initial Value</description>
        ///     </listheader>
        ///     <item>
        ///         <term><see cref="Modified"/></term>
        ///         <description><c>null</c></description>
        ///     </item>
        /// </list>
        /// </remarks>
        public TimeBasedOptions()
        {
            Modified = null;
        }

        /// <summary>
        /// Gets or sets the modified date-time value of an object.
        /// </summary>
        /// <value>The modified date-time value of an object.</value>
        public DateTime? Modified { get; set; }
    }
}