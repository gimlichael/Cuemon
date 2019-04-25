using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Cuemon.Reflection;

namespace Cuemon
{
    /// <summary>
    /// Wraps an exception that was refined with meta information from either <see cref="ExceptionUtility.Refine(Exception,System.Reflection.MethodBase,object[])"/> or <see cref="ExceptionUtility.Refine(Exception,MethodDescriptor,object[])"/>. 
    /// This class cannot be inherited.
    /// </summary>
    /// <seealso cref="Exception" />
    public sealed class MethodWrappedException : Exception
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="MethodWrappedException"/> class.
        /// </summary>
        /// <param name="wrappedException">The exception that is wrapped to preserve details about an exception.</param>
        /// <param name="throwingMethod">The method that was the cause of the <paramref name="wrappedException"/>.</param>
        /// <param name="throwingMethodParameters">The runtime parameters of the <paramref name="wrappedException"/>.</param>
        internal MethodWrappedException(Exception wrappedException, MethodDescriptor throwingMethod, IDictionary<string, object> throwingMethodParameters) : base($"Exception of {wrappedException.GetBaseException().GetType().FullName} was wrapped and thrown by {throwingMethod}.", wrappedException)
        {
            ThrowingMethod = throwingMethod;
            Source = throwingMethod.ToString();
            if (throwingMethodParameters.Count > 0)
            {
                foreach (var item in throwingMethodParameters)
                {
                    var key = item.Key;
                    if (!Data.Contains(key)) { Data.Add(key, StringConverter.FromObject(item.Value)); }
                }
            }
        }

        /// <summary>
        /// Gets the method that was the cause of this <see cref="MethodWrappedException"/>.
        /// </summary>
        /// <value>The method throwing this <see cref="MethodWrappedException"/>.</value>
        public MethodDescriptor ThrowingMethod { get; }

        /// <summary>
        /// Returns a <see cref="string" /> that represents this instance.
        /// </summary>
        /// <returns>A <see cref="string" /> that represents this instance.</returns>
        public override string ToString()
        {
            var builder = new StringBuilder();
            builder.AppendLine($"Throwing Method: {Source}");
            builder.AppendLine($"Runtime Parameters: {Data.Cast<DictionaryEntry>().Select(de => new KeyValuePair<string, object>(de.Key.ToString(), de.Value)).ToDelimitedString(", ", pair => $"{pair.Key}={pair.Value}")}");
            return builder.ToString();
        }
    }
}