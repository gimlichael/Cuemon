using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Cuemon.Collections.Generic;

namespace Cuemon.Diagnostics
{
    /// <summary>
    /// Provides information about an <see cref="Exception"/>, in a developer friendly way.
    /// </summary>
    public class ExceptionDescriptor
    {
        private readonly IDictionary<string, object> _evidence;
        private readonly Lazy<IReadOnlyDictionary<string, object>> _lazyEvidence;

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
        /// <param name="code">The error code that uniquely identifies the type of failure.</param>
        /// <param name="message">The message that explains the reason for the failure.</param>
        /// <param name="helpLink">The optional link to a help page associated with this failure.</param>
        /// <remarks><paramref name="code"/> will remove any spaces that might be present.</remarks>
        public ExceptionDescriptor(Exception failure, string code, string message, Uri helpLink = null)
        {
            Validator.ThrowIfNull(failure, nameof(failure));
            Validator.ThrowIfNullOrWhitespace(message, nameof(message));
            Code = code.RemoveAll(' ');
            Message = message;
            HelpLink = helpLink;
            Failure = failure;
            _evidence = new Dictionary<string, object>();
            _lazyEvidence = new Lazy<IReadOnlyDictionary<string, object>>(() => new ReadOnlyDictionary<string, object>(_evidence));
        }

        /// <summary>
        /// Gets a collection of key/value pairs that can provide additional information about the failure.
        /// </summary>
        /// <value>An optional collection of key/value pairs that can provide additional information about the failure.</value>
        public IReadOnlyDictionary<string, object> Evidence => _lazyEvidence.Value;

        /// <summary>
        /// Adds an element of evidence to associate with the faulted operation.
        /// </summary>
        /// <typeparam name="T">The type of the <paramref name="evidence"/>.</typeparam>
        /// <param name="context">The context of the evidence.</param>
        /// <param name="evidence">The evidence itself.</param>
        /// <param name="evidenceProvider">The function delegate that provides the evidence.</param>
        public void AddEvidence<T>(string context, T evidence, Func<T, object> evidenceProvider)
        {
            _evidence.AddIfNotContainsKey(context, evidenceProvider?.Invoke(evidence));
        }

        /// <summary>
        /// Gets an error code that uniquely identifies the type of failure.
        /// </summary>
        /// <value>The number that identifies the type of failure.</value>
        public string Code { get; private set; }

        /// <summary>
        /// Gets a message that describes the current failure.
        /// </summary>
        /// <value>The message that explains the reason for the failure.</value>
        public string Message { get; private set; }

        /// <summary>
        /// Gets or sets a link to the help page associated with this failure.
        /// </summary>
        /// <value>The location of an optional help page associated with this failure.</value>
        public Uri HelpLink { get; set; }

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
            var attribute = attributes?.SingleOrDefault(eda => eda.FailureType == Failure.GetType());
            if (attribute != null)
            {
                if (!attribute.Code.IsNullOrWhiteSpace()) { Code = attribute.Code; }
                if (!attribute.Message.IsNullOrWhiteSpace()) { Message = attribute.Message; }
                if (!attribute.HelpLink.IsNullOrWhiteSpace()) { HelpLink = new Uri(attribute.HelpLink); }
            }
        }
    }
}