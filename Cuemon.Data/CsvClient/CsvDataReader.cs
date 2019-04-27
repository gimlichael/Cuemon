using System;
using System.Collections.Specialized;
using System.Globalization;
using System.IO;

namespace Cuemon.Data.CsvClient
{
    /// <summary>
    /// Provides a way of reading a forward-only stream of rows from a CSV based data source. This class cannot be inherited.
    /// </summary>
    public sealed class CsvDataReader : StringDataReader
    {
        #region Constructors
        /// <summary>
        /// Initializes a new instance of the <see cref="CsvDataReader"/> class.
        /// </summary>
        /// <param name="reader">The <see cref="StreamReader"/> object that contains the CSV data.</param>
        /// <param name="header">The header defining the columns of the CSV data.</param>
        /// <exception cref="ArgumentException">
        /// <paramref name="header"/> does not contain comma (",") as delimiter.
        /// </exception>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="reader"/> is null -or- <paramref name="header"/> is null.
        /// </exception>
        public CsvDataReader(StreamReader reader, string header) : this(reader, header, ",")
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CsvDataReader"/> class.
        /// </summary>
        /// <param name="reader">The <see cref="StreamReader"/> object that contains the CSV data.</param>
        /// <param name="header">The header defining the columns of the CSV data.</param>
        /// <param name="delimiter">The delimiter specification. Default is comma (,).</param>
        public CsvDataReader(StreamReader reader, string header, string delimiter) : this(reader, header, delimiter, "\"")
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CsvDataReader"/> class.
        /// </summary>
        /// <param name="reader">The <see cref="StreamReader"/> object that contains the CSV data.</param>
        /// <param name="header">The header defining the columns of the CSV data.</param>
        /// <param name="delimiter">The delimiter specification. Default is comma (,).</param>
        /// <param name="qualifier">The qualifier specificiation. Default is double-quote (").</param>
        public CsvDataReader(StreamReader reader, string header, string delimiter, string qualifier) : this(reader, header, delimiter, qualifier, ObjectConverter.FromString)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CsvDataReader"/> class.
        /// </summary>
        /// <param name="reader">The <see cref="StreamReader"/> object that contains the CSV data.</param>
        /// <param name="header">The header defining the columns of the CSV data.</param>
        /// <param name="delimiter">The delimiter specification. Default is comma (,).</param>
        /// <param name="qualifier">The qualifier specificiation. Default is double-quote (").</param>
        /// <param name="parser">The function delegate that returns a primitive object whose value is equivalent to the provided <see cref="string"/> value. Default is <see cref="ObjectConverter.FromString(string)"/>.</param>
        /// <exception cref="ArgumentException">
        /// <paramref name="header"/> does not contain the specified <paramref name="delimiter"/> -or-
        /// <paramref name="delimiter"/> is empty or consist only of white-space characters -or-
        /// <paramref name="qualifier"/> is empty or consist only of white-space characters.
        /// </exception>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="reader"/> is null -or-
        /// <paramref name="header"/> is null -or-
        /// <paramref name="delimiter"/> is null -or-
        /// <paramref name="qualifier"/> is null -or-
        /// <paramref name="parser"/> is null.
        /// </exception>
        public CsvDataReader(StreamReader reader, string header, string delimiter, string qualifier, Func<string, object> parser) : base(parser)
        {
            Validator.ThrowIfNull(reader, nameof(reader));
            Validator.ThrowIfNullOrWhitespace(header, nameof(header));
            Validator.ThrowIfNullOrWhitespace(delimiter, nameof(delimiter));
            Validator.ThrowIfNullOrWhitespace(qualifier, nameof(qualifier));
            if (!header.Contains(delimiter)) { throw new ArgumentException("Header does not contain the specified delimiter."); }
            
            Reader = reader;
            Header = StringUtility.SplitDsv(header, delimiter, qualifier);
            Delimiter = delimiter;
            Qualifier = qualifier;
        }
        #endregion

        #region Properties
        private StreamReader Reader { get; }

        /// <summary>
        /// Gets the delimiter used to separate fields of this instance.
        /// </summary>
        /// <value>The delimiter used to separate fields of this instance.</value>
        public string Delimiter { get; }

        /// <summary>
        /// Gets the header that defines the field names of this instance.
        /// </summary>
        /// <value>The header that defines the field names of this instance.</value>
        public string[] Header { get; }

        /// <summary>
        /// Gets the qualifier that surrounds a field.
        /// </summary>
        /// <value>The qualifier that surrounds a field.</value>
        public string Qualifier { get;  }
        #endregion

        #region Methods
        /// <summary>
        /// Advances this instance to the next line of the CSV data source.
        /// </summary>
        /// <returns><c>true</c> if there are more lines; otherwise, <c>false</c>.</returns>
        protected override bool ReadNext()
        {
            return ReadNextCore(Reader.ReadLine());
        }

        private bool ReadNextCore(string currentLine)
        {
            if (currentLine == null) { return false; }
            var columns = StringUtility.SplitDsv(currentLine, Delimiter, Qualifier);
            if (columns.Length != Header.Length)
            {
                var invalidOperation = new InvalidOperationException(string.Format(CultureInfo.InvariantCulture, "The current line does not match the expected numbers of columns. Actual columns: {0}. Expected: {1}.", columns.Length, Header.Length));
                invalidOperation.Data.Add("CsvDataReader.Header", string.Join(Delimiter, Header));
                invalidOperation.Data.Add("CsvDataReader.Columns", currentLine);
                invalidOperation.Data.Add("CsvDataReader.LineNumber", RowCount + 1);
                throw invalidOperation;
            }

            var fields = new OrderedDictionary(StringComparer.OrdinalIgnoreCase);
            for (var i = 0; i < columns.Length; i++)
            {
                if (fields.Contains(Header[i]))
                {
                    fields[Header[i]] = StringParser(columns[i]);
                }
                else
                {
                    fields.Add(Header[i], StringParser(columns[i]));
                }
            }
            SetFields(fields);
            return (fields.Count > 0);
        }

        /// <summary>
        /// Releases unmanaged and - optionally - managed resources.
        /// </summary>
        /// <param name="disposing"><c>true</c> to release both managed and unmanaged resources; <c>false</c> to release only unmanaged resources.</param>
        protected override void Dispose(bool disposing)
        {
            if (IsDisposed) { return; }
            if (disposing)
            {
                if (Reader != null) { Reader.Dispose(); }
            }
            base.Dispose(disposing);
        }

        /// <summary>
        /// Gets a value that indicates whether this <see cref="T:System.Data.Common.DbDataReader" /> contains one or more rows.
        /// </summary>
        /// <value><c>true</c> if this instance has rows; otherwise, <c>false</c>.</value>
        public override bool HasRows { get; }

        #endregion
    }
}