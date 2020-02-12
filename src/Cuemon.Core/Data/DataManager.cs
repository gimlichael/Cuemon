using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Globalization;
using System.IO;
using System.Text;
using Cuemon.Integrity;

namespace Cuemon.Data
{
    /// <summary>
    /// The DataManager is an abstract class in the <see cref="Data"/> namespace that can be used to implement execute commands of different database providers.
    /// </summary>
    public abstract class DataManager
    {
        #region Constructors

        #endregion

        #region Properties
        /// <summary>
        /// Gets the string used to open the connection.
        /// </summary>
        /// <value>The connection string used to establish the initial connection. The exact contents of the connection string depend on the specific data source for this connection.</value>
        public abstract string ConnectionString { get; }

        /// <summary>
        /// Gets or sets the default connection string.
        /// </summary>
        /// <value>The default connection string.</value>
        public static string DefaultConnectionString { get; set; }

        /// <summary>
        /// Gets or sets the callback delegate that will provide options for transient fault handling.
        /// </summary>
        /// <value>An <see cref="Action{T}"/> with the options for transient fault handling.</value>
        public abstract Action<TransientOperationOptions> TransientFaultHandlingOptionsCallback { get; set; }
        #endregion

        #region Methods
        /// <summary>
        /// Parses and returns a <see cref="Type"/> equivalent of <paramref name="dbType"/>.
        /// </summary>
        /// <param name="dbType">The <see cref="DbType"/> to parse.</param>
        /// <returns>A <see cref="Type"/> equivalent of <paramref name="dbType"/>.</returns>
        public static Type ParseDbType(DbType dbType)
        {
            switch (dbType)
            {
                case DbType.Byte:
                case DbType.SByte:
                    return typeof(byte);
                case DbType.Binary:
                    return typeof(byte[]);
                case DbType.Boolean:
                    return typeof(bool);
                case DbType.Currency:
                case DbType.Double:
                    return typeof(double);
                case DbType.Date:
                case DbType.DateTime:
                case DbType.Time:
                case DbType.DateTimeOffset:
                case DbType.DateTime2:
                    return typeof(DateTime);
                case DbType.Guid:
                    return typeof(Guid);
                case DbType.Int64:
                    return typeof(long);
                case DbType.Int32:
                    return typeof(int);
                case DbType.Int16:
                    return typeof(short);
                case DbType.Object:
                    return typeof(object);
                case DbType.AnsiString:
                case DbType.AnsiStringFixedLength:
                case DbType.StringFixedLength:
                case DbType.String:
                    return typeof(string);
                case DbType.Single:
                    return typeof(float);
                case DbType.UInt64:
                    return typeof(ulong);
                case DbType.UInt32:
                    return typeof(uint);
                case DbType.UInt16:
                    return typeof(ushort);
                case DbType.Decimal:
                case DbType.VarNumeric:
                    return typeof(decimal);
                case DbType.Xml:
                    return typeof(string);
            }
            throw new ArgumentOutOfRangeException(nameof(dbType), string.Format(CultureInfo.InvariantCulture, "Type, '{0}', is unsupported.", dbType));
        }

        /// <summary>
        /// Creates and returns a sequence of column names resolved from the specified <paramref name="reader"/>.
        /// </summary>
        /// <param name="reader">The reader to resolve column names from.</param>
        /// <returns>A sequence of column names resolved from the specified <paramref name="reader"/>.</returns>
        public static IEnumerable<string> GetReaderColumnNames(DbDataReader reader)
        {
            if (reader == null) { throw new ArgumentNullException(nameof(reader)); }
            if (reader.IsClosed) { throw new ArgumentException("Reader is closed.", nameof(reader)); }
            var columns = GetReaderColumns(reader);
            foreach (var column in columns)
            {
                yield return column.Key;
            }
        }

