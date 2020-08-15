using System;
using System.Runtime.Serialization;

namespace Cuemon.Extensions.Resilience
{
    /// <summary>
    /// The exception that is thrown when a latency related operation was taking to long to complete.
    /// </summary>
    [Serializable]
    public class LatencyException : Exception
    {
        #region Constructors
        /// <summary>
        /// Initializes a new instance of the <see cref="LatencyException"/> class.
        /// </summary>
        public LatencyException()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="LatencyException"/> class.
        /// </summary>
        /// <param name="message">The message that describes the error.</param>
        public LatencyException(string message) : base(message)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="LatencyException"/> class.
        /// </summary>
        /// <param name="message">The message that describes the error.</param>
        /// <param name="innerException">The exception that is the cause of the current exception. If the innerException parameter is not a null reference, the current exception is raised in a catch block that handles the inner exception.</param>
        public LatencyException(string message, Exception innerException) : base(message, innerException)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="LatencyException"/> class.
        /// </summary>
        /// <param name="info">The object that holds the serialized object data.</param>
        /// <param name="context">The contextual information about the source or destination.</param>
        protected LatencyException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
        #endregion
    }
}