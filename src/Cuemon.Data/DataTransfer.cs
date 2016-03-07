using System;
using System.Collections.Generic;
using System.Data.Common;
using Cuemon.Collections.Generic;

namespace Cuemon.Data
{
    /// <summary>
    /// Provides a way to convert an <see cref="DbDataReader"/> implementation to a table-like data transfer object.
    /// </summary>
    public static class DataTransfer
    {
        /// <summary>
        /// Converts the specified <paramref name="reader"/> implementation to a table-like data transfer object collection.
        /// </summary>
        /// <param name="reader">The reader to be converted.</param>
        /// <returns>A <see cref="DataTransferRowCollection"/> that is the result of the specified <paramref name="reader"/>.</returns>
        /// <exception cref="System.ArgumentNullException">
        /// <paramref name="reader"/> is null.
        /// </exception>
        /// <exception cref="System.ArgumentException">
        /// <paramref name="reader"/> is closed.
        /// </exception>
        public static DataTransferRowCollection GetRows(DbDataReader reader)
        {
            Validator.ThrowIfNull(reader, nameof(reader));
            Validator.ThrowIfTrue(reader.IsClosed, nameof(reader), "Reader was closed.");
            return new DataTransferRowCollection(reader);
        }

        /// <summary>
        /// Retrieves a paged, table-like data transfer object collection from the specified <paramref name="selector"/>.
        /// </summary>
        /// <param name="settings">The <see cref="PagedSettings"/> that will be applied to the <see cref="PagedCollection{DataTransferRow}"/> result of this method.</param>
        /// <param name="selector">The function delegate that will determine the <see cref="PagedCollection{DataTransferRow}"/> result of this method.</param>
        /// <param name="counter">The function delegate that will determine the total number of rows in a data-source from the specified <paramref name="settings"/>.</param>
        /// <returns>A <see cref="PagedCollection{DataTransferRow}"/> that is the result of the specified <paramref name="selector"/> with associated <paramref name="settings"/>.</returns>
        public static PagedCollection<DataTransferRow> GetPagedRows(PagedSettings settings, Doer<PagedSettings, IEnumerable<DataTransferRow>> selector, Doer<PagedSettings, int> counter)
        {
            return new PagedCollection<DataTransferRow>(selector, settings, counter);
        }

        /// <summary>
        /// Converts the specified and read-initialized <paramref name="reader"/> implementation to a column-like data transfer object collection.
        /// </summary>
        /// <param name="reader">The read-initialized reader to be converted.</param>
        /// <returns>A <see cref="DataTransferColumnCollection"/> that is the result of the specified and read-initialized <paramref name="reader"/>.</returns>
        /// <exception cref="System.ArgumentNullException">
        /// <paramref name="reader"/> is null.
        /// </exception>
        /// <exception cref="System.ArgumentException">
        /// <paramref name="reader"/> is closed.
        /// </exception>
        /// <exception cref="InvalidOperationException">
        /// Invalid attempt to read from <paramref name="reader"/> when no data is present.
        /// </exception>
        public static DataTransferColumnCollection GetColumns(DbDataReader reader)
        {
            Validator.ThrowIfNull(reader, nameof(reader));
            Validator.ThrowIfTrue(reader.IsClosed, nameof(reader), "Reader was closed.");
            IList<KeyValuePair<string, Type>> columns = null;
            return new DataTransferColumnCollection(reader, ref columns);
        }
    }
}