        /// <summary>
        /// Creates and returns a sequence of column values resolved from the specified <paramref name="reader"/>.
        /// </summary>
        /// <param name="reader">The reader to resolve column values from.</param>
        /// <returns>A sequence of column values resolved from the specified <paramref name="reader"/>.</returns>
        public static IEnumerable<object> GetReaderColumnValues(DbDataReader reader)
        {
            if (reader == null) { throw new ArgumentNullException(nameof(reader)); }
            if (reader.IsClosed) { throw new ArgumentException("Reader is closed.", nameof(reader)); }
            var columns = GetReaderColumns(reader);
            foreach (var column in columns)
            {
                yield return column.Value;
            }
        }

        /// <summary>
        /// Creates and returns a <see cref="KeyValuePair{TKey,TValue}"/> sequence of column names and values resolved from the specified <paramref name="reader"/>.
        /// </summary>
        /// <param name="reader">The reader to resolve column names and values from.</param>
        /// <returns>A <see cref="KeyValuePair{TKey,TValue}"/> sequence of column names and values resolved from the specified <paramref name="reader"/>.</returns>
        public static IEnumerable<KeyValuePair<string, object>> GetReaderColumns(DbDataReader reader)
        {
            if (reader == null) { throw new ArgumentNullException(nameof(reader)); }
            if (reader.IsClosed) { throw new ArgumentException("Reader is closed.", nameof(reader)); }
            for (var f = 0; f < reader.FieldCount; f++)
            {
                yield return new KeyValuePair<string, object>(reader.GetName(f), reader.GetValue(f));
            }
        }

        /// <summary>
        /// Converts the given <see cref="DbDataReader"/> compatible object to a stream.
        /// Note: DbDataReader must return only one field (for instance, a XML field), otherwise an exception is thrown!
        /// </summary>
        /// <param name="value">The <see cref="DbDataReader"/> to build a stream from.</param>
        /// <returns>A <b><see cref="Stream"/></b> object.</returns>
        public static Stream ReaderToStream(DbDataReader value)
        {
            if (value == null) { throw new ArgumentNullException(nameof(value)); }
            Stream stream;
            Stream tempStream = null;
            try
            {
                if (value.FieldCount > 1)
                {
                    throw new InvalidDataException(string.Format(CultureInfo.InvariantCulture,
                        "The executed command statement appears to contain invalid fields. Expected field count is 1. Actually field count was {0}.",
                        value.FieldCount));
                }

                tempStream = new MemoryStream();
                while (value.Read())
                {
                    var bytes = Convertible.GetBytes(value.GetString(0));
                    tempStream.Write(bytes, 0, bytes.Length);
                }
                tempStream.Position = 0;
                stream = tempStream;
                tempStream = null;
            }
            finally
            {
                if (tempStream != null) { tempStream.Dispose(); }
            }
            return stream;
        }


        /// <summary>
        /// Converts the given <see cref="DbDataReader"/> compatible object to a string.
        /// Note: DbDataReader must return only one field, otherwise an exception is thrown!
        /// </summary>
        /// <param name="value">The <see cref="DbDataReader"/> to build a string from.</param>
        /// <returns>A <b><see cref="string"/></b> object.</returns>
        public static string ReaderToString(DbDataReader value)
        {
            if (value == null) { throw new ArgumentNullException(nameof(value)); }
            if (value.FieldCount > 1)
            {
                throw new InvalidDataException(string.Format(CultureInfo.InvariantCulture,
                    "The executed command statement appears to contain invalid fields. Expected field count is 1. Actually field count was {0}.",
                    value.FieldCount));
            }
            var stringBuilder = new StringBuilder();
            while (value.Read())
            {
                stringBuilder.Append(value.GetString(0));
            }
            return stringBuilder.ToString();
        }

        /// <summary>
        /// Creates a new object that is a copy of the current instance.
        /// </summary>
        /// <returns>
        /// A new object that is a copy of this instance.
        /// </returns>
        public abstract DataManager Clone();

