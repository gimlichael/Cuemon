using System;
using System.Collections;
using System.Collections.Specialized;
using System.Data.Common;
using System.Globalization;
using System.Text;

namespace Cuemon.Data
{
    /// <summary>
    /// Provides a generic way of reading a forward-only stream of rows from a <see cref="string"/> based data source. This is an abstract class.
    /// </summary>
    public abstract class StringDataReader : DbDataReader
    {
        #region Constructors
        /// <summary>
        /// Initializes a new instance of the <see cref="StringDataReader"/> class.
        /// </summary>
        protected StringDataReader() : this(ObjectConverter.FromString)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="StringDataReader"/> class.
        /// </summary>
        /// <param name="parser">The function delegate that returns a primitive object whose value is equivalent to the provided <see cref="string"/> value.</param>
        /// <remarks>The default implementation uses <see cref="ObjectConverter.FromString(System.String)"/> as <paramref name="parser"/>.</remarks>
        protected StringDataReader(Doer<string, object> parser)
        {
            Validator.ThrowIfNull(parser, nameof(parser));
            StringParser = parser;
            Fields = new OrderedDictionary(StringComparer.OrdinalIgnoreCase);
        }
        #endregion

        #region Properties

        /// <summary>
        /// Determines whether this instance contains a column with the specified name.
        /// </summary>
        /// <param name="name">The name of the column to find.</param>
        /// <returns><c>true</c> if this instance contains a column with the specified name; otherwise, <c>false</c>.</returns>
        public bool Contains(string name)
        {
            return Fields.Contains(name);
        }

        /// <summary>
        /// Gets the column with the specified name.
        /// </summary>
        /// <param name="name">The name of the column to find.</param>
        /// <returns>The column with the specified name as an <see cref="object"/>.</returns>
        public override object this[string name]
        {
            get { return Fields[name]; }
        }

        /// <summary>
        /// Gets the column located at the specified index.
        /// </summary>
        /// <param name="i">The zero-based index of the column to get.</param>
        /// <returns>The column located at the specified index as an <see cref="object"/>.</returns>
        public override object this[int i]
        {
            get { return Fields[i]; }
        }

        public override int RecordsAffected
        {
            get { return -1; }
        }

        private IOrderedDictionary Fields { get; set; }

        /// <summary>
        /// Gets a value indicating whether the data reader is closed.
        /// </summary>
        /// <value><c>true</c> if this instance is closed; otherwise, <c>false</c>.</value>
        public override bool IsClosed
        {
            get { return IsDisposed; }
        }

        /// <summary>
        /// Gets a value indicating whether this instance is disposed.
        /// </summary>
        /// <value><c>true</c> if this instance is disposed; otherwise, <c>false</c>.</value>
        protected bool IsDisposed { get; private set; }

        /// <summary>
        /// Gets a reference to the function delegate that returns a primitive object whose value is equivalent to the provided <see cref="string"/> value.
        /// </summary>
        /// <value>A reference to the function delegate that this instance was constructed with.</value>
        protected Doer<string, object> StringParser { get; private set; }

        /// <summary>
        /// Gets the currently processed row count of this instance.
        /// </summary>
        /// <value>The currently processed row count of this instance.</value>
        /// <remarks>This property is incremented when the invoked <see cref="Read"/> method returns <c>true</c>.</remarks>
        public int RowCount { get; private set; }

        /// <summary>
        /// Gets the number of columns in the current row.
        /// </summary>
        /// <value>When not positioned in a valid recordset, 0; otherwise, the number of columns in the current record.</value>
        public override int FieldCount
        {
            get { return Fields.Count; }
        }
        #endregion

        #region Methods
        /// <summary>
        /// Returns a <see cref="System.String" /> that represents the current row of this instance.
        /// </summary>
        /// <returns>A <see cref="System.String" /> that represents the current row of this instance.</returns>
        public override string ToString()
        {
            StringBuilder builder = new StringBuilder();
            for (int i = 0; i < FieldCount; i++)
            {
                builder.AppendFormat("{0}={1}, ", GetName(i), GetValue(i));
            }
            if (builder.Length > 0) { builder.Remove(builder.Length - 2, 2); }
            return builder.ToString();
        }

        /// <summary>
        /// Advances the <see cref="T:System.Data.DbDataReader" /> to the next record.
        /// </summary>
        /// <returns><c>true</c> if there are more rows; otherwise, <c>false</c>.</returns>
        public override bool Read()
        {
            bool next = ReadNext();
            if (next) { RowCount++; }
            return next;
        }

