using System;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization;
using System.Text;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.WebUtilities;

namespace Cuemon.AspNetCore.Http
{
    /// <summary>
    /// Provides a base-class for exceptions based on an HTTP status code.
    /// </summary>
    /// <seealso cref="Exception" />
    [Serializable]
    public abstract class HttpStatusCodeException : Exception
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="HttpStatusCodeException"/> class.
        /// </summary>
        /// <param name="statusCode">The HTTP status code to associate with this exception.</param>
        /// <param name="message">The message that describes the HTTP status code.</param>
        protected HttpStatusCodeException(int statusCode, string message) : this(statusCode, Patterns.InvokeOrDefault(() => ReasonPhrases.GetReasonPhrase(statusCode), string.Empty), message)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="HttpStatusCodeException" /> class.
        /// </summary>
        /// <param name="statusCode">The HTTP status code to associate with this exception.</param>
        /// <param name="reasonPhrase">The HTTP reason phrase to associate with <paramref name="statusCode"/> and this exception.</param>
        /// <param name="message">The message that describes the HTTP status code.</param>
        protected HttpStatusCodeException(int statusCode, string reasonPhrase, string message) : base(message)
        {
            Validator.ThrowIfLowerThan(statusCode, StatusCodes.Status100Continue, nameof(statusCode), FormattableString.Invariant($"{nameof(statusCode)} cannot be less than {StatusCodes.Status100Continue}."));
            Validator.ThrowIfGreaterThan(statusCode, StatusCodes.Status511NetworkAuthenticationRequired, nameof(statusCode), FormattableString.Invariant($"{nameof(statusCode)} cannot be greater than {StatusCodes.Status511NetworkAuthenticationRequired}."));
            StatusCode = statusCode;
            ReasonPhrase = reasonPhrase;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="HttpStatusCodeException"/> class.
        /// </summary>
        /// <param name="info">The object that holds the serialized object data.</param>
        /// <param name="context">The contextual information about the source or destination.</param>
        protected HttpStatusCodeException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
            StatusCode = info.GetInt32(nameof(StatusCode));
            ReasonPhrase = info.GetString(nameof(ReasonPhrase));
        }

        /// <summary>
        /// Gets the HTTP status code associated with this exception.
        /// </summary>
        /// <value>The HTTP status code associated with this exception.</value>
        public int StatusCode { get; }

        /// <summary>
        /// Gets the HTTP reason phrase associated with this exception.
        /// </summary>
        /// <value>The HTTP reason phrase associated with this exception.</value>
        public string ReasonPhrase { get; }

        /// <summary>
        /// When overridden in a derived class, sets the <see cref="T:System.Runtime.Serialization.SerializationInfo"></see> with information about the exception.
        /// </summary>
        /// <param name="info">The object that holds the serialized object data.</param>
        /// <param name="context">The contextual information about the source or destination.</param>
        public override void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            info.AddValue(nameof(StatusCode), StatusCode);
            info.AddValue(nameof(ReasonPhrase), ReasonPhrase);
            base.GetObjectData(info, context);
        }

        /// <summary>
        /// Returns a <see cref="string" /> that represents this instance.
        /// </summary>
        /// <returns>A <see cref="string" /> that represents this instance.</returns>
        public override string ToString()
        {
            var sb = new StringBuilder(base.ToString());
            sb.AppendLine();
            sb.AppendLine();
            sb.AppendLine("Additional Information:");
            foreach (var pi in this.GetType().GetProperties().Where(pi => pi.CanRead && Decorator.Enclose(pi.DeclaringType).HasTypes(typeof(HttpStatusCodeException))))
            {
                var value = Patterns.InvokeOrDefault(() => pi.GetValue(this, null)); // we cannot risk exceptions being thrown in a ToString method
                if (value != null) { sb.AppendLine($"{Alphanumeric.Tab}{pi.Name}: {value}"); }
            }
            return sb.ToString();
        }
    }
}