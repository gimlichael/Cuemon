using System;

namespace Cuemon
{
    /// <summary>
    /// Provides information about an <see cref="Exception"/>, in a developer friendly way, optimized for open- and otherwise public application programming interfaces (API).
    /// </summary>
    public class ExceptionDescriptor
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ExceptionDescriptor"/> class.
        /// </summary>
        protected ExceptionDescriptor()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ExceptionDescriptor" /> class.
        /// </summary>
        /// <param name="failure">The <see cref="Exception"/> that caused the current failure.</param>
        /// <param name="code">The number that identifies the type of failure.</param>
        /// <param name="message">The message that explains the reason for the failure.</param>
        /// <param name="helpLink">The optional link to a help page associated with this failure.</param>
        public ExceptionDescriptor(Exception failure, int code, string message, Uri helpLink = null)
        {
            Validator.ThrowIfNull(failure, nameof(failure));
            Validator.ThrowIfNullOrEmpty(message, nameof(message));
            Code = code;
            Message = message;
            HelpLink = helpLink;
            Failure = failure.GetBaseException();
        }

        /// <summary>
        /// Gets or sets the request identifier that uniquely identifies the service request the caller made.
        /// </summary>
        /// <value>An identifier that uniquely identifies the service request the caller made.</value>
        public string RequestId { get; set; }

        /// <summary>
        /// Gets a number that identifies the type of failure.
        /// </summary>
        /// <value>The number that identifies the type of failure.</value>
        public int Code { get; }

        /// <summary>
        /// Gets a message that describes the current failure.
        /// </summary>
        /// <value>The message that explains the reason for the failure.</value>
        public string Message { get; }

        /// <summary>
        /// Gets a link to the help page associated with this failure.
        /// </summary>
        /// <value>The location of an optional help page associated with this failure.</value>
        public Uri HelpLink { get; }

        /// <summary>
        /// Gets the <see cref="Exception"/> that caused the current failure.
        /// </summary>
        /// <value>The deeper cause of the failure.</value>
        public Exception Failure { get; }
    }
}