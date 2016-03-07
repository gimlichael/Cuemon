using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Data.Common;
using System.Globalization;
using System.Text;

namespace Cuemon.Data
{
    /// <summary>
    /// Provides a way of copying an existing object implementing the <see cref="DbDataReader"/> interface to a filtered forward-only stream of rows that is mapped for bulk upload. This class cannot be inherited.
    /// </summary>
    public sealed class BulkCopyDataReader : DbDataReader
    {
        private static readonly object PadLock = new object();
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
            foreach (Mapping mapping in Mappings)
            {
                if (string.IsNullOrEmpty(mapping.Source)) { continue; }
                Fields.Add(mapping.Source, null);
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
        /// Gets a value that indicates whether this <see cref="T:System.Data.Common.DbDataReader" /> contains one or more rows.
        /// </summary>
        /// <value><c>true</c> if this instance has rows; otherwise, <c>false</c>.</value>
        public override bool HasRows {
            get { return Reader.HasRows; }
        }

        /// <summary>
        /// Gets a value indicating whether the data reader is closed.
        /// </summary>
        /// <value><c>true</c> if this instance is closed; otherwise, <c>false</c>.</value>
        public override bool IsClosed
        {
            get { return IsDisposed; }
        }

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
        public override int FieldCount
        {
            get { return Mappings.Count; }
        }
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
                            foreach (IndexMapping mapping in Mappings)
                            {
                                _defaultFields.Add(mapping.SourceIndex, null);
                            }
                        }
                        else
                        {
                            foreach (Mapping mapping in Mappings)
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
            OrderedDictionary result = UseOrdinal ? new OrderedDictionary(source.Count, EqualityComparer<int>.Default) : new OrderedDictionary(source.Count, StringComparer.OrdinalIgnoreCase);
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
            if (Reader.Read())
            {
                IOrderedDictionary fields = GetDefault();
                for (int i = 0; i < Reader.FieldCount; i++)
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
                        if (IsMatch(Reader.GetName(i)))
                        {
                            if (fields.Contains(Reader.GetName(i)))
                            {
                                fields[Reader.GetName(i)] = Reader[Reader.GetName(i)];
                            }
                        }
                    }
                }
                RowCount++;
                SetFields(fields);
                return true;
            }
            return false;
        }

        private bool IsMatch(string localName)
        {
            foreach (Mapping mapping in Mappings)
            {
                if (mapping.Source.Equals(localName, StringComparison.OrdinalIgnoreCase)) { return true; }
            }
            return false;
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
        /// Returns an <see cref="T:System.Collections.IEnumerator" /> that can be used to iterate through the rows in the data reader.
        /// </summary>
        /// <returns>An <see cref="T:System.Collections.IEnumerator" /> that can be used to iterate through the rows in the data reader.</returns>
        /// <exception cref="System.NotImplementedException"></exception>
        public override IEnumerator GetEnumerator()
        {
            throw new NotImplementedException();
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
            foreach (Mapping mapping in Mappings)
            {
                if (i == current) { return mapping.Source; }
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
            foreach (Mapping mapping in Mappings)
            {
                if (mapping.Source.Equals(name, StringComparison.OrdinalIgnoreCase)) { return current; }
                current++;
            }
            throw new ArgumentOutOfRangeException(nameof(name), "The name specified name is not a valid column name.");
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
        #endregion
    }
}