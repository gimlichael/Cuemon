using System;

namespace Cuemon
{
    /// <summary>
    /// The exception that is thrown when a transient fault handling was unsuccessful.
    /// </summary>
    public class TransientFaultException : Exception
    {
        #region Constructors
        /// <summary>
        /// Initializes a new instance of the <see cref="TransientFaultException"/> class.
        /// </summary>
        public TransientFaultException()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="TransientFaultException"/> class.
        /// </summary>
        /// <param name="message">The message that describes the error.</param>
        public TransientFaultException(string message) : base(message)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="TransientFaultException"/> class.
        /// </summary>
        /// <param name="message">The message that describes the error.</param>
        /// <param name="innerException">The exception that is the cause of the current exception. If the innerException parameter is not a null reference, the current exception is raised in a catch block that handles the inner exception.</param>
        public TransientFaultException(string message, Exception innerException) : base(message, innerException)
        {
        }
        #endregion
    }
}