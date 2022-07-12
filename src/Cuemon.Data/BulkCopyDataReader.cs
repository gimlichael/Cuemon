using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Data.Common;
using System.Globalization;
using System.Linq;
using System.Text;

namespace Cuemon.Data
{
    /// <summary>
    /// Provides a way of copying an existing object implementing the <see cref="DbDataReader"/> class to a filtered forward-only stream of rows that is mapped for bulk upload. This class cannot be inherited.
    /// </summary>
    public sealed class BulkCopyDataReader : DbDataReader
    {
        private static readonly object PadLock = new();
        private IOrderedDictionary _defaultFields;

        /// <summary>
        /// Initializes a new instance of the <see cref="BulkCopyDataReader"/> class.
        /// </summary>
        /// <param name="reader">The <see cref="DbDataReader"/> object that contains the data.</param>
        /// <param name="mappings">A sequence of <see cref="Mapping"/> elements that specifies the data to be copied.</param>
        public BulkCopyDataReader(DbDataReader reader, IEnumerable<Mapping> mappings)
        {
            Validator.ThrowIfNull(reader, "source");
            Validator.ThrowIfNull(mappings, nameof(mappings));

            Reader = reader;
            Fields = new OrderedDictionary(StringComparer.OrdinalIgnoreCase);
            Mappings = new List<Mapping>(mappings);
            Init();
        }

        private void Init()
        {
            foreach (var mapping in Mappings.Select(mapping => mapping.Source))
            {
                if (string.IsNullOrEmpty(mapping)) { continue; }
                Fields.Add(mapping, null);
            }
            if (Fields.Count > 0 &&
                Fields.Count != Mappings.Count) { throw new InvalidOperationException("Mappings must be either all name or all ordinal based."); }
            UseOrdinal = Fields.Count == 0;
        }
        
        private DbDataReader Reader { get; set; }

        private void SetFields(IOrderedDictionary fields)
        {
            Fields = fields;
        }

        private bool UseOrdinal { get; set; }

        /// <summary>
        /// Gets the sequence of <see cref="Mapping"/> elements that specifies the data to be copied.
        /// </summary>
        /// <value>The <see cref="Mapping"/> elements that specifies the data to be copied.</value>
        public IList<Mapping> Mappings { get; private set; }

        #region Properties
        /// <summary>
        /// Gets the column with the specified name.
        /// </summary>
        /// <param name="name">The name of the column to find.</param>
        /// <returns>The column with the specified name as an <see cref="object"/>.</returns>
        public override object this[string name] => Fields[name];

        /// <summary>
        /// Gets the column located at the specified index.
        /// </summary>
        /// <param name="i">The zero-based index of the column to get.</param>
        /// <returns>The column located at the specified index as an <see cref="object"/>.</returns>
        public override object this[int i] => Fields[i];

        /// <summary>
        /// Gets the number of rows changed, inserted, or deleted by execution of the SQL statement.
        /// </summary>
        /// <value>The records affected.</value>
        public override int RecordsAffected => -1;

        private IOrderedDictionary Fields { get; set; }

        /// <summary>
        /// Gets a value that indicates whether this <see cref="T:System.Data.Common.DbDataReader" /> contains one or more rows.
        /// </summary>
        /// <value><c>true</c> if this instance has rows; otherwise, <c>false</c>.</value>
        public override bool HasRows => Reader.HasRows;

        /// <summary>
        /// Gets a value indicating whether the data reader is closed.
        /// </summary>
        /// <value><c>true</c> if this instance is closed; otherwise, <c>false</c>.</value>
        public override bool IsClosed => IsDisposed;

        private bool IsDisposed { get; set; }

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
        public override int FieldCount => Mappings.Count;

        #endregion

        #region Methods
        private IOrderedDictionary GetDefault()
        {
            if (_defaultFields == null)
            {
                lock (PadLock)
                {
                    if (_defaultFields == null)
                    {
                        _defaultFields = new OrderedDictionary();
                        if (UseOrdinal)
                        {
                            foreach (var mapping in Mappings.Where(mapping => mapping is IndexMapping).Cast<IndexMapping>())
                            {
                                _defaultFields.Add(mapping.SourceIndex, null);
                            }
                        }
                        else
                        {
                            foreach (var mapping in Mappings)
                            {
                                _defaultFields.Add(mapping.Source, null);
                            }
                        }
                    }
                }
            }
            return Reset(_defaultFields);
        }