        /// <summary>
        /// Advances the <see cref="T:System.Data.DbDataReader" /> to the next record.
        /// </summary>
        /// <returns><c>true</c> if there are more rows; otherwise, <c>false</c>.</returns>
        protected abstract bool ReadNext();

        /// <summary>
        /// Sets the fields of the current record invoked by <see cref="ReadNext"/>.
        /// </summary>
        /// <param name="fields">The fields of the current record invoked by <see cref="ReadNext"/>.</param>
        protected void SetFields(IOrderedDictionary fields)
        {
            Fields = fields;
        }

        /// <summary>
        /// Gets the value of the specified column as a Boolean.
        /// </summary>
        /// <param name="i">The zero-based column ordinal.</param>
        /// <returns>The value of the column.</returns>
        public override bool GetBoolean(int i)
        {
            return Convert.ToBoolean(GetValue(i), CultureInfo.InvariantCulture);
        }

        /// <summary>
        /// Gets the 8-bit unsigned integer value of the specified column.
        /// </summary>
        /// <param name="i">The zero-based column ordinal.</param>
        /// <returns>The 8-bit unsigned integer value of the specified column.</returns>
        public override byte GetByte(int i)
        {
            return Convert.ToByte(GetValue(i), CultureInfo.InvariantCulture);
        }

        public override long GetBytes(int i, long fieldOffset, byte[] buffer, int bufferoffset, int length)
        {
            return 0;
        }

        /// <summary>
        /// Gets the character value of the specified column.
        /// </summary>
        /// <param name="i">The zero-based column ordinal.</param>
        /// <returns>The character value of the specified column.</returns>
        public override char GetChar(int i)
        {
            return Convert.ToChar(GetValue(i), CultureInfo.InvariantCulture);
        }

        /// <summary>
        /// Gets the date and time data value of the specified field.
        /// </summary>
        /// <param name="i">The index of the field to find.</param>
        /// <returns>The date and time data value of the specified field.</returns>
        public override DateTime GetDateTime(int i)
        {
            return (DateTime)GetValue(i);
        }

        /// <summary>
        /// Gets the fixed-position numeric value of the specified field.
        /// </summary>
        /// <param name="i">The index of the field to find.</param>
        /// <returns>The fixed-position numeric value of the specified field.</returns>
        public override decimal GetDecimal(int i)
        {
            return Convert.ToDecimal(GetValue(i), CultureInfo.InvariantCulture);
        }

        /// <summary>
        /// Gets the double-precision floating point number of the specified field.
        /// </summary>
        /// <param name="i">The index of the field to find.</param>
        /// <returns>The double-precision floating point number of the specified field.</returns>
        public override double GetDouble(int i)
        {
            return Convert.ToDouble(GetValue(i), CultureInfo.InvariantCulture);
        }

        /// <summary>
        /// Gets the <see cref="T:System.Type" /> information corresponding to the type of <see cref="T:System.Object" /> that would be returned from <see cref="M:System.Data.IDataRecord.GetValue(System.Int32)" />.
        /// </summary>
        /// <param name="i">The index of the field to find.</param>
        /// <returns>The <see cref="T:System.Type" /> information corresponding to the type of <see cref="T:System.Object" /> that would be returned from <see cref="M:System.Data.IDataRecord.GetValue(System.Int32)" />.</returns>
        public override Type GetFieldType(int i)
        {
            return GetValue(i).GetType();
        }

        /// <summary>
        /// Gets the single-precision floating point number of the specified field.
        /// </summary>
        /// <param name="i">The index of the field to find.</param>
        /// <returns>The single-precision floating point number of the specified field.</returns>
        public override float GetFloat(int i)
        {
            return Convert.ToSingle(GetValue(i), CultureInfo.InvariantCulture);
        }

        /// <summary>
        /// Returns the GUID value of the specified field.
        /// </summary>
        /// <param name="i">The index of the field to find.</param>
        /// <returns>The GUID value of the specified field.</returns>
        public override Guid GetGuid(int i)
        {
            return (Guid)GetValue(i);
        }

        /// <summary>
        /// Gets the 16-bit signed integer value of the specified field.
        /// </summary>
        /// <param name="i">The index of the field to find.</param>
        /// <returns>The 16-bit signed integer value of the specified field.</returns>
        public override short GetInt16(int i)
        {
            return Convert.ToInt16(GetValue(i), CultureInfo.InvariantCulture);
        }

        /// <summary>
        /// Gets the 32-bit signed integer value of the specified field.
        /// </summary>
        /// <param name="i">The index of the field to find.</param>
        /// <returns>The 32-bit signed integer value of the specified field.</returns>
        public override int GetInt32(int i)
        {
            return Convert.ToInt32(GetValue(i), CultureInfo.InvariantCulture);
        }

