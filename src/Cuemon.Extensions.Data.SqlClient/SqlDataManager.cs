using System;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Globalization;
using System.Linq;
using Cuemon.Collections.Generic;
using Cuemon.Data;
using Cuemon.Resilience;

namespace Cuemon.Extensions.Data.SqlClient
{
    /// <summary>
    /// The SqlDataManager is the primary class of the <see cref="SqlClient"/> namespace that can be used to execute commands targeted Microsoft SQL Server.
    /// </summary>
    public class SqlDataManager : DataManager
    {
        #region Constructors
        /// <summary>
        /// Initializes a new instance of the <see cref="SqlDataManager"/> class.
        /// Will resolve the default data connection element from the calling application, using the ConfigurationManager to get a CuemonDataSection.
        /// </summary>
        protected SqlDataManager()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SqlDataManager"/> class.
        /// </summary>
        /// <param name="connectionString">The connection string used to establish the connection.</param>
        public SqlDataManager(string connectionString)
        {
            ConnectionString = connectionString;
        }
        #endregion

        #region Properties
        /// <summary>
        /// Gets the string used to open a SQL Server database.
        /// </summary>
        /// <value>The connection string that includes the source database name, and other parameters needed to establish the initial connection.</value>
        public override string ConnectionString { get; }

        /// <summary>
        /// Gets or sets the callback delegate that will provide options for transient fault handling.
        /// </summary>
        /// <value>An <see cref="Action{T}" /> with the options for transient fault handling.</value>
        /// <remarks>
        /// This implementation is compatible with transient related faults on Microsoft SQL Azure.<br/>
        /// Microsoft SQL Server is supported as well.
        /// </remarks>
        public Action<TransientOperationOptions> TransientFaultHandlingOptionsCallback { get; set; } = options =>
        {
            options.EnableRecovery = true;
            options.DetectionStrategy = exception =>
            {
                if (exception == null) { return false; }

                var sqlException = ParseException(exception);
                if (sqlException != null)
                {
                    switch (sqlException.Number)
                    {
                        case -2:
                        case 20:
                        case 64:
                        case 233:
                        case 10053:
                        case 10054:
                        case 10060:
                        case 10928:
                        case 10929:
                        case 40001:
                        case 40143:
                        case 40166:
                        case 40174:
                        case 40197:
                        case 40501:
                        case 40544:
                        case 40549:
                        case 40550:
                        case 40551:
                        case 40552:
                        case 40553:
                        case 40613:
                        case 40615:
                            return true;
                    }
                }

                var fault = exception.Message.StartsWith("Timeout expired.", StringComparison.OrdinalIgnoreCase);
                fault |= exception.Message.IndexOf("The wait operation timed out", StringComparison.OrdinalIgnoreCase) >= 0;
                fault |= exception.Message.IndexOf("The semaphore timeout period has expired", StringComparison.OrdinalIgnoreCase) >= 0;

                return fault;
            };
        };
        #endregion

        #region Methods
        /// <summary>
        /// Executes the command statement and returns an identity value as int.
        /// </summary>
        /// <param name="dataCommand">The data command to execute.</param>
        /// <param name="parameters">The parameters to use in the command.</param>
        /// <returns><see cref="int"/></returns>
        public override int ExecuteIdentityInt32(IDataCommand dataCommand, params DbParameter[] parameters)
        {
            if (dataCommand == null) throw new ArgumentNullException(nameof(dataCommand));
            if (dataCommand.Type != CommandType.Text) { throw new ArgumentException("This method only supports CommandType.Text specifications.", nameof(dataCommand)); }
            return ExecuteScalarAsInt32(new DataCommand(FormattableString.Invariant($"{dataCommand.Text} SELECT CONVERT(INT, SCOPE_IDENTITY())"))
                {
                    Timeout = dataCommand.Timeout
                }, parameters);
        }

        /// <summary>
        /// Executes the command statement and returns an identity value as long.
        /// </summary>
        /// <param name="dataCommand">The data command to execute.</param>
        /// <param name="parameters">The parameters to use in the command.</param>
        /// <returns><see cref="long"/></returns>
        public override long ExecuteIdentityInt64(IDataCommand dataCommand, params DbParameter[] parameters)
        {
            if (dataCommand == null) throw new ArgumentNullException(nameof(dataCommand));
            if (dataCommand.Type != CommandType.Text) { throw new ArgumentException("This method only supports CommandType.Text specifications.", nameof(dataCommand)); }
            return ExecuteScalarAsInt64(new DataCommand(FormattableString.Invariant($"{dataCommand.Text} SELECT CONVERT(BIGINT, SCOPE_IDENTITY())"))
            {
                Timeout = dataCommand.Timeout
            }, parameters);
        }

        /// <summary>
        /// Executes the command statement and returns an identity value as decimal.
        /// </summary>
        /// <param name="dataCommand">The data command to execute.</param>
        /// <param name="parameters">The parameters to use in the command.</param>
        /// <returns><see cref="decimal"/></returns>
        public override decimal ExecuteIdentityDecimal(IDataCommand dataCommand, params DbParameter[] parameters)
        {
            if (dataCommand == null) throw new ArgumentNullException(nameof(dataCommand));
            if (dataCommand.Type != CommandType.Text) { throw new ArgumentException("This method only supports CommandType.Text specifications.", nameof(dataCommand)); }
            return ExecuteScalarAsDecimal(new DataCommand(FormattableString.Invariant($"{dataCommand.Text} SELECT CONVERT(NUMERIC, SCOPE_IDENTITY())"))
            {
                Timeout = dataCommand.Timeout
            }, parameters);
        }

