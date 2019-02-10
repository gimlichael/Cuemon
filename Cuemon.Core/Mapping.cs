namespace Cuemon
{
    /// <summary>
    /// Defines the mapping between a column/field/item in a data source and a column/field/item in the data destination.
    /// </summary>
    public class Mapping
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Mapping"/> class.
        /// </summary>
        /// <param name="source">The name of the source column/field/item within the data source.</param>
        /// <param name="destination">The name of the destination column/field/item within the data destination.</param>
        public Mapping(string source, string destination)
        {
            Source = source;
            Destination = destination;
        }

        /// <summary>
        /// Gets the name of the column/field/item being mapped in the data source.
        /// </summary>
        /// <value>The string value of the column/field/item being mapped in the data source.</value>
        public string Source { get; private set; }

        /// <summary>
        /// Get the name of the column/field/item being mapped in the data destination.
        /// </summary>
        /// <value>The string value of the column/field/item being mapped in the data destination.</value>
        public string Destination { get; private set; }
    }
}