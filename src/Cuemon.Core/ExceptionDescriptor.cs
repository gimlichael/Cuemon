using System;
using System.Collections.Generic;
using System.Linq;
using Cuemon.Collections.Generic;

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
        public int Code { get; private set; }

        /// <summary>
        /// Gets a message that describes the current failure.
        /// </summary>
        /// <value>The message that explains the reason for the failure.</value>
        public string Message { get; private set; }

        /// <summary>
        /// Gets a link to the help page associated with this failure.
        /// </summary>
        /// <value>The location of an optional help page associated with this failure.</value>
        public Uri HelpLink { get; private set; }

        /// <summary>
        /// Gets the <see cref="Exception"/> that caused the current failure.
        /// </summary>
        /// <value>The deeper cause of the failure.</value>
        public Exception Failure { get; }

        /// <summary>
        /// Post initialize this instance with the specified <paramref name="attribute"/>.
        /// </summary>
        public void PostInitializeWith(ExceptionDescriptorAttribute attribute)
        {
            PostInitializeWith(attribute.Yield());
        }

        /// <summary>
        /// Post initialize this instance with a matching <see cref="ExceptionDescriptorAttribute"/> from the specified <paramref name="attributes"/>.
        /// </summary>
        /// <param name="attributes">The attributes to find a match within.</param>
        public void PostInitializeWith(IEnumerable<ExceptionDescriptorAttribute> attributes)
        {
            var attribute = attributes?.FirstOrDefault(eda => eda.FailureType == Failure.GetType());
            if (attribute != null)
            {
                Code = attribute.Code;
                if (!attribute.Message.IsNullOrWhiteSpace()) { Message = attribute.Message; }
                if (!attribute.HelpLink.IsNullOrWhiteSpace()) { HelpLink = new Uri(attribute.HelpLink); }
            }
        }
    }
}