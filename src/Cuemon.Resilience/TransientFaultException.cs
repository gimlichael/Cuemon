using System;
using System.Runtime.Serialization;

namespace Cuemon.Resilience
{
    /// <summary>
    /// The exception that is thrown when a transient fault handling was unsuccessful.
    /// </summary>
    [Serializable]
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
        /// Initializes a new instance of the <see cref="TransientFaultException" /> class.
        /// </summary>
        /// <param name="message">The message that describes the error.</param>
        /// <param name="evidence">The evidence that provide details about the transient fault.</param>
        public TransientFaultException(string message, TransientFaultEvidence evidence) : base(message)
        {
            Validator.ThrowIfNull(evidence, nameof(evidence));
            Evidence = evidence;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="TransientFaultException"/> class.
        /// </summary>
        /// <param name="message">The message that describes the error.</param>
        /// <param name="innerException">The exception that is the cause of the current exception. If the innerException parameter is not a null reference, the current exception is raised in a catch block that handles the inner exception.</param>
        /// <param name="evidence">The evidence that provide details about the transient fault.</param>
        public TransientFaultException(string message, Exception innerException, TransientFaultEvidence evidence) : base(message, innerException)
        {
            Validator.ThrowIfNull(evidence, nameof(evidence));
            Evidence = evidence;
        }
        /// <summary>
        /// Initializes a new instance of the <see cref="TransientFaultException"/> class.
        /// </summary>
        /// <param name="info">The object that holds the serialized object data.</param>
        /// <param name="context">The contextual information about the source or destination.</param>
        protected TransientFaultException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
            Evidence = info.GetValue(nameof(Evidence), typeof(TransientFaultEvidence)) as TransientFaultEvidence;
        }
        #endregion

        #region Properties
        /// <summary>
        /// Gets the evidence that provide details about the transient fault of this instance.
        /// </summary>
        /// <value>The evidence that provide details about the transient fault.</value>
        public TransientFaultEvidence Evidence { get; }
        #endregion

        /// <summary>
        /// When overridden in a derived class, sets the <see cref="T:System.Runtime.Serialization.SerializationInfo"></see> with information about the exception.
        /// </summary>
        /// <param name="info">The object that holds the serialized object data.</param>
        /// <param name="context">The contextual information about the source or destination.</param>
        public override void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            info.AddValue(nameof(Evidence), Evidence);
            base.GetObjectData(info, context);
        }

        /// <summary>
        /// Returns a <see cref="string" /> that represents this instance.
        /// </summary>
        /// <returns>A <see cref="string" /> that represents this instance.</returns>
        public override string ToString()
        {
            return FormattableString.Invariant($"{base.ToString()} {Evidence}");
        }
    }
}