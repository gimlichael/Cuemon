using System;
using System.Runtime.CompilerServices;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Cuemon.Extensions.Newtonsoft.Json
{
    /// <summary>
    /// Extension methods for the <see cref="Validator"/> class.
    /// </summary>
    public static class ValidatorExtensions
    {
        /// <summary>
        /// Validates and throws an <see cref="ArgumentException"/> if the specified <paramref name="argument"/> is not a valid JSON representation as specified in RFC 8259.
        /// </summary>
        /// <param name="_">The <see cref="Validator"/> to extend.</param>
        /// <param name="argument">The JSON string to be evaluated.</param>
        /// <param name="paramName">The name of the parameter that caused the exception.</param>
        /// <param name="message">A message that describes the error.</param>
        /// <exception cref="ArgumentException">
        /// <paramref name="argument"/> must be a JSON representation that complies with RFC 8259.
        /// </exception>
        public static void InvalidJsonDocument(this Validator _, string argument, [CallerArgumentExpression(nameof(argument))] string paramName = null, string message = "Value must be a JSON representation that complies with RFC 8259.")
        {
            try
            {
                JToken.Parse(argument);
            }
            catch (Exception e)
            {
                throw new ArgumentException(message, paramName, e);
            }
        }

        /// <summary>
        /// Validates and throws an <see cref="ArgumentException"/> if the specified <paramref name="argument"/> is not a valid JSON representation as specified in RFC 8259.
        /// </summary>
        /// <param name="_">The <see cref="Validator"/> to extend.</param>
        /// <param name="argument">The <see cref="JsonReader"/> to be evaluated.</param>
        /// <param name="paramName">The name of the parameter that caused the exception.</param>
        /// <param name="message">A message that describes the error.</param>
        /// <exception cref="ArgumentException">
        /// <paramref name="argument"/> must be a JSON representation that complies with RFC 8259.
        /// </exception>
        public static void InvalidJsonDocument(this Validator _, ref JsonReader argument, [CallerArgumentExpression(nameof(argument))] string paramName = null, string message = "Value must be a JSON representation that complies with RFC 8259.")
        {
            if (argument == null) { return; }
            var reader = argument;
            try
            {
                var o = JToken.Load(reader);
                reader = o.CreateReader();
            }
            catch (Exception e)
            {
                throw new ArgumentException(message, paramName, e);
            }
            argument = reader;
        }
    }
}
