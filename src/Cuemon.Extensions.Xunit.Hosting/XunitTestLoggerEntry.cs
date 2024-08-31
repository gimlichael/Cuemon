using Microsoft.Extensions.Logging;

namespace Cuemon.Extensions.Xunit.Hosting
{
    /// <summary>
    /// Represents a captured log-entry for testing purposes. This record encapsulates the <see cref="LogLevel"/>, <see cref="EventId"/> and message.
    /// </summary>
    public record XunitTestLoggerEntry
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="XunitTestLoggerEntry"/> class.
        /// </summary>
        /// <param name="severity">The <see cref="LogLevel"/> for this entry.</param>
        /// <param name="id">The <see cref="EventId"/> of this entry.</param>
        /// <param name="message">The message of this entry.</param>
        public XunitTestLoggerEntry(LogLevel severity, EventId id, string message)
        {
            Severity = severity;
            Id = id;
            Message = message;
        }

        /// <summary>
        /// Gets the event identifier.
        /// </summary>
        /// <value>The event identifier.</value>
        public EventId Id { get; }

        /// <summary>
        /// Gets the log level.
        /// </summary>
        /// <value>The log level.</value>
        public LogLevel Severity { get; }

        /// <summary>
        /// Gets the value of the message.
        /// </summary>
        /// <value>The value of the message.</value>
        public string Message { get; }

        /// <summary>
        /// Returns a <see cref="string" /> that represents this instance.
        /// </summary>
        /// <returns>A <see cref="string" /> that represents this instance.</returns>
        public override string ToString()
        {
            return Message;
        }
    }
}
