using System.Collections.Generic;
using System.Collections.Specialized;
using Cuemon.Extensions.Core;

namespace Cuemon.Extensions.Collections.Specialized
{
    /// <summary>
    /// Extension methods for the <see cref="IDictionary{TKey,TValue}"/> interface.
    /// </summary>
    public static class DictionaryExtensions
    {
        /// <summary>
        /// Creates a <see cref="NameValueCollection"/> from the specified <paramref name="source"/>.
        /// </summary>
        /// <param name="source">An <see cref="IDictionary{TKey,TValue}"/> to convert into an <see cref="NameValueCollection"/> equivalent.</param>
        /// <returns>A <see cref="NameValueCollection"/> that is equivalent to the specified <paramref name="source"/>.</returns>
        public static NameValueCollection ToNameValueCollection(this IDictionary<string, string[]> source)
        {
            Validator.ThrowIfNull(source, nameof(source));
            var result = new NameValueCollection();
            foreach (var item in source)
            {
                result.Add(item.Key, item.Value.ToDelimitedString());
            }
            return result;
        }
    }
}