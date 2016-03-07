using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Text;

namespace Cuemon.Data
{
    /// <summary>
    /// Represents the row of a table in a database. This class cannot be inherited.
    /// </summary>
    public sealed class DataTransferRow
    {
        internal DataTransferRow(DbDataReader record, int rowNumber, ref IList<KeyValuePair<string, Type>> columns)
        {
            Columns = new DataTransferColumnCollection(record, ref columns);
            Number = rowNumber;
        }

        /// <summary>
        /// Gets the row number.
        /// </summary>
        /// <value>The row number.</value>
        public int Number { get; private set; }

        /// <summary>
        /// Gets the associated columns of this row.
        /// </summary>
        /// <value>The associated columns of this row.</value>
        public DataTransferColumnCollection Columns
        {
            get; private set;
        }

        /// <summary>
        /// Gets the value of a <see cref="DataTransferColumn"/> from the <see cref="Columns"/> with the specified name.
        /// </summary>
        /// <param name="name">The name of the column from which to return the value from.</param>
        /// <returns>An <see cref="object"/> that contains the data of the column.</returns>
        public object this[string name]
        {
            get { return Columns[name]; }
        }

        /// <summary>
        /// Gets the value of a <see cref="DataTransferColumn"/> from the <see cref="Columns"/> with the specified name.
        /// </summary>
        /// <param name="index">The zero-based index of the column from which to return the value from.</param>
        /// <returns>An <see cref="object"/> that contains the data of the column.</returns>
        public object this[int index]
        {
            get { return Columns[index]; }
        }

        /// <summary>
        /// Returns a <see cref="System.String" /> that represents this instance.
        /// </summary>
        /// <returns>A <see cref="System.String" /> that represents this instance.</returns>
        public override string ToString()
        {
            StringBuilder builder = new StringBuilder();
            for (int i = 0; i < Columns.Count; i++)
            {
                DataTransferColumn column = Columns.Get(i);
                builder.AppendFormat("{0}={1} [{2}],", column.Name, column.Value, column.DataType.Name);
            }
            return builder.ToString(0, builder.Length - 1);
        }
    }
}