        /// <summary>
        /// Executes the command statement and returns the number of rows affected.
        /// </summary>
        /// <param name="dataCommand">The data command to execute.</param>
        /// <param name="parameters">The parameters to use in the command.</param>
        /// <returns>
        /// A <b><see cref="int"/></b> value.
        /// </returns>
        public int Execute(IDataCommand dataCommand, params DbParameter[] parameters)
        {
            return ExecuteCore(dataCommand, parameters, dbCommand =>
            {
                try
                {
                    return dbCommand.ExecuteNonQuery();
                }
                finally
                {
                    if (dbCommand.Connection.State != ConnectionState.Closed) { dbCommand.Connection.Close(); }
                }
            });
        }

        /// <summary>
        /// Executes the command statement and returns <c>true</c> if one or more records exists; otherwise <c>false</c>.
        /// </summary>
        /// <param name="dataCommand">The data command to execute.</param>
        /// <param name="parameters">The parameters to use in the command.</param>
        /// <returns>
        /// A <b><see cref="bool"/></b> value.
        /// </returns>
        public bool ExecuteExists(IDataCommand dataCommand, params DbParameter[] parameters)
        {
            using (var reader = ExecuteReader(dataCommand, parameters))
            {
                return reader.Read();
            }
        }

        /// <summary>
        /// Executes the command statement and returns an identity value as int.
        /// </summary>
        /// <param name="dataCommand">The data command to execute.</param>
        /// <param name="parameters">The parameters to use in the command.</param>
        /// <returns><see cref="int"/></returns>
        public abstract int ExecuteIdentityInt32(IDataCommand dataCommand, params DbParameter[] parameters);

        /// <summary>
        /// Executes the command statement and returns an identity value as long.
        /// </summary>
        /// <param name="dataCommand">The data command to execute.</param>
        /// <param name="parameters">The parameters to use in the command.</param>
        /// <returns><see cref="long"/></returns>
        public abstract long ExecuteIdentityInt64(IDataCommand dataCommand, params DbParameter[] parameters);

        /// <summary>
        /// Executes the command statement and returns an identity value as decimal.
        /// </summary>
        /// <param name="dataCommand">The data command to execute.</param>
        /// <param name="parameters">The parameters to use in the command.</param>
        /// <returns><see cref="decimal"/></returns>
        public abstract decimal ExecuteIdentityDecimal(IDataCommand dataCommand, params DbParameter[] parameters);

        /// <summary>
        /// Executes the command statement and returns an object supporting the DbDataReader interface.
        /// </summary>
        /// <param name="dataCommand">The data command to execute.</param>
        /// <param name="parameters">The parameters to use in the command.</param>
        /// <returns>
        /// An object supporting the <b><see cref="DbDataReader"/></b> interface.
        /// </returns>
        public DbDataReader ExecuteReader(IDataCommand dataCommand, params DbParameter[] parameters)
        {
            return ExecuteCore(dataCommand, parameters, dbCommand => dbCommand.ExecuteReader(CommandBehavior.CloseConnection));
        }

        /// <summary>
        /// Executes the command statement and returns a string object with the retrieved XML.
        /// </summary>
        /// <param name="dataCommand">The data command to execute.</param>
        /// <param name="parameters">The parameters to use in the command.</param>
        /// <returns>
        /// An <b><see cref="string"/></b> object.
        /// </returns>
        public virtual string ExecuteXmlString(IDataCommand dataCommand, params DbParameter[] parameters)
        {
            using (var reader = ExecuteReader(dataCommand, parameters))
            {
                return ReaderToString(reader);
            }
        }

        /// <summary>
        /// Executes the command statement, and returns the value from the first column of the first row in the result set.
        /// Additional columns or rows are ignored.
        /// </summary>
        /// <param name="dataCommand">The data command to execute.</param>
        /// <param name="parameters">The parameters to use in the command.</param>
        /// <returns>The first column of the first row in the result from <paramref name="dataCommand"/>.</returns>
        public object ExecuteScalar(IDataCommand dataCommand, params DbParameter[] parameters)
        {
            return ExecuteCore(dataCommand, parameters, dbCommand =>
            {
                try
                {
                    return dbCommand.ExecuteScalar();
                }
                finally
                {
                    if (dbCommand.Connection.State != ConnectionState.Closed) { dbCommand.Connection.Close(); }
                }
            });
        }

