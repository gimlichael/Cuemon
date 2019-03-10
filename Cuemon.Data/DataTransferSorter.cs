using System;
using System.Collections.Generic;
using Cuemon.Collections.Generic;

namespace Cuemon.Data
{
    /// <summary>
    /// Provides a default implementation of a <see cref="DataTransferRow"/> sorter. This class cannot be inherited.
    /// </summary>
    public sealed class DataTransferSorter : ISortableTable<DataTransferRow>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DataTransferSorter"/> class.
        /// </summary>
        /// <param name="orderByColumn">The column on which to perform a sorting operation.</param>
        public DataTransferSorter(string orderByColumn)
        {
            OrderByColumn = orderByColumn;
        }

        /// <summary>
        /// Sorts the elements of <paramref name="source"/> using the order of <paramref name="direction"/>.
        /// </summary>
        /// <param name="source">A sequence of values to order by <paramref name="direction"/>.</param>
        /// <param name="direction">The <see cref="SortOrder"/> to apply to the <paramref name="source"/> sequence.</param>
        /// <returns>An <see cref="IEnumerable{DataTransferRow}"/> whose elements are sorted according to <see cref="OrderByColumn"/> and <paramref name="direction"/>.</returns>
        public IEnumerable<DataTransferRow> OrderBy(IEnumerable<DataTransferRow> source, SortOrder direction)
        {
            switch (direction)
            {
                case SortOrder.Ascending:
                    return source.OrderBy(DataTransferRowComparison);
                case SortOrder.Descending:
                    return source.OrderByDescending(DataTransferRowComparison);
                default:
                    return source;
            }
        }

        /// <summary>
        /// Gets the column on which to perform a sorting operation.
        /// </summary>
        /// <value>The column on which to perform a sorting operation.</value>
        public string OrderByColumn { get; private set; }

        private int DataTransferRowComparison(DataTransferRow x, DataTransferRow y)
        {
            return ComparisonUtility.Default(x, y, TypeSelector, ValueSelector);
        }

        private Type TypeSelector(DataTransferRow row)
        {
            return row.Columns[OrderByColumn].DataType;
        }

        private object ValueSelector(DataTransferRow row)
        {
            return row[OrderByColumn];
        }
    }
}