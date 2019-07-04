using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Cuemon.Data
{
    public sealed class ConcurrentDelimiterSeparatedValuesDataReader : DataReader<string[]>
    {
        private int _rowCount = 0;

        /// <summary>
        /// Initializes a new instance of the <see cref="DelimiterSeparatedValuesDataReader"/> class.
        /// </summary>
        /// <param name="reader">The <see cref="StreamReader"/> object that contains the DSV data.</param>
        /// <param name="header">The header defining the columns of the DSV data. Default is reading the first line of the <paramref name="reader"/>.</param>
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
        public ConcurrentDelimiterSeparatedValuesDataReader(StreamReader reader, string header = null, char delimiter = ',', char qualifier = '"', Func<string, Action<FormattingOptions<CultureInfo>>, object> parser = null) : base(parser)
        {
            Validator.ThrowIfNull(reader, nameof(reader));

            if (header == null)
            {
                header = reader.ReadLine();
                Validator.ThrowIfNull(header, nameof(header));
            }

            if (!header.Contains(delimiter)) { throw new ArgumentException("Header does not contain the specified delimiter."); }

            Reader = reader;
            Header = StringUtility.SplitDsv(header, delimiter.ToString(CultureInfo.InvariantCulture), qualifier.ToString(CultureInfo.InvariantCulture));
            Delimiter = delimiter;
            Qualifier = qualifier;
            var headerFields = new OrderedDictionary();
            foreach (var hf in Header) { headerFields.Add(hf, null); }
            SetFields(headerFields);
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
        public char Qualifier { get; }

        public override int RowCount => _rowCount;

        protected override string[] NullRead => null;

        protected override void OnDisposeManagedResources()
        {
            Reader?.Dispose();
            Ss?.Dispose();
        }

        private static readonly SemaphoreSlim Ss = new SemaphoreSlim(1);

        public async Task<bool> ReadAsync()
        {
            await Ss.WaitAsync();
            try
            {
                if (Disposed) { throw new ObjectDisposedException(GetType().FullName); }
                string line;
                while ((line = await Reader.ReadLineAsync()) != null)
                {

                    var tb = new TokenBuilder(Delimiter, Qualifier, Header.Length).Append(line);
                    while (!tb.IsValid && !Reader.EndOfStream)
                    {
                        tb.Append(await Reader.ReadLineAsync());
                    }
                    Interlocked.Increment(ref _rowCount);
                    return ReadNext(StringUtility.SplitDsv(tb.ToString(), Delimiter.ToString(CultureInfo.InvariantCulture), Qualifier.ToString(CultureInfo.InvariantCulture))) != null;
                }
                return false;
            }
            finally
            {
               Ss.Release();
            }
        }


        protected override string[] ReadNext(string[] columns = null)
        {
            if (columns != null)
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
    }
}
