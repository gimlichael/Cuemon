using System;

namespace Cuemon.Data
{
    /// <summary>
    /// The exception that is thrown when a <see cref="DataAdapter"/> operation is in an invalid state.
    /// </summary>
    public class DataAdapterException : Exception
    {
        #region Constructors
        /// <summary>
        /// Initializes a new instance of the <see cref="DataAdapterException"/> class.
        /// </summary>
        public DataAdapterException()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="DataAdapterException"/> class.
        /// </summary>
        /// <param name="message">The message that describes the error.</param>
        public DataAdapterException(string message) 
            : base(message)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="DataAdapterException"/> class.
        /// </summary>
        /// <param name="message">The message that describes the error.</param>
        /// <param name="innerException">The exception that is the cause of the current exception. If the innerException parameter is not a null reference, the current exception is raised in a catch block that handles the inner exception.</param>
        public DataAdapterException(string message, Exception innerException) 
            : base(message, innerException)
        {
        }
        #endregion
    }
}