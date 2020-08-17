using System;
using System.Runtime.Serialization;

namespace Cuemon
{
    /// <summary>
    /// The exception that is thrown when one of the type arguments provided to a method is not valid.
    /// </summary>
    [Serializable]
    public class TypeArgumentException : ArgumentException
    {
        #region Constructors
        /// <summary>
        /// Initializes a new instance of the <see cref="TypeArgumentException"/> class.
        /// </summary>
        public TypeArgumentException() 
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="TypeArgumentException"/> class.
        /// </summary>
        /// <param name="typeParamName">The name of the type parameter that caused the exception.</param>
        public TypeArgumentException(string typeParamName) : this(typeParamName, "Value does not fall within the expected range.")
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="TypeArgumentException"/> class.
        /// </summary>
        /// <param name="typeParamName">The name of the type parameter that caused the exception.</param>
        /// <param name="message">The message that describes the error.</param>
        public TypeArgumentException(string typeParamName, string message) : base(message, typeParamName)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="TypeArgumentException"/> class.
        /// </summary>
        /// <param name="message">The message that describes the error.</param>
        /// <param name="innerException">The exception that is the cause of the current exception. If the innerException parameter is not a null reference, the current exception is raised in a catch block that handles the inner exception.</param>
        public TypeArgumentException(string message, Exception innerException) : base(message, innerException)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="TypeArgumentException"/> class.
        /// </summary>
        /// <param name="info">The object that holds the serialized object data.</param>
        /// <param name="context">The contextual information about the source or destination.</param>
        protected TypeArgumentException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
        #endregion
    }
}