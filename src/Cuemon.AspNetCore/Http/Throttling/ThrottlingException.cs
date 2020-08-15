using System;
using System.Runtime.Serialization;

namespace Cuemon.AspNetCore.Http.Throttling
{
    /// <summary>
    /// The exception that is thrown when a given request threshold has been reached and then throttled.
    /// </summary>
    /// <seealso cref="HttpStatusCodeException" />
    [Serializable]
    public class ThrottlingException : HttpStatusCodeException
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ThrottlingException"/> class.
        /// </summary>
        /// <param name="statusCode">The HTTP status code to associate with this exception.</param>
        /// <param name="message">The message that describes the HTTP status code.</param>
        /// <param name="rateLimit">The allowed rate of requests for a given window.</param>
        /// <param name="delta">The remaining duration of a window.</param>
        /// <param name="reset">The date and time when a window is being reset.</param>
        public ThrottlingException(int statusCode, string message, int rateLimit, TimeSpan delta, DateTime reset) : base(statusCode, message)
        {
            RateLimit = rateLimit;
            Delta = delta;
            Reset = reset;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ThrottlingException"/> class.
        /// </summary>
        /// <param name="info">The object that holds the serialized object data.</param>
        /// <param name="context">The contextual information about the source or destination.</param>
        protected ThrottlingException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
            Delta = (TimeSpan)info.GetValue(nameof(Delta), typeof(TimeSpan));
            RateLimit = info.GetInt32(nameof(RateLimit));
            Reset = info.GetDateTime(nameof(Reset));
        }

        /// <summary>
        /// Gets the allowed rate of requests for a given window.
        /// </summary>
        /// <value>The allowed rate of requests for a given window.</value>
        public int RateLimit { get; }

        /// <summary>
        /// Gets the remaining duration of a window.
        /// </summary>
        /// <value>The remaining duration of a window.</value>
        public TimeSpan Delta { get; }

        /// <summary>
        /// Gets date and time when a window is being reset.
        /// </summary>
        /// <value>The date and time when a window is being reset.</value>
        public DateTime Reset { get; }

        /// <summary>
        /// When overridden in a derived class, sets the <see cref="T:System.Runtime.Serialization.SerializationInfo"></see> with information about the exception.
        /// </summary>
        /// <param name="info">The object that holds the serialized object data.</param>
        /// <param name="context">The contextual information about the source or destination.</param>
        public override void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            info.AddValue(nameof(RateLimit), RateLimit);
            info.AddValue(nameof(Reset), Reset);
            info.AddValue(nameof(Delta), Delta);
            base.GetObjectData(info, context);
        }
    }
}