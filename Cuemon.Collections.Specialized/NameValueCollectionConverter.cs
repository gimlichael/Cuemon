using System.Collections.Generic;
using System.Collections.Specialized;

namespace Cuemon.Collections.Specialized
{
    /// <summary>
    /// This utility class is designed to make <see cref="NameValueCollection"/> related conversions easier to work with.
    /// </summary>
    public static class NameValueCollectionConverter
    {
        /// <summary>
        /// Creates a <see cref="NameValueCollection"/> from the specified <paramref name="source"/>.
        /// </summary>
        /// <param name="source">An <see cref="IDictionary{TKey,TValue}"/> to convert into an <see cref="NameValueCollection"/> equivalent.</param>
        /// <returns>A <see cref="NameValueCollection"/> that is equivalent to the specified <paramref name="source"/>.</returns>
        public static NameValueCollection FromDictionary(IDictionary<string, string[]> source)
        {
            Validator.ThrowIfNull(source, nameof(source));
            NameValueCollection result = new NameValueCollection();
            foreach (var item in source)
            {
                result.Add(item.Key, StringConverter.ToDelimitedString(item.Value));
            }
            return result;
        }

        /// <summary>
        /// Creates a <see cref="IDictionary{TKey,TValue}"/> from the specified <paramref name="source"/>.
        /// </summary>
        /// <param name="source">A <see cref="NameValueCollection"/> to convert into an <see cref="IDictionary{TKey,TValue}"/> equivalent.</param>
        /// <returns>A <see cref="IDictionary{TKey,TValue}"/> that is equivalent to the specified <paramref name="source"/>.</returns>
        public static IDictionary<string, string[]> ToDictionary(NameValueCollection source)
        {
            Validator.ThrowIfNull(source, nameof(source));
            Dictionary<string, string[]> result = new Dictionary<string, string[]>();
            foreach (string item in source)
            {
                result.Add(item, source[item].Split(','));
            }
            return result;
        }
    }
}