        private IOrderedDictionary Reset(IOrderedDictionary source)
        {
            var result = UseOrdinal ? new OrderedDictionary(source.Count, EqualityComparer<int>.Default) : new OrderedDictionary(source.Count, StringComparer.OrdinalIgnoreCase);
            foreach (DictionaryEntry o in source)
            {
                result.Add(o.Key, o.Value);
            }
            return result;
        }

        /// <summary>
        /// Advances the <see cref="T:System.Data.DbDataReader" /> to the next record.
        /// </summary>
        /// <returns><c>true</c> if there are more rows; otherwise, <c>false</c>.</returns>
        public override bool Read()
        {
            if (!Reader.Read()) { return false; }
            var fields = GetDefault();
            for (var i = 0; i < Reader.FieldCount; i++)
            {
                if (UseOrdinal)
                {
                    if (fields.Contains(i))
                    {
                        fields[i] = Reader[i];
                    }
                }
                else
                {
                    if (IsMatch(Reader.GetName(i)) && fields.Contains(Reader.GetName(i)))
                    {
                        fields[Reader.GetName(i)] = Reader[Reader.GetName(i)];
                    }
                }
            }
            RowCount++;
            SetFields(fields);
            return true;
        }

        private bool IsMatch(string localName)
        {
            foreach (var mapping in Mappings)
            {
                if (mapping.Source.Equals(localName, StringComparison.OrdinalIgnoreCase)) { return true; }
            }
            return false;
        }

        /// <summary>
        /// Gets the value of the specified column as a Boolean.
        /// </summary>
        /// <param name="ordinal">The zero-based column ordinal.</param>
        /// <returns>The value of the column.</returns>
        public override bool GetBoolean(int ordinal)
        {
            return Convert.ToBoolean(GetValue(ordinal), CultureInfo.InvariantCulture);
        }

        /// <summary>
        /// Gets the 8-bit unsigned integer value of the specified column.
        /// </summary>
        /// <param name="ordinal">The zero-based column ordinal.</param>
        /// <returns>The 8-bit unsigned integer value of the specified column.</returns>
        public override byte GetByte(int ordinal)
        {
            return Convert.ToByte(GetValue(ordinal), CultureInfo.InvariantCulture);
        }

        /// <summary>
        /// Reads a stream of bytes from the specified column, starting at location indicated by dataOffset, into the buffer, starting at the location indicated by bufferOffset.
        /// </summary>
        /// <param name="ordinal">The zero-based column ordinal.</param>
        /// <param name="dataOffset">The index within the row from which to begin the read operation.</param>
        /// <param name="buffer">The buffer into which to copy the data.</param>
        /// <param name="bufferOffset">The index with the buffer to which the data will be copied.</param>
        /// <param name="length">The maximum number of characters to read.</param>
        /// <returns>The actual number of bytes read.</returns>
        public override long GetBytes(int ordinal, long dataOffset, byte[] buffer, int bufferOffset, int length)
        {
            return 0;
        }

        /// <summary>
        /// Gets the character value of the specified column.
        /// </summary>
        /// <param name="ordinal">The zero-based column ordinal.</param>
        /// <returns>The character value of the specified column.</returns>
        public override char GetChar(int ordinal)
        {
            return Convert.ToChar(GetValue(ordinal), CultureInfo.InvariantCulture);
        }

        /// <summary>
        /// Gets the date and time data value of the specified field.
        /// </summary>
        /// <param name="ordinal">The index of the field to find.</param>
        /// <returns>The date and time data value of the specified field.</returns>
        public override DateTime GetDateTime(int ordinal)
        {
            return (DateTime)GetValue(ordinal);
        }

        /// <summary>
        /// Gets the fixed-position numeric value of the specified field.
        /// </summary>
        /// <param name="ordinal">The index of the field to find.</param>
        /// <returns>The fixed-position numeric value of the specified field.</returns>
        public override decimal GetDecimal(int ordinal)
        {
            return Convert.ToDecimal(GetValue(ordinal), CultureInfo.InvariantCulture);
        }

