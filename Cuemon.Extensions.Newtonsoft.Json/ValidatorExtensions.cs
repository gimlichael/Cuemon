using System;
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
        /// Validates and throws an <see cref="ArgumentException"/> if the specified <paramref name="value"/> is not a valid JSON representation as specified in RFC 8259.
        /// </summary>
        /// <param name="validator">The <see cref="Validator"/> to extend.</param>
        /// <param name="value">The value to be evaluated.</param>
        /// <param name="paramName">The name of the parameter that caused the exception.</param>
        /// <param name="message">A message that describes the error.</param>
        /// <exception cref="ArgumentException">
        /// <paramref name="value"/> must be a JSON representation that complies with RFC 8259.
        /// </exception>
        public static void IfNotValidJsonDocument(this Validator validator, ref JsonReader value, string paramName, string message = "Value must be a JSON representation that complies with RFC 8259.")
        {
            if (value == null) { return; }
            var reader = value;
            try
            {
                Exception inner = null;
                validator.ThrowWhenCondition(c => c.IsFalse(() =>
                {
                    try
                    {
                        var o = JToken.Load(reader);
                        reader = o.CreateReader();
                        return true;
                    }
                    catch (Exception e)
                    {
                        inner = e;
                        return false;
                    }
                }).Create(() => new ArgumentException(message, paramName, inner)).TryThrow());
            }
            catch (ArgumentException ex)
            {
                throw ex;
            }
            value = reader;
        }
    }
}