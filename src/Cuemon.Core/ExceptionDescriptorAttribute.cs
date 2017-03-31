using System;

namespace Cuemon
{
    /// <summary>
    /// Provides information about an <see cref="Exception"/>, in a developer friendly way, optimized for open- and otherwise public application programming interfaces (API).
    /// </summary>
    /// <seealso cref="System.Attribute" />
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = true)]
    public class ExceptionDescriptorAttribute : Attribute
    {
        private string _helpLink;

        /// <summary>
        /// Initializes a new instance of the <see cref="ExceptionDescriptorAttribute"/> class.
        /// </summary>
        /// <param name="failureType">The <see cref="Type"/> of the failure to match and describe.</param>
        public ExceptionDescriptorAttribute(Type failureType)
        {
            Validator.ThrowIfNull(failureType, nameof(failureType));
            Validator.ThrowIfNotContainsType(failureType, nameof(failureType), "The specified type is not an Exception.", typeof(Exception));
            FailureType = failureType;
        }

        /// <summary>
        /// Gets or sets a number that identifies the type of failure.
        /// </summary>
        /// <value>The number that identifies the type of failure.</value>
        public int Code { get; set; }

        /// <summary>
        /// Gets or sets a message that describes the current failure.
        /// </summary>
        /// <value>The message that explains the reason for the failure.</value>
        public string Message { get; set; }

        /// <summary>
        /// Gets or sets a link to the help page associated with this failure.
        /// </summary>
        /// <value>The location of an optional help page associated with this failure.</value>
        public string HelpLink
        {
            get
            {
                return _helpLink;
            }
            set
            {
                if (value != null) { Validator.ThrowIfNotUri(value, nameof(value)); }
                _helpLink = value;
            }
        }

        /// <summary>
        /// Gets the <see cref="Type"/> of <see cref="Exception"/> to match and describe.
        /// </summary>
        /// <value>The <see cref="Type"/> of <see cref="Exception"/> to match.</value>
        public Type FailureType { get; }
    }
}