        /// <summary>
        /// Gets the double-precision floating point number of the specified field.
        /// </summary>
        /// <param name="ordinal">The index of the field to find.</param>
        /// <returns>The double-precision floating point number of the specified field.</returns>
        public override double GetDouble(int ordinal)
        {
            return Convert.ToDouble(GetValue(ordinal), CultureInfo.InvariantCulture);
        }

        /// <summary>
        /// Returns an <see cref="T:System.Collections.IEnumerator" /> that can be used to iterate through the rows in the data reader.
        /// </summary>
        /// <returns>An <see cref="T:System.Collections.IEnumerator" /> that can be used to iterate through the rows in the data reader.</returns>
        /// <exception cref="NotImplementedException"></exception>
        public override IEnumerator GetEnumerator()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Gets the <see cref="T:System.Type" /> information corresponding to the type of <see cref="T:System.Object" /> that would be returned from <see cref="M:System.Data.IDataRecord.GetValue(System.Int32)" />.
        /// </summary>
        /// <param name="ordinal">The index of the field to find.</param>
        /// <returns>The <see cref="T:System.Type" /> information corresponding to the type of <see cref="T:System.Object" /> that would be returned from <see cref="M:System.Data.IDataRecord.GetValue(System.Int32)" />.</returns>
        public override Type GetFieldType(int ordinal)
        {
            return GetValue(ordinal).GetType();
        }

        /// <summary>
        /// Gets the single-precision floating point number of the specified field.
        /// </summary>
        /// <param name="ordinal">The index of the field to find.</param>
        /// <returns>The single-precision floating point number of the specified field.</returns>
        public override float GetFloat(int ordinal)
        {
            return Convert.ToSingle(GetValue(ordinal), CultureInfo.InvariantCulture);
        }

        /// <summary>
        /// Returns the GUID value of the specified field.
        /// </summary>
        /// <param name="ordinal">The index of the field to find.</param>
        /// <returns>The GUID value of the specified field.</returns>
        public override Guid GetGuid(int ordinal)
        {
            return (Guid)GetValue(ordinal);
        }

        /// <summary>
        /// Gets the 16-bit signed integer value of the specified field.
        /// </summary>
        /// <param name="ordinal">The index of the field to find.</param>
        /// <returns>The 16-bit signed integer value of the specified field.</returns>
        public override short GetInt16(int ordinal)
        {
            return Convert.ToInt16(GetValue(ordinal), CultureInfo.InvariantCulture);
        }

        /// <summary>
        /// Gets the 32-bit signed integer value of the specified field.
        /// </summary>
        /// <param name="ordinal">The index of the field to find.</param>
        /// <returns>The 32-bit signed integer value of the specified field.</returns>
        public override int GetInt32(int ordinal)
        {
            return Convert.ToInt32(GetValue(ordinal), CultureInfo.InvariantCulture);
        }

        /// <summary>
        /// Gets the 64-bit signed integer value of the specified field.
        /// </summary>
        /// <param name="ordinal">The index of the field to find.</param>
        /// <returns>The 64-bit signed integer value of the specified field.</returns>
        public override long GetInt64(int ordinal)
        {
            return Convert.ToInt64(GetValue(ordinal), CultureInfo.InvariantCulture);
        }

        /// <summary>
        /// Gets the name for the field to find.
        /// </summary>
        /// <param name="ordinal">The index of the field to find.</param>
        /// <returns>The name of the field or the empty string (""), if there is no value to return.</returns>
        public override string GetName(int ordinal)
        {
            var current = 0;
            foreach (var mapping in Mappings)
            {
                if (ordinal == current) { return mapping.Source; }
                current++;
            }
            return string.Empty;
        }

        /// <summary>
        /// Return the index of the named field.
        /// </summary>
        /// <param name="name">The name of the field to find.</param>
        /// <returns>The index of the named field.</returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="name"/> is null.
        /// </exception>
        /// <exception cref="ArgumentOutOfRangeException">
        /// <paramref name="name"/> is not a valid column name.
        /// </exception>
        public override int GetOrdinal(string name)
        {
            Validator.ThrowIfNull(name, nameof(name));
            var current = 0;
            foreach (var mapping in Mappings)
            {
                if (mapping.Source.Equals(name, StringComparison.OrdinalIgnoreCase)) { return current; }
                current++;
            }
            throw new ArgumentOutOfRangeException(nameof(name), "The name specified name is not a valid column name.");
        }

