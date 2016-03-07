using System.Collections.Generic;
using System.Data.Common;
using System.Data.SqlClient;

namespace Cuemon.Data.SqlClient
{
    /// <summary>
    /// Provides a safe way to include a Transact-SQL WHERE clause with an IN operator to execute against a SQL Server database.
    /// </summary>
    /// <typeparam name="T">The type of the data in the IN operation of the WHERE clause to execute against a SQL Server database.</typeparam>
    public sealed class SqlInOperator<T> : InOperator<T>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="InOperator{T}"/> class.
        /// </summary>
        /// <param name="expressions">The expressions to test for a match in the IN operator of the WHERE clause.</param>
        public SqlInOperator(params T[] expressions) : base(expressions)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="InOperator{T}"/> class.
        /// </summary>
        /// <param name="expressions">The expressions to test for a match in the IN operator of the WHERE clause.</param>
        public SqlInOperator(IEnumerable<T> expressions) : base(expressions)
        {
        }


        /// <summary>
        /// A callback method that is responsible for the values passed to the <see cref="InOperator{T}.Parameters"/> property.
        /// </summary>
        /// <param name="expression">An expression to test for a match in the IN operator.</param>
        /// <param name="index">The index of the <paramref name="expression"/>.</param>
        /// <returns>An <see cref="DbParameter"/> representing the value of the <paramref name="expression"/>.</returns>
        protected override DbParameter ParametersSelector(T expression, int index)
        {
            return new SqlParameter(string.Concat(this.ParameterName, index), expression);
        }
    }
}