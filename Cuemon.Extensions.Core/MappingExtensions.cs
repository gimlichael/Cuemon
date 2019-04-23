using System.Collections.Generic;

namespace Cuemon.Extensions.Core
{
    /// <summary>
    /// Extension methods for the <see cref="Mapping"/> class.
    /// </summary>
    public static class MappingExtensions
    {
        /// <summary>
        /// Creates a new <see cref="Mapping"/> using column/field/item names to refer to a <paramref name="source"/> and a <paramref name="destination"/> which is added to the specified <paramref name="collection"/> of mappings.
        /// </summary>
        /// <param name="collection">A collection of <see cref="Mapping"/> elements.</param>
        /// <param name="source">The name of the column/field/item within the data source.</param>
        /// <param name="destination">The name of the destination column/field/item within the data destination.</param>
        public static void Add(this ICollection<Mapping> collection, string source, string destination)
        {
            Validator.ThrowIfNull(collection, nameof(collection));
            collection.Add(new Mapping(source, destination));
        }

        /// <summary>
        /// Creates a new <see cref="Mapping"/> using an ordinal for the <paramref name="sourceIndex"/> and a column/field/item name to describe the <paramref name="destination"/> which is added to the specified <paramref name="collection"/> of mappings.
        /// </summary>
        /// <param name="collection">A collection of <see cref="Mapping"/> elements.</param>
        /// <param name="sourceIndex">The ordinal position of the source column/field/item within the data source.</param>
        /// <param name="destination">The name of the destination column/field/item within the data destination.</param>
        public static void Add(this ICollection<Mapping> collection, int sourceIndex, string destination)
        {
            Validator.ThrowIfNull(collection, nameof(collection));
            collection.Add(new IndexMapping(sourceIndex, destination));
        }

        /// <summary>
        /// Creates a new <see cref="Mapping"/> using a column/field/item name to describe the <paramref name="source"/> and an ordinal to specify the <paramref name="destinationIndex"/> which is added to the specified <paramref name="collection"/> of mappings.
        /// </summary>
        /// <param name="collection">A collection of <see cref="Mapping"/> elements.</param>
        /// <param name="source">The name of the column/field/item within the data source.</param>
        /// <param name="destinationIndex">The ordinal position of the destination column/field/item within the data destination.</param>
        public static void Add(this ICollection<Mapping> collection, string source, int destinationIndex)
        {
            Validator.ThrowIfNull(collection, nameof(collection));
            collection.Add(new IndexMapping(source, destinationIndex));
        }

        /// <summary>
        /// Creates a new <see cref="Mapping"/> using ordinals to specify both <paramref name="sourceIndex"/> and <paramref name="destinationIndex"/> columns/fields/items which is added to the specified <paramref name="collection"/> of mappings.
        /// </summary>
        /// <param name="collection">A collection of <see cref="Mapping"/> elements.</param>
        /// <param name="sourceIndex">The ordinal position of the source column/field/item within the data source.</param>
        /// <param name="destinationIndex">The ordinal position of the destination column/field/item within the data destination.</param>
        public static void Add(this ICollection<Mapping> collection, int sourceIndex, int destinationIndex)
        {
            Validator.ThrowIfNull(collection, nameof(collection));
            collection.Add(new IndexMapping(sourceIndex, destinationIndex));
        }
    }
}