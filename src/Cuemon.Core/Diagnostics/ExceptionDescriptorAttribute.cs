using System;
using System.Reflection;
using Cuemon.Globalization;

namespace Cuemon.Diagnostics
{
    /// <summary>
    /// Provides information about an <see cref="Exception"/>, in a developer friendly way, optimized for open- and otherwise public application programming interfaces (API).
    /// </summary>
    /// <seealso cref="Attribute" />
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = true)]
    public class ExceptionDescriptorAttribute : Attribute, IMessageLocalizer
    {
        private string _helpLink;
        private string _message;
        private readonly Lazy<string> _messageResolver;

        /// <summary>
        /// Initializes a new instance of the <see cref="ExceptionDescriptorAttribute"/> class.
        /// </summary>
        /// <param name="failureType">The <see cref="Type"/> of the failure to match and describe.</param>
        public ExceptionDescriptorAttribute(Type failureType)
        {
            Validator.ThrowIfNull(failureType, nameof(failureType));
            Validator.ThrowIfNotContainsType(failureType, nameof(failureType), "The specified type is not an Exception.", typeof(Exception));
            FailureType = failureType;
            _messageResolver = new Lazy<string>(() =>
            {
                if (!string.IsNullOrWhiteSpace(MessageResourceName) && MessageResourceType != null)
                {
                    var property = MessageResourceType.GetProperty(MessageResourceName, BindingFlags.Public | BindingFlags.Static | BindingFlags.NonPublic);
                    var getMethod = property?.GetGetMethod(true);
                    if (getMethod != null && (getMethod.IsAssembly || getMethod.IsPublic))
                    {
                        return property.GetValue(null, null) as string;
                    }
                }
                return _message;
            });
        }

        /// <summary>
        /// Gets or sets an error code that uniquely identifies the type of failure.
        /// </summary>
        /// <value>The number that identifies the type of failure.</value>
        public string Code { get; set; }

        /// <summary>
        /// Gets or sets a default message that describes the current failure.
        /// </summary>
        /// <value>The default message that explains the reason for the failure.</value>
        public string Message
        {
            get => _messageResolver.Value;
            set => _message = value;
        }

        /// <summary>
        /// Gets or sets the resource name (property name) to use as the key for lookups on the resource type.
        /// </summary>
        /// <value>Use this property to set the name of the property within <see cref="MessageResourceType" /> that will provide a localized message that describes the current failure.</value>
        public string MessageResourceName { get; set; }

        /// <summary>
        /// Gets or sets the resource type to use for message lookups.
        /// </summary>
        /// <value>Use this property only in conjunction with <see cref="MessageResourceName" />. They are used together to retrieve localized messages at runtime.</value>
        public Type MessageResourceType { get; set; }

        /// <summary>
        /// Gets or sets a link to the help page associated with this failure.
        /// </summary>
        /// <value>The location of an optional help page associated with this failure.</value>
        public string HelpLink
        {
            get => _helpLink;
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