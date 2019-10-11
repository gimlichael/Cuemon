using System;
using System.Collections.Specialized;
using System.Globalization;
using System.IO;
using System.Linq;
using Cuemon.Text;

namespace Cuemon.Data
{
    /// <summary>
    /// Provides a way of reading a forward-only stream of rows from a DSV based data source. This class cannot be inherited.
    /// </summary>
    public sealed class DelimiterSeparatedValuesDataReader : DataReader<string[]>
    {
        private int _rowCount;

        /// <summary>
        /// Initializes a new instance of the <see cref="DelimiterSeparatedValuesDataReader"/> class.
        /// </summary>
        /// <param name="reader">The <see cref="StreamReader"/> object that contains the DSV data.</param>
        /// <param name="header">The header defining the columns of the DSV data. Default is reading the first line of the <paramref name="reader"/>.</param>
        /// <param name="delimiter">The delimiter specification. Default is comma (,).</param>
        /// <param name="qualifier">The qualifier specification. Default is double-quote (").</param>
        /// <param name="parser">The function delegate that returns a primitive object whose value is equivalent to the provided <see cref="string"/> value. Default is <see cref="SimpleValueParser.Parse"/>.</param>
        /// <exception cref="ArgumentException">
        /// <paramref name="header"/> cannot be empty or consist only of white-space characters -or-
        /// <paramref name="header"/> does not contain the specified <paramref name="delimiter"/>.
        /// </exception>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="reader"/> is null -or-
        /// <paramref name="delimiter"/> is null -or-
        /// <paramref name="qualifier"/> is null.
        /// </exception>
        public DelimiterSeparatedValuesDataReader(StreamReader reader, string header = null, char delimiter = ',', char qualifier = '"', Func<string, Action<FormattingOptions<CultureInfo>>, object> parser = null) : base(parser ?? ParseFactory.FromSimpleValue().Parse)
        {
            Validator.ThrowIfNull(reader, nameof(reader));
            Validator.ThrowIfNull(delimiter, nameof(delimiter));
            Validator.ThrowIfNull(qualifier, nameof(qualifier));
            if (header == null)
            {
                header = reader.ReadLine();
                Validator.ThrowIfNullOrWhitespace(header, nameof(header));
            }
            if (!header.Contains(delimiter)) { throw new ArgumentException("Header does not contain the specified delimiter."); }
            
            Reader = reader;
            Header = DelimitedString.Split(header, o =>
            {
                o.Delimiter = delimiter.ToString(CultureInfo.InvariantCulture);
                o.Qualifier = qualifier.ToString(CultureInfo.InvariantCulture);
            });
            Delimiter = delimiter;
            Qualifier = qualifier;
        }

        private StreamReader Reader { get; }

        /// <summary>
        /// Gets the delimiter used to separate fields of this instance.
        /// </summary>
        /// <value>The delimiter used to separate fields of this instance.</value>
        public char Delimiter { get; }

        /// <summary>
        /// Gets the header that defines the field names of this instance.
        /// </summary>
        /// <value>The header that defines the field names of this instance.</value>
        public string[] Header { get; }

        /// <summary>
        /// Gets the qualifier that surrounds a field.
        /// </summary>
        /// <value>The qualifier that surrounds a field.</value>
        public char Qualifier { get;  }

        /// <summary>
        /// Gets the currently processed row count of this instance.
        /// </summary>
        /// <value>The currently processed row count of this instance.</value>
        /// <remarks>This property is incremented when the invoked <see cref="Read"/> method returns <c>true</c>.</remarks>
        public override int RowCount => _rowCount;


        /// <summary>
        /// Advances this instance to the next line of the DSV data source.
        /// </summary>
        /// <returns><c>true</c> if there are more lines; otherwise, <c>false</c>.</returns>
        public bool Read()
        {
            if (Disposed) { throw new ObjectDisposedException(GetType().FullName); }
            string line;
            while ((line = Reader.ReadLine()) != null)
            {
                var tb = new TokenBuilder(Delimiter, Qualifier, Header.Length).Append(line);
                while (!tb.IsValid && !Reader.EndOfStream)
                {
                    tb.Append(Reader.ReadLine());
                }
                _rowCount++;
                return ReadNext(DelimitedString.Split(tb.ToString(), o =>
                {
                    o.Delimiter = Delimiter.ToString(CultureInfo.InvariantCulture);
                    o.Qualifier = Qualifier.ToString(CultureInfo.InvariantCulture);
                })) != null;
            }
            return false;
        }

        /// <summary>
        /// Advances this instance to the next record.
        /// </summary>
        /// <returns>A <see cref="T:string[]"/> for as long as there are rows; <see cref="NullRead"/> when no more rows exists.</returns>
        protected override string[] ReadNext(string[] columns = null)
        {
            if (columns != NullRead)
            {
                if (columns.Length != Header.Length) { throw new InvalidOperationException(FormattableString.Invariant($"Line {RowCount + 1} does not match the expected numbers of columns. Actual columns: {columns.Length}. Expected: {Header.Length}.")); }
                var fields = new OrderedDictionary(StringComparer.OrdinalIgnoreCase);
                for (var i = 0; i < columns.Length; i++)
                {
                    if (fields.Contains(Header[i]))
                    {
                        fields[Header[i]] = StringParser(columns[i], null);
                    }
                    else
                    {
                        fields.Add(Header[i], StringParser(columns[i], null));
                    }
                }
                SetFields(fields);
            }
            return columns;
        }

        /// <summary>
        /// Gets the value that indicates that no more rows exists.
        /// </summary>
        /// <value>The value that indicates that no more rows exists.</value>
        protected override string[] NullRead => null;

        /// <summary>
        /// Called when this object is being disposed by either <see cref="Disposable.Dispose()" /> or <see cref="Disposable.Dispose(bool)" /> having <c>disposing</c> set to <c>true</c> and <see cref="Disposable.Disposed" /> is <c>false</c>.
        /// </summary>
        protected override void OnDisposeManagedResources()
        {
            Reader?.Dispose();
        }
    }
}