        /// <summary>
        /// Executes the command statement, and returns the value as the specified <paramref name="returnType"/> from the first column of the first row in the result set.
        /// Additional columns or rows are ignored.
        /// </summary>
        /// <param name="dataCommand">The data command to execute.</param>
        /// <param name="returnType">The type to return the first column value as.</param>
        /// <param name="parameters">The parameters to use in the command.</param>
        /// <returns>The first column of the first row in the result from <paramref name="dataCommand"/> as the specified <paramref name="returnType"/>.</returns>
        /// <remarks>This method uses <see cref="CultureInfo.InvariantCulture"/> when casting the first column of the first row in the result from <paramref name="dataCommand"/>.</remarks>
        public object ExecuteScalarAsType(IDataCommand dataCommand, Type returnType, params DbParameter[] parameters)
        {
            return ExecuteScalarAsType(dataCommand, returnType, CultureInfo.InvariantCulture, parameters);
        }

        /// <summary>
        /// Executes the command statement, and returns the value as the specified <paramref name="returnType"/> from the first column of the first row in the result set.
        /// Additional columns or rows are ignored.
        /// </summary>
        /// <param name="dataCommand">The data command to execute.</param>
        /// <param name="returnType">The type to return the first column value as.</param>
        /// <param name="provider">An object that supplies culture-specific formatting information.</param>
        /// <param name="parameters">The parameters to use in the command.</param>
        /// <returns>The first column of the first row in the result from <paramref name="dataCommand"/> as the specified <paramref name="returnType"/>.</returns>
        public object ExecuteScalarAsType(IDataCommand dataCommand, Type returnType, IFormatProvider provider, params DbParameter[] parameters)
        {
            return ConvertFactory.FromObject().ChangeType(ExecuteScalar(dataCommand, parameters), returnType, o => o.FormatProvider = provider);
        }

        /// <summary>
        /// Executes the command statement, and returns the value as <typeparamref name="TResult" /> from the first column of the first row in the result set.
        /// Additional columns or rows are ignored.
        /// </summary>
        /// <typeparam name="TResult">The type of the return value.</typeparam>
        /// <param name="dataCommand">The data command to execute.</param>
        /// <param name="parameters">The parameters to use in the command.</param>
        /// <returns>The first column of the first row in the result from <paramref name="dataCommand" /> as <typeparamref name="TResult"/>.</returns>
        /// <remarks>This method uses <see cref="CultureInfo.InvariantCulture"/> when casting the first column of the first row in the result from <paramref name="dataCommand"/>.</remarks>
        public TResult ExecuteScalarAs<TResult>(IDataCommand dataCommand, params DbParameter[] parameters)
        {
            return (TResult)ExecuteScalarAsType(dataCommand, typeof(TResult), parameters);
        }

        /// <summary>
        /// Executes the command statement, and returns the value as <typeparamref name="TResult" /> from the first column of the first row in the result set.
        /// Additional columns or rows are ignored.
        /// </summary>
        /// <typeparam name="TResult">The type of the return value.</typeparam>
        /// <param name="dataCommand">The data command to execute.</param>
        /// <param name="provider">An object that supplies culture-specific formatting information.</param>
        /// <param name="parameters">The parameters to use in the command.</param>
        /// <returns>The first column of the first row in the result from <paramref name="dataCommand" /> as <typeparamref name="TResult"/>.</returns>
        public TResult ExecuteScalarAs<TResult>(IDataCommand dataCommand, IFormatProvider provider, params DbParameter[] parameters)
        {
            return (TResult)ExecuteScalarAsType(dataCommand, typeof(TResult), provider, parameters);
        }

        /// <summary>
        /// Executes the command statement, and returns the value as <see cref="bool"/> from the first column of the first row in the result set.
        /// Additional columns or rows are ignored.
        /// </summary>
        /// <param name="dataCommand">The data command to execute.</param>
        /// <param name="parameters">The parameters to use in the command.</param>
        /// <returns>The first column of the first row in the result from <paramref name="dataCommand" /> as <see cref="bool"/>.</returns>
        public bool ExecuteScalarAsBoolean(IDataCommand dataCommand, params DbParameter[] parameters)
        {
            return ExecuteScalarAs<bool>(dataCommand, parameters);
        }


