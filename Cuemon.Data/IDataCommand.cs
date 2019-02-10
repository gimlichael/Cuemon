using System;
using System.Data;

namespace Cuemon.Data
{
    /// <summary>
    /// Represents a statement that is executed while an open connection to a data source exists.
    /// </summary>
    public interface IDataCommand
    {
        /// <summary>
        /// Gets or sets the command text to execute.
        /// </summary>
        /// <value>The command text to execute.</value>
        string Text { get; set; }
        /// <summary>
        /// Gets or sets the command type value to execute.
        /// </summary>
        /// <value>The command type value to execute.</value>
        CommandType Type { get; set; }
        /// <summary>
        /// Gets or sets the wait time before terminating the attempt to execute a command and generating an error.
        /// </summary>
        /// <value>The timespan to wait for the command to execute.</value>
        TimeSpan Timeout { get; set; }
    }
}