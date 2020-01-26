using System;
using System.Data;

namespace Cuemon.Data
{
    /// <summary>
    /// Represents a Transact-SQL statement or stored procedure to execute against a SQL Server database.
    /// </summary>
    public class DataCommand : IDataCommand
    {
        /// <summary>
		/// Initializes a new instance of the <see cref="DataCommand"/> class.
		/// </summary>
        /// <param name="text">The command text to execute.</param>
        public DataCommand(string text)
        {
            Validator.ThrowIfNullOrWhitespace(text, nameof(text));
            Text = text;
        }

        /// <summary>
        /// Gets or sets the default wait time before terminating the attempt to execute a command and generating an error.
        /// </summary>
        /// <value>
        /// The <see cref="TimeSpan"/> to wait for the command to execute. Default value is 1 minute and 30 seconds.
        /// </value>
        public static TimeSpan DefaultTimeout { get; set; } = TimeSpan.FromSeconds(90);

        /// <summary>
        /// Gets or sets the command text to execute.
        /// </summary>
        /// <value>The command text to execute.</value>
        public string Text { get; set; }

        /// <summary>
        /// Gets or sets the command type value to execute.
        /// </summary>
        /// <value>The command type value to execute. Default type value is <see cref="CommandType.Text"/>.</value>
        public CommandType Type { get; set; } = CommandType.Text;

        /// <summary>
        /// Gets or sets the wait time before terminating the attempt to execute a command and generating an error.
        /// </summary>
        /// <value>The timespan to wait for the command to execute. Default value is 1 minute and 30 seconds.</value>
        public TimeSpan Timeout { get; set; } = DefaultTimeout;
    }
}