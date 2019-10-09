namespace Cuemon.ComponentModel
{
    /// <summary>
    /// Configuration options for <see cref="TimeConverter"/>.
    /// </summary>
    public class TimeOptions
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="TimeOptions"/> class.
        /// </summary>
        /// <remarks>
        /// The following table shows the initial property values for an instance of <see cref="TimeOptions"/>.
        /// <list type="table">
        ///     <listheader>
        ///         <term>Property</term>
        ///         <description>Initial Value</description>
        ///     </listheader>
        ///     <item>
        ///         <term><see cref="TimeUnit"/></term>
        ///         <description><see cref="Cuemon.TimeUnit.Seconds"/></description>
        ///     </item>
        /// </list>
        /// </remarks>
        public TimeOptions()
        {
            TimeUnit = TimeUnit.Seconds;
        }

        /// <summary>
        /// Gets or sets the unit of time.
        /// </summary>
        /// <value>The unit of time.</value>
        public TimeUnit TimeUnit { get; set; }
    }
}