        /// <summary>
        /// Executes the command statement, and returns the value as <see cref="DateTime"/> from the first column of the first row in the result set.
        /// Additional columns or rows are ignored.
        /// </summary>
        /// <param name="dataCommand">The data command to execute.</param>
        /// <param name="parameters">The parameters to use in the command.</param>
        /// <returns>The first column of the first row in the result from <paramref name="dataCommand" /> as <see cref="DateTime"/>.</returns>
        public DateTime ExecuteScalarAsDateTime(IDataCommand dataCommand, params DbParameter[] parameters)
        {
            return ExecuteScalarAs<DateTime>(dataCommand, parameters);
        }

        /// <summary>
        /// Executes the command statement, and returns the value as <see cref="short"/> from the first column of the first row in the result set.
        /// Additional columns or rows are ignored.
        /// </summary>
        /// <param name="dataCommand">The data command to execute.</param>
        /// <param name="parameters">The parameters to use in the command.</param>
        /// <returns>The first column of the first row in the result from <paramref name="dataCommand" /> as <see cref="short"/>.</returns>
        public short ExecuteScalarAsInt16(IDataCommand dataCommand, params DbParameter[] parameters)
        {
            return ExecuteScalarAs<short>(dataCommand, parameters);
        }

        /// <summary>
        /// Executes the command statement, and returns the value as <see cref="int"/> from the first column of the first row in the result set.
        /// Additional columns or rows are ignored.
        /// </summary>
        /// <param name="dataCommand">The data command to execute.</param>
        /// <param name="parameters">The parameters to use in the command.</param>
        /// <returns>The first column of the first row in the result from <paramref name="dataCommand" /> as <see cref="int"/>.</returns>
        public int ExecuteScalarAsInt32(IDataCommand dataCommand, params DbParameter[] parameters)
        {
            return ExecuteScalarAs<int>(dataCommand, parameters);
        }

        /// <summary>
        /// Executes the command statement, and returns the value as <see cref="long"/> from the first column of the first row in the result set.
        /// Additional columns or rows are ignored.
        /// </summary>
        /// <param name="dataCommand">The data command to execute.</param>
        /// <param name="parameters">The parameters to use in the command.</param>
        /// <returns>The first column of the first row in the result from <paramref name="dataCommand" /> as <see cref="long"/>.</returns>
        public long ExecuteScalarAsInt64(IDataCommand dataCommand, params DbParameter[] parameters)
        {
            return ExecuteScalarAs<long>(dataCommand, parameters);
        }

        /// <summary>
        /// Executes the command statement, and returns the value as <see cref="byte"/> from the first column of the first row in the result set.
        /// Additional columns or rows are ignored.
        /// </summary>
        /// <param name="dataCommand">The data command to execute.</param>
        /// <param name="parameters">The parameters to use in the command.</param>
        /// <returns>The first column of the first row in the result from <paramref name="dataCommand" /> as <see cref="byte"/>.</returns>
        public byte ExecuteScalarAsByte(IDataCommand dataCommand, params DbParameter[] parameters)
        {
            return ExecuteScalarAs<byte>(dataCommand, parameters);
        }

        /// <summary>
        /// Executes the command statement, and returns the value as <see cref="sbyte"/> from the first column of the first row in the result set.
        /// Additional columns or rows are ignored.
        /// </summary>
        /// <param name="dataCommand">The data command to execute.</param>
        /// <param name="parameters">The parameters to use in the command.</param>
        /// <returns>The first column of the first row in the result from <paramref name="dataCommand" /> as <see cref="sbyte"/>.</returns>
        public sbyte ExecuteScalarAsSByte(IDataCommand dataCommand, params DbParameter[] parameters)
        {
            return ExecuteScalarAs<sbyte>(dataCommand, parameters);
        }

