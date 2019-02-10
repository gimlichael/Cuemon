using System.Collections.Generic;
using System.Linq;

namespace Cuemon.Serialization.Json
{
    /// <summary>
    /// Extension methods for the <see cref="JsonReaderResult"/>.
    /// </summary>
    public static class JsonReaderResultExtensions
    {
        /// <summary>
        /// Returns the only <see cref="JsonReaderResult"/> matching the specified <paramref name="propertyName"/>; this method throws an exception if more than one property is found.
        /// </summary>
        /// <param name="source">The sequence to return the single property value of.</param>
        /// <param name="propertyName">The property name to find in <paramref name="source"/>.</param>
        /// <returns>The single <see cref="JsonReaderResult"/> of the input sequence, or null if the sequence does not contains the specified <paramref name="propertyName"/>.</returns>
        public static JsonReaderResult SingleProperty(this IEnumerable<JsonReaderResult> source, string propertyName)
        {
            return source.SingleOrDefault(c => c.PropertyName == propertyName);
        }

        /// <summary>
        /// Returns the first <see cref="JsonReaderResult"/> matching the specified <paramref name="propertyName"/>, or a default value if no property is found.
        /// </summary>
        /// <param name="source">The sequence to return the first property value of.</param>
        /// <param name="propertyName">The property name to find in <paramref name="source"/>.</param>
        /// <returns>The first <see cref="JsonReaderResult"/> of the input sequence, or null if the sequence does not contains the specified <paramref name="propertyName"/>.</returns>
        public static JsonReaderResult FirstProperty(this IEnumerable<JsonReaderResult> source, string propertyName)
        {
            return source.FirstOrDefault(c => c.PropertyName == propertyName);
        }
    }
}