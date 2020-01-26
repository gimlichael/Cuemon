using System;

namespace Cuemon.Data
{
    /// <summary>
    /// Represents a connection to a data source.
    /// </summary>
    public interface IDataConnection
    {
        /// <summary>
        /// Gets or sets the server address of the connection.
        /// </summary>
        /// <value>The server address of the connection.</value>
        string Address { get; set; }

        /// <summary>
        /// Gets or sets the network library of the connection.
        /// </summary>
        /// <value>The network library of the connection.</value>
        string NetworkLibrary { get; set; }

        /// <summary>
        /// Gets or sets the password of the connection.
        /// </summary>
        /// <value>The password of the connection.</value>
        string Password { get; set; }

        /// <summary>
        /// Gets or sets the time to wait while trying to establish a connection before terminating the attempt and generating an error.
        /// </summary>
        /// <value>The timespan to wait for a connection to open. The default value is 10 seconds.</value>
        TimeSpan Timeout { get; set; }

        /// <summary>
        /// Gets or sets the user id of the connection.
        /// </summary>
        /// <value>The user id of the connection.</value>
        string UserId { get; set; }
    }
}