        /// <summary>
        /// Executes the command statement, and returns the value as <see cref="decimal"/> from the first column of the first row in the result set.
        /// Additional columns or rows are ignored.
        /// </summary>
        /// <param name="dataCommand">The data command to execute.</param>
        /// <param name="parameters">The parameters to use in the command.</param>
        /// <returns>The first column of the first row in the result from <paramref name="dataCommand" /> as <see cref="decimal"/>.</returns>
        public decimal ExecuteScalarAsDecimal(IDataCommand dataCommand, params DbParameter[] parameters)
        {
            return ExecuteScalarAs<decimal>(dataCommand, parameters);
        }

        /// <summary>
        /// Executes the command statement, and returns the value as <see cref="double"/> from the first column of the first row in the result set.
        /// Additional columns or rows are ignored.
        /// </summary>
        /// <param name="dataCommand">The data command to execute.</param>
        /// <param name="parameters">The parameters to use in the command.</param>
        /// <returns>The first column of the first row in the result from <paramref name="dataCommand" /> as <see cref="double"/>.</returns>
        public double ExecuteScalarAsDouble(IDataCommand dataCommand, params DbParameter[] parameters)
        {
            return ExecuteScalarAs<double>(dataCommand, parameters);
        }

        /// <summary>
        /// Executes the command statement, and returns the value as <see cref="ushort"/> from the first column of the first row in the result set.
        /// Additional columns or rows are ignored.
        /// </summary>
        /// <param name="dataCommand">The data command to execute.</param>
        /// <param name="parameters">The parameters to use in the command.</param>
        /// <returns>The first column of the first row in the result from <paramref name="dataCommand" /> as <see cref="ushort"/>.</returns>
        public ushort ExecuteScalarAsUInt16(IDataCommand dataCommand, params DbParameter[] parameters)
        {
            return ExecuteScalarAs<ushort>(dataCommand, parameters);
        }

        /// <summary>
        /// Executes the command statement, and returns the value as <see cref="uint"/> from the first column of the first row in the result set.
        /// Additional columns or rows are ignored.
        /// </summary>
        /// <param name="dataCommand">The data command to execute.</param>
        /// <param name="parameters">The parameters to use in the command.</param>
        /// <returns>The first column of the first row in the result from <paramref name="dataCommand" /> as <see cref="uint"/>.</returns>
        public uint ExecuteScalarAsUInt32(IDataCommand dataCommand, params DbParameter[] parameters)
        {
            return ExecuteScalarAs<uint>(dataCommand, parameters);
        }

        /// <summary>
        /// Executes the command statement, and returns the value as <see cref="ulong"/> from the first column of the first row in the result set.
        /// Additional columns or rows are ignored.
        /// </summary>
        /// <param name="dataCommand">The data command to execute.</param>
        /// <param name="parameters">The parameters to use in the command.</param>
        /// <returns>The first column of the first row in the result from <paramref name="dataCommand" /> as <see cref="ulong"/>.</returns>
        public ulong ExecuteScalarAsUInt64(IDataCommand dataCommand, params DbParameter[] parameters)
        {
            return ExecuteScalarAs<ulong>(dataCommand, parameters);
        }

        /// <summary>
        /// Executes the command statement, and returns the value as <see cref="string"/> from the first column of the first row in the result set.
        /// Additional columns or rows are ignored.
        /// </summary>
        /// <param name="dataCommand">The data command to execute.</param>
        /// <param name="parameters">The parameters to use in the command.</param>
        /// <returns>The first column of the first row in the result from <paramref name="dataCommand" /> as <see cref="string"/>.</returns>
        public string ExecuteScalarAsString(IDataCommand dataCommand, params DbParameter[] parameters)
        {
            return ExecuteScalarAs<string>(dataCommand, parameters);
        }


        /// <summary>
        /// Executes the command statement, and returns the value as <see cref="Guid"/> from the first column of the first row in the result set.
        /// Additional columns or rows are ignored.
        /// </summary>
        /// <param name="dataCommand">The data command to execute.</param>
        /// <param name="parameters">The parameters to use in the command.</param>
        /// <returns>The first column of the first row in the result from <paramref name="dataCommand" /> as <see cref="Guid"/>.</returns>
        public Guid ExecuteScalarAsGuid(IDataCommand dataCommand, params DbParameter[] parameters)
        {
            return ExecuteScalarAs<Guid>(dataCommand, parameters);
        }

