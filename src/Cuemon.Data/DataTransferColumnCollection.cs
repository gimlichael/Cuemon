using System;
using System.Collections.Generic;
using System.Data;

namespace Cuemon.Data
{
    /// <summary>
    /// Represents a collection of <see cref="DataTransferColumn"/> objects for a table in a database. This class cannot be inherited.
    /// </summary>
    public sealed class DataTransferColumnCollection : List<DataTransferColumn>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DataTransferColumnCollection"/> class.
        /// </summary>
        /// <param name="record">The record to convert.</param>
        internal DataTransferColumnCollection(IDataRecord record)
        {
            int fieldCount = record == null ? 0 : record.FieldCount;
            for (int i = 0; i < fieldCount; i++)
            {
                string columnName = record.GetName(i);
                Type columnType = record.GetFieldType(i);
                Add(new DataTransferColumn(i, columnName, columnType));
                Names.Add(columnName);
            }
        }

        private List<string> Names { get; } = new List<string>();

        /// <summary>
        /// Gets the <see cref="DataTransferColumn"/> from the collection with the specified name.
        /// </summary>
        /// <param name="name">The name of the column from which to return.</param>
        /// <returns>A <see cref="DataTransferColumn"/> if found; otherwise null.</returns>
        public DataTransferColumn this[string name]
        {
            get
            {
                var presult = GetIndex(name);
                return presult.HasValue ? this[presult.Value] : null;
            }
        }

        internal int? GetIndex(string name)
        {
            if (name == null) { return null; }
            int index = Names.IndexOf(name);
            return index < 0 ? null : (int?)index;
        }
    }
}