        /// <summary>
        /// Gets the string value of the specified field.
        /// </summary>
        /// <param name="ordinal">The index of the field to find.</param>
        /// <returns>The string value of the specified field.</returns>
        public override string GetString(int ordinal)
        {
            return GetValue(ordinal) as string;
        }

        /// <summary>
        /// Return the value of the specified field.
        /// </summary>
        /// <param name="ordinal">The index of the field to find.</param>
        /// <returns>The <see cref="T:System.Object" /> which will contain the field value upon return.</returns>
        public override object GetValue(int ordinal)
        {
            return Fields[ordinal];
        }

        /// <summary>
        /// Return whether the specified field is set to null.
        /// </summary>
        /// <param name="ordinal">The index of the field to find.</param>
        /// <returns>true if the specified field is set to null; otherwise, false.</returns>
        public override bool IsDBNull(int ordinal)
        {
            return GetValue(ordinal) == null || GetValue(ordinal) == DBNull.Value;
        }

        /// <summary>
        /// Releases the managed resources used by the <see cref="T:System.Data.Common.DbDataReader" /> and optionally releases the unmanaged resources.
        /// </summary>
        /// <param name="disposing">true to release managed and unmanaged resources; false to release only unmanaged resources.</param>
        protected override void Dispose(bool disposing)
        {
            if (IsDisposed) { return; }
            base.Dispose(disposing);
            try
            {
                if (disposing)
                {
                    Reader.Dispose();
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
        /// <value>The depth of nesting for the current row.</value>
        /// <remarks>The outermost table has a depth of zero.</remarks>
        public override int Depth => 0;

        /// <summary>
        /// Populates an array of objects with the column values of the current record.
        /// </summary>
        /// <param name="values">An array of <see cref="T:System.Object" /> to copy the attribute fields into.</param>
        /// <returns>The number of instances of <see cref="T:System.Object" /> in the array.</returns>
        public override int GetValues(object[] values)
        {
            Validator.ThrowIfNull(values, nameof(values));
            var length = FieldCount;
            for (var i = 0; i < length; i++)
            {
                values[i] = GetValue(i);
            }
            return length;
        }

        /// <summary>
        /// Advances the reader to the next result when reading the results of a batch of statements.
        /// </summary>
        /// <returns><c>true</c> if there are more result sets; otherwise <c>false</c>.</returns>
        public override bool NextResult()
        {
            return false;
        }

        /// <summary>
        /// Reads a stream of characters from the specified column, starting at location indicated by dataOffset, into the buffer, starting at the location indicated by bufferOffset.
        /// </summary>
        /// <param name="ordinal">The zero-based column ordinal.</param>
        /// <param name="dataOffset">The index within the row from which to begin the read operation.</param>
        /// <param name="buffer">The buffer into which to copy the data.</param>
        /// <param name="bufferOffset">The index with the buffer to which the data will be copied.</param>
        /// <param name="length">The maximum number of characters to read.</param>
        /// <returns>The actual number of characters read.</returns>
        public override long GetChars(int ordinal, long dataOffset, char[] buffer, int bufferOffset, int length)
        {
            return 0;
        }

        /// <summary>
        /// Gets the name of the data type.
        /// </summary>
        /// <param name="ordinal">The zero-based column ordinal.</param>
        /// <returns>System.String.</returns>
        public override string GetDataTypeName(int ordinal)
        {
            return typeof(string).ToString();
        }

        /// <summary>
        /// Returns a <see cref="string" /> that represents the current row of this instance.
        /// </summary>
        /// <returns>A <see cref="string" /> that represents the current row of this instance.</returns>
        public override string ToString()
        {
            var builder = new StringBuilder();
            for (var i = 0; i < FieldCount; i++)
            {
                builder.AppendFormat("{0}={1}, ", GetName(i), GetValue(i));
            }
            if (builder.Length > 0) { builder.Remove(builder.Length - 2, 2); }
            return builder.ToString();
        }
        #endregion
    }
}