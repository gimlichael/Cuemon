namespace Cuemon
{
    /// <summary>
    /// Defines the mapping between a column/field/item in a data source and a column/field/item in the data destination. This class cannot be inherited.
    /// </summary>
    public sealed class IndexMapping : Mapping
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Mapping" /> class.
        /// </summary>
        /// <param name="source">The name of the source column/field/item within the data source.</param>
        /// <param name="destination">The name of the destination column/field/item within the data destination.</param>
        public IndexMapping(string source, string destination) : base(source, destination)
        {
            SourceIndex = -1;
            DestinationIndex = -1;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="IndexMapping"/> class.
        /// </summary>
        /// <param name="source">The name of the source column/field/item within the data source.</param>
        /// <param name="destinationIndex">The ordinal position of the destination column/field/item within the data destination.</param>
        public IndexMapping(string source, int destinationIndex) : base(source, "")
        {
            Validator.ThrowIfLowerThan(destinationIndex, 0, nameof(destinationIndex));
            SourceIndex = -1;
            DestinationIndex = destinationIndex;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="IndexMapping"/> class.
        /// </summary>
        /// <param name="sourceIndex">The ordinal position of the source column/field/item within the data source.</param>
        /// <param name="destination">The name of the destination column/field/item within the data destination.</param>
        public IndexMapping(int sourceIndex, string destination) : base("", destination)
        {
            Validator.ThrowIfLowerThan(sourceIndex, 0, nameof(sourceIndex));
            SourceIndex = sourceIndex;
            DestinationIndex = -1;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="IndexMapping"/> class.
        /// </summary>
        /// <param name="sourceIndex">The ordinal position of the source column/field/item within the data source.</param>
        /// <param name="destinationIndex">The ordinal position of the destination column/field/item within the data destination.</param>
        public IndexMapping(int sourceIndex, int destinationIndex) : base("", "")
        {
            Validator.ThrowIfLowerThan(sourceIndex, 0, nameof(sourceIndex));
            Validator.ThrowIfLowerThan(destinationIndex, 0, nameof(destinationIndex));
            SourceIndex = sourceIndex;
            DestinationIndex = destinationIndex;
        }

        /// <summary>
        /// Gets the ordinal position of the column/field/item within the data source.
        /// </summary>
        /// <value>The integer value of the column/field/item within the data source.</value>
        public int SourceIndex { get; private set; }

        /// <summary>
        /// Gets the ordinal position of the column/field/item within the data destination.
        /// </summary>
        /// <value>The integer value of the column/field/item within the data destination.</value>
        public int DestinationIndex { get; private set; }
    }
}