using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Globalization;
using System.Linq;

namespace Cuemon.Data
{
    /// <summary>
    /// Provides a safe way to include a Transact-SQL WHERE clause with an IN operator.
    /// </summary>
    /// <typeparam name="T">The type of the data in the IN operation of the WHERE clause.</typeparam>
    public abstract class InOperator<T>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="InOperator{T}"/> class.
        /// </summary>
        /// <param name="expressions">The expressions to test for a match in the IN operator of the WHERE clause.</param>
        protected InOperator(params T[] expressions) : this(expressions as IEnumerable<T>)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="InOperator{T}"/> class.
        /// </summary>
        /// <param name="expressions">The expressions to test for a match in the IN operator of the WHERE clause.</param>
        protected InOperator(IEnumerable<T> expressions)
        {
            if (expressions == null) { throw new ArgumentNullException(nameof(expressions)); }
            IList<T> elements = expressions as List<T> ?? new List<T>(expressions);
            ParameterName = string.Format(CultureInfo.InvariantCulture, "@param{0}{1}", StringUtility.CreateRandomString(1, StringUtility.EnglishAlphabetCharactersMajuscule), StringUtility.CreateRandomString(5, StringUtility.EnglishAlphabetCharactersMinuscule));
            Arguments = QueryUtility.GetQueryFragment(QueryFormat.Delimited, elements.Select(ArgumentsSelector));
            Parameters = elements.Select(ParametersSelector).ToArray();
        }

        /// <summary>
        /// Gets the arguments for the IN operator.
        /// </summary>
        /// <value>The arguments for the IN operator.</value>
        /// <remarks>Default format of the arguments is @param0, @param1, @param2, etc. and is controlled by the <see cref="ArgumentsSelector"/> method.</remarks>
        public string Arguments { get; private set; }

        /// <summary>
        /// Gets the parameters for the IN operator.
        /// </summary>
        /// <value>The parameters for the IN operator.</value>
        public DbParameter[] Parameters { get; private set; }

        /// <summary>
        /// Gets the name of the parameter that will be concatenated with <c>index</c> of both <see cref="ArgumentsSelector"/> and <see cref="ParametersSelector"/>.
        /// </summary>
        /// <value>The name of the parameter that will be concatenated with <c>index</c>.</value>
        protected string ParameterName { get; private set; }

        /// <summary>
        /// A callback method that is responsible for the values passed to the <see cref="Arguments"/> property.
        /// </summary>
        /// <param name="expression">An expression to test for a match in the IN operator.</param>
        /// <param name="index">The index of the <paramref name="expression"/>.</param>
        /// <returns>A <see cref="string"/> representing the argument of the <paramref name="expression"/>.</returns>
        /// <remarks>Default is @param{index}.</remarks>
        protected virtual string ArgumentsSelector(T expression, int index)
        {
            return string.Concat(ParameterName, index);
        }

        /// <summary>
        /// A callback method that is responsible for the values passed to the <see cref="Parameters"/> property.
        /// </summary>
        /// <param name="expression">An expression to test for a match in the IN operator.</param>
        /// <param name="index">The index of the <paramref name="expression"/>.</param>
        /// <returns>An <see cref="DbParameter"/> representing the value of the <paramref name="expression"/>.</returns>
        protected abstract DbParameter ParametersSelector(T expression, int index);
    }
}