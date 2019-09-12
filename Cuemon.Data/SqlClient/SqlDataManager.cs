using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Globalization;
using System.Linq;
using Cuemon.Collections.Generic;

namespace Cuemon.Data.SqlClient
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
        /// This implementation is compatible with transient related faults on Microsoft SQL Azure including the latest addition of error code 10928 and 10929.<br/>
        /// Microsoft SQL Server is supported as well.
        /// </remarks>
        public override Action<TransientOperationOptions> TransientFaultHandlingOptionsCallback { get; set; } = options =>
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
            return ExecuteScalarAsInt32(new DataCommand(string.Format(CultureInfo.InvariantCulture, "{0} {1}",
                dataCommand.Text,
                "SELECT CONVERT(INT, SCOPE_IDENTITY())"),
                dataCommand.Type,
                dataCommand.Timeout), parameters);
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
            return ExecuteScalarAsInt64(new DataCommand(string.Format(CultureInfo.InvariantCulture, "{0} {1}",
                dataCommand.Text,
                "SELECT CONVERT(BIGINT, SCOPE_IDENTITY())"),
                dataCommand.Type,
                dataCommand.Timeout), parameters);
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
            return ExecuteScalarAsDecimal(new DataCommand(string.Format(CultureInfo.InvariantCulture, "{0} {1}",
                dataCommand.Text,
                "SELECT CONVERT(NUMERIC, SCOPE_IDENTITY())"),
                dataCommand.Type,
                dataCommand.Timeout), parameters);
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
                foreach (SqlParameter parameter in parameters) // we use the explicit type, as this is a >Sql<DataManager class
                {
                    // handle dates so they are compatible with SQL 200X and forward
                    if (parameter.SqlDbType == SqlDbType.SmallDateTime || parameter.SqlDbType == SqlDbType.DateTime)
                    {
                        if (parameter.Value != null)
                        {
                            if (DateTime.TryParse(parameter.Value.ToString(), out var dateTime))
                            {
                                if (dateTime == DateTime.MinValue)
                                {
                                    parameter.Value = parameter.SqlDbType == SqlDbType.DateTime ? DateTime.Parse("1753-01-01", CultureInfo.InvariantCulture) : DateTime.Parse("1900-01-01", CultureInfo.InvariantCulture);
                                }

                                if (dateTime == DateTime.MaxValue & parameter.SqlDbType == SqlDbType.SmallDateTime)
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
            var exceptions = Arguments.Yield(exception).Concat(ExceptionUtility.Flatten(exception));
            return exceptions.FirstOrDefault(ex => ex is SqlException) as SqlException;
        }
        #endregion
    }
}