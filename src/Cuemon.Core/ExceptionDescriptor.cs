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
        public ExceptionDescriptor(Exception failure, int code, string message, Uri helpLink = null)
        {
            Validator.ThrowIfNull(failure, nameof(failure));
            Validator.ThrowIfNullOrEmpty(message, nameof(message));
            Code = code;
            Message = message;
            HelpLink = helpLink;
            Failure = failure;
        }

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
        /// <value>The failure.</value>
        public Exception Failure { get; }
    }
}