        /// <summary>
        /// Gets the 64-bit signed integer value of the specified field.
        /// </summary>
        /// <param name="i">The index of the field to find.</param>
        /// <returns>The 64-bit signed integer value of the specified field.</returns>
        public override long GetInt64(int i)
        {
            return Convert.ToInt64(GetValue(i), CultureInfo.InvariantCulture);
        }

        /// <summary>
        /// Gets the name for the field to find.
        /// </summary>
        /// <param name="i">The index of the field to find.</param>
        /// <returns>The name of the field or the empty string (""), if there is no value to return.</returns>
        public override string GetName(int i)
        {
            int current = 0;
            foreach (string name in Fields.Keys)
            {
                if (i == current) { return name; }
                current++;
            }
            return string.Empty;
        }

        /// <summary>
        /// Return the index of the named field.
        /// </summary>
        /// <param name="name">The name of the field to find.</param>
        /// <returns>The index of the named field.</returns>
        /// <exception cref="System.ArgumentNullException">
        /// <paramref name="name"/> is null.
        /// </exception>
        /// <exception cref="ArgumentOutOfRangeException">
        /// <paramref name="name"/> is not a valid column name.
        /// </exception>
        public override int GetOrdinal(string name)
        {
            Validator.ThrowIfNull(name, nameof(name));
            int current = 0;
            foreach (string columnName in Fields.Keys)
            {
                if (columnName.Equals(name, StringComparison.OrdinalIgnoreCase)) { return current; }
                current++;
            }
            throw new ArgumentOutOfRangeException(nameof(name), "The name specified is not a valid column name.");
        }

        /// <summary>
        /// Gets the string value of the specified field.
        /// </summary>
        /// <param name="i">The index of the field to find.</param>
        /// <returns>The string value of the specified field.</returns>
        public override string GetString(int i)
        {
            return GetValue(i) as string;
        }

        /// <summary>
        /// Return the value of the specified field.
        /// </summary>
        /// <param name="i">The index of the field to find.</param>
        /// <returns>The <see cref="T:System.Object" /> which will contain the field value upon return.</returns>
        public override object GetValue(int i)
        {
            return Fields[i];
        }

        /// <summary>
        /// Return whether the specified field is set to null.
        /// </summary>
        /// <param name="i">The index of the field to find.</param>
        /// <returns>true if the specified field is set to null; otherwise, false.</returns>
        public override bool IsDBNull(int i)
        {
            return GetValue(i) == null || GetValue(i) == DBNull.Value;
        }

        /// <summary>
        /// Releases unmanaged and - optionally - managed resources.
        /// </summary>
        /// <param name="disposing"><c>true</c> to release both managed and unmanaged resources; <c>false</c> to release only unmanaged resources.</param>
        protected override void Dispose(bool disposing)
        {
            if (IsDisposed) { return; }
            base.Dispose(disposing);
            try
            {
                if (disposing)
                {
                }
            }
            finally
            {
                IsDisposed = true;    
            }
        }

        /// <summary>
        /// Gets a value indicating the depth of nesting for the current row.
        /// </summary>
        /// <value>The level of nesting.</value>
        public override int Depth
        {
            get { return 0; }
        }

        /// <summary>
        /// Populates an array of objects with the column values of the current record.
        /// </summary>
        /// <param name="values">An array of <see cref="T:System.Object" /> to copy the attribute fields into.</param>
        /// <returns>The number of instances of <see cref="T:System.Object" /> in the array.</returns>
        public override int GetValues(object[] values)
        {
            Validator.ThrowIfNull(values, nameof(values));
            int length = FieldCount;
            for (int i = 0; i < length; i++)
            {
                values[i] = GetValue(i);
            }
            return length;
        }

        public override bool NextResult()
        {
            return false;
        }

        public override long GetChars(int i, long fieldoffset, char[] buffer, int bufferoffset, int length)
        {
            return 0;
        }

        public override string GetDataTypeName(int i)
        {
            return typeof(string).ToString();
        }

        /// <summary>
        /// Returns an <see cref="T:System.Collections.IEnumerator" /> that can be used to iterate through the rows in the data reader.
        /// </summary>
        /// <returns>An <see cref="T:System.Collections.IEnumerator" /> that can be used to iterate through the rows in the data reader.</returns>
        /// <exception cref="System.NotImplementedException"></exception>
        public override IEnumerator GetEnumerator()
        {
            throw new NotImplementedException();
        }
        #endregion
    }
}