        /// <summary>
        /// Creates a new object that is a copy of the current instance.
        /// </summary>
        /// <returns>
        /// A new object that is a copy of this instance.
        /// </returns>
        public override DataManager Clone()
        {
            return new SqlDataManager(ConnectionString);
        }

        /// <summary>
        /// Core method for executing methods on the <see cref="T:System.Data.Common.DbCommand" /> object resolved from the virtual <see cref="M:Cuemon.Data.DataManager.ExecuteCommandCore(Cuemon.Data.IDataCommand,System.Data.Common.DbParameter[])" /> method.
        /// </summary>
        /// <typeparam name="T">The type to return.</typeparam>
        /// <param name="dataCommand">The data command to execute.</param>
        /// <param name="parameters">The parameters to use in the command.</param>
        /// <param name="commandInvoker">The function delegate that will invoke a method on the resolved <see cref="T:System.Data.Common.DbCommand" /> from the virtual <see cref="M:Cuemon.Data.DataManager.ExecuteCommandCore(Cuemon.Data.IDataCommand,System.Data.Common.DbParameter[])" /> method.</param>
        /// <returns>A value of <typeparamref name="T" /> that is equal to the invoked method of the <see cref="T:System.Data.Common.DbCommand" /> object.</returns>
        /// <remarks>
        /// If <see cref="TransientFaultHandlingOptionsCallback"/> is null, no SQL operation is wrapped inside a transient fault handling operation.
        /// Otherwise, if <see cref="TransientFaultHandlingOptionsCallback"/> has the <see cref="TransientOperationOptions.EnableRecovery"/> set to <c>true</c>, this method will, with it's default implementation, try to gracefully recover from transient faults when the following condition is met:<br/>
        /// <see cref="TransientOperationOptions.RetryAttempts"/> is less than the current attempt starting from 1 with a maximum of <see cref="byte.MaxValue"/> retries<br/>
        /// <see cref="TransientOperationOptions.DetectionStrategy"/> must evaluate to <c>true</c><br/>
        /// In case of a transient failure the default implementation will use <see cref="TransientOperationOptions.RetryStrategy"/>.<br/>
        /// In any other case the originating exception is thrown.
        /// </remarks>
        protected override T ExecuteCore<T>(IDataCommand dataCommand, DbParameter[] parameters, Func<DbCommand, T> commandInvoker)
        {
            return TransientFaultHandlingOptionsCallback == null 
                ? base.ExecuteCore(dataCommand, parameters, commandInvoker) 
                : TransientOperation.WithFunc(() => base.ExecuteCore(dataCommand, parameters, commandInvoker), TransientFaultHandlingOptionsCallback);
        }

        /// <summary>
        /// Gets the command object used by all execute related methods.
        /// </summary>
        /// <param name="dataCommand">The data command to execute.</param>
        /// <param name="parameters">The parameters to use in the command.</param>
        /// <returns></returns>
        protected override DbCommand GetCommandCore(IDataCommand dataCommand, params DbParameter[] parameters)
        {
            if (dataCommand == null) { throw new ArgumentNullException(nameof(dataCommand)); }
            if (parameters == null) { throw new ArgumentNullException(nameof(parameters)); }
            SqlCommand command;
            SqlCommand tempCommand = null;
            try
            {
                tempCommand = new SqlCommand(dataCommand.Text, new SqlConnection(ConnectionString));
                foreach (var parameter in parameters)
                {
                    if (parameter is SqlParameter sqlParameter)
                    {
                        // handle dates so they are compatible with SQL 200X and forward
                        if (sqlParameter.SqlDbType == SqlDbType.SmallDateTime || sqlParameter.SqlDbType == SqlDbType.DateTime)
                        {
                            if (parameter.Value != null && DateTime.TryParse(parameter.Value.ToString(), out var dateTime))
                            {
                                if (dateTime == DateTime.MinValue)
                                {
                                    parameter.Value = sqlParameter.SqlDbType == SqlDbType.DateTime ? DateTime.Parse("1753-01-01", CultureInfo.InvariantCulture) : DateTime.Parse("1900-01-01", CultureInfo.InvariantCulture);
                                }

                                if (dateTime == DateTime.MaxValue && sqlParameter.SqlDbType == SqlDbType.SmallDateTime)
                                {
                                    parameter.Value = DateTime.Parse("2079-06-01", CultureInfo.InvariantCulture);
                                }
                            }
                        }
                    }

                    if (parameter.Value == null) { parameter.Value = DBNull.Value; }
                    tempCommand.Parameters.Add(parameter);
                }
                command = tempCommand;
                tempCommand = null;
            }
            finally
            {
                if (tempCommand != null) { tempCommand.Dispose(); }
            }
            return command;
        }

        private static SqlException ParseException(Exception exception)
        {
            var exceptions = Arguments.Yield(exception).Concat(Decorator.Enclose(exception).Flatten());
            return exceptions.FirstOrDefault(ex => ex is SqlException) as SqlException;
        }
        #endregion
    }
}