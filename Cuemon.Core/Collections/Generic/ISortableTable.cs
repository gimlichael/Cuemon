using System.Collections.Generic;

namespace Cuemon.Collections.Generic
{
    /// <summary>
    /// Defines a generic way to perform a table-like sorting operation.
    /// </summary>
    /// <typeparam name="TSource">The type of the elements of to <see cref="OrderBy"/>.</typeparam>
    public interface ISortableTable<TSource>
    {
        /// <summary>
        /// Gets the column on which to perform a sorting operation.
        /// </summary>
        /// <value>The column on which to perform a sorting operation.</value>
        string OrderByColumn { get; }

        /// <summary>
        /// Sorts the elements of <paramref name="source"/> using the order of <paramref name="direction"/>.
        /// </summary>
        /// <param name="source">A sequence of values to order by <paramref name="direction"/>.</param>
        /// <param name="direction">The <see cref="SortOrder"/> to apply to the <paramref name="source"/> sequence.</param>
        /// <returns>An <see cref="IEnumerable{T}"/> whose elements are sorted according to <see cref="OrderByColumn"/> and <paramref name="direction"/>.</returns>
        IEnumerable<TSource> OrderBy(IEnumerable<TSource> source, SortOrder direction);
    }
}