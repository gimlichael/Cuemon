using System;
using System.Data;
using System.Globalization;

namespace Cuemon.Data
{
    /// <summary>
    /// Represents a Transact-SQL statement or stored procedure to execute against a SQL Server database.
    /// </summary>
    public class DataCommand : IDataCommand
    {
        private string _text;
        private CommandType _type = CommandType.Text;
        private static TimeSpan _timeout = TimeSpan.FromSeconds(90);

        #region Constructors
        /// <summary>
        /// Initializes a new instance of the <see cref="DataCommand"/> class.
        /// </summary>
        public DataCommand()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="DataCommand"/> class.
        /// </summary>
        /// <param name="text">The command text to execute.</param>
        public DataCommand(string text) : this(text, null)
        {
        }

		/// <summary>
		/// Initializes a new instance of the <see cref="DataCommand"/> class.
		/// </summary>
		/// <param name="textFormat">The command text to execute as a composite format string (see Remarks).</param>
		/// <param name="args">An object array that contains zero or more objects to format.</param>
		/// <remarks>
		/// For more information regarding the <paramref name="textFormat"/>, have a look here: http://msdn.microsoft.com/en-us/library/txafckwd(v=vs.80).aspx.
		/// </remarks>
		public DataCommand(string textFormat, params object[] args) : this(textFormat, DefaultTimeout,  args)
		{
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="DataCommand"/> class.
		/// </summary>
		/// <param name="text">The command text to execute.</param>
		/// <param name="timeout">The timeout for the command to execute.</param>
		public DataCommand(string text, TimeSpan timeout) : this(text, timeout, null)
		{
		}

        /// <summary>
        /// Initializes a new instance of the <see cref="DataCommand"/> class.
        /// </summary>
		/// <param name="textFormat">The command text to execute as a composite format string (see Remarks).</param>
        /// <param name="timeout">The timeout for the command to execute.</param>
		/// <param name="args">An object array that contains zero or more objects to format.</param>
		/// <remarks>
		/// For more information regarding the <paramref name="textFormat"/>, have a look here: http://msdn.microsoft.com/en-us/library/txafckwd(v=vs.80).aspx.
		/// </remarks>
		public DataCommand(string textFormat, TimeSpan timeout, params object[] args) : this(textFormat, CommandType.Text, timeout, args)
        {
        }

		/// <summary>
		/// Initializes a new instance of the <see cref="DataCommand"/> class.
		/// </summary>
		/// <param name="text">The command text to execute.</param>
		/// <param name="type">The command type value to execute.</param>
		public DataCommand(string text, CommandType type) : this(text, type, null)
		{
		}

        /// <summary>
        /// Initializes a new instance of the <see cref="DataCommand"/> class.
        /// </summary>
		/// <param name="textFormat">The command text to execute as a composite format string (see Remarks).</param>
        /// <param name="type">The command type value to execute.</param>
		/// <param name="args">An object array that contains zero or more objects to format.</param>
		/// <remarks>
		/// For more information regarding the <paramref name="textFormat"/>, have a look here: http://msdn.microsoft.com/en-us/library/txafckwd(v=vs.80).aspx.
		/// </remarks>
		public DataCommand(string textFormat, CommandType type, params object[] args) : this(textFormat, type, DefaultTimeout, args)
        {
        }

		/// <summary>
		/// Initializes a new instance of the <see cref="DataCommand"/> class.
		/// </summary>
		/// <param name="text">The command text to execute.</param>
		/// <param name="type">The command type value to execute.</param>
		/// <param name="timeout">The timeout for the command to execute.</param>
		public DataCommand(string text, CommandType type, TimeSpan timeout) : this(text, type, timeout, null)
		{
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="DataCommand"/> class.
		/// </summary>
		/// <param name="textFormat">The command text to execute as a composite format string (see Remarks).</param>
		/// <param name="type">The command type value to execute.</param>
		/// <param name="timeout">The timeout for the command to execute.</param>
		/// <param name="args">An object array that contains zero or more objects to format.</param>
		/// <remarks>
		/// For more information regarding the <paramref name="textFormat"/>, have a look here: http://msdn.microsoft.com/en-us/library/txafckwd(v=vs.80).aspx.
		/// </remarks>
        public DataCommand(string textFormat, CommandType type, TimeSpan timeout, params object[] args)
        {
			if (textFormat == null) { throw new ArgumentNullException(nameof(textFormat)); }
			if (textFormat.Length == 0) { throw new ArgumentEmptyException(nameof(textFormat)); }
			_text = args == null ? textFormat : string.Format(CultureInfo.InvariantCulture, textFormat, args);
            _type = type;
            _timeout = timeout;
        }
        #endregion

        #region Methods
        /// <summary>
        /// Gets or sets the default wait time before terminating the attempt to execute a command and generating an error.
        /// </summary>
        /// <value>
        /// The <see cref="System.TimeSpan"/> to wait for the command to execute. Default value is 1 minute and 30 seconds.
        /// </value>
        public static TimeSpan DefaultTimeout
        {
            get { return _timeout; }
            set { _timeout = value; }
        }
        #endregion

        #region Properties
        /// <summary>
        /// Gets or sets the command text to execute.
        /// </summary>
        /// <value>The command text to execute.</value>
        public string Text
        {
            get { return _text; }
            set { _text = value; }
        }

        /// <summary>
        /// Gets or sets the command type value to execute.
        /// </summary>
        /// <value>The command type value to exceute. Default type value is Text.</value>
        public CommandType Type
        {
            get { return _type; }
            set { _type = value; }
        }

        /// <summary>
        /// Gets or sets the wait time before terminating the attempt to execute a command and generating an error.
        /// </summary>
        /// <value>The timespan to wait for the command to execute. Default value is 1 minute and 30 seconds.</value>
        public TimeSpan Timeout
        {
            get { return _timeout; }
            set { _timeout = value; }
        }
        #endregion
    }
}