        /// <summary>
        /// Core method for executing methods on the <see cref="DbCommand"/> object resolved from the virtual <see cref="ExecuteCommandCore"/> method.
        /// </summary>
        /// <typeparam name="T">The type to return.</typeparam>
        /// <param name="dataCommand">The data command to execute.</param>
        /// <param name="parameters">The parameters to use in the command.</param>
        /// <param name="commandInvoker">The function delegate that will invoke a method on the resolved <see cref="DbCommand"/> from the virtual <see cref="ExecuteCommandCore"/> method.</param>
        /// <returns>A value of <typeparamref name="T"/> that is equal to the invoked method of the <see cref="DbCommand"/> object.</returns>
        protected virtual T ExecuteCore<T>(IDataCommand dataCommand, DbParameter[] parameters, Func<DbCommand, T> commandInvoker)
        {
            return TransientOperation.WithFunc(() => InvokeCommandCore(dataCommand, parameters, commandInvoker), TransientFaultHandlingOptionsCallback);
        }

        private T InvokeCommandCore<T>(IDataCommand dataCommand, DbParameter[] parameters, Func<DbCommand, T> sqlInvoker)
        {
            T result;
            DbCommand command = null;
            try
            {
                using (command = ExecuteCommandCore(dataCommand, parameters))
                {
                    result = sqlInvoker(command);
                }
            }
            finally
            {
                if (command != null) { command.Parameters.Clear(); }
            }
            return result;
        }

        /// <summary>
        /// Core method for executing all commands.
        /// </summary>
        /// <param name="dataCommand">The data command to execute.</param>
        /// <param name="parameters">The parameters to use in the command.</param>
        /// <returns>System.Data.Common.DbCommand</returns>
        /// <remarks>
        /// If <see cref="TransientFaultHandlingOptionsCallback"/> has the <see cref="TransientOperationOptions.EnableRecovery"/> set to <c>true</c>, this method will with it's default implementation try to gracefully recover from transient faults when the following condition is met:<br/>
        /// <see cref="TransientOperationOptions.RetryAttempts"/> is less than the current attempt starting from 1 with a maximum of <see cref="byte.MaxValue"/> retries<br/>
        /// <see cref="TransientOperationOptions.DetectionStrategy"/> must evaluate to <c>true</c><br/>
        /// In case of a transient failure the default implementation will use <see cref="TransientOperationOptions.RetryStrategy"/>.<br/>
        /// In any other case the originating exception is thrown.
        /// </remarks>
        protected virtual DbCommand ExecuteCommandCore(IDataCommand dataCommand, params DbParameter[] parameters)
        {
            if (dataCommand == null) throw new ArgumentNullException(nameof(dataCommand));
            DbCommand command = null;
            try
            {
                command = GetCommandCore(dataCommand, parameters);
                command.CommandTimeout = (int)dataCommand.Timeout.TotalSeconds;
                OpenConnection(command);
            }
            catch (Exception)
            {
                if (command != null) { command.Parameters.Clear(); }
                throw;
            }
            return command;
        }

        private void OpenConnection(DbCommand command)
        {
            if (command == null) { throw new ArgumentNullException(nameof(command)); }
            if (command.Connection == null) { throw new ArgumentNullException(nameof(command), "No connection was set for this command object."); }
            if (command.Connection.State != ConnectionState.Open) { command.Connection.Open(); }
        }

        /// <summary>
        /// Gets the command object to be used by all execute related methods.
        /// </summary>
        /// <param name="dataCommand">The data command to execute.</param>
        /// <param name="parameters">The parameters to use in the command.</param>
        /// <returns></returns>
        protected abstract DbCommand GetCommandCore(IDataCommand dataCommand, params DbParameter[] parameters);
        #endregion
    }
}