using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.Common;
using System.Linq;

namespace Cuemon.Data
{
    /// <summary>
    /// Represents a collection of <see cref="DataTransferRow"/> objects for a table in a database. This class cannot be inherited.
    /// </summary>
    public sealed class DataTransferRowCollection : IEnumerable<DataTransferRow>
    {
        private readonly Collection<DataTransferRow> _dataTransferRows = new Collection<DataTransferRow>();

        private string ColumnNamesSelector(KeyValuePair<string, Type> keyValuePair)
        {
            return keyValuePair.Key;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="DataTransferRowCollection"/> class.
        /// </summary>
        /// <param name="reader">The reader to convert.</param>
        internal DataTransferRowCollection(DbDataReader reader)
        {
            Validator.ThrowIfNull(reader, nameof(reader));
            Validator.ThrowIfTrue(reader.IsClosed, nameof(reader), "Reader was closed.");

            int rowNumber = 1;
            IList<KeyValuePair<string, Type>> columns = null;
            while (reader.Read())
            {
                DataTransferRows.Add(new DataTransferRow(reader, rowNumber, ref columns));
                rowNumber++;
            }
            ColumnNames = columns.Select(ColumnNamesSelector);
        }

        /// <summary>
        /// Gets the column names that is present in this <see cref="DataTransferRow"/>.
        /// </summary>
        /// <value>The column names of a table-row in a database.</value>
        public IEnumerable<string> ColumnNames
        {
            get;
            private set;
        }

        private Collection<DataTransferRow> DataTransferRows
        {
            get { return _dataTransferRows; }
        }

        /// <summary>
        /// Gets the <see cref="DataTransferRow"/> at the specified index.
        /// </summary>
        /// <param name="index">The zero-based index of the row to return.</param>
        /// <returns>The specified <see cref="DataTransferRow"/>.</returns>
        public DataTransferRow this[int index]
        {
            get { return DataTransferRows[index]; }
        }

        /// <summary>
        /// Gets a value indicating whether the <see cref="T:System.Collections.Generic.ICollection`1" /> is read-only.
        /// </summary>
        /// <value><c>true</c> if this instance is read only; otherwise, <c>false</c>.</value>
        /// <returns>true if the <see cref="T:System.Collections.Generic.ICollection`1" /> is read-only; otherwise, false.</returns>
        public bool IsReadOnly
        {
            get { return true; }
        }

        /// <summary>
        /// Determines whether the <see cref="T:System.Collections.Generic.ICollection`1" /> contains a specific value.
        /// </summary>
        /// <param name="item">The object to locate in the <see cref="T:System.Collections.Generic.ICollection`1" />.</param>
        /// <returns>true if <paramref name="item" /> is found in the <see cref="T:System.Collections.Generic.ICollection`1" />; otherwise, false.</returns>
        public bool Contains(DataTransferRow item)
        {
            return DataTransferRows.Contains(item);
        }

        /// <summary>
        /// Gets the number of elements contained in the <see cref="T:System.Collections.Generic.ICollection`1" />.
        /// </summary>
        /// <value>The count.</value>
        /// <returns>The number of elements contained in the <see cref="T:System.Collections.Generic.ICollection`1" />.</returns>
        public int Count
        {
            get { return DataTransferRows.Count; }
        }



        /// <summary>
        /// Returns an enumerator that iterates through the collection.
        /// </summary>
        /// <returns>A <see cref="T:System.Collections.Generic.IEnumerator`1" /> that can be used to iterate through the collection.</returns>
        public IEnumerator<DataTransferRow> GetEnumerator()
        {
            return DataTransferRows.GetEnumerator();
        }


        /// <summary>
        /// Determines the index of a specific item in the <see cref="T:System.Collections.Generic.IList`1" />.
        /// </summary>
        /// <param name="item">The object to locate in the <see cref="T:System.Collections.Generic.IList`1" />.</param>
        /// <returns>The index of <paramref name="item" /> if found in the list; otherwise, -1.</returns>
        public int IndexOf(DataTransferRow item)
        {
            return DataTransferRows.IndexOf(item);
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
