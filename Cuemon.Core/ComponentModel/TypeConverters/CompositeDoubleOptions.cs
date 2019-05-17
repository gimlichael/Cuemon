namespace Cuemon.ComponentModel.TypeConverters
{
    /// <summary>
    /// Configuration options for <see cref="CompositeDoubleConverter"/>.
    /// </summary>
    public class CompositeDoubleOptions
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CompositeDoubleOptions"/> class.
        /// </summary>
        /// <remarks>
        /// The following table shows the initial property values for an instance of <see cref="CompositeDoubleOptions"/>.
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
        public CompositeDoubleOptions()
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