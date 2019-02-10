using System;
using System.Data;
using System.Data.Common;
using System.Globalization;

namespace Cuemon.Data
{
    /// <summary>
    /// Represents a connection to a database.
    /// </summary>
    public abstract class DataConnection : DbConnection, IDataConnection
    {
        private TimeSpan _timeout = TimeSpan.FromSeconds(10);
        private string _database;
        private string _address;
        private string _userId;
        private string _password;
        private string _networkLibrary;
        private string _connectionString;

        #region Constructors
        /// <summary>
        /// Initializes a new instance of the <see cref="DataConnection"/> class.
        /// </summary>
        protected DataConnection()
        {
        }

 /// <summary>
        /// Initializes a new instance of the <see cref="DataConnection"/> class.
        /// </summary>
        /// <param name="connectionString">The connection string used to establish the connection.</param>
        protected DataConnection(string connectionString)
        {
            _connectionString = connectionString;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="DataConnection"/> class.
        /// </summary>
        /// <param name="database">The database of the connection.</param>
        /// <param name="address">The address of the connection.</param>
        /// <param name="userId">The user id of the connection.</param>
        /// <param name="password">The password of the connection.</param>
        /// <param name="networkLibrary">The network library of the connection.</param>
        protected DataConnection(string database, string address, string userId, string password, string networkLibrary)
        {
            _database = database;
            _address = address;
            _userId = userId;
            _password = password;
            _networkLibrary = networkLibrary;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="DataConnection"/> class.
        /// </summary>
        /// <param name="database">The database of the connection.</param>
        /// <param name="address">The address of the connection.</param>
        /// <param name="userId">The user id of the connection.</param>
        /// <param name="password">The password of the connection.</param>
        /// <param name="networkLibrary">The network library of the connection.</param>
        /// <param name="timeout">The timespan to wait of the connection to open.</param>
        protected DataConnection(string database, string address, string userId, string password, string networkLibrary, TimeSpan timeout)
        {
            _database = database;
            _address = address;
            _userId = userId;
            _password = password;
            _networkLibrary = networkLibrary;
            _timeout = timeout;
        }
        #endregion

        #region Properties
        /// <summary>
        /// Gets or sets the string used to open the connection.
        /// </summary>
        /// <value></value>
        /// <returns>The connection string used to establish the initial connection. The exact contents of the connection string depend on the specific data source for this connection. The default value is based upon the properties of this class.</returns>
        public override string ConnectionString
        {
            get
            {
                if (string.IsNullOrEmpty(_connectionString))
                {
                    _connectionString = ToString();
                }
                return _connectionString;
            }
            set
            {
                _connectionString = value;
            }
        }

        /// <summary>
        /// Gets or sets the database of the connection.
        /// </summary>
        /// <value>The database of the connection.</value>
        public override string Database
        {
            get { return _database; }
        }

        /// <summary>
        /// Gets or sets the server address of the connection.
        /// </summary>
        /// <value>The server address of the connection.</value>
        public string Address
        {
            get { return _address; }
            set { _address = value; }
        }

        /// <summary>
        /// Gets the time to wait while trying to establish a connection before terminating the attempt and generating an error. Preserved for backward compatibility.
        /// </summary>
        /// <returns>The time (in seconds) to wait for a connection to open. The value is taken from the Timeout value, and is default 10 seconds.</returns>
        public override int ConnectionTimeout
        {
            get { return (int)Timeout.TotalSeconds; }
        }

        /// <summary>
        /// Gets or sets the user id of the connection.
        /// </summary>
        /// <value>The user id of the connection.</value>
        public string UserId
        {
            get { return _userId; }
            set { _userId = value; }
        }

        /// <summary>
        /// Gets or sets the password of the connection.
        /// </summary>
        /// <value>The password of the connection.</value>
        public string Password
        {
            get { return _password; }
            set { _password = value; }
        }

        /// <summary>
        /// Gets or sets the network library of the connection.
        /// </summary>
        /// <value>The network library of the connection.</value>
        public string NetworkLibrary
        {
            get { return _networkLibrary; }
            set { _networkLibrary = value; }
        }

        /// <summary>
        /// Gets or sets the time to wait while trying to establish a connection before terminating the attempt and generating an error.
        /// </summary>
        /// <value>The timespan to wait for a connection to open. The default value is 10 seconds.</value>
        public TimeSpan Timeout
        {
            get { return _timeout; }
            set { _timeout = value; }
        }

        /// <summary>
        /// Gets the current state of the connection.
        /// </summary>
        /// <value></value>
        /// <returns>One of the <see cref="T:System.Data.ConnectionState"></see> values.</returns>
        public abstract override ConnectionState State { get; }
        #endregion

        #region Methods
        /// <summary>
        /// Renders a connection string from objects with the implemented <see cref="IDataConnection"/> interface.
        /// </summary>
        /// <param name="dataConnection">The data connection interface.</param>
        /// <returns>A connection string.</returns>
        public static string GetConnectionString(DbConnection dataConnection)
        {
            if (dataConnection == null) { throw new ArgumentNullException(nameof(dataConnection), "The supplied data connection object cannot be null."); }
            return dataConnection.ConnectionString;
        }

        /// <summary>
        /// Renders the properties of this class to a connection string (if no "manuel" connectionString has been specified). 
        /// If ConfigurationElement has been set, values are derived from this object.
        /// </summary>
        /// <returns>
        /// A connection string.
        /// </returns>
        public override string ToString()
        {
            if (string.IsNullOrEmpty(_connectionString))
            {
                return string.Format(CultureInfo.InvariantCulture, "Address={0};Database={1};Network Library={2};User ID={3};Password={4};Connection Timeout={5}",
                    Address,
                    Database,
                    NetworkLibrary,
                    UserId,
                    Password,
                    Convert.ToInt32(Timeout.TotalSeconds));
            }
            return _connectionString;
        }

        /// <summary>
        /// Changes the current database for an open Connection object.
        /// </summary>
        /// <param name="databaseName">The name of the database to use in place of the current database.</param>
        public abstract override void ChangeDatabase(string databaseName);

        /// <summary>
        /// Closes the connection to the database.
        /// </summary>
        public abstract override void Close();

        /// <summary>
        /// Opens a database connection with the settings specified by the ConnectionString property of the provider-specific Connection object.
        /// </summary>
        public abstract override void Open();
        #endregion
    }
}