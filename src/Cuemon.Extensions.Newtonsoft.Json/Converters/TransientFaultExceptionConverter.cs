using System;
using System.Linq;
using Cuemon.Reflection;
using Cuemon.Resilience;
using Cuemon.Runtime.Serialization.Formatters;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Cuemon.Extensions.Newtonsoft.Json.Converters
{
    /// <summary>
    /// Converts a <see cref="TransientFaultException"/> to or from JSON.
    /// </summary>
    /// <seealso cref="JsonConverter" />
    public class TransientFaultExceptionConverter : JsonConverter
    {
        /// <summary>
        /// Writes the JSON representation of the object.
        /// </summary>
        /// <param name="writer">The <see cref="T:Newtonsoft.Json.JsonWriter" /> to write to.</param>
        /// <param name="value">The value to serialize.</param>
        /// <param name="serializer">The calling serializer.</param>
        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            var converter = serializer.Converters.FirstOrDefault(converter => converter.CanConvert(typeof(Exception)));
            converter?.WriteJson(writer, value, serializer);
        }

        /// <summary>
        /// Reads the JSON representation of the object.
        /// </summary>
        /// <param name="reader">The <see cref="T:Newtonsoft.Json.JsonReader" /> to read from.</param>
        /// <param name="objectType">Type of the object to deserialize.</param>
        /// <param name="existingValue">The existing value of object being read.</param>
        /// <param name="serializer">The calling serializer.</param>
        /// <returns>The deserialized object value.</returns>
        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            var jo = JObject.Load(reader);
            var message = jo["message"]?.Value<string>();
            var evidenceJson = jo["evidence"];
            var innerExceptionJson = jo["inner"];
            Exception innerException = null;
            TransientFaultEvidence evidence = null;

            if (evidenceJson != null)
            {
                var attempts = evidenceJson["attempts"].Value<int>();
                var recoveryWaitTime = Decorator.Enclose(evidenceJson["recoveryWaitTime"].Value<string>()).ChangeType<TimeSpan>();
                var totalRecoveryWaitTime = Decorator.Enclose(evidenceJson["totalRecoveryWaitTime"].Value<string>()).ChangeType<TimeSpan>();
                var latency = Decorator.Enclose(evidenceJson["latency"].Value<string>()).ChangeType<TimeSpan>();
                var caller = evidenceJson["descriptor"]["caller"].Value<string>();
                var methodName = evidenceJson["descriptor"]["methodName"].Value<string>();
                var parameters = evidenceJson["descriptor"]["parameters"].Values<string>().ToArray();
                var arguments = evidenceJson["descriptor"]["arguments"].Values<object>().ToArray();
                evidence = new TransientFaultEvidence(attempts, recoveryWaitTime, totalRecoveryWaitTime, latency, new MethodSignature(caller, methodName, parameters, arguments));
            }

            if (innerExceptionJson != null)
            {
                var converter = serializer.Converters.FirstOrDefault(converter => converter.CanConvert(typeof(Exception)));
                innerException = converter?.ReadJson(innerExceptionJson.CreateReader(), Formatter.GetType(innerExceptionJson["type"].Value<string>()), existingValue, serializer) as Exception;
            }

            return new TransientFaultException(message, innerException, evidence);
        }

        /// <summary>
        /// Determines whether this instance can convert the specified object type.
        /// </summary>
        /// <param name="objectType">Type of the object.</param>
        /// <returns><c>true</c> if this instance can convert the specified object type; otherwise, <c>false</c>.</returns>
        public override bool CanConvert(Type objectType)
        {
            return typeof(TransientFaultException).IsAssignableFrom(objectType);
        }
    }
}
