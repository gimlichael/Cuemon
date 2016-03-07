using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.Common;

namespace Cuemon.Data
{
    /// <summary>
    /// Represents a collection of <see cref="DataTransferColumn"/> objects for a table in a database. This class cannot be inherited.
    /// </summary>
    public sealed class DataTransferColumnCollection : IEnumerable<DataTransferColumn>
    {
        private readonly Collection<DataTransferColumn> _dataTransferColumns = new Collection<DataTransferColumn>();

        /// <summary>
        /// Initializes a new instance of the <see cref="DataTransferColumnCollection"/> class.
        /// </summary>
        /// <param name="record">The record to convert.</param>
        /// <param name="columns">The columns meta data to reference.</param>
        internal DataTransferColumnCollection(DbDataReader record, ref IList<KeyValuePair<string, Type>> columns)
        {
            if (columns == null) { columns = new List<KeyValuePair<string, Type>>(); }
            int fieldCount = record == null ? columns.Count : record.FieldCount;
            for (int i = 0; i < fieldCount; i++)
            {
                if (record != null && 
                    columns.Count == i &&
                    columns.Count < record.FieldCount) { columns.Add(new KeyValuePair<string, Type>(record.GetName(i), record.GetFieldType(i))); }

                string columnName = columns[i].Key;
                Type columnType = columns[i].Value;
                DataTransferColumns.Add(new DataTransferColumn(i, columnName, record == null ? TypeUtility.GetDefaultValue(columnType) : record.IsDBNull(i) ? null : record.GetValue(i), columnType));
            }
        }

        /// <summary>
        /// Gets the <see cref="DataTransferColumn"/> from the collection with the specified name.
        /// </summary>
        /// <param name="name">The name of the column from which to return.</param>
        /// <returns>A <see cref="DataTransferColumn"/> if found; otherwise null.</returns>
        public DataTransferColumn Get(string name)
        {
            foreach (DataTransferColumn column in this)
            {
                if (column.Name.Equals(name, StringComparison.OrdinalIgnoreCase)) { return column; }
            }
            return null;
        }

        /// <summary>
        /// Gets the <see cref="DataTransferColumn"/> from the collection with the specified name.
        /// </summary>
        /// <param name="index">The zero-based index of the column from which to return.</param>
        /// <returns>A <see cref="DataTransferColumn"/> if found; otherwise null.</returns>
        public DataTransferColumn Get(int index)
        {
            return DataTransferColumns[index];
        }

        /// <summary>
        /// Gets the value of a <see cref="DataTransferColumn"/> from the collection with the specified name.
        /// </summary>
        /// <param name="name">The name of the column from which to return the value from.</param>
        /// <returns>An <see cref="object"/> that contains the data of the column.</returns>
        public object this[string name]
        {
            get
            {
                DataTransferColumn column = Get(name);
                return column == null ? null : column.Value;
            }
        }

        /// <summary>
        /// Gets the value of a <see cref="DataTransferColumn"/> from the collection with the specified name.
        /// </summary>
        /// <param name="index">The zero-based index of the column from which to return the value from.</param>
        /// <returns>An <see cref="object"/> that contains the data of the column.</returns>
        public object this[int index]
        {
            get
            {
                DataTransferColumn column = Get(index);
                return column == null ? null : column.Value;
            }
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
        public bool Contains(DataTransferColumn item)
        {
            return DataTransferColumns.Contains(item);
        }

        /// <summary>
        /// Gets the number of elements contained in the <see cref="T:System.Collections.Generic.ICollection`1" />.
        /// </summary>
        /// <value>The count.</value>
        /// <returns>The number of elements contained in the <see cref="T:System.Collections.Generic.ICollection`1" />.</returns>
        public int Count
        {
            get { return DataTransferColumns.Count; }
        }

        /// <summary>
        /// Returns an enumerator that iterates through the collection.
        /// </summary>
        /// <returns>A <see cref="T:System.Collections.Generic.IEnumerator`1" /> that can be used to iterate through the collection.</returns>
        public IEnumerator<DataTransferColumn> GetEnumerator()
        {
            return DataTransferColumns.GetEnumerator();
        }

        /// <summary>
        /// Determines the index of a specific item in the <see cref="T:System.Collections.Generic.IList`1" />.
        /// </summary>
        /// <param name="item">The object to locate in the <see cref="T:System.Collections.Generic.IList`1" />.</param>
        /// <returns>The index of <paramref name="item" /> if found in the list; otherwise, -1.</returns>
        public int IndexOf(DataTransferColumn item)
        {
            return DataTransferColumns.IndexOf(item);
        }

        private Collection<DataTransferColumn> DataTransferColumns
        {
            get { return _dataTransferColumns; }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}