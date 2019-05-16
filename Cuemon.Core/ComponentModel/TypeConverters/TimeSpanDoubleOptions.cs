namespace Cuemon.ComponentModel.TypeConverters
{
    /// <summary>
    /// Configuration options for <see cref="TimeSpanDoubleParser"/> and <see cref="TimeSpanDoubleConverter"/>.
    /// </summary>
    public class TimeSpanDoubleOptions
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="TimeSpanDoubleOptions"/> class.
        /// </summary>
        /// <remarks>
        /// The following table shows the initial property values for an instance of <see cref="GuidOptions"/>.
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
        public TimeSpanDoubleOptions()
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