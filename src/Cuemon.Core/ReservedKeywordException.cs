using System;
using System.Runtime.Serialization;

namespace Cuemon
{
/// <summary>
    /// The exception that is thrown when the value of an argument is a reserved keyword.
    /// </summary>
    [Serializable]
    public class ReservedKeywordException : ArgumentOutOfRangeException
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ReservedKeywordException"/> class.
        /// </summary>
        public ReservedKeywordException()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ReservedKeywordException"/> class.
        /// </summary>
        /// <param name="paramName">The name of the parameter that caused the exception.</param>
        public ReservedKeywordException(string paramName) : this(paramName, "Specified argument is a reserved keyword.")
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ReservedKeywordException"/> class.
        /// </summary>
        /// <param name="paramName">The name of the parameter that caused the exception.</param>
        /// <param name="message">The message that describes the error.</param>
        public ReservedKeywordException(string paramName, string message) : base(paramName, message)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ReservedKeywordException"/> class.
        /// </summary>
        /// <param name="paramName">The name of the parameter that caused the exception.</param>
        /// <param name="actualValue">The value of the argument that causes this exception.</param>
        /// <param name="message">The message that describes the error.</param>
        public ReservedKeywordException(string paramName, string actualValue, string message) : base(paramName, actualValue, message)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ReservedKeywordException"/> class.
        /// </summary>
        /// <param name="message">The message that describes the error.</param>
        /// <param name="innerException">The exception that is the cause of the current exception. If the innerException parameter is not a null reference, the current exception is raised in a catch block that handles the inner exception.</param>
        public ReservedKeywordException(string message, Exception innerException) : base(message, innerException)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ReservedKeywordException"/> class.
        /// </summary>
        /// <param name="info">The object that holds the serialized object data.</param>
        /// <param name="context">The contextual information about the source or destination.</param>
        protected ReservedKeywordException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}
